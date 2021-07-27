"""
A basic script to control the sub using the keyboardS
"""

import tcp_client
import udp_client
import keyboard
import time


class SimpleClient():
    def __init__(self, host, tcp_port, udp_ports):

        self.host = host
        self.tcp_port = tcp_port
        self.udp_ports = udp_ports

        self.tcp_client = tcp_client.SimpleTcpClient(
            (self.host, self.tcp_port))

        time.sleep(1)
        self.udp_clients = [udp_client.SimpleUdpClient(
            (self.host, port)) for port in self.udp_ports]

        self.drive_loop()

    def is_key_pressed(self, key):
        if keyboard.is_pressed(key):
            return True
        else:
            return False

    def manual_steering(self):
        kb_keys = {
            'forward': "up",
            'backward': "down",
            'left': "left",
            'right': "right",
            'roll_right': "e",
            'roll_left': "a",
            'up': 'r',
            'down': 'f',
            'pitch_up': 's',
            'pitch_down': 'w'
        }

        up = self.is_key_pressed(kb_keys['up'])
        down = self.is_key_pressed(kb_keys['down'])

        forward = self.is_key_pressed(kb_keys['forward'])
        backward = self.is_key_pressed(kb_keys['backward'])

        left = self.is_key_pressed(kb_keys['left'])
        right = self.is_key_pressed(kb_keys['right'])

        roll_left = self.is_key_pressed(kb_keys['roll_left'])
        roll_right = self.is_key_pressed(kb_keys['roll_right'])

        pitch_up = self.is_key_pressed(kb_keys['pitch_up'])
        pitch_down = self.is_key_pressed(kb_keys['pitch_down'])

        self.tcp_client.send_controls(
            up_force=up-down,
            forward_force=forward-backward,
            roll_force=roll_left-roll_right,
            yaw_force=right-left,
            pitch_force=pitch_up-pitch_down)

    def drive_loop(self):
        do_drive = True
        while do_drive:
            self.manual_steering()
            if self.tcp_client.aborted:
                print("TcpClient socket problem, stopping driving.")
                do_drive = False


if __name__ == "__main__":

    client = SimpleClient("127.0.0.1", 9093, [9094, 9095])
