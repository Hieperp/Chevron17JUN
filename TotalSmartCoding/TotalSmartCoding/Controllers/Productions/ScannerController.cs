using Ninject;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TotalBase;
using TotalCore.Services.Productions;
using TotalDTO.Productions;
using TotalService.Productions;
using TotalSmartCoding.CommonLibraries.BP;

using TotalBase.Enums;
using TotalSmartCoding.ViewModels.Productions;
using TotalSmartCoding.Builders.Productions;
using TotalSmartCoding.CommonLibraries;

using TotalSmartCoding.Controllers.Productions;

namespace TotalSmartCoding.Controllers.Productions
{
    public class ScannerController : CodingController //this is CommonThreadProperty
    {


        public bool MyTest; //Test only
        public bool MyHold;//Test only

        private FillingLineData privateFillingLineData;

        private TcpClient barcodeTcpClient;
        private NetworkStream barcodeNetworkStream;

        private GlobalVariables.BarcodeScannerName barcodeScannerName;
        private IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        private int portNumber = 2112;




        private SerialPort serialPort;


        private BarcodeQueue<OnlinePackDTO> matchingPackList;
        private BarcodeQueue<OnlinePackDTO> packInOneCarton;

        private BarcodeQueue<OnlineCartonDTO> cartonDataTable;
        private BarcodeQueue<OnlinePalletDTO> OnlinePalletQueue;

        private bool onPrinting;
        private bool resetMessage;
        private bool bufferReset;

        private string lastStringBarcode;

        //barcodeScannerNameID
        private BarcodeScannerStatus[] barcodeScannerStatus;

        #region Contructor

        //Initialize
        //READ, WRITE Registry
        //Servernme + database name
        //Toolbar enable

        private OnlinePackController onlinePackService;
        private OnlineCartonController onlineCartonService;
        private OnlinePalletController onlinePalletService;

        public ScannerController(FillingLineData fillingLineData)
        {
            try
            {
                base.FillingLineData = fillingLineData;
                this.privateFillingLineData = this.FillingLineData.ShallowClone();

                this.barcodeScannerName = GlobalVariables.BarcodeScannerName.MatchingScanner;

                this.ipAddress = IPAddress.Parse(GlobalVariables.IpAddress(this.BarcodeScannerName));


                this.onlinePackService = new OnlinePackController(CommonNinject.Kernel.Get<IOnlinePackService>(), CommonNinject.Kernel.Get<IOnlinePackViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<OnlinePackViewModel>());
                this.onlineCartonService = new OnlineCartonController(CommonNinject.Kernel.Get<IOnlineCartonService>(), CommonNinject.Kernel.Get<IOnlineCartonViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<OnlineCartonViewModel>());
                this.onlinePalletService = new OnlinePalletController(CommonNinject.Kernel.Get<IOnlinePalletService>(), CommonNinject.Kernel.Get<IOnlinePalletViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<OnlinePalletViewModel>());



                this.barcodeScannerStatus = new BarcodeScannerStatus[3];
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.QualityScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.QualityScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.MatchingScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.MatchingScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.CartonScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.CartonScanner);




                //if (GlobalVariables.PortName != "COM 0")
                //{
                this.serialPort = new SerialPort();
                this.serialPort.PortName = GlobalVariables.PortName;
                this.serialPort.BaudRate = 9600;
                this.serialPort.NewLine = GlobalVariables.charETX.ToString();


                this.serialPort.ReadTimeout = 500;
                //this.serialPort.WriteTimeout = 500;
                //this.serialPort.Parity = value;
                //this.serialPort.DataBits = value;
                //this.serialPort.StopBits = value;
                //this.serialPort.Handshake = value;

                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                this.serialPort.PinChanged += new SerialPinChangedEventHandler(serialPort_PinChanged);
                //}


                this.matchingPackList = new BarcodeQueue<OnlinePackDTO>(GlobalVariables.NoItemDiverter());
                this.packInOneCarton = new BarcodeQueue<OnlinePackDTO>();
                this.cartonDataTable = new BarcodeQueue<OnlineCartonDTO>();
                this.OnlinePalletQueue = new BarcodeQueue<OnlinePalletDTO>();
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }




