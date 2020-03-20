using DG;
using QoL_Mods.Data_Classes;
using QoL_Mods.Data_Classes.Facelock;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QoL_Mods.Private
{
    #region Group Descriptions
    [GroupDescription(Group = "Face Lock", Name = "Variable Face Lock Moves", Description = "(PRIVATE) Allows players to override the default Face Lock attack with custom actions.")]
    [GroupDescription(Group = "Stamina Affects Reversals", Name = "Stamina Affects Reversals", Description = "(PRIVATE) Lower stamina increases the chance that a defender will reverse moves.")]
    [GroupDescription(Group = "Reversal Cheer", Name = "Cheer on Reversals", Description = "(PRIVATE) Audience may cheer when a defender reverses a move.")]
    #endregion

    #region Field Access
    #region Face Lock Access
    [FieldAccess(Class = "FormAnimator", Field = "plObj", Group = "Face Lock")]

    [FieldAccess(Class = "Player", Field = "ProcessKeyInput_Grapple_Run", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "skillInfo", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "CompetitionWorkIdx", Group = "Face Lock")]

    [FieldAccess(Class = "PlayerController_AI", Field = "PlObj", Group = "Face Lock")]
    [FieldAccess(Class = "PlayerController_AI", Field = "Process_OpponentStands_AfterHammerThrow", Group = "Face Lock")]
    #endregion
    #endregion
    class PrivateOverrides
    {
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

        #region Face Lock override

        #region Variables
        public static Dictionary<String, FaceLockMoves> faceLockMoves = new Dictionary<String, FaceLockMoves>();
        public static SlotStorage[] slotStorage = new SlotStorage[8];
        public static SkillSlotEnum[] safeCritSlot = new SkillSlotEnum[8];
        public static String finishingMove = "";
        #endregion

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
                    safeCritSlot[i] = SkillSlotEnum.Grapple_XA;
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

                    if (safeCritSlot[i] != SkillSlotEnum.Grapple_XA)
                    {
                        slotStorage[i].criticalSlot = player.WresParam.skillSlot[(int)safeCritSlot[i]];
                    }

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
                if ((attacker.padPush & PadBtnEnum.AllAtk) == 0)
                {
                    return false;
                }

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

                if (moveset.Type[damageLevel] == SkillType.BasicMove)
                {
                    attacker.animator.ReqBasicAnm((BasicSkillEnum)moveset.BasicSkills[damageLevel].SkillID, true, attacker.TargetPlIdx);
                }
                else if (moveset.Type[damageLevel] == SkillType.IrishWhip)
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

                    //attacker.ProcessKeyInput_Grapple_Run();

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
                player.WresParam.skillSlot[(int)safeCritSlot[player.PlIdx]] = slotStorage[player.PlIdx].criticalSlot;
            }
            catch (Exception e)
            {
                L.D("RefreshSlotMoves Error: " + e.Message);
            }

        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Face Lock")]
        public static void RefreshAllSlots()
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
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = slotStorage[i].weakSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A] = slotStorage[i].mediumSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B] = slotStorage[i].heavySlot;
                    player.WresParam.skillSlot[(int)safeCritSlot[player.PlIdx]] = slotStorage[player.PlIdx].criticalSlot;
                }
                catch (Exception e)
                {
                    L.D("RefreshAllSlots - Error on Player " + i + ": " + e.Message);
                }

            }

            //Ensure that the finishing move is correct
            try
            {
                finishingMove = global::MatchEvaluation.GetInst().GetWinningTechName(true);
            }
            catch
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
            try
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
            catch (Exception e)
            {
                L.D("SetLastSkillHit_FacelockError: " + e);
            }
        }

        #endregion

        #region Stamina Affects Reversal Rate

        [Hook(TargetClass = "MatchMisc", TargetMethod = "CheckReversal_Grapple", InjectionLocation = 93,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 6 },
            Group = "Stamina Affects Reversals")]
        public static void IncreaseReversalChance(ref int num, int atk_pl_idx, int skill_idx, global::SkillEquipTypeEnum type, int def_pl_idx)
        {
            try
            {
                Player player = global::PlayerMan.inst.GetPlObj(atk_pl_idx);
                int staminaModifier = (5 * GetStaminaLevel(player));
                num += staminaModifier;
            }
            catch (Exception e)
            {
                L.D("IncreaseReversalError: " + e);
            }
        }

        #endregion

        #region Cheer on Reversal

        [Hook(TargetClass = "MatchMisc", TargetMethod = "CheckReversal_Grapple", InjectionLocation = 115,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal,
            Group = "Reversal Cheer")]
        public static void CheerOnReversal(int atk_pl_idx, int skill_idx, global::SkillEquipTypeEnum type,
            int def_pl_idx)
        {
            try
            {
                global::Player plObj = global::PlayerMan.inst.GetPlObj(def_pl_idx);

                //Determine whether we should play the cheer
                int damageLevel = GetDamageLevel(plObj);
                if (damageLevel < UnityEngine.Random.Range(0, 4) || (int)plObj.WresParam.wrestlerRank < 2)
                {
                    return;
                }
                PlayCheer(plObj.WresParam.fightStyle, ((int)plObj.WresParam.charismaRank + damageLevel) / 2);
            }
            catch (Exception e)
            {
                L.D("Error on Play Reversal Cheer:" + e);
            }

        }

        #endregion

        #region Helper Methods
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
        private static void PlayCheer(FightStyleEnum style, int charisma)
        {
            if (style == FightStyleEnum.Heel)
            {
                global::Audience.inst.PlayCheerVoice(CheerVoiceEnum.BOOING, charisma);
            }
            else
            {
                global::Audience.inst.PlayCheerVoice(CheerVoiceEnum.ODOROKI_L, charisma);
            }
        }
        public static int GetStaminaLevel(Player player)
        {
            if (player.BP >= 49152f)
            {
                return 0;
            }
            else if (player.BP >= 24576f)
            {
                return 1;
            }
            else if (player.BP >= 12288f)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        #endregion
    }
}
