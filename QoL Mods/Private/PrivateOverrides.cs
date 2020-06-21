using DG;
using ModPack;
using QoL_Mods.Data_Classes;
using QoL_Mods.Data_Classes.Facelock;
using QoL_Mods.Data_Classes.Reversal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using static Common_Classes.EnumLibrary;
using WresIDGroup = ModPack.WresIDGroup;

namespace QoL_Mods.Private
{
    #region Group Descriptions
    [GroupDescription(Group = "Face Lock", Name = "Variable Face Lock Moves", Description = "(PRIVATE) Allows players to override the default Face Lock attack with custom actions.\nIncompatible with the ModPack's Extended Move Lists.")]
    [GroupDescription(Group = "Stamina Affects Reversals", Name = "Stamina Affects Reversals", Description = "(PRIVATE) Lower stamina increases the chance that a defender will reverse moves.")]
    [GroupDescription(Group = "Reversal Cheer", Name = "Cheer on Reversals", Description = "(PRIVATE) Audience may cheer when a defender reverses a move.")]
    [GroupDescription(Group = "Custom Reversals", Name = "Custom Reversal Moves", Description = "(PRIVATE) Adds functionality to perform Custom Moves as Reversals under certain conditions.\nIncompatible with the ModPack's Extended Move Lists.")]
    [GroupDescription(Group = "TOS Override", Name = "Test of Strength Replacement", Description = "(PRIVATE) Allows players to override the Test of Strength animation with custom actions.\nIncompatible with the ModPack's Extended Move Lists.")]
    [GroupDescription(Group = "Entrance Taunts", Name = "Random Entrance Taunts", Description = "(PRIVATE) Executes random stage taunt for teams in a match.")]
    [GroupDescription(Group = "Dynamic Highlights", Name = "Dynamic Wrestler Highlights", Description = "(PRIVATE) Changes base part highlight levels for wrestlers depending on different conditions.")]
    [GroupDescription(Group = "Modify Plates", Name = "Modify Name Plates", Description = "(PRIVATE) Changes the text displayed on name plates.")]
    //[GroupDescription(Group = "Pin Critical Opponent", Name = "Pin Critical Opponents", Description = "(PRIVATE) Forces edits to pin criticaled opponents under certain conditions.")]
    [GroupDescription(Group = "Waza Support", Name = "Waza Support", Description = "(PRIVATE) Support functionality for Waza")]
    #endregion

    #region Field Access
    [FieldAccess(Class = "FormAnimator", Field = "plObj", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "ProcessKeyInput_Grapple_Run", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "skillInfo", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "CompetitionWorkIdx", Group = "Face Lock")]
    [FieldAccess(Class = "PlayerController_AI", Field = "PlObj", Group = "Face Lock")]
    [FieldAccess(Class = "PlayerController_AI", Field = "Process_OpponentStands_AfterHammerThrow", Group = "Face Lock")]

    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mWaza", Group = "Waza Support")]
    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mData", Group = "Waza Support")]
    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mFileBank", Group = "Waza Support")]
    [FieldAccess(Class = "Menu_CraftLoadSkill", Field = "mPreview", Group = "Waza Support")]
    [FieldAccess(Class = "Data", Field = "WazaData", Group = "Waza Support")]
    [FieldAccess(Class = "ToolSettingInfo", Field = "mData", Group = "Waza Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mWaza", Group = "Waza Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mData", Group = "Waza Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mFileBank", Group = "Waza Support")]
    [FieldAccess(Class = "WazaMenu_CraftLoadSkill", Field = "mPreview", Group = "Waza Support")]
    [FieldAccess(Class = "AnimListData", Field = "dataAccessor", Group = "Waza Support")]
    [FieldAccess(Class = "AnimListData", Field = "mRepeatBlock", Group = "Waza Support")]
    [FieldAccess(Class = "AnimListData", Field = "mRepeatClickBlock", Group = "Waza Support")]
    [FieldAccess(Class = "AnimListData", Field = "m_form", Group = "Waza Support")]
    [FieldAccess(Class = "AnimListData", Field = "_buildinOldForm", Group = "Waza Support")]
    //[FieldAccess(Class = "PlayerController_AI", Field = "IsEffectiveFall", Group = "Pin Critical Opponent")]
    //[FieldAccess(Class = "PlayerController_AI", Field = "AIActFunc_DragDownOpponent", Group = "Pin Critical Opponent")]

    #endregion

    class PrivateOverrides
    {
        #region Form Initialize
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

        [ControlPanel(Group = "Custom Reversals")]
        public static Form ReversalForm()
        {
            if (CustomReversalForm.reversalForm == null)
            {
                return new CustomReversalForm();
            }
            {
                return CustomReversalForm.reversalForm;
            }
        }

        [ControlPanel(Group = "Dynamic Highlights")]
        public static Form HighlightForm()
        {
            if (DynamicHighlightsForm.highlightsForm == null)
            {
                return new DynamicHighlightsForm();
            }
            {
                return DynamicHighlightsForm.highlightsForm;
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

        #region Face Lock override

        #region Variables
        public static Dictionary<String, FaceLockMoves> faceLockMoves = new Dictionary<String, FaceLockMoves>();
        public static SlotStorage[] flSlotStorage = new SlotStorage[8];
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
                flSlotStorage = new SlotStorage[8];
                safeCritSlot = new SkillSlotEnum[8];
                //for (int i = 0; i < 8; i++)
                //{
                //    flSlotStorage[i] = new SlotStorage();
                //    safeCritSlot[i] = SkillSlotEnum.Grapple_XA;
                //}

                for (int i = 0; i < 8; i++)
                {
                    flSlotStorage[i] = new SlotStorage();
                    safeCritSlot[i] = SkillSlotEnum.Grapple_XA;
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

                    flSlotStorage[i].weakSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X];
                    flSlotStorage[i].mediumSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A];
                    flSlotStorage[i].heavySlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B];
                    flSlotStorage[i].criticalSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_XA];

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
                        flSlotStorage[i].criticalSlot = player.WresParam.skillSlot[(int)safeCritSlot[i]];
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
                defender.ChangeState(global::PlStateEnum.NormalAnm);

