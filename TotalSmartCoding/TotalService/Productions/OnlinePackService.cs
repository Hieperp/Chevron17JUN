using System.Text;

using TotalBase;
using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;
using System;

namespace TotalService.Productions
{
    public class OnlinePackService : GenericService<OnlinePack, OnlinePackDTO, OnlinePackPrimitiveDTO>, IOnlinePackService
    {
        IOnlinePackRepository goodsReceiptRepository;
        public OnlinePackService(IOnlinePackRepository goodsReceiptRepository)
            : base(goodsReceiptRepository)
        {
            this.goodsReceiptRepository = goodsReceiptRepository;
        }

        public bool UpdateEntryStatus(string onlinePackIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            try
            {
                this.goodsReceiptRepository.UpdateEntryStatus(onlinePackIDs, barcodeStatus);
                return true;
            }
            catch (Exception ex)
            {
                this.ServiceTag = ex.Message;
                return false;
            }
        }

        protected override bool TryValidateModel(OnlinePackDTO dto, ref StringBuilder invalidMessage)
        {
            if (!base.TryValidateModel(dto, ref invalidMessage)) return false;
            // cần phải ktra DTO here in order to save: có nên kết hợp IsValid của DTO để ktra ngay trong GenericService cho tất cả DTO object??? if (dto.EntryDate < new DateTime(2015, 7, 1) || dto.EntryDate > DateTime.Today.AddDays(2)) invalidMessage.Append(" Ngày không hợp lệ;");
            return true;
        }
    }
}
