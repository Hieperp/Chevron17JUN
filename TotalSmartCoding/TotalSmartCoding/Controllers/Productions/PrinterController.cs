using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using TotalBase;
using TotalBase.Enums;
using TotalCore.Services.Productions;
using TotalDTO.Productions;
using TotalSmartCoding.Libraries.Communications;

namespace TotalSmartCoding.Controllers.Productions
{
    public class PrinterController : CodingController
    {
        #region Storage

        private IBatchService batchService;

        private FillingData privateFillingData;

        private readonly GlobalVariables.PrinterName printerName;
        private readonly bool isLaser;


        private IONetSocket ionetSocket;
        private IOSerialPort ioserialPort;


        private bool onPrinting;
        private bool resetMessage;


        private string lastNACKCode;

        #endregion Storage


        #region Contructor

        public PrinterController(IBatchService batchService, FillingData fillingData, GlobalVariables.PrinterName printerName) : this(batchService, fillingData, printerName, false) { }
        public PrinterController(IBatchService batchService, FillingData fillingData, GlobalVariables.PrinterName printerName, bool isLaser)
        {

            try
            {
                this.batchService = batchService;

                this.FillingData = fillingData;
                this.privateFillingData = this.FillingData.ShallowClone();

                this.printerName = printerName;
                this.isLaser = isLaser;

                this.ionetSocket = new IONetSocket(IPAddress.Parse(GlobalVariables.IpAddress(this.printerName)), 7000, this.isLaser);
                this.ioserialPort = new IOSerialPort(GlobalVariables.ComportName, 9600, Parity.None, 8, StopBits.One, false, "Zebra");


                this.ioserialPort.PropertyChanged += new PropertyChangedEventHandler(ioserialPort_PropertyChanged);
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }


        private void ioserialPort_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                PropertyInfo prop = this.GetType().GetProperty(e.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                    prop.SetValue(this, sender.GetType().GetProperty(e.PropertyName).GetValue(sender, null), null);
                else
                    this.MainStatus = e.PropertyName + ": " + sender.GetType().GetProperty(e.PropertyName).GetValue(sender, null).ToString();
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        #endregion Contructor


        #region Public Properties


        public void StartPrint()
        {
            if (GlobalEnums.OnTestPrinter) this.MainStatus = "Đang in ...";
            this.OnPrinting = true;
        }
        public void StopPrint() { this.OnPrinting = false; }


        public bool OnPrinting
        {
            get { return this.onPrinting; }
            private set { this.onPrinting = value; this.resetMessage = true; }
        }

        public string NextPackNo
        {
            get { return this.privateFillingData.NextPackNo; }
            private set
            {
                if (this.privateFillingData.NextPackNo != value)
                {
                    this.privateFillingData.NextPackNo = value;
                    this.NotifyPropertyChanged("NextPackNo");
                }
            }
        }

        public string NextCartonNo
        {
            get { return this.privateFillingData.NextCartonNo; }
            private set
            {
                if (this.privateFillingData.NextCartonNo != value)
                {
                    this.privateFillingData.NextCartonNo = value;
                    this.NotifyPropertyChanged("NextCartonNo");
                }
            }
        }

        public string NextPalletNo
        {
            get { return this.privateFillingData.NextPalletNo; }
            private set
            {
                if (this.privateFillingData.NextPalletNo != value)
                {
                    this.privateFillingData.NextPalletNo = value;
                    this.NotifyPropertyChanged("NextPalletNo");
                }
            }
        }

        public string getPreviousNo()
        {
            return (int.Parse(this.getNextNo()) - 1).ToString("0000000").Substring(1);
        }

        private string getNextNo()
        {
            if (this.printerName == GlobalVariables.PrinterName.DigitInkjet || this.printerName == GlobalVariables.PrinterName.PackInkjet)
                return this.NextPackNo;
            else
                if (this.printerName == GlobalVariables.PrinterName.CartonInkjet)
                    return this.NextCartonNo;
                else
                    if (this.printerName == GlobalVariables.PrinterName.PalletLabel)
                        return this.NextPalletNo;
                    else
                        return "XXXXXX"; //THIS return value WILL RAISE ERROR TO THE CALLER FUNCTION, BUCAUSE IT DON'T HAVE A CORRECT FORMAT

        }

        private void feedbackNextNo(string nextNo)
        {
            this.feedbackNextNo(nextNo, "");
        }

        private void feedbackNextNo(string nextNo, string receivedFeedback)
        {
            if (nextNo == "" && receivedFeedback.Length > 12)
            {
                int serialNumber = 0;
                if (int.TryParse(receivedFeedback.Substring(6, 6), out serialNumber))
                    nextNo = serialNumber.ToString("0000000").Substring(1);//SHOULD OR NOT: Increase serialNumber by 1 (BECAUSE: nextNo MUST GO AHEAD BY 1??): TEST AT DATMY: FOR AX350: NO NEED, BUCAUSE: AX350 RETURN THE NEXT VALUE. BUT FOR A200+: RETURN THE PRINTED VALUE
            }

            if (nextNo != "")
            {
                if (this.printerName == GlobalVariables.PrinterName.PackInkjet)
                    this.NextPackNo = nextNo;
                else
                    if (this.printerName == GlobalVariables.PrinterName.CartonInkjet)
                        this.NextCartonNo = nextNo;
                    else
                        if (this.printerName == GlobalVariables.PrinterName.PalletLabel)
                            this.NextPalletNo = nextNo;

                lock (this.batchService) //ALL PrinterController MUST SHARE THE SAME IBatchService, BECAUSE WE NEED TO LOCK IBatchService IN ORDER TO CORRECTED UPDATE DATA BY IBatchService
                {
                    if (!this.batchService.CommonUpdate(this.FillingData.BatchID, this.printerName == GlobalVariables.PrinterName.PackInkjet ? nextNo : "", this.printerName == GlobalVariables.PrinterName.CartonInkjet ? nextNo : "", this.printerName == GlobalVariables.PrinterName.PalletLabel ? nextNo : ""))
                        this.MainStatus = this.batchService.ServiceTag;
                }
            }
        }


        #endregion Public Properties


        #region Message Configuration

        /// <summary>
        /// Numeric Serial Only, No Alpha Serial, Zero Leading, 6 Digit: 000001 -> 999999, Step 1, Start this.currentSerialNumber, Repeat: 0 
        /// Don use this Startup Serial Value, because some Dimino printer do no work!!! - DON't KNOW!!! serialNumberFormat = GlobalVariables.charESC + "/j/" + serialNumberIndentity.ToString() + "/N/06/000001/999999/000001/Y/N/0/" + this.currentSerialNumber + "/00000/N/"; //WITH START VALUE---No need to update serial number
        /// WITH START VALUE = 1 ---> NEED TO UPDATE serial number
        /// 
        /// serialNumberIndentity = 1 when print as text on first line, 2 when insert into 2D Barcode
        /// 
        /// IMPORTANT: [EAN BARCODE] DOES NOT ALLOW INSERT SERIAL NUMBER
        /// </summary>
        /// <param name="serialNumberIndentity"></param>
        /// <returns></returns>
        private string dominoSerialNumber(int serialNumberIndentity)
        {
            if (this.printerName != GlobalVariables.PrinterName.PalletLabel)
                return GlobalVariables.charESC + "/j/" + serialNumberIndentity.ToString() + "/N/06/000001/999999/000001/Y/N/0/000000/00000/N/";
            else
                return this.privateFillingData.NextPalletNo; //---Dont use counter (This will be updated MANUALLY for each pallet)
        }




        private string thirdLine(bool isReadableText, int serialIndentity)
        {
            return this.privateFillingData.ThirdLine(isReadableText) + (this.printerName == GlobalVariables.PrinterName.PackInkjet ? "B" : (this.printerName == GlobalVariables.PrinterName.CartonInkjet ? "C" : (this.printerName == GlobalVariables.PrinterName.PalletLabel ? "P" : ""))) + (isReadableText ? " " : "") + (serialIndentity == 1 || serialIndentity == 2 ? this.dominoSerialNumber(serialIndentity) : (this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Freshnew ? this.getNextNo() : this.getPreviousNo()));
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

        private string wholeMessageLine()
        {//THE FUNCTION laserDigitMessage totally base on this.wholeMessageLine. Later, if there is any thing change in this.wholeMessageLine, THE FUNCTION laserDigitMessage should be considered
            if (this.printerName == GlobalVariables.PrinterName.DigitInkjet)
                return this.privateFillingData.FirstLine(true) + " " + this.privateFillingData.SecondLineA1(true) + this.thirdLine(true, 1); //GlobalVariables.charESC + "u/1/" + 
            else if (this.printerName == GlobalVariables.PrinterName.PackInkjet || this.printerName == GlobalVariables.PrinterName.CartonInkjet)
            {
                return GlobalVariables.charESC + "u/3/" + GlobalVariables.charESC + "/z/1/0/26/20/20/1/0/0/0/" + this.privateFillingData.FirstLine(false) + " " + this.privateFillingData.SecondLine(false) + " " + this.thirdLine(false, 2) + "/" + GlobalVariables.charESC + "/z/0" + //2D DATA MATRIX Barcode
                       GlobalVariables.charESC + "u/1/" + " " + this.privateFillingData.FirstLine(true) + "/" +
                       GlobalVariables.charESC + "/r/" + " " + GlobalVariables.charESC + "u/1/" + this.privateFillingData.SecondLine(true) +
                       GlobalVariables.charESC + "/r/" + " " + GlobalVariables.charESC + "u/1/" + this.thirdLine(true, 1);
            }
            else //this.printerName == GlobalVariables.PrinterName.PalletLabel
            {
                string stringMessage = "";
                if (this.OnPrinting)
                {
                    stringMessage = stringMessage + "^XA"; //[^XA - Indicates start of label format.]
                    stringMessage = stringMessage + "^LH60,20"; //[^LH - Sets label home position 80 dots to the right and 30 dots down from top edge of label.]

                    stringMessage = stringMessage + "^FO0,10  ^BC,360,N  ^FD" + this.privateFillingData.FirstLine(false) + this.privateFillingData.SecondLine(false) + this.thirdLine(false, 0) + "^FS";// [^FO0,10 - Set field origin 10 dots to the right and 10 dots down from the home position defined by the ^LH instruction.] [^BC - Select Code 128 bar code.] [^FD - Start of field data for the bar code.] [AAA001 - Actual field data.] [^FS - End of field data.]

                    stringMessage = stringMessage + "^FO770,10 ^AV ^FD" + this.privateFillingData.FirstLine(true) + "^FS"; //[^FO0,330 - Set field origin 10 dots to the right and 330 dots down from the home position defined by the ^LH instruction.] [^AG - Select font “G.”] [^FD - Start of field data.] [ZEBRA - Actual field data.] [^FS - End of field data.]
                    stringMessage = stringMessage + "^FO770,100 ^AV ^FD" + this.privateFillingData.SecondLineA1(true) + "^FS";
                    stringMessage = stringMessage + "^FO770,200 ^AV ^FD" + this.privateFillingData.SecondLineA2(true) + "^FS";
                    stringMessage = stringMessage + "^FO770,306 ^AV ^FD" + this.thirdLine(true, 0) + "^FS";

                    stringMessage = stringMessage + "^XZ"; //[^XZ - Indicates end of label format.]
                }
                else //TEST PAGE ONLY
                {
                    stringMessage = stringMessage + "^XA"; //[^XA - Indicates start of label format.]
                    stringMessage = stringMessage + "^LH60,20"; //[^LH - Sets label home position 80 dots to the right and 30 dots down from top edge of label.]

                    stringMessage = stringMessage + "^FO0,30 ^AS ^FD" + "If you can read this, your printer is ready" + "^FS";
                    stringMessage = stringMessage + "^FO0,80 ^AS ^FD" + "**PLEASE PRESS THE START BUTTON TO BEGIN**" + "^FS";

                    stringMessage = stringMessage + "^FO770,10 ^AV ^FD" + this.privateFillingData.FirstLine(true) + "^FS"; //[^FO0,330 - Set field origin 10 dots to the right and 330 dots down from the home position defined by the ^LH instruction.] [^AG - Select font “G.”] [^FD - Start of field data.] [ZEBRA - Actual field data.] [^FS - End of field data.]
                    stringMessage = stringMessage + "^FO770,100 ^AV ^FD" + this.privateFillingData.SecondLineA1(true) + "^FS";
                    stringMessage = stringMessage + "^FO770,200 ^AV ^FD" + this.privateFillingData.SecondLineA2(true) + "^FS";
                    stringMessage = stringMessage + "^FO770,306 ^AV ^FD" + this.thirdLine(true, 0) + "^FS";

                    stringMessage = stringMessage + "^XZ"; //[^XZ - Indicates end of label format.]
                }

                return stringMessage;
            }
        }

        private string laserDigitMessage(bool isSerialNumber)
        {//THE FUNCTION laserDigitMessage totally base on this.wholeMessageLine. Later, if there is any thing change in this.wholeMessageLine, THE FUNCTION laserDigitMessage should be considered
            if (isSerialNumber)
                return this.getNextNo();
            else
                return this.privateFillingData.CommodityCode + this.privateFillingData.FillingLineCode;
        }//NOTE: NEVER CHANGE THIS FUNCTION WITHOUT HAVE A LOOK AT this.wholeMessageLine

        #endregion Message Configuration


        #region Public Method

        private bool Connect()
        {
            try
            {
                if (!GlobalEnums.OnTestPrinter)
                {
                    this.MainStatus = "Bắt đầu kết nối ....";

                    if (this.printerName != GlobalVariables.PrinterName.PalletLabel)
                        this.ionetSocket.Connect();

                    if (this.printerName == GlobalVariables.PrinterName.PalletLabel)
                    {
                        this.ioserialPort.Connect();
                        //this.ioserialPort.WritetoSerial(this.wholeMessageLine());//FOR TEST AT DESIGN ONLY                        
                    }
                }
                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; return false;
            }

        }

        private bool Disconnect()
        {
            try
            {
                if (!GlobalEnums.OnTestPrinter)
                {
                    this.ionetSocket.Disconnect();
                    this.ioserialPort.Disconnect();

                    this.MainStatus = "Đã ngắt kết nối ...";
                    this.setLED();
                }
                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; return false;
            }

        }


        private void storeMessage(string stringMessage)
        {
            string receivedFeedback = "";

            //S: Message Storage
            this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/S/001/" + stringMessage + "/" + GlobalVariables.charEOT);
            if (this.waitforDomino(ref receivedFeedback, true)) Thread.Sleep(750); else throw new System.InvalidOperationException("Lỗi cài đặt bản tin 001: " + receivedFeedback);

            //P: Message To Head Assignment '//CHU Y QUAN TRONG: DUA MSG SO 1 LEN DAU IN (SAN SANG KHI IN, BOI VI KHI IN TA STORAGE MSG VAO VI TRI SO 1 MA KHONG QUAN TAM DEN VI TRI SO 2, 3,...)
            this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/P/1/001/" + GlobalVariables.charEOT); //FOR AX SERIES: MUST CALL P: Message To Head Assignment FOR EVERY CALL S: Message Storage
            if (this.waitforDomino(ref receivedFeedback, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("Lỗi sẳn sàng in phun, bản tin 001: " + receivedFeedback);
        }


        /// <summary>
        /// NEVER waiforACK WHEN This.IsLaser
        /// </summary>
        /// <param name="receivedFeedback"></param>
        /// <param name="waitforACK"></param>
        /// <returns></returns>
        private bool waitforDomino(ref string receivedFeedback, bool waitforACK)
        {
            return waitforDomino(ref receivedFeedback, waitforACK, "", 0);
        }

        /// <summary>
        /// /// NEVER waitForACK WHEN This.IsLaser
        /// </summary>
        /// <param name="receivedFeedback"></param>
        /// <param name="waitforACK"></param>
        /// <param name="commandCode"></param>
        /// <param name="commandLength"></param>
        /// <returns></returns>
        private bool waitforDomino(ref string receivedFeedback, bool waitforACK, string commandCode, long commandLength)
        {
            try
            {
                receivedFeedback = this.ionetSocket.ReadoutStream();

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
                this.MainStatus = exception.Message; return false;
            }

        }





        /// <summary>
        /// ONLY WHEN: Freshnew OR Reprint
        /// WHEN: Freshnew => Printing1: WAIT FOR PRINTING
        /// WHEN: Reprint => Reprinting1: WAIT FOR REPRINTING
        /// </summary>
        private void sendtoZebra()
        {
            if (this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Freshnew || this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Reprint)
            {//ONLY PRINT WHEN: PrintStatus.Freshnew: AUTO PRINT FOR EACH NEW CartonsetQueue, AND: WHEN = PrintStatus.Reprint: USER PRESS RE-PRINT BUTTON

                if (GlobalEnums.SendToZebra) this.ioserialPort.WritetoSerial(this.wholeMessageLine());

                this.FillingData.CartonsetQueueZebraStatus = this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Freshnew ? GlobalVariables.ZebraStatus.Printing1 : GlobalVariables.ZebraStatus.Reprinting1; Thread.Sleep(88);
            }
        }

        /// <summary>
        /// ONLY WHEN: Printing1 OR Reprinting1
        /// WHEN: Printing1 => Printing2 => Printing3 => IF STILL NOT ACK: Freshnew
        /// WHEN: Reprinting1 => Reprinting2 => Reprinting3 => IF STILL NOT ACK: Reprint
        /// 
        /// FINALLY: WHEN ACK: IF >= Printing1 => MANUAL INCREASE BY 1 THE NextNo FOR THE Freshnew: Format 7 digit, then cut 6 right digit: This will reset a 0 when reach the limit of 6 digit
        /// </summary>
        private void waitforZebra()
        {
            if (this.FillingData.CartonsetQueueZebraStatus >= GlobalVariables.ZebraStatus.Printing1 || this.FillingData.CartonsetQueueZebraStatus <= GlobalVariables.ZebraStatus.Reprinting1)
            {
                string stringReadFrom = "";
                if (true || this.ioserialPort.ReadoutSerial(true, ref stringReadFrom)) //NOW: THE ZEBRA USING IN THIS CHEVRON PROJECT DOES NOT SUPPORT: "Error Detection Protocol" => WE CAN NOT USING TRANSACTION TO GET RESPOND FROM ZEBRA PRINTER
                {
                    this.feedbackNextNo(this.FillingData.CartonsetQueueZebraStatus >= GlobalVariables.ZebraStatus.Printing1 ? (int.Parse(this.getNextNo()) + 1).ToString("0000000").Substring(1) : this.getNextNo());
                    this.FillingData.CartonsetQueueZebraStatus = GlobalVariables.ZebraStatus.Printed;
                }
                else
                {
                    this.MainStatus = "Lỗi máy in zebra. Đang thử in lại ... ";
                    if (this.FillingData.CartonsetQueueZebraStatus >= GlobalVariables.ZebraStatus.Printing1) this.FillingData.CartonsetQueueZebraStatus = this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Printing1 ? GlobalVariables.ZebraStatus.Printing2 : (this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Printing2 ? GlobalVariables.ZebraStatus.Printing3 : GlobalVariables.ZebraStatus.Freshnew);
                    if (this.FillingData.CartonsetQueueZebraStatus <= GlobalVariables.ZebraStatus.Reprinting1) this.FillingData.CartonsetQueueZebraStatus = this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Reprinting1 ? GlobalVariables.ZebraStatus.Reprinting2 : (this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Reprinting2 ? GlobalVariables.ZebraStatus.Reprinting3 : GlobalVariables.ZebraStatus.Reprint);
                }
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
                { //DATE 26-JUL-2017: LedGreenOn = LedGreenOn (ORIGINAL LedGreenOn) || LedAmberOn. THIS MEANS: LedGreenOn OR LedAmberOn: THE PRINTER IS READY FOR PRINTING!!!
                    this.LedGreenOn = ((int)receivedFeedback.ElementAt(7) & int.Parse("1")) == 1 || ((int)receivedFeedback.ElementAt(7) & int.Parse("2")) == 2;
                    this.LedAmberOn = ((int)receivedFeedback.ElementAt(7) & int.Parse("2")) == 2;
                    this.LedRedOn = ((int)receivedFeedback.ElementAt(7) & int.Parse("3")) == 3;
                }
                this.NotifyPropertyChanged("LedStatus");

                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; return false;
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
                this.MainStatus = exception.Message; return false;
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

            string receivedFeedback = ""; bool printerReady = false; bool readytoPrint = false; bool headEnable = false;


            this.LoopRoutine = true; this.StopPrint();


            if (GlobalEnums.OnTestPrinter && this.printerName != GlobalVariables.PrinterName.DigitInkjet) this.feedbackNextNo((int.Parse(this.getNextNo()) + 1).ToString("0000000").Substring(1));

            //This command line is specific to: PalletLabel ON FillingLine.Drum || CartonInkjet ON FillingLine.Pail (Just here only for this specific)
            if (GlobalEnums.OnTestPrinter || (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Drum && this.printerName != GlobalVariables.PrinterName.PalletLabel) || (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Pail && this.printerName == GlobalVariables.PrinterName.PackInkjet) || (this.printerName == GlobalVariables.PrinterName.DigitInkjet && GlobalEnums.OnTestDigit)) { this.LedGreenOn = true; return; } //DO NOTHING


            try
            {
                if (!this.Connect()) throw new System.InvalidOperationException("Lỗi kết nối máy in");

                if (this.printerName == GlobalVariables.PrinterName.PalletLabel)
                { //SHOULD HAVE COMMAND TO CHECK ZEBRA PRINTER EXISTING AND WORKING WELL. !!!!BUT: NOW: THE ZEBRA USING IN THIS CHEVRON PROJECT DOES NOT SUPPORT: "Error Detection Protocol" => WE CAN NOT USING TRANSACTION TO GET RESPOND FROM ZEBRA PRINTER 
                    if (!GlobalEnums.OnTestZebra) this.ioserialPort.WritetoSerial(this.wholeMessageLine()); //TRY TO PRINT THE TEST PAGE -> TO VERIFY THAT: THE ZEBRA IS OK
                    this.setLED(true, this.LedAmberOn, this.LedRedOn);
                }
                else
                {//USING DOMINO PRINTER
                    #region INITIALISATION PRINTER
                    do  //INITIALISATION COMMAND
                    {
                        if (this.isLaser)
                        {
                            this.ionetSocket.WritetoStream("GETVERSION"); //Obtains the alphanumeric identifier of the printer
                            if (this.waitforDomino(ref receivedFeedback, false, "RESULT GETVERSION", "RESULT GETVERSION".Length)) printerReady = true; //Printer Identity OK"
                        }
                        else
                        {
                            this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/A/?/" + GlobalVariables.charEOT);   //A: Printer Identity
                            if (this.waitforDomino(ref receivedFeedback, false, "A", 14)) printerReady = true; //A: Printer Identity OK"
                        }


                        if (printerReady)
                        {
                            do //CHECK PRINTER READY TO PRINT
                            {
                                if (this.isLaser)
                                    this.ionetSocket.WritetoStream("GETSTATUS"); //Determines the current status of the controller
                                else
                                    this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status


                                if ((this.isLaser && this.waitforDomino(ref receivedFeedback, false, "RESULT GETSTATUS", "RESULT GETSTATUS".Length)) || (!this.isLaser && this.waitforDomino(ref receivedFeedback, false, "O", 9)))
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
                                        this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/O/S/1/" + GlobalVariables.charEOT); //O/S/1: Turn on ink-jet
                                        if (this.waitforDomino(ref receivedFeedback, true))
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
                                        this.ionetSocket.WritetoStream("GETMARKMODE"); //Determines the current state of the marking engine on the laser controller
                                    else
                                        this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/Q/1/?/" + GlobalVariables.charEOT);    //Q: HEAD ENABLE: ENABLE


                                    if ((this.isLaser && this.waitforDomino(ref receivedFeedback, false, "RESULT GETMARKMODE", "RESULT GETMARKMODE".Length)) || (!this.isLaser && this.waitforDomino(ref receivedFeedback, false, "Q", 5)))
                                    {
                                        if ((this.isLaser && receivedFeedback.ElementAt(19).ToString() == "1") || (!this.isLaser && receivedFeedback.ElementAt(3).ToString() == "Y"))
                                            headEnable = true;
                                        else
                                        {
                                            if (this.isLaser)
                                                this.ionetSocket.WritetoStream("MARK START");
                                            else
                                                this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/Q/1/Y/" + GlobalVariables.charEOT);


                                            if ((this.isLaser && this.waitforDomino(ref receivedFeedback, false, "OK", "OK".Length)) || (!this.isLaser && this.waitforDomino(ref receivedFeedback, true)))
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
                        this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/C/" + DateTime.Now.ToString("yyyy/MM/dd/00/hh/mm/ss") + "/" + GlobalVariables.charEOT);     //C: Set Clock
                        if (!this.waitforDomino(ref receivedFeedback, true)) throw new System.InvalidOperationException("Lỗi cài đặt ngày giờ máy in phun: " + receivedFeedback);

                        //T: Reset Product Counting
                        this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/T/1/0/" + GlobalVariables.charEOT);
                        if (!this.waitforDomino(ref receivedFeedback, true)) throw new System.InvalidOperationException("Lỗi cài đặt bộ đếm số lần in phun: " + receivedFeedback);
                    }
                    #endregion GENERAL SETUP


                    #region Status (NOT LASER ONLY)
                    //SET STATUS
                    if (!this.isLaser)
                    {
                        this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/0/N/0/" + GlobalVariables.charEOT);     //0: Status Report Mode: OFF: NO ERROR REPORTING
                        if (!this.waitforDomino(ref receivedFeedback, true)) throw new System.InvalidOperationException("NMVN: Can not set status report mode: " + receivedFeedback);

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
                }

                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    if (!this.OnPrinting)
                    {
                        #region Reset Message: Clear message: printerName != PalletLabel

                        if (this.printerName != GlobalVariables.PrinterName.PalletLabel && this.resetMessage)
                        {
                            this.MainStatus = "Vui lòng chờ ... ";

                            if (this.isLaser)
                            {
                                this.ionetSocket.WritetoStream("MARK STOP");
                                if (this.waitforDomino(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(7000); else throw new System.InvalidOperationException("Can not disables printing ... : " + receivedFeedback);
                            }
                            else
                                this.storeMessage("  ");


                            if (this.isLaser)
                                this.ionetSocket.WritetoStream("LOADPROJECT store: SLASHSYMBOL Demo");
                            else
                                this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/I/1/ /" + GlobalVariables.charEOT); //SET OF: Print Acknowledgement Flags I

                            if ((this.isLaser && this.waitforDomino(ref receivedFeedback, false, "OK", "OK".Length)) || (!this.isLaser && this.waitforDomino(ref receivedFeedback, true))) Thread.Sleep(250); else throw new System.InvalidOperationException("Can not set off printing acknowledge/ Load Demo project: " + receivedFeedback);

                            this.resetMessage = false; //Setup first message: Only one times      
                            this.MainStatus = "Đang kết nối với máy in.";
                        }
                        #endregion Reset Message: Clear message: this.printerName != GlobalVariables.PrinterName.PalletLabel
                    }

                    else //this.OnPrinting
                    {
                        #region Reset Message: printerName != PalletLabel

                        if (this.printerName != GlobalVariables.PrinterName.PalletLabel && this.resetMessage)
                        {
                            #region SETUP MESSAGE
                            this.MainStatus = "Vui lòng chờ ...";

                            if (this.isLaser && this.printerName == GlobalVariables.PrinterName.DigitInkjet) //stringWriteTo = " SETVARIABLES \"MonthCodeAndLine\" \"10081\"\r\n"
                            {//BEGINTRANS [ENTER] OK   SETTEXT "Text 1" "Domino AG" [ENTER]   OK   SETTEXT "Barcode 1" "Sator Laser GmbH" [ENTER]   OK EXECTRANS [ENTER] OK MSG 1 
                                //this.WriteToStream("BEGINTRANS");
                                //if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message: " + receivedFeedback);

                                this.ionetSocket.WritetoStream("LOADPROJECT store: SLASHSYMBOL " + (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Smallpack ? "BPCODigit" : (this.FillingData.FillingLineID == GlobalVariables.FillingLine.Pail ? "BPPailDigit" : "BPPailDigit")));
                                if (this.waitforDomino(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(500); else throw new System.InvalidOperationException("NMVN: Can not load message: " + receivedFeedback);

                                this.ionetSocket.WritetoStream("SETVARIABLES \"MonthCodeAndLine\" \"" + this.laserDigitMessage(false) + "\"");
                                if (this.waitforDomino(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message code: " + receivedFeedback);

                                this.ionetSocket.WritetoStream("SETCOUNTERVALUE Serialnumber01 " + this.laserDigitMessage(true));
                                if (this.waitforDomino(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message counter: " + receivedFeedback);

                                this.ionetSocket.WritetoStream("MARK START");
                                if (this.waitforDomino(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(7000); else throw new System.InvalidOperationException("NMVN: Can not enables marking ... : " + receivedFeedback);

                                //this.WriteToStream("EXECTRANS");
                                //if (this.ReadFromStream(ref receivedFeedback, false, "OK", "OK".Length)) Thread.Sleep(20); else throw new System.InvalidOperationException("NMVN: Can not set message: " + receivedFeedback);
                            }
                            else
                            {
                                this.storeMessage(this.wholeMessageLine()); //SHOULD Update serial number: - Note: Some DOMINO firmware version does not need to update serial number. Just set startup serial number only when insert serial number. BUT: FOR SURE, It will be updated FOR ALL

                                //    U: UPDATE SERIAL NUMBER - Counter 1
                                this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/U/001/1/" + this.getNextNo() + "/" + GlobalVariables.charEOT);
                                if (this.waitforDomino(ref receivedFeedback, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("Lỗi không thể cài đặt số thứ tự sản phẩm: " + receivedFeedback);

                                //    U: UPDATE SERIAL NUMBER - Counter 2
                                this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/U/001/2/" + this.getNextNo() + "/" + GlobalVariables.charEOT);
                                if (this.waitforDomino(ref receivedFeedback, true)) Thread.Sleep(1000); else throw new System.InvalidOperationException("Lỗi không thể cài đặt số thứ tự sản phẩm: " + receivedFeedback);
                            }
                            #endregion SETUP MESSAGE

                            this.MainStatus = "Đang in ...";
                            this.resetMessage = false; //Setup first message: Only one times                            
                        }

                        #endregion Reset Message: this.printerName != GlobalVariables.PrinterName.PalletLabel


                        #region Read counter: printerName == PackInkjet || printerName == CartonInkjet
                        if (this.printerName == GlobalVariables.PrinterName.PackInkjet || this.printerName == GlobalVariables.PrinterName.CartonInkjet)
                        {
                            this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/U/001/1/?/" + GlobalVariables.charEOT);//    U: Read Counter 1 (ONLY COUNTER 1---COUNTER 2: THE SAME COUNTER 1: Principlely)
                            if (this.waitforDomino(ref receivedFeedback, false, "U", 13))
                                this.feedbackNextNo("", receivedFeedback);
                        }
                        #endregion Read counter


                        #region Setup for every message: printerName == PalletLabel
                        if (this.printerName == GlobalVariables.PrinterName.PalletLabel && (this.FillingData.CartonsetQueueCount > 0 || !this.FillingData.HasCarton))
                        {
                            //if (!this.FillingData.HasCarton && this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Printed) 
                            //    this.FillingData.CartonsetQueueZebraStatus = GlobalVariables.ZebraStatus.Freshnew;

                            this.sendtoZebra();
                            this.waitforZebra();
                        }
                        #endregion setup for every message: printerName == PalletLabel
                    }


                    if (!this.OnPrinting || this.printerName != GlobalVariables.PrinterName.CartonInkjet)
                    {
                        #region Get current status: ONLY printerName != PalletLabel
                        if (this.printerName != GlobalVariables.PrinterName.PalletLabel)
                        {
                            if (this.isLaser)
                                this.ionetSocket.WritetoStream("GETSTATUS"); //Determines the current status of the controller
                            else
                                this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status

                            if ((this.isLaser && this.waitforDomino(ref receivedFeedback, false, "RESULT GETSTATUS", "RESULT GETSTATUS".Length)) || (!this.isLaser && this.waitforDomino(ref receivedFeedback, false, "O", 9)))
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
                        }
                        #endregion Get current status
                        Thread.Sleep(110);
                    }

                } //End while this.LoopRoutine

                #region ON EXIT LOOP
                if (this.printerName != GlobalVariables.PrinterName.PalletLabel)
                {
                    if (this.isLaser)
                        this.ionetSocket.WritetoStream("GETSTATUS"); //Determines the current status of the controller
                    else
                        this.ionetSocket.WritetoStream(GlobalVariables.charESC + "/O/1/?/" + GlobalVariables.charEOT);  //O/1: Current status

                    if ((this.isLaser && this.waitforDomino(ref receivedFeedback, false, "RESULT GETSTATUS", "RESULT GETSTATUS".Length)) || (!this.isLaser && this.waitforDomino(ref receivedFeedback, false, "O", 9)))
                        lfStatusLED(ref receivedFeedback);
                }
                #endregion ON EXIT LOOP
            }
            catch (Exception exception)
            {
                this.LoopRoutine = false;
                this.MainStatus = exception.Message;

                this.setLED(this.LedGreenOn, this.LedAmberOn, true);
            }
            finally
            {
                this.Disconnect();
            }

        }

        #endregion Public Thread

    }
}
