using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QoL_Mods.Private
{
    public partial class DynamicHighlightsForm : Form
    {
        public DynamicHighlightsForm()
        {
            highlightsForm = this;
            InitializeComponent();
            eh_sweatSpeed.SelectedIndex = 0;
            eh_isDefaultLevel.Checked = true;
            eh_sweatLevel.SelectedIndex = 3;
        }
        public static DynamicHighlightsForm highlightsForm = null;
    }
}
