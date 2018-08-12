using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DG;


namespace QoL_Mods
{
    [GroupDescription(Description = "This is a collection of Quality-of-Life options, aiming to improve the experience by providing features that would be nice to have or building upon existing game options.", Group = "ExtraFeatures", Name = "Extra Game Options")]
    [FieldAccess(Class = "Audience", Field = "TensionUp", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Audience", Field = "TensionDown", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Audience", Field = "CheerLevel_Base", Group = "ExtraFeatures")]
    class CommentaryOptions
    {
        private static List<String> excitementLevels;

        public static void Initialize()
        {
            SetExcitementLevels();
        }
        [Hook(TargetClass = "Audience", TargetMethod = "TensionUp", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void ExcitementIncrease(Audience inst)
        {
            if(QoL_Form.form.cb_comment.Checked)
            {
                //SetExcitementLevels();
                DispNotification.inst.Show("The Audience is " + GetExcitementLevel(inst.CheerLevel_Base) + "!");
            }
            
        }
        [Hook(TargetClass = "Audience", TargetMethod = "TensionDown", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void ExcitementDecrease(Audience inst)
        {
            if (QoL_Form.form.cb_comment.Checked)
            {
                SetExcitementLevels();
                DispNotification.inst.Show("The Audience is " + GetExcitementLevel(inst.CheerLevel_Base) + "!");
            }
        }

        public static void SetExcitementLevels()
        {
            excitementLevels = new List<string>();
            excitementLevels.Add("irate and preparing to abandon the show");
            excitementLevels.Add("upset by what they're watching");
            excitementLevels.Add("annoyed by the action thus far");
            excitementLevels.Add("bored with the match");
            excitementLevels.Add("neutral towards the match");
            excitementLevels.Add("showing interest in the match");
            excitementLevels.Add("impressed by the action thus far");
            excitementLevels.Add("excited by what they're watching");
            excitementLevels.Add("on the edge of their seats");

        }

        public static String GetExcitementLevel(int cheerLevel)
        {
            try
            {
                return excitementLevels[cheerLevel + 4];
            }

            catch(IndexOutOfRangeException ex)
            {
                return "Error - " + cheerLevel;
            }
        }

        [ControlPanel(Group = "ExtraFeatures")]
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
