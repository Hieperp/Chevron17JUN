using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Inventories;

using TotalDTO.Inventories;

using TotalSmartCoding.Controllers;
using TotalSmartCoding.Builders.Inventories;
using TotalSmartCoding.ViewModels.Inventories;


using System.ComponentModel;

namespace TotalSmartCoding.Controllers.Inventories
{
    public class GoodsReceiptsController : GenericViewDetailController<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO, GoodsReceiptViewModel>
    {
        public GoodsReceiptsController(IGoodsReceiptService goodsReceiptService, IGoodsReceiptViewModelSelectListBuilder goodsReceiptViewModelSelectListBuilder)
            : base(goodsReceiptService, goodsReceiptViewModelSelectListBuilder)
        {
            //this.DtoViewModel.PropertyChanged += new PropertyChangedEventHandler(MarketingIncentiveMaster_PropertyChanged);
            this.DtoViewModel.Reference = "123456";
            this.DtoViewModel.EntryDate = new System.DateTime(2018, 1, 1);
        }


        private void MarketingIncentiveMaster_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int i = 0;
            i = i + 1;
        }

    }
}
