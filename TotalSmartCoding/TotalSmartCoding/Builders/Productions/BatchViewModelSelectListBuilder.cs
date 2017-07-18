using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Builders.Productions
{
    public interface IBatchViewModelSelectListBuilder : IViewModelSelectListBuilder<BatchViewModel>
    {
    }

    public class BatchViewModelSelectListBuilder : IBatchViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(BatchViewModel batchViewModel)
        {
        }
    }
}