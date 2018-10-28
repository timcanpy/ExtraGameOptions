using System.Windows.Forms;
using DG;
using UnityEngine;
using DLC;

namespace QoL_Mods
{
    [FieldAccess(Class = "DLC.DLCChecker", Field = "IsValidDLCID", Group = "WrestlerSearch")]
    [FieldAccess(Class = "DLC.DLCChecker", Field = "IsOwner", Group = "WrestlerSearch")]
    [FieldAccess(Class = "DLC.DLCChecker", Field = "IsDownloaded", Group = "WrestlerSearch")]
    [FieldAccess(Class = "DLC.DLCChecker", Field = "IsInstalled", Group = "WrestlerSearch")]

    class Overrides
    {
        [ControlPanel(Group = "WrestlerSearch")]
        public static Form MSForm()
        {
            if (QoL_Form.form == null)
            {
                return new QoL_Form();
            }
            {
                return null;
            }
        }
    }
}
