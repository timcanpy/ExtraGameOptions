using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using static Common_Classes.EnumLibrary;

namespace QoL_Mods.Data_Classes.Reversal
{
    public class ReversalMove : Move
    {
        private List<Move> replacementsList;

        public List<Move> ReplacementsList
        {
            get { return replacementsList; }
            set { replacementsList = value; }
        }

        public String SaveReversalData()
        {
            //Save rerversal move specific data first
            String data = GetMoveData() + lineDelimeter;
            int userIndex = 0;
            foreach (var move in replacementsList)
            {
                data += move.GetMoveData() + lineDelimeter;
                userIndex++;
            }

            return data;
        }

        public void LoadReversalData(String data)
        {
            try
            {
                var lines = data.Split(lineDelimeter);

                //First line always includes ReversalMove data
                var reversalData = lines[0].Split(fieldDelimeter);
                name = reversalData[0];
                id = Int32.Parse(reversalData[1]);
                type = (MoveType)Enum.Parse(typeof(MoveType), reversalData[2]);

                //Load the replacement data; we can skip the first line since it includes ReversalMove data
                replacementsList = new List<Move>();
                for (int i = 1; i < lines.Length; i++)
                {
                    var moveData = lines[i].Split(fieldDelimeter);
                    if (moveData.Length != 4)
                    {
                        continue;
                    }
                    Move move = new Move
                    {
                        Name = moveData[0],
                        ID = Int32.Parse(moveData[1]),
                        Type = (MoveType)Enum.Parse(typeof(MoveType), moveData[2]),
                        User = (Executor)Enum.Parse(typeof(Executor), moveData[3])
                    };

                    replacementsList.Add(move);
                }

                //Update move name with the latest version
                UpdateName();
                foreach (Move move in replacementsList)
                {
                    move.UpdateName();
                }

            }
            catch (Exception e)
            {
                L.D("LoadReversalDataException: " + e);
            }
        }

        public void AddReplacement(Move move)
        {
            replacementsList.Add(move);
        }
    }
}
