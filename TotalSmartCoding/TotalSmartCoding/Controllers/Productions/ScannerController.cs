using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.ComponentModel;

using Ninject;

using TotalBase;
using TotalBase.Enums;
using TotalCore.Services.Productions;
using TotalDTO.Productions;
using TotalService.Productions;
using TotalSmartCoding.CommonLibraries;
using TotalSmartCoding.ViewModels.Productions;
using TotalSmartCoding.Builders.Productions;
using TotalSmartCoding.Controllers.Productions;


namespace TotalSmartCoding.Controllers.Productions
{
    public class ScannerController : CodingController //this is CommonThreadProperty
    {
        #region Storage
        private FillingPackController fillingPackController;
        private FillingCartonController fillingCartonController;
        private FillingPalletController fillingPalletController;


        private IONetSocket ionetSocketPack;
        private IONetSocket ionetSocketCarton;
        private IONetSocket ionetSocketPallet;


        private BarcodeQueue<FillingPackDTO> packQueue;
        private BarcodeQueue<FillingPackDTO> packsetQueue;

        private BarcodeQueue<FillingCartonDTO> cartonQueue;
        private BarcodeQueue<FillingCartonDTO> localcartonsetQueue;
        private BarcodeQueue<FillingCartonDTO> cartonsetQueue
        {
            get { return this.localcartonsetQueue; }
            set { this.localcartonsetQueue = value; this.FillingData.CartonsetQueueCount = this.localcartonsetQueue.Count; this.FillingData.CartonsetQueueZebraStatus = GlobalVariables.ZebraStatus.Freshnew; }
        }

        private BarcodeQueue<FillingPalletDTO> palletQueue;


        private BarcodeScannerStatus[] barcodeScannerStatus;

        #endregion Storage


        #region Contructor

        public ScannerController(FillingData fillingData)
        {
            try
            {
                base.FillingData = fillingData;


                this.fillingPackController = new FillingPackController(CommonNinject.Kernel.Get<IFillingPackService>(), CommonNinject.Kernel.Get<IFillingPackViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<FillingPackViewModel>());
                this.fillingCartonController = new FillingCartonController(CommonNinject.Kernel.Get<IFillingCartonService>(), CommonNinject.Kernel.Get<IFillingCartonViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<FillingCartonViewModel>());
                this.fillingPalletController = new FillingPalletController(CommonNinject.Kernel.Get<IFillingPalletService>(), CommonNinject.Kernel.Get<IFillingPalletViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<FillingPalletViewModel>());


                this.ionetSocketPack = new IONetSocket(IPAddress.Parse(GlobalVariables.IpAddress(GlobalVariables.ScannerName.PackScanner)), 23, 120); //PORT 2112: DATA LOGIC
                this.ionetSocketCarton = new IONetSocket(IPAddress.Parse(GlobalVariables.IpAddress(GlobalVariables.ScannerName.CartonScanner)), 2112, 120);
                this.ionetSocketPallet = new IONetSocket(IPAddress.Parse(GlobalVariables.IpAddress(GlobalVariables.ScannerName.PalletScanner)), 2112, 120);


                this.packQueue = new BarcodeQueue<FillingPackDTO>(this.FillingData.NoSubQueue, this.FillingData.ItemPerSubQueue, this.FillingData.RepeatSubQueueIndex) { ItemPerSet = this.FillingData.PackPerCarton };
                this.packsetQueue = new BarcodeQueue<FillingPackDTO>(this.FillingData.NoSubQueue, this.FillingData.ItemPerSubQueue, false) { ItemPerSet = this.FillingData.PackPerCarton };

                this.cartonQueue = new BarcodeQueue<FillingCartonDTO>() { ItemPerSet = this.FillingData.CartonPerPallet };
                this.cartonsetQueue = new BarcodeQueue<FillingCartonDTO>() { ItemPerSet = this.FillingData.CartonPerPallet };

                this.palletQueue = new BarcodeQueue<FillingPalletDTO>();


                base.FillingData.PropertyChanged += new PropertyChangedEventHandler(fillingData_PropertyChanged);

                this.barcodeScannerStatus = new BarcodeScannerStatus[3];
                this.barcodeScannerStatus[(int)GlobalVariables.ScannerName.QualityScanner - 1] = new BarcodeScannerStatus(GlobalVariables.ScannerName.QualityScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.ScannerName.PackScanner - 1] = new BarcodeScannerStatus(GlobalVariables.ScannerName.PackScanner);
                this.barcodeScannerStatus[(int)GlobalVariables.ScannerName.CartonScanner - 1] = new BarcodeScannerStatus(GlobalVariables.ScannerName.CartonScanner);

                this.Initialize();
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

                //////this.NotifyPropertyChanged("PackQueue");
                //////this.NotifyPropertyChanged("PacksetQueue");
                ////////////--------------
                //HIEP*******************************22-MAY-2017.BEGIN

                ////////Initialize CartonList
                //////this.cartonDataTable = this.CartonTableAdapter.GetDataByCartonStatus((byte)GlobalVariables.BarcodeStatus.BlankBarcode);
                //////this.NotifyPropertyChanged("CartonQueue");

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

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

        #endregion Contructor


        #region Public Properties

        public bool OnScanning { get; private set; }

        public void StartScanner() { this.OnScanning = true; }
        public void StopScanner() { this.OnScanning = false; }

        public int PackQueueCount { get { return this.packQueue.Count; } }
        public int CartonQueueCount { get { return this.cartonQueue.Count; } }
        public int PalletQueueCount { get { return this.palletQueue.Count; } }

        #endregion Public Properties


        #region LedMCU

        public bool LedMCUQualityOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.ScannerName.QualityScanner - 1].MCUReady; }
        }

