using System.Windows.Forms;
using System.ComponentModel;

using TotalBase.Enums;

namespace TotalSmartCoding.Libraries.Helpers
{
    public interface IToolstripMerge
    {
        ToolStrip toolstripChild { get; }
    }


    public interface IToolstripChild : INotifyPropertyChanged
    {
        GlobalEnums.NmvnTaskID NMVNTaskID { get; }

        void Escape();
        void Loading();

        void New();
        void Edit();
        void Save();
        void Delete();

        void Approve();

        void Import();
        void Export();

        void Print(GlobalEnums.PrintDestination printDestination);

        void ApplyFilter(string filterTexts);


        bool Closable { get; }
        bool Loadable { get; }

        bool Newable { get; }
        bool Editable { get; }
        bool IsDirty { get; }
        bool IsValid { get; }
        bool Deletable { get; }

        bool Approvable { get; }
        bool Unapprovable { get; }

        bool Printable { get; }

        bool Importable { get; }
        bool Exportable { get; }


        bool Filterable { get; }


        bool ReadonlyMode { get; }
        bool EditableMode { get; }
    }
}
