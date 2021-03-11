using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QoL_Mods.Helper_Classes
{
    public class NorthRope
    {
        public static void Use_H(Player plObj, GameObject obj)
        {

        }
        public static void Collide(Player plObj, GameObject obj)
        {

        }
        public static void Trigger(Player plObj, GameObject obj)
        {
            if (plObj.Zone == ZoneEnum.InRing)
            {
                DispNotification.inst.Show($"{DataBase.GetWrestlerFullName(plObj.WresParam)} needs to avoid falling out of the ring!", 180);
            }
        }
    }

    public class WestRope
    {
        public static void Use_L(Player plObj, GameObject obj)
        {

        }
        public static void Use_M(Player plObj, GameObject obj)
        {

        }
        public static void Use_H(Player plObj, GameObject obj)
        {

        }
        public static void Collide(Player plObj, GameObject obj)
        {

        }
        public static void Trigger(Player plObj, GameObject obj)
        {

        }

    }

    public class SouthRope
    {
        public static void Use_L(Player plObj, GameObject obj)
        {

        }
        public static void Use_M(Player plObj, GameObject obj)
        {

        }
        public static void Use_H(Player plObj, GameObject obj)
        {

        }
        public static void Collide(Player plObj, GameObject obj)
        {

        }
        public static void Trigger(Player plObj, GameObject obj)
        {

        }

    }

    public class EastRope
    {
        public static void Trigger(Player plObj, GameObject obj)
        {
            if (plObj.Zone == ZoneEnum.InRing)
            {
                DispNotification.inst.Show($"{DataBase.GetWrestlerFullName(plObj.WresParam)} needs to avoid falling out of the ring!", 180);
            }
        }
    }
}
