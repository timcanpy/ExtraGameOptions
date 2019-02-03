﻿using System.Windows.Forms;
using DG;
using UnityEngine;
using DLC;
using System;

namespace QoL_Mods
{

    [GroupDescription(Group = "Variable Front Neck Lock Moves", Name = "Variable Front Neck Lock Moves", Description = "Changes the Front Neck Lock finishing move based on edit's fighting style.")]
    [GroupDescription(Group = "Wrestler Search", Name = "Wrestler Search Tool", Description = "Provides a UI for loading edits within Edit Mode.")]
    [GroupDescription(Group = "Low Tag Recovery", Name = "Low Tag Recovery", Description = "Forces tag teams to use low recovery.")]
    [GroupDescription(Group = "Forced Sell", Name = "Forced Finisher Sell", Description = "Increases down-time after special moves and finishers. The effect is lost after the second finisher is used.")]
    [GroupDescription(Group = "Ref Positions For Pinfall", Name = "Referee Behavior Override", Description = "Forces the referee to move towards the active players after big moves performed late in a match. When the referee decides to start moving depends on his Involvement skill.")]
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "Wrestler Search")]
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "Wrestler Search")]

    #region Pinfall Field Access
    [FieldAccess(Class = "MatchEvaluation", Field = "EvaluateSkill", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "Ref Positions For Pinfall")]
    [FieldAccess(Class = "Referee", Field = "GoToPlayer", Group = "Ref Positions For Pinfall")]
    #endregion

    class Overrides
    {
        [ControlPanel(Group = "Wrestler Search")]
        public static Form MSForm()
        {
            if (QoL_Form.form == null)
            {
                return new QoL_Form();
            }
            else
            {
                return null;
            }
        }

        #region Increase Down time
        [Hook(TargetClass = "MatchEvaluation", TargetMethod = "EvaluateSkill", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "Forced Sell")]
        public static void IncreaseDownTime(int plIDx, SkillData sd, SkillSlotAttr skillAttr)
        {
            Player attacker = PlayerMan.inst.GetPlObj(plIDx);
            Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

            if ((skillAttr == SkillSlotAttr.CriticalMove || skillAttr == SkillSlotAttr.SpecialMove) && attacker.CriticalMoveHitCnt < 2 && sd.filteringType != SkillFilteringType.Performance)
            {
                defender.DownTime += 300;
                CheckForFall(defender.PlIdx);
                //if (attacker.WresParam.fightStyle == FightStyleEnum.Heel)
                //{
                //    global::Audience.inst.PlayCheerVoice(CheerVoiceEnum.BOOING, 4);
                //}
                //else
                //{
                //    global::Audience.inst.PlayCheerVoice(CheerVoiceEnum.ODOROKI_L, 4);
                //}
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

        #region Variable Front Necklock Moves
        [Hook(TargetClass = "Player", TargetMethod = "StandardKeyInput", InjectionLocation = 185, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "Variable Front Neck Lock Moves")]
        public static bool UpdateNeckLockMove(Player attacker)
        {
            try
            {
                attacker.ChangeState(global::PlStateEnum.NormalAnm);

                //Get Player's fight style
                FightStyleEnum style = attacker.WresParam.fightStyle;

                //Determine edit's current damage threshold

                //Determine if a custom  move should be used for this edit
                bool useCustomMove = false;

                if (useCustomMove)
                {

                }
                else
                {
                    switch (style)
                    {
                        case FightStyleEnum.Orthodox:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_ElbowBat, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Technician:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_LegScissors, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Wrestling:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_TuckleSingleLeg, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Ground:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_Oosotogari, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Power:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_HeadBat, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.American:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_KnackleArrow, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Junior:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_CycloneWhip, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Luchador:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_CycloneWhip, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Heel:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_KnackleArrow, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Mysterious:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_CycloneWhip, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Shooter:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_TuckleSingleLeg, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Fighter:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_BodyKneeLift, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Grappler:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_Oosotogari, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Panther:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_BodyKneeLift, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Giant:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_HammerBlow, true, attacker.TargetPlIdx);
                            break;
                        case FightStyleEnum.Devilism:
                            attacker.animator.ReqBasicAnm(BasicSkillEnum.PowerCompetitionWin_KnackleArrow, true, attacker.TargetPlIdx);
                            break;
                    }
                }


                return true;
            }
            catch (Exception e)
            {
                L.D(e.Message);
                return false;
            }

        }
        #endregion
    }
}
