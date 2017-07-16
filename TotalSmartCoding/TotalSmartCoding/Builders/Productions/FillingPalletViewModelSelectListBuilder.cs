using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Builders.Productions
{
    public interface IFillingPalletViewModelSelectListBuilder : IViewModelSelectListBuilder<FillingPalletViewModel>
    {
    }

    public class FillingPalletViewModelSelectListBuilder : IFillingPalletViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(FillingPalletViewModel fillingPalletViewModel)
        {
        }
    }
}