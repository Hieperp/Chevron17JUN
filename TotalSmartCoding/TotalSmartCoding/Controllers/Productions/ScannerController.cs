﻿using Ninject;

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
using System.ComponentModel;

namespace TotalSmartCoding.Controllers.Productions
{
    public class ScannerController : CodingController //this is CommonThreadProperty
    {
        private GlobalVariables.BarcodeScannerName barcodeScannerName;
        private IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        private int portNumber = 2112;

        private TcpClient barcodeTcpClient;
        private NetworkStream barcodeNetworkStream;






        private BarcodeQueue<FillingPackDTO> packQueue;
        private BarcodeQueue<FillingPackDTO> packsetQueue;

        private BarcodeQueue<FillingCartonDTO> cartonQueue;
        private BarcodeQueue<FillingCartonDTO> cartonsetQueue;

        private BarcodeQueue<FillingPalletDTO> palletQueue;








        private bool onScanning;
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

        private FillingPackController fillingPackController;
        private FillingCartonController fillingCartonController;
        private FillingPalletController fillingPalletController;

        public ScannerController(FillingData fillingData)
        {
            try
            {
                base.FillingData = fillingData;




                this.barcodeScannerName = GlobalVariables.BarcodeScannerName.MatchingScanner;

                this.ipAddress = IPAddress.Parse(GlobalVariables.IpAddress(this.BarcodeScannerName));



                this.barcodeScannerStatus = new BarcodeScannerStatus[3];
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.QualityScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.QualityScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.MatchingScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.MatchingScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.BarcodeScannerName.CartonScanner - 1] = new BarcodeScannerStatus(GlobalVariables.BarcodeScannerName.CartonScanner);














                this.fillingPackController = new FillingPackController(CommonNinject.Kernel.Get<IFillingPackService>(), CommonNinject.Kernel.Get<IFillingPackViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<FillingPackViewModel>());
                this.fillingCartonController = new FillingCartonController(CommonNinject.Kernel.Get<IFillingCartonService>(), CommonNinject.Kernel.Get<IFillingCartonViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<FillingCartonViewModel>());
                this.fillingPalletController = new FillingPalletController(CommonNinject.Kernel.Get<IFillingPalletService>(), CommonNinject.Kernel.Get<IFillingPalletViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<FillingPalletViewModel>());


                this.packQueue = new BarcodeQueue<FillingPackDTO>(this.FillingData.NoSubQueue, this.FillingData.ItemPerSubQueue, this.FillingData.RepeatSubQueueIndex) { ItemPerSet = this.FillingData.PackPerCarton };
                this.packsetQueue = new BarcodeQueue<FillingPackDTO>(this.FillingData.NoSubQueue, this.FillingData.ItemPerSubQueue, false) { ItemPerSet = this.FillingData.PackPerCarton };

                this.cartonQueue = new BarcodeQueue<FillingCartonDTO>() { ItemPerSet = this.FillingData.CartonPerPallet };
                this.cartonsetQueue = new BarcodeQueue<FillingCartonDTO>() { ItemPerSet = this.FillingData.CartonPerPallet };

                this.palletQueue = new BarcodeQueue<FillingPalletDTO>();


                base.FillingData.PropertyChanged += new PropertyChangedEventHandler(fillingData_PropertyChanged);
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
                //////////////Initialize MatchingPackList and PackInOneCarton (of the this.FillingData.FillingLineID ONLY)
                //////this.PackDataTable = this.PackTableAdapter.GetData();
                //////foreach (DataDetail.DataDetailPackRow packRow in this.PackDataTable)
                //////{
                //////    if (packRow.FillingLineID == (int)this.FillingData.FillingLineID)
                //////    {
                //////        FillingPackDTO messageData = new FillingPackDTO(packRow.PackBarcode);
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



        private void fillingData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "PackPerCarton") { packQueue.ItemPerSet = this.FillingData.PackPerCarton; packsetQueue.ItemPerSet = this.FillingData.PackPerCarton; }
                if (e.PropertyName == "CartonPerPallet") { cartonQueue.ItemPerSet = this.FillingData.CartonPerPallet; cartonsetQueue.ItemPerSet = this.FillingData.CartonPerPallet; }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


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




