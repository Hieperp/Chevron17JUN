using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

using Ninject;

//using Global.Class.Library;
//using BusinessLogicLayer;
//using BusinessLogicLayer.InkjetDominoPrinter;
//using BusinessLogicLayer.BarcodeScanner;
//using DataTransferObject;

//using DataAccessLayer;

using TotalSmartCoding.CommonLibraries;
using TotalBase;
using TotalSmartCoding.CommonLibraries.BP;
using TotalDTO.Productions;

using TotalSmartCoding.Controllers.Productions;
using TotalCore.Services.Productions;
using TotalCore.Repositories.Productions;
using TotalSmartCoding.Controllers.APIs.Productions;
using TotalModel.Models;
using TotalSmartCoding.Views.Commons;

using TotalSmartCoding.Views.Mains;
using TotalBase.Enums;
using AutoMapper;

namespace TotalSmartCoding.Views.Productions
{
    /// <summary>
    /// "C:\Program Files\Microsoft SQL Server\100\Tools\Binn\VSShell\Common7\IDE\Ssms.exe"
    /// </summary>
    public partial class SmartCoding : Form, IMergeToolStrip
    {

        #region Declaration

        delegate void SetTextCallback(string text);

        delegate void propertyChangedThread(object sender, PropertyChangedEventArgs e);

        private PrinterController digitPrinterController;
        private PrinterController barcodePrinterController;
        private PrinterController cartonPrinterController;

        private ScannerController scannerController;


        private Thread digitPrinterThread;
        private Thread barcodePrinterThread;
        private Thread cartonPrinterThread;

        private Thread scannerThread;

        private Thread backupDataThread;


        private FillingData fillingData;


        #endregion Declaration



        #region Contructor & Implement Interface

        public SmartCoding()
        {
            if (GlobalVariables.shouldRestoreProcedure)
            {
                SQLRestore sqlRestore = new SQLRestore();
                sqlRestore.RestoreProcedure();
                sqlRestore.RestoreBackupData();
            }

            InitializeComponent();

            try
            {
                this.fillingData = new FillingData();

                BatchAPIController batchAPIController = new BatchAPIController(CommonNinject.Kernel.Get<IBatchAPIRepository>());
                BatchIndex batchIndex = batchAPIController.GetActiveBatchIndex();
                if (batchIndex != null) Mapper.Map<BatchIndex, FillingData>(batchIndex, this.fillingData);







                this.textBoxFillingLineName.TextBox.DataBindings.Add("Text", this.fillingData, "FillingLineName");
                this.textBoxCommodityCode.TextBox.DataBindings.Add("Text", this.fillingData, "CommodityCode");
                this.textBoxCommodityOfficialCode.TextBox.DataBindings.Add("Text", this.fillingData, "CommodityOfficialCode");
                this.textBoxBatchCode.TextBox.DataBindings.Add("Text", this.fillingData, "BatchCode");
                this.textBoxLastPackNo.TextBox.DataBindings.Add("Text", this.fillingData, "LastPackNo");
                this.textBoxLastCartonNo.TextBox.DataBindings.Add("Text", this.fillingData, "LastCartonNo");
                this.textBoxLastPalletNo.TextBox.DataBindings.Add("Text", this.fillingData, "LastPalletNo");






                digitPrinterController = new PrinterController(GlobalVariables.DominoPrinterName.DegitInkJet, this.fillingData, this.fillingData.FillingLineID == GlobalVariables.FillingLine.Pail);
                barcodePrinterController = new PrinterController(GlobalVariables.DominoPrinterName.BarCodeInkJet, this.fillingData, false);
                cartonPrinterController = new PrinterController(GlobalVariables.DominoPrinterName.CartonInkJet, this.fillingData, false);

                this.scannerController = new ScannerController(this.fillingData);

                digitPrinterController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);
                barcodePrinterController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);
                cartonPrinterController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);

