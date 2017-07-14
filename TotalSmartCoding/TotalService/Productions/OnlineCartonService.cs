using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class OnlineCartonService : GenericService<OnlineCarton, OnlineCartonDTO, OnlineCartonPrimitiveDTO>, IOnlineCartonService
    {
        public OnlineCartonService(IOnlineCartonRepository onlineCartonRepository)
            : base(onlineCartonRepository, null, "OnlineCartonSaveRelative")
        {
        }

        protected override ObjectParameter[] SaveRelativeParameters(OnlineCarton entity, SaveRelativeOption saveRelativeOption)
        {
            ObjectParameter[] baseParameters = base.SaveRelativeParameters(entity, saveRelativeOption); //IMPORTANT: WE SHOULD SET OnlinePackIDs WHEN SaveRelativeOption.Update. WE DON'T CARE OnlinePackIDs WHEN SaveRelativeOption.Undo [SEE STORE PROCEDURE OnlineCartonSaveRelative FOR MORE INFORMATION] 
            return new ObjectParameter[] { baseParameters[0], baseParameters[1], new ObjectParameter("OnlinePackIDs", this.ServiceBag["OnlinePackIDs"] != null ? this.ServiceBag["OnlinePackIDs"] : "") };
        }
    }
}
