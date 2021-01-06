using System.Windows.Forms;
using DG;
using UnityEngine;
using DLC;
using System;
using System.Collections.Generic;
using QoL_Mods.Data_Classes;
using System.IO;
using QoL_Mods.Helper_Classes;
using QoL_Mods.Public;
using UnityEngine.UI;
using WresIDGroup = ModPack.WresIDGroup;
using ModPack;
using QoL_Mods.Private;
using Ace.AttireExtension;
using QoL_Mods.Data_Classes.Facelock;
using UnityEngine.Windows.Speech;
using System.Reflection;
using MoreMatchTypes;

namespace QoL_Mods
{
    #region Group Descriptions
    [GroupDescription(Group = "Wrestler Search", Name = "Edit Search Tool", Description = "Provides a UI for easily loading edits and referee within Edit Mode.\nNote that any changes to a Referee are automatically saved when exiting Edit Mode.")]
    [GroupDescription(Group = "Low Tag Recovery", Name = "Low Tag Recovery", Description = "Forces tag teams to use low recovery.")]
    [GroupDescription(Group = "Ignore Downtime", Name = "Attacker Ignores Pinfall/Submission Downtime", Description = "Attacker immediately recovers after a pinfall or grounded submission.")]
    [GroupDescription(Group = "Ref Positions For Pinfall", Name = "Referee Behavior Override", Description = "Forces the referee to move towards the active players after big moves performed late in a match. When the referee decides to start moving depends on his Involvement skill.")]
    [GroupDescription(Group = "Resilient Critical", Name = "Critical Resilience", Description = "Gives players a chance to ignore the knock out effects of criticals based on their body part defense. Players receive slight spirit & breathing restoration to remain competitive afterwards.")]
    [GroupDescription(Group = "ChangeCritImage", Name = "Change Critical Image", Description = "Allows players to replace the Critical! graphic with custom images.\n Images should be placed in the Fire Prowrestling World\\EGOData\\Images folder.\n All images must measure 648 x 328 or they will be ignored.")]
    [GroupDescription(Group = "Recovery Taunts", Name = "Recovery Taunt Options", Description = "Allows players to perform recovery taunts when down.\nEach taunt must be categorized as a Performance.\nEach taunt must begin on either form 100 or 101 to be applicable.\nTaunts can end standing or grounded.\nChance of a recovery taunt is based on a player's Showmanship rating.\nPlayers can perform taunts a number of times equal to their (Wrestler Rank + Charisma)/2.")]
    [GroupDescription(Group = "Audience Sounds", Name = "Dynamic Audience Sounds", Description = "Makes the audience use different cheers during a match, instead of the default every time.")]
    [GroupDescription(Group = "2.9Call", Name = "Referee Calls Near Falls", Description = "Makes the referee announce near falls on 2.9 counts\nUses the 'Down Count 2' audio file.")]
    [GroupDescription(Group = "GruntForSubmission", Name = "Edits Sell Holds", Description = "Makes edits play voice lines when under a submission hold.\nFrequency is determined by Showmanship and the current Damage Threshold.")]
    [GroupDescription(Group = "UkeNotification", Name = "Ukemi Trigger Notification", Description = "Plays specific crowd cheers (HolyShit, ThisIsWrestling, Stomping) when a wrestler triggers Ukemi.\nCheers may trigger when a match ends.")]
    [GroupDescription(Group = "Bleeding Headbutts", Name = "Dangerous Headbutts", Description = "Grapple Headbutts can cause self bleeding")]
    [GroupDescription(Group = "Referee Calls Downs", Name = "Referee Calls Downs", Description = "Referee calls for a break when an edit goes down.")]
    [GroupDescription(Group = "Allow Dives", Name = "Defender Sets up Dives", Description = "Gives the defender a chance (based on Showmanship and damage taken) to allow the completion of dives by the attacker.\n1) For standing dives, the defender will stand up dazed.\n2) For ground dives, the defender will remain down longer. If he is face down, the defender will also roll over to allow potential pinning dives to occur.")]
    [GroupDescription(Group = "Corner Daze", Name = "Corner Moves Cause Stun", Description = "Makes corner moves executed during large/critical damage force the opponent to stand up dazed, if the attacker's finisher is a Corner To Center/Apron To Ring/Dive vs Standing Opponent move.")]
    [GroupDescription(Group = "Fly Range", Name = "Increase Fly Range", Description = "Increases Fly Range for Juniors/Lucha/Panther styles.\nFor reference, the range was originally 1.1875f. It's been increased to 1.4600f")]
    [GroupDescription(Group = "Dynamic Attendance", Name = "Dynamic Attendance Level", Description = "Set the Attendance Level based on participating edits' Rank & Charisma.")]
    [GroupDescription(Group = "Ring Config", Name = "Automatic Ring Configuration", Description = "Automates match settings for different rings.")]
    [GroupDescription(Group = "Forced Sell", Name = "Forced Signature Move Sell", Description = "Increases down-time after signature moves, to facilitate sequences involving downed opponents.")]
    [GroupDescription(Group = "Ref Costume", Name = "Referee Costume Extension", Description = "Extends the number of referee costumes, using costume files.\nThis was originally a component of Ace's AttireExtension mod.")]
    [GroupDescription(Group = "TOS Override", Name = "Test of Strength Replacement", Description = "Allows players to override the Test of Strength animation with custom actions.")]
    [GroupDescription(Group = "Enable Seconds", Name = "Enable Seconds in All Matches", Description = "Allows players to select Seconds in all matches.\nThis can have unintended effects for modes like S-1, so use your best judgement.")]
    [GroupDescription(Group = "Submission Ignore", Name = "Referee Ignores Submissions", Description = "Gives the referee a chance, based on his Tolerance, to ignore 'Give Up' attempts from the defender.")]
    #endregion
    #region Field Access
    #region Miscellaneous Fields
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "Wrestler Search")]
    [FieldAccess(Class = "Referee", Field = "GoToPlayer", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Referee", Field = "CheckCount29", Group = "2.9Call")]
    [FieldAccess(Class = "Menu_SoundManager", Field = "audio_source_index", Group = "GruntForSubmission")]
    [FieldAccess(Class = "Menu_SoundManager", Field = "sRefAudio", Group = "GruntForSubmission")]
    [FieldAccess(Class = "Menu_SoundManager", Field = "audioSrcInfo", Group = "GruntForSubmission")]
    [FieldAccess(Class = "Menu_SoundManager", Field = "AudioSrcInfo", Group = "GruntForSubmission")]
    [FieldAccess(Class = "Audience", Field = "CheerLevel_Total", Group = "Audience Sounds")]
    [FieldAccess(Class = "PlayerController_AI", Field = "AIActFunc_CornerDive_Stand", Group = "Allow Dives")]
    [FieldAccess(Class = "PlayerController_AI", Field = "AIActFunc_CornerDive_Down", Group = "Allow Dives")]
    [FieldAccess(Class = "PlayerController_AI", Field = "PlObj", Group = "Allow Dives")]
    [FieldAccess(Class = "Player", Field = "ProcessGrapple_Corner", Group = "Corner Daze")]
    #endregion

    #region Pinfall Field Access
    [FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Referee", Field = "GoToPlayer", Group = "Ref Positions For Pinfall")]
    #endregion

    #region MatchUI Fields
    [FieldAccess(Class = "MatchUI", Field = "gameObj_Fight", Group = "ChangeCritImage")]
    [FieldAccess(Class = "MatchUI", Field = "gameObj_Critical", Group = "ChangeCritImage")]
    [FieldAccess(Class = "MatchUI", Field = "animator_Fight", Group = "ChangeCritImage")]
    #endregion

    #region Recovery Taunt Fields
    [FieldAccess(Class = "FormAnimator", Field = "plObj", Group = "Recovery Taunts")]
    //[FieldAccess(Class = "FormAnimator", Field = "InitAnimation", Group = "Recovery Taunts")]
    #endregion
    #endregion

    class GeneralComponents
    {
        #region Forms
        [ControlPanel(Group = "Ring Config")]
        public static Form RingForm()
        {
            if (RingConfigForm.ringForm == null)
            {
                return new RingConfigForm();
            }
            {
                return RingConfigForm.ringForm;
            }
        }

        [ControlPanel(Group = "Wrestler Search")]
        public static Form MSForm()
        {
            if (SearchForm.form == null)
            {
                return new SearchForm();
            }
            else
            {
                return SearchForm.form;
            }
        }

        [ControlPanel(Group = "Recovery Taunts")]
        public static Form RTForm()
        {
            if (RecoveryTauntForm.form == null)
            {
                return new RecoveryTauntForm();
            }
            else
            {
                return RecoveryTauntForm.form;
            }
        }

        [ControlPanel(Group = "Low Tag Recovery")]
        public static Form ReportForm()
        {
            if (Reports.form == null)
            {
                return new Reports();
            }
            else
            {
                return Reports.form;
            }
        }

        [ControlPanel(Group = "UkeNotification")]
        public static Form UkemiForm()
        {
            if (UkemiNotificationForm.form == null)
            {
                return new UkemiNotificationForm();
            }
            else
            {
                return UkemiNotificationForm.form;
            }
        }

        [ControlPanel(Group = "Ref Costume")]
        public static Form RefForm()
        {
            if (AttireExtensionForm.instance == null)
            {
                return new AttireExtensionForm();
            }
            {
                return AttireExtensionForm.instance;
            }
        }

        [ControlPanel(Group = "TOS Override")]
        public static Form TestOfStrengthForm()
        {
            if (TOSForm.form == null)
            {
                return new TOSForm();
            }
            {
                return TOSForm.form;
            }
        }
        #endregion

        #region Force Preemptive Pinfall Count

        private static int downedPlayer;
        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "Ref Positions For Pinfall")]
        public static void PrepareForPinfall(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (MatchMain.inst.isMatchEnd)
            {
                return;
            }
            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

            if (!defender.hasRight)
            {
                return;
            }

            //Prepare for pinfall
            Referee mRef = RefereeMan.inst.GetRefereeObj();
            int refInvolvement = mRef.RefePrm.interfereTime;
            float multiplier = .25f;
            switch (refInvolvement)
            {
                case 0:
                    multiplier = .10f;
                    break;
                case 1:
                    multiplier = .15f;
                    break;
                case 2:
                    multiplier = .25f;
                    break;
                case 3:
                    multiplier = .40f;
                    break;
                case 4:
                    multiplier = .50f;
                    break;
                default:
                    multiplier = .25f;
                    break;
            }
            float checkValue = 65535 * multiplier;

            if (((sd.primarySkillStrength == PrimarySkillStrength.H) && sd.anmLoopTimes == 0 && (defender.SP < checkValue || defender.HP < checkValue)) || defender.isCriticalMoveRecieved && attacker.Zone == ZoneEnum.InRing)
            {
                downedPlayer = defender.PlIdx;
            }
            else
            {
                return;
            }

            //Force check for all signatures and finishers
            //Ensure standing moves don't trigger the code unless it's a finisher; currently experiencing issues with missed strike attacks
            if (sd.anmType == SkillAnmTypeEnum.HitBranch_Single || sd.anmType == SkillAnmTypeEnum.HitBranch_Pair ||
                sd.anmType == SkillAnmTypeEnum.Single || sd.anmType == SkillAnmTypeEnum.Pair && skillAttr != SkillSlotAttr.CriticalMove)
            {
                return;
            }

            if ((skillAttr == SkillSlotAttr.CriticalMove || skillAttr == SkillSlotAttr.SpecialMove) && attacker.CriticalMoveHitCnt < 2 && sd.filteringType != SkillFilteringType.Performance)
            {
                if (defender.Zone == ZoneEnum.InRing)
                {
                    CheckForFall(defender.PlIdx);
                }
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "Ref Positions For Pinfall")]
        public static void CheckForPinfall(Player player)
        {
            if (MatchMain.inst.isMatchEnd || player.Zone != ZoneEnum.InRing ||
                !RefereeIsFree(RefereeMan.inst.GetRefereeObj()))
            {
                return;
            }

            if ((player.PlIdx == downedPlayer && player.hasRight) && (player.State == PlStateEnum.Down_FaceDown || player.State == PlStateEnum.Down_FaceUp))
            {
                downedPlayer = -1;
                CheckForFall(player.PlIdx);
            }
        }

        public static void CheckForFall(int defender)
        {
            Referee mRef = RefereeMan.inst.GetRefereeObj();
            mRef.GoToPlayer(defender, 0f);
            mRef.RefeCount = 0;
            mRef.NextState = RefeStateEnum.CheckSubmission;
        }

        private static bool RefereeIsFree(Referee referee)
        {
            var state = referee.State;
            return (state != RefeStateEnum.OutOfRingCount && state != RefeStateEnum.CheckSubmission && state != RefeStateEnum.DownCount && state != RefeStateEnum.FallCount &&
                    state != RefeStateEnum.Disturbed && state != RefeStateEnum.StartToDown &&
                    state != RefeStateEnum.R_KOCHECK && state != RefeStateEnum.DeclareVictory && state != RefeStateEnum.R_TKOCHECK
                    && state != RefeStateEnum.FoulCount_Normal && state != RefeStateEnum.FoulCount_CornerPost && state != RefeStateEnum.FoulCount_Weapon
                    && state != RefeStateEnum.R_DOWN);
        }
        #endregion

        #region Override Tag Team Recovery

        public static int[,] recoveryParams = new Int32[8, 4];

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Low Tag Recovery")]
        public static void SetTagRecovery()
        {
            recoveryParams = new Int32[8, 4];
            for (int i = 0; i < 8; i++)
            {
                recoveryParams[i, 0] = -1;
                recoveryParams[i, 1] = -1;
                recoveryParams[i, 2] = -1;
                recoveryParams[i, 3] = -1;
            }

            try
            {
                if (isTagMatch())
                {
                    SetLowRecovery();
                }
            }
            catch (Exception e)
            {
                L.D("Tag Recovery Error:" + e.Message);
            }

        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Low Tag Recovery")]
        public static void ResetRecovery()
        {
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);

                if (!plObj)
                {
                    continue;
                }

                if (recoveryParams[i, 0] == -1)
                {
                    continue;
                }
                else
                {
                    plObj.WresParam.hpRecovery = recoveryParams[i, 0];
                    plObj.WresParam.spRecovery = recoveryParams[i, 1];
                    plObj.WresParam.hpRecovery_Bleeding = recoveryParams[i, 2];
                    plObj.WresParam.spRecovery_Bleeding = recoveryParams[i, 3];
                    L.D("Reset recovery for " + DataBase.GetWrestlerFullName(plObj.WresParam));
                }
            }
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

                if (plObj.isSecond || plObj.isIntruder)
                {
                    continue;
                }

                memberCount++;
            }

            if (memberCount > 2)
            {
                if (!GlobalWork.GetInst().MatchSetting.isTornadoBattle &&
                    GlobalWork.inst.MatchSetting.BattleRoyalKind == BattleRoyalKindEnum.Off)
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
                if (!plObj)
                {
                    continue;
                }

                //Ensure that original settings are saved, regardless of the Player type.
                //This is important for scenarios where the player is originally a Second/Intruder, but changes due to match circumstances.
                recoveryParams[i, 0] = plObj.WresParam.hpRecovery;
                recoveryParams[i, 1] = plObj.WresParam.spRecovery;
                recoveryParams[i, 2] = plObj.WresParam.hpRecovery_Bleeding;
                recoveryParams[i, 3] = plObj.WresParam.spRecovery_Bleeding;

                if (plObj.isSecond || plObj.isIntruder)
                {
                    continue;
                }

                plObj.WresParam.hpRecovery = 0;
                plObj.WresParam.spRecovery = 0;
                plObj.WresParam.hpRecovery_Bleeding = 0;
                plObj.WresParam.spRecovery_Bleeding = 0;
                L.D("Updated recovery for " + DataBase.GetWrestlerFullName(plObj.WresParam));
            }
        }
        #endregion

        #region Resilient Criticals
        [Hook(TargetClass = "Player", TargetMethod = "ProcessCritical", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "Resilient Critical")]
        public static void SetInjury(Player plObj)
        {
            try
            {
                if (plObj == null)
                {
                    return;
                }

                //Get targetting player
                Player targetPlObj = PlayerMan.inst.GetPlObj(plObj.TargetPlIdx);
                String wrestlerName = DataBase.GetWrestlerFullName(plObj.WresParam);
                int resilience = 0;

                global::SkillData currentSkill = targetPlObj.animator.CurrentSkill;

                if (currentSkill == null)
                {
                    currentSkill = new SkillData
                    {
                        atkPow_Arm = 0,
                        atkPow_Waist = 0,
                        atkPow_Leg = 0,
                        atkPow_Neck = 0
                    };
                }
                if (currentSkill.atkPow_Neck != 0)
                {
                    resilience = plObj.WresParam.defNeck;
                }

                if (currentSkill.atkPow_Waist != 0)
                {
                    if (resilience == 0 || plObj.WresParam.defWaist > resilience)
                    {
                        resilience = plObj.WresParam.defWaist;
                    }
                }

                if (currentSkill.atkPow_Arm != 0)
                {
                    if (resilience == 0 || plObj.WresParam.defArm > resilience)
                    {
                        resilience = plObj.WresParam.defArm;
                    }
                }

                if (currentSkill.atkPow_Leg != 0)
                {
                    if (resilience == 0 || plObj.WresParam.defLeg > resilience)
                    {
                        resilience = plObj.WresParam.defLeg;
                    }
                }

                //Non-move criticals are ignored.
                if (currentSkill.atkPow_Arm == 0 && currentSkill.atkPow_Neck == 0 &&
                    currentSkill.atkPow_Waist == 0 && currentSkill.atkPow_Leg == 0)
                {
                    return;
                }

                //If the check fails, do nothing
                if (UnityEngine.Random.Range(0, 10 + (resilience * 5)) < 10)
                {
                    L.D("Resilient Critical Check failed");
                    return;
                }

                plObj.isKO = false;
                if (resilience == 0)
                {
                    plObj.SetSP(10923);
                    plObj.SetBP(10923);
                }
                else if (resilience == 1)
                {
                    plObj.SetSP(13107);
                    plObj.SetBP(13107);
                }
                else
                {
                    plObj.SetSP(16383);
                    plObj.SetBP(16383);
                }

            }
            catch (Exception e)
            {
                L.D("Resilient Critical Error - " + e.Message);
                plObj.isKO = true;
                plObj.SetSP(0);
                plObj.SetBP(0);
            }
        }
        #endregion

        #region Modify Critical Graphic
        public static String rootFolder = "EGOData\\";
        public static String imageFolder = "Images";
        public static String _noImageValue = "None";
        public static Dictionary<int, String> critImages = new Dictionary<int, String>();
        public static GameObject critImage;

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ChangeCritImage")]
        public static void SetCriticalImages()
        {
            //Ensure the folder is created
            if (!Directory.Exists(rootFolder + imageFolder))
            {
                Directory.CreateDirectory(rootFolder + imageFolder);
                return;
            }

            //Get files in the image folder
            String currentPath = System.IO.Directory.GetCurrentDirectory();

            String ringName = "";
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            if ((int)settings.ringID < (int)RingID.EditRingIDTop)
            {
                ringName += settings.ringID;
            }
            else if ((int)settings.ringID >= (int)RingID.EditRingIDTop)
            {
                ringName = global::SaveData.GetInst().GetEditRingData(settings.ringID).name;
            }
            //NOTE: File names are being returned with the full path. Appending the path in subsequent calls is unnecessary.
            var images = Directory.GetFiles(currentPath + @"\" + rootFolder + imageFolder, "*.png");

            //Ensure all files are the correct dimensions.
            List<String> correctImages = new List<String>();
            foreach (var image in images)
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(image))
                {
                    if (img.Width == 648 && img.Height == 328)
                    {
                        correctImages.Add(image);
                    }
                }
            }
            images = correctImages.ToArray();

            //Determine which file to use for each player
            critImages.Clear();
            foreach (var id in GetPlayerList())
            {
                bool imageFound = false;
                String editName = DataBase.GetWrestlerFullName(PlayerMan.GetInst().GetPlObj(id).WresParam);
                foreach (var image in images)
                {
                    String imageName = System.IO.Path.GetFileName(image);

                    //Remove the file type; handle instances where the name includes '.' characters.
                    String[] splitImageName = imageName.Split('.');
                    if (splitImageName.Length > 2)
                    {
                        for (int i = 0; i < splitImageName.Length; i++)
                        {
                            imageName += splitImageName[i];
                        }
                    }
                    else
                    {
                        imageName = splitImageName[0];
                    }

                    //Look for ring images
                    if (imageName.Equals(editName) || imageName.Equals(ringName))
                    {
                        critImages.Add(id, image);
                        imageFound = true;
                        break;
                    }
                }

                if (!imageFound)
                {
                    //Use generic critical image if it exists
                    if (File.Exists(currentPath + @"\" + rootFolder + imageFolder + @"\Critical.png"))
                    {
                        critImages.Add(id, currentPath + @"\" + rootFolder + imageFolder + @"\Critical.png");
                    }
                    else
                    {
                        critImages.Add(id, _noImageValue);
                    }
                }
            }

        }

        [Hook(TargetClass = "Player", TargetMethod = "ProcessCritical", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "ChangeCritImage")]
        public static void ReplaceCriticalImage(Player p)
        {
            int attacker = p.TargetPlIdx;

            //Handle instances where an attacker did not cause a critical
            if (attacker < 0 || attacker > 7)
            {
                L.D("Attacker value out of range: " + attacker);
                return;
            }

            //Get image to display
            String image = critImages[attacker];

            //Determine whether a custom image should be used
            if (image.Equals(_noImageValue) || !File.Exists(image))
            {
                L.D("Image file does not exist: " + image);
                global::MatchSEPlayer.inst.PlayMatchSE(global::MatchSEEnum.Critical, 1f, -1);
                MatchUI.inst.gameObj_Critical.SetActive(true);
            }
            else
            {
                ShowCustomCritImage(image);
            }
        }


        [Hook(TargetClass = "Referee", TargetMethod = "CallFight", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.None,
            Group = "ChangeCritImage")]
        public static void SetFightImageCopy()
        {
            try
            {
                //critImage = UnityEngine.Object.Instantiate(MatchUI.inst.gameObj_Fight.transform.FindChild("Image_Fight").gameObject);
                critImage = MatchUI.inst.gameObj_Critical;
            }
            catch (Exception e)
            {
                L.D("Error Setting Up Custom Critical Image: " + e);
                critImage = null;
            }
        }

        //Prevent default critical image from being displayed
        [Hook(TargetClass = "MatchUI", TargetMethod = "Show_Critical", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn,
            Group = "ChangeCritImage")]
        public static bool DisableDefaultCritical(MatchUI ui)
        {
            return true;
        }

        public static void ShowCustomCritImage(String imageName)
        {
            try
            {
                L.D("Replacing Critical Image with " + imageName);
                Sprite sprite = null;
                String imagePath = imageName;

                byte[] data = File.ReadAllBytes(imageName);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(data);
                sprite = Sprite.Create(texture, new Rect(0f, 0f, 648f, 328f), new Vector2(90f, 90f));
                MatchUI.inst.animator_Critical.speed = .5f;

                if (sprite != null && critImage != null)
                {
                    L.D("Crit Image Position x: " + critImage.transform.position.x + "\nCrit Image Position y: " + critImage.transform.position.y);
                    L.D("Crit Image Rotation x: " + critImage.transform.rotation.x + "\nCrit Image Rotation y: " + critImage.transform.rotation.y + "\nCrit Image Rotation w: " + critImage.transform.rotation.w);

                    Image img = critImage.GetComponent<Image>();
                    if (img == null)
                    {
                        L.D("gameObj_Critical<Image> = null");
                        return;
                    }
                    img.sprite = sprite;
                    MatchSEPlayer.inst.PlayMatchSE(MatchSEEnum.Critical, 1f, -1);
                    critImage.SetActive(true);
                }
            }
            catch (Exception e)
            {
                L.D("CustomCritError: " + e.Message);
            }

        }

        #endregion

        #region Recovery Taunts

        #region Variables
        public static SkillData[] tauntData;
        public static int[] recoveryTauntCount;
        public static TauntExecution[] tauntStatus;
        public static Dictionary<String, WakeUpTaunt> styleTaunts = new Dictionary<String, WakeUpTaunt>();
        public static Dictionary<String, WakeUpTaunt> wrestlerTaunts = new Dictionary<String, WakeUpTaunt>();
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Recovery Taunts")]
        public static void SetWakeUpTaunts()
        {
            tauntData = new SkillData[8];
            tauntStatus = new TauntExecution[8];
            recoveryTauntCount = new int[8];
            styleTaunts.Clear();
            wrestlerTaunts.Clear();

            //Determine the maximum number of taunts per edit
            for (int i = 0; i < 8; i++)
            {
                tauntStatus[i] = TauntExecution.Reset;
                Player player = PlayerMan.inst.GetPlObj(i);
                if (!player)
                {
                    continue;
                }

                recoveryTauntCount[i] = ((int)player.WresParam.charismaRank + (int)player.WresParam.wrestlerRank) / 2;

                //Ensure that every edit can use at least one Recovery Taun
                if (recoveryTauntCount[i] <= 0)
                {
                    recoveryTauntCount[i] = 1;
                }
            }

            foreach (WakeUpTaunt taunt in RecoveryTauntForm.form.wu_styles.Items)
            {
                //Ensure that we aren't adding duplicates.
                if (!styleTaunts.ContainsKey(taunt.StyleItem.Name))
                {
                    styleTaunts.Add(taunt.StyleItem.Name, taunt);
                }
            }

            foreach (WakeUpTaunt taunt in RecoveryTauntForm.form.wu_wrestlers.Items)
            {
                //Ensure that we aren't adding duplicates.
                if (!wrestlerTaunts.ContainsKey(taunt.StyleItem.Name))
                {
                    wrestlerTaunts.Add(taunt.StyleItem.Name, taunt);
                }
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "Process_Down", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "Recovery Taunts")]
        public static bool ProcessWakeUp(Player player)
        {
            try
            {
                if (!player)
                {
                    return false;
                }

                if (player.isKO)
                {
                    return false;
                }

                string wrestler = DataBase.GetWrestlerFullName(player.WresParam);
                int damageLevel = GetDamageLevel(player);
                bool executeTaunt = false;

                //Ensure that the AI check is not executed multiple times
                if (player.DownTime != 0 && (tauntStatus[player.PlIdx] == TauntExecution.Skip || tauntStatus[player.PlIdx] == TauntExecution.Executed) && (player.State == PlStateEnum.Down_FaceDown || player.State == PlStateEnum.Down_FaceUp))
                {
                    return false;
                }

                //Ensure taunt data is reset once the player has risen.
                if (player.DownTime == 0 && tauntStatus[player.PlIdx] != TauntExecution.Reset)
                {
                    tauntStatus[player.PlIdx] = TauntExecution.Reset;
                    tauntData[player.PlIdx] = null;
                    return false;
                }

                //Ensure recovery taunts remain before proceeding
                if (recoveryTauntCount[player.PlIdx] <= 0)
                {
                    return false;
                }

                //Ensure the check is only processed when applicable
                if (player.isKOCount || global::MatchMain.inst.isMatchEnd || (player.State != PlStateEnum.Down_FaceDown && player.State != PlStateEnum.Down_FaceUp))
                {
                    return false;
                }

                //Return WakeUp Taunt
                if (!wrestlerTaunts.TryGetValue(wrestler, out WakeUpTaunt taunt))
                {
                    taunt = styleTaunts[player.WresParam.fightStyle.ToString()];
                }

                if (taunt == null)
                {
                    return false;
                }
                else if (taunt.WakeupMoves[damageLevel] == null)
                {
                    return false;
                }

                //Humans can taunt on demand
                //TauntExecution.Force ensure that taunts always trigger after a roll.
                if (player.plController.kind != PlayerControllerKind.AI)
                {
                    if (player.plController.padPush == PadBtnEnum.Performance1 || player.plController.padPush == PadBtnEnum.Performance2
                        || player.plController.padPush == PadBtnEnum.Performance3 || player.plController.padPush == PadBtnEnum.Performance4 ||
                        tauntStatus[player.PlIdx] == TauntExecution.Force)
                    {
                        executeTaunt = true;
                    }
                }
                //AI must pass a check in order to taunt
                else
                {
                    if (player.State == PlStateEnum.Down_FaceDown || player.State == PlStateEnum.Down_FaceUp)
                    {
                        //AI Formula
                        //Starting Value : Showmanship
                        //Number to beat : 100 - (10 * damageLevel), where damageLevel starts at 0
                        //Modifier : 4 * Number of Taunts remaining
                        int showmanship = (player.WresParam.aiParam.personalTraits / 2) + (4 * recoveryTauntCount[player.PlIdx]);
                        int tauntCeiling = 100 - (10 * damageLevel);
                        int checkValue = UnityEngine.Random.Range(showmanship, 100);
                        if (checkValue >= tauntCeiling || tauntStatus[player.PlIdx] == TauntExecution.Force)
                        {
                            executeTaunt = true;
                        }
                        else
                        {
                            tauntStatus[player.PlIdx] = TauntExecution.Skip;
                        }
                    }
                }

                if (executeTaunt)
                {
                    CheckWakeUpTauntConditions(player, taunt, damageLevel);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        [Hook(TargetClass = "FormAnimator", TargetMethod = "InitAnimation", InjectionLocation = 135,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "Recovery Taunts")]
        public static void ReplaceCurrentSkill(FormAnimator animator)
        {
            try
            {
                if (animator.plObj == null || animator.AnmReqType != AnmReqTypeEnum.SkillID || tauntData == null)
                {
                    return;
                }

                //Ensure that the priority chain is not broken
                if (tauntData[animator.plObj.PlIdx] != null)
                {
                    animator.CurrentSkill = tauntData[animator.plObj.PlIdx];
                    tauntData[animator.plObj.PlIdx] = null;
                }
            }
            catch (Exception e)
            {
                L.D("ReplaceCurrentSkillError: " + e);
            }

        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Recovery Taunts")]
        public static void RefreshTauntSlots()
        {
            for (int i = 0; i < 8; i++)
            {
                tauntData[i] = null;
                tauntStatus[i] = TauntExecution.Reset;
                recoveryTauntCount[i] = 0;
            }
        }

        #region Helper Methods
        public static void CheckWakeUpTauntConditions(Player player, WakeUpTaunt taunt, int damageLevel)
        {
            //Determine whether this edit is using the Roll Skill
            //public static Skill rollSkill = new Skill("Rolling", -619);
            if (taunt.WakeupMoves[damageLevel].SkillID == -100)
            {
                ExecuteRoll(player, player.State);
                tauntStatus[player.PlIdx] = TauntExecution.Executed;
                return;
            }

            //Determine whether the edit needs to be rolled before taunting.
            //Ensure that the edit can never roll endlessly
            if ((player.State == PlStateEnum.Down_FaceDown &&
                 taunt.StartPositions[damageLevel] != TauntStartPosition.FaceDown) ||
                (player.State == PlStateEnum.Down_FaceUp &&
                 taunt.StartPositions[damageLevel] != TauntStartPosition.FaceUp) && tauntStatus[player.PlIdx] != TauntExecution.Force)
            {
                ExecuteRoll(player, player.State);

                return;
            }

            //Increase downtime to ensure the edit doesn't stand immediately after taunt's execution
            if (taunt.EndPositions[damageLevel] == TauntEndPosition.Grounded)
            {
                SkillData skillData = global::SkillDataMan.inst.GetSkillData((SkillID)taunt.WakeupMoves[damageLevel].SkillID)[0];
                int totalAnimationFrame = skillData.GetTotalAnimationFrame(0);
                player.AddDownTime(totalAnimationFrame + 300);
                player.isAddedDownTimeByPerformance = true;
            }

            //Ensure that taunts ending in a grounded position do not recovery SP
            bool isGrounded = taunt.EndPositions[damageLevel] == TauntEndPosition.Grounded;
            ExecuteWakeUpTaunt(taunt.WakeupMoves[damageLevel].SkillID, player, !isGrounded);

            tauntStatus[player.PlIdx] = TauntExecution.Executed;
        }
        public static void ExecuteWakeUpTaunt(int skillID, Player player, bool regainSP)
        {
            player.animator.AnmReqType = AnmReqTypeEnum.SkillID;
            player.animator.anmType = SkillAnmTypeEnum.Single;
            global::SkillData skillData = global::SkillDataMan.inst.GetSkillData((SkillID)skillID)[0];

            tauntData[player.PlIdx] = skillData;
            player.ChangeState(global::PlStateEnum.Performance);

            //Ensure we're handling skill data correctly
            if (player.lastSkill == SkillSlotEnum.Invalid || !player.lastSkillHit)
            {
                player.lastSkill = SkillSlotEnum.Performance_1;
            }

            //Resolve rare bug in some AI methods
            Player defender = PlayerMan.inst.GetPlObj(player.TargetPlIdx);
            if (!defender)
            {
                player.TargetPlIdx = FindAnOpponent(player.PlIdx);
            }

            //player.animator.InitAnimation();
            ModPack.ModPack.InvokeMethod(player.animator, "InitAnimation", false, null);
            //Ensure recovery taunts are subtracted
            recoveryTauntCount[player.PlIdx] -= 1;

            //Increase spirit for every taunt executed
            if (regainSP)
            {
                L.D("Recovery Taunt regens Spirit");
                player.AddSP(256 * GetDamageLevel(player));
            }
        }
        public static void ExecuteRoll(Player player, PlStateEnum position)
        {
            //Determine player location and roll accordingly
            global::AreaEnum colAreaInRhombus_AroundRing = global::Ring.inst.GetColAreaInRhombus_AroundRing(player.PlPos);

            //Player is at the top of the ring
            if ((colAreaInRhombus_AroundRing == global::AreaEnum.LU || colAreaInRhombus_AroundRing == global::AreaEnum.LD))
            {
                if (position == PlStateEnum.Down_FaceDown)
                {
                    player.animator.ReqBasicAnm(BasicSkillEnum.Rolling_To_Down_OnFace, false, -1);
                }
                else
                {
                    player.animator.ReqBasicAnm(BasicSkillEnum.Rolling_To_Down_OnBack, false, -1);
                }
            }
            else
            {
                if (position == PlStateEnum.Down_FaceDown)
                {
                    player.animator.ReqBasicAnm(BasicSkillEnum.Rolling_To_Up_OnFace, false, -1);
                }
                else
                {
                    player.animator.ReqBasicAnm(BasicSkillEnum.Rolling_To_Up_OnBack, false, -1);
                }
            }
        }
        public static void RefereshTauntData()
        {
            tauntData = new SkillData[8];
            for (int i = 0; i < 8; i++)
            {
                tauntData[i] = null;
                tauntStatus[i] = TauntExecution.Reset;
            }
        }
        public static int GetDamageLevel(Player player)
        {
            if (player.HP >= 49152f)
            {
                return 0;
            }
            else if (player.HP >= 24576f)
            {
                return 1;
            }
            else if (player.HP >= 12288f)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        public static int FindAnOpponent(int playerIndex)
        {
            if (playerIndex < 4)
            {
                for (int i = 4; i < 8; i++)
                {
                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (player)
                    {
                        if (!player.isSleep)
                        {
                            return i;
                        }
                    }
                }

                //We should not reach this point
                return 4;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (player)
                    {
                        if (!player.isSleep)
                        {
                            return i;
                        }
                    }
                }

                //We should not reach this point
                return 0;
            }
        }
        #endregion
        #endregion

        #region Variable Audience Cheers
        [Hook(TargetClass = "Audience", TargetMethod = "PlayCheerVoice", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.ModifyReturn, Group = "Audience Sounds")]
        public static bool PlayCheerVoice(CheerVoiceEnum vid, int nLevel)
        {
            if (RefereeMan.inst && vid == CheerVoiceEnum.ODOROKI_D && RefereeMan.inst.GetRefereeObj().SkillID != 1110)
            //If a 2.9-count cheer has been requested but there is no 2.9 count, replace the cheer with a generic one.
            {
                vid = CheerVoiceEnum.ODOROKI_L;
                int num = 0;
                for (int i = 0; i < 8; i++)
                {
                    Player plObj = PlayerMan.GetInst().GetPlObj(i);
                    if (plObj && !plObj.isSleep && plObj.WresParam.wrestlerRank >= (RankEnum)num)
                    {
                        num = (int)plObj.WresParam.wrestlerRank;
                    }
                }
                nLevel = ((num == 0) ? 1 : (num - 1));
            }
            MatchMain inst = MatchMain.GetInst();
            if (vid == CheerVoiceEnum.ODOROKI_L)
            {
                for (int j = 0; j < 8; j++)
                {
                    Player plObj2 = PlayerMan.GetInst().GetPlObj(j);
                    if (plObj2 && plObj2.State == PlStateEnum.Performance && nLevel >= 2)
                    //If a wrestler is taunting, adjust the cheer volume so it matches the background audience level.
                    {
                        nLevel = Audience.inst.CheerLevel_Total;
                        if (plObj2.WresParam.wrestlerRank >= RankEnum.A && nLevel <= 2)
                        {
                            nLevel++;
                        }
                    }
                }
                int num2 = nLevel * 2;
                num2 += UnityEngine.Random.Range(-2, 3);
                if (num2 < 0)
                {
                    num2 = 0;
                }
                if (num2 > 9)
                {
                    num2 = 9;
                }
                switch (num2)
                {
                    case 0:
                        vid = CheerVoiceEnum.Oooh;
                        break;
                    case 1:
                        vid = CheerVoiceEnum.SOUZEN;
                        break;
                    case 2:
                        vid = CheerVoiceEnum.ODOROKI_S;
                        break;
                    case 3:
                        vid = CheerVoiceEnum.ODOROKI_M;
                        break;
                    case 4:
                        vid = CheerVoiceEnum.Hoo;
                        break;
                    case 5:
                        vid = CheerVoiceEnum.Despair;
                        break;
                    case 6:
                        vid = CheerVoiceEnum.MORIAGA_S;
                        break;
                    case 7:
                        vid = CheerVoiceEnum.MORIAGA_M;
                        break;
                    case 8:
                        vid = CheerVoiceEnum.ODOROKI_L;
                        break;
                    case 9:
                        vid = CheerVoiceEnum.MORIAGA_L;
                        break;
                }
            }
            bool result;
            if (inst.isFastForwardMatch && Fade.GetInst().IsFadeFinish() && inst.MchFrameCnt % 20u > 0u)
            {
                result = true;
            }
            else
            {
                float num3 = 0.5f;
                float num4 = 1f;
                if (inst.AttendanceRate == 0f)
                {
                    num3 = 0f;
                    num4 = 0f;
                }
                else
                {
                    num3 -= (1f - inst.AttendanceRate) * 0.2f;
                    num4 -= (1f - inst.AttendanceRate) * 0.5f;
                }
                float num5 = num3 + (num4 - num3) * ((float)nLevel / 4f);
                num5 *= 1f;
                Menu_SoundManager.Play_CheerVoice_OneShot(vid, num5);
                result = true;
            }
            return result;
        }

        #endregion

        #region 2.9 Calls
        public static System.Timers.Timer voiceTimer;

        [Hook(TargetClass = "Audience", TargetMethod = "Play_Surprise", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "2.9Call")]
        public static bool Play29Sound()
        {
            Referee refe = RefereeMan.inst.GetRefereeObj();
            if (((BasicSkillEnum)refe.SkillID == BasicSkillEnum.BFALL4 || (BasicSkillEnum)refe.SkillID == BasicSkillEnum.FFALL4)
                && GlobalWork.GetInst().MatchSetting.VictoryCondition != global::VictoryConditionEnum.Count2)
            {
                try
                {
                    if (voiceTimer == null)
                    {
                        voiceTimer = new System.Timers.Timer
                        {
                            Interval = 700
                        };
                        voiceTimer.Elapsed += OnTimedEvent;
                    }

                    L.D("Preparing to play referee voice");
                    voiceTimer.Start();
                    return true;
                }
                catch (Exception e)
                {
                    L.D("Play29Sound Error: " + e);
                }

            }
            return false;
        }

        public static void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                L.D("Playing referee voice");
                MatchSEPlayer.inst.PlayRefereeVoice(RefeVoiceEnum.DownCount_2);
                voiceTimer.Stop();
            }
            catch (Exception exception)
            {
                L.D("VoiceTimer_Tick Error: " + exception);
            }

        }

        #endregion

        #region Sell Submissions

        public static WrestlerVoiceTypeEnum[] voiceType = new WrestlerVoiceTypeEnum[8];
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "GruntForSubmission")]
        public static void GetVoiceTypes()
        {
            voiceType = new WrestlerVoiceTypeEnum[8];
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (plObj)
                {
                    //First Voice Slot determines voice type
                    voiceType[i] = plObj.WresParam.voiceType[0];
                    try
                    {
                        //Ensure that females and japanese wrestlers use an appropriate voice type
                        if (voiceType[i] == WrestlerVoiceTypeEnum.American_M_0)
                        {
                            if ((plObj.WresParam.sex == SexEnum.Male || plObj.WresParam.sex == SexEnum.MaybeMale) && (plObj.WresParam.country == CountryEnum.Japan ||
                                                                                                                      plObj.WresParam.country == CountryEnum.NorthKorea || plObj.WresParam.country == CountryEnum.SouthKorea))
                            {
                                voiceType[i] = WrestlerVoiceTypeEnum.Japanease_M_0;
                            }

                            if ((plObj.WresParam.sex == SexEnum.Female || plObj.WresParam.sex == SexEnum.MaybeFemale)
                                && (plObj.WresParam.country == CountryEnum.Japan || plObj.WresParam.country == CountryEnum.NorthKorea || plObj.WresParam.country == CountryEnum.SouthKorea))
                            {
                                voiceType[i] = WrestlerVoiceTypeEnum.Japanease_F_0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        L.D("GetVoiceTypesError: " + ex);
                    }
                }
            }
        }

        [Hook(TargetClass = "Referee", TargetMethod = "Start_SubmissionCheck", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "GruntForSubmission")]
        public static void PrepareGrunt(Referee refe)
        {
            if (!refe)
            {
                L.D("Submission check: referee is null");
                return;
            }
            global::Player player = global::PlayerMan.inst.GetPlObj(refe.TargetPlIdx);

            if (!player)
            {
                L.D("Submission check: target player is null");
                return;
            }

            if (!player.isSubmissionDef)
            {
                L.D("Submission check: player is not in a submission");
                return;
            }

            int showmanship = (player.WresParam.aiParam.personalTraits / 2);
            int damageLevel = GetDamageLevel(player);

            int tauntCeiling = 80 - (10 * damageLevel);

            //Make grunts more likely if an edit is ready to tap.
            L.D("Attempting Submission Grunt for " + DataBase.GetWrestlerFullName(player.WresParam));
            if (player.isWannaGiveUp)
            {
                tauntCeiling -= 20;
            }
            int checkValue = UnityEngine.Random.Range(showmanship, 100);
            if (checkValue >= tauntCeiling || player.SP == 0f)
            {
                PlayGrunt(voiceType[player.PlIdx], damageLevel);
            }
        }


        public static void PlayGrunt(WrestlerVoiceTypeEnum voiceType, int damageLevel)
        {
            try
            {
                String[] voices = new String[4];
                switch (voiceType)
                {
                    case WrestlerVoiceTypeEnum.American_M_0:

                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAM0/WRAM0_004",
                        "Sound/Voice/wrestler/WRAM0/WRAM0_005",
                        "Sound/Voice/wrestler/WRAM0/WRAM0_005",
                        "Sound/Voice/wrestler/WRAM0/WRAM0_018"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.American_M_1:
                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAM1/WRAM1_004",
                        "Sound/Voice/wrestler/WRAM1/WRAM1_005",
                        "Sound/Voice/wrestler/WRAM1/WRAM1_005",
                        "Sound/Voice/wrestler/WRAM1/WRAM1_036"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.American_M_2:
                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAM2/WRAM2_004",
                        "Sound/Voice/wrestler/WRAM2/WRAM2_005",
                        "Sound/Voice/wrestler/WRAM2/WRAM2_005",
                        "Sound/Voice/wrestler/WRAM2/WRAM2_017"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.American_M_3:
                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAM3/WRAM3_004",
                        "Sound/Voice/wrestler/WRAM3/WRAM3_005",
                        "Sound/Voice/wrestler/WRAM3/WRAM3_005",
                        "Sound/Voice/wrestler/WRAM3/WRAM3_017"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.American_M_4:
                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAM4/WRAM4_004",
                        "Sound/Voice/wrestler/WRAM4/WRAM4_005",
                        "Sound/Voice/wrestler/WRAM4/WRAM4_005",
                        "Sound/Voice/wrestler/WRAM4/WRAM4_036"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.American_M_5:
                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAM5/WRAM5_004",
                        "Sound/Voice/wrestler/WRAM5/WRAM5_005",
                        "Sound/Voice/wrestler/WRAM5/WRAM5_005",
                        "Sound/Voice/wrestler/WRAM5/WRAM5_036"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.Japanease_M_0:
                        voices = new String[]
                        {
                            "Sound/Voice/wrestler/WRJM0/WRJM0_002",
                            "Sound/Voice/wrestler/WRJM0/WRJM0_000",
                            "Sound/Voice/wrestler/WRJM0/WRJM0_011",
                            "Sound/Voice/wrestler/WRJM0/WRJM0_033"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.Japanease_M_1:
                        voices = new String[]
                        {
                            "Sound/Voice/wrestler/WRJM1/WRJM1_002",
                            "Sound/Voice/wrestler/WRJM1/WRJM1_000",
                            "Sound/Voice/wrestler/WRJM1/WRJM1_033",
                            "Sound/Voice/wrestler/WRJM1/WRJM1_011"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.Japanease_M_2:
                        voices = new String[]
                        {
                            "Sound/Voice/wrestler/WRJM2/WRJM2_002",
                            "Sound/Voice/wrestler/WRJM2/WRJM2_000",
                            "Sound/Voice/wrestler/WRJM2/WRJM2_033",
                            "Sound/Voice/wrestler/WRJM2/WRJM2_011"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.Japanease_M_3:
                        voices = new String[]
                        {
                            "Sound/Voice/wrestler/WRJM3/WRJM3_008",
                            "Sound/Voice/wrestler/WRJM3/WRJM3_009",
                            "Sound/Voice/wrestler/WRJM3/WRJM3_017",
                            "Sound/Voice/wrestler/WRJM3/WRJM3_018"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.Japanease_M_4:
                        voices = new String[]
                        {
                            "Sound/Voice/wrestler/WRJM4/WRJM4_008",
                            "Sound/Voice/wrestler/WRJM4/WRJM4_009",
                            "Sound/Voice/wrestler/WRJM4/WRJM4_017",
                            "Sound/Voice/wrestler/WRJM4/WRJM4_018"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.Japanease_M_5:
                        voices = new String[]
                        {
                            "Sound/Voice/wrestler/WRJM5/WRJM5_008",
                            "Sound/Voice/wrestler/WRJM5/WRJM5_009",
                            "Sound/Voice/wrestler/WRJM5/WRJM5_017",
                            "Sound/Voice/wrestler/WRJM5/WRJM5_018"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.Japanease_M_6:
                        voices = new String[]
                        {
                            "Sound/Voice/wrestler/WRJM6/WRJM6_008",
                            "Sound/Voice/wrestler/WRJM6/WRJM6_009",
                            "Sound/Voice/wrestler/WRJM6/WRJM6_017",
                            "Sound/Voice/wrestler/WRJM6/WRJM6_018"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.American_F_0:
                    case WrestlerVoiceTypeEnum.Japanease_F_0:
                    case WrestlerVoiceTypeEnum.Japanease_F_2:
                    case WrestlerVoiceTypeEnum.Japanease_F_4:
                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAF0/WRAF0_004",
                        "Sound/Voice/wrestler/WRAF0/WRAF0_005",
                        "Sound/Voice/wrestler/WRAF0/WRAF0_005",
                        "Sound/Voice/wrestler/WRAF0/WRAF0_059"
                        };
                        break;
                    case WrestlerVoiceTypeEnum.American_F_1:
                    case WrestlerVoiceTypeEnum.Japanease_F_1:
                    case WrestlerVoiceTypeEnum.Japanease_F_3:
                        voices = new String[]
                        {
                        "Sound/Voice/wrestler/WRAF1/WRAF1_004",
                        "Sound/Voice/wrestler/WRAF1/WRAF1_005",
                        "Sound/Voice/wrestler/WRAF1/WRAF1_005",
                        "Sound/Voice/wrestler/WRAF1/WRAF1_059"
                        };
                        break;
                    default:
                        //Do not play a submission grunt
                        L.D("Invalid grunt: " + voiceType);
                        return;
                }

                L.D("Attempting to play " + voices[damageLevel]);
                var clip = (AudioClip)Resources.Load(voices[damageLevel]);
                var audioSrcInfo = global::Menu_SoundManager.audioSrcInfo[global::Menu_SoundManager.audio_source_index + 3];
                var audioSource = audioSrcInfo.sRefAudio;
                audioSource.volume = .75f;
                audioSource.PlayOneShot(clip);
            }
            catch (Exception e)
            {
                L.D("PlayGruntError: " + e);
            }

        }
        #endregion

        #region Ukemi Notification
        [Hook(TargetClass = "Player", TargetMethod = "InvokeUkeBonus", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "UkeNotification")]
        public static void NotifyForUkemiTrigger(Player player)
        {
            try
            {
                if (MatchMain.inst.isMatchEnd)
                {
                    return;
                }

                //Select cheer from form
                MatchSetting settings = GlobalWork.GetInst().MatchSetting;
                String ringName = global::SaveData.GetInst().GetEditRingData(settings.ringID).name;
                String wrestler = DataBase.GetWrestlerFullName(player.WresParam);

                CheerVoiceEnum cheer = CheerVoiceEnum.Num;

                //Check the wrestlers first
                foreach (UkemiNotification item in UkemiNotificationForm.form.uk_wrestlers.Items)
                {
                    if (item.Name.Equals(wrestler))
                    {
                        cheer = GetRandomCheer(item.CheerList);
                        break;
                    }
                }

                if (cheer == CheerVoiceEnum.Num)
                {

                    //Check the rings next
                    foreach (UkemiNotification item in UkemiNotificationForm.form.uk_ringsList.Items)
                    {
                        if (item.Name.Equals(ringName))
                        {
                            cheer = GetRandomCheer(item.CheerList);
                            break;
                        }
                    }
                }

                if (cheer != CheerVoiceEnum.Num)
                {
                    L.D("Playing Ukemi Notification for " + wrestler + " in " + ringName + ": " + cheer);
                    Audience.inst.PlayCheerVoice(cheer, (int)player.WresParam.charismaRank);
                }
                else
                {
                    L.D("No Ukemi Notification for " + wrestler + " in " + ringName + ": " + cheer);
                }
            }
            catch (Exception e)
            {
                L.D("NotifyForUkemiTriggerError: " + e);
            }

        }

        public static CheerVoiceEnum GetRandomCheer(List<CheerVoiceEnum> cheers)
        {
            int index = UnityEngine.Random.Range(0, cheers.Count);
            return cheers[index];
        }
        #endregion

        #region Referee Calls for Breaks
        [Hook(TargetClass = "Referee", TargetMethod = "CheckStartRefereeing", InjectionLocation = 325,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, ParamTypes = new Type[] { typeof(int) },
            Group = "Referee Calls Downs")]
        public static void CallForDownBreaks()
        {
            global::MatchSEPlayer.inst.PlayRefereeVoice(global::RefeVoiceEnum.Break);
        }
        #endregion

        #region Headbutt Logic
        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "Bleeding Headbutts")]
        public static void CheckHeadbutt(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            if (sd.skillName[1].ToLower().Contains("headbutt") && sd.anmType != SkillAnmTypeEnum.HitBranch_Single && sd.anmType != SkillAnmTypeEnum.HitBranch_Pair)
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

        #region Attacker Ignores Pinfall/Submission Downtime
        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "Ignore Downtime")]
        public static void IgnoreDowntime(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);
            //Remove downtime for attackers after grounded submissions and pinfalls
            if ((sd.filteringType == SkillFilteringType.Choke || sd.filteringType == SkillFilteringType.Claw ||
                 sd.filteringType == SkillFilteringType.Stretch
                 || sd.filteringType == SkillFilteringType.Submission_Arm ||
                 sd.filteringType == SkillFilteringType.Submission_Leg ||
                 sd.filteringType == SkillFilteringType.Submission_Neck
                 || sd.filteringType == SkillFilteringType.Submission_Complex) && (defender.State == PlStateEnum.Down_FaceDown || defender.State == PlStateEnum.Down_FaceUp) ||
                sd.filteringType == SkillFilteringType.Fall)
            {
                attacker.SetDownTime(0);
            }
        }
        #endregion

        #region Force Defender to Set-up for Dives

        [Hook(TargetClass = "PlayerController_AI", TargetMethod = "AIActFunc_CornerDive_Down", InjectionLocation = 0,
       InjectDirection = HookInjectDirection.Before,
       InjectFlags = HookInjectFlags.PassInvokingInstance,
       Group = "Allow Dives")]
        public static void SetupPostGroundDives(PlayerController_AI ai)
        {
            try
            {
                if (ai.PlObj.Zone != ZoneEnum.OnCornerPost)
                {
                    return;
                }

                Player defender = global::PlayerMan.inst.GetPlObj(ai.PlObj.TargetPlIdx);
                if (defender.State != PlStateEnum.Down_FaceDown && defender.State != PlStateEnum.Down_FaceUp)
                {
                    return;
                }

                //Determine whether action proceeds based on defender's current damage and showmanship
                if (UnityEngine.Random.Range(1, 50) - (GetDamageLevel(defender) * 10) <
                    defender.WresParam.aiParam.personalTraits)
                {
                    //Ensure the dive triggers
                    if (defender.DownTime <= 48)
                    {
                        //Allow instances for dives to miss early in the match.
                        defender.DownTime = (51 + GetDamageLevel(defender)) * 2;
                    }

                    //Ensure that the defender takes dives face up, to allow pins
                    if (defender.State == PlStateEnum.Down_FaceDown)
                    {
                        ExecuteRoll(defender, defender.State);
                    }

                }
            }
            catch (Exception e)
            {
                L.D("SetupPostGroundDivesError:" + e);
            }

        }

        [Hook(TargetClass = "PlayerController_AI", TargetMethod = "AIActFunc_CornerDive_Stand", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "Allow Dives")]
        public static void SetupPostStandDives(PlayerController_AI ai)
        {
            try
            {
                if (ai.PlObj.Zone != ZoneEnum.OnCornerPost && ai.PlObj.Zone != ZoneEnum.Apron)
                {
                    return;
                }

                Player defender = global::PlayerMan.inst.GetPlObj(ai.PlObj.TargetPlIdx);
                if (ai.PlObj.DistanceToTarget > 3.54166651f)
                {
                    return;
                }

                //Determine whether action proceeds based on defender's current damage and showmanship
                if (UnityEngine.Random.Range(1, 50) - (GetDamageLevel(defender) * 5) <
                    defender.WresParam.aiParam.personalTraits)
                {
                    if (defender.StunTime <= 48)
                    {
                        defender.StunTime = (51 + GetDamageLevel(defender)) * 2;
                    }

                    defender.isStandingStunOK = true;
                }
            }
            catch (Exception e)
            {
                L.D("SetupPostStandDivesError:" + e);
            }


        }
        #endregion

        #region Force Standing Daze on Corner Moves

        [Hook(TargetClass = "Player", TargetMethod = "ProcessGrapple_Corner", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "Corner Daze")]
        public static void CornerMovesDaze(Player player)
        {
            try
            {
                //Determine whether finishing move is C2C, Apron to Ring or Dive to Standing Opponent
                bool isValidFinisher = false;

                //C2C Move, Apron to Ring, Run Up Turnbuckle
                if (player.WresParam.skillAttr[13] == SkillSlotAttr.CriticalMove || player.WresParam.skillAttr[16] == SkillSlotAttr.CriticalMove || player.WresParam.skillAttr[17] == SkillSlotAttr.CriticalMove)
                {
                    isValidFinisher = true;
                }

                //Determine whether any of the diving moves are finishers
                if (!isValidFinisher)
                {
                    int finisherSlot = 0;

                    for (int i = 19; i < 23; i++)
                    {
                        if (player.WresParam.skillAttr[i] == SkillSlotAttr.CriticalMove)
                        {
                            finisherSlot = i;
                            break;
                        }
                    }

                    //If a diving move is a finisher, determine if it's against a standing opponent.
                    if (finisherSlot > 0)
                    {
                        var skillData = global::SkillDataMan.inst.GetSkillData(player.WresParam.skillSlot[finisherSlot]);
                        if (skillData[1].anmType == SkillAnmTypeEnum.Dive_Stand_Single ||
                            skillData[1].anmType == SkillAnmTypeEnum.Dive_Stand_Pair)
                        {
                            isValidFinisher = true;
                        }
                    }
                }

                if (!isValidFinisher)
                {
                    return;
                }
                //if (player.grappleType == GrappleTypeEnum.Corner)
                //{
                //This should only occur in large/critical damage
                Player defender = PlayerMan.inst.GetPlObj(player.TargetPlIdx);
                if (GetDamageLevel(defender) > 1)
                {
                    defender.isStandingStunOK = true;
                    defender.AddStunTime(180);
                }
                //}
            }
            catch (Exception e)
            {
                L.D("CornerMovesDazeException: " + e);
            }

        }
        #endregion

        #region Increase Fly Range for Juniors/Lucha/Panther

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Fly Range")]
        public static void IncreaseFlyRange()
        {
            //Originally   1.1875f
            global::MatchData.FlyRangeMdfTbl[(int)FightStyleEnum.Junior] = 1.4600f;
            global::MatchData.FlyRangeMdfTbl[(int)FightStyleEnum.Luchador] = 1.4600f;
            global::MatchData.FlyRangeMdfTbl[(int)FightStyleEnum.Panther] = 1.4600f;
        }

        #endregion

        #region Dynamic Crowd Based on Participating Edits
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitArena", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Dynamic Attendance")]
        public static void SetAttendanceLevel()
        {
            try
            {
                //Check for ModPack, ensure that we aren't over-writing any options.
                if (CheckForMOTWMatch())
                {
                    return;
                }

                double appeal = GetAverageCharisma() + GetAverageRank();

                //Title matches gain extra Audience Appeal
                if (GlobalParam.TitleMatch_BeltData != null)
                    appeal += 2;

                L.D("Audience Appeal: " + appeal);

                //If the maximum average is greater than the final appeal, increase the final value
                //Not valid for matches with only two edits
                int maxAppeal = GetMaxCharisma() + GetMaxRank();

                if (maxAppeal > appeal && GetPlayerList().Length > 2)
                {
                    L.D("Max Appeal: " + maxAppeal + "\n");
                    double difference = appeal - maxAppeal;

                    if (difference > 3)
                    {
                        appeal += 2;
                    }
                    else
                    {
                        appeal += 1;
                    }
                }

                if (appeal < 0)
                {
                    appeal = 0;
                }
                else if (appeal > 10)
                {
                    appeal = 10;
                }

                switch (appeal)
                {
                    case 0:
                    case 1:
                    case 2:
                        ModPackForm.instance.comboBox2.SelectedIndex = 0;
                        MatchMain.inst.AttendanceRate = 0f;
                        break;
                    case 3:
                    case 4:
                        ModPackForm.instance.comboBox2.SelectedIndex = 1;
                        MatchMain.inst.AttendanceRate = 0.24f;
                        break;
                    case 5:
                    case 6:
                        ModPackForm.instance.comboBox2.SelectedIndex = 2;
                        MatchMain.inst.AttendanceRate = 0.49f;
                        break;
                    case 7:
                    case 8:
                        ModPackForm.instance.comboBox2.SelectedIndex = 3;
                        MatchMain.inst.AttendanceRate = 0.74f;
                        break;
                    default:
                        ModPackForm.instance.comboBox2.SelectedIndex = 4;
                        MatchMain.inst.AttendanceRate = 1f;
                        break;
                }

            }
            catch (Exception e)
            {
                L.D("SetAttendanceLevelError: " + e);
            }
        }
        public static double GetAverageRank()
        {
            double rank = 0;
            for (int i = 0; i < 8; i++)
            {
                MatchWrestlerInfo plObj = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];
                if (!plObj.entry)
                {
                    continue;
                }

                if (!plObj.isIntruder)
                {
                    rank += (int)plObj.param.wrestlerRank;
                }
            }

            rank = rank / GetPlayerList().Length;

            L.D("Average rank: " + rank);
            return Math.Ceiling(rank);
        }
        public static double GetAverageCharisma()
        {
            double charisma = 0;
            for (int i = 0; i < 8; i++)
            {
                MatchWrestlerInfo plObj = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];
                if (!plObj.entry)
                {
                    continue;
                }

                if (!plObj.isIntruder)
                {
                    charisma += (int)plObj.param.charismaRank;
                }
            }

            charisma = charisma / GetPlayerList().Length;

            L.D("Average Charisma: " + charisma);
            return Math.Ceiling(charisma);
        }
        public static int GetMaxRank()
        {
            int rank = 0;
            for (int i = 0; i < 8; i++)
            {
                MatchWrestlerInfo plObj = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];
                if (!plObj.entry)
                {
                    continue;
                }

                if (!plObj.isIntruder)
                {
                    if ((int)plObj.param.wrestlerRank > rank)
                    {
                        rank = (int)plObj.param.wrestlerRank;
                    }

                }
            }

            return rank;
        }
        public static int GetMaxCharisma()
        {
            int charisma = 0;
            for (int i = 0; i < 8; i++)
            {
                MatchWrestlerInfo plObj = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];
                if (!plObj.entry)
                {
                    continue;
                }

                if (!plObj.isIntruder)
                {
                    if ((int)plObj.param.charismaRank > charisma)
                    {
                        charisma = (int)plObj.param.charismaRank;
                    }
                }
            }

            return charisma;
        }

        #endregion

        #region Automatic Match Configuration

        public static String configRingName = "";


        [Hook(TargetClass = "GlobalParam", TargetMethod = "Set_MatchSetting_Rule", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Ring Config")]
        public static void AutomateRefSettingn()
        {
            try
            {
                MatchSetting settings = GlobalWork.inst.MatchSetting;
                String ringName = global::SaveData.GetInst().GetEditRingData(settings.ringID).name;
                foreach (RingConfiguration config in RingConfigForm.ringForm.rc_ringList.Items)
                {
                    if (config.RingName.Equals(ringName))
                    {
                        //Referee
                        if (config.Referees.Count > 0)
                        {
                            RefereeInfo referee = config.Referees[UnityEngine.Random.Range(0, config.Referees.Count)];
                            L.D("Adding " + referee.Data.Prm.name + " with id " + referee.Data.editRefereeID);
                            settings.RefereeID = referee.Data.editRefereeID;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                L.D("AutomateRefError: " + e);
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Ring Config")]
        public static void AutomateRingSetting()
        {
            try
            {
                MatchSetting settings = GlobalWork.inst.MatchSetting;
                String ringName = global::SaveData.inst.GetEditRingData(settings.ringID).name;
                configRingName = "";

                foreach (RingConfiguration config in RingConfigForm.ringForm.rc_ringList.Items)
                {
                    if (config.RingName.Equals(ringName))
                    {
                        configRingName = ringName;

                        //Grapple Settings
                        ModPackForm.instance.numericUpDown5.Value = config.GrappleSetting.Low;
                        ModPackForm.instance.numericUpDown6.Value = config.GrappleSetting.Medium;
                        ModPackForm.instance.numericUpDown7.Value = config.GrappleSetting.High;

                        //Check for ModPack, ensure that we aren't over-riding any options.
                        if (CheckForMOTWMatch())
                        {
                            return;
                        }

                        //Clock Speed
                        //ModPack Royal Rumbles should always use the fastest speed
                        if(ModPack.ModPack.isExtendedRumble || MoreMatchTypes_Form.moreMatchTypesForm.cb_ttt.Checked)
                        {
                            //Double Speed
                            ModPackForm.instance.comboBox8.SelectedIndex = 2;
                        }
                        else if (GetWrestlerList().Length == 2)
                        {
                            ModPackForm.instance.comboBox8.SelectedIndex = config.SinglesSpeed;
                        }
                        else
                        {
                            ModPackForm.instance.comboBox8.SelectedIndex = config.MultiSpeed;
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                L.D("AutomateRingSettingError: " + e);
            }
        }

        [Hook(TargetClass = "Menu_SoundManager", TargetMethod = "MyMusic_Play", InjectionLocation = 68,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Ring Config")]
        public static void AutomateBGM()
        {
            try
            {
                if (!configRingName.Equals(String.Empty))
                {
                    foreach (RingConfiguration config in RingConfigForm.ringForm.rc_ringList.Items)
                    {
                        if (config.RingName.Equals(configRingName))
                        {
                            //BGM
                            if (config.Bgms.Count > 0)
                            {
                                string matchBGM = "";
                                string bgmPath = System.IO.Directory.GetCurrentDirectory() + @"\BGM";

                                matchBGM = Path.Combine(bgmPath,
                                    config.Bgms[UnityEngine.Random.Range(0, config.Bgms.Count)]);
                                Menu_SoundManager.MyMusic_SelectFile_Match = matchBGM;

                                L.D("Loading BGM " + matchBGM + " for " + configRingName);
                                configRingName = "";
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                L.D("AutomateBGMError: " + e);
            }
        }

        public static int[] GetWrestlerList()
        {
            List<int> players = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (plObj)
                {
                    if (!plObj.isIntruder && !plObj.isSecond)
                    {
                        players.Add(plObj.PlIdx);
                    }
                }
            }

            return players.ToArray();
        }
        #endregion

        #region Increase Down-time after Signatures
        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "Forced Sell")]
        public static void IncreaseDownTime(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

            //Ensure standing moves don't trigger the code unless it's a signature; currently experiencing issues with missed strike attacks
            if (sd.anmType == SkillAnmTypeEnum.HitBranch_Single || sd.anmType == SkillAnmTypeEnum.HitBranch_Pair ||
                sd.anmType == SkillAnmTypeEnum.Single || sd.anmType == SkillAnmTypeEnum.Pair && skillAttr != SkillSlotAttr.SpecialMove)
            {
                return;
            }

            if (skillAttr == SkillSlotAttr.SpecialMove && sd.filteringType != SkillFilteringType.Performance)
            {
                //Increase down time for submissions
                if (sd.filteringType == SkillFilteringType.Choke || sd.filteringType == SkillFilteringType.Claw ||
                    sd.filteringType == SkillFilteringType.Stretch
                    || sd.filteringType == SkillFilteringType.Submission_Arm ||
                    sd.filteringType == SkillFilteringType.Submission_Leg ||
                    sd.filteringType == SkillFilteringType.Submission_Neck
                    || sd.filteringType == SkillFilteringType.Submission_Complex)
                {
                    L.D("Increasing down time for " + DataBase.GetWrestlerFullName(defender.WresParam));
                    defender.DownTime += GetDownTime(attacker);
                }

                L.D("Increasing down time for " + DataBase.GetWrestlerFullName(defender.WresParam));
                defender.DownTime += GetDownTime(attacker);
                defender.isAddedDownTimeByPerformance = false;
                if (defender.Zone == ZoneEnum.InRing)
                {
                    CheckForFall(defender.PlIdx);
                }

            }
        }

        private static int GetDownTime(Player player)
        {
            switch (player.CriticalMoveHitCnt)
            {
                case 0:
                case 1:
                    return 300;
                case 2:
                case 3:
                    return 200;
                default:
                    return 100;
            }
        }

        #endregion

        #region Extend Referee Attires

        public static Referee refObj;
        public static String costumePath = "./EGOData/RefereeCostumes/";

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = 2147483647,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Ref Costume")]
        public static void LoadRefereeCostume()
        {
            try
            {
                GeneralComponents.refObj = RefereeMan.inst.GetRefereeObj();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(costumePath);
                if (!directoryInfo2.Exists)
                {
                    directoryInfo2.Create();
                    return;
                }
                string text2 = AttireExtensionForm.RemoveSpecialCharacters(GeneralComponents.refObj.RefePrm.name);
                FileInfo[] files2 = directoryInfo2.GetFiles(text2 + "*.cos");
                bool flag36 = files2.Length != 0;
                if (flag36)
                {
                    Attire_Select attire_Select2 = new Attire_Select(files2, 0, "ref");
                    attire_Select2.ShowDialog();
                    bool flag39 = File.Exists(costumePath + text2 +
                                              attire_Select2.chosenAttire + ".cos");
                    if (flag39)
                    {
                        StreamReader streamReader6 = new StreamReader(
                            costumePath + text2 + attire_Select2.chosenAttire + ".cos");
                        CostumeData costumeData6 = new CostumeData();
                        while (streamReader6.Peek() != -1)
                        {
                            costumeData6.valid = true;
                            for (int num45 = 0; num45 < 9; num45++)
                            {
                                for (int num46 = 0; num46 < 16; num46++)
                                {
                                    costumeData6.layerTex[num45, num46] = streamReader6.ReadLine();
                                    costumeData6.color[num45, num46].r = float.Parse(streamReader6.ReadLine());
                                    costumeData6.color[num45, num46].g = float.Parse(streamReader6.ReadLine());
                                    costumeData6.color[num45, num46].b = float.Parse(streamReader6.ReadLine());
                                    costumeData6.color[num45, num46].a = float.Parse(streamReader6.ReadLine());
                                    costumeData6.highlightIntensity[num45, num46] =
                                        float.Parse(streamReader6.ReadLine());
                                }

                                costumeData6.partsScale[num45] = float.Parse(streamReader6.ReadLine());
                            }
                        }

                        streamReader6.Dispose();
                        streamReader6.Close();
                        try
                        {
                            GeneralComponents.refObj.FormRen.DestroySprite();
                            GeneralComponents.refObj.FormRen.InitTexture(costumeData6, null);
                            for (int num47 = 0; num47 < 9; num47++)
                            {
                                GeneralComponents.refObj.FormRen.partsScale[num47] = costumeData6.partsScale[num47];
                            }

                            GeneralComponents.refObj.FormRen.InitSprite(false);
                            L.D("ATTIRE EXTENSION: REFEREE ATTIRE CHANGED", new object[0]);
                        }
                        catch
                        {
                            L.D("ATTIRE EXTENSION: REFEREE ATTIRE NOT CHANGED", new object[0]);
                            RefereeID refereeID2 = GlobalWork.inst.MatchSetting.RefereeID;
                            RefereeData editRefereeData2 = SaveData.inst.GetEditRefereeData(refereeID2);
                            GeneralComponents.refObj.FormRen.InitTexture(editRefereeData2.appearanceData.costumeData[0],
                                null);
                            GeneralComponents.refObj.FormRen.InitSprite(false);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                L.D("LoadRefereeCostumeError: " + ex);
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 2147483647, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Ref Costume")]
        public static void ResetRefereeCostume()
        {
            try
            {
                GeneralComponents.refObj = RefereeMan.inst.GetRefereeObj();
                RefereeID refereeID = GlobalWork.inst.MatchSetting.RefereeID;
                GeneralComponents.refObj.FormRen.DestroySprite();
                GeneralComponents.refObj.FormRen.InitTexture(SaveData.GetInst().GetEditRefereeData(refereeID).appearanceData.costumeData[0], null);
                for (int k = 0; k < 9; k++)
                {
                    GeneralComponents.refObj.FormRen.partsScale[k] = SaveData.GetInst().GetEditRefereeData(refereeID).appearanceData.costumeData[0].partsScale[k];
                }
                GeneralComponents.refObj.FormRen.InitSprite(false);
            }
            catch (Exception e)
            {
                L.D("ResetRefereeCostumeError: " + e);
            }

        }
        #endregion

        #region Test Of Strength Override

        #region Variables

        public static SkillID tosSkill = 0;
        public static SlotStorage[] tosStorage = new SlotStorage[8];

        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "TOS Override")]
        public static void StoreTOSMoves()
        {
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    tosStorage[i] = new SlotStorage();

                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (!player)
                    {
                        continue;
                    }

                    tosStorage[i].weakSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X];
                }
            }
            catch (Exception e)
            {
                L.D("StoreTOSMovesException: " + e);
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "CheckStartPowerCompetition", InjectionLocation = 74, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "TOS Override")]
        public static bool ReplaceTestOfStrength(Player attacker, out bool result)
        {
            //If animListData method returns true, then the result should be false.
            //Otherwise, the code will continue if animListData method returns false.
            result = false;
            try
            {
                //Ensure we aren't running the check twice
                if (tosSkill != 0)
                {
                    L.D("TOS replacement already in progress");
                    return false;
                }

                if (!attacker)
                {
                    return false;
                }

                Player winner = attacker;
                Player defender = global::PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

                //Determine whether animListData should be replaced with unique animations.
                //If the current player loses the checks, then the opponent will naturally win on the separate check.
                //If HP and SP are equal, perform normal test of strength
                if (defender.SP == attacker.SP && defender.HP == attacker.HP)
                {
                    L.D("TOSFail: Defender HP & SP = Attacker");
                    return false;
                }

                if (defender.SP == attacker.SP)
                {
                    if (defender.HP > attacker.HP)
                    {
                        L.D("TOSFail: Defender HP > Attacker");
                        return false;
                    }
                }
                //else if (defender.SP > attacker.SP)
                //{
                //    L.D("TOSFail: Defender SP > Attacker");
                //    L.D("Defender SP: " + defender.SP);
                //    L.D("Attacker SP: " + attacker.SP);
                //    return false;
                //}

                List<Skill> tosMoves = new List<Skill>();

                //Determine whether we should pull from the Wrestler or Style list
                String style = winner.WresParam.fightStyle.ToString();
                String name = DataBase.GetWrestlerFullName(winner.WresParam);

                L.D("Wrestler is " + name + ".\nStyle is " + style);

                foreach (TOSMoves move in TOSForm.form.tos_wrestlers.Items)
                {
                    //L.D("Checking " + move.Name + " with " + move.Skills.Count + " moves.");
                    if (move.Name.Trim().Equals(name))
                    {
                        L.D("Match found");
                        tosMoves = move.Skills;
                        break;
                    }
                }

                if (tosMoves.Count == 0)
                {
                    L.D("No wrestler move found");
                    foreach (TOSMoves move in TOSForm.form.tos_styles.Items)
                    {
                        //L.D("Checking " + move.Name + " with " + move.Skills.Count + " moves.");
                        if (move.Name.Trim().Equals(style))
                        {
                            L.D("Match found");
                            tosMoves = move.Skills;
                            break;
                        }
                    }
                }

                if (tosMoves.Count == 0)
                {
                    L.D("No style move found");
                    return false;
                }

                //Ensure that the replacement is randomized
                int rngValue = UnityEngine.Random.Range(1, 11);
                if (rngValue > 8)
                {
                    L.D("Failed TOS replacement attempt: " + rngValue);
                    return false;
                }
                else
                {
                    L.D("Success TOS replacement attempt: " + rngValue);
                }

                int skillID = tosMoves[UnityEngine.Random.Range(0, tosMoves.Count)].SkillID;

                //Handle the Clinch action
                if (skillID == -1)
                {
                    {
                        winner.ChangeState(global::PlStateEnum.NormalAnm);
                        winner.animator.ReqBasicAnm(global::BasicSkillEnum.S1_Substitution_FrontHold, true,
                            winner.TargetPlIdx);

                        return true;
                    }
                }

                L.D("Replacing TOS with " + DataBase.GetSkillName((SkillID)skillID));

                //Ensure that animListData move exists
                if (DataBase.GetSkillName((SkillID)skillID).Equals(String.Empty))
                {
                    L.D("Invalid Skill ID");
                    return false;
                }

                tosSkill = (SkillID)skillID;

                winner.animator.AnmReqType = AnmReqTypeEnum.SkillID;
                global::SkillData skillData = global::SkillDataMan.inst.GetSkillData((SkillID)skillID)[0];

                //Ensure that both players return to neutral position
                attacker.ChangeState(global::PlStateEnum.NormalAnm);
                //defender.ChangeState(global::PlStateEnum.NormalAnm);
                //attacker.ChangeState(global::PlStateEnum.Grapple);
                //defender.ChangeState(global::PlStateEnum.Grapple);

                //moveData[winner.PlIdx] = skillData;

                var skillSlot = SkillSlotEnum.Grapple_X;

                winner.lastSkill = skillSlot;

                winner.WresParam.skillSlot[(int)skillSlot] = (SkillID)skillID;
                winner.animator.ReqSlotAnm(skillSlot, false, defender.PlIdx, true);
                winner.lastSkillHit = true;

                return true;
            }
            catch (Exception e)
            {
                L.D("ReplaceTestOfStrengthException: " + e);
                return false;
            }

        }

        [Hook(TargetClass = "FormAnimator", TargetMethod = "ReqSlotAnm", InjectionLocation = 2,
            InjectDirection = HookInjectDirection.Before, InjectFlags = (HookInjectFlags)34, Group = "TOS Override")]
        public static void VerifyTOSMove(FormAnimator animator, SkillSlotEnum skill_slot, bool rev, int def_pl_idx, bool atk_side)
        {
            try
            {
                Player plObj = PlayerMan.inst.GetPlObj(animator.plObj.PlIdx);

                if (!plObj)
                {
                    L.D("VerifyTOSMove - Player Not Found");
                    return;
                }

                if (tosSkill == 0)
                {
                    return;
                }

                if (skill_slot == SkillSlotEnum.Grapple_X)
                {
                    L.D("Verifying TOS Replacement for " + DataBase.GetWrestlerFullName(plObj.WresParam));
                    L.D("Current skill " + DataBase.GetSkillName(plObj.WresParam.skillSlot[(int)skill_slot]));
                    L.D("Replacement Skill: " + DataBase.GetSkillName(tosSkill));

                    if (tosSkill != plObj.WresParam.skillSlot[(int)skill_slot])
                    {
                        L.D("Skills do not match.");
                        plObj.WresParam.skillSlot[(int)skill_slot] = tosSkill;
                    }
                    else
                    {
                        L.D("Skills match");
                    }

                    tosSkill = 0;
                }
            }
            catch (Exception e)
            {
                L.D("VerifyTOSMoveException: " + e);
                tosSkill = 0;
            }

        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "TOS Override")]
        public static void RefreshTOSSlots()
        {

            for (int i = 0; i < 8; i++)
            {
                Player player = PlayerMan.inst.GetPlObj(i);
                if (!player)
                {
                    continue;
                }

                try
                {
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = tosStorage[i].weakSlot;
                }
                catch (Exception e)
                {
                    L.D("RefreshTOSSlots - Error on Player " + i + ": " + e.Message);
                }

            }
        }

        #endregion

        #region Enable Seconds
        [Hook(TargetClass = "Menu_BattleSetting", TargetMethod = "IsSecondSelectableMode", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "Enable Seconds")]
        public static bool EnableSeconds(ref bool isEnabled)
        {
            isEnabled = true;
            return true;

        }

        [Hook(TargetClass = "Menu_BattleSetting", TargetMethod = "IsSingleMatchOnly", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn, Group = "Enable Seconds")]
        public static bool OverrideSingleMatchOnly(ref bool isSingle)
        {
            isSingle = false;
            return true;

        }
        #endregion

        #region Referee Ignores Submission

        private static int ignoreChecksRemaining = 0;
        private static int ignoreDC = 0;
        private static bool ignoreFlag;
        private static System.Timers.Timer ignoreTimer;

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Submission Ignore")]
        public static void SetRefereeIgnoreValues()
        {

            if (ignoreTimer == null)
            {
                ignoreTimer = new System.Timers.Timer
                {
                    Interval = 2000
                };
                ignoreTimer.Elapsed += ResetIgnoreCheck;
            }

            Referee mRef = RefereeMan.inst.GetRefereeObj();
            ignoreChecksRemaining = 4 - mRef.RefePrm.interfereTime;
            ignoreFlag = false;

            if (ignoreChecksRemaining <= 2)
            {
                ignoreDC = 14;
            }
            else if (ignoreChecksRemaining == 3)
            {
                ignoreDC = 12;
            }
            else
            {
                ignoreDC = 10;
            }
            L.D("Ignore checks set to " + ignoreChecksRemaining);
        }

        [Hook(TargetClass = "Referee", TargetMethod = "Process_SubmissionCheck", InjectionLocation = 22,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn,
            Group = "Submission Ignore")]
        public static bool OverrideGiveUp(Referee referee)
        {
            try
            {
                //Check for disturbing players first, based on the method's hook location.
                //If a distrubing player exists, we need to return to the parent method for execution.
                int disturbingPlayer = global::PlayerMan.inst.GetDisturbingPlayer();
                if (disturbingPlayer >= 0)
                {
                    L.D("Disturbing player exists, go back to the main method for processing");
                    return false;
                }

                global::Player plObj = global::PlayerMan.inst.GetPlObj(referee.TargetPlIdx);
                if (plObj.isWannaGiveUp)
                {
                    if (ignoreFlag)
                    {
                        return true;
                    }

                    if (ignoreChecksRemaining > 0)
                    {
                        L.D("Rolling ignore check");

                        int roll = UnityEngine.Random.Range(1, 20);
                        if (roll >= ignoreDC)
                        {
                            ignoreDC += 2;
                            ignoreChecksRemaining -= 1;
                            ignoreFlag = true;

                            L.D("Check passed, " + ignoreChecksRemaining + " checks remaining. DC is now " + ignoreDC +
                                ".");
                            ShowMessage(referee.RefePrm.name + " is watching closely!");
                            ignoreTimer.Start();
                            return true;
                        }
                        else
                        {
                            L.D("Check failed: " + roll + " vs " + ignoreDC + ".");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                L.D("OverrideGiveUpError: " + e);
            }

            return false;
        }

        public static void ResetIgnoreCheck(System.Object source, System.Timers.ElapsedEventArgs e)
        {
            L.D("Resetting ignore flag");
            ignoreFlag = false;
            ignoreTimer.Stop();
        }

        #endregion

        #region General Helper Methods
        private static bool CheckForMOTWMatch()
        {
            var value = MotW.PromotionMenuForm.instance;
            if (value == null)
            {
                return false;
            }

            if (value.MotWMatch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static object GetField(object obj, string field, bool isStatic)
        {
            try
            {
                Type myType = obj.GetType();
                FieldInfo myInfo = myType.GetField(field, isStatic ? (BindingFlags.Static | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.NonPublic));
                return myInfo.GetValue(isStatic ? null : obj);

            }
            catch
            {
                return null;
            }
        }
        public static void SetField(object obj, string field, bool isStatic, object value)
        {
            Type myType = obj.GetType();
            FieldInfo myInfo = myType.GetField(field, isStatic ? (BindingFlags.Static | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.NonPublic));
            myInfo.SetValue(isStatic ? null : obj, value);
        }
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
        private static void ShowMessage(String message, int time = 200)
        {
            //DispNotification.inst.Show(message, time);
            CutSceneMessage.GetInst().Show(message, time);
        }

        #endregion
    }
}
