using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QoL_Mods.Public
{
    public partial class RingOut : Form
    {
        public static RingOut form = null;

        public RingOut()
        {
            form = this;
            InitializeComponent();
        }
    }
}

