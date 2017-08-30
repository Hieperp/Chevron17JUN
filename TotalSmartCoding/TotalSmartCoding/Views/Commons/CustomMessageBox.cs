using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TotalSmartCoding.Views.Commons
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            InitializeComponent();

            this.labelText.Text = text;
            this.Text = caption;


            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    this.button1.Visible = true;
                    this.button2.Visible = false;
                    this.button3.Visible = false;

                    this.button1.Text = "OK";
                    this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.button1.Image = this.button2.Image;

                    break;
                case MessageBoxButtons.OKCancel:
                    this.button1.Visible = true;
                    this.button2.Visible = true;
                    this.button3.Visible = false;

                    this.button1.Text = "Cancel";
                    this.button2.Text = "OK";

                    this.button1.Image = global::TotalSmartCoding.Properties.Resources.signout_icon_24;

                    this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;

                    break;
                case MessageBoxButtons.YesNo:
                    this.button1.Visible = true;
                    this.button2.Visible = true;
                    this.button3.Visible = false;

                    this.button1.Text = "No";
                    this.button2.Text = "Yes";

                    this.button1.DialogResult = System.Windows.Forms.DialogResult.No;
                    this.button2.DialogResult = System.Windows.Forms.DialogResult.Yes;

                    break;
                case MessageBoxButtons.YesNoCancel:
                    this.button1.Visible = true;
                    this.button2.Visible = true;
                    this.button3.Visible = true;

                    this.button1.Text = "Cancel";
                    this.button2.Text = "No";
                    this.button3.Text = "Yes";

                    this.button2.Image = global::TotalSmartCoding.Properties.Resources.Yellow_cross;
                    this.button1.Image = global::TotalSmartCoding.Properties.Resources.signout_icon_24;

                    this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.button2.DialogResult = System.Windows.Forms.DialogResult.No;
                    this.button3.DialogResult = System.Windows.Forms.DialogResult.Yes;

                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            this.AcceptButton = defaultButton == MessageBoxDefaultButton.Button1 ? this.button1 : (defaultButton == MessageBoxDefaultButton.Button1 ? this.button2 : this.button3);
        }
    }
}
