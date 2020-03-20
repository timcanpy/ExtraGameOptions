using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QoL_Mods
{
    public partial class Reports : Form
    {
        #region Variables
        private static String[] saveFolderNames = new String[] { "./EGOData/" };
        private static String[] reportFileNames = new String[] { "LowRecoveryEdits.txt" };
        private static List<String> editList = new List<String>();
        public static Reports form = null;

        #endregion
        public Reports()
        {
            form = this;
            InitializeComponent();
        }

        private void reports_recovery_Click(object sender, EventArgs e)
        {
            editList.Clear();
            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                if (current.wrestlerParam.hpRecovery == 0 && current.wrestlerParam.hpRecovery_Bleeding == 0
                                                          && current.wrestlerParam.spRecovery == 0 &&
                                                          current.wrestlerParam.spRecovery_Bleeding == 0)
                {
                    editList.Add(DataBase.GetWrestlerFullName(current.wrestlerParam));
                }
            }

            if (!Directory.Exists(saveFolderNames[0]))
            {
                Directory.CreateDirectory(saveFolderNames[0]);
            }

            String filePath = saveFolderNames[0] + reportFileNames[0];
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (String wrestler in editList)
                {
                    sw.WriteLine(wrestler);
                }
            }

        }
    }
}
