using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TotalSmartCoding.CommonLibraries
{
    public class GlobalNetSockets
    {
        #region Common Stream action

        public static void WritetoStream(NetworkStream networkStream, string stringToWrite)
        {
            try
            {
                stringToWrite = stringToWrite.Replace("/", "");
                stringToWrite = stringToWrite.Replace(" SLASHSYMBOL ", "/");


                if (networkStream.CanWrite)
                {
                    Byte[] arrayBytesWriteTo = Encoding.ASCII.GetBytes(stringToWrite);
                    if (networkStream != null) networkStream.Write(arrayBytesWriteTo, 0, arrayBytesWriteTo.Length);
                }
                else
                {
                    throw new System.InvalidOperationException("NMVN: Network stream cannot be written");
                }
            }
            catch (Exception exception)
            { throw exception; }
        }

        public static string ReadoutStream(TcpClient tcpClient, NetworkStream networkStream)
        {
            try
            {
                if (networkStream.CanRead)
                {
                    byte[] arrayBytesReadFrom = new byte[tcpClient.ReceiveBufferSize]; // Reads NetworkStream into a byte buffer. 
                    networkStream.Read(arrayBytesReadFrom, 0, (int)tcpClient.ReceiveBufferSize); // This method blocks until at least one byte is read (Read can return anything from 0 to numBytesToRead).

                    string stringReadFrom = Encoding.ASCII.GetString(arrayBytesReadFrom);

                    stringReadFrom = stringReadFrom.Replace("\0", "");
                    stringReadFrom = stringReadFrom.Replace("\r", ""); //New: 22.JAN.2014
                    stringReadFrom = stringReadFrom.Replace("\n", ""); //New: 22.JAN.2014
                    //stringReadFrom = stringReadFrom.Replace(GlobalVariables.charESC.ToString(), "");
                    //stringReadFrom = stringReadFrom.Replace(GlobalVariables.charEOT.ToString(), "");

                    return stringReadFrom;
                }
                else
                {
                    throw new System.InvalidOperationException("NMVN: Network stream cannot be read");
                }
            }
            catch (Exception exception)
            { throw exception; }
        }

        #endregion Common Stream action

    }
}
