using System.Windows.Forms;
using DG;
using UnityEngine;

namespace QoL_Mods
{
    [FieldAccess(Class = "DLCChecker", Field = "IsValidDLCID", Group = "WrestlerSearch")]
    [FieldAccess(Class = "DLCChecker", Field = "IsOwner", Group = "WrestlerSearch")]
    [FieldAccess(Class = "DLCChecker", Field = "IsDownloaded", Group = "WrestlerSearch")]
    [FieldAccess(Class = "DLCChecker", Field = "IsInstalled", Group = "WrestlerSearch")]
    class Overrides
    {
        [Hook(TargetClass = "DLCChecker", TargetMethod = "IsValidDLCID", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "WrestlerSearch")]
        public static bool ValidDLCID(out bool result)
        {
            result = true;
            return result;
        }
        [Hook(TargetClass = "DLCChecker", TargetMethod = "IsOwner", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "WrestlerSearch")]
        public static bool ValidOwner(out bool result)
        {
            result = true;
            return result;
        }
        [Hook(TargetClass = "DLCChecker", TargetMethod = "IsDownloaded", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "WrestlerSearch")]
        public static bool ValidDownload(out bool result)
        {
            result = true;
            return result;
        }
        [Hook(TargetClass = "DLCChecker", TargetMethod = "IsInstalled", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "WrestlerSearch")]
        public static bool ValidInstall(out bool result)
        {
            result = true;
            return result;
        }

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
