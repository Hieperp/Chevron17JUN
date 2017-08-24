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
    public class FillingPalletService : GenericService<FillingPallet, FillingPalletDTO, FillingPalletPrimitiveDTO>, IFillingPalletService
    {
        IFillingPalletRepository fillingPalletRepository;
        public FillingPalletService(IFillingPalletRepository fillingPalletRepository)
            : base(fillingPalletRepository, null, "FillingPalletSaveRelative")
        {
            this.fillingPalletRepository = fillingPalletRepository;
        }

        protected override ObjectParameter[] SaveRelativeParameters(FillingPallet entity, SaveRelativeOption saveRelativeOption)
        {
            ObjectParameter[] baseParameters = base.SaveRelativeParameters(entity, saveRelativeOption); //IMPORTANT: WE SHOULD SET FillingCartonIDs WHEN SaveRelativeOption.Update. WE DON'T CARE FillingCartonIDs WHEN SaveRelativeOption.Undo [SEE STORE PROCEDURE FillingPalletSaveRelative FOR MORE INFORMATION] 
            ObjectParameter[] objectParameters = new ObjectParameter[] { baseParameters[0], baseParameters[1], new ObjectParameter("FillingCartonIDs", this.ServiceBag["FillingCartonIDs"] != null ? this.ServiceBag["FillingCartonIDs"] : "") };

            this.ServiceBag.Remove("FillingCartonIDs");

            return objectParameters;
        }


        public IList<FillingPallet> GetFillingPallets(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs)
        {
            return this.fillingPalletRepository.GetFillingPallets(fillingLineID, entryStatusIDs);
        }

        public bool UpdateEntryStatus(string fillingPalletIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            try
            {
                this.fillingPalletRepository.UpdateEntryStatus(fillingPalletIDs, barcodeStatus);
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
