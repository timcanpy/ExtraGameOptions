using System.Windows.Forms;
using DG;
using UnityEngine;
using DLC;
using System;
using System.Collections.Generic;
using QoL_Mods.Data_Classes;
using QoL_Mods.Data_Classes.Facelock;

namespace QoL_Mods
{

    [GroupDescription(Group = "Variable Front Neck Lock Moves", Name = "Variable Front Neck Lock Moves", Description = "Changes the Front Neck Lock finishing move based on edit's fighting style.")]
    [GroupDescription(Group = "Wrestler Search", Name = "Wrestler Search Tool", Description = "Provides a UI for loading edits within Edit Mode.")]
    [GroupDescription(Group = "Low Tag Recovery", Name = "Low Tag Recovery", Description = "Forces tag teams to use low recovery.")]
    [GroupDescription(Group = "Forced Sell", Name = "Forced Finisher Sell", Description = "Increases down-time after special moves and finishers. The effect is lost after the second finisher is used.")]
    [GroupDescription(Group = "Ref Positions For Pinfall", Name = "Referee Behavior Override", Description = "Forces the referee to move towards the active players after big moves performed late in a match. When the referee decides to start moving depends on his Involvement skill.")]
    [GroupDescription(Group = "Face Lock", Name = "Variable Face Lock Moves", Description = "Allows players to override the default Face Lock attack with custom actions.")]
    [GroupDescription(Group = "Resilient Critical", Name = "Critical Resilience", Description = "Gives players a chance to ignore the knock out effects of criticals based on their body part defense. Players receive slight spirit & breathing restoration to remain competitive afterwards.")]
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "Wrestler Search")]
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "Wrestler Search")]
    [FieldAccess(Class = "Referee", Field = "GoToPlayer", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Referee", Field = "GoToPlayer", Group = "Forced Sell")]

    #region Face Lock Access
    [FieldAccess(Class = "FormAnimator", Field = "plObj", Group = "Face Lock")]

    [FieldAccess(Class = "Player", Field = "ProcessKeyInput_Grapple_Run", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "skillInfo", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "CompetitionWorkIdx", Group = "Face Lock")]

    [FieldAccess(Class = "PlayerController_AI", Field = "PlObj", Group = "Face Lock")]
    [FieldAccess(Class = "PlayerController_AI", Field = "Process_OpponentStands_AfterHammerThrow", Group = "Face Lock")]
    #endregion

    #region Pinfall Field Access
    [FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Referee", Field = "GoToPlayer", Group = "Ref Positions For Pinfall")]
    #endregion

    class Overrides
    {
        public static Dictionary<String, FaceLockMoves> faceLockMoves = new Dictionary<String, FaceLockMoves>();
        public static SlotStorage[] slotStorage = new SlotStorage[8];
        public static SkillSlotEnum[] safeCritSlot = new SkillSlotEnum[8];
        public static String finishingMove = "";

        [ControlPanel(Group = "Face Lock")]
        public static Form FLForm()
        {
            if (FaceLockForm.flForm == null)
            {
                return new FaceLockForm();
            }
            else
            {
                return FaceLockForm.flForm;
            }
        }

        [ControlPanel(Group = "Wrestler Search")]
        public static Form MSForm()
        {
            if (QoL_Form.form == null)
            {
                return new QoL_Form();
            }
            else
            {
                return QoL_Form.form;
            }
        }

        #region Increase Down time
        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "Forced Sell")]
        public static void IncreaseDownTime(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

            //Ensure standing moves don't trigger the code unless it's a finisher; currently experiencing issues with missed strike attacks
            if (sd.anmType == SkillAnmTypeEnum.HitBranch_Single || sd.anmType == SkillAnmTypeEnum.HitBranch_Pair ||
                sd.anmType == SkillAnmTypeEnum.Single || sd.anmType == SkillAnmTypeEnum.Pair && skillAttr != SkillSlotAttr.CriticalMove)
            {
                return;
            }

            if ((skillAttr == SkillSlotAttr.CriticalMove || skillAttr == SkillSlotAttr.SpecialMove) && attacker.CriticalMoveHitCnt < 2 && sd.filteringType != SkillFilteringType.Performance)
            {
                defender.DownTime += 300;
                CheckForFall(defender.PlIdx);

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
        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "Ref Positions For Pinfall")]
        public static void CheckForPinfall(Player player)
        {
            if (MatchMain.inst.isMatchEnd)
            {
                return;
            }

            if (player.PlIdx == downedPlayer && (player.State == PlStateEnum.Down_FaceDown || player.State == PlStateEnum.Down_FaceUp) && player.Zone == ZoneEnum.InRing)
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
        #endregion

        #region Override Tag Team Recovery

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Low Tag Recovery")]
        public static void SetTagRecovery()
        {
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
                if (!plObj)
                {
                    continue;
                }
                plObj.WresParam.hpRecovery = 0;
                plObj.WresParam.spRecovery = 0;
                plObj.WresParam.hpRecovery_Bleeding = 0;
                plObj.WresParam.spRecovery_Bleeding = 0;
            }
        }
        #endregion

        #region Face Lock override
      
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Face Lock")]
        public static void SetFaceLockMoves()
        {
            //Add FaceLock Moves
            //Styles
            finishingMove = "";
            faceLockMoves.Clear();
           
            //faceLockMoves = new Dictionary<String, FaceLockMoves>();
            try
            {
                if (FaceLockForm.flForm.nl_styleBox.Items.Count > 0)
                {
                    foreach (FaceLockMoves moves in FaceLockForm.flForm.nl_styleBox.Items)
                    {
                        faceLockMoves.Add(moves.StyleItem.Name, moves);
                    }
                }

                //Wrestlers
                if (FaceLockForm.flForm.nl_wresterList.Items.Count > 0)
                {
                    foreach (FaceLockMoves moves in FaceLockForm.flForm.nl_wresterList.Items)
                    {
                        faceLockMoves.Add(moves.StyleItem.Name, moves);
                    }
                }

                //Save Move Slots to Handle Over-writing
                slotStorage = new SlotStorage[8];
                safeCritSlot = new SkillSlotEnum[8];
                for (int i = 0; i < 8; i++)
                {
                    slotStorage[i] = new SlotStorage();
                    safeCritSlot[i] = SkillSlotEnum.Grapple_X;
                }

                for (int i = 0; i < 8; i++)
                {
                    bool[] bigSlotOptions = new Boolean[4];
                    for (int j = 0; j < 4; j++)
                    {
                        bigSlotOptions[j] = true;
                    }

                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (!player)
                    {
                        continue;
                    }

                    slotStorage[i].weakSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X];
                    slotStorage[i].mediumSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A];
                    slotStorage[i].heavySlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B];
                    slotStorage[i].criticalSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_XA];

                    //Find a safe big slot
                    foreach (AIPriorityAct priority in player.WresParam.aiParam.priorityAct)
                    {
                        if (priority.triggerAct == SkillSlotEnum.Grapple_B)
                        {
                            bigSlotOptions[0] = false;
                        }

                        if (priority.triggerAct == SkillSlotEnum.Grapple_B_U)
                        {
                            bigSlotOptions[1] = false;
                        }

                        if (priority.triggerAct == SkillSlotEnum.Grapple_B_LR)
                        {
                            bigSlotOptions[2] = false;
                        }

                        if (priority.triggerAct == SkillSlotEnum.Grapple_B_D)
                        {
                            bigSlotOptions[3] = false;
                        }
                    }

                    if (bigSlotOptions[0])
                    {
                        safeCritSlot[i] = SkillSlotEnum.Grapple_B;
                    }
                    else if (bigSlotOptions[1])
                    {
                        safeCritSlot[i] = SkillSlotEnum.Grapple_B_U;
                    }
                    else if (bigSlotOptions[2])
                    {
                        safeCritSlot[i] = SkillSlotEnum.Grapple_B_LR;
                    }
                    else if (bigSlotOptions[3])
                    {
                        safeCritSlot[i] = SkillSlotEnum.Grapple_B_D;
                    }

                    L.D("Player " + i + " is using crit slot " + safeCritSlot[i] + ".");
                }
            }
            catch (Exception e)
            {
                L.D("FaceLock Setup Error: " + e.Message);
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "StandardKeyInput", InjectionLocation = 185, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "Face Lock")]
        public static bool UpdateNeckLockMove(Player attacker)
        {
            try
            {
                //Get Player's fight style
                FightStyleEnum style = attacker.WresParam.fightStyle;

                //Determine current damage threshold
                Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);
                if (!defender)
                {
                    return false;
                }

                int damageLevel = 0;
                if (defender.HP >= 49152f)
                {
                    damageLevel = 0;
                }
                else if (defender.HP >= 24576f)
                {
                    damageLevel = 1;
                }
                else if (defender.HP >= 12288f)
                {
                    damageLevel = 2;
                }
                else
                {
                    damageLevel = 3;
                }

                //Determine which move should be used
                String wrestler = DataBase.GetWrestlerFullName(attacker.WresParam);
                if (!faceLockMoves.TryGetValue(wrestler, out FaceLockMoves moveset))
                {
                    moveset = faceLockMoves[style.ToString()];
                }

                if (moveset == null)
                {
                    return false;
                }

                attacker.ChangeState(global::PlStateEnum.NormalAnm);

                if (moveset.Type[damageLevel] == SkillType.BasicMove && !moveset.BasicSkills[damageLevel].SkillName.Contains("HammerThrough"))
                {
                    attacker.animator.ReqBasicAnm((BasicSkillEnum)moveset.BasicSkills[damageLevel].SkillID, true, attacker.TargetPlIdx);
                }
                else if (moveset.Type[damageLevel] == SkillType.IrishWhip || moveset.BasicSkills[damageLevel].SkillName.Contains("HammerThrough"))
                {
                    //Determine the direction to press
                    if (attacker.plController.kind == PlayerControllerKind.AI)
                    {
                        int direction = UnityEngine.Random.Range(1, 2);
                        if (moveset.BasicSkills[damageLevel].SkillName.Equals("Irish Whip (Horizontal)"))
                        {
                            if (attacker.PlDir == PlDirEnum.Right)
                            {
                                attacker.padOn = PadBtnEnum.Dir_L;
                            }
                            else
                            {
                                attacker.padOn = PadBtnEnum.Dir_R;
                            }
                        }
                        else if (moveset.BasicSkills[damageLevel].SkillName.Equals("Irish Whip (Vertical)"))
                        {
                            if (attacker.PlDir == PlDirEnum.Right)
                            {
                                attacker.padOn = PadBtnEnum.Dir_U;
                            }
                            else
                            {
                                attacker.padOn = PadBtnEnum.Dir_D;
                            }
                        }
                        else
                        {
                            if (attacker.PlDir == PlDirEnum.Right)
                            {
                                attacker.padOn = PadBtnEnum.Dir_LD;
                            }
                            else
                            {
                                attacker.padOn = PadBtnEnum.Dir_RU;
                            }
                        }
                    }

                    attacker.ProcessKeyInput_Grapple_Run();

                    if (attacker.plController.kind == PlayerControllerKind.AI)
                    {
                        attacker.plCont_AI.Process_OpponentStands_AfterHammerThrow();
                    }

                }
                else
                {
               
                    SkillSlotEnum slotEnum = SkillSlotEnum.Grapple_X;
                    if (damageLevel == 0)
                    {
                        slotEnum = SkillSlotEnum.Grapple_X;
                    }
                    else if (damageLevel == 1)
                    {
                        slotEnum = SkillSlotEnum.Grapple_A;
                    }
                    else if (damageLevel == 2)
                    {
                        slotEnum = SkillSlotEnum.Grapple_B;
                        defender.isStandingStunOK = true;
                        defender.AddStunTime(240);
                    }
                    else
                    {
                        slotEnum = safeCritSlot[attacker.PlIdx];
                        defender.isStandingStunOK = true;
                        defender.AddStunTime(240);
                    }

                    SkillID currentSkill = attacker.WresParam.skillSlot[(int)slotEnum];
                    attacker.WresParam.skillSlot[(int)slotEnum] = (SkillID)moveset.CustomSkills[damageLevel].SkillID;
                    attacker.animator.ReqSlotAnm(slotEnum, false, -1, true);
                    attacker.lastSkillHit = true;
                }

                return true;
            }
            catch (Exception e)
            {
                L.D("Facelock Override Error:" + e.Message);
                return false;
            }

        }

        [Hook(TargetClass = "Player", TargetMethod = "ProcessKeyInput_Grapple", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "Face Lock")]
        public static void RefreshSlotMoves(Player player)
        {
            try
            {
                if (!player)
                {
                    return;
                }

                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = slotStorage[player.PlIdx].weakSlot;
                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A] = slotStorage[player.PlIdx].mediumSlot;
                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B] = slotStorage[player.PlIdx].heavySlot;
                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_XA] = slotStorage[player.PlIdx].criticalSlot;
            }
            catch (Exception e)
            {
                L.D("RefreshSlotMoves Error: " + e.Message);
            }

        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Face Lock")]
        public static void RefreshAllSlots()
        {
            finishingMove = global::MatchEvaluation.GetInst().GetWinningTechName(true);

            for (int i = 0; i < 8; i++)
            {
                Player player = PlayerMan.inst.GetPlObj(i);
                if (!player)
                {
                    continue;
                }

                try
                {
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = slotStorage[i].weakSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A] = slotStorage[i].mediumSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B] = slotStorage[i].heavySlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_XA] = slotStorage[i].criticalSlot;
                }
                catch (Exception e)
                {
                    L.D("RefreshAllSlots - Error on Player " + i + ": " + e.Message);
                }

            }

            //Ensure that the finishing move is correct
            if (finishingMove.Equals(global::MatchEvaluation.GetInst().GetWinningTechName(true)))
            {
                finishingMove = "";
            }

        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8,
            InjectDirection = HookInjectDirection.After,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersVal |
                          HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "Face Lock")]
        public static void CorrectFinishingMove(Menu_Result result, ref UILabel finishText, string str)
        {
            if (String.IsNullOrEmpty(finishingMove))
            {
                return;
            }
            else
            {
                finishText.text.Replace(global::MatchEvaluation.GetInst().GetWinningTechName(true), finishingMove);
            }
        }

        [Hook(TargetClass = "FormAnimator", TargetMethod = "ReqSlotAnm", InjectionLocation = 176, InjectDirection = HookInjectDirection.Before, InjectFlags = (HookInjectFlags)34, Group = "Face Lock")]
        public static void SetLastSkillHit_FaceLock(FormAnimator animator, SkillSlotEnum skill_slot, bool rev, int def_pl_idx, bool atk_side)
        {
            if (atk_side)
            {
                string skillName = DataBase.GetSkillName(animator.plObj.WresParam.skillSlot[(int)skill_slot]);
                for (int i = 26; i <= 38; i++)
                {
                    //Ensure we aren't checking against the same slot
                    if (i == (int)skill_slot)
                    {
                        continue;
                    }
                    String skill = DataBase.GetSkillName(animator.plObj.WresParam.skillSlot[i]);
                    if (skillName.Equals(skill))
                    {
                        animator.plObj.lastSkill = (SkillSlotEnum)i;
                        break;
                    }
                }
                animator.plObj.lastSkillHit = true;
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

    }
}