        public bool OnScanning
        {
            get { return this.onScanning; }
            private set { this.onScanning = value; this.resetMessage = true; }
        }


        public void StartScanner() { this.lastStringBarcode = ""; GlobalVariables.DuplicateCartonBarcodeFound = false; this.bufferReset = true; this.OnScanning = true; } //ONLY bufferReset WHEN StartPrint
        public void StopScanner() { this.OnScanning = false; }






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


        public int MatchingPackCount { get { return this.packQueue.Count; } }
        public int PackInOneCartonCount { get { return this.packsetQueue.Count; } }

        #endregion Public Properties

        #region Public Method

        public DataTable GetMatchingPackList()
        {
            if (this.packQueue != null)
            {
                lock (this.packQueue)
                {
                    return this.packQueue.GetAllElements();
                }
            }
            else return null;
        }

        public DataTable GetPackInOneCarton()
        {
            if (this.packsetQueue != null)
            {
                lock (this.packsetQueue)
                {
                    return this.packsetQueue.GetAllElements();
                }
            }
            else return null;
        }

        public DataTable GetCartonList()
        {
            if (this.cartonQueue != null)
            {
                lock (this.cartonQueue)
                {
                    return this.cartonQueue.GetAllElements();
                }
            }
            else return null;
        }

        public DataTable GetFillingPalletQueue()
        {
            if (this.palletQueue != null)
            {
                lock (this.palletQueue)
                {
                    return this.palletQueue.GetAllElements();
                }
            }
            else return null;
        }

