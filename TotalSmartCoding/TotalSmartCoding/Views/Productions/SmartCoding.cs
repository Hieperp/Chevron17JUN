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
using TotalModel.Models;
using TotalDTO.Productions;

using TotalSmartCoding.Controllers.Productions;
using TotalSmartCoding.Controllers.APIs.Productions;
using TotalSmartCoding.CommonLibraries;
using TotalSmartCoding.Views.Commons;
using TotalSmartCoding.Views.Mains;

namespace TotalSmartCoding.Views.Productions
{
    public partial class SmartCoding : Form, IMergeToolStrip
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


                BatchIndex batchIndex = (new BatchAPIController(CommonNinject.Kernel.Get<IBatchAPIRepository>())).GetActiveBatchIndex();
                if (batchIndex != null) Mapper.Map<BatchIndex, FillingData>(batchIndex, this.fillingData);


                digitController = new PrinterController(this.fillingData, GlobalVariables.PrinterName.DegitInkjet);
                packController = new PrinterController(this.fillingData, GlobalVariables.PrinterName.PackInkjet);
                cartonController = new PrinterController(this.fillingData, GlobalVariables.PrinterName.CartonInkjet);
                palletController = new PrinterController(this.fillingData, GlobalVariables.PrinterName.PalletLabel);

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

