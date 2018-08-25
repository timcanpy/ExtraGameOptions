using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using QoL_Mods.Data_Classes;

namespace QoL_Mods
{
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Player", Field = "ProcessCritical", Group = "ExtraFeatures")]
    [FieldAccess(Class = "MatchMain", Field = "EndMatch", Group = "ExtraFeatures")]
    class EditManagement
    {/*Existing Bugs
        Remove One Option crashes game
        */
        #region Variables
        public static String[] injuryTypes = new String[] { "Healthy", "Bruised", "Sprained", "Hurt", "Injured", "Broken" };
        public static bool isInjury = false;
        public static int injuryCeiling = 5;
        public static int injuryFloor = 3;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void SetMatchRules()
        {
            if (QoL_Form.form.em_injuries.Checked)
            {
                isInjury = true;

                //Determine if existing players are on the injury list
                for (int i = 0; i < 8; i++)
                {
                    Player plObj = PlayerMan.inst.GetPlObj(i);

                    if (!plObj)
                    {
                        continue;
                    }
                    String wrestlerName = DataBase.GetWrestlerFullName(plObj.WresParam);
                    if (IsWrestlerInjured(wrestlerName))
                    {
                        L.D(wrestlerName + " is on the injury list");

                        //Update Wrestler Part Health
                        SetPartHealth(i, QoL_Form.form.GetWrestlerHealthInfo(wrestlerName));
                    }
                }
            }
        }

        [Hook(TargetClass = "Player", TargetMethod = "ProcessCritical", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void SetInjury(Player plObj)
        {
            if (!isInjury)
            {
                return;
            }
            //Get targetting player
            Player targetPlObj = PlayerMan.inst.GetPlObj(plObj.TargetPlIdx);
            global::SkillData currentSkill = targetPlObj.animator.CurrentSkill;

            //Determine if wrestler currently exists in the injury list
            String wrestlerName = DataBase.GetWrestlerFullName(plObj.WresParam);
            WrestlerHealth healthInfo = QoL_Form.form.GetWrestlerHealthInfo(wrestlerName);

            L.D("Processing critical for " + wrestlerName);
            //Determine which areas have been injured
            if (currentSkill.atkPow_Neck != 0)
            {
                healthInfo = CalculateNewInjury("Neck", healthInfo, plObj);
            }
            if (currentSkill.atkPow_Waist != 0)
            {
                healthInfo = CalculateNewInjury("Body", healthInfo, plObj);
            }
            if (currentSkill.atkPow_Arm != 0)
            {
                healthInfo = CalculateNewInjury("Arm", healthInfo, plObj);
            }
            if (currentSkill.atkPow_Leg != 0)
            {
                healthInfo = CalculateNewInjury("Leg", healthInfo, plObj);
            }

            //Add details to the injury form
            QoL_Form.form.UpdateWrestlerHealthInfo(healthInfo);
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void CheckInjuredPlayersInMatch()
        {
            if (!isInjury)
            {
                return;
            }

            for (int i = 0; i < 8; i++)
            {
                try
                {
                    Player plObj = PlayerMan.inst.GetPlObj(i);

                    if (!plObj)
                    {
                        continue;
                    }

                    //Avoid calculating the recovery time twice for criticals
                    if (plObj.isKO)
                    {
                        continue;
                    }

                    String wrestlerName = DataBase.GetWrestlerFullName(plObj.WresParam);
                    if (IsWrestlerInjured(wrestlerName))
                    {
                        //Check injured wrestler parts
                        bool partReinjured = false;
                        WrestlerHealth healthInfo = QoL_Form.form.GetWrestlerHealthInfo(wrestlerName);
                        if (!healthInfo.NeckHealth.Equals(injuryTypes[0]))
                        {
                            if (plObj.HP_Neck <= 0)
                            {
                                healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.NeckHealth, plObj.WresParam.defNeck);
                                partReinjured = true;
                            }
                        }
                        if (!healthInfo.BodyHealth.Equals(injuryTypes[0]))
                        {
                            if (plObj.HP_Waist <= 0)
                            {
                                healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.BodyHealth, plObj.WresParam.defWaist);
                                partReinjured = true;
                            }
                        }
                        if (!healthInfo.ArmHealth.Equals(injuryTypes[0]))
                        {
                            if (plObj.HP_Arm <= 0)
                            {
                                healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.ArmHealth, plObj.WresParam.defArm);
                                partReinjured = true;
                            }
                        }
                        if (!healthInfo.LegHealth.Equals(injuryTypes[0]))
                        {
                            if (plObj.HP_Leg <= 0)
                            {
                                healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.LegHealth, plObj.WresParam.defLeg);
                                partReinjured = true;
                            }
                        }

                        if (partReinjured)
                        {
                            L.D("Updating Recovery Time Due to Reinjury");
                            QoL_Form.form.UpdateWrestlerHealthInfo(healthInfo);
                        }
                    }
                }
                catch(Exception ex)
                {
                    L.D("Error Updating Injured Players: " + ex.Message);
                }
            }
        }