                scannerController.PropertyChanged += new PropertyChangedEventHandler(controller_PropertyChanged);



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

                this.comboBoxViewOption.ComboBox.Items.AddRange(new string[] { "Compact View", "Normal View" });
                this.comboBoxViewOption.ComboBox.SelectedIndex = this.fillingData.FillingLineID == GlobalVariables.FillingLine.Pail ? 1 : 0;
                this.comboBoxViewOption.Enabled = this.fillingData.FillingLineID != GlobalVariables.FillingLine.Pail;

                this.comboBoxEmptyCarton.ComboBox.Items.AddRange(new string[] { "Ignore Empty Carton", "Keep Empty Carton" });
                this.comboBoxEmptyCarton.ComboBox.SelectedIndex = (this.fillingData.FillingLineID == GlobalVariables.FillingLine.Pail || !GlobalVariables.IgnoreEmptyCarton) ? 1 : 0;
                this.comboBoxEmptyCarton.Enabled = this.fillingData.FillingLineID != GlobalVariables.FillingLine.Pail;

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void SmartCoding_Activated(object sender, EventArgs e)
        {
            if (this.dataGridViewCartonList.CanSelect) this.dataGridViewCartonList.Select();
        }

        public ToolStrip ChildToolStrip
        {
            get
            {
                return this.toolStripChildForm;
            }
            set
            {
                this.toolStripChildForm = value;
            }
        }

