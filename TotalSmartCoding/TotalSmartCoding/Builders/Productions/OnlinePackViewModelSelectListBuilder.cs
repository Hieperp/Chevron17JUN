using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Builders.Productions
{
    public interface IOnlinePackViewModelSelectListBuilder : IViewModelSelectListBuilder<OnlinePackViewModel>
    {
    }

    public class OnlinePackViewModelSelectListBuilder : IOnlinePackViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(OnlinePackViewModel onlinePackViewModel)
        {
        }
    }
}