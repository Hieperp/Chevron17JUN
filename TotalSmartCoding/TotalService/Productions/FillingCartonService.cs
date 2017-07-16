using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class FillingCartonService : GenericService<FillingCarton, FillingCartonDTO, FillingCartonPrimitiveDTO>, IFillingCartonService
    {
        public FillingCartonService(IFillingCartonRepository fillingCartonRepository)
            : base(fillingCartonRepository, null, "FillingCartonSaveRelative")
        {
        }

        protected override ObjectParameter[] SaveRelativeParameters(FillingCarton entity, SaveRelativeOption saveRelativeOption)
        {
            ObjectParameter[] baseParameters = base.SaveRelativeParameters(entity, saveRelativeOption); //IMPORTANT: WE SHOULD SET FillingPackIDs WHEN SaveRelativeOption.Update. WE DON'T CARE FillingPackIDs WHEN SaveRelativeOption.Undo [SEE STORE PROCEDURE FillingCartonSaveRelative FOR MORE INFORMATION] 
            return new ObjectParameter[] { baseParameters[0], baseParameters[1], new ObjectParameter("FillingPackIDs", this.ServiceBag["FillingPackIDs"] != null ? this.ServiceBag["FillingPackIDs"] : "") };
        }
    }
}
