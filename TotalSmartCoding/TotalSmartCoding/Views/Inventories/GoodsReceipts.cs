using System;
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

using TotalSmartCoding.Controllers.Inventories;

namespace TotalSmartCoding.Views.Inventories
{
    public partial class GoodsReceipts : BasicView
    {
        private GoodsReceiptsController goodsReceiptsController {get; set;}

        public GoodsReceipts()
        {
            InitializeComponent();

            this.goodsReceiptsController = new GoodsReceiptsController();

            this.ChildToolStrip = this.toolStripChildForm;


            //this.olvFast.SetObjects(goodsReceiptsController.In);
        }
    }
}
