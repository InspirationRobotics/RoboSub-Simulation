import base64
import json
import logging
import random
import re
import select
import socket
import time
from io import BytesIO
from threading import Thread

import cv2
import numpy as np
from PIL import Image

logger = logging.getLogger(__name__)


def replace_float_notation(string):
    """
    Replace unity float notation for languages like
    French or German that use comma instead of dot.
    This convert the json sent by Unity to a valid one.
    Ex: "test": 1,2, "key": 2 -> "test": 1.2, "key": 2

    :param string: (str) The incorrect json string
    :return: (str) Valid JSON string
    """
    regex_french_notation = r'"[a-zA-Z_]+":(?P<num>[0-9,E-]+),'
    regex_end = r'"[a-zA-Z_]+":(?P<num>[0-9,E-]+)}'

    for regex in [regex_french_notation, regex_end]:
        matches = re.finditer(regex, string, re.MULTILINE)

        for match in matches:
            num = match.group('num').replace(',', '.')
            string = string.replace(match.group('num'), num)
    return string


class SDTcpClient:
    def __init__(self, host, port, poll_socket_sleep_time=0.05):
        self.msg = None
        self.host = host
        self.port = port
        self.poll_socket_sleep_sec = poll_socket_sleep_time
        self.th = None

        # the aborted flag will be set when we have detected a problem with the socket
        # that we can't recover from.
        self.aborted = False
        self.connect()

    def connect(self):
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        # connecting to the server
        logger.info("connecting to %s:%d " % (self.host, self.port))
        try:
            self.s.connect((self.host, self.port))
        except ConnectionRefusedError:
            raise (
                Exception(
                    "Could not connect to server. Is it running? "
                    "If you specified 'remote', then you must start it manually."
                )
            )

        # time.sleep(pause_on_create)
        self.do_process_msgs = True
        self.th = Thread(target=self.proc_msg, args=(self.s,), daemon=True)
        self.th.start()

    def send(self, m):
        self.msg = m

    def send_now(self, msg):
        logger.debug("send_now:" + msg)
        self.s.sendall(msg.encode("utf-8"))

    def on_msg_recv(self, j):
        logger.debug("got:" + j["msg_type"])

    def stop(self):
        # signal proc_msg loop to stop, then wait for thread to finish
        # close socket
        self.do_process_msgs = False
        if self.th is not None:
            self.th.join()
        if self.s is not None:
            self.s.close()

    def proc_msg(self, sock):  # noqa: C901
        """
        This is the thread message loop to process messages.
        We will send any message that is queued via the self.msg variable
        when our socket is in a writable state.
        And we will read any messages when it's in a readable state and then
        call self.on_msg_recv with the json object message.
        """
        sock.setblocking(0)
        inputs = [sock]
        outputs = [sock]
        localbuffer = ""

        while self.do_process_msgs:
            # without this sleep, I was getting very consistent socket errors
            # on Windows. Perhaps we don't need this sleep on other platforms.
            time.sleep(self.poll_socket_sleep_sec)
            try:
                # test our socket for readable, writable states.
                readable, writable, exceptional = select.select(
                    inputs, outputs, inputs)

                for s in readable:
                    try:
                        data = s.recv(1024 * 1024)
                    except ConnectionAbortedError:
                        logger.warn("socket connection aborted")
                        print("socket connection aborted")
                        self.do_process_msgs = False
                        break

                    # we don't technically need to convert from bytes to string
                    # for json.loads, but we do need a string in order to do
                    # the split by \n newline char. This seperates each json msg.
                    data = data.decode("utf-8")

                    localbuffer += data

                    n0 = localbuffer.find("{")
                    n1 = localbuffer.rfind("}\n")
                    if n1 >= 0 and 0 <= n0 < n1:  # there is at least one message :
                        msgs = localbuffer[n0: n1 + 1].split("\n")
                        localbuffer = localbuffer[n1:]

                        for m in msgs:
                            if len(m) <= 2:
                                continue
                            # Replace comma with dots for floats
                            # useful when using unity in a language different from English
                            m = replace_float_notation(m)
                            try:
                                j = json.loads(m)
                            except Exception as e:
                                logger.error("Exception:" + str(e))
                                logger.error("json: " + m)
                                continue

                            if "msg_type" not in j:
                                logger.error("Warning expected msg_type field")
                                logger.error("json: " + m)
                                continue
                            else:
                                self.on_msg_recv(j)

                for s in writable:
                    if self.msg is not None:
                        logger.debug("sending " + self.msg)
                        s.sendall(self.msg.encode("utf-8"))
                        self.msg = None

                if len(exceptional) > 0:
                    logger.error("problems w sockets!")

            except Exception as e:
                print("Exception:", e)
                self.aborted = True
                self.on_msg_recv({"msg_type": "aborted"})
                break

###########################################


class SimpleTcpClient(SDTcpClient):

    def __init__(self, address, poll_socket_sleep_time=0.01, verbose=True):
        super().__init__(*address, poll_socket_sleep_time=poll_socket_sleep_time)
        self.sub_loaded = False
        self.verbose = verbose

    def on_msg_recv(self, json_packet):
        if json_packet['msg_type'] == "sub_loaded":
            self.sub_loaded = True

        if self.verbose:
            print("got:", json_packet)

    def send_controls(self, up_force=0, lateral_force=0, forward_force=0, roll_force=0, pitch_force=0, yaw_force=0):
        msg = {
            "msg_type": "control",
            "up_force": str(up_force),
            "lateral_force": str(lateral_force),
            "forward_force": str(forward_force),
            "roll_force": str(roll_force),
            "pitch_force": str(pitch_force),
            "yaw_force": str(yaw_force)
        }
        self.send(json.dumps(msg))

        # this sleep lets the SDTcpClient thread poll our message and send it out.
        time.sleep(self.poll_socket_sleep_sec)

    def update(self):
        # do what you want here
        self.send_controls()


if __name__ == "__main__":
    # test just the TCP

    tcp_client = SimpleTcpClient(("127.0.0.1", 9093))
