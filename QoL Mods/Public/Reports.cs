using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModPack;
//using Newtonsoft.Json;
using DG;
//using Newtonsoft.Json;

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
            FormClosing += Reports_FormClosing;
        }

        private void Reports_FormClosing(object sender, FormClosingEventArgs e)
        {
            //    try
            //    {
            //        SaveTeams();
            //    }
            //    catch (Exception ex)
            //    {
            //        L.D("Reports Team Save Error: " + ex); 
            //    }
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

        ////Temporary, until FireProWar form build issue is resolved
        //private void SaveTeams()
        //{
        //    List<String> jsonTeams = new List<string>();
        //    string userID = "76561198100955117";

        //    foreach (Team team in ModPack.ModPack.Teams)
        //    {
        //        jsonTeams.Add(CreateJSONTeam(team.Members, team.Name, userID));
        //    }

        //    File.WriteAllLines("./EGOData/Teams.json", jsonTeams.ToArray());
        //}

        //private static String CreateJSONTeam(List<String> members, String name, String userID)
        //{
        //    StringWriter stringWriter = new StringWriter();
        //    JsonTextWriter writer = new JsonTextWriter(stringWriter);

        //    writer.WriteStartObject();

        //    writer.WritePropertyName("id");
        //    writer.WriteValue(userID + name);

        //    writer.WritePropertyName("ownerId");
        //    writer.WriteValue(userID);

        //    writer.WritePropertyName("name");
        //    writer.WriteValue(name);

        //    writer.WritePropertyName("members");
        //    writer.WriteStartArray();
        //    foreach (String member in members)
        //    {
        //        writer.WriteValue(member);
        //    }
        //    writer.WriteEndArray();

        //    writer.WriteEndObject();

        //    return stringWriter.ToString();
        //}

        //private void btn_teamSave_Click(object sender, EventArgs e)
        //{
        //    //SaveTeams();
        //}

        private void critTestbtn_Click(object sender, EventArgs e)
        {
            try
            {
                Player plObj = PlayerMan.inst.GetPlObj(0);
                if (plObj)
                {
                    plObj.ProcessCritical(plObj.GetSkillData_Equip(SkillSlotEnum.Grapple_XA));
                }
            }
            catch (Exception ex)
            {
                L.D("Error on Crit Test Btn: " + ex.Message);
            }
        }
    }
}
