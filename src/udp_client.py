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
    regex_french_notation = r':(?P<num>[0-9,E-]+),'
    regex_end = r':(?P<num>[0-9,E-]+)}'

    for regex in [regex_french_notation, regex_end]:
        matches = re.finditer(regex, string, re.MULTILINE)

        for match in matches:
            num = match.group('num').replace(',', '.')
            string = string.replace(match.group('num'), num)
    return string


class SDUdpClient:
    def __init__(self, host, port):
        self.host = host
        self.port = port

        self.do_process_msgs = True

        self.socket = None
        self.th = None

        self.connect()

    def connect(self):
        self.socket = socket.socket(
            socket.AF_INET, socket.SOCK_DGRAM)

        self.socket.bind((self.host, self.port))
        # self.socket.connect((self.host, self.port))

        self.th = Thread(target=self.proc_msg)
        self.th.start()

    def proc_msg(self):
        # self.socket.setblocking(1)
        localbuffer = ""

        while self.do_process_msgs:

            bytesAddressPair = self.socket.recvfrom(1024*1024)
            data = bytesAddressPair[0]

            # we don't technically need to convert from bytes to string
            # for json.loads, but we do need a string in order to do
            # the split by \n newline char. This seperates each json msg.
            data = data.decode("utf-8")

            # print(data)

            localbuffer += data

            try:
                n0 = localbuffer.find("{")
                n1 = localbuffer.rfind("}\n")
                if n1 >= 0 and 0 <= n0 < n1:  # there is at least one message :
                    msgs = localbuffer[n0: n1 + 1].split("\n")
                    localbuffer = localbuffer[n1:]

                    for m in msgs:
                        if len(m) <= 2:
                            continue

                        m = replace_float_notation(m)
                        j = json.loads(m)
                        self.on_msg_recv(j)

            except Exception:
                # empty the buffer (most likely the cause of the miss-reading)
                localbuffer = ""
                print("Got Exception")
                continue

    def on_msg_recv(self, j):
        logger.debug("got:" + j["msg_type"])


class SimpleUdpClient(SDUdpClient):

    def __init__(self, address, verbose=True):
        super().__init__(*address)
        self.last_images = [None, None]
        self.verbose = verbose
        self.name = str(random.random())

    def on_msg_recv(self, json_packet):
        if json_packet['msg_type'] == "telemetry":
            image = Image.open(
                BytesIO(base64.b64decode(json_packet["CameraSensor"])))
            self.last_images[1] = cv2.cvtColor(
                np.asarray(image), cv2.COLOR_RGB2BGR)

            image = Image.open(
                BytesIO(base64.b64decode(json_packet["CameraSensor_1"])))
            self.last_images[0] = cv2.cvtColor(
                np.asarray(image), cv2.COLOR_RGB2BGR)

            # display the image
            cv2.imshow(f"img_{self.port}_0", self.last_images[0])
            cv2.imshow(f"img_{self.port}_1", self.last_images[1])
            cv2.waitKey(1)

            # don't have to, but to clean up the print, delete the image string.
            del json_packet["CameraSensor"]
            del json_packet["CameraSensor_1"]

            if self.verbose:
                print(json_packet)


if __name__ == "__main__":
    # test just the UDP

    udp_client = SimpleUdpClient(("127.0.0.1", 9094))
