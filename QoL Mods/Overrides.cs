using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DG;
using MatchConfig;
using QoL_Mods.Data_Classes;
using UnityEngine;

namespace QoL_Mods
{
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Referee", Field = "mRefereeClash", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "ExtraFeatures")]
    [FieldAccess(Class = "MatchMain", Field = "InitRound", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Referee", Field = "ReturnWrestlers", Group = "ExtraFeatures")]
    [FieldAccess(Class = "PlayerController_AI", Field = "area_InOctagon", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Player", Field = "PostprocessEachState", Group = "ExtraFeatures")]
    [FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Menu_SoundManager", Field = "MyMusic_Play", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Menu_Result", Field = "Set_FinishSkill", Group = "ExtraFeatures")]
    [FieldAccess(Class = "MatchMain", Field = "PrepareEntranceScene", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "MatchMain", Field = "Awake", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "Ring", Field = "InitArena", Group = "ExtraFeatures")]

    #region Entrance Scene Fields
    //[FieldAccess(Class = "EntranceScene", Field = "isS1Rule", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "counter", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "totalTimer", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "isSkipAll", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "isPadControl", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "plCnt", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "effectRoot", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "InitThemeMusic", Group = "ExtraFeatures")]
    //[FieldAccess(Class = "EntranceScene", Field = "ChangeState", Group = "ExtraFeatures")]
    #endregion
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
        public static bool topRopeStun = false;
        public static bool isHeadbuttBleed = false;
        public static bool refIsBumped = false;
        public static bool isStarRating = false;

        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void SetMatchRules()
        {
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;

            if (QoL_Form.form.ma_convertSeconds.Checked)
            {
                isRefBumpOverride = true;
                refBumpValue = 50;
                Int32.TryParse(QoL_Form.form.refBum_minLogic.SelectedItem.ToString(), out refBumpValue);
            }
            else
            {
                isRefBumpOverride = false;
            }

            if (QoL_Form.form.ma_forceTag.Checked && !settings.isTornadoBattle && settings.BattleRoyalKind == BattleRoyalKindEnum.Off)
            {
                isForceTag = true;
                forceTagIdx = new int[8];
                forceTag = new bool[8];
                for (int i = 0; i < 8; i++)
                {
                    forceTagIdx[i] = -1;
                }
                GetTouchValues();
            }
            else
            {
                isForceTag = false;
            }

            if (QoL_Form.form.ma_throwOut.Checked)
            {
                topRopeStun = true;
            }
            else
            {
                topRopeStun = false;
            }

            if (QoL_Form.form.ma_headbutt.Checked)
            {
                isHeadbuttBleed = true;
            }
            else
            {
                isHeadbuttBleed = false;
            }

            if (QoL_Form.form.or_lowRecover.Checked)
            {
                if (isTagMatch())
                {
                    SetLowRecovery();
                }
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

            if (QoL_Form.form.or_referee.Checked)
            {
                settings.RefereeID = 10000 + QoL_Form.form.or_refList.SelectedIndex;
            }

            if (QoL_Form.form.or_default.Checked)
            {
                List<WresIDGroup> wrestlers = MatchConfiguration.LoadWrestlers();
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        if ((int)settings.matchWrestlerInfo[i].wrestlerID > -1 && (int)settings.matchWrestlerInfo[i].wrestlerID <= 1000 && settings.matchWrestlerInfo[i].entry)
                        {
                            String wrestlerName = Enum.GetName(settings.matchWrestlerInfo[i].wrestlerID.GetType(), settings.matchWrestlerInfo[i].wrestlerID);

                            foreach (WresIDGroup wrestler in wrestlers)
                            {
                                String replacementName = wrestler.Name.Replace(" ", "");
                                if (replacementName.ToLower().Equals(wrestlerName.ToLower()))
                                {
                                    settings = MatchConfiguration.AddPlayers(true, (WrestlerID)wrestler.ID, i, 0, settings.matchWrestlerInfo[i].isSecond, 0, settings);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        L.D("Default Change Error on " + i + ": " + ex.Message);
                    }
                }
            }

            if (QoL_Form.form.sr_usage.Checked)
            {
                isStarRating = true;
            }
            else
            {
                isStarRating = false;
            }

            if (QoL_Form.form.ed_ringSettings.Checked)
            {
                string referee = "";
                try
                {

                    //Get current ring name
                    int ringID = settings.ringID - 10000 + 1;
                    String ringName = (String)QoL_Form.form.or_ringList.Items[ringID];

                    foreach (CustomRing ring in QoL_Form.form.rs_customRings.Items)
                    {
                        L.D("Current Ring: " + ring.Name);
                        if (ringName.Equals(ring.Name))
                        {
                            int index = UnityEngine.Random.Range(0, ring.RefereeList.Count - 1);
                            referee = ring.RefereeList.ToArray()[index];
                            L.D("Referee" + referee);
                            break;
                        }
                    }

                    //Set referee
                    if (!referee.Equals(""))
                    {
                        int refIndex = QoL_Form.form.or_refList.Items.IndexOf(referee);
                        if (refIndex >= 0)
                        {
                            settings.RefereeID = 10000 + refIndex;
                        }
                    }
                }
                catch
                {
                    L.D("Index of " + referee + ": " + QoL_Form.form.or_refList.Items.IndexOf(referee));
                }
            }
        }

        [Hook(TargetClass = "Menu_SoundManager", TargetMethod = "MyMusic_Play", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void UpdateMusic()
        {
            if (!QoL_Form.form.or_bgm.Checked && !QoL_Form.form.ed_ringSettings.Checked)
            {
                return;
            }

            string matchBGM = "";
            string bgmPath = System.IO.Directory.GetCurrentDirectory() + @"\BGM";

            if (QoL_Form.form.or_bgm.Checked)
            {
                matchBGM = bgmPath + @"\" + QoL_Form.form.or_bgmList.SelectedItem.ToString();
            }
            else
            {
                try
                {
                    int index = UnityEngine.Random.Range(0, ((CustomRing)QoL_Form.form.rs_customRings.SelectedItem).ThemeList.Count - 1);
                    matchBGM = bgmPath + @"\" + QoL_Form.form.rs_customThemes.Items[index];
                }
                catch (NullReferenceException ex)
                {
                    return;
                }
            }
            try
            {
                //Force change the Match BGM; this implementation allows the theme to be changed before each new match
                global::Menu_SoundManager.MyMusic_SelectFile_Match = matchBGM;
            }
            catch (Exception ex)
            {
                L.D("Change Music Exception: " + ex.Message + "\nMatch Bgm: " + matchBGM);
            }

        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "ExtraFeatures")]
        public static void SetResultScreenDisplay(ref UILabel finishText, string str)
        {
            if (!isStarRating)
            {
                return;
            }

            int num = global::MatchEvaluation.GetInst().EvaluateMatch();

            //Determine if any wrestlers influence the rating
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (!plObj)
                {
                    continue;
                }

                String wrestler = DataBase.GetWrestlerFullName(plObj.WresParam);
                foreach (String likedWrestler in QoL_Form.form.sr_likedList.Items)
                {
                    if (wrestler.Equals(likedWrestler))
                    {
                        num += 5;
                    }
                }

                foreach (String dislikeWrestler in QoL_Form.form.sr_dislikedList.Items)
                {
                    if (wrestler.Equals(dislikeWrestler))
                    {
                        num -= 10;
                    }
                }
            }
            String starRating = "";

            if (num < 20)
            {
                starRating += "★";
            }
            else if (num < 30)
            {
                starRating += "★☆";
            }
            else if (num < 40)
            {
                starRating += "★★";
            }
            else if (num < 50)
            {
                starRating += "★★☆";
            }
            else if (num < 60)
            {
                starRating += "★★★";
            }
            else if (num < 70)
            {
                starRating += "★★★☆";
            }
            else if (num < 80)
            {
                starRating += "★★★★";
            }
            else if (num < 90)
            {
                starRating += "★★★★☆";
            }
            else if (num < 100)
            {
                starRating += "★★★★★";
            }
            else if (num < 110)
            {
                starRating += "★★★★★☆";
            }
            else if (num < 120)
            {
                starRating += "★★★★★★";
            }
            else if (num < 130)
            {
                starRating += "★★★★★★☆";
            }
            else
            {
                starRating += "★★★★★★★";
            }

            finishText.text = starRating + "\n\n" + finishText.text;
        }

        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "ExtraFeatures")]
        public static void IncreaseDownTime(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (!QoL_Form.form.ma_downtime.Checked)
            {
                return;
            }
            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

            if (defender.isCriticalMoveRecieved)
            {
                defender.DownTime += 300;
            }
        }

        #region Enable Yurakuen Entrances
        //[Hook(TargetClass = "MatchMain", TargetMethod = "Awake", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        //public static void AllowYurakuenEntrances(MatchMain main)
        //{
        //    MatchSetting settings = GlobalWork.GetInst().MatchSetting;
        //    global::VenueSetting venueSetting = global::Ring.inst.venueSetting;
        //    if (QoL_Form.form.en_yurakuen.Checked && !settings.isSkipEntranceScene && settings.arena == VenueEnum.YurakuenHall)
        //    {
        //        main.PrepareEntranceScene();
        //        main.State = global::MatchMain.StateEnum.ReqEntranceScene;

        //    }
        //}

        //[Hook(TargetClass = "Ring", TargetMethod = "InitArena", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        //public static void SetEntranceType()
        //{

        //    MatchSetting settings = GlobalWork.GetInst().MatchSetting;
        //    global::VenueSetting venueSetting = global::Ring.inst.venueSetting;
        //    if (QoL_Form.form.en_yurakuen.Checked && !settings.isSkipEntranceScene && settings.arena == VenueEnum.YurakuenHall)
        //    {
        //        venueSetting.entranceSceneKind = EntranceSceneKind.SCSStadium;
        //    }
        //}

        //[Hook(TargetClass = "EntranceScene", TargetMethod = "StartEntranceScene", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "ExtraFeatures")]
        //public static bool SetYurakuenEntrance(EntranceScene scene)
        //{
        //    try
        //    {
        //        MatchSetting settings = GlobalWork.GetInst().MatchSetting;
        //        global::VenueSetting venueSetting = global::Ring.inst.venueSetting;
        //        if (QoL_Form.form.en_yurakuen.Checked && !settings.isSkipEntranceScene && settings.arena == VenueEnum.YurakuenHall)
        //        {

        //            //Get Player List
        //            scene.plIdxList = GetPlayerList();

        //            global::MatchSetting matchSetting = global::GlobalWork.inst.MatchSetting;
        //            scene.isS1Rule = matchSetting.isS1Rule;
        //            matchSetting.isS1Rule = false;
        //            scene.kind = EntranceSceneKind.SCSStadium;
        //            scene.counter = 0;
        //            scene.totalAnmCnt = 0;
        //            scene.totalTimer = 0;
        //            scene.isSkipAll = false;
        //            scene.isFadeOut = false;
        //            scene.isPadControl = false;
        //            scene.plNum = scene.plIdxList.Length;
        //            scene.wrestlerIDList = new global::WrestlerID[scene.plNum];
        //            scene.plPadControl = new bool[scene.plNum];
        //            if (scene.plIdxList[0] < 4)
        //            {
        //                scene.cornerSide = global::CornerSide.Blue;
        //            }
        //            else
        //            {
        //                scene.cornerSide = global::CornerSide.Red;
        //            }
        //            scene.plCnt = scene.plIdxList.Count();
        //            for (int i = 0; i < scene.plNum; i++)
        //            {
        //                int num = scene.plIdxList[i];
        //                global::Player plObj = global::PlayerMan.inst.GetPlObj(num);
        //                plObj.SetSleep(false);
        //                plObj.Start_ForceControl(global::ForceCtrlEnum.WaitMatchStart);
        //                plObj.PlPos = new Vector3(-1.07f, 3.38f, 1f);
        //                plObj.Zone = global::ZoneEnum.SlopeRunway;
        //                global::MatchWrestlerInfo matchWrestlerInfo = matchSetting.matchWrestlerInfo[num];
        //                scene.wrestlerIDList[i] = matchWrestlerInfo.wrestlerID;
        //            }
        //            global::MatchCamera.inst.SetValid(false);
        //            global::EntranceSceneCamera.inst.SetValid(true);
        //            if (scene.effectRoot)
        //            {
        //                scene.effectRoot.SetActive(true);
        //            }
        //            if (scene.oneShotEffect)
        //            {
        //                scene.oneShotEffect.SetActive(false);
        //            }
        //            global::Referee refereeObj = global::RefereeMan.inst.GetRefereeObj();
        //            if (refereeObj != null)
        //            {
        //                refereeObj.PlPos = new Vector3(-1.07f, 3.38f, 1f);
        //            }
        //            Carlzilla.MusicDatabaseHook(scene);
        //            Carlzilla.HoRemoval();
        //            scene.InitThemeMusic();
        //            global::Menu_SoundManager.Play_BGM(global::Menu_SoundManager.SYSTEM_SOUND.BGM_ADMISSION, global::Menu_SoundManager.PLAY_TYPE.LOOP);
        //            global::Audience.inst.SetBaseCheerLevel(4);
        //            global::Audience.inst.PlayLoopCheerVoice(4, true);
        //            global::Fade.inst.FadeIn(30);
        //            scene.ChangeState(global::EntranceScene.StateEnum.Wait_RingIn);
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        L.D(ex.Message);
        //        return false;
        //    }
        //}
        #endregion

        #region Referee Bump Logic
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void StartMatch()
        {
            if (isRefBumpOverride)
            {
                StoreSeconds();
                refIsBumped = false;
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "mRefereeClash", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void ProcessRefBump()
        {
            if (isRefBumpOverride)
            {
                ConvertSeconds(true);
                refIsBumped = true;
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "ReturnWrestlers", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void ForceSecondOut(Referee mref)
        {
            if (isRefBumpOverride)
            {
                if (mref.State != RefeStateEnum.R_DOWN && mref.State != RefeStateEnum.StartToDown && refIsBumped)
                {
                    ConvertSeconds(false);
                    refIsBumped = false;
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
                        continue;
                    }

                    Player tagPartner = PlayerMan.inst.GetPlObj(i);
                    if (!tagPartner)
                    {
                        continue;
                    }
                    if (tagPartner.animator.AnmReqType == AnmReqTypeEnum.BasicSkill && (tagPartner.animator.BasicSkillID == BasicSkillEnum.CallPartner_U || tagPartner.animator.BasicSkillID == BasicSkillEnum.CallPartner_D))
                    {
                        //Set details on which partner requested a tag
                        if (!forceTag[plObj.PlIdx])
                        {
                            forceTag[plObj.PlIdx] = true;
                            forceTagIdx[plObj.PlIdx] = tagPartner.PlIdx;
                            return;
                        }
                    }

                }
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
                        //Determine if anyone requested a tag
                        if (forceTag[ai.plIdx])
                        {

                            if (ExecuteTag(ai))
                            {
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

            result = false;
            return false;
        }

        #endregion
        #region Toss Out Logic
        [Hook(TargetClass = "Player", TargetMethod = "PostprocessEachState", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void AddStunTime(Player plObj)
        {
            if (!topRopeStun)
            {
                return;
            }
            if (!GlobalWork.inst.MatchSetting.isOverTheTopRopeOn)
            {
                if (plObj.Zone == ZoneEnum.OutOfRing && plObj.OTR_Lose)
                {
                    plObj.isStandingStunOK = true;
                    plObj.AddStunTime(360);
                    plObj.OTR_Lose = false;
                }
            }
        }
        #endregion
        #region Headbutt Logic
        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "ExtraFeatures")]
        public static void CheckHeadbutt(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (!isHeadbuttBleed)
            {
                return;
            }

            if (sd.skillName[1].ToLower().Contains("headbutt"))
            {
                if (sd.bleedingRate > 0)
                {
                    int rngValue = UnityEngine.Random.Range(0, 100);
                    if (rngValue <= sd.bleedingRate)
                    {
                        PlayerMan.inst.GetPlObj(plIDx).Bleeding();
                    }
                }
            }

        }
        #endregion  
        #region Helper Methods
        public static int[] GetPlayerList()
        {
            List<int> players = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (plObj)
                {
                    players.Add(plObj.PlIdx);
                }
            }

            return players.ToArray();
        }
        public static bool ExecuteTag(PlayerController_AI ai)
        {
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
                    //plObj.hasRight = false;
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
                //plObj.hasRight = true;
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
        public static bool isTagMatch()
        {
            bool isTag = false;
            int memberCount = 0;
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);

                if (!plObj)
                {
                    continue;
                }

                if (plObj.isSecond)
                {
                    continue;
                }

                memberCount++;

            }

            if (memberCount > 2)
            {
                if (!GlobalWork.GetInst().MatchSetting.isTornadoBattle)
                {
                    isTag = true;
                }
            }

            return isTag;
        }

        public static void SetLowRecovery()
        {
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                plObj.WresParam.hpRecovery = 0;
                plObj.WresParam.spRecovery = 0;
                plObj.WresParam.hpRecovery_Bleeding = 0;
                plObj.WresParam.spRecovery_Bleeding = 0;
            }
        }
        #endregion
    }
}
