using System.Windows.Forms;
using System.Collections.Generic;

namespace TotalSmartCoding.Libraries.Helpers
{
    public class ViewHelpers
    {
        public static List<Control> GetAllControls(Control controlContainer, List<Control> controlList)
        {
            foreach (Control control in controlContainer.Controls)
            {
                controlList.Add(control);
                if (control.Controls.Count > 0) controlList = GetAllControls(control, controlList);
            }

            return controlList;
        }
        public static List<Control> GetAllControls(Control controlContainer)
        {
            return GetAllControls(controlContainer, new List<Control>());
        }

    }
}
