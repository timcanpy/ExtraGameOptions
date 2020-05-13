using MoveFileCleanup.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GetCustomIDs
{
    class Program
    {
        static void Main(string[] args)
        {
            String movePath;
            String loadOrderName = "LoadOrder.dat";
            String idFileName = "CustomMoveIDs.dat";
            String whiteSpace = "-----------------------------------------------------";
            List<Move> moveList = new List<Move>();

            //Check for NewMoves folder
            movePath = Path.Combine(Directory.GetCurrentDirectory(), "NewMoves");
            if (Directory.Exists(movePath))
            {
                //Check for LoadOrder.dat file
                movePath = Path.Combine(movePath, loadOrderName);
                Console.WriteLine(whiteSpace);

                if (File.Exists(movePath))
                {
                    foreach (var line in File.ReadAllLines(movePath))
                    {
                        if (line == String.Empty)
                        {
                            continue;
                        }

                        //Check for file
                        if (File.Exists(line))
                        {
                            //Get byte file path
                            var properties = (File.ReadAllLines(line));

                            //Create move object
                            Move move = new Move
                            {
                                Name = Path.GetFileName(line),
                                Properties = properties
                            };

                            moveList.Add(move);
                        }
                        else
                        {
                            Console.WriteLine(Path.GetFileName(line) + " does not exist in the NewMoves directory.");
                            Console.WriteLine(whiteSpace);
                        }

                    }

                    Console.WriteLine("There are currently " + moveList.Count + " installed moves (includes move angles).");
                    Console.WriteLine(whiteSpace);

                    String idPath = Path.Combine(Directory.GetCurrentDirectory(), idFileName);
                    if (File.Exists(idPath))
                    {
                        File.Delete(idPath);
                    }

                    using (StreamWriter sw = File.AppendText(idPath))
                    {
                        foreach (var move in moveList)
                        {
                            String moveData = move.Properties[0] + ":" + move.Properties[121];
                            sw.WriteLine(moveData);
                        }
                    }

                    Console.WriteLine(idPath + " has been created.");
                }
                else
                {
                    Console.WriteLine("LoadOrder file does not exist");
                }
            }
            else
            {
                Console.WriteLine("New Moves folder does not exist");
            }

            Console.WriteLine(whiteSpace);
            Console.WriteLine("Press any key to end the program.");
            Console.ReadKey();
        }
    }
}
