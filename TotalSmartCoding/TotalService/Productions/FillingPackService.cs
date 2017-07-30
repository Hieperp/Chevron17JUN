using System.Text;

using TotalBase;
using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;
using System;
using System.Collections.Generic;

namespace TotalService.Productions
{
    public class FillingPackService : GenericService<FillingPack, FillingPackDTO, FillingPackPrimitiveDTO>, IFillingPackService
    {
        private IFillingPackRepository fillingPackRepository;
        public FillingPackService(IFillingPackRepository fillingPackRepository)
            : base(fillingPackRepository)
        {
            this.fillingPackRepository = fillingPackRepository;
        }

        public IList<FillingPack> GetFillingPacks(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingCartonID)
        {
            return this.fillingPackRepository.GetFillingPacks(fillingLineID, entryStatusIDs, fillingCartonID);
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
        public bool UpdateQueueID(string fillingPackIDs, int queueID)
        {
            try
            {
                this.fillingPackRepository.UpdateQueueID(fillingPackIDs, queueID);
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