        public void Initialize()
        {
            try
            {
                //return;

                //HIEP*******************************22-MAY-2017.BEGIN
                ////////////--------------DON'T LOAD. THIS IS VERY OK, AND READY TO LOAD PACK IF NEEDED.
                //////////////Initialize MatchingPackList and PackInOneCarton (of the this.FillingLineData.FillingLineID ONLY)
                //////this.PackDataTable = this.PackTableAdapter.GetData();
                //////foreach (DataDetail.DataDetailPackRow packRow in this.PackDataTable)
                //////{
                //////    if (packRow.FillingLineID == (int)this.FillingLineData.FillingLineID)
                //////    {
                //////        OnlinePackDTO messageData = new OnlinePackDTO(packRow.PackBarcode);
                //////        messageData.PackID = packRow.PackID;
                //////        messageData.QueueID = packRow.QueueID;

                //////        if (packRow.PackStatus == (byte)GlobalVariables.BarcodeStatus.Normal)
                //////            this.matchingPackList.AddPack(messageData);
                //////        else if (packRow.PackStatus == (byte)GlobalVariables.BarcodeStatus.ReadyToCarton)
                //////            this.packInOneCarton.AddPack(messageData);
                //////    }
                //////}
                //////this.PackDataTable = null;

                //////this.NotifyPropertyChanged("MatchingPackList");
                //////this.NotifyPropertyChanged("PackInOneCarton");
                ////////////--------------
                //HIEP*******************************22-MAY-2017.BEGIN

                ////////Initialize CartonList
                //////this.cartonDataTable = this.CartonTableAdapter.GetDataByCartonStatus((byte)GlobalVariables.BarcodeStatus.BlankBarcode);
                //////this.NotifyPropertyChanged("CartonList");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion Contructor


        #region Public Properties


        public GlobalVariables.BarcodeScannerName BarcodeScannerName
        {
            get
            {
                return this.barcodeScannerName;
            }
        }


        public IPAddress IpAddress
        {
            get
            {
                return this.ipAddress;
            }
        }


        public int PortNumber
        {
            get
            {
                return this.portNumber;
            }
        }




        public bool OnPrinting
        {
            get { return this.onPrinting; }
            private set { this.onPrinting = value; this.resetMessage = true; }
        }


        public void StartPrint() { this.lastStringBarcode = ""; GlobalVariables.DuplicateCartonBarcodeFound = false; this.bufferReset = true; this.OnPrinting = true; } //ONLY bufferReset WHEN StartPrint
        public void StopPrint() { this.OnPrinting = false; }






        public bool LedMCUQualityOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.QualityScanner - 1].MCUReady; }
        }

