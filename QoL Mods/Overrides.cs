using System.Windows.Forms;
using DG;
using UnityEngine;
using DLC;

namespace QoL_Mods
{
    [GroupDescription(Group = "Wrestler Search", Name = "Wrestler Search Tool", Description = "Provides a UI for loading edits within Edit Mode.")]
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
    }
}
