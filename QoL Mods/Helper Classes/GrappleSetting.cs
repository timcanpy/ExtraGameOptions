using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Helper_Classes
{
    public class GrappleSetting
    {
        private char dataSeparator = '-';

        private int low;
        private int medium;
        private int high;

        public int Low { get => low; set => low = value; }
        public int Medium { get => medium; set => medium = value; }
        public int High { get => high; set => high = value; }

        public String SaveSettings()
        {
            return "" + low + dataSeparator + medium + dataSeparator + high;
        }

        public void LoadSettings(String data)
        {
            try
            {
                var values = data.Split(dataSeparator);
                Int32.TryParse(values[0], out low);
                Int32.TryParse(values[1], out medium);
                Int32.TryParse(values[2], out high);
            }
            catch
            { }
        }
    }
}
