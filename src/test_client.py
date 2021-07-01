"""
A test script to see if the TCP + UDP connections to the server are working properly
the TCP client will mainly be used to spawn the sub + control it
UDP clients only listen to the server to get the telemetry data
"""

import tcp_client
import udp_client


class SimpleClient():
    def __init__(self, host, tcp_port, udp_ports):

        self.host = host
        self.tcp_port = tcp_port
        self.udp_ports = udp_ports

        self.tcp_client = tcp_client.SimpleTcpClient((self.host, self.tcp_port))
        self.udp_clients = [udp_client.SimpleUdpClient((self.host, port)) for port in self.udp_ports]

        self.drive_loop()

    def drive_loop(self):
        do_drive = True
        while do_drive:
            self.tcp_client.update()
            if self.tcp_client.aborted:
                print("TcpClient socket problem, stopping driving.")
                do_drive = False


if __name__ == "__main__":

    client = SimpleClient("127.0.0.1", 9093, [9093])  # donkey-sim.roboticist.dev
