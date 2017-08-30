namespace TotalSmartCoding.Views.Mains
{
    partial class Logon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logon));
            this.groupBoxMainButton = new System.Windows.Forms.GroupBox();
            this.buttonListEmployee = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelFillingLineID = new System.Windows.Forms.Label();
            this.comboBoxEmployeeID = new System.Windows.Forms.ComboBox();
            this.comboBoxAutonicsPortName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelChangePassword = new System.Windows.Forms.Label();
            this.comboFillingLineID = new System.Windows.Forms.ComboBox();
            this.lbProductionLineID = new System.Windows.Forms.Label();
            this.labelPortAutonis = new System.Windows.Forms.Label();
            this.labelNoDomino = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.groupBoxMainButton.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMainButton
            // 
            this.groupBoxMainButton.Controls.Add(this.buttonListEmployee);
            this.groupBoxMainButton.Controls.Add(this.buttonCancel);
            this.groupBoxMainButton.Controls.Add(this.buttonOK);
            this.groupBoxMainButton.Location = new System.Drawing.Point(-417, 198);
            this.groupBoxMainButton.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxMainButton.Name = "groupBoxMainButton";
            this.groupBoxMainButton.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxMainButton.Size = new System.Drawing.Size(966, 82);
            this.groupBoxMainButton.TabIndex = 9;
            this.groupBoxMainButton.TabStop = false;
            // 
            // buttonListEmployee
            // 
            this.buttonListEmployee.Image = ((System.Drawing.Image)(resources.GetObject("buttonListEmployee.Image")));
            this.buttonListEmployee.Location = new System.Drawing.Point(444, 23);
            this.buttonListEmployee.Margin = new System.Windows.Forms.Padding(4);
            this.buttonListEmployee.Name = "buttonListEmployee";
            this.buttonListEmployee.Size = new System.Drawing.Size(33, 28);
            this.buttonListEmployee.TabIndex = 2;
            this.buttonListEmployee.UseVisualStyleBackColor = true;
            this.buttonListEmployee.Visible = false;
            this.buttonListEmployee.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Image = global::TotalSmartCoding.Properties.Resources.signout_icon_24;
            this.buttonCancel.Location = new System.Drawing.Point(807, 16);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(105, 50);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Image = global::TotalSmartCoding.Properties.Resources.Saki_NuoveXT_Actions_ok;
            this.buttonOK.Location = new System.Drawing.Point(693, 16);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(105, 50);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelFillingLineID
            // 
            this.labelFillingLineID.AutoSize = true;
            this.labelFillingLineID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFillingLineID.Location = new System.Drawing.Point(136, 71);
            this.labelFillingLineID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFillingLineID.Name = "labelFillingLineID";
            this.labelFillingLineID.Size = new System.Drawing.Size(38, 20);
            this.labelFillingLineID.TabIndex = 5;
            this.labelFillingLineID.Text = "User";
            // 
            // comboBoxEmployeeID
            // 
            this.comboBoxEmployeeID.Enabled = false;
            this.comboBoxEmployeeID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxEmployeeID.FormattingEnabled = true;
            this.comboBoxEmployeeID.Location = new System.Drawing.Point(138, 95);
            this.comboBoxEmployeeID.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxEmployeeID.Name = "comboBoxEmployeeID";
            this.comboBoxEmployeeID.Size = new System.Drawing.Size(357, 28);
            this.comboBoxEmployeeID.TabIndex = 14;
            // 
            // comboBoxAutonicsPortName
            // 
            this.comboBoxAutonicsPortName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxAutonicsPortName.FormattingEnabled = true;
            this.comboBoxAutonicsPortName.Location = new System.Drawing.Point(138, 157);
            this.comboBoxAutonicsPortName.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxAutonicsPortName.Name = "comboBoxAutonicsPortName";
            this.comboBoxAutonicsPortName.Size = new System.Drawing.Size(358, 28);
            this.comboBoxAutonicsPortName.TabIndex = 15;
            this.comboBoxAutonicsPortName.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Mật khẩu";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(4, 73);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(323, 26);
            this.textBoxPassword.TabIndex = 17;
            // 
            // labelChangePassword
            // 
            this.labelChangePassword.AutoSize = true;
            this.labelChangePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChangePassword.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelChangePassword.Location = new System.Drawing.Point(77, 53);
            this.labelChangePassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelChangePassword.Name = "labelChangePassword";
            this.labelChangePassword.Size = new System.Drawing.Size(196, 17);
            this.labelChangePassword.TabIndex = 18;
            this.labelChangePassword.Text = "Click vào đây để đổi mật khẩu";
            this.labelChangePassword.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboFillingLineID
            // 
            this.comboFillingLineID.Enabled = false;
            this.comboFillingLineID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboFillingLineID.FormattingEnabled = true;
            this.comboFillingLineID.Location = new System.Drawing.Point(138, 33);
            this.comboFillingLineID.Margin = new System.Windows.Forms.Padding(4);
            this.comboFillingLineID.Name = "comboFillingLineID";
            this.comboFillingLineID.Size = new System.Drawing.Size(357, 28);
            this.comboFillingLineID.TabIndex = 21;
            this.comboFillingLineID.Validated += new System.EventHandler(this.comboFillingLineID_Validated);
            // 
            // lbProductionLineID
            // 
            this.lbProductionLineID.AutoSize = true;
            this.lbProductionLineID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProductionLineID.Location = new System.Drawing.Point(136, 9);
            this.lbProductionLineID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbProductionLineID.Name = "lbProductionLineID";
            this.lbProductionLineID.Size = new System.Drawing.Size(36, 20);
            this.lbProductionLineID.TabIndex = 20;
            this.lbProductionLineID.Text = "Line";
            this.lbProductionLineID.DoubleClick += new System.EventHandler(this.lbProductionLineID_DoubleClick);
            // 
            // labelPortAutonis
            // 
            this.labelPortAutonis.AutoSize = true;
            this.labelPortAutonis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortAutonis.Location = new System.Drawing.Point(136, 133);
            this.labelPortAutonis.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPortAutonis.Name = "labelPortAutonis";
            this.labelPortAutonis.Size = new System.Drawing.Size(111, 20);
            this.labelPortAutonis.TabIndex = 24;
            this.labelPortAutonis.Text = "Zebra Comport";
            this.labelPortAutonis.Visible = false;
            // 
            // labelNoDomino
            // 
            this.labelNoDomino.AutoSize = true;
            this.labelNoDomino.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNoDomino.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelNoDomino.Location = new System.Drawing.Point(0, 111);
            this.labelNoDomino.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNoDomino.Name = "labelNoDomino";
            this.labelNoDomino.Size = new System.Drawing.Size(308, 17);
            this.labelNoDomino.TabIndex = 26;
            this.labelNoDomino.Text = "Double click vào đây không kết nối máy in phun";
            this.labelNoDomino.DoubleClick += new System.EventHandler(this.labelNoDomino_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxPassword);
            this.panel1.Controls.Add(this.labelNoDomino);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelChangePassword);
            this.panel1.Location = new System.Drawing.Point(502, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 150);
            this.panel1.TabIndex = 27;
            this.panel1.Visible = false;
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Image = global::TotalSmartCoding.Properties.Resources.Graphicloads_100_Flat_2_Inside_logout;
            this.pictureBoxIcon.Location = new System.Drawing.Point(27, 16);
            this.pictureBoxIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(66, 67);
            this.pictureBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxIcon.TabIndex = 11;
            this.pictureBoxIcon.TabStop = false;
            this.pictureBoxIcon.DoubleClick += new System.EventHandler(this.pictureBoxIcon_DoubleClick);
            // 
            // Logon
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(506, 275);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelPortAutonis);
            this.Controls.Add(this.comboFillingLineID);
            this.Controls.Add(this.lbProductionLineID);
            this.Controls.Add(this.comboBoxAutonicsPortName);
            this.Controls.Add(this.comboBoxEmployeeID);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.groupBoxMainButton);
            this.Controls.Add(this.labelFillingLineID);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Logon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logon";
            this.Load += new System.EventHandler(this.PublicApplicationLogon_Load);
            this.groupBoxMainButton.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMainButton;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelFillingLineID;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.ComboBox comboBoxEmployeeID;
        private System.Windows.Forms.ComboBox comboBoxAutonicsPortName;
        private System.Windows.Forms.Button buttonListEmployee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelChangePassword;
        private System.Windows.Forms.ComboBox comboFillingLineID;
        private System.Windows.Forms.Label lbProductionLineID;
        private System.Windows.Forms.Label labelPortAutonis;
        private System.Windows.Forms.Label labelNoDomino;
        private System.Windows.Forms.Panel panel1;
    }
}