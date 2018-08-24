using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DG;

namespace QoL_Mods
{
    [FieldAccess(Class = "Referee", Field = "RefereeingFinishCheck_OverTheTopRopeOn", Group = "ExtraFeatures")]
    [FieldAccess(Class = "MatchMain", Field = "CreatePlayers", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Player", Field = "ProcessThrowOverTheTopRope", Group = "ExtraFeatures")]
    [FieldAccess(Class = "PlayerController_AI", Field = "CheckOverTheTopRope", Group = "ExtraFeatures")]
    [FieldAccess(Class = "PlayerController_AI", Field = " ProcessOutOfRing", Group = "ExtraFeatures")]
    [FieldAccess(Class = "Player", Field = "UpdatePlayer", Group = "MoreMatchTypes")]
    [FieldAccess(Class = "Player", Field = "ProcessAttackHit_Normal_General", Group = "MoreMatchTypes")]
    class ActionUnlock
    {
        #region Variables
        public static bool enableTopRopeThrow = false;
        #endregion

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        public static void UnlockAbilities()
        {
            MatchSetting settings = GlobalWork.GetInst().MatchSetting;

            //Enable Top Rope Throw
            if (QoL_Form.form.au_overTopRope.Checked && GlobalWork.inst.MatchSetting.BattleRoyalKind == BattleRoyalKindEnum.Off)
            {
                enableTopRopeThrow = true;
            }
            else
            {
                enableTopRopeThrow = false;
            }
        }

        //[Hook(TargetClass = "Referee", TargetMethod = "RefereeingFinishCheck_OverTheTopRopeOn", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        //public static void IgnoreTopRopeRule()
        //{
        //    if (enableTopRopeThrow)
        //    {
        //        GlobalWork.GetInst().MatchSetting.isOverTheTopRopeOn = false;
        //    }
        //}

        //[Hook(TargetClass = "Player", TargetMethod = "ProcessThrowOverTheTopRope", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        //public static void AllowTopRopeThrow_Player()
        //{
        //    if (enableTopRopeThrow)
        //    {
        //        if (minuteCeiling < MatchMain.inst.matchTime.min)
        //        {
        //            GlobalWork.GetInst().MatchSetting.isOverTheTopRopeOn = true;
        //        }
        //        else
        //        {
        //            GlobalWork.GetInst().MatchSetting.isOverTheTopRopeOn = false;
        //        }
        //    }
        //}

        //[Hook(TargetClass = "PlayerController_AI", TargetMethod = "CheckOverTheTopRope", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        //public static void AllowTopRopeThrow_AI()
        //{
        //    if (enableTopRopeThrow)
        //    {
        //        if (minuteCeiling < MatchMain.inst.matchTime.min)
        //        {
        //            GlobalWork.GetInst().MatchSetting.isOverTheTopRopeOn = true;
        //        }
        //        else
        //        {
        //            GlobalWork.GetInst().MatchSetting.isOverTheTopRopeOn = false;
        //        }
        //    }
        //}

        //[Hook(TargetClass = "Player", TargetMethod = "UpdatePlayer", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "ExtraFeatures")]
        //public static void UpdatePlayerState(Player player)
        //{
        //    if(enableTopRopeThrow && (minuteCeiling < MatchMain.inst.matchTime.min))
        //    {
        //        if((player.State == PlStateEnum.Down_FaceDown || player.State == PlStateEnum.Down_FaceUp) && player.Zone == ZoneEnum.OutOfRing)
        //        {
        //            player.isStandingStunOK = true;
        //            //player.AddStunTime(600);
        //        }
        //    }
        //}

        //[Hook(TargetClass = "Player", TargetMethod = "ProcessAttackHit_Normal_General", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ExtraFeatures")]
        //public static void MakeOverRopeThrowsStun()
        //{
        //    if (enableTopRopeThrow && (minuteCeiling < MatchMain.inst.matchTime.min))
        //    {
        //        GlobalWork.GetInst().MatchSetting.isOverTheTopRopeOn = false;
        //    }
        //}

        #region Carl's Code
        //        [Hook(TargetClass = "Player", TargetMethod = "ProcessGrapple_Normal", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersRef, Group = "ThrowOut")]
        //        public static void overrideHammerThrow(Player plObj, ref PadBtnEnum padPush, ref PadBtnEnum padOn)
        //        {
        //            if (padPush != PadBtnEnum.Run)
        //            {
        //                return;
        //            }
        //            if (plObj.Zone != ZoneEnum.InRing)
        //            {
        //                return;
        //            }

        //            Player opPlObj = global::PlayerMan.inst.GetPlObj(plObj.TargetPlIdx);
        //            if (!opPlObj)
        //            {
        //                return;
        //            }

        //            if (throwOut[plObj.PlIdx] == null)
        //            {
        //                return;
        //            }
        //            int rnd = UnityEngine.Random.Range(0, 100);
        //            if (rnd > throwOut[plObj.PlIdx].Logic)
        //            {
        //                return;
        //            }

        //            padPush = PadBtnEnum.SpMove | PadBtnEnum.Run;
        //        }

        //        private void ThrowOutLogicForm_Load(object sender, EventArgs e)
        //        {
        //            LoadTOL();
        //            UpdateTOL();
        //            UpdateTOLList();
        //        }

        //        private void LoadTOL()
        //        {
        //            if (File.Exists("./ModSuiteData/ThowOutLogic.dat"))
        //            {
        //                StreamReader tReader = new StreamReader("./ModSuiteData/ThowOutLogic.dat");
        //                while (tReader.Peek() != -1)
        //                {
        //                    ThrowOutLogic nTOL = new ThrowOutLogic();
        //                    nTOL.Name = tReader.ReadLine();
        //                    nTOL.Logic = int.Parse(tReader.ReadLine());
        //                    ThrowOut.throwOutLogic.Add(nTOL);
        //                }
        //                tReader.Dispose();
        //                tReader.Close();
        //            }
        //        }

        //        private void UpdateTOL()
        //        {
        //            foreach (EditWrestlerData edit in SaveData.inst.editWrestlerData)
        //            {
        //                bool Found = false;
        //                string Name = DataBase.GetWrestlerFullName(edit.wrestlerParam);
        //                foreach (ThrowOutLogic tol in ThrowOut.throwOutLogic)
        //                {
        //                    if (tol.Name == Name)
        //                    {
        //                        Found = true;
        //                        break;
        //                    }
        //                }

        //                if (!Found)
        //                {
        //                    ThrowOutLogic nTOL = new ThrowOutLogic() { Name = Name, Logic = 0 };
        //                    for (int i = 2; i < 8; i++)
        //                    {
        //                        if (edit.wrestlerParam.aiParam.opponentOutOfRing[i] > 25)
        //                        {
        //                            nTOL.Logic = UnityEngine.Random.Range(5, 15);
        //                            break;
        //                        }
        //                    }
        //                    ThrowOut.throwOutLogic.Add(nTOL);
        //                }
        //            }

        //            if (!File.Exists("./ModSuiteData/ThowOutLogic.dat"))
        //            {
        //                SaveTOL();
        //            }
        //        }

        //        public static List<ThrowOutLogic> throwOutLogic = new List<ThrowOutLogic>();
        //        public static ThrowOutLogic[] throwOut = new ThrowOutLogic[8] { null, null, null, null, null, null, null, null };

        //        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ThrowOut")]
        //        public static void ConfigureThrowOutLogic()
        //        {
        //            for (int i = 0; i < 8; i++)
        //            {
        //                throwOut[i] = null;
        //                if (GlobalWork.inst.MatchSetting.matchWrestlerInfo[i].entry == false || GlobalWork.inst.MatchSetting.matchWrestlerInfo[i].isSecond)
        //                {
        //                    continue;
        //                }

        //                string name = DataBase.GetWrestlerFullName(GlobalWork.inst.MatchSetting.matchWrestlerInfo[i].param);
        //                foreach (ThrowOutLogic tol in throwOutLogic)
        //                {
        //                    if (tol.Name == name)
        //                    {
        //                        throwOut[i] = tol;
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        [Hook(TargetClass = "Menu_Title", TargetMethod = "Awake", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "ThrowOut")]
        //        public static void LoadDatabase()
        //        {
        //            ThrowOutLogic nTOL = new ThrowOutLogic() { Name = "Carl Zilla", Logic = 100 };
        //            throwOutLogic.Add(nTOL);
        //        }

        //        public class ThrowOutLogic()
        //{
        //public string name;
        //        public int logic;

        //        public override ToString()
        //        {
        //            return name;
        //        }
        //    }

        //[FieldAccess(Class = "Referee", Field = "DownCntTbl", Group = "LongerDownTime")]

        //public class LongerRefDownTimes
        //{
        //    public static int[] origDownCntTbl = new int[] { 40, 32, 24, 16, 8 };

        //    [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "LongerDownTime")]
        //    private void DoubleRefDownTime()
        //    {
        //        for (int i = 0; i < 5; i++)
        //        {
        //            Referee.DownCntTbl[i] = Referee.DownCntTbl[i] * 2;
        //        }
        //    }

        //    [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "CrowdChants")]
        //    public static void ResetRefDownTime()
        //    {
        //        Referee.DownCntTbl = origDownCntTbl;
        //    }
        //}
        #endregion

    }
}