        public bool LedMCUMatchingOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.ScannerName.PackScanner - 1].MCUReady; }
        }

        public bool LedMCUCartonOn
        {
            get { return !this.barcodeScannerStatus[(int)GlobalVariables.ScannerName.CartonScanner - 1].MCUReady; }
        }

        public void SetBarcodeScannerStatus(GlobalVariables.ScannerName barcodeScannerNameID, bool mcuReady)
        {
            if (this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUReady != mcuReady)
            {
                this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUReady = mcuReady;
                this.NotifyPropertyChanged("LedMCU");
            }
        }

        public void SetBarcodeScannerStatus(GlobalVariables.ScannerName barcodeScannerNameID, string mcuStatus)
        {
            if (this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUStatus != mcuStatus)
            {
                this.barcodeScannerStatus[(int)barcodeScannerNameID - 1].MCUStatus = mcuStatus;
                this.MainStatus = barcodeScannerNameID.ToString() + ": " + mcuStatus;
            }

            this.SetBarcodeScannerStatus(barcodeScannerNameID, false);
        }
        #endregion

        #region Public Method

        public DataTable GetPackQueue()
        {
            if (this.packQueue != null)
            {
                lock (this.packQueue)
                {
                    return this.packQueue.ConverttoTable();
                }
            }
            else return null;
        }

        public DataTable GetPacksetQueue()
        {
            if (this.packsetQueue != null)
            {
                lock (this.packsetQueue)
                {
                    return this.packsetQueue.ConverttoTable();
                }
            }
            else return null;
        }

        public DataTable GetCartonQueue()
        {
            if (this.cartonQueue != null)
            {
                lock (this.cartonQueue)
                {
                    return this.cartonQueue.ConverttoTable();
                }
            }
            else return null;
        }

        public DataTable GetCartonsetQueue()
        {
            if (this.cartonsetQueue != null)
            {
                lock (this.cartonsetQueue)
                {
                    return this.cartonsetQueue.ConverttoTable();
                }
            }
            else return null;
        }

        public DataTable GetPalletQueue()
        {
            if (this.palletQueue != null)
            {
                lock (this.palletQueue)
                {
                    return this.palletQueue.ConverttoTable();
                }
            }
            else return null;
        }


        private bool Connect()
        {
            try
            {
                if (GlobalEnums.OnTestScanner)
                {
                    //this.StartScanner();
                }
                else
                {
                    this.MainStatus = "Đang kết nối máy đọc mã vạch ...";

                    if (this.FillingData.HasPack)
                        this.ionetSocketPack.Connect();

                    if (this.FillingData.HasCarton)
                        this.ionetSocketCarton.Connect();

                    if (this.FillingData.HasPallet && !GlobalEnums.OnTestPalletScanner)
                        this.ionetSocketPallet.Connect();
                }

                this.setLED(true, false, false);

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
                this.MainStatus = "Đã ngắt kết nối ...";

                this.ionetSocketPack.Disconnect();
                this.ionetSocketCarton.Disconnect();
                this.ionetSocketPallet.Disconnect();

                this.setLED();

                return true;
            }

            catch (Exception exception)
            {
                this.MainStatus = exception.Message; return false;
            }

        }

        #endregion Public Method


        #region Public Thread

        /// <summary>
        /// TEST: 
        /// 1) CARTON: NO READ
        /// 2) PALLET: MORE THAN 1 PALLET IS IN THE QUEUE. 
        /// HOW THE ZEBRA WORK? 
        /// WHAT'S HAPPEND WHEN 1 BARCODE PRINTED BY ZEBRA IS READ MORE THAN 1 TIMES (DUPLICATE READ)?
        /// HOW TO RE-PRINNT?
        /// HOW TO PROCESS DUPLICATE CODE IN THE WHOLE TABLE (BY READ 1 CODE MORE THAN 1 TIMES: PACK/ CARTON/ PALLET)
        /// </summary>
        public void ThreadRoutine()
        {
            string stringReceived = ""; bool packQueueChanged = false; bool packsetQueueChanged = false; bool cartonQueueChanged = false; bool cartonsetQueueChanged = false; bool palletQueueChanged = false;

            this.LoopRoutine = true; this.StopScanner();


            try
            {
                if (!this.Connect()) throw new System.InvalidOperationException("Lỗi kết nối đầu đọc mã vạch");

                while (this.LoopRoutine)    //MAIN LOOP. STOP WHEN PRESS DISCONNECT
                {
                    this.MainStatus = this.OnScanning ? "Đang đọc mã vạch ..." : "Sẳn sàng đọc mã vạch";

                    if (this.OnScanning && this.FillingData.HasPack)
                    {
                        if (this.waitforPack(ref stringReceived))
                            packQueueChanged = this.ReceivePack(stringReceived) || packQueueChanged;

                        packsetQueueChanged = this.MakePackset(); packQueueChanged = packQueueChanged || packsetQueueChanged;
                    }

                    if (this.OnScanning && this.FillingData.HasCarton)
                    {
                        if (this.waitforCarton(ref stringReceived))
                        {
                            cartonQueueChanged = this.ReceiveCarton(stringReceived) || cartonQueueChanged;
                            packsetQueueChanged = packsetQueueChanged || cartonQueueChanged;
                        }
                        cartonsetQueueChanged = this.MakeCartonset(); cartonQueueChanged = cartonQueueChanged || cartonsetQueueChanged;
                    }

                    if (this.OnScanning && this.FillingData.HasPallet)
                    {
                        if (this.waitforPallet(ref stringReceived))
                        {
                            palletQueueChanged = this.ReceivePallet(stringReceived) || palletQueueChanged;
                            cartonsetQueueChanged = cartonsetQueueChanged || palletQueueChanged;
                        }
                    }


                    if (packQueueChanged) { this.NotifyPropertyChanged("PackQueue"); packQueueChanged = false; }
                    if (packsetQueueChanged) { this.NotifyPropertyChanged("PacksetQueue"); packsetQueueChanged = false; }

                    if (cartonQueueChanged) { this.NotifyPropertyChanged("CartonQueue"); cartonQueueChanged = false; }
                    if (cartonsetQueueChanged) { this.NotifyPropertyChanged("CartonsetQueue"); cartonsetQueueChanged = false; }

                    if (palletQueueChanged) { this.NotifyPropertyChanged("PalletQueue"); palletQueueChanged = false; }


                    Thread.Sleep(100);

                } //End while this.LoopRoutine
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








        private bool waitforPack(ref string stringReceived)
        {
            if (GlobalEnums.OnTestScanner)
                if ((DateTime.Now.Second % 4) == 0) stringReceived = "22677531 087 030117 443" + DateTime.Now.Millisecond.ToString("000000") + " 000003"; else stringReceived = "";
            else
                stringReceived = this.ionetSocketPack.ReadoutStream().Trim();

            return stringReceived != "";
        }

        private bool ReceivePack(string stringReceived)
        {
            bool barcodeReceived = false;
            string[] arrayBarcode = stringReceived.Split(new Char[] { GlobalVariables.charETX });

            foreach (string stringBarcode in arrayBarcode)
            {
                string receivedBarcode = stringBarcode.Replace("NoRead", "").Trim();

                if (receivedBarcode != "")
                {
                    lock (this.packQueue)
                    {
                        FillingPackDTO messageData = this.addPack(receivedBarcode, this.packQueue.NextQueueID);
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

        private FillingPackDTO addPack(string packCode, int queueID)
        {
            try
            {
                FillingPackDTO fillingPackDTO = new FillingPackDTO(this.FillingData) { Code = packCode, QueueID = queueID };

                if (this.fillingPackController.fillingPackService.Save(fillingPackDTO))
                    return fillingPackDTO;
                else
                    throw new Exception();
            }
            catch (System.Exception exception)
            {
                throw new Exception("Lỗi lưu mã vạch chai [" + packCode + "] " + exception.Message);
            }
        }

        private bool MakePackset()
        {
            bool isSuccessfully = false;
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
                            isSuccessfully = true;
                            if (!this.fillingPackController.fillingPackService.UpdateEntryStatus(this.packsetQueue.EntityIDs, GlobalVariables.BarcodeStatus.ReadyToCarton)) this.MainStatus = "Lỗi lưu trạng thái chai: " + this.fillingPackController.fillingPackService.ServiceTag;
                        }
                    }
                }
            }
            return isSuccessfully;
        }









        private bool waitforCarton(ref string stringReceived)
        {
            if (GlobalEnums.OnTestScanner)
                if ((DateTime.Now.Second % 6) == 0 && (this.packsetQueue.Count > 0 || !this.FillingData.HasPack)) stringReceived = "22677531 087 030117 443" + DateTime.Now.Millisecond.ToString("000000") + " 000003"; else stringReceived = "";
            else
                stringReceived = this.ionetSocketCarton.ReadoutStream().Trim();

            return stringReceived != "";
        }

        /// <summary>
        /// FillingData.HasPack:        CARTON: WHEN RECEIVE NoRead => ACCEPT: IT WILL ADD NEW CARTON, WHICH CODE WILL BE 'NoRead' 
        ///                             AND THEN: => USER MUST UPDATE ACTUAL BARCODE BY MANUALL LATER/ OR: REMOVE CARTON WRAPPER => TO WRAP CARTON AGAIN (PRINT + SCAN AGAIN) FROM THE PRODUCTION LINE
        /// 
        /// NOT FillingData.HasPack:    CARTON: NO READ => IGNORE. MEANS: USER MUST TRY TO SCAN UNTIL READ SUCCESSFULLY/ OR USER MUST REMOVE THIS PAIL OUT OF PRODUCTION LINE (DELETE) (JUST ONLY SUCCESSFULLY READ => TO ADD TO CartonQueue)
        ///                             IF REPEATED READ => THE SOFTWARE JUST ACCEPT THE FIRST ONE ONLY (ONLY FOR: NOT FillingData.HasPack)
        /// 
        /// WHEN this.packsetQueue.Count = 0: EMPTY CARTON WILL BE IGNORE. SEE MatchingAndAddCarton FOR MORE INFORMATION
        /// </summary>
        /// <param name="stringReceived"></param>
        /// <returns></returns>
        private string lastCartonCode = "";
        private bool ReceiveCarton(string stringReceived)
        {
            bool barcodeReceived = false;
            string[] arrayBarcode = stringReceived.Split(new Char[] { GlobalVariables.charETX });

            foreach (string stringBarcode in arrayBarcode)
            {
                string receivedBarcode = stringBarcode.Trim();
                if (!this.FillingData.HasPack) receivedBarcode = receivedBarcode.Replace("NoRead", "").Trim();
                //NOTES: this.FillingData.HasPack && lastCartonCode == receivedBarcode: KHI HasPack: TRÙNG CARTON  || HOẶC LÀ "NoRead": THI CẦN PHẢI ĐƯA SANG 1 QUEUE KHÁC. XỬ LÝ CUỐI CA
                if (receivedBarcode != "" && (this.FillingData.HasPack || lastCartonCode != receivedBarcode || receivedBarcode == "NoRead"))
                    if (this.matchPacktoCarton(receivedBarcode))
                    {
                        lastCartonCode = receivedBarcode;
                        barcodeReceived = true;
                    }
            }
            return barcodeReceived;
        }

        private bool matchPacktoCarton(string cartonCode)
        {
            lock (this.cartonQueue)
            {
                lock (this.packsetQueue)
                {//BY NOW: GlobalVariables.IgnoreEmptyCarton = TRUE. LATER, WE WILL ADD AN OPTION ON MENU FOR USER, IF NEEDED
                    if (!GlobalVariables.IgnoreEmptyCarton || this.packsetQueue.Count > 0 || !this.FillingData.HasPack) //BY NOT this.FillingData.HasPack: this.packsetQueue.Count WILL ALWAYS BE: 0 (NO PACK RECEIVED)
                    {//IF this.packsetQueue.Count <= 0 => THIS WILL SAVE AN EMPTY CARTON. this.packsetQueue.EntityIDs WILL BE BLANK => NO PACK BE UPDATED FOR THIS CARTON

                        FillingCartonDTO fillingCartonDTO = new FillingCartonDTO(this.FillingData) { Code = cartonCode };

                        this.fillingCartonController.fillingCartonService.ServiceBag["FillingPackIDs"] = this.packsetQueue.EntityIDs; //VERY IMPORTANT: NEED TO ADD FillingPackIDs TO NEW FillingCartonDTO
                        if (this.fillingCartonController.fillingCartonService.Save(fillingCartonDTO))
                            this.packsetQueue = new BarcodeQueue<FillingPackDTO>(); //CLEAR AFTER ADD TO FillingCartonDTO
                        else
                            throw new Exception("Lỗi lưu mã vạch carton: " + fillingCartonDTO.Code);


                        this.cartonQueue.Enqueue(fillingCartonDTO);
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        private bool MakeCartonset()
        {
            bool isSuccessfully = false;
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
                            isSuccessfully = true;
                            if (!this.fillingCartonController.fillingCartonService.UpdateEntryStatus(this.cartonsetQueue.EntityIDs, GlobalVariables.BarcodeStatus.ReadyToCarton)) this.MainStatus = "Lỗi lưu trạng thái carton: " + this.fillingCartonController.fillingCartonService.ServiceTag;
                        }
                    }
                }
            }
            return isSuccessfully;
        }








        private bool waitforPallet(ref string stringReceived)
        {
            if (GlobalEnums.OnTestScanner || GlobalEnums.OnTestPalletScanner)
                if ((GlobalEnums.OnTestPrinter || GlobalEnums.OnTestPalletReceivedNow) && ((DateTime.Now.Second % 10) == 0 || GlobalEnums.OnTestPalletReceivedNow) && (this.cartonsetQueue.Count > 0 || !this.FillingData.HasCarton)) { stringReceived = "22677531 087 030117 443" + DateTime.Now.Millisecond.ToString("000000") + " 000003"; GlobalEnums.OnTestPalletReceivedNow = false; } else stringReceived = "";
            else
                stringReceived = this.ionetSocketPallet.ReadoutStream().Trim();

            return stringReceived != "";
        }

        /// <summary>
        /// ONLY WHEN cartonsetQueue IS FULL (MEANS: ONE PALLET IS AVAILABLE): THE SOFTWARE WILL SEND DATA TO ZEBRA FOR PRINTING ONE LABEL
        /// AND: ONLY ONE PRINT PER PALLET (MEANS: PER EACH FULL cartonsetQueue). IF NEEDED: USER MUST PRESS PRINT AGAIN => TO PRINT MANUALLY
        /// THE SOFTWARE ONLY CHECK DUPLICATE CODE FOR CURRENT PALLET WITH THE PREVIOUS RECEIVED CODE ONLY. THE UNIQUE CONSTRAINT ON TABLE WILL CHECK THE WHOLE TABLE FOR DUPLICATE CODE
        /// </summary>
        /// <param name="stringReceived"></param>
        /// <returns></returns>
        private string lastPalletCode = "";
        private bool ReceivePallet(string stringReceived) //ONLY WHEN cartonsetQueue IS FULL + ONLY ONE PRINT PER PALLET: thêm số đếm printed cho cartonsetQueue?????
        {
            bool barcodeReceived = false;
            string[] arrayBarcode = stringReceived.Split(new Char[] { GlobalVariables.charETX });

            foreach (string stringBarcode in arrayBarcode)
            {
                string receivedBarcode = stringBarcode.Replace("NoRead", "").Trim();//FOR PALLET: IGNORE FOR NoRead => USER MUST TRY TO READ AGAIN,     OR: EVENT PRESS BUTTON TO RE-PRINT MANUALLY IN ORDER TO READ AGAIN
                if (receivedBarcode != "" && lastPalletCode != receivedBarcode)
                    if (this.matchCartontoPallet(receivedBarcode))
                    {
                        lastPalletCode = receivedBarcode;
                        barcodeReceived = true;
                    }
            }
            return barcodeReceived;
        }

        private bool matchCartontoPallet(string palletCode)
        {
            lock (this.palletQueue)
            {
                lock (this.cartonsetQueue)
                {
                    if (!GlobalVariables.IgnoreEmptyPallet || ((this.cartonsetQueue.Count > 0 || !this.FillingData.HasCarton) && (this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Printed || GlobalEnums.OnTestScanner))) //BY NOW: GlobalVariables.IgnoreEmptyPallet = TRUE. LATER, WE WILL ADD AN OPTION ON MENU FOR USER, IF NEEDED.               NOTES: HERE WE CHECK this.FillingData.CartonsetQueueLabelPrintedCount != 0: TO ENSURE THAT A NEW LABEL HAS BEEN PRINTED BY PrinterController IN ORDER TO MatchingAndAddPallet
                    {//IF this.cartonsetQueue.Count <= 0 => THIS WILL SAVE AN EMPTY PALLET: this.cartonsetQueue.EntityIDs WILL BE BLANK => NO CARTON BE UPDATED FOR THIS PALLET

                        FillingPalletDTO fillingPalletDTO = new FillingPalletDTO(this.FillingData) { Code = palletCode };

                        this.fillingPalletController.fillingPalletService.ServiceBag["FillingCartonIDs"] = this.cartonsetQueue.EntityIDs; //VERY IMPORTANT: NEED TO ADD FillingCartonIDs TO NEW FillingPalletDTO
                        if (this.fillingPalletController.fillingPalletService.Save(fillingPalletDTO))
                            this.cartonsetQueue = new BarcodeQueue<FillingCartonDTO>(); //CLEAR AFTER ADD TO FillingPalletDTO
                        else
                            throw new Exception("Lỗi lưu pallet: " + fillingPalletDTO.Code);


                        this.palletQueue.Enqueue(fillingPalletDTO);
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        public void Reprint()
        {
            try
            {
                if (this.OnScanning)
                {
                    lock (this.cartonsetQueue)
                    {
                        if ((this.cartonsetQueue.Count > 0 || !this.FillingData.HasCarton) && this.FillingData.CartonsetQueueZebraStatus == GlobalVariables.ZebraStatus.Printed)
                            this.FillingData.CartonsetQueueZebraStatus = GlobalVariables.ZebraStatus.Reprint;
                    }
                }
            }
            catch (Exception exception)
            {
                this.MainStatus = exception.Message;
            }
        }

        #endregion Public Thread


        #region Handle Exception


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

                this.NotifyPropertyChanged("PackQueue");
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
                        this.NotifyPropertyChanged("PackQueue");

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

                            this.NotifyPropertyChanged("PacksetQueue");
                            this.NotifyPropertyChanged("PackQueue");

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

                        //    this.NotifyPropertyChanged("PacksetQueue");

                        //    lock (this.CartonTableAdapter)
                        //    {
                        //        //REMOVE data IN cartonDataTable
                        //        int rowsAffected = this.CartonTableAdapter.Delete(dataDetailCartonRow.CartonID);//Delete
                        //        if (rowsAffected == 1) this.cartonDataTable.Rows.Remove(dataDetailCartonRow); else throw new System.ArgumentException("Fail to handle this carton", "Insufficient remove carton");
                        //    }

                        //    this.NotifyPropertyChanged("CartonQueue");

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

                //    this.NotifyPropertyChanged("CartonQueue");

                //    return true;
                //}
                //else return false;
            }

        }

        #endregion Handle Exception

        #region NmvnBackup



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



        #endregion NmvnBackup

    }

    public class BarcodeScannerStatus
    {
        public GlobalVariables.ScannerName BarcodeScannerNameID { get; set; }
        public bool MCUReady { get; set; }
        public string MCUStatus { get; set; }

        public BarcodeScannerStatus(GlobalVariables.ScannerName barcodeScannerNameID)
        {
            this.BarcodeScannerNameID = barcodeScannerNameID;
            this.MCUReady = true;
            this.MCUStatus = "";
        }
    }
}
