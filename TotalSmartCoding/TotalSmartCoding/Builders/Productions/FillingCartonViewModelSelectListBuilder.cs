using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Builders.Productions
{
    public interface IFillingCartonViewModelSelectListBuilder : IViewModelSelectListBuilder<FillingCartonViewModel>
    {
    }

    public class FillingCartonViewModelSelectListBuilder : IFillingCartonViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(FillingCartonViewModel fillingCartonViewModel)
        {
        }
    }
}