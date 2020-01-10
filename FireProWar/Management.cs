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
using UnityEngine;
using UnityEngine.UI;

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
                try
                {
                    if ((int)settings.ringID < (int)RingID.EditRingIDTop)
                    {
                        ringName += settings.ringID;
                    }
                    else if ((int)settings.ringID >= (int)RingID.EditRingIDTop)
                    {
                        ringName = global::SaveData.GetInst().GetEditRingData(settings.ringID).name;
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
                MatchEvaluation evaluation = global::MatchEvaluation.inst;

                //Variables for Match Details
                String titleMatch = "";
                String winners = "";
                String losers = "";

                #region Setting End Match Time
                String time = "";
                if (MatchMain.inst.matchTime.min < 10)
                {
                    time = "0" + MatchMain.inst.matchTime.min;
                }
                else
                {
                    time = MatchMain.inst.matchTime.min.ToString();
                }

                time += ":";

                if (MatchMain.inst.matchTime.sec < 10)
                {
                    time += "0" + MatchMain.inst.matchTime.sec;
                }
                else
                {
                    time += MatchMain.inst.matchTime.sec;
                }
                #endregion

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
                    bool isWinner = true;

                    //Check Match Rating
                    moralePoints += EvaluateMatchRating(rating);

                    //Check Win Condition
                    if (CheckWinCondition(evaluation))
                    {
                        if (evaluation.PlResult[i].resultPosition == ResultPosition.Loser)
                        {
                            if (String.IsNullOrEmpty(losers))
                            {
                                losers = DataBase.GetWrestlerFullName(player.WresParam);
                            }
                            else
                            {
                                losers += " & " + DataBase.GetWrestlerFullName(player.WresParam);
                            }
                            moralePoints -= 1;

                            //Determine Skill rank of the winner
                            Player winner = GetPlayer(ResultPosition.Winner);
                            int rankPoints = CompareRank(winner, player, ResultPosition.Loser);
                            moralePoints += rankPoints;
                            isWinner = false;

                            //Record if this was an upset victory
                            if (rankPoints != 0)
                            {
                                promotion.AddHistory(DataBase.GetWrestlerFullName(winner.WresParam) +
                                                     " has scored an upset victory against " +
                                                     DataBase.GetWrestlerFullName(player.WresParam) + ".");
                            }
                        }
                        else if (evaluation.PlResult[i].resultPosition == ResultPosition.Winner)
                        {
                            if (String.IsNullOrEmpty(winners))
                            {
                                winners = DataBase.GetWrestlerFullName(player.WresParam);
                            }
                            else
                            {
                                winners += " & " + DataBase.GetWrestlerFullName(player.WresParam);
                            }
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
                        titleMatch = GlobalParam.TitleMatch_BeltData.titleName + ": ";

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
                            promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                                  " has lost the " + GlobalParam.TitleMatch_BeltData.titleName + ".");
                            moralePoints -= 3;
                        }
                        //Awarding a new champion
                        else if ((evaluation.PlResult[i].resultPosition == ResultPosition.Winner ||
                                  evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner)
                                  && i < 4)
                        {
                            promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has gained the " + GlobalParam.TitleMatch_BeltData.titleName + ".");
                            moralePoints += (6 - employee.MoraleRank);
                        }
                        //Logging record for a successful defense
                        else if (evaluation.PlResult[i].resultPosition == ResultPosition.Winner ||
                                 evaluation.PlResult[i].resultPosition == ResultPosition.Winner_Partner && i >= 4)
                        {
                            promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " successfully defends the " + GlobalParam.TitleMatch_BeltData.titleName + ".");
                        }
                    }

                    //Check Employee Health
                    if (player.isKO)
                    {
                        moralePoints -= 1;
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has been knocked out.");
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

                    //Ensure that we store match results, even if the winner/loser isn't a member of the promotion
                    Player resultPlayer;
                    if (String.IsNullOrEmpty(winners) && !isDraw)
                    {
                        resultPlayer = PlayerMan.inst.GetPlObj(evaluation.GetWinnerPlayer());
                        if (resultPlayer)
                        {
                            winners = DataBase.GetWrestlerFullName(resultPlayer.WresParam);
                        }
                    }

                    if (String.IsNullOrEmpty(losers) && !isDraw)
                    {
                        resultPlayer = PlayerMan.inst.GetPlObj(evaluation.GetLoserPlayer());
                        if (resultPlayer)
                        {
                            losers = DataBase.GetWrestlerFullName(resultPlayer.WresParam);
                        }
                    }

                    String matchDetails = promotion.MatchDetails.Count + 1 + ") " + DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + titleMatch + winners + " defeat(s) " + losers + " (" + rating + "%).";

                    if (!String.IsNullOrEmpty(winners) && !String.IsNullOrEmpty(losers))
                    {
                        promotion.AddMatchDetails(matchDetails);
                    }
                    War_Form.form.UpdatePromotionData(promotion);
                }
            }
            catch
            { }
        }
        [Hook(TargetClass = "EntranceScene", TargetMethod = "StartEntranceScene", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersVal, Group = "FirePro War")]
        public static void ShowRecord(EntranceScene es, EntranceSceneKind kind, int[] pl_idx, int pl_cnt)
        {
            try
            {
                String message = "";
                //Get record for each edit
                foreach (var index in pl_idx)
                {
                    Employee employee = employeeData[index];
                    if (employee == null)
                    {
                        continue;
                    }
                    message += CleanUpName(employee.Name) + ": " + employee.Wins + "-" +
                               employee.Draws + "-" + employee.Losses +
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

        //[Hook(TargetClass = "MatchMain", TargetMethod = "Awake", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "FirePro War")]
        public static void SetTVLogo()
        {
            if (fpwEnable)
            {
                //Set company logo
                SetLogo(promotion);
            }
        }

        #region Helper Methods
        public static void SetLogo(Promotion promotion)
        {
            try
            {
                #region Load the Unity Asset

                if (tvLogo == null)
                {
                    tvLogo = AssetBundle.LoadFromFile(logoAssetLocation);
                }

                if (GameObject.Find("TVLogo") == null)
                {
                    L.D("Could not find TVLogo");
                    GameObject gameObject = (GameObject)tvLogo.LoadAsset("LogoCanvas");
                    GameObject rootObj = (GameObject)GetField(Ring.inst, "rootObj", false);
                    var logoObj = (GameObject)UnityEngine.Object.Instantiate(gameObject, rootObj.transform, false);
                }
                Image component;
                if (GameObject.Find("TVLogo") == null)
                {
                    L.D("Can't find TVLogo GameObject", new object[0]);
                    return;
                }
                GameObject logoObject = GameObject.Find("TVLogo");
                component = logoObject.GetComponent<Image>();
                if (component == null)
                {
                    L.D("Still unable to find uiComponent", new object[0]);
                    return;
                }
                #endregion

                #region Get the logo
                string logo = "";
                L.D("Checking logo location at " + storedLogoPath);
                string[] logoCollection = Directory.GetFiles(storedLogoPath);
                foreach (var file in logoCollection)
                {
                    string modifiedFileName = Path.GetFileName(file);
                    modifiedFileName = modifiedFileName.Substring(0, (modifiedFileName.IndexOf('.')));
                    if (promotion != null)
                    {
                        if (modifiedFileName.Equals(promotion.Name))
                        {
                            logo = promotion.Name + ".png";
                            break;
                        }
                        else if (modifiedFileName.Equals(promotion.Ring))
                        {
                            logo = promotion.Ring + ".png";
                            break;
                        }
                    }
                    else
                    {
                        logo = defaultLogo;
                    }
                }

                if (logo.Equals(String.Empty))
                {
                    logo = "Default.png";
                }
                String logoPath = Path.Combine(storedLogoPath, logo);
                #endregion

                byte[] array = File.ReadAllBytes(logoPath);
                Texture2D texture2D = new Texture2D(6, 6);
                texture2D.LoadImage(array);
                component.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero, 125f);

                //Get transform lines from file
                if (File.Exists(logoPositionFile))
                {
                    var lines = File.ReadAllLines(logoPositionFile);
                    Single.TryParse(lines[0], out float x);
                    Single.TryParse(lines[1], out float y);
                    Single.TryParse(lines[2], out float z);
                    component.rectTransform.Translate(x, y, z);
                }
                else
                {
                    component.rectTransform.Translate(0, 0, 0);
                }
            }
            catch (Exception e)
            {
                L.D("SetLogoException:" + e);
            }

        }
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

