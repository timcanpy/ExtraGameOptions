using DG;
using ModPack;
using QoL_Mods.Data_Classes;
using QoL_Mods.Data_Classes.Facelock;
using QoL_Mods.Data_Classes.Reversal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using QoL_Mods.Helper_Classes;
using UnityEngine;
using UnityEngine.UI;
using static Common_Classes.EnumLibrary;
using System.IO;
using MatchConfig;
using WresIDGroup = ModPack.WresIDGroup;

namespace QoL_Mods.Private
{
    #region Group Descriptions
    [GroupDescription(Group = "Face Lock", Name = "Variable Face Lock Moves", Description = "(PRIVATE) Allows players to override the default Face Lock attack with custom actions.")]
    [GroupDescription(Group = "Stamina Affects Reversals", Name = "Stamina Affects Reversals", Description = "(PRIVATE) Lower stamina increases the chance that a defender will reverse moves.")]
    [GroupDescription(Group = "Reversal Cheer", Name = "Cheer on Reversals", Description = "(PRIVATE) Audience may cheer when a defender reverses a move.")]
    [GroupDescription(Group = "Custom Reversals", Name = "Custom Reversal Moves", Description = "(PRIVATE) Adds functionality to perform Custom Moves as Reversals under certain conditions.")]
    [GroupDescription(Group = "Fly Range", Name = "Increase Fly Range", Description = "(PRIVATE) Increases Fly Range for Juniors/Lucha/Panther styles.")]
    [GroupDescription(Group = "Entrance Taunts", Name = "Random Entrance Taunts", Description = "(PRIVATE) Executes random stage taunt for teams in a match.")]
    [GroupDescription(Group = "Dynamic Highlights", Name = "Dynamic Wrestler Highlights", Description = "(PRIVATE) Changes base part highlight levels for wrestlers depending on different conditions.")]
    [GroupDescription(Group = "Modify Plates", Name = "Modify Name Plates", Description = "(PRIVATE) Changes the text displayed on name plates.")]
    //[GroupDescription(Group = "Pin Critical Opponent", Name = "Pin Critical Opponents", Description = "(PRIVATE) Forces edits to pin criticaled opponents under certain conditions.")]
    [GroupDescription(Group = "Dynamic Attendance", Name = "Dynamic Attendance Level", Description = "(PRIVATE) Set the Attendance Level based on participating edits' Rank & Charisma.")]
    [GroupDescription(Group = "Ring Config", Name = "Automatic Ring Configuration", Description = "(PRIVATE) Automates match settings for different rings.")]
    #endregion

    #region Field Access
    [FieldAccess(Class = "FormAnimator", Field = "plObj", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "ProcessKeyInput_Grapple_Run", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "skillInfo", Group = "Face Lock")]
    [FieldAccess(Class = "Player", Field = "CompetitionWorkIdx", Group = "Face Lock")]
    [FieldAccess(Class = "PlayerController_AI", Field = "PlObj", Group = "Face Lock")]
    [FieldAccess(Class = "PlayerController_AI", Field = "Process_OpponentStands_AfterHammerThrow", Group = "Face Lock")]

    [FieldAccess(Class = "PlayerController_AI", Field = "IsEffectiveFall", Group = "Pin Critical Opponent")]
    [FieldAccess(Class = "PlayerController_AI", Field = "AIActFunc_DragDownOpponent", Group = "Pin Critical Opponent")]
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
        #endregion

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
                player.WresParam.skillSlot[(int)safeCritSlot[player.PlIdx]] = slotStorage[player.PlIdx].criticalSlot;
            }
            catch (Exception e)
            {
                L.D("RefreshSlotMoves Error: " + e.Message);
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

        public static int maxReversalChance = 50;
        public static SkillData[] moveData;

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
                //Ensure this is a valid slot type
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

                //Ensure this is a valid move;
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
                        //DispNotification.inst.Show(DataBase.GetWrestlerFullName(defender.WresParam) + " reversed with a " + DataBase.GetSkillName((SkillID)skillID) + "!", 120);
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

        [Hook(TargetClass = "FormAnimator", TargetMethod = "InitAnimation", InjectionLocation = 115,
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

        #region WIP
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

                        ////Referee
                        //if (config.Referees.Count > 0)
                        //{
                        //    RefereeInfo referee = config.Referees[UnityEngine.Random.Range(0, config.Referees.Count)];
                        //    L.D("Adding " + referee.Data.Prm.name + " with id " + referee.Data.editRefereeID);
                        //    settings.RefereeID = referee.Data.editRefereeID;
                        //}

                        //Grapple Settings
                        ModPackForm.instance.numericUpDown5.Value = config.GrappleSetting.Low;
                        ModPackForm.instance.numericUpDown6.Value = config.GrappleSetting.Medium;
                        ModPackForm.instance.numericUpDown7.Value = config.GrappleSetting.High;
                     
                        //Clock Speed
                        if (GetWrestlerList().Length == 2)
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

        [Hook(TargetClass = "Menu_SoundManager", TargetMethod = "MyMusic_Play", InjectionLocation = 0,
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
        #endregion
        #endregion

        #region Dynamic Crowd Based on Participating Edits
        [Hook(TargetClass = "MatchMain", TargetMethod = "InitArena", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None,
            Group = "Dynamic Attendance")]
        public static void SetAttendanceLevel()
        {
            try
            {
                double appeal = GetAverageCharisma() + GetAverageRank();

                //Title matches gain extra Audience Appeal
                if (GlobalParam.TitleMatch_BeltData != null)
                    appeal += 2;

                if (appeal < 0)
                {
                    appeal = 0;
                }
                else if (appeal > 10)
                {
                    appeal = 10;
                }

                L.D("Audience Appeal: " + appeal);

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
        #endregion

        #region Helper Methods

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
                    teamName = wrestlers[0];
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