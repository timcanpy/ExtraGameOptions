using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using Data_Classes;
using DG;
using JetBrains.Annotations;

namespace FireProWar
{
    [GroupDescription(Description = "This adds a system for competitve booking among various promotions, as well as the management of roster morale.", Group = "FirePro War", Name = "Fire Promotion Wars")]
    [FieldAccess(Class = "MatchMain", Field = "InitMatch", Group = "FirePro War")]
    [FieldAccess(Class = "Player", Field = "InvokeUkeBonus", Group = "FirePro War")]
    [FieldAccess(Class = "MatchMain", Field = "EndMatch", Group = "FirePro War")]
    [FieldAccess(Class = "Menu_Result", Field = "Start", Group = "FirePro War")]
    public class Management
    {

        [ControlPanel(Group = "FirePro War")]
        public static Form MSForm()
        {
            if (War_Form.form == null)
            {
                return new War_Form();
            }
            {
                return null;
            }
        }

        #region Variables
        public static bool fpwEnable = false;
        public static Employee[] employeeData = new Employee[8];
        public static String ringName = "";
        public static Promotion promotion = null;
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
                L.D("Ring ID - " +settings.ringID);
                if ((int)settings.ringID >= (int)RingID.EditRingIDTop)
                {
                    ringName = SaveData.inst.editRingData[(int)settings.ringID - (int)RingID.EditRingIDTop].name;
                }
                else
                {
                    ringName = settings.ringID.ToString();
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
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "InvokeUkeBonus", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "FirePro War")]
        public static void ProcessUkemi(Player player)
        {
            if (!fpwEnable || player.isInvokedUkeBonus)
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
            if (!fpwEnable)
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
                if (!fpwEnable || MatchMain.inst.isInterruptedMatch)
                {
                    return;
                }

                //Get Loser
                MatchEvaluation evaluation = global::MatchEvaluation.inst;
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
                    if (GlobalParam.TitleMatch_BeltData != null)
                    {
                        String champion = GlobalParam.TitleMatch_BeltData.GetCurrentTitleHolderName();
                        if (evaluation.ResultType == MatchResultEnum.RingOutDraw ||
                            evaluation.ResultType == MatchResultEnum.TimeOutDraw)
                        {
                            moralePoints += 1;
                        }
                        //Removing belt from previous champion
                        else if ((evaluation.PlResult[i].resultPosition == ResultPosition.Loser ||
                                  evaluation.PlResult[i].resultPosition == ResultPosition.Loser_Partner) &&
                                 champion.Contains(employee.Name))
                        {
                            L.D(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                " has lost the " + GlobalParam.TitleMatch_BeltData.titleName + ".");
                           promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                                 " has lost the "+ GlobalParam.TitleMatch_BeltData.titleName+".");
                            moralePoints -= 3;
                        }
                        //Awarding a new champion
                        else if ((evaluation.PlResult[i].resultPosition == ResultPosition.Winner ||
                                  evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner) &&
                                 !champion.Contains(employee.Name))
                        {
                            L.D(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has gained the " + GlobalParam.TitleMatch_BeltData.titleName + ".");
                            promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has gained the "+ GlobalParam.TitleMatch_BeltData.titleName+ ".");
                            moralePoints += (6 - employee.MoraleRank);
                        }
                        //Logging record for a successful defense
                        else if (evaluation.PlResult[i].resultPosition == ResultPosition.Winner ||
                                 evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner &&
                                 champion.Contains(employee.Name))
                        {
                            L.D(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                " successfully defends the " + GlobalParam.TitleMatch_BeltData.titleName + ".");
                            promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " successfully defends the " + GlobalParam.TitleMatch_BeltData.titleName + ".");
                        }
                    }

                    //Check Employee Health
                    if (player.isKO)
                    {
                        moralePoints -= 1;
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") +"-"+employee.Name + " has been knocked out.");
                    }

                    if (player.HP_Arm <= 0)
                    {
                        moralePoints -= 1;
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has suffered extreme arm damage.");
                    }

                    if (player.HP_Neck <= 0)
                    {
                        moralePoints -= 1;
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has suffered extreme neck damage.");
                    }

                    if (player.HP_Waist <= 0)
                    {
                        moralePoints -= 1;
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has suffered extreme body damage.");
                    }

                    if (player.HP_Leg <= 0)
                    {
                        moralePoints -= 1;
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has suffered extreme leg damage.");
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

                    War_Form.form.UpdatePromotionData(promotion);
                }
            }
            catch
            { }
        }

        #region Helper Methods

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
        #endregion

    }
}
