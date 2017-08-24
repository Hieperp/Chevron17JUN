using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase;
using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class FillingCartonService : GenericService<FillingCarton, FillingCartonDTO, FillingCartonPrimitiveDTO>, IFillingCartonService
    {
        IFillingCartonRepository fillingCartonRepository;
        public FillingCartonService(IFillingCartonRepository fillingCartonRepository)
            : base(fillingCartonRepository, null, "FillingCartonSaveRelative")
        {
            this.fillingCartonRepository = fillingCartonRepository;
        }

        protected override ObjectParameter[] SaveRelativeParameters(FillingCarton entity, SaveRelativeOption saveRelativeOption)
        {
            ObjectParameter[] baseParameters = base.SaveRelativeParameters(entity, saveRelativeOption); //IMPORTANT: WE SHOULD SET FillingPackIDs WHEN SaveRelativeOption.Update. WE DON'T CARE FillingPackIDs WHEN SaveRelativeOption.Undo [SEE STORE PROCEDURE FillingCartonSaveRelative FOR MORE INFORMATION] 
            ObjectParameter[] objectParameters = new ObjectParameter[] { baseParameters[0], baseParameters[1], new ObjectParameter("FillingPackIDs", this.ServiceBag.ContainsKey("FillingPackIDs") && this.ServiceBag["FillingPackIDs"] != null ? this.ServiceBag["FillingPackIDs"] : ""), new ObjectParameter("DeleteFillingPack", this.ServiceBag.ContainsKey("DeleteFillingPack") && this.ServiceBag["DeleteFillingPack"] != null ? true : false) };

            this.ServiceBag.Remove("FillingPackIDs");
            this.ServiceBag.Remove("DeleteFillingPack");

            if (this.ServiceBag.ContainsKey("DeleteFillingPack") && this.ServiceBag["DeleteFillingPack"] != null)
            {
                int i = 1;

            }

            return objectParameters;
        }

        protected override bool TryValidateModel(FillingCartonDTO dto, ref System.Text.StringBuilder invalidMessage)
        {
            if (!base.TryValidateModel(dto, ref invalidMessage)) return false;

            if (this.ServiceBag.ContainsKey("EntryStatusIDs") && this.ServiceBag["EntryStatusIDs"] != null && (this.ServiceBag["EntryStatusIDs"]).ToString().IndexOf(dto.EntryStatusID.ToString()) < 0) { invalidMessage.Append("Trạng thái carton không phù hợp [" + dto.EntryStatusID + "]"); return false; }

            return true;
        }

        public IList<FillingCarton> GetFillingCartons(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingPalletID)
        {
            return this.fillingCartonRepository.GetFillingCartons(fillingLineID, entryStatusIDs, fillingPalletID);
        }

        public bool UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            try
            {
                this.fillingCartonRepository.UpdateEntryStatus(fillingCartonIDs, barcodeStatus);
                return true;
            }
            catch (Exception ex)
            {
                this.ServiceTag = ex.Message;
                return false;
            }
        }
    }
}
