using System.Text;

using TotalBase;
using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;
using System;

namespace TotalService.Productions
{
    public class FillingPackService : GenericService<FillingPack, FillingPackDTO, FillingPackPrimitiveDTO>, IFillingPackService
    {
        IFillingPackRepository fillingPackRepository;
        public FillingPackService(IFillingPackRepository fillingPackRepository)
            : base(fillingPackRepository)
        {
            this.fillingPackRepository = fillingPackRepository;
        }

        public bool UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            try
            {
                this.fillingPackRepository.UpdateEntryStatus(fillingPackIDs, barcodeStatus);
                return true;
            }
            catch (Exception ex)
            {
                this.ServiceTag = ex.Message;
                return false;
            }
        }
        //CAI NAY TAM TOI VAY THOI, CHUA CO CODE DAY DU!!! CAN PHAI XEM LAI
        public bool UpdateListOfPackSubQueueID(string fillingPackIDs, int QueueID)
        {
            try
            {
                //this.goodsReceiptRepository.UpdateEntryStatus(fillingPackIDs, barcodeStatus);
                return true;
            }
            catch (Exception ex)
            {
                this.ServiceTag = ex.Message;
                return false;
            }
        }


        protected override bool TryValidateModel(FillingPackDTO dto, ref StringBuilder invalidMessage)
        {
            if (!base.TryValidateModel(dto, ref invalidMessage)) return false;
            // cần phải ktra DTO here in order to save: có nên kết hợp IsValid của DTO để ktra ngay trong GenericService cho tất cả DTO object??? if (dto.EntryDate < new DateTime(2015, 7, 1) || dto.EntryDate > DateTime.Today.AddDays(2)) invalidMessage.Append(" Ngày không hợp lệ;");
            return true;
        }
    }
}
