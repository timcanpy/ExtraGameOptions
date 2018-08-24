using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DG;
using MatchConfig;

namespace QoL_Mods
{

    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Referee", Field = "mRefereeClash", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "ExtraFeatures")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Referee", Field = "ReturnWrestlers", Group = "ExtraFeatures")]
    [FieldAccess(Class = "PlayerController_AI", Field = "area_InOctagon", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Menu_Title", Field = "Awake", Group = "ExtraFeatures")]
    class Overrides
    {
        #region Variables
        public static bool isRefBumpOverride = false;
        public static List<int> secondList = new List<int>();
        public static int[] touchArray;
        public static bool isForceTag = false;
        public static int refBumpValue;
        public static bool[] forceTag;
        public static int[] forceTagIdx;
        public static float[,] tagPosition = new float[,]
        {
            {
                0f,
                0f
            },
            {
                0f,
                0f
            },
            {
                0.833333254f,
                1.66666651f
            },
            {
                0.833333254f,
                1.66666651f
            },
            {
                -1.66666651f,
                -0.833333254f
            },
            {
                -1.66666651f,
                -0.833333254f
            },
            {
                0.833333254f,
                1.66666651f
            },
            {
                0.833333254f,
                1.66666651f
            }
        };
        public static float[] tagArea = new float[]
            {
            0f,
            0f,
            -0.2708333f,
            -0.2708333f,
            0.2708333f,
            0.2708333f,
            -0.2708333f,
            -0.2708333f
            };
        #endregion

        [Hook(TargetClass = "Menu_Title", TargetMethod = "Awake", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None)]
        public static void FuckYouPirates()
        {
            if (File.Exists("./steam_emu.ini") || File.Exists("./steam_api.cdx"))
            {
                UnityEngine.Application.Quit();
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;

            if (QoL_Form.form.ma_convertSeconds.Checked)
            {
                isRefBumpOverride = true;
                refBumpValue = 50;
                Int32.TryParse(QoL_Form.form.refBum_minLogic.SelectedItem.ToString(), out refBumpValue);
                L.D("Minimum ref bump value: " + refBumpValue);
            }

            if (QoL_Form.form.ma_forceTag.Checked && !settings.isTornadoBattle && settings.BattleRoyalKind == BattleRoyalKindEnum.Off && (GetMemberCount() == 3 || GetMemberCount() == 4))
            {
                isForceTag = true;
                forceTagIdx = new int[8];
                forceTag = new bool[8];
                for (int i = 0; i < 8; i++)
                {
                    forceTagIdx[i] = -1;
                }
                GetTouchValues();
                L.D("Member count: " + GetMemberCount());
            }

            //Override match settings
            try
            {


                if (QoL_Form.form.or_ring.Checked)
                {
                    settings.ringID = 10000 + QoL_Form.form.or_ringList.SelectedIndex - 1;
                }
            }
            catch (Exception ex)
            {
                L.D("Ring Overrride Error: " + ex.Message);
            }
            if (QoL_Form.form.or_venue.Checked)
            {
                settings.arena = (VenueEnum)QoL_Form.form.or_venueList.SelectedIndex;
            }

            if (QoL_Form.form.or_default.Checked)
            {
                List<WresIDGroup> wrestlers = MatchConfiguration.LoadWrestlers();
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        //if (Enum.IsDefined(typeof(WrestlerID), settings.matchWrestlerInfo[i].wrestlerID))
                        if ((int)settings.matchWrestlerInfo[i].wrestlerID > -1 && (int)settings.matchWrestlerInfo[i].wrestlerID <= 1000 && settings.matchWrestlerInfo[i].entry)
                        {
                            String wrestlerName = Enum.GetName(settings.matchWrestlerInfo[i].wrestlerID.GetType(), settings.matchWrestlerInfo[i].wrestlerID);

                            L.D("Replacing " + wrestlerName);
                            foreach (WresIDGroup wrestler in wrestlers)
                            {
                                String replacementName = wrestler.Name.Replace(" ","");
                                if (replacementName.ToLower().Equals(wrestlerName.ToLower()))
                                {
                                    L.D("Replacement found");
                                    settings = MatchConfiguration.AddPlayers(true, (WrestlerID)wrestler.ID, i, 0, settings.matchWrestlerInfo[i].isSecond, 0, settings);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        L.D("Default Change Error on " + i + ": " + ex.Message);
                    }
                }
            }
        }

        #region Referee Bump Logic
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void StartMatch()
        {
            if (isRefBumpOverride)
            {
                StoreSeconds();
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "mRefereeClash", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void ProcessRefBump()
        {
            if (isRefBumpOverride)
            {
                ConvertSeconds(true);
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "ReturnWrestlers", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void ForceSecondOut(Referee mref)
        {
            if (isRefBumpOverride)
            {
                if (mref.State != RefeStateEnum.R_DOWN && mref.State != RefeStateEnum.StartToDown)
                {
                    ConvertSeconds(false);
                }
            }
        }
        #endregion
        #region Force Tag Logic
        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void ForceTag(Player plObj)
        {
            if (isForceTag)
            {
                if (!plObj.hasRight || plObj.isSecond)
                {
                    L.D("Ending Tag Call");
                    return;
                }

                int minVal, maxVal;
                if (plObj.PlIdx < 4)
                {
                    minVal = 0;
                    maxVal = 4;
                }
                else
                {
                    minVal = 4;
                    maxVal = 8;
                }
                for (int i = minVal; i < maxVal; i++)
                {
                    if (i == plObj.PlIdx)
                    {
                        L.D("Continue Tag Partner Check");
                        continue;
                    }

                    Player tagPartner = PlayerMan.inst.GetPlObj(i);
                    if (!tagPartner)
                    {
                        L.D("Continue Tag Partner Check");
                        continue;
                    }
                    if (tagPartner.animator.AnmReqType == AnmReqTypeEnum.BasicSkill && (tagPartner.animator.BasicSkillID == BasicSkillEnum.CallPartner_U || tagPartner.animator.BasicSkillID == BasicSkillEnum.CallPartner_D))
                    {
                        L.D("Initiate tag call: Force Tag " + forceTag[plObj.PlIdx]);
                        //Set details on which partner requested a tag
                        if (!forceTag[plObj.PlIdx])
                        {
                            L.D("Processing tag call");
                            forceTag[plObj.PlIdx] = true;
                            forceTagIdx[plObj.PlIdx] = tagPartner.PlIdx;
                            //plObj.plCont_AI.Process_Touch();
                            return;
                        }
                    }
                    L.D("Nothing Done in Tag Call");
                }

                //if (plObj.Zone == ZoneEnum.Apron)
                //{
                //    plObj.WresParam.aiParam.touchCond = touchArray[plObj.PlIdx];
                //}
            }
        }

        [Hook(TargetClass = "PlayerController_AI", TargetMethod = "Process_Touch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "ExtraFeatures")]
        public static bool ForcedTag(PlayerController_AI ai, out bool result)
        {
            try
            {

                if (isForceTag)
                {
                    //Ensure that the ai object is valid
                    if (ai.PlObj.hasRight && ai.PlObj.Zone == global::ZoneEnum.InRing && ai.PlObj.State <= global::PlStateEnum.RecoverBreath)
                    {
                        L.D("Preparing to execute tag call");
                        //Determine if anyone requested a tag
                        if (forceTag[ai.plIdx])
                        {

                            if (ExecuteTag(ai))
                            {

                                L.D("Ending Tag Call: Pass");
                                result = true;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        forceTag[ai.plIdx] = false;
                        forceTagIdx[ai.plIdx] = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("Tag Call Error: " + ex.Message);
            }

            //L.D("Ending Tag Call: Fail");
            result = false;
            return false;
        }

        #endregion
        #region Helper Methods
        public static bool ExecuteTag(PlayerController_AI ai)
        {
            L.D("In ExecuteTag method");
            bool tagComplete = false;
            Player tagPartner = PlayerMan.inst.GetPlObj(forceTagIdx[ai.plIdx]);

            //Ensure the tag partner is valid
            if (tagPartner.isTagPartnerStandby && tagPartner.PlPos.y > tagPosition[(int)tagPartner.TagStandbyPos, 0] && tagPartner.PlPos.y < tagPosition[(int)tagPartner.TagStandbyPos, 1])
            {
                AIParam aiParam = ai.PlObj.WresParam.aiParam;
                AreaEnum areaEnum = ai.area_InOctagon;
                float updatedPosition = tagPartner.PlPos.y + tagArea[(int)tagPartner.TagStandbyPos];
                bool flag = global::Ring.inst.TestCollision_OctagonEdge_InRing(0.0208333321f, ai.PlObj.PlPos);

                //Position active wrestler for tag
                float y = ai.PlObj.PlPos.y;
                tagComplete = true;
                L.D("Updating Wrestler Position");
                switch (tagPartner.TagStandbyPos)
                {
                    case global::TagStandbyPosEnum.LeftCorner_U1:
                    case global::TagStandbyPosEnum.LeftCorner_U2:
                        if (y < updatedPosition - 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_U;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_LU;
                            }
                        }
                        else if (y > updatedPosition + 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_D;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_LD;
                            }
                        }
                        else if (flag && areaEnum == global::AreaEnum.LU)
                        {
                            ai.padOn = global::PadBtnEnum.Dir_LU;
                            ai.padPush = global::PadBtnEnum.Rest;
                            forceTag[ai.plIdx] = false;
                            forceTagIdx[ai.plIdx] = -1;
                            tagComplete = false;
                        }
                        else
                        {
                            ai.padOn = global::PadBtnEnum.Dir_L;
                        }
                        break;
                    case global::TagStandbyPosEnum.RightCorner_U1:
                    case global::TagStandbyPosEnum.RightCorner_U2:
                        if (y < updatedPosition - 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_U;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_RU;
                            }
                        }
                        else if (y > updatedPosition + 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_D;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_RD;
                            }
                        }
                        else if (flag && areaEnum == global::AreaEnum.RU)
                        {
                            ai.padOn = global::PadBtnEnum.Dir_RU;
                            ai.padPush = global::PadBtnEnum.Rest;
                            forceTag[ai.plIdx] = false;
                            forceTagIdx[ai.plIdx] = -1;
                            tagComplete = false;
                        }
                        else
                        {
                            ai.padOn = global::PadBtnEnum.Dir_R;
                        }
                        break;
                    case global::TagStandbyPosEnum.LeftCorner_D:
                        if (y < updatedPosition - 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_U;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_LU;
                            }
                        }
                        else if (y > updatedPosition + 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_D;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_LD;
                            }
                        }
                        else if (flag && areaEnum == global::AreaEnum.LD)
                        {
                            ai.padOn = global::PadBtnEnum.Dir_LD;
                            ai.padPush = global::PadBtnEnum.Rest;
                            forceTag[ai.plIdx] = false;
                            forceTagIdx[ai.plIdx] = -1;
                            tagComplete = false;
                        }
                        else
                        {
                            ai.padOn = global::PadBtnEnum.Dir_L;
                        }
                        break;
                    case global::TagStandbyPosEnum.RightCorner_D:
                        if (y < updatedPosition - 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_U;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_RU;
                            }
                        }
                        else if (y > updatedPosition + 0.0416666642f)
                        {
                            if (flag)
                            {
                                ai.padOn = global::PadBtnEnum.Dir_D;
                            }
                            else
                            {
                                ai.padOn = global::PadBtnEnum.Dir_RD;
                            }
                        }
                        else if (flag && areaEnum == global::AreaEnum.RD)
                        {
                            ai.padOn = global::PadBtnEnum.Dir_RD;
                            ai.padPush = global::PadBtnEnum.Rest;
                            forceTag[ai.plIdx] = false;
                            forceTagIdx[ai.plIdx] = -1;
                            tagComplete = false;
                        }
                        else
                        {
                            ai.padOn = global::PadBtnEnum.Dir_R;
                        }
                        break;
                }
            }

            L.D("Tag Complete: " + tagComplete);
            forceTag[ai.plIdx] = false;
            forceTagIdx[ai.plIdx] = -1;

            return tagComplete;

        }
        public static void ConvertSeconds(bool updateValue)
        {
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            int rngValue = UnityEngine.Random.Range(0, refBumpValue);

            foreach (int i in secondList)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);

                //When the referee rises, all seconds should return to their original state
                if (!updateValue)
                {
                    plObj.isSecond = true;
                    plObj.hasRight = false;
                    plObj.Start_ForceControl(ForceCtrlEnum.SecondStanbdby);
                }
                else
                {
                    //Determine whether the player should be updated as a second
                    if (settings.matchWrestlerInfo[i].assignedPad != PadPort.AI && settings.matchWrestlerInfo[i].assignedPad != PadPort.Invalid)
                    {
                        ActivateAllSeconds();
                        break;
                    }
                    else
                    {
                        int interfereRate = plObj.WresParam.aiParam.secondAggressiveness;

                        if (rngValue <= interfereRate)
                        {
                            ActivateAllSeconds();
                            break;
                        }
                    }
                }
            }
        }
        public static void StoreSeconds()
        {
            secondList = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }
                if (plObj.isSecond)
                {
                    secondList.Add(i);
                }
            }
        }
        public static void ActivateAllSeconds()
        {
            foreach (int i in secondList)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                plObj.isSecond = false;
                plObj.hasRight = true;
            }
        }
        public static Player GetActivePartner(Player plObj)
        {
            Player partner = null;
            if (plObj.PlIdx < 4)
            {

            }
            return partner;
        }
        public static void GetTouchValues()
        {
            touchArray = new int[8];
            for (int i = 0; i < 8; i++)
            {
                Player player = PlayerMan.inst.GetPlObj(i);
                if (!player)
                {
                    continue;
                }

                touchArray[i] = player.WresParam.aiParam.touchCond;
            }
        }
        public static int GetMemberCount()
        {
            int members = 0;
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }
                if (!plObj.isSecond)
                {
                    i++;
                }
            }
            return members;
        }
        #endregion
    }
}

#region Carl's Code
//Player tagPartner = PlayerMan.inst.getPlObj(tagPartnerIdx);
//if (tagPartner.Animator.AnmReqType == AnmReqTypeEnum.BasicSkill && (tagPartner.Animator.BasicSkillID == BasicSkillEnum.CallPartner_U || tagPartner.Animator.BasicSkillID == BasicSkillEnum.CallPartner_D)
//{
//    code to force tag goes here
//}
#endregion