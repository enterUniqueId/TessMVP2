using System.Windows.Forms;

namespace TessMVP2.View.Interfaces
{
    interface IMyViewFormCompareContacts
    {
        Form Form3 { get; }
        Button BtnUpdate { get; set; }
        Button BtnCreateNew { get; set; }
    }
}
