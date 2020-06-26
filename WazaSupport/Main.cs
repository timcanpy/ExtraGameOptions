using DG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WazaSupport
{
    [GroupDescription(Group = "Move Craft Support", Name = "Move Craft Support", Description = "Support functionality for Move Craft")]

    [FieldAccess(Class = "Data", Field = "WazaData", Group = "Move Craft Support")]
    [FieldAccess(Class = "ToolSettingInfo", Field = "mData", Group = "Move Craft Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mWaza", Group = "Move Craft Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mData", Group = "Move Craft Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mFileBank", Group = "Move Craft Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mPreview", Group = "Move Craft Support")]
    [FieldAccess(Class = "AnimListData", Field = "dataAccessor", Group = "Move Craft Support")]
    [FieldAccess(Class = "AnimListData", Field = "mRepeatBlock", Group = "Move Craft Support")]
    [FieldAccess(Class = "AnimListData", Field = "mRepeatClickBlock", Group = "Move Craft Support")]
    [FieldAccess(Class = "AnimListData", Field = "m_form", Group = "Move Craft Support")]
    [FieldAccess(Class = "AnimListData", Field = "_buildinOldForm", Group = "Move Craft Support")]
    public class Main
    {
        //Players with access (ids in order)
        //ViewThePhenom
        //DakotaK
        //JamesRah
        //ihaQ

        public static List<String> steamIDs = new List<String>
        {
            "76561198100955117",
            "76561197966612994",
                "76561198451263822",
                    "76561198202550973"
        };

        public static List<String> fp2Moves = new List<String>
        {
            "Alternating Lariats",
            "Arm Trap Chop & Kesagiri Chop",
            "Arm Trap Chop",
            "Avalanche Half-Nelson Suplex",
            "Black Crush",
            "Burning Hammer C. Roll",
            "Burning Lariat C. Axehandle",
            "Burning Lariat",
            "Burning Sword",
            "Chest Chop & Kesagiri Chop 1",
            "Chest Chop & Kesagiri Chop 2",
            "Chest Chop & Kesagiri Chop 3",
            "Clashing Lariats",
            "Dangerous Chop Combination",
            "Dangerous Chop",
            "Diamond Head",
            "Elbow Rush & Back Switch",
            "Elbow Rush",
            "Enzui Fierce Chop",
            "Falling Chop",
            "Fierce Chop & Back Switch",
            "Fierce Chop",
            "Fighting Spirit Backdrop",
            "Fighting Spirit German Suplex",
            "Head Pickup Lariat B",
            "Head Pickup Lariat C. Cyclone Whip",
            "Head Pickup Lariat C. Duck",
            "Head Pickup Lariat C. Exploder",
            "Lifting Head Pickup Lariat",
            "Machinegun Chop Rush A",
            "Machinegun Chop Rush B",
            "Orange Crush B",
            "Pickup Sleeper Suplex",
            "Punching Lariat B",
            "Quick Chop Combo Fierce",
            "Quick Chop Combo Spinning",
            "Rebound Sleeper Suplex",
            "Revenge Chop",
            "Rolling Chop & Back Switch",
            "Rolling Chop",
            "Rolling Chop 2",
            "Three Chop Combination",
            "Top Rope Moonsault Press",
            "Turnbuckle Burning Sword",
            "Wrist Clutch Burning Hammer",
            "Running Phenomenal Kneedrop",
                "Turnbuckle Snap Suplex",
                "Phenomenal Lariat",
                "Pumphandle Gutbuster",
                "Phenomenal Blitz",
                "Torturerack Spinning Bomb",
                "Phenomenal Forearm",
                "Outside Phenomenal Forearm",
                "Slingshot Forearm",
                "Phenomenal DDT",
                "Pele",
                "Phenomenal Blitz Pele",
                "Kip Up Rana",
                "Corner Pele",
                "Back Suplex Pele",
                "Phenomenal Kneedrop",
                "Back Suplex Facebuster",
                "Snap Body Buster",
                "Tiger Body Buster",
                "Avalanche Body Buster",
                "Rolling Body Buster",
                "Deadlift Body Buster",
                "Jackknife Body Buster",
                "R.T. Body Buster",
                "Styles Suplex Combo",
                "Cliffhanger",
                "Spiral Tap",
                "Brainbuster Neckbreaker",
                "Backbreaker & Gutbuster",
                "Reverse Tombstone & Body Buster",
                "Rolling Calf Killer",
                "Calf Killer",
                "Body Buster Combo EX",
                "Body Buster Backdrop Counter",
                "Phenomenal Blitz Forearm Counter",
                "Phenomenal Blitz German Counter",
                "Body Buster Trip Counter",
                "Body Buster Rana Counter",
                "Pele Dodge Counter",
            "Dangerous Monkey Flip",
            "Rebound Cyclone Whip",
            "Slingshot Head-Scissors",
            "Run Up & Apron",
            "Cartwheel Poison-Rana",
            "Somersault Guillotine Drop",
            "Twisting Moonsault Fall",
            "Rolling Senton Atomico",
            "Cartwheel Clutch",
            "Run Up Poison-Rana",
            "Double-Spring Moonsault",
            "Rolling Rana Clutch",
            "Tilta-Tornado-DDT",
            "Molly Go Round",
            "Gori Backslide",
            "Cyclone Backstabber",
            "Swan Dive Swinging Rana",
            "Tiltawhirl Stunner",
            "Arm Whip 2",
            "Arm Whip 3",
            "Arm Wrench Arm Whip",
            "Counter Arm Drag (Powerbomb)",
            "Counter Hammerlock",
            "Cyclone Whip B",
            "Force Chest Slap",
            "Force Chest Slap B",
            "Frankensteiner Whip Reversal",
            "Hammerlock to BackMount",
            "Hammerlock",
            "Headlock (BackMount)",
            "Hesitation Dropkick",
            "Low Dropkick 2",
            "Pick Opp. Up 2",
            "Rollup Pin (FrontGrapple)",
            "Rollup Pin (FrontGrapple) Counter 1",
            "Rollup Pin (FrontGrapple) Counter 2",
            "Rollup Pin (FrontGrapple) Counter 3",
            "Runup Arm Drag",
            "Standing Wristlock",
            "Test of Strength (Body Press)",
            "Test of Strength (Counter)",
            "Test of Strength (Counter Monkey Flip)",
            "Test of Strength (Monkey Flip)",
            "Twisting Wristlock",
            "Tiltawhirl Arm Drag",
            "Twisting Arm Drag",
            "Wristlock Counter 1",
            "Wristlock Counter 2",
            "Wristlock Counter 3",
            "Wristlock Counter 4",
            "Dash Start",
            "Run Up Stunner",
            "Run Up Corbata",
            "Quebradoracongiro",
            "Rolling Hina Tonic",
            "Fake Muscle Revenger",
            "Sobat B",
            "De javu",
            "De javu B",
            "Spread Wing",
            "Strike Back",
            "Back Mount Hammerfists",
            "Back Mount Hammerfists B",
            "Belly-to-Belly B",
            "Body Knees Pin Down",
            "Crown Knee Kick B",
            "Deadlift Gutwrench",
            "Double Leg Slam",
            "Double Leg Slam B",
            "Fireman's Carry Spike",
            "Floatover Body Choke Sleeper",
            "Floatover Cross Armbreaker",
            "Guard Cross Kneebreaker",
            "Gutwrench",
            "Gutwrench B",
            "Judo Throw",
            "Mount Elbows",
            "Mount Elbows B",
            "Mount Hammerfists",
            "Mount Hammerfists B",
            "Pin Down Original Powerbomb",
            "Pinned Omoplata Crossface",
            "Power Double Leg",
            "Power Double Leg B",
            "Pride Soccer Kick",
            "Pride Soccer Kick B",
            "Quick Double Leg",
            "Roll to Guard",
            "Sweeping Hip Toss Armbar",
        };
        
    public static String userID = Steamworks.SteamUser.GetSteamID().ToString();
        //public static long userID = 0;

        #region Move Craft Support

        #region Variables
        public static int maxCFormNum = 99999;
        public static int maxCFormID = 100000 + maxCFormNum;
        #endregion

        [Hook(TargetClass = "WazaMenu_CraftLoadSkill", TargetMethod = "SetActiveBackObj", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "Move Craft Support")]
        public static bool FixPreview(WazaMenu_CraftLoadSkill craftSkill)
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

        [Hook(TargetClass = "WazaMenu_CraftLoadSkill", TargetMethod = "LoadSkillData", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersVal,
            Group = "Move Craft Support")]
        public static void AddCustomForms(WazaMenu_CraftLoadSkill craftSkill, SkillID skill_id)
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
                //Allow moves exported in Move Craft to be processed as well
                if ((int)skill_id >= 6660 && (int)skill_id <= 10000)
                {

                    //Ensure that ONLY moves current in the LoadOrder.dat file can be used.
                    //This version of the tool is meant for converting Byte files into the new format ONLY
                    if (userID.Equals("76561198451263822")) //Covering our bases
                    {
                        if (!IsInLoadOrder(DataBase.GetSkillName(skill_id)))
                        {
                            return;
                        }
                    }

                    //Ensure that FP2 Move Pack release moves cannot be used
                    if (IsFp2Move(DataBase.GetSkillName(skill_id)))
                    {
                        return;
                    }

                    int index = craftSkill.mFileBank.GetSelecting();
                    var saveList = craftSkill.mData.WazaData[index].toolFormSaveList;

                    SkillData[] skillData = SkillDataMan.inst.GetSkillData(skill_id);
                    L.D("Adding custom forms for " + DataBase.GetSkillName(skill_id));
                    foreach (var skill in skillData)
                    {
                        L.D("Total animation indices: " + skill.anmNum);
                        for (int i = 0; i < skill.anmNum; i++)
                        {
                            //L.D("Checking Index " + i);
                            var anmData = skill.anmData[i];
                            for (int j = 0; j < anmData.formNum; j++)
                            {
                                var formDispList = anmData.formDispList[j];

                                //Ensure that the Custom ID matches the Preset ID
                                int formIdx = craftSkill.mData.WazaData[index].anmData[i].formDispList[j].formIdx;

                                //Necessary to ensure that the upper limit for created custom forms is properly set.
                                if (formIdx > craftSkill.mData.WazaData[index].formEditIdx)
                                {
                                    craftSkill.mData.WazaData[index].formEditIdx = formIdx;
                                }


                                //All custom forms begin at 100000
                                //Ensure that we are loading existing custom form indexes, where applicable
                                if (formIdx < 100000)
                                {
                                    formIdx += 100000;
                                }

                                ToolFormSaveData saveData =
                                    new ToolFormSaveData(formDispList.formPartsList, formIdx);
                                saveList.Add(saveData);

                                //saveList[formIdx] = saveData;
                            }
                        }
                    }

                    if (craftSkill.mData.WazaData[index].formEditIdx + 5 <= maxCFormID)
                    {
                        craftSkill.mData.WazaData[index].formEditIdx += 5;
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("AddCustomFormsException: " + ex);
            }
        }

        //Ensure that we can navigate beyond the hard-coded upper limit
        [Hook(TargetClass = "AnimListData", TargetMethod = "AddValue", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn | HookInjectFlags.PassParametersVal,
            Group = "Move Craft Support")]
        public static bool IncreaseMaxCFormLimit(AnimListData animListData, out bool result, string name, int diff, bool pad, bool self, ref int repeatBlock)
        {
            
            result = false;

            //Ensure that only players in the ID list have access to Custom Fomrs
            if (!steamIDs.Contains(userID))
            {
                L.D("User does not have access");
                return false;
            }

            bool change = false;

            L.D(name);
            try
            {
                bool check = true;
                int _bank = global::AnimBankSetting.Context.bank.number;
                int anmBankNo = global::AnimBankSetting.Context.GetSelectAnimNo();
                int selectMin = global::AnimList.Context.selectMin;
                int selectMax = global::AnimList.Context.selectMax;

                List<String> validFormOps = new List<String> { "FORM1", "FORM10", "FORM100", "FORM1000", "FORM10000" };
                if (!validFormOps.Contains(name))
                {
                    return result;
                }
                else if (validFormOps.Contains(name))
                {
                    int preForm = animListData.GetFORM();
                    int max = (preForm < 100000) ? global::AnimListConst.FORMMax : ((animListData.dataAccessor.GetWazaData().formEditIdx <= maxCFormNum) ? (animListData.dataAccessor.GetWazaData().formEditIdx + 100000) : maxCFormID);
                    L.D("Max: " + max);
                    L.D("Diff: " + diff);

                    //Get value for the _form calculation
                    string formValue = name.Substring(4);
                    Int32.TryParse(formValue, out int value);

                    int _form = global::AnimListData.CalcFormNo(preForm, diff * value, self, pad, max, ref animListData.mRepeatBlock, ref animListData.mRepeatClickBlock, ref repeatBlock);

                    L.D("Form Number: " + _form);

                    global::CommandManager.Instance.Do(new global::Command(delegate
                    {
                        change = animListData.SetFORM(_form, true);
                        if (diff > 0 || max == _form)
                        {
                            check = false;
                        }
                        if (change)
                        {
                            global::FormNumber.Instance.CheckDeleteInitData(preForm, _form);
                            animListData.SetFormTextColor(animListData.GetFormTextColor());
                            animListData.DataUpdateFORM();
                            animListData.SetupForm();
                        }
                    }, delegate
                    {
                        global::AnimList.Context.selectMin = selectMin;
                        global::AnimList.Context.selectMax = selectMax;
                        global::AnimList.Instance.RefreshSelect();
                        global::AnimBankSetting.Context.bank.number = _bank;
                        global::AnimBankSetting.Context.bank.no[_bank] = anmBankNo;
                        animListData.m_form = _form;
                        animListData.SetupForm();
                        change = animListData.SetFORM(preForm, true);
                        if (change)
                        {
                            animListData.SetFormTextColor(animListData.GetFormTextColor());
                            animListData.DataUpdateFORM();
                            animListData.SetupForm();
                        }
                    }, delegate
                    {
                        global::AnimList.Context.selectMin = selectMin;
                        global::AnimList.Context.selectMax = selectMax;
                        global::AnimList.Instance.RefreshSelect();
                        global::AnimBankSetting.Context.bank.number = _bank;
                        global::AnimBankSetting.Context.bank.no[_bank] = anmBankNo;
                        animListData.m_form = preForm;
                        animListData.SetupForm();
                        change = animListData.SetFORM(_form, true);
                        if (change)
                        {
                            global::FormNumber.Instance.CheckDeleteInitData(preForm, _form);
                            animListData.SetFormTextColor(animListData.GetFormTextColor());
                            animListData.DataUpdateFORM();
                            animListData.SetupForm();
                        }
                    }));
                }
                if (check)
                {
                    global::FormNumber.Instance.CheckAddEditIdx();
                }
                if (change)
                {
                    animListData.dataAccessor.GetWazaData().changeData = true;
                    global::Menu_SoundManager.Play_SE(global::Menu_SoundManager.SYSTEM_SOUND.CURSOR, global::Menu_SoundManager.PLAY_TYPE.ONCE, 1f);
                }
            }
            catch (Exception e)
            {
                L.D("IncreaseMaxCFormLimitException: " + e);
                return result;
            }

            result = change;
            return result;

        }


        public static bool IsInLoadOrder(String name)
        {

            String loadOrderPath = Path.Combine(Directory.GetCurrentDirectory(), "NewMoves"); ;
            loadOrderPath = Path.Combine(loadOrderPath, "LoadOrder.dat");
            if (File.Exists(loadOrderPath))
            {
                var installedMoves = File.ReadAllLines(loadOrderPath);
                foreach (var line in installedMoves)
                {
                    var items = line.Split('/');
                    var move = items[items.Length - 1].Replace(".move", "");
                    if (name.Equals(move))
                    {
                        L.D(name + " is valid");
                        return true;
                    }
                }
            }

            L.D(name + " is invalid");
            return false;
        }

        public static bool IsFp2Move(String name)
        {
            if (fp2Moves.Contains(name))
            {
                L.D(name + " cannot be used");
                return true;
            }

            return false;
        }

        #endregion
    }
}
