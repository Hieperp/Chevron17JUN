using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Builders.Productions
{
    public interface IFillingPackViewModelSelectListBuilder : IViewModelSelectListBuilder<FillingPackViewModel>
    {
    }

    public class FillingPackViewModelSelectListBuilder : IFillingPackViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(FillingPackViewModel fillingPackViewModel)
        {
        }
    }
}