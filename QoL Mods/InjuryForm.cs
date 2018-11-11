using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QoL_Mods
{
    public partial class InjuryForm : Form
    {
        public InjuryForm()
        {
            InitializeComponent();
        }

        private static String[] injuryTypes = new String[] { "Healthy", "Bruised", "Sprained", "Hurt", "Injured", "Broken" };
        private static int injuryFloor = 3;
        private static int injuryCeiling = injuryTypes.Length;

        
    }
}