        private bool Connect(bool connectMatchingScanner)
        {
            try
            {
                if (GlobalEnums.OnTestOnly)
                    this.StartScanner();
                else
                {
                    this.MainStatus = "Try to connect....";

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

        private bool WaitforPack(ref string stringReadFrom)
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


        private bool WaitforCarton(ref string stringReadFrom)
        {
            try
            {
                if (GlobalEnums.OnTestOnly)
                    if ((DateTime.Now.Second % 6) == 0 && this.packsetQueue.Count > 0) stringReadFrom = "22677531 087 030117 443" + DateTime.Now.Millisecond.ToString("000000") + " 000003"; else stringReadFrom = "";
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



        private bool WaitforPallet(ref string stringReadFrom)
        {
            try
            {
                if (GlobalEnums.OnTestOnly)
                    if ((DateTime.Now.Second % 6) == 0 && this.cartonsetQueue.Count > 0) stringReadFrom = "22677531 087 030117 443" + DateTime.Now.Millisecond.ToString("000000") + " 000003"; else stringReadFrom = "";
                else
                {
                    
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


        private FillingPackDTO AddDataDetailPack(string code, int queueID)
        {
            try
            {
                FillingPackDTO fillingPackDTO = new FillingPackDTO() { FillingLineID = (int)this.FillingData.FillingLineID, CommodityID = this.FillingData.CommodityID, PCID = "ABCD123456EF", EntryStatusID = (int)GlobalVariables.BarcodeStatus.Normal, QueueID = queueID, Code = code };

                if (this.fillingPackController.fillingPackService.Save(fillingPackDTO))
                    return fillingPackDTO;
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
            if (GlobalVariables.IgnoreEmptyCarton && this.packsetQueue.Count <= 0) return;

            lock (this.cartonQueue)
            {
                lock (this.packsetQueue)
                {
                    //CẦN CHÚ Ý: this.packInOneCarton.Count = 0: CARTON RỔNG => PHẢI XỬ LÝ NHƯ THẾ NÀO???

                    FillingCartonDTO fillingCartonDTO = new FillingCartonDTO() { FillingLineID = (int)this.FillingData.FillingLineID, CommodityID = this.FillingData.CommodityID, PCID = "ABCD123456EF", EntryStatusID = (int)GlobalVariables.BarcodeStatus.Normal, Code = cartonCode };

                    this.fillingCartonController.fillingCartonService.ServiceBag["FillingPackIDs"] = this.packsetQueue.EntityIDs; //VERY IMPORTANT: NEED TO ADD FillingPackIDs TO NEW FillingCartonDTO
                    if (this.fillingCartonController.fillingCartonService.Save(fillingCartonDTO))
                        this.packsetQueue = new BarcodeQueue<FillingPackDTO>(); //CLEAR AFTER ADD TO FillingCartonDTO
                    else
                        throw new Exception("Insufficient save carton: " + fillingCartonDTO.Code);


                    this.cartonQueue.Enqueue(fillingCartonDTO);
                }
            }
        }




        #endregion Public Method


        #region Public Thread

        public void ThreadRoutine()
        {
            string stringReadFrom = ""; bool packQueueChanged = false; bool packsetQueueChanged = false; bool cartonQueueChanged = false; bool cartonsetQueueChanged = false;

            this.LoopRoutine = true; this.StopScanner();


            try
            {

                if (!this.Connect(this.FillingData.FillingLineID != GlobalVariables.FillingLine.Pail)) throw new System.InvalidOperationException("NMVN: Can not connect to comport.");

                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    this.MainStatus = this.OnScanning ? "Scanning..." : "Ready to scan";


                    if (this.OnScanning && this.WaitforPack(ref stringReadFrom))
                        packQueueChanged = this.ReceivePack(stringReadFrom) || packQueueChanged;

                    packsetQueueChanged = this.MakePackset(); packQueueChanged = packQueueChanged || packsetQueueChanged;


                    if (this.OnScanning && this.WaitforCarton(ref stringReadFrom))
                    {
                        cartonQueueChanged = this.ReceiveCarton(stringReadFrom) || cartonQueueChanged;
                        packsetQueueChanged = packsetQueueChanged || cartonQueueChanged;
                    }
                    cartonsetQueueChanged = this.MakeCartonset(); cartonQueueChanged = cartonQueueChanged || cartonsetQueueChanged;





                    if (packQueueChanged) { this.NotifyPropertyChanged("MatchingPackList"); packQueueChanged = false; }
                    if (packsetQueueChanged) { this.NotifyPropertyChanged("PackInOneCarton"); packsetQueueChanged = false; }
                    if (cartonQueueChanged) { this.NotifyPropertyChanged("CartonList"); packQueueChanged = false; }


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


        private bool ReceivePack(string stringReadFrom)
        {
            bool barcodeReceived = false;
            string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

            foreach (string stringBarcode in arrayBarcode)
            {
                string receivedBarcode = stringBarcode.Replace("NoRead", "");

                if (receivedBarcode.Trim() != "" && receivedBarcode.Trim() != "NoRead") //Add to MatchingPackList
                {
                    lock (this.packQueue)
                    {
                        FillingPackDTO messageData = this.AddDataDetailPack(receivedBarcode, this.packQueue.NextQueueID);
                        if (messageData != null)
                        {
                            this.packQueue.Enqueue(messageData);
                            barcodeReceived = true;
                        }
                    }
                }
            }

            return barcodeReceived;
        }

        private bool MakePackset()
        {
            bool packsetSuccessfully = false;
            if (this.OnScanning)
            {
                lock (this.packsetQueue)
                {
                    if (this.packsetQueue.Count <= 0)
                    {
                        lock (this.packQueue)
                        {
                            this.packsetQueue = this.packQueue.Dequeueset();
                        }

                        if (this.packsetQueue.Count > 0)
                        {
                            packsetSuccessfully = true;
                            if (!this.fillingPackController.fillingPackService.UpdateEntryStatus(this.packsetQueue.EntityIDs, GlobalVariables.BarcodeStatus.ReadyToCarton)) this.MainStatus = "Insufficient update pack status: " + this.fillingPackController.fillingPackService.ServiceTag;
                        }
                    }
                }
            }
            return packsetSuccessfully;
        }

        private bool ReceiveCarton(string stringReadFrom)
        {
            bool barcodeReceived = false;
            string[] arrayBarcode = stringReadFrom.Split(new Char[] { GlobalVariables.charETX });

            foreach (string stringBarcode in arrayBarcode)
            {
                //stringBarcode = stringBarcode.Replace("NoRead", ""); //FOR CARTON: NoRead: MEANS: CAN NOT READ => SHOULD HANDLE LATER FOR NoRead. SHOULD NOT IGNORE NoRead
                if (stringBarcode.Trim() != "" && stringBarcode.Trim() != "NoRead")
                {
                    this.MatchingAndAddCarton(stringBarcode);
                    barcodeReceived = true;
                }
            }
            return barcodeReceived;
        }

        private bool MakeCartonset()
        {
            bool cartonsetSuccessfully = false;
            if (this.OnScanning)
            {
                lock (this.cartonsetQueue)
                {
                    if (this.cartonsetQueue.Count <= 0)
                    {
                        lock (this.cartonQueue)
                        {
                            this.cartonsetQueue = this.cartonQueue.Dequeueset();
                        }

                        if (this.cartonsetQueue.Count > 0)
                        {
                            cartonsetSuccessfully = true;
                            if (!this.fillingCartonController.fillingCartonService.UpdateEntryStatus(this.cartonsetQueue.EntityIDs, GlobalVariables.BarcodeStatus.ReadyToCarton)) this.MainStatus = "Insufficient update carton status: " + this.fillingCartonController.fillingCartonService.ServiceTag;
                        }
                    }
                }
            }
            return cartonsetSuccessfully;
        }


        #endregion Public Thread


        //#region Handle Exception


        /// <summary>
        /// If this.Count = noItemPerCarton then Reallocation an equal amount of messageData to every subQueue. Each subQueue will have the same NoItemPerSubQueue.
        /// </summary>
        /// <returns></returns>
        public bool ReAllocation()
        {
            if (this.packQueue.Count >= this.packQueue.ItemPerSet)
            {
                lock (this.packQueue)
                {
                    for (int subQueueID = 0; subQueueID < this.packQueue.NoSubQueue; subQueueID++)
                    {
                        while (this.packQueue.GetSubQueueCount(subQueueID) < (this.packQueue.ItemPerSet / this.packQueue.NoSubQueue) && this.packQueue.Count >= this.packQueue.ItemPerSet)
                        {
                            #region Get a message and swap its' QueueID

                            for (int i = 0; i < this.packQueue.NoSubQueue; i++)
                            {
                                if (this.packQueue.GetSubQueueCount(i) > (this.packQueue.ItemPerSet / this.packQueue.NoSubQueue))
                                {
                                    FillingPackDTO messageData = this.packQueue.ElementAt(i, this.packQueue.GetSubQueueCount(i) - 1).ShallowClone(); //Get the last pack of SubQueue(i)
                                    messageData.QueueID = subQueueID; //Set new QueueID

                                    lock (this.fillingPackController)
                                    {
                                        if (this.fillingPackController.fillingPackService.UpdateListOfPackSubQueueID(messageData.PackID.ToString(), messageData.QueueID))
                                        {
                                            this.packQueue.Dequeue(messageData.PackID); //First: Remove from old subQueue
                                            this.packQueue.AddPack(messageData);//Next: Add to new subQueue
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

            lock (this.packQueue)
            {
                if (this.packQueue.Count > 0)
                {
                    FillingPackDTO messageData = this.packQueue.Dequeue(packID);
                    if (messageData != null)
                    {
                        this.NotifyPropertyChanged("MatchingPackList");

                        lock (this.fillingPackController)
                        {
                            this.fillingPackController.DeleteConfirmed(messageData.PackID); return true; //if (!this.fillingPackService.DeleteConfirmed(messageData.PackID)) return true; //Delete successfully
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

            lock (this.packQueue)
            {
                lock (this.packsetQueue)
                {
                    if (this.packQueue.Count > 0 && this.packsetQueue.Count == this.packsetQueue.ItemPerSet)
                    {
                        FillingPackDTO messageData = this.packQueue.ElementAt(0).ShallowClone(); //Get the first pack

                        if (this.packsetQueue.Replace(packID, messageData)) //messageData.QueueID: Will change to new value (new position) after replace
                        {
                            this.packQueue.Dequeue(messageData.PackID); //Dequeue the first pack

                            this.NotifyPropertyChanged("PackInOneCarton");
                            this.NotifyPropertyChanged("MatchingPackList");

                            lock (this.fillingPalletController)
                            {
                                this.fillingPalletController.Delete(packID);//if ( !) throw new System.ArgumentException("Fail to handle this pack", "Can not delete pack from the line");

                                if (!this.fillingPackController.fillingPackService.UpdateListOfPackSubQueueID(messageData.PackID.ToString(), messageData.QueueID)) throw new System.ArgumentException("Fail to handle this pack", "Can not update new pack subqueue");

                                if (!this.fillingPackController.fillingPackService.UpdateEntryStatus(messageData.PackID.ToString(), GlobalVariables.BarcodeStatus.ReadyToCarton)) return true;
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

            lock (this.packsetQueue)
            {
                if (this.packsetQueue.Count <= 0)
                {
                    lock (this.cartonQueue)
                    {
                        return true;

                        //CAN PHAI XEM XET LAI DOAN CODE NAY, CODE LAI CHO THICH HOP
                        //CO THE LAM NHU SAU
                        //1) XEM LAI SAVE CARTON: SAU KHI SAVE -> NGOAI VIEC UPDATE FillingCartonID TO FillingPacks => can phai update them EntryStatus to 3: carton already
                        //2) tai cho nay: hien tai da lock(packInOneCarton) => sau do: can kiem tra xem o table FillingPack: xem co phai con dung 1 pack list where Status = 2 hay kg? (cung san pham, so luong pack: bang chinh xac so luong Pack per caton: cua chuyen hien tai), neu ok: thi cho phep xoa carton
                        //3) sau khi xoa carton: thi add pack to packInOneCarton: nen: lam cung 1 cach giong nhu load packInOneCarton tai thoi diem mo phan mem
                        //TOM LAI: tai day, chung ta quan tam den viec: dam bao du lieu sach se, thong nhat voi cai gi hien thi tren giao dien (hien tai, BP: du lieu sau khi tat phan mem mo lai khong con chinh xac nua
                        //DataDetail.DataDetailCartonRow dataDetailCartonRow = this.cartonDataTable.FindByCartonID(cartonID);
                        //if (dataDetailCartonRow != null && dataDetailCartonRow.CartonStatus == (byte)GlobalVariables.BarcodeStatus.BlankBarcode)
                        //{
                        //    //COPY Data FROM cartonDataTable TO packInOneCarton
                        //    for (int i = 0; i < this.packInOneCarton.NoItemPerCarton; i++)
                        //    {                               //This use NON TYPED DATASET  -- Not so good, bescause the compiler can not detect error at compile time, but it is ok (I know exactly what in every row)
                        //        FillingPackDTO messageData = this.AddDataDetailPack(dataDetailCartonRow["Pack" + i.ToString("00") + "Barcode"].ToString(), packInOneCarton.NextQueueID);
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

            lock (this.cartonQueue)
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


        public void BackupData()
        {
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
        }



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