        private int SplitterDistanceQuality()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Ocme:
                    return 115; //142
                case GlobalVariables.FillingLine.CO:
                    return 296; //364 
                case GlobalVariables.FillingLine.WH:
                    return 70; //86;
                case GlobalVariables.FillingLine.CM:
                    return 86;
                case GlobalVariables.FillingLine.Pail:
                    return 0;
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
                case GlobalVariables.FillingLine.Ocme:
                    return 821; //1032
                case GlobalVariables.FillingLine.CO:
                    return 955;//1199
                case GlobalVariables.FillingLine.WH:
                    return GlobalVariables.noItemPerCartonSetByProductID == 6 ? 860 : 880;
                case GlobalVariables.FillingLine.CM:
                    return GlobalVariables.noItemPerCartonSetByProductID == 6 ? 860 : 880;
                case GlobalVariables.FillingLine.Pail:
                    return 760;//760---24
                default:
                    return 1;
            }
        }

        private int SplitterDistanceCarton()
        {
            switch (GlobalVariables.FillingLineID)
            {
                case GlobalVariables.FillingLine.Ocme:
                    return 330; //430
                case GlobalVariables.FillingLine.CO:
                    return 160; //213
                case GlobalVariables.FillingLine.WH:
                    return 361; //485
                case GlobalVariables.FillingLine.CM:
                    return 361;
                case GlobalVariables.FillingLine.Pail:
                    return 429;
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
                this.CutTextBox(true);

                if (backupDataThread == null || !backupDataThread.IsAlive)
                {
                    if (digitPrinterThread != null && digitPrinterThread.IsAlive) digitPrinterThread.Abort();
                    digitPrinterThread = new Thread(new ThreadStart(digitPrinterController.ThreadRoutine));

                    if (barcodePrinterThread != null && barcodePrinterThread.IsAlive) barcodePrinterThread.Abort();
                    barcodePrinterThread = new Thread(new ThreadStart(barcodePrinterController.ThreadRoutine));

                    if (cartonPrinterThread != null && cartonPrinterThread.IsAlive) cartonPrinterThread.Abort();
                    cartonPrinterThread = new Thread(new ThreadStart(cartonPrinterController.ThreadRoutine));

                    if (scannerThread != null && scannerThread.IsAlive) scannerThread.Abort();
                    scannerThread = new Thread(new ThreadStart(scannerController.ThreadRoutine));


                    digitPrinterThread.Start();
                    barcodePrinterThread.Start();
                    cartonPrinterThread.Start();
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
                digitPrinterController.LoopRoutine = false;
                barcodePrinterController.LoopRoutine = false;
                cartonPrinterController.LoopRoutine = false;

                scannerController.LoopRoutine = false;

                this.SetToolStripActive();
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
                this.CutTextBox(true);

                this.digitPrinterController.StartPrint();
                this.barcodePrinterController.StartPrint();
                this.cartonPrinterController.StartPrint();

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
                if (MessageBox.Show("The software will not monitor the scanners affter stopped." + (char)13 + (char)13 + "Are you sure you want to stop?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                    if (MessageBox.Show("Do you really want to stop?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
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
            this.StopPrint(true, true, true);
        }

        private void StopPrint(bool stopDigit, bool stopBarcode, bool stopCarton)
        {
            if (stopDigit) this.digitPrinterController.StopPrint();
            if (stopBarcode) this.barcodePrinterController.StopPrint();
            if (stopCarton) this.cartonPrinterController.StopPrint();
        }

        #endregion Toolstrip bar


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
                this.SetToolStripActive();

                if (sender.Equals(this.digitPrinterController))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxDigitStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.digitPrinterController.MainStatus + "\r\n" + this.textBoxDigitStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripDigitLEDGreen.Enabled = this.digitPrinterController.LedGreenOn; this.toolStripDigitLEDAmber.Enabled = this.digitPrinterController.LedAmberOn; this.toolStripDigitLEDRed.Enabled = this.digitPrinterController.LedRedOn; if (this.digitPrinterController.LedRedOn) this.StopPrint(true, true, false); return; }
                }
                else if (sender.Equals(this.barcodePrinterController))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxBarcodeStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.barcodePrinterController.MainStatus + "\r\n" + this.textBoxBarcodeStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripBarcodeLEDGreen.Enabled = this.barcodePrinterController.LedGreenOn; this.toolStripBarcodeLEDAmber.Enabled = this.barcodePrinterController.LedAmberOn; this.toolStripBarcodeLEDRed.Enabled = this.barcodePrinterController.LedRedOn; if (this.barcodePrinterController.LedRedOn) this.StopPrint(true, true, false); return; }

                    if (e.PropertyName == "MonthSerialNumber") { this.fillingData.MonthSerialNumber = this.barcodePrinterController.MonthSerialNumber; this.fillingData.Update(); return; }
                }
                else if (sender.Equals(this.cartonPrinterController))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxCartonStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.cartonPrinterController.MainStatus + "\r\n" + this.textBoxCartonStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripCartonLEDGreen.Enabled = this.cartonPrinterController.LedGreenOn; this.toolStripCartonLEDAmber.Enabled = this.cartonPrinterController.LedAmberOn; this.toolStripCartonLEDRed.Enabled = this.cartonPrinterController.LedRedOn; return; }//if (this.cartonInkjetDominoPrinter.LedRedOn) this.StopPrint(); 

                    if (e.PropertyName == "LastCartonNo") { this.fillingData.LastCartonNo = this.cartonPrinterController.LastCartonNo; this.fillingData.MonthCartonNumber = this.cartonPrinterController.MonthCartonNumber; this.fillingData.Update(); return; }
                    if (e.PropertyName == "MonthCartonNumber") { this.fillingData.LastCartonNo = this.cartonPrinterController.LastCartonNo; this.fillingData.MonthCartonNumber = this.cartonPrinterController.MonthCartonNumber; this.fillingData.Update(); return; }
                }
                else if (sender.Equals(this.scannerController))
                {
                    if (e.PropertyName == "MainStatus") { this.textBoxScannerStatus.Text = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + this.scannerController.MainStatus + "\r\n" + this.textBoxScannerStatus.Text; this.CutTextBox(false); return; }
                    if (e.PropertyName == "LedStatus") { this.toolStripScannerLEDGreen.Enabled = this.scannerController.LedGreenOn; this.toolStripScannerLEDAmber.Enabled = this.scannerController.LedAmberOn; this.toolStripScannerLEDRed.Enabled = this.scannerController.LedRedOn; if (this.scannerController.LedRedOn) this.StopPrint(); return; }

                    if (e.PropertyName == "LedMCU") { this.toolStripMCUQuanlity.Enabled = this.scannerController.LedMCUQualityOn; this.toolStripMCUMatching.Enabled = this.scannerController.LedMCUMatchingOn; this.toolStripMCUCarton.Enabled = this.scannerController.LedMCUCartonOn; return; }



                    if (e.PropertyName == "MatchingPackList")
                    {
                        int currentRowIndex = -1; int currentColumnIndex = -1;
                        if (this.dataGridViewMatchingPackList.CurrentCell != null) { currentRowIndex = this.dataGridViewMatchingPackList.CurrentCell.RowIndex; currentColumnIndex = this.dataGridViewMatchingPackList.CurrentCell.ColumnIndex; }

                        this.dataGridViewMatchingPackList.DataSource = this.scannerController.GetMatchingPackList();

                        if (currentRowIndex >= 0 && currentRowIndex < this.dataGridViewMatchingPackList.Rows.Count && currentColumnIndex >= 0 && currentColumnIndex < this.dataGridViewMatchingPackList.ColumnCount) this.dataGridViewMatchingPackList.CurrentCell = this.dataGridViewMatchingPackList[currentColumnIndex, currentRowIndex]; //Keep current cell

                        this.toolStripButtonMessageCount.Text = "[" + this.scannerController.MatchingPackCount.ToString("N0") + "]";
                    }

                    if (e.PropertyName == "PackInOneCarton") { this.dataGridViewPackInOneCarton.DataSource = this.scannerController.GetPackInOneCarton(); }
                    if (e.PropertyName == "CartonList") { this.dataGridViewCartonList.DataSource = this.scannerController.GetCartonList(); if (this.dataGridViewCartonList.Rows.Count > 1) this.dataGridViewCartonList.CurrentCell = this.dataGridViewCartonList.Rows[0].Cells[0]; }

                }

            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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


        private bool displayCommpactView;

        private void comboBoxViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                displayCommpactView = this.comboBoxViewOption.SelectedIndex == 0;
            }
            catch
            { }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (displayCommpactView || !sender.Equals(this.dataGridViewCartonList))
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
            this.dataGridViewMatchingPackList.ScrollBars = ScrollBars.Horizontal;
        }


        private void dataGridView_Leave(object sender, EventArgs e)
        {
            this.dataGridViewMatchingPackList.ScrollBars = ScrollBars.None;
        }


        /// <summary>
        /// Find a specific pack number in matching queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewMatchingPackList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string cellValue = "";
                if (CustomInputBox.Show("BP Filling System", "Please input pack number", ref cellValue) == System.Windows.Forms.DialogResult.OK)
                {
                    for (int rowIndex = 0; rowIndex < this.dataGridViewMatchingPackList.Rows.Count; rowIndex++)
                    {
                        for (int columnIndex = 0; columnIndex < this.dataGridViewMatchingPackList.Rows[rowIndex].Cells.Count; columnIndex++)
                        {
                            if (this.GetSerialNumber(this.dataGridViewMatchingPackList[columnIndex, rowIndex].Value.ToString()).IndexOf(cellValue) != -1)
                            {
                                if (rowIndex >= 0 && rowIndex < this.dataGridViewMatchingPackList.Rows.Count && columnIndex >= 0 && columnIndex < this.dataGridViewMatchingPackList.ColumnCount)
                                    this.dataGridViewMatchingPackList.CurrentCell = this.dataGridViewMatchingPackList[columnIndex, rowIndex];
                                else
                                    this.dataGridViewMatchingPackList.CurrentCell = null;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                this.dataGridViewMatchingPackList.CurrentCell = null;
            }
        }

        /// <summary>
        /// Remove a specific pack in matching queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewMatchingPackList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.dataGridViewMatchingPackList.CurrentCell != null)
            {
                try
                {                //Handle exception for PackInOneCarton
                    string selectedBarcode = "";
                    int packID = this.GetPackID(this.dataGridViewMatchingPackList.CurrentCell, out selectedBarcode);
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
            if (e.KeyCode == Keys.Delete && this.dataGridViewPackInOneCarton.CurrentCell != null)
            {
                try
                {                //Handle exception for PackInOneCarton
                    string selectedBarcode = "";
                    int packID = this.GetPackID(this.dataGridViewPackInOneCarton.CurrentCell, out selectedBarcode);
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
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Delete) && this.dataGridViewCartonList.CurrentRow != null)
            {
                try
                {                //Handle exception for carton
                    DataGridViewRow dataGridViewRow = this.dataGridViewCartonList.CurrentRow;
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

        private string GetSerialNumber(string printedBarcode)
        {
            if (printedBarcode.IndexOf(GlobalVariables.doubleTabChar.ToString()) == 0) printedBarcode = "";
            //else if (printedBarcode.Length > 6) printedBarcode = printedBarcode.Substring(printedBarcode.Length - 7, 6); //Char[3][4][5]...[9]: Serial Number
            else
                if (printedBarcode.Length >= 29) printedBarcode = printedBarcode.Substring(23, 6); //Char[3][4][5]...[9]: Serial Number
                else if (printedBarcode.Length >= 12) printedBarcode = printedBarcode.Substring(6, 5);

            return printedBarcode;
        }


        private void timerEverySecond_Tick(object sender, EventArgs e)
        {
            try
            {
                this.textBoxCurrentDate.TextBox.Text = DateTime.Now.ToString("dd/MM/yy");
                if (this.fillingData != null)
                {
                    if (this.fillingData.SettingMonthID != 1) //GlobalStaticFunction.DateToContinuosMonth()
                    {
                        this.toolStripButtonWarningNewMonth.Visible = !this.toolStripButtonWarningNewMonth.Visible; this.toolStripLabelWarningNewMonth.Visible = !this.toolStripLabelWarningNewMonth.Visible;
                    }
                    else
                    {
                        this.toolStripButtonWarningNewMonth.Visible = false; this.toolStripLabelWarningNewMonth.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void SmartCoding_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (digitPrinterThread != null && digitPrinterThread.IsAlive) { e.Cancel = true; return; }
                if (barcodePrinterThread != null && barcodePrinterThread.IsAlive) { e.Cancel = true; return; }
                if (cartonPrinterThread != null && cartonPrinterThread.IsAlive) { e.Cancel = true; return; }

                if (scannerThread != null && scannerThread.IsAlive) { e.Cancel = true; return; }

                if (backupDataThread != null && backupDataThread.IsAlive) { e.Cancel = true; return; }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
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


        private void SetToolStripActive()
        {
            bool anyLoopRoutine = digitPrinterController.LoopRoutine | barcodePrinterController.LoopRoutine | cartonPrinterController.LoopRoutine | scannerController.LoopRoutine;
            bool allLoopRoutine = digitPrinterController.LoopRoutine && barcodePrinterController.LoopRoutine && cartonPrinterController.LoopRoutine && scannerController.LoopRoutine;

            bool anyOnPrinting = digitPrinterController.OnPrinting | barcodePrinterController.OnPrinting | cartonPrinterController.OnPrinting | scannerController.OnScanning;
            //bool allOnPrinting = digitInkjetDominoPrinter.OnPrinting && barcodeInkjetDominoPrinter.OnPrinting && cartonInkjetDominoPrinter.OnPrinting && barcodeScannerMCU.OnPrinting;

            bool allLedGreenOn = digitPrinterController.LedGreenOn && barcodePrinterController.LedGreenOn && cartonPrinterController.LedGreenOn && scannerController.LedGreenOn;

            this.buttonConnect.Enabled = !anyLoopRoutine;
            this.buttonDisconnect.Enabled = anyLoopRoutine && !anyOnPrinting;
            this.buttonStart.Enabled = allLoopRoutine && !anyOnPrinting && allLedGreenOn;
            this.buttonStop.Enabled = anyOnPrinting;

            this.buttonBatches.Enabled = !anyLoopRoutine;



            this.toolStripDigitLEDGreen.Enabled = digitPrinterController.LoopRoutine && this.digitPrinterController.LedGreenOn;
            this.toolStripBarcodeLEDGreen.Enabled = barcodePrinterController.LoopRoutine && this.barcodePrinterController.LedGreenOn;
            this.toolStripCartonLEDGreen.Enabled = cartonPrinterController.LoopRoutine && this.cartonPrinterController.LedGreenOn;
            this.toolStripScannerLEDGreen.Enabled = scannerController.LoopRoutine && this.scannerController.LedGreenOn;


            this.toolStripDigitOnPrinting.Enabled = digitPrinterController.OnPrinting && this.digitPrinterController.LedGreenOn;
            this.toolStripBarcodeOnPrinting.Enabled = barcodePrinterController.OnPrinting && this.barcodePrinterController.LedGreenOn;
            this.toolStripCartonOnPrinting.Enabled = cartonPrinterController.OnPrinting && this.cartonPrinterController.LedGreenOn;
            this.toolStripScannerOnPrinting.Enabled = scannerController.OnScanning && this.scannerController.LedGreenOn;
        }

        private void CutTextBox(bool clearTextBox)
        {
            if (clearTextBox)
            {
                this.textBoxBarcodeStatus.Text = "";
                this.textBoxCartonStatus.Text = "";
                this.textBoxDigitStatus.Text = "";
                this.textBoxScannerStatus.Text = "";
            }
            else
            {
                if (this.textBoxBarcodeStatus.TextLength > 1000) this.textBoxBarcodeStatus.Text = this.textBoxBarcodeStatus.Text.Substring(0, 1000);
                if (this.textBoxCartonStatus.TextLength > 1000) this.textBoxCartonStatus.Text = this.textBoxCartonStatus.Text.Substring(0, 1000);
                if (this.textBoxDigitStatus.TextLength > 1000) this.textBoxDigitStatus.Text = this.textBoxDigitStatus.Text.Substring(0, 1000);
                if (this.textBoxScannerStatus.TextLength > 1000) this.textBoxScannerStatus.Text = this.textBoxScannerStatus.Text.Substring(0, 1000);
            }
        }


        #region Test only
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.splitContainerQuality.SplitterDistance = this.SplitterDistanceQuality();
            this.splitContainerMatching.SplitterDistance = this.SplitterDistanceMatching();
            this.splitContainerCarton.SplitterDistance = this.SplitterDistanceCarton();

            ////barcodeScannerMCU.MyTest = true;

            ////if (barcodeScannerMCUThread != null && barcodeScannerMCUThread.IsAlive) barcodeScannerMCUThread.Abort();
            ////barcodeScannerMCUThread = new Thread(new ThreadStart(barcodeScannerMCU.ThreadRoutine));
            ////barcodeScannerMCUThread.Start();

            ////Thread.Sleep(1000); //Delay 1s, then Start print
            ////barcodeScannerMCU.StartPrint();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (scannerController.OnScanning)
                scannerController.StopScanner();
            else
                scannerController.StartScanner();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            scannerController.LoopRoutine = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //scannerController.MyHold = !scannerController.MyHold;
        }

        #endregion Test only

        private void toolStripButtonMessageCount_Click(object sender, EventArgs e)
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
                if (digitPrinterThread == null || !digitPrinterThread.IsAlive)
                {
                    if (barcodePrinterThread == null || !barcodePrinterThread.IsAlive)
                    {
                        if (cartonPrinterThread == null || !cartonPrinterThread.IsAlive)
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
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}


