using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes
{
    public enum TauntStartPosition
    {
        FaceUp,
        FaceDown
    }

    public enum TauntEndPosition
    {
        Standing,
        Grounded
    }

    public enum TauntExecution
    {
        Reset,
        Skip,
        Force,
        Executed
    }

    public enum DamageState
    {
        Low,
        Medium,
        Heavy,
        Critical
    }

    public enum Modversion
    {
        v1,
        v2
    }
}
