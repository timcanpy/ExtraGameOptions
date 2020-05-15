using DG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WazaSupport
{
    [GroupDescription(Group = "Move Craft Support", Name = "Move Craft Support", Description = "Support functionality for Move Craft")]

    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mWaza", Group = "Move Craft Support")]
    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mData", Group = "Move Craft Support")]
    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mFileBank", Group = "Move Craft Support")]
    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mPreview", Group = "Move Craft Support")]
    [FieldAccess(Class = "Data", Field = "WazaData", Group = "Move Craft Support")]
    [FieldAccess(Class = "ToolSettingInfo", Field = "mData", Group = "Move Craft Support")]
    public class Main
    {
        //Players with access (ids in order)
        //ViewThePhenom

        public static List<String> steamIDs = new List<String>
        {
            "76561198100955117",
            "76561197966612994"
        };

        public static String userID = Steamworks.SteamUser.GetSteamID().ToString();
        //public static long userID = 0;

        #region Move Craft Support
        [Hook(TargetClass = "Menu_CraftLoadSkill", TargetMethod = "SetActiveBackObj", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "Move Craft Support")]
        public static bool FixPreview(Menu_CraftLoadSkill craftSkill)
        {
            if (craftSkill.mPreview == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        [Hook(TargetClass = "Menu_CraftLoadSkill", TargetMethod = "LoadSkillData", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersVal,
            Group = "Move Craft Support")]
        public static void AddCustomForms(Menu_CraftLoadSkill craftSkill, SkillID skill_id)
        {
            try
            {
                //Ensure that only players in the ID list have access to Custom Fomrs
                if (!steamIDs.Contains(userID))
                {
                    L.D("User does not have access");
                    return;
                }
                //Ensure that only Custom Moves are processed
                if ((int)skill_id >= 6660 && (int)skill_id <= 10000)
                {

                    int index = craftSkill.mFileBank.GetSelecting();
                    var saveList = craftSkill.mData.WazaData[index].toolFormSaveList;
                    SkillData[] skillData = SkillDataMan.inst.GetSkillData(skill_id);
                    L.D("Adding custom forms for " + DataBase.GetSkillName(skill_id));
                    foreach (var skill in skillData)
                    {
                        for (int i = 0; i < skill.anmNum; i++)
                        {
                            var anmData = skill.anmData[i];
                            for (int j = 0; j < anmData.formNum; j++)
                            {
                                var formDispList = anmData.formDispList[j];
                                //Ensure that the Custom ID matches the Preset ID
                                //int formIdx = formDispList.formIdx;
                                int formIdx = craftSkill.mData.WazaData[index].anmData[i].formDispList[j].formIdx;

                                ToolFormSaveData saveData =
                                    new ToolFormSaveData(formDispList.formPartsList, formIdx + 100000); //All custom forms begin at 100000
                                saveList.Add(saveData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("AddCustomFormsException: " + ex);
            }
        }

        #endregion
    }
}
