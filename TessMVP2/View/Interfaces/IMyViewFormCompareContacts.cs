using System.Windows.Forms;

namespace TessMVP2.View.Interfaces
{
    interface IMyViewFormCompareContacts
    {
        Form Form3 { get; }
        Button BtnUpdate { get; set; }
        Button BtnCreateNew { get; set; }
        Button BtnCancel { get; set; }
        ContextMenu F3CmName { get; set; }
        ContextMenu F3CmTelefonNummer { get; set; }
        ContextMenu F3CmTelefonNummer2 { get; set; }
        ContextMenu F3CmMobilNummer { get; set; }
        ContextMenu F3CmFaxNummer { get; set; }
        ContextMenu F3CmStrasse { get; set; }
        ContextMenu F3CmPostleitzahl { get; set; }
        ContextMenu F3CmOrt { get; set; }
        ContextMenu F3CmPostfach { get; set; }
        ContextMenu F3CmPosition { get; set; }
        ContextMenu F3CmHomepage { get; set; }
        ContextMenu F3CmFirma { get; set; }
        ContextMenu F3CmEmail { get; set; }
        ContextMenu F3CmEmail2 { get; set; }
        ContextMenu F3CmEmail3 { get; set; }
    }
}
