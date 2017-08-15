namespace TotalSmartCoding.Views.Mains
{
    partial class BaseView
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
            this.components = new System.ComponentModel.Container();
            this.errorProviderMaster = new System.Windows.Forms.ErrorProvider(this.components);
            this.checkBoxIsDirty = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProviderMaster
            // 
            this.errorProviderMaster.ContainerControl = this;
            // 
            // checkBoxIsDirty
            // 
            this.checkBoxIsDirty.AutoSize = true;
            this.checkBoxIsDirty.Location = new System.Drawing.Point(-300, 32);
            this.checkBoxIsDirty.Name = "checkBoxIsDirty";
            this.checkBoxIsDirty.Size = new System.Drawing.Size(59, 21);
            this.checkBoxIsDirty.TabIndex = 64;
            this.checkBoxIsDirty.Text = "Dirty";
            this.checkBoxIsDirty.UseVisualStyleBackColor = true;
            // 
            // BaseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.checkBoxIsDirty);
            this.Name = "BaseView";
            this.Text = "BaseView";
            this.Load += new System.EventHandler(this.BaseView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProviderMaster;
        private System.Windows.Forms.CheckBox checkBoxIsDirty;
    }
}