                if (moveset.Type[damageLevel] == SkillType.BasicMove)
                {
                    //defender.ChangeState(global::PlStateEnum.NormalAnm);
                    attacker.animator.ReqBasicAnm((BasicSkillEnum)moveset.BasicSkills[damageLevel].SkillID, true, attacker.TargetPlIdx);
                }
                else if (moveset.Type[damageLevel] == SkillType.IrishWhip)
                {
                    //defender.ChangeState(global::PlStateEnum.NormalAnm);

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
                                attacker.padOn = PadBtnEnum.Dir_RU;
                            }
                            else
                            {
                                attacker.padOn = PadBtnEnum.Dir_LD;
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


                    //Ensure we're handling skill data correctly
                    //Attempt to change how skills are called
                    //Need to set animator.SkillAnmID
                    //attacker.animator.AnmReqType = AnmReqTypeEnum.SkillID;
                    //attacker.TargetPlIdx = defender.PlIdx;
                    //attacker.lastSkill = slotEnum;
                    //attacker.animator.CurrentSkill = SkillDataMan.inst.GetSkillData((SkillID)moveset.CustomSkills[damageLevel].SkillID)[0];
                    //attacker.animator.InitAnimation();
                    //attacker.lastSkillHit = true;

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
        public static void RefreshSlotMoves_FaceLock(Player player)
        {
            try
            {
                if (!player)
                {
                    return;
                }

                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = flSlotStorage[player.PlIdx].weakSlot;
                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A] = flSlotStorage[player.PlIdx].mediumSlot;
                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B] = flSlotStorage[player.PlIdx].heavySlot;
                player.WresParam.skillSlot[(int)safeCritSlot[player.PlIdx]] = flSlotStorage[player.PlIdx].criticalSlot;
            }
            catch (Exception e)
            {
                L.D("RefreshSlotMoves_FaceLockError: " + e.Message);
            }

        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before,
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

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Face Lock")]
        public static void RefreshAllSlots_FaceLocks()
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
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = flSlotStorage[i].weakSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A] = flSlotStorage[i].mediumSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B] = flSlotStorage[i].heavySlot;
                    player.WresParam.skillSlot[(int)safeCritSlot[player.PlIdx]] = flSlotStorage[player.PlIdx].criticalSlot;
                }
                catch (Exception e)
                {
                    L.D("RefreshAllSlots_FaceLocks - Error on Player " + i + ": " + e.Message);
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

        #endregion

        #region Stamina Affects Reversal Rate

        [Hook(TargetClass = "MatchMisc", TargetMethod = "CheckReversal_Grapple", InjectionLocation = 92,
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

        [Hook(TargetClass = "MatchMisc", TargetMethod = "CheckReversal_Grapple", InjectionLocation = 118,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, ParamTypes = new Type[] { typeof(int), typeof(int), typeof(SkillEquipTypeEnum), typeof(int) },
            Group = "Reversal Cheer")]
        public static void CheerOnReversal(int atk_pl_idx, int skill_idx, global::SkillEquipTypeEnum type, int def_pl_idx)
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

        #region Entrance Taunts
        #region Variables
        public static bool[] stageTaunt;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Entrance Taunts")]
        public static void PrepareTaunts()
        {
            stageTaunt = new Boolean[8];
        }

        [Hook(TargetClass = "EntranceScene", TargetMethod = "Update_ZoomCamera", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "Entrance Taunts")]
        public static void EnterRampVenue(EntranceScene es)
        {
            if (Ring.inst.venueSetting.arenaModel == ArenaModelEnum.Dojo || Ring.inst.venueSetting.arenaModel == ArenaModelEnum.TakafumiCityGym || Ring.inst.venueSetting.arenaModel == ArenaModelEnum.YurakuenHall)
            {
                return;
            }

            //Check the first member to determine if taunts occur
            Player player = PlayerMan.inst.GetPlObj(es.plIdxList[0]);

            EntranceSceneCamera.inst.ChangeMode(EntranceSceneCamera.Mode.ChasePlayer, es.plIdxList[0]);

            //Eliminate bug that occurs with team entrances
            if (stageTaunt[player.PlIdx])
            {
                es.MoveStart_ToRing();
                return;
            }

            //Ensure that we flag the attempt after the initial check
            stageTaunt[player.PlIdx] = true;

            if (UnityEngine.Random.Range(1, 100) <= player.WresParam.aiParam.personalTraits)
            {
                //Play taunt for every player on the team
                int start = 0;
                int end = 0;
                if (es.cornerSide == CornerSide.Blue)
                {
                    start = 0;
                    end = 4;
                }
                else
                {
                    start = 4;
                    end = 8;
                }

                for (int i = start; i < end; i++)
                {
                    player = PlayerMan.inst.GetPlObj(i);
                    if (player)
                    {
                        ExecuteTaunt(player);
                        stageTaunt[player.PlIdx] = true;
                    }
                }
            }

            es.MoveStart_ToRing();
        }

        #endregion

        #region Customize Nameplate Text

        [Hook(TargetClass = "EntranceScene", TargetMethod = "DispName", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "Modify Plates")]
        public static void ModifyNamePlate(EntranceScene es)
        {
            try
            {
                int num = 0;
                if (es.plIdxList[0] >= 4)
                {
                    num = 1;
                }

                //Required to access entrance scene properties
                Type typeFromHandle = typeof(EntranceScene);
                FieldInfo field = typeFromHandle.GetField("namePlate", BindingFlags.Instance | BindingFlags.NonPublic);
                GameObject[] array = (GameObject[])field.GetValue(es);
                GameObject gameObject = array[num];

                MatchSetting matchSetting = global::GlobalWork.inst.MatchSetting;
                String name = "";
                String title = "";
                String seconds = "";
                List<String> teamMembers = new List<String>();
                for (int i = 0; i < es.plNum; i++)
                {
                    global::MatchWrestlerInfo matchWrestlerInfo = matchSetting.matchWrestlerInfo[es.plIdxList[i]];
                    global::WrestlerParam wrestlerParam =
                        global::DataBase.GetWrestlerParam(matchWrestlerInfo.wrestlerID);
                    bool isNickEmpty = wrestlerParam.nickName.Trim() == String.Empty;

                    //Add delimeters to the titles
                    if (i != 0)
                    {
                        if (!matchWrestlerInfo.isSecond)
                        {
                            if (i == es.plNum - 1)
                            {
                                if (!isNickEmpty)
                                {
                                    if (title != String.Empty)
                                    {
                                        title += " & ";
                                    }
                                }

                                if (name != String.Empty)
                                {
                                    name += " & ";
                                }
                            }
                            else
                            {
                                if (!isNickEmpty)
                                {
                                    if (title != String.Empty)
                                    {
                                        title += ", ";
                                    }
                                }

                                if (name != String.Empty)
                                {
                                    name += ", ";
                                }
                            }
                        }
                    }

                    if (!isNickEmpty && !matchWrestlerInfo.isSecond)
                    {
                        title += wrestlerParam.nickName;
                    }

                    //Only add participants to the name plate
                    if (!matchWrestlerInfo.isSecond)
                    {
                        name += CleanUpName(DataBase.GetWrestlerFullName(wrestlerParam));
                        teamMembers.Add(DataBase.GetWrestlerFullName(wrestlerParam));
                    }
                    else
                    {
                        if (seconds != String.Empty)
                        {
                            seconds += " & ";
                        }

                        seconds += CleanUpName(DataBase.GetWrestlerFullName(wrestlerParam));
                    }

                }

                //Avoid over-writing team names set-up by other mods
                String dbTeamName = "";
                if (teamMembers.Count > 1)
                {
                    GetTeamName(teamMembers, out dbTeamName);
                    if (dbTeamName == String.Empty)
                    {
                        gameObject.transform.FindChild("Text_Name").gameObject.GetComponent<Text>().text = name;

                        //If title information exists, avoid over-writing it.
                        if (gameObject.transform.FindChild("Text_Title").gameObject.GetComponent<Text>().text.Trim() ==
                            String.Empty)
                        {
                            gameObject.transform.FindChild("Text_Title").gameObject.GetComponent<Text>().text = title;
                        }
                    }
                    else
                    {
                        gameObject.transform.FindChild("Text_Name").gameObject.GetComponent<Text>().text = dbTeamName;

                        //If title information exists, avoid over-writing it.
                        if (gameObject.transform.FindChild("Text_Title").gameObject.GetComponent<Text>().text.Trim() ==
                            String.Empty)
                        {
                            gameObject.transform.FindChild("Text_Title").gameObject.GetComponent<Text>().text = name;
                        }
                    }
                }
                else
                {
                    //For single wrestlers, we only need to set the modified name.
                    gameObject.transform.FindChild("Text_Name").gameObject.GetComponent<Text>().text = name;

                    //If title information exists, avoid over-writing it.
                    if (gameObject.transform.FindChild("Text_Title").gameObject.GetComponent<Text>().text.Trim() ==
                        String.Empty)
                    {
                        gameObject.transform.FindChild("Text_Title").gameObject.GetComponent<Text>().text = title;
                    }
                }

                //Add seconds to the team
                if (seconds != String.Empty)
                {
                    gameObject.transform.FindChild("Text_Name").gameObject.GetComponent<Text>().text +=
                        " w/ " + seconds;
                }
            }
            catch (Exception e)
            {
                L.D("ModifyNamePlateError: " + e);
            }

        }

        #endregion

        #region Dynamic Highlights

        public static WrestlerAppearanceData[] origAppear = new WrestlerAppearanceData[8]
            {null, null, null, null, null, null, null, null};

        public static int[] timeInMatch = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] SweatLevel = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int SweatSpeed = 30;
        public static int SweatLvl = 1;
        public static bool EnableSweat;

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Dynamic Highlights")]
        public static void StoreOriginalWAD()
        {
            try
            {
                try
                {
                    EnableSweat = DynamicHighlightsForm.highlightsForm.eh_enableSweat.Checked;
                }
                catch
                {
                    EnableSweat = false;
                    L.D("Failed to check enable sweat");
                }

                origAppear = new WrestlerAppearanceData[8] { null, null, null, null, null, null, null, null };
                SweatLevel = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                try
                {
                    int.TryParse((String)DynamicHighlightsForm.highlightsForm.eh_sweatSpeed.SelectedItem, out SweatSpeed);
                    int.TryParse((String)DynamicHighlightsForm.highlightsForm.eh_sweatLevel.SelectedItem, out SweatLvl);

                }
                catch (NullReferenceException)
                {
                    L.D("Error when attempting to parse the form fields");
                }

                if (SweatSpeed <= 0)
                {
                    SweatSpeed = 1;
                }

                MatchWrestlerInfo[] mwi = GlobalWork.inst.MatchSetting.matchWrestlerInfo;
                for (int i = 0; i < 8; i++)
                {
                    if (mwi[i] == null)
                    {
                        continue;
                    }
                    if (mwi[i].entry)
                    {
                        origAppear[i] = DataBase.GetAppearanceData(mwi[i].wrestlerID);

                        //Determine if a default highlight should be set for all edits
                        if (DynamicHighlightsForm.highlightsForm.eh_isDefaultLevel.Checked)
                        {
                            CostumeData cd = new CostumeData();
                            if (cd == null)
                            {
                                continue;
                            }
                            cd.Set(DataBase.GetCostumeData(mwi[i].wrestlerID, mwi[i].costume_no));
                            for (int p = 0; p < 9; p++)
                            {
                                cd.highlightIntensity[p, 0] = SweatLvl / 100;
                                try
                                {
                                    var plObj = PlayerMan.inst.GetPlObj(i);
                                    if (plObj == null)
                                    {
                                        continue;
                                    }
                                    plObj.FormRen.DestroySprite();
                                    plObj.FormRen.InitTexture(cd);
                                    plObj.FormRen.InitSprite();
                                }
                                catch (Exception e)
                                {
                                    L.D("Error updating initial highlights: " + e);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                L.D("StoreOriginalWAD: " + e);
            }

        }

        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "Dynamic Highlights")]
        public static void TimeInMatch(Player plObj)
        {
            if (!EnableSweat)
            {
                return;
            }

            if (MatchMain.inst == null)
            {
                return;
            }

            if (MatchMain.inst.Pause)
            {
                return;
            }

            if (plObj.hasRight || plObj.isSallying)
            {
                float bpct = MatchMisc.GetParamRate(plObj.BP);
                int sp = 0;
                if (bpct < 90)
                {
                    sp += SweatSpeed;
                }

                if (bpct < 70)
                {
                    sp += SweatSpeed;
                }

                if (bpct < 50)
                {
                    sp += SweatSpeed;
                }

                if (bpct < 30)
                {
                    sp += SweatSpeed;
                }

                if (bpct < 10)
                {
                    sp += SweatSpeed;
                }

                timeInMatch[plObj.PlIdx] += sp;
            }

            if (plObj.isTagPartnerStandby && MatchMain.inst.matchTime.frm % (5) == 0)
            {
                timeInMatch[plObj.PlIdx]--;
            }

            int minsInMatch = GetMins(timeInMatch[plObj.PlIdx]);
            int[] sweatLevelTbl = new int[10] { 7, 14, 20, 22, 27, 30, 35, 45, 50, 60 };

            int sweatLevel = 0;
            for (int i = 0; i < 10; i++)
            {
                if (minsInMatch >= sweatLevelTbl[i])
                {
                    sweatLevel++;
                }
                else
                {
                    break;
                }
            }

            if (sweatLevel > SweatLevel[plObj.PlIdx])
            {
                MatchWrestlerInfo mwi = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                CostumeData cd = new CostumeData();
                cd.Set(DataBase.GetCostumeData(mwi.wrestlerID, mwi.costume_no));

                SweatLevel[plObj.PlIdx] = sweatLevel;

                for (int p = 0; p < 9; p++)
                {
                    cd.highlightIntensity[p, 0] += 0.1f;
                    if (cd.highlightIntensity[p, 0] > 1f)
                    {
                        cd.highlightIntensity[p, 0] = 1f;
                    }
                }

                plObj.FormRen.DestroySprite();
                plObj.FormRen.InitTexture(cd);
                plObj.FormRen.InitSprite();
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitRound", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal,
            Group = "Dynamic Highlights")]
        public static void TowelOff(int round)
        {
            if (round == 0)
            {
                return;
            }

            for (int i = 0; i < 8; i++)
            {
                if (timeInMatch[i] > 0)
                {
                    timeInMatch[i] /= 2;
                }
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Dynamic Highlights")]
        public static void RestoreAppearanceData()
        {
            MatchWrestlerInfo[] mwi = GlobalWork.inst.MatchSetting.matchWrestlerInfo;
            for (int i = 0; i < 8; i++)
            {
                if (origAppear[i] != null)
                {
                    if (mwi[i] != null)
                    {
                        if (mwi[i].wrestlerID < WrestlerID.EditWrestlerIDTop)
                        {
                            PresetWrestlerData preset =
                                PresetWrestlerDataMan.inst.GetPresetWrestlerData(mwi[i].wrestlerID);
                            preset.appearance.Set(origAppear[i]);
                        }
                        else
                        {
                            EditWrestlerData edit = SaveData.inst.GetEditWrestlerData(mwi[i].wrestlerID);
                            edit.appearanceData.Set(origAppear[i]);
                        }
                    }
                }
            }
        }

        #endregion

        #region Implement Fake Reversal Moves

        #region Variables
        public static int maxReversalChance = 50;
        public static SkillData[] moveData;
        public static SlotStorage[] rvSlotStorage = new SlotStorage[8];
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Custom Reversals")]
        public static void StoreReversalSlotMoves()
        {
            rvSlotStorage = new SlotStorage[8];
            for (int i = 0; i < 8; i++)
            {
                rvSlotStorage[i] = new SlotStorage();

                Player player = PlayerMan.inst.GetPlObj(i);
                if (!player)
                {
                    continue;
                }

                //Ensure that we're storing the initial slot moves for refreshing
                rvSlotStorage[i].weakSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X];
                rvSlotStorage[i].mediumSlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A];
                rvSlotStorage[i].heavySlot = player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B];
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "ProcessKeyInput_Grapple", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance,
            Group = "Custom Reversals")]
        public static void RefreshSlotMoves_Reversals(Player player)
        {
            try
            {
                if (!player)
                {
                    return;
                }

                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = rvSlotStorage[player.PlIdx].weakSlot;
                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A] = rvSlotStorage[player.PlIdx].mediumSlot;
                player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B] = rvSlotStorage[player.PlIdx].heavySlot;
            }
            catch (Exception e)
            {
                L.D("RefreshSlotMoves_ReversalError: " + e.Message);
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Custom Reversals")]
        public static void RefreshAllSlots_Reversals()
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
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_X] = rvSlotStorage[i].weakSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_A] = rvSlotStorage[i].mediumSlot;
                    player.WresParam.skillSlot[(int)SkillSlotEnum.Grapple_B] = rvSlotStorage[i].heavySlot;
                }
                catch (Exception e)
                {
                    L.D("RefreshAllSlots_Reversals - Error on Player " + i + ": " + e.Message);
                }

            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "Custom Reversals")]
        public static void PrepareCustomReversalArray()
        {
            moveData = new SkillData[8];
        }

        //Only works for Front Grapples and Corner Grapples currently
        [Hook(TargetClass = "Player", TargetMethod = "ProcessKeyInput_Grapple", InjectionLocation = 234,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.ModifyReturn | HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 2 }, Group = "Custom Reversals")]
        public static bool SetFakeFrontGrappleReversal(Player attacker, ref SkillSlotEnum skillSlotEnum)
        {
            try
            {
                //Ensure animListData is a valid slot type
                if (skillSlotEnum != SkillSlotEnum.Grapple_X && skillSlotEnum != SkillSlotEnum.Grapple_X_LR &&
                    skillSlotEnum != SkillSlotEnum.Grapple_X_U && skillSlotEnum != SkillSlotEnum.Grapple_X_D &&
                    skillSlotEnum != SkillSlotEnum.Grapple_A && skillSlotEnum != SkillSlotEnum.Grapple_A_LR &&
                    skillSlotEnum != SkillSlotEnum.Grapple_A_U && skillSlotEnum != SkillSlotEnum.Grapple_A_D &&
                    skillSlotEnum != SkillSlotEnum.Grapple_B && skillSlotEnum != SkillSlotEnum.Grapple_B_LR &&
                    skillSlotEnum != SkillSlotEnum.Grapple_B_U && skillSlotEnum != SkillSlotEnum.Grapple_B_D &&
                    skillSlotEnum != SkillSlotEnum.Grapple_XA)
                {
                    L.D("Invalid skill slot: " + skillSlotEnum);
                    return false;
                }

                var moves = CustomReversalForm.fgrappleMoves;
                if (moves == null)
                {
                    return false;
                }

                //Replacing front grapple move
                Player defender = PlayerMan.inst.GetPlObj(attacker.TargetPlIdx);

                //Ensure animListData is a valid move;
                SkillData skill = attacker.GetSkillData_Equip(skillSlotEnum);
                string skillName = DataBase.GetSkillName(attacker.WresParam.skillSlot[(int)skillSlotEnum]);

                //Determine if Skill Exists
                int skillIndex = CustomReversalForm.FindMoveIndex(skillName, moves);
                if (skillIndex < 0)
                {
                    return false;
                }

                var move = GetRandomMove(moves[skillIndex].ReplacementsList);
                if (move == null)
                {
                    return false;
                }

                if (CheckForReversal(attacker, defender, skill))
                {
                    int skillID = move.ID;
                    SkillData skillData = global::SkillDataMan.inst.GetSkillData((SkillID)skillID)[0];
                    L.D("Replaced " + skillName + " with " + DataBase.GetSkillName((SkillID)skillID));

                    skillSlotEnum = ChooseSkillSlot(skillSlotEnum);

                    L.D("Using skill slot: " + skillSlotEnum);

                    //Determine whether the attacker or defender executes the move
                    if (move.User == Executor.Attacker)
                    {
                        moveData[attacker.PlIdx] = skillData;
                        attacker.WresParam.skillSlot[(int)skillSlotEnum] = (SkillID)skillID;
                        L.D("Executor (Attacker): " + DataBase.GetWrestlerFullName(attacker.WresParam));
                        return false;
                    }
                    else
                    {
                        moveData[defender.PlIdx] = skillData;
                        defender.WresParam.skillSlot[(int)skillSlotEnum] = (SkillID)skillID;
                        L.D("Executor (Defender): " + DataBase.GetWrestlerFullName(defender.WresParam));

                        //If the defender needs to execute a reversal move, we call the animator for it
                        //Then we return true to ensure that the attacker's animator does not proceed.
                        attacker.State = PlStateEnum.NormalAnm;
                        defender.animator.ReqSlotAnm(skillSlotEnum, true, attacker.PlIdx, true);
                        MatchEvaluation.inst.SkillHit(defender.PlIdx, skillSlotEnum);
                        defender.lastSkillHit = true;
                        defender.SetGrappleResult(attacker.PlIdx, true);
                        attacker.SetGrappleResult(defender.PlIdx, false);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                L.D("SetFakeFrontGrappleReversal: " + e);
            }

            return false;
        }

        [Hook(TargetClass = "FormAnimator", TargetMethod = "InitAnimation", InjectionLocation = 135,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "Custom Reversals")]
        public static void ReplaceSkillWithReversal(FormAnimator animator)
        {
            try
            {
                if (animator.plObj == null || moveData == null)
                {
                    return;
                }

                //Ensure that the priority chain is not broken
                if (moveData[animator.plObj.PlIdx] != null)
                {
                    animator.CurrentSkill = moveData[animator.plObj.PlIdx];
                    moveData[animator.plObj.PlIdx] = null;
                }
            }
            catch (Exception e)
            {
                L.D("ReplaceSkillWithReversalException: " + e);
            }

        }
        #endregion

        #region Waza Support

        #region Variables
        public static int maxCFormNum = 99999;
        public static int maxCFormID = 100000 + maxCFormNum;
        #endregion

        [Hook(TargetClass = "WazaMenu_CraftLoadSkill", TargetMethod = "SetActiveBackObj", InjectionLocation = 0,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "Waza Support")]
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
            Group = "Waza Support")]
        public static void AddCustomForms(WazaMenu_CraftLoadSkill craftSkill, SkillID skill_id)
        {
            try
            {
                //Ensure that only Custom Moves are processed
                //if ((int)skill_id >= 6660 && (int)skill_id <= 10000)
                //Allow moves exported in Move Craft to be processed as well
                L.D("Checking skill: " + DataBase.GetSkillName(skill_id) + " with ID " + skill_id);
                if ((int)skill_id >= 6660)
                {
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
            Group = "Waza Support")]
        public static bool IncreaseMaxCFormLimit(AnimListData animListData, out bool result, string name, int diff, bool pad, bool self, ref int repeatBlock)
        {
            result = false;
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
                if (!validFormOps.Contains(name)) //&& !name.Equals("FORMTYPE"))
                {
                    return result;
                }
                else if (validFormOps.Contains(name))
                {
                    int preForm = animListData.GetFORM();
                    int max = (preForm < 100000) ? global::AnimListConst.FORMMax : ((animListData.dataAccessor.GetWazaData().formEditIdx <= maxCFormNum) ? (animListData.dataAccessor.GetWazaData().formEditIdx + 100000) : maxCFormID);
                    //int max = (preForm < 100000) ? global::AnimListConst.FORMMax : ((animListData.dataAccessor.GetWazaData().formEditIdx <= 1540) ? (animListData.dataAccessor.GetWazaData().formEditIdx + 100000) : 101540);
                    //int max = (preForm < 100000) ? global::AnimListConst.FORMMax : ((animListData.dataAccessor.GetWazaData().formEditIdx <= 1540) ? (animListData.dataAccessor.GetWazaData().formEditIdx + 100000) : 101540);
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
                //else if (name.Equals("FORMTYPE"))
                //{
                //    check = false;
                //    int _form = animListData.GetFORM();
                //    int formIdx = animListData.GetFORM();
                //    int oldForm = _form;
                //    int oldBuildIn = animListData._buildinOldForm;
                //    int formEditIdx = animListData.dataAccessor.GetWazaData().formEditIdx;
                //    if (pad)
                //    {
                //        if (self)
                //        {
                //            animListData.mRepeatBlock |= 3;
                //        }
                //        else
                //        {
                //            repeatBlock |= 3;
                //        }
                //    }
                //    else
                //    {
                //        animListData.mRepeatClickBlock |= 3;
                //    }
                //    global::CommandManager.Instance.Do(new global::Command(delegate
                //    {
                //        //Custom to Preset Forms
                //        if (_form >= 100000)
                //        {
                //            L.D("Case 1");
                //            _form = animListData._buildinOldForm;
                //            global::FormNumber.Instance.CheckDeleteInitData(oldForm, _form);
                //        }
                //        //Preset to Custom Forms
                //        else
                //        {
                //            L.D("Case 2");
                //            animListData._buildinOldForm = oldForm;
                //            //_form = 100000 + animListData.dataAccessor.GetWazaData().formEditIdx;
                //            _form = 100000 + oldForm;
                //            L.D("oldForm: " + oldForm);
                //            L.D("_form: " + _form);
                //            if (_form > maxCFormID)
                //            {
                //                _form = maxCFormID;
                //            }

                //            //Determine whether a custom form already exists
                //            bool formExists = false;
                //            var instance = FormNumber.Instance;
                //            var toolSkillData = instance.mData.GetWazaData().toolFormSaveList;
                //            foreach (var data in toolSkillData)
                //            {
                //                if (data.formNo == _form)
                //                {
                //                    formExists = true;
                //                }
                               
                //                break;
                //            }


                //            if (formExists)
                //            {
                //                L.D(_form + " exists");
                //                instance.mData.GetWazaData().changeData = true;
                //            }
                //            else
                //            {
                //                L.D(_form + " does not exists");
                //                instance.InitPaste(oldForm, _form);
                //            }

                //        }
                //        if (_form != oldForm)
                //        {
                //            L.D("Case 3");
                //            animListData.SetFORM(_form, true);
                //            animListData.SetFormTextColor(animListData.GetFormTextColor());
                //            animListData.DataUpdateFORM();
                //            animListData.SetupForm();
                //            change = true;
                //        }
                //    }, delegate
                //    {
                //        global::AnimList.Context.selectMin = selectMin;
                //        global::AnimList.Context.selectMax = selectMax;
                //        global::AnimList.Instance.RefreshSelect();
                //        global::AnimBankSetting.Context.bank.number = _bank;
                //        global::AnimBankSetting.Context.bank.no[_bank] = anmBankNo;
                //        animListData.m_form = oldForm;
                //        animListData.SetupForm();
                //        int num8 = oldForm;
                //        if (num8 >= 100000)
                //        {
                //            L.D("Case 4");
                //            if (num8 == 100000 + formEditIdx)
                //            {
                //                global::FormNumber.Instance.InitPaste(animListData._buildinOldForm, num8);
                //            }
                //            animListData._buildinOldForm = oldBuildIn;
                //        }
                //        else
                //        {
                //            L.D("Case 5");
                //            num8 = animListData._buildinOldForm;
                //            global::FormNumber.Instance.CheckDeleteInitData(num8, oldForm);
                //        }
                //        animListData.SetFORM(num8, true);
                //        animListData.SetFormTextColor(animListData.GetFormTextColor());
                //        animListData.DataUpdateFORM();
                //        animListData.SetupForm();
                //    }, delegate
                //    {
                //        global::AnimList.Context.selectMin = selectMin;
                //        global::AnimList.Context.selectMax = selectMax;
                //        global::AnimList.Instance.RefreshSelect();
                //        global::AnimBankSetting.Context.bank.number = _bank;
                //        global::AnimBankSetting.Context.bank.no[_bank] = anmBankNo;
                //        animListData.m_form = formIdx;
                //        animListData.SetupForm();
                //        int num8 = oldForm;
                //        if (num8 >= 100000)
                //        {
                //            L.D("Case 6");
                //            num8 = animListData._buildinOldForm;
                //            global::FormNumber.Instance.CheckDeleteInitData(oldForm, num8);
                //        }
                //        else
                //        {
                //            L.D("Case 7");
                //            animListData._buildinOldForm = oldForm;
                //            //num8 = 100000 + animListData.dataAccessor.GetWazaData().formEditIdx;
                //            num8 += 100000;
                //            if (num8 > maxCFormID)
                //            {
                //                num8 = maxCFormID;
                //            }
                //            global::FormNumber.Instance.InitPaste(oldForm, num8);
                //        }
                //        if (num8 != oldForm)
                //        {
                //            L.D("Case 8");
                //            animListData.SetFORM(num8, true);
                //            animListData.SetFormTextColor(animListData.GetFormTextColor());
                //            animListData.DataUpdateFORM();
                //            animListData.SetupForm();
                //            change = true;
                //        }
                //    }));
                //}
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

        #endregion

        #region Test Of Strength Override

        //Attacker freezes up when executing the move.
        [Hook(TargetClass = "Player", TargetMethod = "CheckStartPowerCompetition", InjectionLocation = 74, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "TOS Override")]
        public static bool ReplaceTestOfStrength(Player attacker, out bool result)
        {
            //If animListData method returns true, then the result should be false.
            //Otherwise, the code will continue if animListData method returns false.
            result = false;

            try
            {
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
                    return false;
                }

                if (defender.SP == attacker.SP)
                {
                    if (defender.HP > attacker.HP)
                    {
                        return false;
                    }
                }
                else if (defender.SP > attacker.SP)
                {
                    return false;
                }

                List<Skill> tosMoves = new List<Skill>();

                //Determine whether we should pull from the Wrestler or Style list
                String style = winner.WresParam.fightStyle.ToString();
                String name = DataBase.GetWrestlerFullName(winner.WresParam);

                L.D("Wrestler is " + name + ".\nStyle is " + style);

                foreach (TOSMoves move in TOSForm.form.tos_wrestlers.Items)
                {
                    L.D("Checking " + move.Name + " with " + move.Skills.Count + " moves.");
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
                        L.D("Checking " + move.Name + " with " + move.Skills.Count + " moves.");
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

                //Ensure that animListData move exists
                if (DataBase.GetSkillName((SkillID)skillID).Equals(String.Empty))
                {
                    return false;
                }

                winner.animator.AnmReqType = AnmReqTypeEnum.SkillID;
                global::SkillData skillData = global::SkillDataMan.inst.GetSkillData((SkillID)skillID)[0];

                //Ensure that both players return to neutral position
                attacker.ChangeState(global::PlStateEnum.NormalAnm);
                defender.ChangeState(global::PlStateEnum.NormalAnm);

                //moveData[winner.PlIdx] = skillData;

                var skillSlot = SkillSlotEnum.Grapple_X;

                winner.lastSkill = skillSlot;

                winner.WresParam.skillSlot[(int)skillSlot] = (SkillID)skillID;
                winner.animator.ReqSlotAnm(skillSlot, false, -1, true);
                winner.lastSkillHit = true;

                return true;
            }
            catch (Exception e)
            {
                L.D("ReplaceTestOfStrengthException: " + e);
                return false;
            }

        }

        #endregion

        #region Helper Methods
        public static WresIDGroup GetWresIDGroup(String wrestlerName)
        {
            WresIDGroup wresID = null;
            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                if (wrestlerName.Equals(DataBase.GetWrestlerFullName(current.wrestlerParam)))
                {
                    wresID = new WresIDGroup
                    {
                        Name = DataBase.GetWrestlerFullName(current.wrestlerParam),
                        ID = (Int32)current.editWrestlerID,
                        Group = current.wrestlerParam.groupID
                    };
                    break;
                }
            }
            return wresID;
        }
        public static bool CheckForReversal(Player attacker, Player defender, SkillData skill)
        {
            //Reversal chance starts at 5 % as a base, then has an opportunity to increase to a maximum of 20 %
            bool isReversal = false;
            int reversalChance = 5;

            //Attacker and Defender HP, if Defender is in a lower damage level than increase reversal chance by 5 %.
            if (GetDamageLevel(attacker) > GetDamageLevel(defender))
            {
                reversalChance += 5;
            }

            int attackerParam = attacker.WresParam.atkParam[(int)skill.atkPrmType_Main];
            int defenderParam = defender.WresParam.defParam[(int)skill.defPrmType_Main];

            //Attacker and Defender parameters for move's main attribute, if Defender has higher defense than increase reversal chance by 5%.
            if (defenderParam >= attackerParam)
            {
                reversalChance += 5;
            }

            //Attacker and Defender sizes, add 5 % for every size level above the Attacker.
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            int attackerSize = (int)DataBase.GetAppearanceData(settings.matchWrestlerInfo[attacker.PlIdx].wrestlerID).formSize;
            int defenderSize = (int)DataBase.GetAppearanceData(settings.matchWrestlerInfo[defender.PlIdx].wrestlerID).formSize;

            if (attackerSize == (int)FormSizeEnum.Female)
            {
                attackerSize = -1;
            }

            if (defenderSize == (int)FormSizeEnum.Female)
            {
                defenderSize = -1;
            }

            int sizeDifference = defenderSize - attackerSize;
            if (sizeDifference > 0)
            {
                reversalChance += (5 * sizeDifference);
            }

            //Determine if there's a weight difference for throw/power/arm moves
            if (skill.atkPrmType_Main == AtkPrmType.ArmPower || skill.atkPrmType_Main == AtkPrmType.Throw ||
                skill.atkPrmType_Main == AtkPrmType.HorsePower)
            {
                var moveType = skill.atkPrmType_Main;
                int weightDiff = (defender.WresParam.weight - attacker.WresParam.weight) / 50;
                if (weightDiff > attacker.WresParam.atkParam[(int)moveType])
                {
                    reversalChance += (5 * (weightDiff - attacker.WresParam.atkParam[(int)moveType]));
                }
            }
            //Roll a random number between 1 - 100, if the number is less than or equal to the reversal factor then trigger the move.

            if (reversalChance > maxReversalChance)
            {
                reversalChance = maxReversalChance;
            }

            if (UnityEngine.Random.Range(0, 100) <= reversalChance)
            {
                isReversal = true;
            }

            return isReversal;
        }
        private static SkillSlotEnum ChooseSkillSlot(SkillSlotEnum skillSlotEnum)
        {
            //Using a standard slot to ensure it's refreshed properly
            if (skillSlotEnum >= SkillSlotEnum.Grapple_X && skillSlotEnum <= SkillSlotEnum.Grapple_X_D)
            {
                return SkillSlotEnum.Grapple_X;
            }
            else if (skillSlotEnum >= SkillSlotEnum.Grapple_A && skillSlotEnum <= SkillSlotEnum.Grapple_A_D)
            {
                return SkillSlotEnum.Grapple_A;
            }
            else
            {
                return SkillSlotEnum.Grapple_B;
            }
        }
        private static Move GetRandomMove(List<Move> moves)
        {
            if (moves.Count == 0)
            {
                return null;
            }

            int index = UnityEngine.Random.Range(0, moves.Count);
            return moves[index];
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
        private static void ExecuteTaunt(Player plObj)
        {
            int randomNum = UnityEngine.Random.Range(1, 5);
            if (randomNum == 1)
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_1, 0, true, plObj.PlIdx);
            }
            else if (randomNum == 2)
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_2, 0, true, plObj.PlIdx);
            }
            else if (randomNum == 3)
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_3, 0, true, plObj.PlIdx);
            }
            else
            {
                plObj.animator.StartSlotAnm_Immediately(SkillSlotEnum.Performance_4, 0, true, plObj.PlIdx);
            }
        }
        public static int[] GetPlayerList()
        {
            List<int> players = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);
                if (plObj)
                {
                    if (!plObj.isIntruder)
                    {
                        players.Add(plObj.PlIdx);
                    }
                }
            }

            return players.ToArray();
        }
        private static int GetMins(int frms)
        {
            int result = frms / 60;
            result /= 60;
            return result;
        }
        public static String CleanUpName(String name)
        {
            int length = name.IndexOf("(");
            if (length < 0)
            {
                return name;
            }
            else
            {
                return name.Substring(0, length);
            }
        }
        public static void GetTeamName(List<String> wrestlers, out String teamName)
        {
            List<String> teams = new List<String>();
            foreach (Team currentTeam in ModPack.ModPack.Teams)
            {
                if (wrestlers.Count == 1)
                {
                    break;
                }
                if (Contains(wrestlers, currentTeam.Members))
                {
                    teams.Add(currentTeam.Name);
                }
            }

            //No teams found
            if (teams.Count > 0)
            {
                teamName = teams[UnityEngine.Random.Range(0, teams.Count)];
            }
            else
            {
                if (wrestlers.Count == 1)
                {
                    teamName = CleanUpName(wrestlers[0]);
                }
                else
                {
                    teamName = String.Empty;
                }
            }
        }
        public static bool Contains(List<string> champs, List<string> members)
        {
            bool result;
            if (champs.Count <= members.Count)
            {
                foreach (string current in champs)
                {
                    if (!members.Contains(current))
                    {
                        result = false;
                        return result;
                    }
                }
            }
            else
            {
                foreach (string current2 in members)
                {
                    if (!champs.Contains(current2))
                    {
                        result = false;
                        return result;
                    }
                }
            }
            result = true;
            return result;
        }

        #endregion
    }
}
