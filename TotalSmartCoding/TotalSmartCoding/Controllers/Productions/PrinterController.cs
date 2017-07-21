using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using TotalBase;
using TotalDTO.Productions;
using TotalSmartCoding.CommonLibraries;

namespace TotalSmartCoding.Controllers.Productions
{
    public class PrinterController : CodingController
    {
        #region Storage

        private FillingData privateFillingData;
        private readonly GlobalVariables.PrinterName printerName;
        private readonly bool isLaser;

        private readonly IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        private readonly int portNumber = 7000;

        private TcpClient tcpClient;
        private NetworkStream networkStream;


        private string lastNACKCode;

        private bool onPrinting;
        private bool resetMessage;

        #endregion Storage


        #region Contructor

        public PrinterController(GlobalVariables.PrinterName printerName, FillingData fillingData, bool isLaser)
        {

            try
            {
                this.FillingData = fillingData;
                this.privateFillingData = this.FillingData.ShallowClone();

                this.printerName = printerName;
                this.isLaser = isLaser;

                this.ipAddress = IPAddress.Parse(GlobalVariables.IpAddress(this.printerName));
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        #endregion Contructor


        #region Public Properties
        
        public bool OnPrinting
        {
            get { return this.onPrinting; }
            private set { this.onPrinting = value; this.resetMessage = true; }
        }


        public void StartPrint() { this.OnPrinting = true; }
        public void StopPrint() { this.OnPrinting = false; }



        public string MonthSerialNumber { get { return this.privateFillingData.LastPackNo; } }

        public string LastCartonNo { get { return this.privateFillingData.LastCartonNo; } }

        public string LastPalletNo { get { return this.privateFillingData.LastPalletNo; } }



        #endregion Public Properties


        #region Message Configuration

        private string FirstMessageLine(bool isReadableText) //Only DominoPrinterName.CartonInkJet: HAS SERIAL NUMBER (BUT WILL BE UPDATE MANUAL FOR EACH CARTON - BECAUSE: [EAN BARCODE] DOES NOT ALLOW INSERT SERIAL NUMBER) ===> FOR THIS: LastPackNo FOR EVERY PACK: NEVER USE
        {
            return this.privateFillingData.BatchCode + (this.printerName == GlobalVariables.PrinterName.CartonInkjet ? "/" + "  " + "/" + this.privateFillingData.LastCartonNo.Substring(2) : "");
        }

        private string SecondMessageLine(bool isReadableText)
        {
            return (isReadableText ? this.privateFillingData.NoExpiryDate.ToString("00") : "") + (!isReadableText ? this.privateFillingData.CommodityCode + " " : "NSX") + DateTime.Now.ToString("dd/MM/yy");
            //return "NSX " + GlobalVariables.charESC + "/n/1/A/" + GlobalVariables.charESC + "/n/1/F/" + GlobalVariables.charESC + "/n/1/D/";
        }

        private string ThirdMessageLine(int serialNumberIndentity, bool isReadableText) //serialNumberIndentity = 1 when print as text on first line, 2 when insert into 2D Barcode
        {
            string serialNumberFormat = ""; //Numeric Serial Only, No Alpha Serial, Zero Leading, 6 Digit: 000001 -> 999999, Step 1, Start this.privateFillingLineData.MonthSerialNumber, Repeat: 0
            if (this.printerName == GlobalVariables.PrinterName.DegitInkjet || this.printerName == GlobalVariables.PrinterName.BarcodeInkjet)
                //////---- Don use this Startup Serial Value, because some Dimino printer do no work!!! - DON't KNOW!!! serialNumberFormat = GlobalVariables.charESC + "/j/" + serialNumberIndentity.ToString() + "/N/06/000001/999999/000001/Y/N/0/" + this.privateFillingLineData.MonthSerialNumber + "/00000/N/"; //WITH START VALUE---No need to update serial number
                serialNumberFormat = GlobalVariables.charESC + "/j/" + serialNumberIndentity.ToString() + "/N/06/000001/999999/000001/Y/N/0/000000/00000/N/"; //WITH START VALUE = 1 ---> NEED TO UPDATE serial number
            else //this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet
                serialNumberFormat = this.privateFillingData.LastPalletNo.Substring(1); //---Dont use counter (This will be updated MANUALLY for each carton)


            //return this.privateFillingLineData.CommodityCode + serialNumberFormat;
            return (this.printerName != GlobalVariables.PrinterName.BarcodeInkjet || isReadableText ? this.privateFillingData.CommodityCode : "")  + "/" + this.privateFillingData.FillingLineCode + (isReadableText ? " " : "") + "/" + serialNumberFormat;
        }
        
        private string EANInitialize(string twelveDigitCode)
        {

            int iSum = 0; int iDigit = 0;

            twelveDigitCode = twelveDigitCode.Replace("/", "");

            // Calculate the checksum digit here.
            for (int i = twelveDigitCode.Length; i >= 1; i--)
            {
                iDigit = Convert.ToInt32(twelveDigitCode.Substring(i - 1, 1));
                if (i % 2 == 0)
                {	// odd
                    iSum += iDigit * 3;
                }
                else
                {	// even
                    iSum += iDigit * 1;
                }
            }

            int iCheckSum = (10 - (iSum % 10)) % 10;
            return twelveDigitCode + iCheckSum.ToString();

            #region Checksum rule
            ////Checksum rule
            ////Lấy tổng tất cả các số ở vị trí lẻ (1,3,5,7,9,11) được một số A.
            ////Lấy tổng tất cả các số ở vị trí chẵn (2,4,6,8,10,12). Tổng này nhân với 3 được một số (B).
            ////Lấy tổng của A và B được số A+B.
            ////Lấy phần dư trong phép chia của A+B cho 10, gọi là số x. Nếu số dư này bằng 0 thì số kiểm tra bằng 0, nếu nó khác 0 thì số kiểm tra là phần bù (10-x) của số dư đó.

            //Generate check sum number
            //            --**************************************
            //-- Name: Check Digit for EAN13 Barcode
            //-- Description:Calculates the Check Digit for a EAN13 Barcode
            //-- By: Conradude
            //--
            //-- Inputs:SELECT dbo.fu_EAN13CheckDigit('600100700010')
            //--
            //-- Returns:String with 13 Digits
            //--
            //-- Side Effects:Hair Growth on the palms of your hand
            //--
            //--This code is copyrighted and has-- limited warranties.Please see http://www.Planet-Source-Code.com/vb/scripts/ShowCode.asp?txtCodeId=891&lngWId=5--for details.--**************************************

            //CREATE FUNCTION fu_EAN13CheckDigit (@Barcode varchar(12))																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																								-->>>><<><<<<<>>>>>>>>>>>>>>>>< <><><>												
            //RETURNS varchar(13)
            //AS
            //BEGIN
            //DECLARE @SUM int ,
            //    @COUNTER int,
            //    @RETURN varchar(13),
            //    @Val1 int,
            //    @Val2 int	
            //SET @COUNTER = 1
            //SET @SUM = 0
            //WHILE @Counter < 13
            //BEGIN
            //    SET @VAL1 = SUBSTRING(@Barcode,@counter,1) * 1
            //    SET @VAL2 = SUBSTRING(@Barcode,@counter + 1,1) * 3
            //    SET @SUM = @VAL1 + @SUM	
            //    SET @SUM = @VAL2 + @SUM
            //    SET @Counter = @Counter + 2
            //END
            //SET @Counter = ROUND(@SUM + 5,-1)
            //SET @Return = @BARCODE + CONVERT(varchar,((@Counter - @SUM)))
            //RETURN @Return
            //END
            #endregion


        }

        private string WholeMessageLine()
        {//THE FUNCTION LaserDigitMessage totally base on this.WholeMessageLine. Later, if there is any thing change in this.WholeMessageLine, THE FUNCTION LaserDigitMessage should be considered
            if (this.printerName == GlobalVariables.PrinterName.DegitInkjet)
                return GlobalVariables.charESC + "u/1/ " + GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.ThirdMessageLine(1, true);
            else if (this.printerName == GlobalVariables.PrinterName.BarcodeInkjet) //DATE: 18FEB2017: IN THE READABLE TEXT ONLY: SWAP BETWEEN Second Line <-> Third Line (EVERY THING IN THE BARCODE KEEP CURRENT VERSION)
            {
                //return GlobalVariables.charESC + "u/1/" + this.FirstMessageLine(true) + "/" +  //First Line
                //       GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.ThirdMessageLine(1, true) +   //Second Line
                //       GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.SecondMessageLine(true);     //Third Line   

                return GlobalVariables.charESC + "u/1/" + GlobalVariables.charESC + "/z/1/0/26/20/20/1/0/0/0/" + this.FirstMessageLine(false) + " " + this.SecondMessageLine(false) + " " + this.ThirdMessageLine(2, false) + "/" + GlobalVariables.charESC + "/z/0" + //2D Barcode
                       GlobalVariables.charESC + "u/1/" + this.FirstMessageLine(true) + "/" +  //First Line
                       GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.ThirdMessageLine(1, true) +   //Second Line
                       GlobalVariables.charESC + "/r/" + GlobalVariables.charESC + "u/1/" + this.SecondMessageLine(true);     //Third Line   
            }
            else //this.DominoPrinterNameID == GlobalVariables.DominoPrinterName.CartonInkJet
            {
                //string thirdMessageLine = EANInitialize(this.ThirdMessageLine(1));
                //return GlobalVariables.charESC + "u/2/" + GlobalVariables.charESC + "/q/4/@/" + thirdMessageLine + "/@/" + GlobalVariables.charESC + "/q/0" +   //EAN13 Barcode: the first digit MUST be inserted again at the end as the digit number 14   + thirdMessageLine.Substring(0,1)
                //           GlobalVariables.charESC + "u/1/  " + this.FirstMessageLine() + "/" + //First Line
                //           GlobalVariables.charESC + "/r/  " + GlobalVariables.charESC + "u/1/" + thirdMessageLine + "/ " + DateTime.Now.ToString("dd/MM/yy");


                return GlobalVariables.charESC + "u/2/" + GlobalVariables.charESC + "/q/6/" + this.ThirdMessageLine(1, false) + "/" + this.privateFillingData.LastCartonNo.Substring(2) + GlobalVariables.charESC + "/q/0" +
                           GlobalVariables.charESC + "u/1/  " + this.FirstMessageLine(true) + "/ " + this.privateFillingData.NoExpiryDate.ToString("00") + "/" + //First Line
                           GlobalVariables.charESC + "/r/  " + GlobalVariables.charESC + "u/1/" + this.ThirdMessageLine(1, true) + "/ " + DateTime.Now.ToString("dd/MM/yy");

            }
        }

        private string LaserDigitMessage(bool isSerialNumber)
        {//THE FUNCTION LaserDigitMessage totally base on this.WholeMessageLine. Later, if there is any thing change in this.WholeMessageLine, THE FUNCTION LaserDigitMessage should be considered
            if (isSerialNumber)
                return this.privateFillingData.LastPackNo;
            else
                return this.privateFillingData.CommodityCode  + this.privateFillingData.FillingLineCode;
        }//NOTE: NEVER CHANGE THIS FUNCTION WITHOUT HAVE A LOOK AT this.WholeMessageLine

        #endregion Message Configuration


        #region Public Method


        private bool Connect()
        {
            try
            {
                this.MainStatus = "Bắt đầu kết nối ....";

                this.tcpClient = new TcpClient();

                if (!this.tcpClient.Connected)
                {
                    this.tcpClient.Connect(this.ipAddress, this.portNumber);
                    this.networkStream = tcpClient.GetStream();
                }
                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
                return false;
            }

        }

        private bool Disconnect()
        {
            try
            {
                this.MainStatus = "Disconnect....";

                //if (this.tcpClient.Connected) --- Theoryly, it should CHECK this.tcpClient.Connected BEFORE close. BUT: DON'T KNOW why GlobalVariables.PrinterName.CartonInkjet DISCONECTED ALREADY!!!! Should check this cerefully later!
                //{
                if (this.networkStream != null) { this.networkStream.Close(); this.networkStream.Dispose(); }
                if (this.tcpClient != null) this.tcpClient.Close();
                //}

                this.LedGreenOn = false;
                this.LedAmberOn = false;
                this.LedRedOn = false;
                this.NotifyPropertyChanged("LedStatus");

                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }


        private void WriteToStream(string stringWriteTo)
        {
            try
            {
                if (this.isLaser) stringWriteTo = stringWriteTo + GlobalVariables.charCR + GlobalVariables.charLF;
                GlobalNetSockets.WritetoStream(networkStream, stringWriteTo);
            }
            catch (Exception exception)
            { throw exception; }
        }

        /// <summary>
        /// NEVER waiforACK WHEN This.IsLaser
        /// </summary>
        /// <param name="receivedFeedback"></param>
        /// <param name="waitforACK"></param>
        /// <returns></returns>
        private bool ReadoutStream(ref string receivedFeedback, bool waitforACK)
        {
            return ReadFromStream(ref receivedFeedback, waitforACK, "", 0);
        }

        /// <summary>
        /// /// NEVER waitForACK WHEN This.IsLaser
        /// </summary>
        /// <param name="receivedFeedback"></param>
        /// <param name="waitforACK"></param>
        /// <param name="commandCode"></param>
        /// <param name="commandLength"></param>
        /// <returns></returns>
        private bool ReadFromStream(ref string receivedFeedback, bool waitforACK, string commandCode, long commandLength)
        {
            try
            {
                receivedFeedback = GlobalNetSockets.ReadoutStream(tcpClient, networkStream);

                if (!this.isLaser && waitforACK)
                {
                    if (receivedFeedback.ElementAt(0) == GlobalVariables.charACK)
                        return true;
                    else
                    {
                        if (receivedFeedback.ElementAt(0) == GlobalVariables.charNACK && receivedFeedback.Length >= 4) lastNACKCode = receivedFeedback.Substring(1, 3); //[0]: NACK + [1][2][3]: 3 Digit --- Error Code
                        return false;
                    }
                }
                else if (commandLength == 0 || receivedFeedback.Length >= commandLength)
                {
                    if (this.isLaser)
                        return receivedFeedback.Contains(commandCode);
                    else//receivedFeedback(0): ESC;----receivedFeedback(1): COMMAND;----receivedFeedback(2->N): PARAMETER;----receivedFeedback(receivedFeedback.Length): EOT
                        if (receivedFeedback.ElementAt(0) == GlobalVariables.charESC && receivedFeedback.ElementAt(1) == commandCode.ElementAt(0) && receivedFeedback.ElementAt(receivedFeedback.Length - 1) == GlobalVariables.charEOT) return true; else return false;
                }
                else return false;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }



        private void storeMessage(string stringMessage)
        {
            string receivedFeedback = "";

            //S: Message Storage
            this.WriteToStream(GlobalVariables.charESC + "/S/001/" + stringMessage + "/" + GlobalVariables.charEOT);
            if (this.ReadoutStream(ref receivedFeedback, true)) Thread.Sleep(750); else throw new System.InvalidOperationException("Lỗi cài đặt bản tin 001: " + receivedFeedback);

            //P: Message To Head Assignment '//CHU Y QUAN TRONG: DUA MSG SO 1 LEN DAU IN (SAN SANG KHI IN, BOI VI KHI IN TA STORAGE MSG VAO VI TRI SO 1 MA KHONG QUAN TAM DEN VI TRI SO 2, 3,...)
            this.WriteToStream(GlobalVariables.charESC + "/P/1/001/" + GlobalVariables.charEOT); //FOR AX SERIES: MUST CALL P: Message To Head Assignment FOR EVERY CALL S: Message Storage
            if (this.ReadoutStream(ref receivedFeedback, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("Lỗi sẳn sàng in phun, bản tin 001: " + receivedFeedback);
        }


        private bool WaitforPrintingAcknowledge(ref string receivedFeedback)
        {
            try
            {
                bool returnValue = false;
                this.networkStream.ReadTimeout = 300; //Default = -1; 

                //this.MainStatus = "Wait for PrintingAcknowledge";

                receivedFeedback = GlobalNetSockets.ReadoutStream(tcpClient, networkStream);

                if (receivedFeedback == GlobalVariables.charPrintingACK.ToString())   //OK State
                {
                    returnValue = true;    // GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID]++;
                }
                else//  !=  : Need to check some other condition
                {
                    int countPrintingACK = 1;// GlobalStaticFunction.CountStringOccurrences(receivedFeedback, GlobalVariables.charPrintingACK.ToString());

                    if (countPrintingACK == 1)      //OK: but in case of receive only one PrintingACK, following by something: Ignore. Later: Maybe ADD SOME CODE to SHOW on screen what receivedFeedback is
                    {
                        this.MainStatus = "NMVN: Some extra thing received: " + receivedFeedback;
                        returnValue = true;    // GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID]++;                      
                    }
                    else if (countPrintingACK > 1)
                    {
                        this.MainStatus = "NMVN: Receive more than one printing acknowledge, a message may printed more than one times";
                        returnValue = true;    // GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID] = GlobalVariables.DominoProductCounter[(int)this.DominoPrinterNameID] + countPrintingACK;   
                    }
                    else // countPrintingACK < 1    :   Fail
                    {
                        this.MainStatus = "NMVN: Random received: " + receivedFeedback;
                        returnValue = false;
                    }
                }

                this.networkStream.ReadTimeout = -1; //Default = -1
                return returnValue;
            }

            catch (Exception exception)
            {
                this.MainStatus = "NO ACK time out";
                //Ignore when timeout
                if (exception.Message != "Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.") this.MainStatus = exception.Message;

                this.networkStream.ReadTimeout = -1; //Default = -1
                return false;
            }
        }


        #region STASTUS
        private bool lfStatusLED(ref string receivedFeedback)
        {//DISPLAY 3 LEDS STATUS
            try
            {
                if (this.isLaser)
                {//RESULT GETSTATUS <severity>: • 0=information • 1=warning • 2=temporary fault • 3=critical fault • 4=critical fault (needs to be reset by hardware) 
                    this.LedGreenOn = receivedFeedback.ElementAt(17).ToString() == "0" || receivedFeedback.ElementAt(17).ToString() == "1";
                    this.LedAmberOn = receivedFeedback.ElementAt(17).ToString() == "1" || receivedFeedback.ElementAt(17).ToString() == "2";
                    this.LedRedOn = receivedFeedback.ElementAt(17).ToString() == "3" || receivedFeedback.ElementAt(17).ToString() == "4";
                }
                else
                {
                    this.LedGreenOn = ((int)receivedFeedback.ElementAt(7) & int.Parse("1")) == 1;
                    this.LedAmberOn = ((int)receivedFeedback.ElementAt(7) & int.Parse("2")) == 2;
                    this.LedRedOn = ((int)receivedFeedback.ElementAt(7) & int.Parse("3")) == 3;
                }
                this.NotifyPropertyChanged("LedStatus");

                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }


        private string lStatusHHMM;
        private bool lfStatusHistory(ref string receivedFeedback)
        {
            try
            {
                if (lStatusHHMM != "" + receivedFeedback.ElementAt(7) + receivedFeedback.ElementAt(8) + receivedFeedback.ElementAt(9) + receivedFeedback.ElementAt(10))
                {
                    lStatusHHMM = "" + receivedFeedback.ElementAt(7) + receivedFeedback.ElementAt(8) + receivedFeedback.ElementAt(9) + receivedFeedback.ElementAt(10);
                    this.MainStatus = "" + receivedFeedback.ElementAt(3) + receivedFeedback.ElementAt(4) + receivedFeedback.ElementAt(5);//Get the status TEXT & Maybe: Add to database
                }
                return true;
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }
        }


        private bool lfStatusAlert(ref string receivedFeedback)
        {
            return true;
        }

        //Private Function lfStatusAlert(ByRef lInReceive() As Byte) As Boolean
        //    Static lAlertArray() As Byte '//lAlertArray: DE LUU LAI Alert TRUOC DO, KHI Alert THAY DOI => UPDATE TO DATABASE
        //    Dim lAlertNew As Boolean, i As Long

        //On Error GoTo ARRAY_INIT '//KIEM TRA, TRONG T/H CHU KHOI TAO CHO lAlertArray THI REDIM lAlertArray (0)
        //    If LBound(lAlertArray()) >= 0 Then GoTo ARRAY_OK
        //ARRAY_INIT:
        //    ReDim lAlertArray(0)

        //ARRAY_OK:
        //On Error GoTo ERR_HANDLER

        //    lAlertNew = UBound(lAlertArray) <> UBound(lInReceive)
        //    If Not lAlertNew Then
        //        For i = LBound(lInReceive) To UBound(lInReceive)
        //            If lInReceive(i) <> lAlertArray(i) Then lAlertNew = True: Exit For
        //        Next i
        //    End If
        //    If lAlertNew Then
        //        'DISPLAY NEW ALERT
        //        'SAVE NEW ALERT
        //        'Debug.Print "ALERT - " + Chr(lInReceive(5)) + Chr(lInReceive(6)) + Chr(lInReceive(7))
        //    End If

        //    lfStatusAlert = True
        //ERR_RESUME:
        //    Exit Function
        //ERR_HANDLER:
        //    Call psShowError: lfStatusAlert = False: GoTo ERR_RESUME
        //End Function
        
        #endregion STASTUS




        #endregion Public Method


        #region Public Thread

        public void ThreadRoutine()
        {
            this.privateFillingData = this.FillingData.ShallowClone(); //WE NEED TO CLONE FillingData, BECAUSE: IN THIS CONTROLLER: WE HAVE TO UPDATE THE NEW PRINTED BARCODE NUMBER TO FillingData, WHICH IS CREATED IN ANOTHER THREAD (FillingData IS CREATED IN VIEW: SmartCoding). SO THAT: WE CAN NOT UPDATE FillingData DIRECTLY, INSTEAD: WE REAISE EVENT ProertyChanged => THEN: WE CATCH THE EVENT IN SmartCoding VIEW AND UPDATE BACK TO THE ORIGINAL FillingData, BECAUSE: THE ORIGINAL FillingData IS CREATED AND BINDED IN THE VIEW: SmartCoding

            string receivedFeedback = ""; bool printerReady = false;
            bool readytoPrint = false; bool headEnable = false;


            this.LoopRoutine = true; this.StopPrint();


            //This command line is specific to CartonInkJet ON FillingLine.Pail (Just here only for this specific)
            if (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Drum || (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Pail && this.printerName == GlobalVariables.PrinterName.CartonInkjet)) { this.LedGreenOn = true; return; } //DO NOTHING


            try
            {

                if (!this.Connect()) throw new System.InvalidOperationException("NMVN: Can not connect to printer.");

                #region INITIALISATION PRINTER
                do  //INITIALISATION COMMAND
                {
                    if (this.isLaser)
                    {
                        this.WriteToStream("GETVERSION"); //Obtains the alphanumeric identifier of the printer
                        if (this.ReadFromStream(ref receivedFeedback, false, "RESULT GETVERSION", "RESULT GETVERSION".Length)) printerReady = true; //Printer Identity OK"
                    }
                    else
                    {
                        this.WriteToStream(GlobalVariables.charESC + "/A/?/" + GlobalVariables.charEOT);   //A: Printer Identity
                        if (this.ReadFromStream(ref receivedFeedback, false, "A", 14)) printerReady = true; //A: Printer Identity OK"
                    }


                    if (printerReady)
                    {
                        do //CHECK PRINTER READY TO PRINT
                        {
                            if (this.isLaser)
                                this.WriteToStream("GETSTATUS"); //Determines the current status of the controller
                            else
                                this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status


                            if ((this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "RESULT GETSTATUS", "RESULT GETSTATUS".Length)) || (!this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "O", 9)))
                            {
                                lfStatusLED(ref receivedFeedback);
                                readytoPrint = this.LedGreenOn || this.LedAmberOn; this.LedGreenOn = false; //After Set LED, If LedGreenOn => ReadyToPrint
                            }


                            if (!readytoPrint)
                            {
                                if (this.isLaser)
                                {
                                    this.MainStatus = "Máy in laser chưa sẳn sàng in, vui lòng kiểm tra lại.";
                                    Thread.Sleep(20000);
                                }
                                else
                                {
                                    this.WriteToStream(GlobalVariables.charESC + "/O/S/1/" + GlobalVariables.charEOT); //O/S/1: Turn on ink-jet
                                    if (this.ReadoutStream(ref receivedFeedback, true))
                                    {
                                        this.MainStatus = "Đang khởi động máy in, vui lòng chờ trong ít phút.";
                                        Thread.Sleep(50000);
                                    }
                                    else throw new System.InvalidOperationException("Lỗi không thể khởi động máy in: " + receivedFeedback);
                                }
                            }
                            else //readytoPrint: OK
                            {
                                if (this.isLaser)
                                    this.WriteToStream("GETMARKMODE"); //Determines the current state of the marking engine on the laser controller
                                else
                                    this.WriteToStream(GlobalVariables.charESC + "/Q/1/?/" + GlobalVariables.charEOT);    //Q: HEAD ENABLE: ENABLE


                                if ((this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "RESULT GETMARKMODE", "RESULT GETMARKMODE".Length)) || (!this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "Q", 5)))
                                {
                                    if ((this.isLaser && receivedFeedback.ElementAt(19).ToString() == "1") || (!this.isLaser && receivedFeedback.ElementAt(3).ToString() == "Y"))
                                        headEnable = true;
                                    else
                                    {
                                        if (this.isLaser)
                                            this.WriteToStream("MARK START");
                                        else
                                            this.WriteToStream(GlobalVariables.charESC + "/Q/1/Y/" + GlobalVariables.charEOT);


                                        if ((this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) || (!this.isLaser && this.ReadoutStream(ref receivedFeedback, true)))
                                        {
                                            this.MainStatus = this.isLaser ? "Đang bật chế độ in" : "Đang mở in phun" + ", vui lòng chờ trong ít phút.";
                                            Thread.Sleep(10000);
                                        }
                                        else throw new System.InvalidOperationException("Lỗi mở in phun: " + receivedFeedback);
                                    }
                                }
                            }

                        } while (this.LoopRoutine && (!readytoPrint || !headEnable));
                    }
                    else
                    {
                        this.MainStatus = "Không thể kết nối máy in. Đang tự động thử kết nối lại ... Nhấn Disconnect để thoát.";
                    }
                } while (this.LoopRoutine && !printerReady && !readytoPrint && !headEnable);

                #endregion INITIALISATION COMMAND


                #region GENERAL SETUP (NOT LASER ONLY)
                if (!this.isLaser)
                {
                    //C: Set Clock
                    this.WriteToStream(GlobalVariables.charESC + "/C/" + DateTime.Now.ToString("yyyy/MM/dd/00/hh/mm/ss") + "/" + GlobalVariables.charEOT);     //C: Set Clock
                    if (!this.ReadoutStream(ref receivedFeedback, true)) throw new System.InvalidOperationException("Lỗi cài đặt ngày giờ máy in phun: " + receivedFeedback);

                    //T: Reset Product Counting
                    this.WriteToStream(GlobalVariables.charESC + "/T/1/0/" + GlobalVariables.charEOT);
                    if (!this.ReadoutStream(ref receivedFeedback, true)) throw new System.InvalidOperationException("Lỗi cài đặt bộ đếm số lần in phun: " + receivedFeedback);
                }
                #endregion GENERAL SETUP


                #region Status (NOT LASER ONLY)
                //SET STATUS
                if (!this.isLaser)
                {
                    this.WriteToStream(GlobalVariables.charESC + "/0/N/0/" + GlobalVariables.charEOT);     //0: Status Report Mode: OFF: NO ERROR REPORTING
                    if (!this.ReadoutStream(ref receivedFeedback, true)) throw new System.InvalidOperationException("NMVN: Can not set status report mode: " + receivedFeedback);

                    //co gang viet cho nay cho hay hay
                    //this.WriteToStream( GlobalVariables.charESC + "/1/C/?/" + GlobalVariables.charEOT) ;   //1: REQUEST CURRENT STATUS
                    //if (!this.ReadFromStream(ref receivedFeedback, false, "1", 12) )
                    //    throw new System.InvalidOperationException("NMVN: Can not request current status: " + receivedFeedback);
                    //else
                    ////        Debug.Print "STATUS " + Chr(lInReceive(3)) + Chr(lInReceive(4)) + Chr(lInReceive(5))
                    ////'        If Not ((lInReceive(3) = Asc("0") Or lInReceive(3) = Asc("1")) And lInReceive(4) = Asc("0") And lInReceive(5) = Asc("0")) Then GoTo ERR_HANDLER   'NOT (READY OR WARNING)
                    ////    End If
                }
                #endregion Status


                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    if (!this.OnPrinting)
                    {
                        #region Reset Message: Clear message

                        if (this.resetMessage)
                        {
                            this.resetMessage = false; //Setup first message: Only one times                            
                            this.MainStatus = "Please wait .... ";

                            if (this.isLaser)
                            {
                                this.WriteToStream("MARK STOP");
                                if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(7000); else throw new System.InvalidOperationException("Can not disables printing ... : " + receivedFeedback);
                            }
                            else
                                this.storeMessage("  ");


                            if (this.isLaser)
                                this.WriteToStream("LOADPROJECT store: SLASHSYMBOL Demo");
                            else
                                this.WriteToStream(GlobalVariables.charESC + "/I/1/ /" + GlobalVariables.charEOT); //SET OF: Print Acknowledgement Flags I

                            if ((this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) || (!this.isLaser && this.ReadoutStream(ref receivedFeedback, true))) Thread.Sleep(250); else throw new System.InvalidOperationException("Can not set off printing acknowledge/ Load Demo project: " + receivedFeedback);


                            this.MainStatus = "Ready to print";
                        }
                        #endregion Reset Message: Clear message
                    }

                    else //this.OnPrinting
                    {
                        #region Reset Message

                        if (this.resetMessage)
                        {
                            #region SETUP MESSAGE
                            this.MainStatus = "Please wait ...";

                            if (this.isLaser && this.printerName == GlobalVariables.PrinterName.DegitInkjet) //stringWriteTo = " SETVARIABLES \"MonthCodeAndLine\" \"10081\"\r\n"
                            {//BEGINTRANS [ENTER] OK   SETTEXT "Text 1" "Domino AG" [ENTER]   OK   SETTEXT "Barcode 1" "Sator Laser GmbH" [ENTER]   OK EXECTRANS [ENTER] OK MSG 1 
                                //this.WriteToStream("BEGINTRANS");
                                //if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message: " + receivedFeedback);

                                this.WriteToStream("LOADPROJECT store: SLASHSYMBOL " + (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Smallpack ? "BPCODigit" : (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Pail ? "BPPailDigit" : "BPPailDigit")));
                                if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(500); else throw new System.InvalidOperationException("NMVN: Can not load message: " + receivedFeedback);

                                this.WriteToStream("SETVARIABLES \"MonthCodeAndLine\" \"" + this.LaserDigitMessage(false) + "\"");
                                if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message code: " + receivedFeedback);

                                this.WriteToStream("SETCOUNTERVALUE Serialnumber01 " + this.LaserDigitMessage(true));
                                if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message counter: " + receivedFeedback);

                                this.WriteToStream("MARK START");
                                if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(7000); else throw new System.InvalidOperationException("NMVN: Can not enables marking ... : " + receivedFeedback);

                                //this.WriteToStream("EXECTRANS");
                                //if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message: " + receivedFeedback);
                            }
                            else
                            {
                                this.storeMessage(this.WholeMessageLine()); //SHOULD Update serial number: - Note: Some DOMINO firmware version does not need to update serial number. Just set startup serial number only when insert serial number. BUT: FOR SURE, It will be updated FOR ALL

                                //    U: UPDATE SERIAL NUMBER - Counter 1
                                this.WriteToStream(GlobalVariables.charESC + "/U/001/1/" + this.privateFillingData.LastPackNo + "/" + GlobalVariables.charEOT);
                                if (this.ReadoutStream(ref receivedFeedback, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("Lỗi không thể cài đặt số thứ tự sản phẩm: " + receivedFeedback);

                                //    U: UPDATE SERIAL NUMBER - Counter 2
                                this.WriteToStream(GlobalVariables.charESC + "/U/001/2/" + this.privateFillingData.LastPackNo + "/" + GlobalVariables.charEOT);
                                if (this.ReadoutStream(ref receivedFeedback, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("Lỗi không thể cài đặt số thứ tự sản phẩm: " + receivedFeedback);
                            }
                            #endregion Reset Message


                            this.MainStatus = "Printing ....";
                            this.resetMessage = false; //Setup first message: Only one times                            
                        }

                        #endregion Reset Message: Setup FirstMessage


                        //TO READ FOR ALL PRINTER!!!!
                        #region Read counter; only for DominoPrinterName.BarCodeInkJet
                        if (this.printerName != GlobalVariables.PrinterName.DegitInkjet)
                        {
                            //    U: Read Counter 1 (ONLY COUNTER 1---COUNTER 2: THE SAME COUNTER 1: Principlely)
                            this.WriteToStream(GlobalVariables.charESC + "/U/001/1/?/" + GlobalVariables.charEOT);
                            if (this.ReadFromStream(ref receivedFeedback, false, "U", 13))
                            {
                                //this.MainStatus = receivedFeedback;

                                int serialNumber = 0;
                                if (int.TryParse(receivedFeedback.Substring(6, 6), out serialNumber) && int.Parse(this.privateFillingData.LastPackNo) != ++serialNumber) //Increase serialNumber by 1: Because: this.privateFillingLineData.MonthSerialNumber MUST GO AHEAD BY 1
                                {
                                    this.privateFillingData.LastPackNo = serialNumber.ToString("0000000").Substring(1);
                                    this.NotifyPropertyChanged("MonthSerialNumber");
                                }
                            }
                        }
                        #endregion Read counter
                    }


                    if (!this.OnPrinting || this.printerName != GlobalVariables.PrinterName.CartonInkjet)
                    {
                        #region Get current status

                        if (this.isLaser)
                            this.WriteToStream("GETSTATUS"); //Determines the current status of the controller
                        else
                            this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status

                        if ((this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "RESULT GETSTATUS", "RESULT GETSTATUS".Length)) || (!this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "O", 9)))
                        {
                            lfStatusLED(ref receivedFeedback);
                            if (!this.LedGreenOn && !this.LedAmberOn) throw new System.InvalidOperationException("Connection fail! Please check your printer.");
                        }


                        ////'//STATUS ONLY.BEGIN
                        //this.WriteToStream(GlobalVariables.charESC + "/1/H/?/" + GlobalVariables.charEOT);      //H: Request History Status
                        //if (this.ReadFromStream(ref receivedFeedback, false, "1", 12)) this.lfStatusHistory(ref receivedFeedback);

                        //this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);      //O/1: Get Current LED Status
                        //if (this.ReadFromStream(ref receivedFeedback, false, "O", 9)) this.lfStatusLED(ref receivedFeedback);

                        //this.WriteToStream(GlobalVariables.charESC + "/O/2/?/" + GlobalVariables.charEOT);      //O/2: Get Current Alert
                        //if (this.ReadFromStream(ref receivedFeedback, false, "O", 0)) this.lfStatusAlert(ref receivedFeedback);
                        ////'//STATUS ONLY.END
                        #endregion Get current status

                        Thread.Sleep(110);
                    }

                } //End while this.LoopRoutine


                //DISCONNECT.BEGIN
                if (this.isLaser)
                    this.WriteToStream("GETSTATUS"); //Determines the current status of the controller
                else
                    this.WriteToStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status

                if ((this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "RESULT GETSTATUS", "RESULT GETSTATUS".Length)) || (!this.isLaser && this.ReadFromStream(ref receivedFeedback, false, "O", 9)))
                    lfStatusLED(ref receivedFeedback);
                //DISCONNECT.END


            }
            catch (Exception exception)
            {
                this.LoopRoutine = false;
                this.MainStatus = exception.Message; // ToString();

                this.LedRedOn = true;
                this.NotifyPropertyChanged("LedStatus");
            }
            finally
            {
                this.Disconnect();
            }



        }

        #endregion Public Thread


    }
}
