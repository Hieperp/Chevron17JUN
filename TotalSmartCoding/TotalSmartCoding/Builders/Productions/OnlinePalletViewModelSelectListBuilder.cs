using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Builders.Productions
{
    public interface IOnlinePalletViewModelSelectListBuilder : IViewModelSelectListBuilder<OnlinePalletViewModel>
    {
    }

    public class OnlinePalletViewModelSelectListBuilder : IOnlinePalletViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(OnlinePalletViewModel onlinePalletViewModel)
        {
        }
    }
}