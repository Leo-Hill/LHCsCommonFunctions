using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions.Source
{

    /***********************************************************************************************
   * 
   * This class provides functions for network operations
   * 
   **********************************************************************************************/
    public static class LHNetworkFunctions
    {
        //This function checks if a tcp client is still connected
        public static bool TcpClientIsConnected(TcpClient qTcpClient)
        {
            try
            {
                if (qTcpClient != null && qTcpClient.Client != null && qTcpClient.Client.Connected)
                {
                    /* When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                    * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                    * -or- true if data is available for reading; 
                    * -or- true if the connection has been closed, reset, or terminated; 
                    * otherwise, returns false
                    */

                    // Detect if client disconnected
                    if (qTcpClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (qTcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            // Client disconnected
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
