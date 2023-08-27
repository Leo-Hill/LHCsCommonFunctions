using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions {
    /// <summary>
    /// This class provides functions for network operations
    /// </summary>
    public static class Networking {
        /// <summary>
        /// This function checks if a TCP client is connected
        /// </summary>
        /// <param name="tcpClient">The TcpClient client to check</param>
        /// <returns>True if the TcpClient is connected, false if not</returns>
        public static bool TcpClientIsConnected(TcpClient tcpClient) {
            try {
                if (tcpClient != null && tcpClient.Client != null && tcpClient.Client.Connected) {
                    /* When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                    * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                    * -or- true if data is available for reading; 
                    * -or- true if the connection has been closed, reset, or terminated; 
                    * otherwise, returns false
                    */
                    // Detect if client disconnected
                    if (tcpClient.Client.Poll(0, SelectMode.SelectRead)) {
                        byte[] buff = new byte[1];
                        if (tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0) {
                            // Client disconnected
                            return false;
                        } else {
                            return true;
                        }
                    }

                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }

    }
}
