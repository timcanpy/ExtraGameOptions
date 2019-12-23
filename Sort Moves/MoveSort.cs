using DG;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sort_Moves
{
    #region Group Descriptions
    [GroupDescription(Group = "View Move Sort", Name = "Custom Move Sort", Description = "Sorts moves according to their custom names.")]
    #endregion

    #region Field Access
    [FieldAccess(Class = "Dialog_ArtsSetting", Field = "WretelerActive", Group = "View Move Sort")]
    #endregion
    public class MoveSort
    {
        #region Sort Moves By Custom Names

        [Hook(TargetClass = "Dialog_ArtsSetting", TargetMethod = "WretelerActive", InjectionLocation = int.MaxValue,
            InjectDirection = HookInjectDirection.Before,
            InjectFlags = HookInjectFlags.None,
            Group = "View Move Sort")]
        public static void SortMoves()
        {
            try
            {
                List<SkillInfo> skillList = new List<SkillInfo>();
                var moveList = SkillInfoManager.inst.skillInfoList;

                foreach (var pair in moveList)
                {
                    var element = pair;
                    element.Value.skillName[1] = DataBase.GetSkillName(pair.Key);

                    if (element.Value.idxStr.Trim().Equals(String.Empty))
                    {
                        element.Value.idxStr = pair.Key.ToString();
                    }
                    skillList.Add(element.Value);
                }

                var sortedList = skillList.OrderBy(skill => skill.skillName[1]).ToList();
                int[] sortIds = new Int32[91];
                for (int i = 0; i < sortedList.Count(); i++)
                {
                    if (sortedList[i].idxStr.Trim().Equals(String.Empty))
                    {
                        continue;
                    }

                    int id = Int32.Parse(sortedList[i].idxStr);
                    for (int j = 0; j < 91; j++)
                    {
                        if (moveList[(SkillID)id].sortingOrder[j] > 0)
                        {
                            sortIds[j]++;
                            moveList[(SkillID)id].sortingOrder[j] = sortIds[j];
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                L.D("SortMoveException: " + exception);
            }
        }
        #endregion
    }
}