        public bool LedMCUMatchingOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.MatchingScanner - 1].MCUReady; }
        }

        public bool LedMCUCartonOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.CartonScanner - 1].MCUReady; }
        }

        public void SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName barcodeScannerNameID, bool mcuReady)
        {
            if (this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUReady != mcuReady)
            {
                this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUReady = mcuReady;
                this.NotifyPropertyChanged("LedMCU");
            }
        }

        public void SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName barcodeScannerNameID, string mcuStatus)
        {
            if (this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUStatus != mcuStatus)
            {
                this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUStatus = mcuStatus;
                this.MainStatus = barcodeScannerNameID.ToString() + ": " + mcuStatus;
            }

            this.SetBarcodeScannerStatus(barcodeScannerNameID, false);
        }





        public string BatchSerialNumber { get { return this.privateFillingLineData.BatchSerialNumber; } }

        public string MonthSerialNumber { get { return this.privateFillingLineData.MonthSerialNumber; } }

        public string BatchCartonNumber { get { return this.privateFillingLineData.BatchCartonNumber; } }

        public string MonthCartonNumber { get { return this.privateFillingLineData.MonthCartonNumber; } }

        public int MatchingPackCount { get { return this.matchingPackList.Count; } }
        public int PackInOneCartonCount { get { return this.packInOneCarton.Count; } }

        #endregion Public Properties

        #region Public Method

        public DataTable GetMatchingPackList()
        {
            if (this.matchingPackList != null)
            {
                lock (this.matchingPackList)
                {
                    return this.matchingPackList.GetAllElements();
                }
            }
            else return null;
        }

        public DataTable GetPackInOneCarton()
        {
            if (this.packInOneCarton != null)
            {
                lock (this.packInOneCarton)
                {
                    return this.packInOneCarton.GetAllElements();
                }
            }
            else return null;
        }

        public DataTable GetCartonList()
        {
            if (this.cartonDataTable != null)
            {
                lock (this.cartonDataTable)
                {
                    return this.cartonDataTable.GetAllElements();
                }
            }
            else return null;
        }

        public DataTable GetOnlinePalletQueue()
        {
            if (this.OnlinePalletQueue != null)
            {
                lock (this.OnlinePalletQueue)
                {
                    return this.OnlinePalletQueue.GetAllElements();
                }
            }
            else return null;
        }

        private bool Connect(bool connectMatchingScanner)
        {
            try
            {
                if (GlobalEnums.OnTestOnly)
                    this.StartPrint();
                else
                {
                    this.MainStatus = "Try to connect....";

                    if (GlobalVariables.PortName != "COM 0")
                    {
                        if (this.serialPort.IsOpen) this.serialPort.Close();
                        this.serialPort.Open();

                        if (!this.serialPort.IsOpen) throw new System.InvalidOperationException("NMVN: Can not connect to COMPORT: " + this.serialPort.PortName);
                    }


                    if (connectMatchingScanner)
                    {
                        this.barcodeTcpClient = new TcpClient();

                        if (!this.barcodeTcpClient.Connected)
                        {
                            this.barcodeTcpClient.Connect(this.IpAddress, this.PortNumber);
                            this.barcodeNetworkStream = barcodeTcpClient.GetStream();
                        }
                    }
                }

                this.LedGreenOn = true;
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

        private bool Disconnect()
        {
            try
            {
                this.MainStatus = "Disconnect....";


                if (GlobalVariables.PortName != "COM 0" && this.serialPort.IsOpen) this.serialPort.Close();



                //if (this.inkJetTcpClient.Connected) --- Theoryly, it should CHECK this.inkJetTcpClient.Connected BEFORE close. BUT: DON'T KNOW why GlobalVariables.DominoPrinterName.CartonInkJet DISCONECTED ALREADY!!!! Should check this cerefully later!
                //{
                if (this.barcodeNetworkStream != null) { this.barcodeNetworkStream.Close(); this.barcodeNetworkStream.Dispose(); }

                if (this.barcodeTcpClient != null) this.barcodeTcpClient.Close();
                //}


                this.LedGreenOn = false;
                this.LedAmberOn = false;
                this.LedRedOn = false;
                this.NotifyPropertyChanged("LedStatus");

                this.SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName.QualityScanner, true);
                this.SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName.MatchingScanner, true);
                this.SetBarcodeScannerStatus(GlobalVariables.BarcodeScannerName.CartonScanner, true);



                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; // ToString();
                return false;
            }

        }

        private bool WaitForBarcode(ref string stringReadFrom)
        {
            try
            {
                if (GlobalEnums.OnTestOnly)
                    if ((DateTime.Now.Second % 4) == 0) stringReadFrom = "22677531 087 030117 443" + DateTime.Now.Millisecond.ToString("000000") + " 000003"; else stringReadFrom = "";
                else
                {
                    this.barcodeNetworkStream.ReadTimeout = 120; //Default = -1; 

                    stringReadFrom = GlobalNetSockets.ReadFromStream(barcodeTcpClient, barcodeNetworkStream).Trim();

                    this.barcodeNetworkStream.ReadTimeout = -1; //Default = -1

                    if (stringReadFrom != "") this.MainStatus = stringReadFrom;


                }
                return stringReadFrom != "";
            }

            catch (Exception exception)
            {
                this.barcodeNetworkStream.ReadTimeout = -1; //Default = -1

                //Ignore when timeout
                if (exception.Message == "Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.")
                {
                    stringReadFrom = "";
                    return false;
                }
                else
                    throw exception;




                ////this.LedGreenOn = this.barcodeTcpClient.Connected;//this.LedAmberOn = readyToPrint;
                ////this.LedRedOn = !this.barcodeTcpClient.Connected;
                ////this.NotifyPropertyChanged("LedStatus");


                ////this.MainStatus = this.LedGreenOn ? (this.onPrinting ? "Scanning ...." : "Ready to scan") : exception.Message;


            }
        }


        private bool WaitForCarton(ref string stringReadFrom)
        {
            try
            {
                if (GlobalEnums.OnTestOnly)
                    if ((DateTime.Now.Second % 6) == 0 && this.packInOneCarton.Count > 0) stringReadFrom = "22677531 087 030117 443" + DateTime.Now.Millisecond.ToString("000000") + " 000003"; else stringReadFrom = "";
                else
                {
                    //*****MODIFY HERE: this.barcodeNetworkStream.ReadTimeout = 120; //Default = -1; 

                    //*****MODIFY HERE: stringReadFrom = GlobalNetSockets.ReadFromStream(barcodeTcpClient, barcodeNetworkStream).Trim();

                    //*****MODIFY HERE: this.barcodeNetworkStream.ReadTimeout = -1; //Default = -1

                    //*****MODIFY HERE: if (stringReadFrom != "") this.MainStatus = stringReadFrom;
                }
                return stringReadFrom != "";
            }

            catch (Exception exception)
            {
                //*****MODIFY HERE: this.barcodeNetworkStream.ReadTimeout = -1; //Default = -1

                //Ignore when timeout
                if (exception.Message == "Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.")
                {
                    stringReadFrom = "";
                    return false;
                }
                else
                    throw exception;
            }
        }



        private OnlinePackDTO AddDataDetailPack(string code, int queueID)
        {
            try
            {
                OnlinePackViewModel onlinePackDTO = new OnlinePackViewModel() { QueueID = queueID, Code = code };

                if (this.onlinePackService.onlinePackService.Save(onlinePackDTO))
                    return onlinePackDTO;
                else
                {
                    this.MainStatus = "Insufficient save pack: " + code;
                    return null;
                }
            }
            catch (System.Exception exception)
            {
                throw new Exception("Insufficient save pack: " + exception.Message + " [" + code + "]");
            }
        }


        private void MatchingAndAddCarton(string cartonCode)
        {
            if (GlobalVariables.IgnoreEmptyCarton && this.packInOneCarton.Count <= 0) return;

            lock (this.cartonDataTable)
            {
                lock (this.packInOneCarton)
                {
                    //CẦN CHÚ Ý: this.packInOneCarton.Count = 0: CARTON RỔNG => PHẢI XỬ LÝ NHƯ THẾ NÀO???

                    OnlineCartonDTO onlineCartonDTO = new OnlineCartonDTO() { Code = cartonCode };

                    this.onlineCartonService.onlineCartonService.ServiceBag["OnlinePackIDs"] = this.packInOneCarton.EntityIDs; //VERY IMPORTANT: NEED TO ADD OnlinePackIDs TO NEW OnlineCartonDTO
                    if (this.onlineCartonService.onlineCartonService.Save(onlineCartonDTO))
                        this.packInOneCarton = new BarcodeQueue<OnlinePackDTO>(); //CLEAR AFTER ADD TO OnlineCartonDTO
                    else
                        throw new Exception("Insufficient save carton: " + onlineCartonDTO.Code);


                    this.cartonDataTable.Enqueue(onlineCartonDTO);
                }
            }
        }




        #endregion Public Method


        #region Public Thread

        public void ThreadRoutine()
        {
            this.privateFillingLineData = this.FillingLineData.ShallowClone();

            string stringReadFrom = ""; bool matchingPackListChanged = false; bool packInOneCartonChanged = false;

            this.LoopRoutine = true; this.StopPrint();


            try
            {

                if (this.Connect(this.FillingLineData.FillingLineID != GlobalVariables.FillingLine.Pail)) this.serialPort.NewLine = GlobalVariables.charETX.ToString(); else throw new System.InvalidOperationException("NMVN: Can not connect to comport.");

                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    this.MainStatus = this.OnPrinting ? "Scanning..." : "Ready to scan";

                    if (this.FillingLineData.FillingLineID != GlobalVariables.FillingLine.Pail)
                    {

                        #region Read every pack

                        if (this.WaitForBarcode(ref stringReadFrom) && this.OnPrinting)
                        {
                            string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

                            foreach (string stringBarcode in arrayBarcode)
                            {
                                string receivedBarcode = stringBarcode.Replace("NoRead", "");

                                if (receivedBarcode.Trim() != "" && receivedBarcode.Trim() != "NoRead") //Add to MatchingPackList
                                {
                                    lock (this.matchingPackList)
                                    {
                                        OnlinePackDTO messageData = this.AddDataDetailPack(receivedBarcode, this.matchingPackList.NextQueueID);
                                        if (messageData != null)
                                        {
                                            this.matchingPackList.Enqueue(messageData);
                                            matchingPackListChanged = true;
                                        }
                                    }

                                }
                            }
                        }

                        #endregion Read every pack


                        #region Make One Carton

                        if (this.OnPrinting)
                        {
                            lock (this.packInOneCarton)
                            {
                                if (this.packInOneCarton.Count <= 0)
                                {
                                    lock (this.matchingPackList)
                                    {                             //<Dequeue from matchingPackList to this.packInOneCarton>
                                        this.packInOneCarton = this.matchingPackList.DequeuePack(); //This Dequeue successful WHEN There is enought OnlinePackDTO IN THE COMMON GLOBAL matchingPackList, ELSE: this.packListInCarton is still NULL
                                    }

                                    if (this.packInOneCarton.Count > 0)
                                    {
                                        matchingPackListChanged = true;
                                        packInOneCartonChanged = true;

                                        if (!this.onlinePackService.onlinePackService.UpdateEntryStatus(this.packInOneCarton.EntityIDs, GlobalVariables.BarcodeStatus.ReadyToCarton)) this.MainStatus = "Insufficient update pack status: " + this.onlinePackService.onlinePackService.ServiceTag;
                                    }
                                }
                            }

                        }
                        #endregion Make One Carton



                        #region Read every carton

                        if (this.WaitForCarton(ref stringReadFrom) && this.OnPrinting)
                            this.ReceiveCarton(stringReadFrom);

                        #endregion Read every carton



                        if (matchingPackListChanged) { this.NotifyPropertyChanged("MatchingPackList"); matchingPackListChanged = false; }
                        if (packInOneCartonChanged) { this.NotifyPropertyChanged("PackInOneCarton"); packInOneCartonChanged = false; }

                    }
                    Thread.Sleep(100);

                } //End while this.LoopRoutine
            }
            catch (Exception exception)
            {
                this.LoopRoutine = false;
                this.MainStatus = exception.Message;

                this.LedRedOn = true;
                this.NotifyPropertyChanged("LedStatus");
            }
            finally
            {
                this.Disconnect();
            }

        }


        void ReceiveCarton(string stringReadFrom)
        {
            bool barcodeReceived = false;
            string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

            foreach (string stringBarcode in arrayBarcode)
            {
                string receivedBarcode = stringBarcode.Replace("NoRead", "");

                if (receivedBarcode.Trim() != "" && receivedBarcode.Trim() != "NoRead")
                {
                    MatchingAndAddCarton(receivedBarcode);
                    barcodeReceived = true;
                }
            }

            if (barcodeReceived) { this.NotifyPropertyChanged("PackInOneCarton"); this.NotifyPropertyChanged("CartonList"); barcodeReceived = false; }
        }


        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        { //Carton Barcode
            try
            {
                if (this.OnPrinting)
                {
                    //Thread.Sleep(30);
                    bool barcodeReceived = false;
                    //string stringReadFrom = serialPort.ReadExisting();
                    string stringReadFrom = serialPort.ReadLine();
                    string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

                    foreach (string stringBarcode in arrayBarcode)
                    {
                        string receivedBarcode = stringBarcode.Replace("NoRead", "");

                        if (receivedBarcode.Trim() != "" && receivedBarcode.Trim() != "NoRead")
                        {
                            if (this.lastStringBarcode == receivedBarcode)//Check for DuplicateCartonBarcodeFound
                            {
                                this.serialPort.RtsEnable = true; //PIN 7: RtsEnable    PIN 4: DtrEnable
                                GlobalVariables.DuplicateCartonBarcodeFound = true;
                            }
                            else
                                this.lastStringBarcode = receivedBarcode;


                            MatchingAndAddCarton(receivedBarcode + (this.FillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail ? " " + this.privateFillingLineData.BatchSerialNumber : ""));



                            if (this.FillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail)
                            {//ONLY FOR PAIL LINE: BECAUSE: With PAIL Line: use CartonScanner (datalogic) to read Packbarcode
                                this.privateFillingLineData.BatchSerialNumber = (int.Parse(this.privateFillingLineData.BatchSerialNumber) + 1).ToString("0000000").Substring(1);//Format 7 digit, then cut 6 right digit: This will reset a 0 when reach the limit of 6 digit
                                this.NotifyPropertyChanged("BatchSerialNumber"); //APPEND TO receivedBarcode, and then: Increase BatchSerialNumber by 1 PROGRAMMATICALLY BY SOFTWARE
                            }



                            barcodeReceived = true;
                        }
                    }

                    if (barcodeReceived) { this.NotifyPropertyChanged("PackInOneCarton"); this.NotifyPropertyChanged("CartonList"); barcodeReceived = false; }
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        private bool pinChangedToSlow = false;

        void serialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            try
            {
                if (this.FillingLineData.FillingLineID != GlobalVariables.FillingLine.Pail) //FillingLine.Pail: No need to read [Blank]
                {
                    if (e.EventType == SerialPinChange.CDChanged && this.OnPrinting)
                    {
                        pinChangedToSlow = !pinChangedToSlow;
                        if (pinChangedToSlow)
                        {
                            MatchingAndAddCarton(GlobalVariables.BlankBarcode);
                            this.NotifyPropertyChanged("PackInOneCarton"); this.NotifyPropertyChanged("CartonList");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        #endregion Public Thread


        //#region Handle Exception


        /// <summary>
        /// If this.Count = noItemPerCarton then Reallocation an equal amount of messageData to every subQueue. Each subQueue will have the same NoItemPerSubQueue.
        /// </summary>
        /// <returns></returns>
        public bool ReAllocation()
        {
            if (this.matchingPackList.Count >= this.matchingPackList.NoItemPerCarton)
            {
                lock (this.matchingPackList)
                {
                    for (int subQueueID = 0; subQueueID < this.matchingPackList.SubQueueCount; subQueueID++)
                    {
                        while (this.matchingPackList.GetSubQueueCount(subQueueID) < (this.matchingPackList.NoItemPerCarton / this.matchingPackList.SubQueueCount) && this.matchingPackList.Count >= this.matchingPackList.NoItemPerCarton)
                        {
                            #region Get a message and swap its' QueueID

                            for (int i = 0; i < this.matchingPackList.SubQueueCount; i++)
                            {
                                if (this.matchingPackList.GetSubQueueCount(i) > (this.matchingPackList.NoItemPerCarton / this.matchingPackList.SubQueueCount))
                                {
                                    OnlinePackDTO messageData = this.matchingPackList.ElementAt(i, this.matchingPackList.GetSubQueueCount(i) - 1).ShallowClone(); //Get the last pack of SubQueue(i)
                                    messageData.QueueID = subQueueID; //Set new QueueID

                                    lock (this.onlinePackService)
                                    {
                                        if (this.onlinePackService.onlinePackService.UpdateListOfPackSubQueueID(messageData.PackID.ToString(), messageData.QueueID))
                                        {
                                            this.matchingPackList.Dequeue(messageData.PackID); //First: Remove from old subQueue
                                            this.matchingPackList.AddPack(messageData);//Next: Add to new subQueue
                                        }
                                        else throw new System.ArgumentException("Fail to handle this pack", "Can not update new pack subqueue: " + messageData.Code);
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }

                this.NotifyPropertyChanged("MatchingPackList");
                return true;
            }
            else return false;
        }






        public bool RemoveItemInMatchingPackList(int packID)
        {
            if (packID <= 0) return false;

            lock (this.matchingPackList)
            {
                if (this.matchingPackList.Count > 0)
                {
                    OnlinePackDTO messageData = this.matchingPackList.Dequeue(packID);
                    if (messageData != null)
                    {
                        this.NotifyPropertyChanged("MatchingPackList");

                        lock (this.onlinePackService)
                        {
                            this.onlinePackService.DeleteConfirmed(messageData.PackID); return true; //if (!this.onlinePackService.DeleteConfirmed(messageData.PackID)) return true; //Delete successfully
                            //else throw new System.ArgumentException("Fail to handle this pack", "Can not delete pack from the line");
                        }
                    }
                    else throw new System.ArgumentException("Fail to handle this pack", "Can not remove pack from the line");
                }
                else throw new System.ArgumentException("Fail to handle this pack", "No pack found on the line");
            }
        }


        public bool RemoveItemInPackInOneCarton(int packID)
        {
            if (packID <= 0) return false;

            lock (this.matchingPackList)
            {
                lock (this.packInOneCarton)
                {
                    if (this.matchingPackList.Count > 0 && this.packInOneCarton.Count == this.packInOneCarton.NoItemPerCarton)
                    {
                        OnlinePackDTO messageData = this.matchingPackList.ElementAt(0).ShallowClone(); //Get the first pack

                        if (this.packInOneCarton.Replace(packID, messageData)) //messageData.QueueID: Will change to new value (new position) after replace
                        {
                            this.matchingPackList.Dequeue(messageData.PackID); //Dequeue the first pack

                            this.NotifyPropertyChanged("PackInOneCarton");
                            this.NotifyPropertyChanged("MatchingPackList");

                            lock (this.onlinePalletService)
                            {
                                this.onlinePalletService.Delete(packID);//if ( !) throw new System.ArgumentException("Fail to handle this pack", "Can not delete pack from the line");

                                if (!this.onlinePackService.onlinePackService.UpdateListOfPackSubQueueID(messageData.PackID.ToString(), messageData.QueueID)) throw new System.ArgumentException("Fail to handle this pack", "Can not update new pack subqueue");

                                if (!this.onlinePackService.onlinePackService.UpdateEntryStatus(messageData.PackID.ToString(), GlobalVariables.BarcodeStatus.ReadyToCarton)) return true;
                                else throw new System.ArgumentException("Fail to handle this pack", "Can not update new pack status");
                            }
                        }
                        else throw new System.ArgumentException("Fail to handle this pack", "Can not replace pack");
                    }
                    else throw new System.ArgumentException("Fail to handle this pack", "No pack found on the matching line");
                }
            }
        }


        public Boolean UndoCartonToPack(int cartonID)
        {
            if (cartonID <= 0) return false;

            lock (this.packInOneCarton)
            {
                if (this.packInOneCarton.Count <= 0)
                {
                    lock (this.cartonDataTable)
                    {
                        return true;

                        //CAN PHAI XEM XET LAI DOAN CODE NAY, CODE LAI CHO THICH HOP
                        //CO THE LAM NHU SAU
                        //1) XEM LAI SAVE CARTON: SAU KHI SAVE -> NGOAI VIEC UPDATE OnlineCartonID TO OnlinePacks => can phai update them EntryStatus to 3: carton already
                        //2) tai cho nay: hien tai da lock(packInOneCarton) => sau do: can kiem tra xem o table OnlinePack: xem co phai con dung 1 pack list where Status = 2 hay kg? (cung san pham, so luong pack: bang chinh xac so luong Pack per caton: cua chuyen hien tai), neu ok: thi cho phep xoa carton
                        //3) sau khi xoa carton: thi add pack to packInOneCarton: nen: lam cung 1 cach giong nhu load packInOneCarton tai thoi diem mo phan mem
                        //TOM LAI: tai day, chung ta quan tam den viec: dam bao du lieu sach se, thong nhat voi cai gi hien thi tren giao dien (hien tai, BP: du lieu sau khi tat phan mem mo lai khong con chinh xac nua
                        //DataDetail.DataDetailCartonRow dataDetailCartonRow = this.cartonDataTable.FindByCartonID(cartonID);
                        //if (dataDetailCartonRow != null && dataDetailCartonRow.CartonStatus == (byte)GlobalVariables.BarcodeStatus.BlankBarcode)
                        //{
                        //    //COPY Data FROM cartonDataTable TO packInOneCarton
                        //    for (int i = 0; i < this.packInOneCarton.NoItemPerCarton; i++)
                        //    {                               //This use NON TYPED DATASET  -- Not so good, bescause the compiler can not detect error at compile time, but it is ok (I know exactly what in every row)
                        //        OnlinePackDTO messageData = this.AddDataDetailPack(dataDetailCartonRow["Pack" + i.ToString("00") + "Barcode"].ToString(), packInOneCarton.NextQueueID);
                        //        if (messageData != null) this.packInOneCarton.Enqueue(messageData);
                        //    }

                        //    if (this.packInOneCarton.Count > 0) UpdateDataDetailPack(GlobalVariables.BarcodeStatus.ReadyToCarton);

                        //    this.NotifyPropertyChanged("PackInOneCarton");

                        //    lock (this.CartonTableAdapter)
                        //    {
                        //        //REMOVE data IN cartonDataTable
                        //        int rowsAffected = this.CartonTableAdapter.Delete(dataDetailCartonRow.CartonID);//Delete
                        //        if (rowsAffected == 1) this.cartonDataTable.Rows.Remove(dataDetailCartonRow); else throw new System.ArgumentException("Fail to handle this carton", "Insufficient remove carton");
                        //    }

                        //    this.NotifyPropertyChanged("CartonList");

                        //    return true;
                        //}
                        //else return false;
                    }
                }
                else throw new System.ArgumentException("Fail to handle this carton", "Another carton is on the line");
            }
        }


        public Boolean UpdateCartonBarcode(int cartonID, string cartonBarcode)
        {
            if (cartonID <= 0 || cartonBarcode.Length < 12) throw new Exception("Invalid barcode: It is required 12 letters barcode [" + cartonBarcode + "]");

            lock (this.cartonDataTable)
            {
                return true;
                // can phai xem lai doan code sau;
                //DataDetail.DataDetailCartonRow dataDetailCartonRow = this.cartonDataTable.FindByCartonID(cartonID);
                //if (dataDetailCartonRow != null && dataDetailCartonRow.CartonStatus == (byte)GlobalVariables.BarcodeStatus.BlankBarcode)
                //{
                //    lock (this.CartonTableAdapter)
                //    {
                //        int rowsAffected = this.CartonTableAdapter.UpdateCartonBarcode((byte)GlobalVariables.BarcodeStatus.Normal, cartonBarcode, dataDetailCartonRow.CartonID);
                //        if (rowsAffected == 1)
                //        {
                //            dataDetailCartonRow.CartonStatus = (byte)GlobalVariables.BarcodeStatus.Normal;
                //            dataDetailCartonRow.CartonBarcode = cartonBarcode;
                //        }
                //        else throw new System.ArgumentException("Fail to handle this carton", "Insufficient update carton");
                //    }

                //    this.NotifyPropertyChanged("CartonList");

                //    return true;
                //}
                //else return false;
            }

        }

        //#endregion Handle Exception












        //#region NmvnBackup



        //private BackupLogEventTableAdapter backupLogEventTableAdapter;
        //protected BackupLogEventTableAdapter BackupLogEventTableAdapter
        //{
        //    get
        //    {
        //        if (backupLogEventTableAdapter == null) backupLogEventTableAdapter = new BackupLogEventTableAdapter();
        //        return backupLogEventTableAdapter;
        //    }
        //}


        //public void NmvnBackupDataMaster()
        //{
        //    int backupID = 0;
        //    try
        //    {
        //        string backupLocation = "C:\\BPFillingSystem\\Database\\BACKUP";

        //        DataDetailCartonTableAdapter dataDetailCartonTableAdapter = new DataDetailCartonTableAdapter();
        //        DateTime? minOfCartonDate = (DateTime?)dataDetailCartonTableAdapter.GetMinOfCartonDate();
        //        DateTime? maxOfCartonDate = (DateTime?)dataDetailCartonTableAdapter.GetMaxOfCartonDate();

        //        int noBackupDate = 45 + 1;

        //        if (minOfCartonDate != null && maxOfCartonDate != null && ((DateTime)minOfCartonDate).AddDays(noBackupDate).Date.AddSeconds(-1) < maxOfCartonDate)
        //        { //NEED TO BACKUP
        //            //

        //            //==OKKK== ADODatabase.ExecuteNonQuery("EXEC BackupNmvnDatabases 'BPFillingSystem' , 'D:\\VC PROJECTS\\BP\DATA\\bak'")
        //            //==OKKK== EXEC RestoreNmvnDatabases 'BPFillingSystem_ok', 'D:\\VC PROJECTS\\BP\\DATA\\BakBPFillingSystem_FULL_04192017_202140.BAK' , 'D:\\VC PROJECTS\\BP\DATA\\bak1'

        //            DataDetail.BackupLogEventDataTable backupLogEventDataTable = new DataDetail.BackupLogEventDataTable();
        //            DataDetail.BackupLogEventRow backupLogEventRow = backupLogEventDataTable.NewBackupLogEventRow();

        //            backupLogEventDataTable.AddBackupLogEventRow(backupLogEventRow);

        //            if (this.BackupLogEventTableAdapter.Update(backupLogEventRow) == 1) { backupID = backupLogEventRow.BackupID; } else throw new Exception("Insufficient save backup log");


        //            SqlParameter[] sqlParameter = new SqlParameter[6];
        //            sqlParameter[0] = new SqlParameter("databaseName", GlobalMsADO.DatabaseName); sqlParameter[0].SqlDbType = SqlDbType.NVarChar; sqlParameter[0].Direction = ParameterDirection.Input;
        //            sqlParameter[1] = new SqlParameter("backupLocation", backupLocation); sqlParameter[1].SqlDbType = SqlDbType.NVarChar; sqlParameter[1].Direction = ParameterDirection.Input;
        //            sqlParameter[2] = new SqlParameter("restoreLocation", ""); sqlParameter[2].SqlDbType = SqlDbType.NVarChar; sqlParameter[2].Direction = ParameterDirection.Input;
        //            sqlParameter[3] = new SqlParameter("backupID", backupID); sqlParameter[3].SqlDbType = SqlDbType.Int; sqlParameter[3].Direction = ParameterDirection.Input;
        //            sqlParameter[4] = new SqlParameter("beginningCartonDate", minOfCartonDate); sqlParameter[4].SqlDbType = SqlDbType.DateTime; sqlParameter[4].Direction = ParameterDirection.Input;
        //            sqlParameter[5] = new SqlParameter("endingCartonDate", ((DateTime)minOfCartonDate).AddDays(noBackupDate).Date.AddSeconds(-1)); sqlParameter[5].SqlDbType = SqlDbType.DateTime; sqlParameter[5].Direction = ParameterDirection.Input;
        //            ADODatabase.ExecuteNonQuery("BackupNmvnDatabases", CommandType.StoredProcedure, "", sqlParameter, 60 * 15);

        //            //this.BackupLogEventTableAdapter.BackupNmvnDatabases(GlobalMsADO.DatabaseName, backupLocation, restoreLocation, backupID, minOfCartonDate, ((DateTime)minOfCartonDate).AddDays(10));

        //            //this.NmvnBackupLogEventChange = false;
        //            //this.NmvnBackupLogEventChange = true; //NO NEED TO RAISE EVENT
        //        }

        //    }
        //    catch (System.Exception exception)
        //    {
        //        ADODatabase.ExecuteNonQuery("UPDATE BackupLogEvent SET Description = CASE WHEN Description IS NULL THEN '' ELSE Description END + N'" + exception.Message.Substring(0, exception.Message.Length > 50 ? 50 : exception.Message.Length) + "' WHERE BackupID = " + backupID);
        //    }
        //}



        //#endregion NmvnBackup






    }

    public class BarcodeScannerStatus
    {
        public GlobalVariables.BarcodeScannerName BarcodeScannerNameID { get; set; }
        public bool MCUReady { get; set; }
        public string MCUStatus { get; set; }

        public BarcodeScannerStatus(GlobalVariables.BarcodeScannerName barcodeScannerNameID)
        {
            this.BarcodeScannerNameID = barcodeScannerNameID;
            this.MCUReady = true;
            this.MCUStatus = "";
        }
    }
}
