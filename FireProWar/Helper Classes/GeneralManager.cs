using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DG;
using System.IO;
using Data_Classes;
using FireProWar;

namespace FireProWar
{
    public class FPTDataManager : MonoBehaviour
    {
        public static FPTDataManager inst = null;
        private static String[] saveFolderNames = new String[] { "./EGOData/" };
        private static String[] saveFileNames = new String[] { "FPWData.dat", "FPWConfig.dat" };
        public static String currentVersion = "V2";
        public static char listSeparator = ':';
        private static String promotionDivider = "|--Promotion--|";

        private void Awake()
        {
            inst = this;
            //DO INITIALIZATION AND LOADING STUFF HERE
        }

        private void OnApplicationQuit()
        {
            inst = null;
        }

        public void Save()
        {
            if (War_Form.form == null)
            {
                L.D("War Form is null");
                return;
            }

            String folder = saveFolderNames[0];
            CheckForDirectory(folder);

            #region Saving Promotion Data
            String filePath = saveFolderNames[0] + saveFileNames[0];
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                //Label save file for future version control
                sw.WriteLine(currentVersion);

                //Save Promotions
                foreach (Promotion promotion in War_Form.form.fpw_promoList.Items)
                {
                    //Promotion Details
                    sw.WriteLine(promotionDivider);
                    sw.WriteLine(promotion.Name);
                    sw.WriteLine(promotion.GetRingList());
                    sw.WriteLine(promotion.Type);
                    sw.WriteLine(promotion.Region);
                    sw.WriteLine(promotion.MatchCount);
                    sw.WriteLine(promotion.AverageRating);
                    sw.WriteLine(promotion.History);

                    //Match Details
                    String matchDetails = "";
                    foreach (String result in promotion.MatchDetails)
                    {
                        matchDetails += result + "|";
                    }

                    sw.WriteLine(matchDetails);

                    //Roster
                    String employeeData = "";
                    foreach (Employee employee in promotion.EmployeeList)
                    {
                        employeeData += employee.CompileEmployeeData();
                    }

                    sw.WriteLine(employeeData);
                }
            }
            #endregion

            try
            {
                #region Saving Config Data

                filePath = saveFolderNames[0] + saveFileNames[1];
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("fpw_Enable: " + War_Form.form.fpw_Enable.Checked);
                    sw.WriteLine("fpw_showRecord: " + War_Form.form.fpw_showRecord.Checked);

                    //Save Neck Injuries
                    String injuries = "";
                    foreach (var injury in War_Form.form.fpw_neckInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_neckInjuries-" + injuries);

                    //Save Arm Injuries
                    injuries = "";
                    foreach (var injury in War_Form.form.fpw_armInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_armInjuries-" + injuries);

                    //Save Leg Injuries
                    injuries = "";
                    foreach (var injury in War_Form.form.fpw_legInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_legInjuries-" + injuries);

                    //Save Arm Injuries
                    injuries = "";
                    foreach (var injury in War_Form.form.fpw_waistInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_waistInjuries-" + injuries);
                }
                #endregion

            }
            catch (Exception e)
            {
                L.D("Save Config Data Error:" + e);
            }

            L.D("Fire Pro War Data Saved");
        }

        [Hook(TargetClass = "ResidentObj", TargetMethod = "Awake", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "FirePro War")]
        public static void AddToResidentObj()
        {
            //THIS ADDS THE MANAGER TO THE RESIDENT OBJECT, WHICH IS THE GAME OBJECT THAT HOLDS ALL THE OTHER GAME MANAGER OBJECTS
            ResidentObj resObj = GameObject.FindObjectOfType<ResidentObj>();
            if (resObj == null)
            {
                return;
            }

            resObj.gameObject.AddComponent<FPTDataManager>();
        }

        [Hook(TargetClass = "SaveData", TargetMethod = "Save", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "FirePro War")]
        public static void SaveWithGame()
        {
            //THIS IS OPTIONAL, BUT ENSURES THAT YOUR DATA WILL SAVE WHEN THE GAME'S DATA IS CALLED TO SAVE
            if (inst == null)
            {
                return;
            }

            inst.Save();
        }

        #region Helper Methods
        private void CheckForDirectory(String filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
        #endregion
    }
}
