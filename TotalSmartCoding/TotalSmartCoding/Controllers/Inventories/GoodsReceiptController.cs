using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Inventories;

using TotalDTO.Inventories;

using TotalSmartCoding.Controllers;
using TotalSmartCoding.ViewModels.Inventories;


using System.ComponentModel;

namespace TotalSmartCoding.Controllers.Inventories
{
    public class GoodsReceiptController : GenericViewDetailController<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO, GoodsReceiptViewModel>
    {
        public GoodsReceiptViewModel GoodsReceiptViewModel { get; private set; }
        public GoodsReceiptController(IGoodsReceiptService goodsReceiptService, GoodsReceiptViewModel goodsReceiptViewModel)
            : base(goodsReceiptService, goodsReceiptViewModel)
        {
            this.GoodsReceiptViewModel = goodsReceiptViewModel;

            //this.DtoViewModel.PropertyChanged += new PropertyChangedEventHandler(MarketingIncentiveMaster_PropertyChanged);
            this.GoodsReceiptViewModel.Reference = "123456";
            this.GoodsReceiptViewModel.EntryDate = new System.DateTime(2018, 1, 1);
            
        }


        private void MarketingIncentiveMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int i = 0;
            i = i + 1;
        }

    }
}
