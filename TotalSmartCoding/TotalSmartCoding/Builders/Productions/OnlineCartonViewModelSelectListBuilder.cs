using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Builders.Productions
{
    public interface IOnlineCartonViewModelSelectListBuilder : IViewModelSelectListBuilder<OnlineCartonViewModel>
    {
    }

    public class OnlineCartonViewModelSelectListBuilder : IOnlineCartonViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(OnlineCartonViewModel onlineCartonViewModel)
        {
        }
    }
}