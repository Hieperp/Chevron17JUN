using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TotalBase;
using TotalSmartCoding.Controllers.Productions;

namespace TotalSmartCoding.Libraries.Communications
{
    public class IOSerialPort : CodingController
    {
        private SerialPort serialPort;
        private System.Timers.Timer timerReadTimeOut;

        private bool waitToRead = false;
        private byte[] readResultBuffer = new byte[0];

        private Stopwatch stopwatchPin1;

        string lastNACKCode = "";

        public int StartValue { get; set; }

        public string RS232Name { get; private set; }

        public IOSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, bool dtrEnable, string rs232Name)
        {
            try
            {

                this.serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

                this.serialPort.DtrEnable = dtrEnable;
                this.serialPort.RtsEnable = true; //Use this pin to alarm

                this.RS232Name = rs232Name;

                this.serialPort.ReadTimeout = 700;

                this.serialPort.NewLine = GlobalVariables.charETX.ToString();

                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                this.timerReadTimeOut = new System.Timers.Timer(700);
                this.timerReadTimeOut.Elapsed += new System.Timers.ElapsedEventHandler(timerReadTimeOut_Elapsed);

                this.stopwatchPin1 = new Stopwatch();
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        #region Event Handler

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(30);

                int bytesToRead = serialPort.BytesToRead;

                if (bytesToRead > 0)
                {
                    lock (this.readResultBuffer)
                    {
                        this.readResultBuffer = new byte[bytesToRead];
                        serialPort.Read(this.readResultBuffer, 0, bytesToRead);
                    }
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        void timerReadTimeOut_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.waitToRead = false;
        }

        #endregion Event Handler

        public bool Connect()
        {
            try
            {
                //return true;

                if (this.serialPort.PortName != "COM0")
                {
                    if (this.serialPort.IsOpen) this.serialPort.Close();

                    this.serialPort.Open();

                    if (!this.serialPort.IsOpen) throw new System.InvalidOperationException("NMVN: Can not connect to COMPORT: " + this.serialPort.PortName);

                    Thread.Sleep(88);//NOTES: 06-SEP-2016: USE THIS PIN FOR 'Cách Ly điện OPTO BOARD' AT HOASEN GROUP
                    //this.serialPort.RtsEnable = true;//NEU SU DUNG DTR OK ROI, THI REMOVE CAU LENH NAY, DA GIANH RTS CHO MUC DICH ALARM
                    //this.serialPort.DtrEnable = true;
                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (this.serialPort.IsOpen)
                {
                    this.serialPort.DtrEnable = false;
                    this.serialPort.Close();
                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        public void WritetoSerial(string stringToWrite)
        {
            this.WritetoSerial(stringToWrite, false);
        }

        public void WritetoSerial(string stringToWrite, bool isHEXCommand)
        {
            try
            {
                //stringToWrite = stringToWrite.Replace(" ", "");

                stringToWrite = stringToWrite.Replace("/", "");
                //stringToWrite = stringToWrite.Replace(",", ""); //ZEBRA: DON'T REMOVE ','
                //stringToWrite = stringToWrite.Replace("-", "");
                stringToWrite = stringToWrite.Replace("##", "/");//TO SEND '/' TO PRINTER
                stringToWrite = stringToWrite.Replace("#~$", "/");//TO SEND '/' TO PRINTER


                if (this.serialPort.IsOpen)
                {
                    Byte[] arrayBytesWriteTo;

                    if (isHEXCommand)
                        arrayBytesWriteTo = IOCommon.HexStringToByteArray(stringToWrite);
                    else
                        arrayBytesWriteTo = Encoding.ASCII.GetBytes(stringToWrite);


                    this.serialPort.ReadExisting();//Clear the port before send
                    this.readResultBuffer = new byte[0];//Clear buffer before send
                    this.serialPort.Write(arrayBytesWriteTo, 0, arrayBytesWriteTo.Length);

                }
                else
                {
                    throw new System.InvalidOperationException("NMVN: Network stream cannot be written");
                }
            }
            catch (Exception exception)
            { throw exception; }



            //try //Nguyen thuy code send to serial cua SONG THAN
            //{
            //    stringHex = stringHex.Replace(" ", "");
            //    stringHex = stringHex.Replace("/", "");
            //    stringHex = stringHex.Replace(",", "");
            //    stringHex = stringHex.Replace("-", "");

            //    byte[] buff = GlobalStaticFunction.HexStringToByteArray(stringHex);

            //    if (this.serialPort.IsOpen)
            //    {
            //        this.serialPort.ReadExisting();//Clear the port before send
            //        this.readResultBuffer = new byte[0];//Clear buffer before send
            //        this.serialPort.Write(buff, 0, buff.Length);
            //    }
            //}
            //catch (Exception exception)
            //{
            //    this.MainStatus = exception.Message;
            //}
        }


        public bool ReadoutSerial(bool responseACK)
        {
            byte[] resultBuffer = new byte[0];
            return this.ReadoutSerial(responseACK, out resultBuffer);
        }

        public bool ReadoutSerial(bool responseACK, out byte[] returnResultBuffer)
        {
            return this.ReadoutSerial(responseACK, out returnResultBuffer, "", 0);
        }

        public bool ReadoutSerial(bool responseACK, ref string stringReadFrom)
        {
            return this.ReadoutSerial(responseACK, ref stringReadFrom, "", 0);
        }

        public bool ReadoutSerial(bool responseACK, ref string returnResultString, string commandCode, long commandLength)
        {
            bool returnValue; byte[] resultBuffer = new byte[0];
            returnValue = this.ReadoutSerial(responseACK, out resultBuffer, commandCode, commandLength);

            returnResultString = Encoding.ASCII.GetString(resultBuffer);
            returnResultString = returnResultString.Replace("\0", "");

            return returnValue;
        }

        public bool ReadoutSerial(bool responseACK, out byte[] returnResultBuffer, string commandCode, long commandLength)
        {
            try
            {
                returnResultBuffer = new byte[0];

                this.waitToRead = true;
                this.timerReadTimeOut.Enabled = true;


                while (this.waitToRead)
                {
                    lock (this.readResultBuffer)
                    {
                        if (this.readResultBuffer.Length > 0)
                        {
                            returnResultBuffer = this.readResultBuffer.Clone() as byte[];
                            break;
                        }
                    }
                }

                this.timerReadTimeOut.Enabled = false;


                if (returnResultBuffer.Length > 0)
                {
                    string stringReadFrom = Encoding.ASCII.GetString(returnResultBuffer);

                    stringReadFrom = stringReadFrom.Replace("\0", "");


                    if (responseACK)
                    {
                        if (stringReadFrom.ElementAt(0) == GlobalVariables.charACK)
                            return true;
                        else
                        {
                            if (stringReadFrom.ElementAt(0) == GlobalVariables.charNACK && stringReadFrom.Length >= 4) lastNACKCode = stringReadFrom.Substring(1, 3); //[0]: NACK + [1][2][3]: 3 Digit --- Error Code
                            return false;
                        }
                    }
                    else if (commandLength == 0 || stringReadFrom.Length >= commandLength)
                    {   //stringReadFrom(0): ESC;----stringReadFrom(1): COMMAND;----stringReadFrom(2->N): PARAMETER;----stringReadFrom(stringReadFrom.Length): EOT
                        if (stringReadFrom.ElementAt(0) == GlobalVariables.charESC && stringReadFrom.ElementAt(1) == commandCode.ElementAt(0) && stringReadFrom.ElementAt(stringReadFrom.Length - 1) == GlobalVariables.charEOT) return true; else return false;
                    }
                    else return false;
                }
                else
                    return false;

            }
            catch
            {
                returnResultBuffer = new byte[0];
                return false;
            }
        }


















        public bool ReadFromSerial(bool responseACK, out byte[] returnResultBuffer, string commandCode, long commandLength)
        {
            try
            {
                returnResultBuffer = new byte[0];

                this.waitToRead = true;
                this.timerReadTimeOut.Enabled = true;





                ////string  s = (DateTime.Now.Ticks).ToString("###########00000000000");
                ////if (this.serialPort.PortName == "COM3")
                ////    s = s.Substring(s.Length - 5, 5) + "00000";
                ////else
                ////    s = s.Substring(s.Length - 13, 13) + "00000000000";

                ////returnResultBuffer = Encoding.ASCII.GetBytes(s  );
                //////returnResultBuffer = new byte[] { 56, 56, (byte)(50 + (DateTime.Now.Second % 8)), (byte)(50 + (DateTime.Now.Second % 9)), 56, 56, (byte)(50 + (DateTime.Now.Second % 8)), 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56, 56 };
                ////return true; //TEST ONLY




                while (this.waitToRead)
                {
                    lock (this.readResultBuffer)
                    {
                        if (this.readResultBuffer.Length > 0)
                        {
                            returnResultBuffer = this.readResultBuffer.Clone() as byte[];
                            break;
                        }
                    }
                }

                this.timerReadTimeOut.Enabled = false;


                if (returnResultBuffer.Length > 0)
                {



                    string stringReadFrom = Encoding.ASCII.GetString(returnResultBuffer);

                    stringReadFrom = stringReadFrom.Replace("\0", "");


                    if (responseACK)
                    {
                        if (stringReadFrom.ElementAt(0) == GlobalVariables.charACK)
                            return true;
                        else
                        {
                            if (stringReadFrom.ElementAt(0) == GlobalVariables.charNACK && stringReadFrom.Length >= 4) lastNACKCode = stringReadFrom.Substring(1, 3); //[0]: NACK + [1][2][3]: 3 Digit --- Error Code
                            return false;
                        }
                    }
                    else
                        if (commandCode == "Autonis")
                            return true;
                        else
                        {
                            if (commandLength == 0 || stringReadFrom.Length >= commandLength)
                            {   //stringReadFrom(0): ESC;----stringReadFrom(1): COMMAND;----stringReadFrom(2->N): PARAMETER;----stringReadFrom(stringReadFrom.Length): EOT
                                if (stringReadFrom.ElementAt(0) == GlobalVariables.charESC && stringReadFrom.ElementAt(1) == commandCode.ElementAt(0) && stringReadFrom.ElementAt(stringReadFrom.Length - 1) == GlobalVariables.charEOT) return true; else return false;
                            }
                            else return false;
                        }

                }
                else
                    return false;


            }
            catch
            {
                returnResultBuffer = new byte[0];
                return false;
            }
        }

        public bool ReadFromSerial(bool responseACK, out byte[] returnResultBuffer)
        {
            return this.ReadFromSerial(responseACK, out returnResultBuffer, "", 0);
        }


        public bool ReadFromSerial(bool responseACK)
        {
            byte[] resultBuffer = new byte[0];
            return this.ReadFromSerial(responseACK, out resultBuffer);
        }


        public bool ReadFromStream(ref string stringReadFrom, bool waitForACK, string commandCode, long commandLength)
        {
            byte[] resultBuffer = new byte[0]; bool returnValue;
            returnValue = this.ReadFromSerial(waitForACK, out resultBuffer, commandCode, commandLength);

            stringReadFrom = Encoding.ASCII.GetString(resultBuffer);

            stringReadFrom = stringReadFrom.Replace("\0", "");


            return returnValue;
        }

        public bool ReadFromStream(ref string stringReadFrom, bool waitForACK)
        {
            return this.ReadFromStream(ref stringReadFrom, waitForACK, "", 0);
        }



        public void SetPinRTS(bool enabledOrDisabled)
        {
            try
            {
                return; //NOTES: 06-SEP-2016: USE THIS PIN FOR 'Cách Ly điện OPTO BOARD' AT HOASEN GROUP

                if (this.serialPort.RtsEnable != enabledOrDisabled)
                    this.serialPort.RtsEnable = enabledOrDisabled;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

    }
}
