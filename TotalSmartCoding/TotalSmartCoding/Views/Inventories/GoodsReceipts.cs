﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TotalSmartCoding.Views.Mains;



using TotalBase.Enums;
using TotalSmartCoding.CommonLibraries;

namespace TotalSmartCoding.Views.Inventories
{
    public partial class GoodsReceipts : BasicForm
    {
        public GoodsReceipts()
        {
            InitializeComponent();

            this.ChildToolStrip = this.toolStripChildForm;
        }
    }
}
