using TotalModel.Models;
using TotalDTO.Inventories;

using TotalCore.Services.Inventories;
using TotalSmartCoding.ViewModels.Inventories;

namespace TotalSmartCoding.Controllers.Inventories
{
    public class GoodsReceiptController : GenericViewDetailController<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, GoodsReceiptDTO, GoodsReceiptPrimitiveDTO, GoodsReceiptDetailDTO, GoodsReceiptViewModel>
    {
        public GoodsReceiptController(IGoodsReceiptService goodsReceiptService, GoodsReceiptViewModel goodsReceiptViewModel)
            : base(goodsReceiptService, goodsReceiptViewModel)
        {
        }
    }
}
