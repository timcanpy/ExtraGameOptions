using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using Data_Classes;
using DG;
using JetBrains.Annotations;
using MatchConfig;
using UnityEngine;
using UnityEngine.UI;
using ModPack;

namespace FireProWar
{
    #region Description and Field Access
    [GroupDescription(Description = "This adds a system for competitve booking among various promotions, as well as the management of roster morale.", Group = "FirePro War", Name = "Fire Promotion Tracker")]
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "FirePro War")]
    [FieldAccess(Class = "Player", Field = "InvokeUkeBonus", Group = "FirePro War")]
    [FieldAccess(Class = "MatchMain", Field = "EndMatch", Group = "FirePro War")]
    [FieldAccess(Class = "Menu_Result", Field = "Start", Group = "FirePro War")]

    #endregion
    public class Management
    {
        #region Form Intialization
        [ControlPanel(Group = "FirePro War")]
        public static Form MSForm()
        {
            if (War_Form.form == null)
            {
                return new War_Form();
            }
            {
                return War_Form.form;
            }
        }
        #endregion

        #region Variables
        public static bool fpwEnable = false;
        public static Employee[] employeeData = new Employee[8];
        public static String ringName = "";
        public static Promotion promotion = null;
        public static String storedLogoPath = "./EGOData/Watermarks/";
        public static String logoAssetLocation = "./EGOData/tvlogo.obj";
        public static String logoPositionFile = "./EGOData/LogoPosition.dat";
        public static String defaultLogo = "Default.png";
        public static AssetBundle tvLogo = null;
        public static String[] teamNames = new String[2];
        public static bool hasChamp = false;
        public static TitleMatch_Data[] titleData = new TitleMatch_Data[8];
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "InitMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "FirePro War")]
        public static void SetEmployees()
        {
            ringName = "";
            promotion = null;
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;
            fpwEnable = War_Form.form.fpw_Enable.Checked;
            if (fpwEnable)
            {
                //Disable for Battle Royales
                if (settings.BattleRoyalKind != BattleRoyalKindEnum.Off)
                {
                    promotion = null;
                    return;
                }

                try
                {
                    if ((int)settings.ringID < (int)RingID.EditRingIDTop)
                    {
                        ringName += settings.ringID;
                    }
                    else if ((int)settings.ringID >= (int)RingID.EditRingIDTop)
                    {
                        ringName = global::SaveData.GetInst().GetEditRingData(settings.ringID).name.Replace(War_Form.listSeparator, ' ');
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    ringName = "none";
                }

                promotion = War_Form.form.GetRingPromotion(ringName);
                if (promotion != null)
                {
                    employeeData = new Employee[8];
                    for (int i = 0; i < 8; i++)
                    {
                        Player plObj = PlayerMan.inst.GetPlObj(i);

                        if (!plObj)
                        {
                            continue;
                        }

                        employeeData[i] = promotion.GetEmployeeData(DataBase.GetWrestlerFullName(plObj.WresParam));
                    }
                }

                //Get team names
                teamNames = new String[2];
                List<String> members = new List<String>();

                //Get Blue Team Name
                for (int i = 0; i < 4; i++)
                {
                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (!player)
                    {
                        continue;
                    }

                    if (player.isSecond || player.isIntruder)
                    {
                        continue;
                    }

                    members.Add(DataBase.GetWrestlerFullName(player.WresParam));
                }

                GetTeamName(members, out teamNames[0]);

                //Get Red Team Name
                members.Clear();
                for (int i = 4; i < 8; i++)
                {
                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (!player)
                    {
                        continue;
                    }

                    if (player.isSecond || player.isIntruder)
                    {
                        continue;
                    }

                    members.Add(DataBase.GetWrestlerFullName(player.WresParam));
                }

                GetTeamName(members, out teamNames[1]);

                //Check for champions in a non-title match
                titleData = new TitleMatch_Data[8];
                hasChamp = false;
                try
                {
                    if (GlobalParam.TitleMatch_BeltData == null)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Player player = PlayerMan.inst.GetPlObj(i);
                            if (!player)
                            {
                                continue;
                            }

                            if (player.isSecond || player.isIntruder)
                            {
                                continue;
                            }

                            foreach (var item in SaveData.inst.titleMatch_Data)
                            {
                                String currentChamp = item.GetCurrentTitleHolderName();
                                if (currentChamp.Equals(DataBase.GetWrestlerFullName(player.WresParam)))
                                {
                                    hasChamp = true;
                                    titleData[i] = item;
                                    break;
                                }
                                else
                                {
                                    if (i < 4)
                                    {
                                        if (!teamNames[0].Equals(String.Empty) && teamNames[0].Equals(currentChamp))
                                        {
                                            hasChamp = true;
                                            titleData[i] = item;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (!teamNames[1].Equals(String.Empty) && teamNames[1].Equals(currentChamp))
                                        {
                                            hasChamp = true;
                                            titleData[i] = item;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    L.D("SetEmployees-NonTitleMatchException: " + ex);
                }

                if (teamNames[0] != String.Empty)
                {
                    L.D("Blue Team: " + teamNames[0]);
                }

                if (teamNames[1] != String.Empty)
                {
                    L.D("Red Team: " + teamNames[1]);
                }
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "InvokeUkeBonus", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "FirePro War")]
        public static void ProcessUkemi(Player player)
        {
            if (!fpwEnable || player.isInvokedUkeBonus || promotion == null)
            {
                return;
            }

            Employee employee = employeeData[player.PlIdx];
            if (employee == null)
            {
                return;
            }

            //Determine the type of bonus to be applied
            switch (employee.MoraleRank)
            {
                case 0:
                    player.UkeRecoveryPoint =
                        Convert.ToInt32(player.UkeRecoveryPoint - (player.UkeRecoveryPoint * .4));
                    break;
                case 1:
                    player.UkeRecoveryPoint =
                        Convert.ToInt32(player.UkeRecoveryPoint - (player.UkeRecoveryPoint * .15));
                    break;
                case 3:
                    player.UkeRecoveryPoint =
                        Convert.ToInt32(player.UkeRecoveryPoint + (player.UkeRecoveryPoint * .15));
                    break;
                case 4:
                case 5:
                    player.UkeRecoveryPoint =
                        Convert.ToInt32(player.UkeRecoveryPoint + (player.UkeRecoveryPoint * .4));
                    break;
                default:
                    break;
            }

        }

        [Hook(TargetClass = "Player", TargetMethod = "InvokeUkeBonus", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "FirePro War")]
        public static void IncreaseBonusTime(Player player)
        {
            if (!fpwEnable || promotion == null)
            {
                return;
            }

            Employee employee = employeeData[player.PlIdx];
            if (employee == null)
            {
                return;
            }

            if (employee.MoraleRank == 5)
            {
                player.UkeBonusTime += 1800;
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "FirePro War")]
        public static void UpdateMoralePoints()
        {
            try
            {
                if (!fpwEnable || MatchMain.inst.isInterruptedMatch || promotion == null)
                {
                    return;
                }

                MatchEvaluation evaluation = global::MatchEvaluation.inst;

                //Variables for Match Details
                String titleMatch = "";
                bool winnerLogged = false;
                bool loserLogged = false;
                bool isFinals = IsTourneyFinals();

                //Get Loser
                int rating = evaluation.EvaluateMatch();
                bool isDraw = false;
                //Update Morale Points
                for (int i = 0; i < 8; i++)
                {
                    Employee employee = employeeData[i];
                    if (employee == null)
                    {
                        continue;
                    }

                    int moralePoints = 0;
                    Player player = PlayerMan.inst.GetPlObj(i);

                    //Ensure intruders are exempt
                    if (player.isIntruder)
                    {
                        continue;
                    }

                    bool isWinner = true;

                    //Check Match Rating
                    moralePoints += EvaluateMatchRating(rating);

                    //Check Win Condition
                    if (CheckWinCondition(evaluation))
                    {
                        if (evaluation.PlResult[i].resultPosition == ResultPosition.Loser)
                        {
                            moralePoints -= 1;

                            //Determine Skill rank of the winner
                            Player winner = GetPlayer(ResultPosition.Winner);
                            int rankPoints = CompareRank(winner, player, ResultPosition.Loser);
                            moralePoints += rankPoints;
                            isWinner = false;

                            //Record if this was an upset victory
                            if (rankPoints != 0)
                            {
                                promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" +
                                                     DataBase.GetWrestlerFullName(winner.WresParam) +
                                                     " has scored an upset victory against " +
                                                     DataBase.GetWrestlerFullName(player.WresParam) + ".");
                            }
                        }
                        else if (evaluation.PlResult[i].resultPosition == ResultPosition.Winner)
                        {
                            moralePoints += 1;

                            //Determine Skill rank of loser
                            Player loser = GetPlayer(ResultPosition.Loser);
                            int rankPoints = CompareRank(player, loser, ResultPosition.Winner);
                            moralePoints += rankPoints;
                        }
                        else if (evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner)
                        {
                            moralePoints += 1;
                        }
                        else if (evaluation.PlResult[i].resultPosition == ResultPosition.Loser_Partner)
                        {
                            moralePoints -= 1;
                            isWinner = false;
                        }
                    }
                    else
                    {
                        isDraw = true;
                    }

                    //Check For Title Match
                    if (GlobalParam.TitleMatch_BeltData != null && isFinals)
                    {
                        titleMatch = GlobalParam.TitleMatch_BeltData.titleName.ToUpper() + " - ";

                        String champion = GlobalParam.TitleMatch_BeltData.GetCurrentTitleHolderName();
                        if (evaluation.ResultType == MatchResultEnum.RingOutDraw ||
                            evaluation.ResultType == MatchResultEnum.TimeOutDraw)
                        {
                            moralePoints += 1;
                        }
                        //Removing belt from previous champion
                        else if ((evaluation.PlResult[i].resultPosition == ResultPosition.Loser ||
                                  evaluation.PlResult[i].resultPosition == ResultPosition.Loser_Partner)
                                 && i >= 4)
                        {
                            //Ensure that we aren't logging information in Event History multiple times.
                            if (!loserLogged)
                            {
                                promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" +
                                                     GetTeamName(ResultPosition.Loser) +
                                                     " has lost the " +
                                                     GlobalParam.TitleMatch_BeltData.titleName.ToUpper() + ".");
                                loserLogged = true;
                            }

                            moralePoints -= 3;
                        }
                        //Awarding a new champion
                        else if ((evaluation.PlResult[i].resultPosition == ResultPosition.Winner ||
                                  evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner)
                                 && i < 4)
                        {
                            int championNumber = GlobalParam.TitleMatch_BeltData.titleMatch_Record_Data.Count + 1;
                            string ordinalNumber = global::TitleMatch_Data.GetOrdinalNumberString(championNumber);

                            //Ensure that we aren't logging information in Event History multiple times.
                            if (!winnerLogged)
                            {
                                promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" +
                                                     GetTeamName(ResultPosition.Winner) +
                                                     " has gained the " +
                                                     GlobalParam.TitleMatch_BeltData.titleName.ToUpper() +
                                                     " and becomes the " + ordinalNumber + ".");
                                winnerLogged = true;
                            }

                            moralePoints += (6 - employee.MoraleRank);

                        }
                        //Logging record for a successful defense
                        else if (evaluation.PlResult[i].resultPosition == ResultPosition.Winner ||
                                 evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner && i >= 4)
                        {
                            int defenseNumber = GlobalParam.TitleMatch_BeltData.GetLatestMatchRecord().DefenseCount;
                            string ordinalNumber = global::TitleMatch_Data.GetOrdinalNumberString(defenseNumber);

                            Player loser = GetPlayer(ResultPosition.Loser);

                            //Ensure that we aren't logging information in Event History multiple times.
                            if (!winnerLogged)
                            {
                                promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" +
                                                     GetTeamName(ResultPosition.Winner) + " successfully defend(s) the " +
                                                     GlobalParam.TitleMatch_BeltData.titleName.ToUpper() + " (" +
                                                     ordinalNumber + " defense) against " +
                                                     GetTeamName(ResultPosition.Loser) + ".");
                                winnerLogged = true;
                            }
                        }
                    }

                    //Check for the defeat of champions in a non-title match
                    if (hasChamp)
                    {
                        TitleMatch_Data data = titleData[GetPlayer(ResultPosition.Loser).PlIdx];
                        if (data != null)
                        {
                            //Singles match details
                            if (GetPlayer(ResultPosition.Winner).PlIdx == i)
                            {
                                moralePoints += 3;
                                promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" +
                                                     DataBase.GetWrestlerFullName(GetPlayer(ResultPosition.Winner)
                                                         .WresParam) + " has defeated "
                                                     + DataBase.GetWrestlerFullName(GetPlayer(ResultPosition.Loser)
                                                         .WresParam) + " (" + data.titleName.ToUpper() +
                                                     " champion) in a non-title match.");
                            }
                        }

                    }

                    //Determine whether the player won a league or tournament
                    if ((GlobalParam.m_BattleMode == GlobalParam.BattleMode.Tournament)
                        && isFinals)
                    {
                        if (evaluation.PlResult[i].resultPosition == ResultPosition.Winner ||
                            evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner &&
                            !player.isSecond && !player.isIntruder)
                        {
                            String eventType = "tournament";

                            //Ensure that we aren't logging information in Event History multiple times.
                            if (!winnerLogged)
                            {
                                promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" +
                                                     GetTeamName(ResultPosition.Winner) +
                                                     " has recently won a " + eventType.ToUpper() + ".");
                                winnerLogged = true;
                            }

                            moralePoints += (6 - employee.MoraleRank);
                        }
                    }

                    //Check Employee Health
                    if (player.isKO)
                    {
                        moralePoints -= 1;
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                             " has been knocked out.");
                    }

                    String injury = "";

                    if (player.HP_Arm <= 0)
                    {
                        moralePoints -= 1;

                        if (War_Form.form.fpw_armInjuries.Items.Count == 0)
                        {
                            injury = "extreme arm damage";
                        }
                        else
                        {
                            injury = (String)War_Form.form.fpw_armInjuries.Items[UnityEngine.Random.Range(0, War_Form.form.fpw_armInjuries.Items.Count)];
                        }

                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                             " has suffered " + injury + ".");
                    }

                    if (player.HP_Neck <= 0)
                    {
                        moralePoints -= 1;

                        if (War_Form.form.fpw_neckInjuries.Items.Count == 0)
                        {
                            injury = "extreme neck damage";
                        }
                        else
                        {
                            injury = (String)War_Form.form.fpw_neckInjuries.Items[UnityEngine.Random.Range(0, War_Form.form.fpw_neckInjuries.Items.Count)];
                        }

                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                             " has suffered " + injury + ".");
                    }

                    if (player.HP_Waist <= 0)
                    {
                        moralePoints -= 1;

                        if (War_Form.form.fpw_waistInjuries.Items.Count == 0)
                        {
                            injury = "extreme waist damage";
                        }
                        else
                        {
                            injury = (String)War_Form.form.fpw_waistInjuries.Items[UnityEngine.Random.Range(0, War_Form.form.fpw_waistInjuries.Items.Count)];
                        }

                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                             " has suffered " + injury + ".");
                    }

                    if (player.HP_Leg <= 0)
                    {
                        moralePoints -= 1;

                        if (War_Form.form.fpw_legInjuries.Items.Count == 0)
                        {
                            injury = "extreme leg damage";
                        }
                        else
                        {
                            injury = (String)War_Form.form.fpw_legInjuries.Items[UnityEngine.Random.Range(0, War_Form.form.fpw_legInjuries.Items.Count)];
                        }

                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                             " has suffered " + injury + ".");
                    }


                    //Update Employee Details
                    employee.MoralePoints += moralePoints;
                    employee.MatchCount += 1;

                    if (isDraw)
                    {
                        employee.Draws += 1;
                    }
                    else if (isWinner)
                    {
                        employee.Wins += 1;
                    }
                    else if (!isWinner)
                    {
                        employee.Losses += 1;
                    }

                    if (employee.MatchCount == 1)
                    {
                        employee.AverageRating = rating;
                    }
                    else
                    {
                        employee.AverageRating = (employee.AverageRating + rating) / 2;
                    }

                    War_Form.form.UpdateEmployeeMorale(employee, isWinner);
                }

                //Update Promotion Details
                if (CheckWinCondition(evaluation) || isDraw)
                {
                    promotion.MatchCount += 1;
                    if (promotion.AverageRating == 0)
                    {
                        promotion.AverageRating = rating;
                    }
                    else
                    {
                        promotion.AverageRating = (promotion.AverageRating + rating) / 2;
                    }

                    String matchDetails = "";
                    if (isDraw)
                    {
                        matchDetails = promotion.MatchDetails.Count + 1 + ") " +
                            DateTime.Now.ToString("MM/dd/yyyy HH:mm") + " - " + titleMatch + "Draw between " +
                                       GetTeamName(ResultPosition.Draw) + " (" + rating + "%) in " +
                                       GlobalWork.GetInst().MatchSetting.MatchTime + " minutes.";
                    }
                    else
                    {
                        matchDetails = promotion.MatchDetails.Count + 1 + ") " +
                                       DateTime.Now.ToString("MM/dd/yyyy HH:mm") + " - " + titleMatch +
                                       GetTeamName(ResultPosition.Winner) + " defeat(s) " +
                                       GetTeamName(ResultPosition.Loser) + " (" + rating + "%) in " +
                                       MatchEvaluation.inst.PlResult[GetPlayer(ResultPosition.Loser).PlIdx].finishTime.min + " minutes.";
                    }

                    promotion.AddMatchDetails(matchDetails);
                    War_Form.form.UpdatePromotionData(promotion);
                }
            }
            catch (Exception ex)
            {
                L.D("UpdateMoralePointException: " + ex);
            }
        }

        [Hook(TargetClass = "EntranceScene", TargetMethod = "StartEntranceScene", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersVal, Group = "FirePro War")]
        public static void ShowRecord(EntranceScene es, EntranceSceneKind kind, int[] pl_idx, int pl_cnt)
        {
            try
            {
                if (!War_Form.form.fpw_showRecord.Checked || promotion == null)
                {
                    return;
                }
                String message = "";
                //Get record for each edit
                foreach (var index in pl_idx)
                {
                    Employee employee = employeeData[index];
                    if (employee == null)
                    {
                        continue;
                    }
                    message += CleanUpName(employee.Name) + ": " + employee.Wins + "/" +
                               employee.Losses + "/" + employee.Draws +
                               " || Morale: " + CheckMorale(employee.MoraleRank) + "\n";
                }

                if (!message.Equals(String.Empty))
                {
                    DispNotification.inst.Show(message, 360);
                }
            }

            //Expected crash on Null Reference exceptions.
            catch (NullReferenceException e)
            {

            }
            catch (Exception e)
            {
                L.D("ShowRecordError:" + e);
            }
        }

        #region Helper Methods
        public static String CheckMorale(int moraleRank)
        {
            switch (moraleRank)
            {
                case 0:
                case 1:
                    return "Poor";
                case 2:
                    return "Average";
                case 3:
                case 4:
                    return "Good";
                case 5:
                    return "Outstanding";
                default:
                    return "Average";
            }
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
        public static bool CheckWinCondition(MatchEvaluation evaluation)
        {
            if (evaluation.ResultType == MatchResultEnum.Fall || evaluation.ResultType == MatchResultEnum.GiveUp ||
                evaluation.ResultType == MatchResultEnum.RingOut || evaluation.ResultType == MatchResultEnum.Escape ||
                evaluation.ResultType == MatchResultEnum.OverRope || evaluation.ResultType == MatchResultEnum.KO ||
                evaluation.ResultType == MatchResultEnum.Foul)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Player GetPlayer(ResultPosition resultPosition)
        {
            Player player = null;
            if (resultPosition == ResultPosition.Winner)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (global::MatchEvaluation.inst.PlResult[i].resultPosition == ResultPosition.Winner)
                    {
                        player = PlayerMan.inst.GetPlObj(i);
                        break;
                    }
                }
            }
            else if (resultPosition == ResultPosition.Loser)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (global::MatchEvaluation.inst.PlResult[i].resultPosition == ResultPosition.Loser)
                    {
                        player = PlayerMan.inst.GetPlObj(i);
                        break;
                    }
                }
            }
            return player;
        }
        public static String GetTeamName(ResultPosition position)
        {
            String name = "";

            if (position == ResultPosition.Winner)
            {
                Player winner = GetPlayer(ResultPosition.Winner);
                name = DataBase.GetWrestlerFullName(winner.WresParam);

                if (winner.PlIdx < 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        name += BuildTeamName(ResultPosition.Winner_Partner, i);
                    }
                    if (teamNames[0] != String.Empty)
                    {
                        name = teamNames[0] + " (" + name + ")";
                    }
                }
                else
                {
                    for (int i = 4; i < 8; i++)
                    {
                        name += BuildTeamName(ResultPosition.Winner_Partner, i);
                    }

                    if (teamNames[1] != String.Empty)
                    {
                        name = teamNames[1] + " (" + name + ")";
                    }
                }
            }
            else if (position == ResultPosition.Loser)
            {
                Player loser = GetPlayer(ResultPosition.Loser);
                name = DataBase.GetWrestlerFullName(loser.WresParam);

                if (loser.PlIdx < 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        name += BuildTeamName(ResultPosition.Loser_Partner, i);
                    }

                    if (teamNames[0] != String.Empty)
                    {
                        name = teamNames[0] + " (" + name + ")";
                    }
                }
                else
                {
                    for (int i = 4; i < 8; i++)
                    {
                        name += BuildTeamName(ResultPosition.Loser_Partner, i);
                    }

                    if (teamNames[1] != String.Empty)
                    {
                        name = teamNames[1] + " (" + name + ")";
                    }
                }
            }
            else if (position == ResultPosition.Draw)
            {
                for (int i = 0; i < 4; i++)
                {
                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (player)
                    {
                        if (!player.isIntruder && !player.isSecond)
                        {
                            if (i == 0)
                            {
                                name = DataBase.GetWrestlerFullName(player.WresParam);
                            }
                            else
                            {
                              name += ", " + DataBase.GetWrestlerFullName(player.WresParam);
                            }
                        }
                    }
                }

                if (teamNames[0] != String.Empty)
                {
                    name = teamNames[0] + "(" + name + ")";
                }

                String blueTeam = name;
                name += "";

                for (int i = 4; i < 8; i++)
                {
                    Player player = PlayerMan.inst.GetPlObj(i);
                    if (player)
                    {
                        if (!player.isIntruder && !player.isSecond)
                        {
                            if (i == 4)
                            {
                                name = DataBase.GetWrestlerFullName(player.WresParam);
                            }
                            else
                            {
                                name += ", " + DataBase.GetWrestlerFullName(player.WresParam);
                            }
                        }
                    }
                }

                if (teamNames[1] != String.Empty)
                {
                    name = teamNames[1] + " (" + name + ")";
                }

                string redTeam = name;

                name = blueTeam + " & " + redTeam;
            }

            return name;
        }
        public static void GetTeamName(List<String> wrestlers, out String teamName)
        {
            try
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

                if (teams.Count > 0)
                {
                    teamName = teams[UnityEngine.Random.Range(0, teams.Count)];
                }
                else
                {
                    teamName = String.Empty;
                }
            }
            catch (Exception e)
            {
                L.D("GetTeamNamesException: " + e);
                teamName = String.Empty;
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
        public static int CompareRank(Player winner, Player loser, ResultPosition position)
        {
            RankEnum winnerRank = winner.WresParam.wrestlerRank;
            RankEnum loserRank = loser.WresParam.wrestlerRank;
            int difference = loserRank - winnerRank;
            if (position == ResultPosition.Winner)
            {
                if (difference >= 4)
                {
                    return 2;
                }
                else
                {
                    if (difference >= 2)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                if (difference >= 4)
                {
                    return -2;
                }
                else
                {
                    if (difference >= 2)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

        }
        public static int EvaluateMatchRating(int rating)
        {
            switch (rating)
            {
                case int i when (i >= 90):
                    return 1;
                case int i when (i >= 60 && i < 90):
                    return 0;
                case int i when (i >= 40 && i < 60):
                    return -1;
                case int i when (i < 40):
                    return -2;
                default:
                    return 0;
            }
        }
        public static bool IsTourneyFinals()
        {
            if (GlobalParam.m_BattleMode == GlobalParam.BattleMode.Tournament)
            {
                try
                {
                    MethodInfo finalsInfo = typeof(ModPack.ModPack).GetMethod("CheckFinals", BindingFlags.Static | BindingFlags.NonPublic);
                    bool isFinals = (bool)finalsInfo.Invoke(null, null);
                    L.D("In " + GlobalParam.m_BattleMode + " Finals: " + isFinals);
                    return isFinals;
                }
                catch (Exception e)
                {
                    L.D("IsTourneyLeagueFinalsError: " + e);
                }
            }

            //Ensure that processing continues in the event of an error, or different match type.
            L.D("Current mode is " + GlobalParam.m_BattleMode);
            return true;

        }
        public static String BuildTeamName(ResultPosition position, int index)
        {
            String name = "";

            Player plObj = PlayerMan.inst.GetPlObj(index);
            if (!plObj)
            {
                return "";
            }

            if (plObj.isSecond || plObj.isIntruder)
            {
                return "";
            }

            if (MatchEvaluation.inst.PlResult[index].resultPosition == ResultPosition.Winner_Partner || MatchEvaluation.inst.PlResult[index].resultPosition == ResultPosition.Loser_Partner)
            {
                name += " & " + DataBase.GetWrestlerFullName(plObj.WresParam);
            }

            return name;
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
        #endregion
    }
}

