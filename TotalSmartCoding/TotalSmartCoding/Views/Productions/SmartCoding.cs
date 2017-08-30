using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

using Ninject;
using AutoMapper;

using TotalBase;
using TotalBase.Enums;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;
using TotalModel.Models;
using TotalDTO.Productions;

using TotalSmartCoding.Controllers.Productions;
using TotalSmartCoding.Controllers.APIs.Productions;
using TotalSmartCoding.Libraries;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.Views.Commons;
using TotalSmartCoding.Views.Mains;


namespace TotalSmartCoding.Views.Productions
{
    public partial class SmartCoding : Form, IToolstripMerge
    {
        #region Declaration

        private readonly FillingData fillingData;


        private PrinterController digitController;
        private PrinterController packController;
        private PrinterController cartonController;
        private PrinterController palletController;

        private ScannerController scannerController;



        private Thread digitThread;
        private Thread packThread;
        private Thread cartonThread;
        private Thread palletThread;

        private Thread scannerThread;

        private Thread backupDataThread;



        delegate void SetTextCallback(string text);
        delegate void propertyChangedThread(object sender, PropertyChangedEventArgs e);

        #endregion Declaration

        #region Contructor & Implement Interface

        public SmartCoding()
        {
            InitializeComponent();

            try
            {
                this.fillingData = new FillingData();

                this.Initialize();

                IBatchService batchService = CommonNinject.Kernel.Get<IBatchService>();//ALL PrinterController MUST SHARE THE SAME IBatchService, BECAUSE WE NEED TO LOCK IBatchService IN ORDER TO CORRECTED UPDATE DATA BY IBatchService

                digitController = new PrinterController(batchService, this.fillingData, GlobalVariables.PrinterName.DigitInkjet);
                packController = new PrinterController(batchService, this.fillingData, GlobalVariables.PrinterName.PackInkjet);
                cartonController = new PrinterController(batchService, this.fillingData, GlobalVariables.PrinterName.CartonInkjet);
                palletController = new PrinterController(batchService, this.fillingData, GlobalVariables.PrinterName.PalletLabel);

                this.scannerController = new ScannerController(this.fillingData);

                digitController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);
                packController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);
                cartonController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);
                palletController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);

                scannerController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);


                this.textBoxFillingLineName.TextBox.DataBindings.Add("Text", this.fillingData, "FillingLineName");
                this.textBoxCommodityCode.TextBox.DataBindings.Add("Text", this.fillingData, "CommodityCode");
                this.textBoxCommodityOfficialCode.TextBox.DataBindings.Add("Text", this.fillingData, "CommodityOfficialCode");
                this.textBoxBatchCode.TextBox.DataBindings.Add("Text", this.fillingData, "BatchCode");
                this.textBoxNextPackNo.TextBox.DataBindings.Add("Text", this.fillingData, "NextPackNo");
                this.textBoxNextCartonNo.TextBox.DataBindings.Add("Text", this.fillingData, "NextCartonNo");
                this.textBoxNextPalletNo.TextBox.DataBindings.Add("Text", this.fillingData, "NextPalletNo");

                this.comboBoxEmptyCarton.ComboBox.Items.AddRange(new string[] { "Ignore empty carton", "Keep empty carton" });
                this.comboBoxEmptyCarton.ComboBox.SelectedIndex = GlobalVariables.IgnoreEmptyCarton ? 0 : 1;
                this.comboBoxEmptyCarton.Enabled = this.fillingData.FillingLineID != GlobalVariables.FillingLine.Pail;

                this.buttonCartonNoreadNow.Visible = GlobalEnums.OnTestScanner;
                this.buttonPalletReceivedNow.Visible = GlobalEnums.OnTestScanner;

                if (!fillingData.HasPack) { this.dgvCartonPendingQueue.RowTemplate.Height = 195; this.dgvCartonQueue.RowTemplate.Height = 195; this.dgvCartonsetQueue.RowTemplate.Height = 195; this.dgvPalletQueue.RowTemplate.Height = 195; this.dgvPalletsetQueue.RowTemplate.Height = 195; }

            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        public void Initialize()
        {
            try
            {
                BatchIndex batchIndex = (new BatchAPIs(CommonNinject.Kernel.Get<IBatchAPIRepository>())).GetActiveBatchIndex();
                if (batchIndex != null) Mapper.Map<BatchIndex, FillingData>(batchIndex, this.fillingData);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void SmartCoding_Load(object sender, EventArgs e)
        {
            try
            {
                this.scannerController.Initialize();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void splitPallet_Resize(object sender, EventArgs e)
        {
            try
            {
                this.splitContainerPack.SplitterDistance = this.SplitterDistanceQuality();
                this.splitContainerCarton.SplitterDistance = SplitterContainerCarton();
                this.splitContainerPallet.SplitterDistance = this.SplitterContainerPallet();



                this.splitDigit.SplitterDistance = this.Width / 5;
                this.splitPack.SplitterDistance = this.Width / 5;
                this.splitCarton.SplitterDistance = this.Width / 5;
                this.splitPallet.SplitterDistance = this.Width / 5;
            }
            catch { }
        }

        private void SmartCoding_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (digitThread != null && digitThread.IsAlive) { e.Cancel = true; return; }
                if (packThread != null && packThread.IsAlive) { e.Cancel = true; return; }
                if (cartonThread != null && cartonThread.IsAlive) { e.Cancel = true; return; }
                if (palletThread != null && palletThread.IsAlive) { e.Cancel = true; return; }

                if (scannerThread != null && scannerThread.IsAlive) { e.Cancel = true; return; }

                if (backupDataThread != null && backupDataThread.IsAlive) { e.Cancel = true; return; }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }



        private void comboBoxEmptyCarton_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalVariables.IgnoreEmptyCarton = this.comboBoxEmptyCarton.ComboBox.SelectedIndex == 0;
                GlobalRegistry.Write("IgnoreEmptyCarton", GlobalVariables.IgnoreEmptyCarton ? "1" : "0");
            }
            catch
            { }
        }


        public ToolStrip toolstripChild { get { return this.toolStripChildForm; } }

        private int SplitterDistanceQuality()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Smallpack:
                    return 346; //364 
                case GlobalVariables.FillingLine.Pail:
                    return 0;
                case GlobalVariables.FillingLine.Drum:
                    return 70; //86;
                default:
                    return 1;
            }
        }

        private int SplitterContainerCarton()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Smallpack:
                    return 225;
                case GlobalVariables.FillingLine.Pail:
                    return 521;
                case GlobalVariables.FillingLine.Drum:
                    return 70; //86;
                default:
                    return 1;
            }
        }

        private int SplitterContainerPallet()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Smallpack:
                    return 111;
                case GlobalVariables.FillingLine.Pail:
                    return 259;
                case GlobalVariables.FillingLine.Drum:
                    return 70; //86;
                default:
                    return 1;
            }
        }

        private int SplitterDistanceMatching()
        {
            //if (this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.CM || this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.WH || this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail)
            //{
            //    for (int i = 1; i <= 24; i++)
            //        this.dataGridViewCartonList.Columns[i].Visible = (i > (this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.CM || this.fillingLineData.FillingLineID == GlobalVariables.FillingLine.WH ? GlobalVariables.NoItemPerCarton() : 0)) ? false : true;
            //}

            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Smallpack:
                    return 955;//1199
                case GlobalVariables.FillingLine.Pail:
                    return 955;//760---24
                case GlobalVariables.FillingLine.Drum:
                    return GlobalVariables.noItemPerCartonSetByProductID == 6 ? 860 : 880;
                default:
                    return 1;
            }
        }

        private int SplitterDistanceCarton()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Smallpack:
                    return 160; //213
                case GlobalVariables.FillingLine.Pail:
                    return 160;
                case GlobalVariables.FillingLine.Drum:
                    return 361; //485
                default:
                    return 1;
            }
        }

        #endregion Contructor & Implement Interface

        #region Toolstrip bar

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.cutStatusBox(true);

                if (backupDataThread == null || !backupDataThread.IsAlive)
                {
                    if (digitThread != null && digitThread.IsAlive) digitThread.Abort();
                    digitThread = new Thread(new ThreadStart(digitController.ThreadRoutine));

                    if (packThread != null && packThread.IsAlive) packThread.Abort();
                    packThread = new Thread(new ThreadStart(packController.ThreadRoutine));

                    if (cartonThread != null && cartonThread.IsAlive) cartonThread.Abort();
                    cartonThread = new Thread(new ThreadStart(cartonController.ThreadRoutine));

                    if (palletThread != null && palletThread.IsAlive) palletThread.Abort();
                    palletThread = new Thread(new ThreadStart(palletController.ThreadRoutine));

                    if (scannerThread != null && scannerThread.IsAlive) scannerThread.Abort();
                    scannerThread = new Thread(new ThreadStart(scannerController.ThreadRoutine));


                    digitThread.Start();
                    packThread.Start();
                    cartonThread.Start();
                    palletThread.Start();
                    scannerThread.Start();
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                digitController.LoopRoutine = false;
                packController.LoopRoutine = false;
                cartonController.LoopRoutine = false;
                palletController.LoopRoutine = false;
                scannerController.LoopRoutine = false;

                this.setToolStripActive();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.cutStatusBox(true);

                this.digitController.StartPrint();
                this.packController.StartPrint();
                this.cartonController.StartPrint();
                this.palletController.StartPrint();
                this.scannerController.StartScanner();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (CustomMsgBox.Show(this, "Phần mềm đang kết nối hệ thống máy in và đầu đọc mã vạch." + (char)13 + (char)13 + "Bạn có muốn dừng phần mềm không?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    if (CustomMsgBox.Show(this, "Bạn thật sự muốn dừng phần mềm?" + (char)13 + (char)13 + "Vui lòng nhấn Yes để dừng phần mềm ngay lập tức.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.StopPrint();
                        this.scannerController.StopScanner();
                    }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void StopPrint()
        {
            this.StopPrint(true, true, true, true);
        }

        private void StopPrint(bool stopDigit, bool stopBarcode, bool stopCarton, bool stopPallet)
        {
            if (stopDigit) this.digitController.StopPrint();
            if (stopBarcode) this.packController.StopPrint();
            if (stopCarton) this.cartonController.StopPrint();
            if (stopPallet) this.palletController.StopPrint();
        }


        private void toolStripButtonSetting_Click(object sender, EventArgs e)
        {
            try
            {
                MasterMDI masterMDI = new MasterMDI(GlobalEnums.NmvnTaskID.Batch, new Batches(this, this.scannerController.AllQueueEmpty));

                masterMDI.ShowDialog();

                masterMDI.Dispose();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void buttonReprint_Click(object sender, EventArgs e)
        {
            this.scannerController.Reprint();
        }


        private void buttonPalletReceivedNow_Click(object sender, EventArgs e)
        {
            GlobalEnums.OnTestPalletReceivedNow = true;
        }


        private void buttonCartonNoreadNow_Click(object sender, EventArgs e)
        {
            GlobalEnums.OnTestCartonNoreadNow = true;
        }

        private void timerEverySecond_Tick(object sender, EventArgs e)
        {
            try
            {
                this.textBoxCurrentDate.TextBox.Text = DateTime.Now.ToString("dd/MM/yy");
                if (this.fillingData != null)
                {
                    //if (this.fillingData.SettingMonthID != 1) //GlobalStaticFunction.DateToContinuosMonth()
                    //{
                    //    this.toolStripButtonWarningNewMonth.Visible = !this.toolStripButtonWarningNewMonth.Visible; this.toolStripLabelWarningNewMonth.Visible = !this.toolStripLabelWarningNewMonth.Visible;
                    //}
                    //else
                    //{
                    //    this.toolStripButtonWarningNewMonth.Visible = false; this.toolStripLabelWarningNewMonth.Visible = false;
                    //}
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion Toolstrip bar

        #region Handler

        private void controller_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                propertyChangedThread propertyChangedDelegate = new propertyChangedThread(propertyChangedHandler);
                this.Invoke(propertyChangedDelegate, new object[] { sender, e });
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void propertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                this.setToolStripActive();

                if (sender.Equals(this.digitController))
                {
                    if (e.PropertyName == "MainStatus") { this.digitStatusbox.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.digitController.MainStatus + "\r\n" + this.digitStatusbox.Text; this.cutStatusBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.digitLEDGreen.Enabled = this.digitController.LedGreenOn; this.digitLEDAmber.Enabled = this.digitController.LedAmberOn; this.digitLEDRed.Enabled = this.digitController.LedRedOn; if (this.digitController.LedRedOn) this.StopPrint(true, true, false, false); return; }
                }
                else if (sender.Equals(this.packController))
                {
                    if (e.PropertyName == "MainStatus") { this.packStatusbox.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.packController.MainStatus + "\r\n" + this.packStatusbox.Text; this.cutStatusBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.packLEDGreen.Enabled = this.packController.LedGreenOn; this.packLEDAmber.Enabled = this.packController.LedAmberOn; this.packLEDRed.Enabled = this.packController.LedRedOn; if (this.packController.LedRedOn) this.StopPrint(true, true, false, false); return; }

                    if (e.PropertyName == "NextPackNo") { this.fillingData.NextPackNo = this.packController.NextPackNo; return; }
                }
                else if (sender.Equals(this.cartonController))
                {
                    if (e.PropertyName == "MainStatus") { this.cartonStatusbox.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.cartonController.MainStatus + "\r\n" + this.cartonStatusbox.Text; this.cutStatusBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.cartonLEDGreen.Enabled = this.cartonController.LedGreenOn; this.cartonLEDAmber.Enabled = this.cartonController.LedAmberOn; this.cartonLEDRed.Enabled = this.cartonController.LedRedOn; return; }

                    if (e.PropertyName == "NextCartonNo") { this.fillingData.NextCartonNo = this.cartonController.NextCartonNo; return; }
                }

                else if (sender.Equals(this.palletController))
                {
                    if (e.PropertyName == "MainStatus") { this.palletStatusbox.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.palletController.MainStatus + "\r\n" + this.palletStatusbox.Text; this.cutStatusBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.palletLEDGreen.Enabled = this.palletController.LedGreenOn; this.palletLEDAmber.Enabled = this.palletController.LedAmberOn; this.palletLEDRed.Enabled = this.palletController.LedRedOn; return; }

                    if (e.PropertyName == "NextPalletNo") { this.fillingData.NextPalletNo = this.palletController.NextPalletNo; return; }
                }

                else if (sender.Equals(this.scannerController))
                {
                    if (e.PropertyName == "MainStatus") { this.scannerStatusbox.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.scannerController.MainStatus + "\r\n" + this.scannerStatusbox.Text; this.cutStatusBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.scannerLEDGreen.Enabled = this.scannerController.LedGreenOn; this.scannerLEDAmber.Enabled = this.scannerController.LedAmberOn; this.scannerLEDRed.Enabled = this.scannerController.LedRedOn; if (this.scannerController.LedRedOn) this.StopPrint(); return; }

                    if (e.PropertyName == "LedMCU") { this.toolStripMCUQuanlity.Enabled = this.scannerController.LedMCUQualityOn; this.toolStripMCUMatching.Enabled = this.scannerController.LedMCUMatchingOn; this.toolStripMCUCarton.Enabled = this.scannerController.LedMCUCartonOn; return; }



                    if (e.PropertyName == "PackQueue")
                    {
                        int currentRowIndex = -1; int currentColumnIndex = -1;
                        if (this.dgvPackQueue.CurrentCell != null) { currentRowIndex = this.dgvPackQueue.CurrentCell.RowIndex; currentColumnIndex = this.dgvPackQueue.CurrentCell.ColumnIndex; }

                        this.dgvPackQueue.DataSource = this.scannerController.GetPackQueue();

                        if (currentRowIndex >= 0 && currentRowIndex < this.dgvPackQueue.Rows.Count && currentColumnIndex >= 0 && currentColumnIndex < this.dgvPackQueue.ColumnCount) this.dgvPackQueue.CurrentCell = this.dgvPackQueue[currentColumnIndex, currentRowIndex]; //Keep current cell

                        this.buttonDeleteAllPack.Text = "[" + this.scannerController.PackQueueCount.ToString("N0") + "]";
                        this.buttonPackQueueCount.Text = this.scannerController.NextPackQueueDescription;
                    }

                    if (e.PropertyName == "PacksetQueue") { this.dgvPacksetQueue.DataSource = this.scannerController.GetPacksetQueue(); this.buttonPacksetQueueCount.Text = "[" + this.scannerController.PacksetQueueCount.ToString("N0") + "]"; }

                    if (e.PropertyName == "CartonPendingQueue")
                    {
                        this.dgvCartonPendingQueue.DataSource = this.scannerController.GetCartonPendingQueue();
                        if (this.dgvCartonPendingQueue.Rows.Count > 1) this.dgvCartonPendingQueue.CurrentCell = this.dgvCartonPendingQueue.Rows[0].Cells[0];

                        this.buttonCartonPendingQueueCount.Text = "[" + this.scannerController.CartonPendingQueueCount.ToString("N0") + "]";
                    }

                    if (e.PropertyName == "CartonQueue")
                    {
                        this.dgvCartonQueue.DataSource = this.scannerController.GetCartonQueue();
                        if (this.dgvCartonQueue.Rows.Count > 1) this.dgvCartonQueue.CurrentCell = this.dgvCartonQueue.Rows[0].Cells[0];

                        this.buttonCartonQueueCount.Text = "[" + this.scannerController.CartonQueueCount.ToString("N0") + "]";
                    }

                    if (e.PropertyName == "CartonsetQueue") { this.dgvCartonsetQueue.DataSource = this.scannerController.GetCartonsetQueue(); this.buttonCartonsetQueueCount.Text = "[" + this.scannerController.CartonsetQueueCount.ToString("N0") + "]"; }

                    if (e.PropertyName == "PalletQueue")
                    {
                        this.dgvPalletQueue.DataSource = this.scannerController.GetPalletQueue();
                        this.buttonPalletQueueCount.Text = "[" + this.scannerController.PalletQueueCount.ToString("N0") + "]";
                    }
                }

            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void setToolStripActive()
        {
            bool anyLoopRoutine = digitController.LoopRoutine | packController.LoopRoutine | cartonController.LoopRoutine | palletController.LoopRoutine | scannerController.LoopRoutine;
            bool allLoopRoutine = digitController.LoopRoutine && packController.LoopRoutine && cartonController.LoopRoutine && palletController.LoopRoutine && scannerController.LoopRoutine;

            bool anyOnPrinting = digitController.OnPrinting | packController.OnPrinting | cartonController.OnPrinting | palletController.OnPrinting | scannerController.OnScanning;
            //bool allOnPrinting = digitInkjetDominoPrinter.OnPrinting && barcodeInkjetDominoPrinter.OnPrinting && cartonInkjetDominoPrinter.OnPrinting  && palletInkjetDominoPrinter.OnPrinting && barcodeScannerMCU.OnPrinting;

            bool allLedGreenOn = digitController.LedGreenOn && packController.LedGreenOn && cartonController.LedGreenOn && palletController.LedGreenOn && scannerController.LedGreenOn;

            this.buttonConnect.Enabled = !anyLoopRoutine;
            this.buttonDisconnect.Enabled = anyLoopRoutine && !anyOnPrinting;
            this.buttonStart.Enabled = allLoopRoutine && !anyOnPrinting && allLedGreenOn;
            this.buttonStop.Enabled = anyOnPrinting;

            this.buttonReprint.Enabled = this.palletController.LedGreenOn && palletController.OnPrinting;

            this.buttonBatches.Enabled = !anyLoopRoutine;



            this.digitLEDGreen.Enabled = digitController.LoopRoutine && this.digitController.LedGreenOn;
            this.packLEDGreen.Enabled = packController.LoopRoutine && this.packController.LedGreenOn;
            this.cartonLEDGreen.Enabled = cartonController.LoopRoutine && this.cartonController.LedGreenOn;
            this.palletLEDGreen.Enabled = palletController.LoopRoutine && this.palletController.LedGreenOn;
            this.scannerLEDGreen.Enabled = scannerController.LoopRoutine && this.scannerController.LedGreenOn;


            this.digitLEDPrinting.Enabled = digitController.OnPrinting && this.digitController.LedGreenOn;
            this.packLEDPrinting.Enabled = packController.OnPrinting && this.packController.LedGreenOn;
            this.cartonLEDPrinting.Enabled = cartonController.OnPrinting && this.cartonController.LedGreenOn;
            this.palletLEDPrinting.Enabled = palletController.OnPrinting && this.palletController.LedGreenOn;
            this.scannerLEDScanning.Enabled = scannerController.OnScanning && this.scannerController.LedGreenOn;
        }

        private void cutStatusBox(bool clearStatusBox)
        {
            if (clearStatusBox)
            {
                this.digitStatusbox.Text = "";
                this.packStatusbox.Text = "";
                this.cartonStatusbox.Text = "";
                this.palletStatusbox.Text = "";
                this.scannerStatusbox.Text = "";
            }
            else
            {
                if (this.digitStatusbox.TextLength > 1000) this.digitStatusbox.Text = this.digitStatusbox.Text.Substring(0, 1000);
                if (this.packStatusbox.TextLength > 1000) this.packStatusbox.Text = this.packStatusbox.Text.Substring(0, 1000);
                if (this.cartonStatusbox.TextLength > 1000) this.cartonStatusbox.Text = this.cartonStatusbox.Text.Substring(0, 1000);
                if (this.palletStatusbox.TextLength > 1000) this.palletStatusbox.Text = this.palletStatusbox.Text.Substring(0, 1000);
                if (this.scannerStatusbox.TextLength > 1000) this.scannerStatusbox.Text = this.scannerStatusbox.Text.Substring(0, 1000);
            }
        }


        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                e.Value = this.GetSerialNumber(e.Value.ToString());
            }
            catch
            { }
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)//e.RowIndex == -1 &&
                {
                    e.PaintBackground(e.CellBounds, true);
                    e.Graphics.TranslateTransform(e.CellBounds.Left, e.CellBounds.Bottom);
                    e.Graphics.RotateTransform(270);
                    e.Graphics.DrawString(e.FormattedValue.ToString(), e.CellStyle.Font, Brushes.Black, 5, 5);
                    e.Graphics.ResetTransform();
                    e.Handled = true;
                }
            }
            catch
            { }
        }

        private void dataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dataGridView_Enter(object sender, EventArgs e)
        {
            this.dgvPackQueue.ScrollBars = ScrollBars.Horizontal;
        }

        private void dataGridView_Leave(object sender, EventArgs e)
        {
            this.dgvPackQueue.ScrollBars = ScrollBars.None;
        }

        private string GetSerialNumber(string printedBarcode)
        {
            int indexOfDoubleTabChar = printedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString());
            if (indexOfDoubleTabChar == 0)
                printedBarcode = ""; //10-AUG-2017: WHAT IS GlobalVariables.doubleTabChar.ToString()???
            //else if (printedBarcode.Length > 6) printedBarcode = printedBarcode.Substring(printedBarcode.Length - 7, 6); //Char[3][4][5]...[9]: Serial Number
            else
            {
                if (this.fillingData.HasPack && printedBarcode.Length >= 29)
                    printedBarcode = printedBarcode.Substring(indexOfDoubleTabChar - 6, 6);

                if (!this.fillingData.HasPack && printedBarcode.Length >= 29)
                    printedBarcode = printedBarcode.Substring(0, indexOfDoubleTabChar);
            }

            //////--BP CASTROL
            ////if (printedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString()) == 0) printedBarcode = "";
            //////else if (printedBarcode.Length > 6) printedBarcode = printedBarcode.Substring(printedBarcode.Length - 7, 6); //Char[3][4][5]...[9]: Serial Number
            ////else
            ////    if (printedBarcode.Length >= 29) printedBarcode = printedBarcode.Substring(23, 6); //Char[3][4][5]...[9]: Serial Number
            ////    else if (printedBarcode.Length >= 12) printedBarcode = printedBarcode.Substring(6, 5);

            return printedBarcode;
        }

        #endregion Handler


        #region Exception Handler

        /// <summary>
        /// Find a specific pack number in matching queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPackQueue_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string cellValue = "";
                if (CustomInputBox.Show("BP Filling System", "Please input pack number", ref cellValue) == System.Windows.Forms.DialogResult.OK)
                {
                    for (int rowIndex = 0; rowIndex < this.dgvPackQueue.Rows.Count; rowIndex++)
                    {
                        for (int columnIndex = 0; columnIndex < this.dgvPackQueue.Rows[rowIndex].Cells.Count; columnIndex++)
                        {
                            if (this.GetSerialNumber(this.dgvPackQueue[columnIndex, rowIndex].Value.ToString()).IndexOf(cellValue) != -1)
                            {
                                if (rowIndex >= 0 && rowIndex < this.dgvPackQueue.Rows.Count && columnIndex >= 0 && columnIndex < this.dgvPackQueue.ColumnCount)
                                    this.dgvPackQueue.CurrentCell = this.dgvPackQueue[columnIndex, rowIndex];
                                else
                                    this.dgvPackQueue.CurrentCell = null;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                this.dgvPackQueue.CurrentCell = null;
            }
        }

        /// <summary>
        /// Remove a specific pack in matching queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeletePack_Click(object sender, EventArgs e)
        {
            if (this.dgvPackQueue.CurrentCell != null)
            {
                try
                {                //Handle exception for PackInOneCarton
                    string selectedBarcode = "";
                    int packID = this.getBarcodeID(this.dgvPackQueue.CurrentCell, out selectedBarcode);
                    if (packID > 0 && CustomMsgBox.Show(this, (sender.Equals(this.buttonDeleteAllPack) ? "Xóa toàn bộ chai đang trên chuyền" : "Xóa chai này:" + (char)13 + (char)13 + selectedBarcode) + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (this.scannerController.RemovePackInPackQueue(packID, sender.Equals(this.buttonDeleteAllPack))) CustomMsgBox.Show(this, (sender.Equals(this.buttonDeleteAllPack) ? "Toàn bộ chai đang trên chuyền" : "Pack: " + selectedBarcode) + "\r\n\r\nĐã được xóa thành công.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    ExceptionHandlers.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        /// <summary>
        /// Move Packset To Carton Pending Queue (THE SAME AS NoRead, BUT: WITHOUT READ BY SCANNER)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPackset_Remove(object sender, EventArgs e)
        {
            if (this.dgvPacksetQueue.CurrentCell != null)
            {
                try
                {
                    string selectedBarcode = "";
                    int packID = this.getBarcodeID(this.dgvPacksetQueue.CurrentCell, out selectedBarcode);
                    if (packID > 0 && CustomMsgBox.Show(this, "Thùng carton này chuẩn bị đóng, bạn muốn bỏ ra khỏi chuyền xử lý sau:" + (char)13 + (char)13 + selectedBarcode, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (this.scannerController.MovePacksetToCartonPendingQueue(packID)) CustomMsgBox.Show(this, "Thùng carton (chuẩn bị đóng) chứa chai: " + selectedBarcode + "\r\nđã bỏ ra khỏi chuyền.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    ExceptionHandlers.ShowExceptionMessageBox(this, exception);
                }
            }
        }


        private void dgvCarton_Remove(object sender, EventArgs e)
        {
            try
            {
                DataGridView dataGridView = sender.Equals(this.buttonRemoveCarton) ? this.dgvCartonQueue : this.dgvCartonsetQueue;
                if (dataGridView != null && dataGridView.CurrentCell != null)
                {
                    string selectedBarcode = "";
                    int barcodeID = this.getBarcodeID(dataGridView.CurrentCell, out selectedBarcode);
                    if (barcodeID > 0 && CustomMsgBox.Show(this, "Are you sure you want to remove this carton:" + (char)13 + (char)13 + selectedBarcode, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (this.scannerController.MoveCartonToPendingQueue(barcodeID, dataGridView.Equals(this.dgvCartonsetQueue))) CustomMsgBox.Show(this, "Carton: " + selectedBarcode + "\r\nHas been removed successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        /// <summary>
        /// REMOVE: Unwrap a Noread || Pending carton
        /// DELETE: DELETE ALL PACK IN a Noread || Pending carton => USER MUST TO SCAN BY MATCHING SCANNER AGAIN IN ORDER TO MAKE CARTON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCartonPending_RemoveDelete(object sender, EventArgs e)
        {
            if (this.dgvCartonPendingQueue.CurrentCell != null)
            {
                try
                {
                    string selectedBarcode = "";
                    int cartonID = this.getBarcodeID(this.dgvCartonPendingQueue.CurrentCell, out selectedBarcode);
                    if (cartonID > 0 && CustomMsgBox.Show(this, "Bạn có muốn " + (sender.Equals(this.buttonRemoveCartonPending) ? "xã thùng carton này ra và đóng lại không:" : "xóa toàn bộ thùng carton, bao gồm các chai bên trong,: ") + (char)13 + (char)13 + selectedBarcode, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (sender.Equals(this.buttonRemoveCartonPending))
                            if ((this.fillingData.HasPack && this.scannerController.UnwrapCartontoPack(cartonID)) || (!this.fillingData.HasPack && this.scannerController.TakebackCartonFromPendingQueue(cartonID))) CustomMsgBox.Show(this, "Carton: " + selectedBarcode + "\r\nĐã được xã thành công.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (sender.Equals(this.buttonDeleteCartonPending))
                            if (this.scannerController.DeleteCarton(cartonID)) CustomMsgBox.Show(this, "Carton: " + selectedBarcode + "\r\nĐã được xóa, bao gồm các chai bên trong." + "\r\nVui lòng đọc lại từng chai nếu muốn đóng lại carton.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionHandlers.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        private void dgvQueue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                try
                {
                    DataGridView dataGridView = sender as DataGridView;
                    if (dataGridView != null && dataGridView.CurrentCell != null)
                    {
                        string selectedBarcode = "";
                        int barcodeID = this.getBarcodeID(dataGridView.CurrentCell, out selectedBarcode);
                        if (barcodeID > 0)
                        {
                            QuickView quickView = new QuickView(this.scannerController.GetBarcodeList(sender.Equals(this.dgvCartonPendingQueue) || sender.Equals(this.dgvCartonQueue) || sender.Equals(this.dgvCartonsetQueue) ? barcodeID : 0, sender.Equals(this.dgvCartonPendingQueue) || sender.Equals(this.dgvCartonQueue) || sender.Equals(this.dgvCartonsetQueue) ? 0 : barcodeID));
                            quickView.ShowDialog();
                        }
                    }
                }
                catch (Exception exception)
                {
                    ExceptionHandlers.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        private int getBarcodeID(DataGridViewCell dataGridViewCell, out string selectedBarcode)
        {
            int barcodeID;
            if (dataGridViewCell != null)
            {
                selectedBarcode = dataGridViewCell.Value as string;
                if (selectedBarcode != null)
                {
                    int startIndexOfPackID = selectedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString() + GlobalVariables.doubleTabChar.ToString());
                    if (startIndexOfPackID >= 0 && int.TryParse(selectedBarcode.Substring(startIndexOfPackID + 2), out barcodeID))
                    {
                        selectedBarcode = this.GetSerialNumber(selectedBarcode) + ": " + selectedBarcode.Substring(0, startIndexOfPackID);
                        return barcodeID;
                    }
                }
            }
            selectedBarcode = null;
            return -1;
        }

        private void buttonPackQueueCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (CustomMsgBox.Show(this, "Bạn có muốn reset số thứ tự chia làn auto packer về vị trí gốc [1, 1].?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    this.scannerController.ResetNextPackQueueID();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonPacksetQueueCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.scannerController.PackQueueCount > 0 && this.scannerController.PacksetQueueCount == 0 && CustomMsgBox.Show(this, "Bạn có muốn chia đều chai vào các làn auto packer không?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    this.scannerController.ReAllocationPack();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonCartonQueueCount_Click(object sender, EventArgs e)
        {
            try
            {//HERE: WE CHECK CONDITION BEFORE CALL ToggleLastCartonset (TO PROTEST ACCIDENTAL PRESS BY WORKER). SURELY, WE CAN OMIT THESE CONDITION
                if (this.scannerController.PackQueueCount == 0 && this.scannerController.PacksetQueueCount == 0 && this.scannerController.CartonQueueCount > 0 && this.scannerController.CartonQueueCount < this.fillingData.CartonPerPallet && this.scannerController.CartonPendingQueueCount == 0 && this.scannerController.CartonsetQueueCount == 0 && CustomMsgBox.Show(this, "Số lượng carton ít hơn số lượng cần đóng pallet.\r\n\r\nBạn có muốn đóng pallet ngay bây giờ không?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    this.scannerController.ToggleLastCartonset(true);
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion Exception Handler




        #region Backup

        private void timerNmvnBackup_Tick(object sender, EventArgs e)
        {
            try
            {
                this.BackupDataHandler();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void BackupDataHandler()
        {
            try
            {
                if (digitThread == null || !digitThread.IsAlive)
                {
                    if (packThread == null || !packThread.IsAlive)
                    {
                        if (cartonThread == null || !cartonThread.IsAlive)
                        {
                            if (palletThread == null || !palletThread.IsAlive)
                            {
                                if (scannerThread == null || !scannerThread.IsAlive)
                                {
                                    if (backupDataThread == null || !backupDataThread.IsAlive)
                                    {
                                        backupDataThread = new Thread(new ThreadStart(this.scannerController.BackupData));
                                        backupDataThread.Start();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion Backup










    }
}


