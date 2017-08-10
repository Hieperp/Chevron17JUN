using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using TotalBase;

namespace TotalSmartCoding.Libraries.Communications
{
    public class IONetSocket
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;

        private readonly IPAddress ipAddress;
        private readonly int portNumber;
        private readonly int readTimeout;

        private readonly bool isLaser;

        public IONetSocket(IPAddress ipAddress, int portNumber, bool isLaser)
            : this(ipAddress, portNumber, -1, isLaser)
        { }

        public IONetSocket(IPAddress ipAddress, int portNumber, int readTimeout)
            : this(ipAddress, portNumber, readTimeout, false)
        { }

        public IONetSocket(IPAddress ipAddress, int portNumber, int readTimeout, bool isLaser)
        {
            this.ipAddress = ipAddress;
            this.portNumber = portNumber;

            this.readTimeout = readTimeout;

            this.isLaser = isLaser;
        }

        public bool Connect()
        {
            this.tcpClient = new TcpClient();

            if (!this.tcpClient.Connected)
            {
                this.tcpClient.Connect(this.ipAddress, this.portNumber);
                this.networkStream = this.tcpClient.GetStream();
            }
            return true;
        }


        public bool Disconnect()
        {
            //if (this.tcpClient.Connected) --- Theoryly, it should CHECK this.tcpClient.Connected BEFORE close. BUT: DON'T KNOW why GlobalVariables.PrinterName.CartonInkjet DISCONECTED ALREADY!!!! Should check this cerefully later!
            //{
            if (this.networkStream != null) { this.networkStream.Close(); this.networkStream.Dispose(); }
            if (this.tcpClient != null) this.tcpClient.Close();
            //}
            return true;
        }


        public void WritetoStream(string stringToWrite)
        {
            try
            {
                if (this.isLaser) stringToWrite = stringToWrite + GlobalVariables.charCR + GlobalVariables.charLF;

                stringToWrite = stringToWrite.Replace("/", "");
                stringToWrite = stringToWrite.Replace(" SLASHSYMBOL ", "/");

                if (this.networkStream.CanWrite)
                {
                    Byte[] arrayBytesWriteTo = Encoding.ASCII.GetBytes(stringToWrite);
                    if (this.networkStream != null) this.networkStream.Write(arrayBytesWriteTo, 0, arrayBytesWriteTo.Length);
                }
                else
                {
                    throw new System.InvalidOperationException("NMVN: Network stream cannot be written");
                }
            }
            catch (Exception exception)
            { throw exception; }
        }

        public string ReadoutStream()
        {
            try
            {
                if (this.networkStream.CanRead)
                {
                    if (this.readTimeout != -1) this.networkStream.ReadTimeout = this.readTimeout;

                    byte[] arrayBytesReadFrom = new byte[this.tcpClient.ReceiveBufferSize]; // Reads NetworkStream into a byte buffer. 
                    this.networkStream.Read(arrayBytesReadFrom, 0, (int)this.tcpClient.ReceiveBufferSize); // This method blocks until at least one byte is read (Read can return anything from 0 to numBytesToRead).

                    if (this.readTimeout != -1) this.networkStream.ReadTimeout = -1;//Default = -1


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
                    if (this.readTimeout != -1) this.networkStream.ReadTimeout = -1; //Default = -1
                    throw new System.InvalidOperationException("NMVN: Network stream cannot be read");
                }
            }
            catch (Exception exception)
            {
                if (exception.Message == "Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.")
                    return "";
                else
                    throw exception;
            }
        }

    }
}