                this.splitContainerQuality.SplitterDistance = this.SplitterDistanceQuality();
                this.splitContainerMatching.SplitterDistance = this.SplitterDistanceMatching();
                this.splitContainerCarton.SplitterDistance = this.SplitterDistanceCarton();

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void SmartCoding_Load(object sender, EventArgs e)
        {
            try
            {
                scannerController.Initialize();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void SmartCoding_Activated(object sender, EventArgs e)
        {
            if (this.dgvCartonQueue.CanSelect) this.dgvCartonQueue.Select();
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void splitPallet_Resize(object sender, EventArgs e)
        {
            try
            {
                this.splitDigit.SplitterDistance = this.Width / 5;
                this.splitPack.SplitterDistance = this.Width / 5;
                this.splitCarton.SplitterDistance = this.Width / 5;
                this.splitPallet.SplitterDistance = this.Width / 5;
            }
            catch { }
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


        public ToolStrip ChildToolStrip { get { return this.toolStripChildForm; } }

        private int SplitterDistanceQuality()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Smallpack:
                    return 296; //364 
                case GlobalVariables.FillingLine.Pail:
                    return 0;
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
                    return 760;//760---24
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
                    return 429;
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Phần mềm đang kết nối hệ thống máy in và đầu đọc mã vạch." + (char)13 + (char)13 + "Bạn có muốn dừng phần mềm không?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    if (MessageBox.Show("Bạn thật sự muốn dừng phần mềm?" + (char)13 + (char)13 + "Vui lòng nhấn Yes để dừng phần mềm ngay lập tức.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.StopPrint();
                        this.scannerController.StopScanner();
                    }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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
                MasterMDI masterMDI = new MasterMDI(GlobalEnums.NmvnTaskID.Batch, new Batches(this.fillingData, true));

                masterMDI.ShowDialog();

                masterMDI.Dispose();

                //this.splitContainerMatching.SplitterDistance = this.SplitterDistanceMatching();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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

                    if (e.PropertyName == "NextPackNo") { this.fillingData.NextPackNo = this.packController.NextPackNo; this.fillingData.Update(); return; }
                }
                else if (sender.Equals(this.cartonController))
                {
                    if (e.PropertyName == "MainStatus") { this.cartonStatusbox.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.cartonController.MainStatus + "\r\n" + this.cartonStatusbox.Text; this.cutStatusBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.cartonLEDGreen.Enabled = this.cartonController.LedGreenOn; this.cartonLEDAmber.Enabled = this.cartonController.LedAmberOn; this.cartonLEDRed.Enabled = this.cartonController.LedRedOn; return; }

                    if (e.PropertyName == "NextCartonNo") { this.fillingData.NextCartonNo = this.cartonController.NextCartonNo; this.fillingData.NextPalletNo = this.cartonController.NextPalletNo; this.fillingData.Update(); return; }
                }

                else if (sender.Equals(this.palletController))
                {
                    if (e.PropertyName == "MainStatus") { this.palletStatusbox.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.palletController.MainStatus + "\r\n" + this.palletStatusbox.Text; this.cutStatusBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.palletLEDGreen.Enabled = this.palletController.LedGreenOn; this.palletLEDAmber.Enabled = this.palletController.LedAmberOn; this.palletLEDRed.Enabled = this.palletController.LedRedOn; return; }

                    if (e.PropertyName == "NextPalletNo") { this.fillingData.NextPalletNo = this.palletController.NextPalletNo; this.fillingData.Update(); return; }
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

                        this.buttonPackQueueCount.Text = "[" + this.scannerController.PackQueueCount.ToString("N0") + "]";
                    }

                    if (e.PropertyName == "PacksetQueue") { this.dgvPacksetQueue.DataSource = this.scannerController.GetPacksetQueue(); }

                    if (e.PropertyName == "CartonQueue")
                    {
                        this.dgvCartonQueue.DataSource = this.scannerController.GetCartonQueue();
                        if (this.dgvCartonQueue.Rows.Count > 1) this.dgvCartonQueue.CurrentCell = this.dgvCartonQueue.Rows[0].Cells[0];

                        this.buttonCartonQueueCount.Text = "[" + this.scannerController.CartonQueueCount.ToString("N0") + "]";
                    }

                    if (e.PropertyName == "CartonsetQueue") { this.dgvCartonsetQueue.DataSource = this.scannerController.GetCartonsetQueue(); }

                    if (e.PropertyName == "PalletQueue")
                    {
                        this.dgvPalletQueue.DataSource = this.scannerController.GetPalletQueue();
                        this.buttonPalletQueueCount.Text = "[" + this.scannerController.PalletQueueCount.ToString("N0") + "]";
                    }
                }

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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
            if (printedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString()) == 0) printedBarcode = "";
            //else if (printedBarcode.Length > 6) printedBarcode = printedBarcode.Substring(printedBarcode.Length - 7, 6); //Char[3][4][5]...[9]: Serial Number
            else
                if (printedBarcode.Length >= 29) printedBarcode = printedBarcode.Substring(23, 6); //Char[3][4][5]...[9]: Serial Number
                else if (printedBarcode.Length >= 12) printedBarcode = printedBarcode.Substring(6, 5);

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
        private void dgvPackQueue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.dgvPackQueue.CurrentCell != null)
            {
                try
                {                //Handle exception for PackInOneCarton
                    string selectedBarcode = "";
                    int packID = this.GetPackID(this.dgvPackQueue.CurrentCell, out selectedBarcode);
                    if (packID > 0 && MessageBox.Show("Are you sure you want to remove this pack:" + (char)13 + (char)13 + selectedBarcode, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (this.scannerController.RemoveItemInMatchingPackList(packID)) MessageBox.Show("Pack: " + selectedBarcode + "\r\nHas been removed successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        /// <summary>
        /// Remove a specific pack in PackInOneCarton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewPackInOneCarton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.dgvPacksetQueue.CurrentCell != null)
            {
                try
                {                //Handle exception for PackInOneCarton
                    string selectedBarcode = "";
                    int packID = this.GetPackID(this.dgvPacksetQueue.CurrentCell, out selectedBarcode);
                    if (packID > 0 && MessageBox.Show("Are you sure you want to remove this pack:" + (char)13 + (char)13 + selectedBarcode, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        if (this.scannerController.RemoveItemInPackInOneCarton(packID)) MessageBox.Show("Pack: " + selectedBarcode + "\r\nHas been removed successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }
        }

        /// <summary>
        /// Unpacking a specific carton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCartonList_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Delete) && this.dgvCartonQueue.CurrentRow != null)
            {
                try
                {                //Handle exception for carton
                    DataGridViewRow dataGridViewRow = this.dgvCartonQueue.CurrentRow;
                    if (dataGridViewRow != null)
                    {
                        //DataRowView dataRowView = dataGridViewRow.DataBoundItem as DataRowView;
                        //DataDetail.DataDetailCartonRow selectedCarton = dataRowView.Row as DataDetail.DataDetailCartonRow;

                        //if (selectedCarton != null && selectedCarton.CartonStatus == (byte)GlobalVariables.BarcodeStatus.BlankBarcode)
                        //{
                        //    string selectedCartonDescription = this.GetSerialNumber(selectedCarton.Pack00Barcode) + ": " + selectedCarton.Pack00Barcode + (char)13 + "   " + this.GetSerialNumber(selectedCarton.Pack01Barcode) + ": " + selectedCarton.Pack01Barcode + (char)13 + "   " + this.GetSerialNumber(selectedCarton.Pack02Barcode) + ": " + selectedCarton.Pack02Barcode + (char)13 + "   " + this.GetSerialNumber(selectedCarton.Pack03Barcode) + ": " + selectedCarton.Pack03Barcode + (char)13 + "   " + "[...]";

                        //    if (e.KeyCode == Keys.Space) //Update barcode
                        //    {
                        //        string cartonBarcode = "";
                        //        if (CustomInputBox.Show("BP Filling System", "Please input barcode for this carton:" + (char)13 + (char)13 + selectedCarton.CartonBarcode + (char)13 + "   " + selectedCartonDescription, ref cartonBarcode) == System.Windows.Forms.DialogResult.OK)
                        //            if (this.barcodeScannerMCU.UpdateCartonBarcode(selectedCarton.CartonID, cartonBarcode)) MessageBox.Show("Carton: " + (char)13 + cartonBarcode + (char)13 + "   " + selectedCartonDescription + "\r\nHas been updated successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    }

                        //    if (e.KeyCode == Keys.Delete)
                        //    {
                        //        if (MessageBox.Show("Are you sure you want to remove this carton:" + (char)13 + (char)13 + selectedCarton.CartonBarcode + (char)13 + "   " + selectedCartonDescription, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        //            if (this.barcodeScannerMCU.UndoCartonToPack(selectedCarton.CartonID)) MessageBox.Show("Carton: " + (char)13 + selectedCarton.CartonBarcode + (char)13 + "   " + selectedCartonDescription + "\r\nHas been removed successfully.", "Handle exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    }
                        //}
                    }
                }
                catch (Exception exception)
                {
                    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
                }
            }
        }




        private int GetPackID(DataGridViewCell dataGridViewCell, out string selectedBarcode)
        {
            int packID;
            if (dataGridViewCell != null)
            {
                selectedBarcode = dataGridViewCell.Value as string;
                if (selectedBarcode != null)
                {
                    int startIndexOfPackID = selectedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString() + GlobalVariables.doubleTabChar.ToString());
                    if (startIndexOfPackID >= 0 && int.TryParse(selectedBarcode.Substring(startIndexOfPackID + 2), out packID))
                    {
                        selectedBarcode = this.GetSerialNumber(selectedBarcode) + ": " + selectedBarcode.Substring(0, startIndexOfPackID);
                        return packID;
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
                if (MessageBox.Show("Are you sure you want to reallocate the matching pack queue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    this.scannerController.ReAllocation();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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