        #region Helper Classes
        public static void SetPartHealth(int slot, WrestlerHealth injuryData)
        {
            if (injuryData != null)
            {
                MatchSetting settings = GlobalWork.GetInst().MatchSetting;
                settings.matchWrestlerInfo[slot].HP_Neck = settings.matchWrestlerInfo[slot].HP_Neck * (GetHealthModifier(injuryData.NeckHealth));
                settings.matchWrestlerInfo[slot].HP_Waist = settings.matchWrestlerInfo[slot].HP_Waist * (GetHealthModifier(injuryData.BodyHealth));
                settings.matchWrestlerInfo[slot].HP_Arm = settings.matchWrestlerInfo[slot].HP_Arm * (GetHealthModifier(injuryData.ArmHealth));
                settings.matchWrestlerInfo[slot].HP_Leg = settings.matchWrestlerInfo[slot].HP_Leg * (GetHealthModifier(injuryData.LegHealth));
            }

        }
        public static bool IsWrestlerInjured(String wrestlerName)
        {
            bool isInjured = false;
            foreach (var wrestler in QoL_Form.form.ij_wrestlerList.Items)
            {
                WrestlerHealth healthInfo = (WrestlerHealth)wrestler;
                if (healthInfo.Name.Equals(wrestlerName))
                {
                    isInjured = true;
                    break;
                }
            }

            return isInjured;
        }
        public static float GetHealthModifier(String state)
        {
            L.D("Body part is currently " + state);
            float modifier = 1;

            switch (state)
            {
                case "Healthy":
                    modifier = 1;
                    break;

                case "Bruised":
                    modifier = .85f;
                    break;

                case "Sprained":
                    modifier = .75f;
                    break;

                case "Hurt":
                    modifier = .5f;
                    break;

                case "Injured":
                    modifier = .25f;
                    break;

                case "Broken":
                    modifier = 0;
                    break;

                default:
                    modifier = 1;
                    break;
            }

            return modifier;
        }
        public static String GetNewInjuryState(int partEndurance, String partHealth)
        {
            String currentInjury = partHealth;
            String newInjury = "";

            //Calculate injury for previously healthy part
            if (currentInjury != injuryTypes[0])
            {
                int newFloor = Array.FindIndex(injuryTypes, row => row.Contains(currentInjury));
                L.D("Index of " + currentInjury + ": " + newFloor);

                //Determine what the new injury floor will be.
                newFloor = newFloor + injuryFloor - partEndurance;
                if (newFloor > injuryCeiling)
                {
                    newFloor = injuryCeiling;
                    //Implement save data modification for part endurance here.
                }
                int rngValue = UnityEngine.Random.Range(newFloor, injuryCeiling);
                newInjury = injuryTypes[rngValue];
            }
            else
            {
                int rngValue = UnityEngine.Random.Range(injuryFloor - partEndurance, injuryCeiling);
                newInjury = injuryTypes[rngValue];
            }
            return newInjury;

        }
        public static WrestlerHealth CalculateNewInjury(String part, WrestlerHealth healthInfo, Player plObj)
        {
            int endurance = 0;
            String wrestlerName = DataBase.GetWrestlerFullName(plObj.WresParam);
            healthInfo = InitializeInjuryInfo(healthInfo, wrestlerName, plObj.WresParam.hpRecovery);
            switch (part)
            {
                case "Neck":
                    endurance = plObj.WresParam.defNeck;
                    healthInfo.NeckHealth = GetNewInjuryState(endurance, healthInfo.NeckHealth);
                    healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.NeckHealth, endurance);
                    AnnounceInjury(wrestlerName, "neck", healthInfo.NeckHealth.ToLower());
                    break;

                case "Body":
                    endurance = plObj.WresParam.defWaist;
                    healthInfo.BodyHealth = GetNewInjuryState(endurance, healthInfo.BodyHealth);
                    healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.BodyHealth, endurance);
                    AnnounceInjury(wrestlerName, "back", healthInfo.BodyHealth.ToLower());
                    break;

                case "Arm":
                    endurance = plObj.WresParam.defArm;
                    healthInfo.ArmHealth = GetNewInjuryState(endurance, healthInfo.ArmHealth);
                    healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.ArmHealth, endurance);
                    AnnounceInjury(wrestlerName, "arm", healthInfo.ArmHealth.ToLower());
                    break;

                case "Leg":
                    endurance = plObj.WresParam.defLeg;
                    healthInfo.LegHealth = GetNewInjuryState(endurance, healthInfo.LegHealth);
                    healthInfo = QoL_Form.form.SetRecoveryTime(healthInfo, healthInfo.LegHealth, endurance);
                    AnnounceInjury(wrestlerName, "leg", healthInfo.LegHealth.ToLower());
                    break;
            }

            return healthInfo;
        }
        public static WrestlerHealth InitializeInjuryInfo(WrestlerHealth healthInfo, String name, float recovery)
        {
            if (healthInfo == null)
            {
                healthInfo = new WrestlerHealth(name, injuryTypes[0], injuryTypes[0], injuryTypes[0], injuryTypes[0], recovery, 0);
            }
            return healthInfo;
        }
        public static void AnnounceInjury(String wrestler, String part, String health)
        {
            DispNotification.inst.Show(wrestler + "'s " + part + " is " + health + "!");
        }
        #endregion
    }
}
