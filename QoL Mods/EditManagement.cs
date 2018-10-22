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
    {
        #region Variables
        public static String[] injuryTypes = new String[] { "Healthy", "Bruised", "Sprained", "Hurt", "Injured", "Broken" };
        public static bool isInjury = false;
        public static int injuryCeiling = 5;
        public static int injuryFloor = 3;
        public static bool canSelfCrit = false;
        public static bool[] selfCritCheck;
        public static SkillData[] selfCritSkills;
        public static bool[] injuredBeforeMatch;
        #endregion

        //[Hook(TargetClass = "Menu_Title", TargetMethod = "Awake", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None)]
        //public static void FuckYouPirates()
        //{
        //    string[] commandLineArgs = Environment.GetCommandLineArgs();
        //    for (int i = 0; i < commandLineArgs.Length; i++)
        //    {
        //        if (commandLineArgs[i].Equals("-ImALandLubber"))
        //        {
        //            return;
        //        }
        //    }

        //    if (File.Exists("./steam_emu.ini") || File.Exists("./steam_api.cdx"))
        //    {
        //        for (int i = 0; i < 500; i++)
        //        {
        //            MessageBox.Show("PIRACY IS FOR ASSHOLES, ASSHOLE.", "YOU'RE AN ASSHOLE", MessageBoxButtons.OK);
        //        }

        //        UnityEngine.Application.Quit();
        //    }
        //}

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void SetMatchRules()
        {
            if (QoL_Form.form.em_injuries.Checked)
            {
                isInjury = true;
                injuredBeforeMatch = new bool[8];
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
                        //Update Wrestler Part Health
                        SetPartHealth(i, QoL_Form.form.GetWrestlerHealthInfo(wrestlerName));
                        injuredBeforeMatch[i] = true;
                    }
                }
            }
            else
            {
                isInjury = false;
            }

            if (QoL_Form.form.ij_highRisk.Checked)
            {
                canSelfCrit = true;
                selfCritCheck = new bool[8];
                selfCritSkills = new SkillData[8];
            }
            else
            {
                canSelfCrit = false;
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

            //Determine if this is a self imposed injury
            bool isSelfCrit = false;
            if(selfCritSkills[plObj.PlIdx] != null)
            {
                isSelfCrit = true;
                currentSkill = selfCritSkills[plObj.PlIdx];
                selfCritSkills[plObj.PlIdx] = null;
            }

            //Determine which areas have been injured & whether is is a self-imposed critical from a high risk move
            if (!isSelfCrit)
            {
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

                //Clause for miscellaneous injuries, caused by weapons/explosions/etc
                if (currentSkill.atkPow_Arm == 0 && currentSkill.atkPow_Neck == 0 && currentSkill.atkPow_Waist == 0 && currentSkill.atkPow_Leg == 0)
                {
                    L.D("Randomizing parts for crit injury");
                    //Randomize the injury
                    int rngValue = UnityEngine.Random.Range(1, 4);
                    String injuredPart = "";
                    switch (rngValue)
                    {
                        case 1:
                            injuredPart = "Neck";
                            break;
                        case 2:
                            injuredPart = "Body";
                            break;
                        case 3:
                            injuredPart = "Arm";
                            break;
                        case 4:
                            injuredPart = "Leg";
                            break;
                    }
                    healthInfo = CalculateNewInjury(injuredPart, healthInfo, plObj);
                }
            }
            else
            {
                String injuredPart = "";

                //Check the suicide damage of the move
                if (currentSkill.suicideDamage_Neck != 0)
                {
                    injuredPart = "Neck";
                }
                else if (currentSkill.suicideDamage_Waist != 0)
                {
                    injuredPart = "Body";
                }
                else if (currentSkill.suicideDamage_Leg != 0)
                {
                    injuredPart = "Leg";
                }
                else if (currentSkill.suicideDamage_Arm != 0)
                {
                    injuredPart = "Arm";
                }
                else
                {
                    L.D("Randomizing parts for dive injury");
                    //Randomize the injury
                    int rngValue = UnityEngine.Random.Range(1, 4);
                    switch (rngValue)
                    {
                        case 1:
                            injuredPart = "Neck";
                            break;
                        case 2:
                            injuredPart = "Body";
                            break;
                        case 3:
                            injuredPart = "Arm";
                            break;
                        case 4:
                            injuredPart = "Leg";
                            break;
                    }
                }
                healthInfo = CalculateNewInjury(injuredPart, healthInfo, plObj);
            }

            //Add details to the injury form
            QoL_Form.form.UpdateWrestlerHealthInfo(healthInfo);
        }

        [Hook(TargetClass = "Player", TargetMethod = "ProcessCritical", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void CheckForCriticalInjury(Player plObj)
        {
            if (!isInjury)
            {
                return;
            }

            //Determine if wrestler currently exists in the injury list
            String wrestlerName = DataBase.GetWrestlerFullName(plObj.WresParam);
            WrestlerHealth healthInfo = QoL_Form.form.GetWrestlerHealthInfo(wrestlerName);

            if (healthInfo == null)
            {
                return;
            }

            //Determine if the injury is match ending
            String[] criticalInjuries = new String[] { "Hurt", "Injured", "Broken" };
            bool isCriticalInjury = false;

            if (criticalInjuries.Contains(healthInfo.ArmHealth) || criticalInjuries.Contains(healthInfo.LegHealth) || criticalInjuries.Contains(healthInfo.NeckHealth) || criticalInjuries.Contains(healthInfo.BodyHealth))
            {
                isCriticalInjury = true;
                //Referee mRef = RefereeMan.inst.GetRefereeObj();
                //mRef.ReqRefereeAnm(BasicSkillEnum.);
            }

            if (!isCriticalInjury)
            {
                //Minor injury, wrestler can keep fighting
                plObj.isKO = false;
            }
            else
            { 
                //Determine if wrestler can fight through the pain
                int valueToBeat = 80;

                if(healthInfo.ArmHealth.Equals("Hurt")|| healthInfo.LegHealth.Equals("Hurt")|| healthInfo.NeckHealth.Equals("Hurt") || healthInfo.BodyHealth.Equals("Hurt"))
                {
                    valueToBeat = 65;
                }
                if (healthInfo.ArmHealth.Equals("Injured") || healthInfo.LegHealth.Equals("Injured") || healthInfo.NeckHealth.Equals("Injured") || healthInfo.BodyHealth.Equals("Injured"))
                {
                    valueToBeat = 80;
                }
                if (healthInfo.ArmHealth.Equals("Broken") || healthInfo.LegHealth.Equals("Broken") || healthInfo.NeckHealth.Equals("Broken") || healthInfo.BodyHealth.Equals("Broken"))
                {
                    valueToBeat = 95;
                }

                //Determine if wrestler's discretion affects their ability to fight through the pain.
                int discretion = plObj.WresParam.aiParam.discreation;
                if(discretion <= 25)
                {
                    valueToBeat -= 10;
                }
                else if (discretion >25 && discretion <= 50)
                {
                    valueToBeat -= 5;
                }
                else if (discretion >75)
                {
                    valueToBeat += 5;
                }

                int rngValue = UnityEngine.Random.Range(1, valueToBeat);
                if(rngValue >= valueToBeat)
                {
                    plObj.isKO = false;
                }
            }
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

                    //Since injuries may result in removing the KO state, this now needs to be reworked
                    //if (plObj.isKO)
                    //{
                    //    continue;
                    //}
                    //Avoid calculating the recovery time twice for criticals
                    if (!injuredBeforeMatch[i])
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
                            QoL_Form.form.UpdateWrestlerHealthInfo(healthInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    L.D("Error Updating Injured Players: " + ex.Message);
                }
            }
        }

        #region Dive Injury Logic
        [Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        public static void CheckDiveInjury(Player plObj)
        {
            if (!canSelfCrit)
            {
                return;
            }
            if (plObj.animator.SkillSlotID == SkillSlotEnum.CornerDive_A || plObj.animator.SkillSlotID == SkillSlotEnum.CornerDive_B || plObj.animator.SkillSlotID == SkillSlotEnum.CornerDive_XA || plObj.animator.SkillSlotID == SkillSlotEnum.CornerDive_X ||
                plObj.animator.SkillSlotID == SkillSlotEnum.DiveFromTopOfCage || plObj.animator.SkillSlotID == SkillSlotEnum.RunAndDiveToOutOfRing || plObj.animator.SkillSlotID == SkillSlotEnum.DiveFromNearRopeToOutOfRing ||
                plObj.animator.SkillSlotID == SkillSlotEnum.DiveFromApronToInsideRing || plObj.animator.SkillSlotID == SkillSlotEnum.RunFlyDown || plObj.animator.SkillSlotID == SkillSlotEnum.RunFlyStand)
            {
                if (!selfCritCheck[plObj.PlIdx])
                {
                    CalculateInjuryChance(plObj);
                    selfCritCheck[plObj.PlIdx] = true;
                }
            }
            else
            {
                selfCritCheck[plObj.PlIdx] = false;
            }

        }
        #endregion

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
                healthInfo = new WrestlerHealth(name, injuryTypes[0], injuryTypes[0], injuryTypes[0], injuryTypes[0], recovery, 0, DateTime.Now);
            }
            return healthInfo;
        }
        public static void AnnounceInjury(String wrestler, String part, String health)
        {
            if (QoL_Form.form.ij_notifications.Checked)
            {
                DispNotification.inst.Show(wrestler + "'s " + part + " is " + health + "!");
            }
        }
        public static void CalculateInjuryChance(Player plObj)
        {
            FightStyleEnum style = plObj.WresParam.fightStyle;
            int agilitySkill = agilitySkill = plObj.WresParam.atkParam[6];
            CriticalRateEnum criticalRate = GlobalWork.GetInst().MatchSetting.CriticalRate;

            if (criticalRate == CriticalRateEnum.Off)
            {
                return;
            }

            //Determine the style modifier for injury calculation
            int styleModifier = 1;
            switch (style)
            {
                case FightStyleEnum.Junior:
                case FightStyleEnum.Luchador:
                case FightStyleEnum.Mysterious:
                case FightStyleEnum.Panther:
                    styleModifier = 10;
                    break;
                case FightStyleEnum.Orthodox:
                case FightStyleEnum.American:
                case FightStyleEnum.Power:
                case FightStyleEnum.Technician:
                    styleModifier = 5;
                    break;
                case FightStyleEnum.Shooter:
                case FightStyleEnum.Wrestling:
                case FightStyleEnum.Devilism:
                case FightStyleEnum.Fighter:
                case FightStyleEnum.Grappler:
                case FightStyleEnum.Ground:
                case FightStyleEnum.Heel:
                    styleModifier = 2;
                    break;
                case FightStyleEnum.Giant:
                    styleModifier = 1;
                    break;
            }

            //Determine the rate modifier for injury calculation
            int rateModifier = 1;
            switch (criticalRate)
            {
                case CriticalRateEnum.Double:
                    rateModifier = 10;
                    break;
                case CriticalRateEnum.Normal:
                    rateModifier = 5;
                    break;
                case CriticalRateEnum.Half:
                    rateModifier = 2;
                    break;
            }

            //Calculate chance of injury
            int rngCeiling = (agilitySkill * styleModifier) + rateModifier;
            int rngValue = UnityEngine.Random.Range(0, rngCeiling);
            if (rngValue > agilitySkill * styleModifier)
            {
                plObj.isKO = true;
                selfCritSkills[plObj.PlIdx] = plObj.animator.CurrentSkill;
                plObj.ProcessCritical(plObj.animator.CurrentSkill);
            }
        }
        #endregion
    }
}
