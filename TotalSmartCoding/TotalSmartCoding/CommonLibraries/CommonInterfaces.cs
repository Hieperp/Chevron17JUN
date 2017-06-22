using System.ComponentModel;
using System.Windows.Forms;

using TotalBase.Enums;

namespace TotalSmartCoding.CommonLibraries
{

    public interface IMergeToolStrip
    {
        ToolStrip ChildToolStrip { get; }// set; }
    }


    public interface ICallToolStrip : INotifyPropertyChanged
    {
        GlobalEnums.TaskID TaskID { get; }

        void Escape();
        void Loading();

        void New();
        void Edit();
        void Save();
        void Delete();

        void Verify();

        void Import();
        void Export();

        void Print(GlobalEnums.PrintDestination printDestination);

        void SearchText(string searchText);


        bool Closable { get; }
        bool Loadable { get; }

        bool Newable { get; }
        bool Editable { get; }
        bool IsDirty { get; }
        bool IsValid { get; }
        bool Deletable { get; }

        bool Verifiable { get; }
        bool Unverifiable { get; }

        bool Printable { get; }

        bool Importable { get; }
        bool Exportable { get; }


        bool Searchable { get; }


        bool ReadonlyMode { get; }
        bool EditableMode { get; }
    }

}
