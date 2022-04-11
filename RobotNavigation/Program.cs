using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace RobotNavigation
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\Jack_\\source\\repos\\RobotNavigation\\RobotNavigation\\RobotNav-test.txt";

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            grid gridspace = new grid(convertCoords(lines[0]));

            int[] startLoc = convertCoords(lines[1]);
            robot bot = new robot(gridspace, startLoc);


            string[] splitline = lines[2].Split('|');

            for(int i = 0; i < splitline.Length; i++)
            {
                gridspace.applygoalstate(convertCoords(splitline[i]), true);
            }

            for(int i = 3; i < lines.Length; i++)
            {
                gridspace.applyobstaclestate(convertCoords(lines[i]), true);
            }

            gridspace.makegridinformed();
            printgrid(gridspace, startLoc);

            Console.WriteLine("Select option:" + Environment.NewLine + "1 - BFS" + Environment.NewLine + "2 - DFS" + Environment.NewLine + "3 - GFBS" + Environment.NewLine + "4 - A*");
            int result = Convert.ToInt32(Console.ReadLine());

            List<cell> path = bot.exitmaze(result);

            if (path != null)
                showresults(path, gridspace, startLoc);   
            else
                Console.WriteLine("Robot could not complete the puzzle.");
        }

  
        static void printgrid(grid gridspace, int[] startloc, List<cell> path = null)
        {

            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < gridspace.rowsize; i++)
            {
                for(int j = 0; j < gridspace.colsize; j++)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    if (gridspace.area[i][j].isObstacle)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                    }                      
                    else if (gridspace.area[i][j].isGoal)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write("G ");
                    }                       
                    else if(gridspace.area[i][j].row == startloc[1] && gridspace.area[i][j].col == startloc[0])
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("S ");
                    }
                    else
                    {
                        if(path != null)
                        {
                            if (path.Contains(gridspace.area[i][j]))
                            {
                               Console.BackgroundColor = ConsoleColor.Yellow;
                            }
                        }
                        else 
                            Console.BackgroundColor = ConsoleColor.Gray;

                        //Console.Write(gridspace.area[i][j].distanceToNearestGoal + " ");
                        Console.Write("O ");
                    }
  
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static int[] convertCoords(string gridcoords)
        {
            string trimmed = gridcoords.Trim(' ', ';', '.', '!', ']', '[', ')', '(');
            string[] split = trimmed.Split(',');

            if(split.Length == 2)
            {
                int[] result = { Convert.ToInt32(split[0]), Convert.ToInt32(split[1]) };
                return result;
            }
            else
            {
                int[] result = { Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]) };
                return result;
            }

          
        }

        static void showresults(List<cell> path, grid gridspace, int[] startLoc)
        {
            cell curcell;
            Stack<cell> exitorder = new Stack<cell>();
            int count = 0;

            //extract shortest path from list of all searched nodes and place in new list to determine directions the bot took
            foreach (cell tempcell in path)
            {
                if (tempcell.isGoal)
                {
                    curcell = tempcell;
                    Console.WriteLine("Path taken was:");
                    while (curcell.parentCell != null)
                    {
                        exitorder.Push(curcell);
                        curcell = curcell.parentCell;
                        count++;
                    }

                    break;
                }
            }

            curcell = gridspace.area[startLoc[1]][startLoc[0]];
            while (exitorder.Count > 0)
            {
                cell next = exitorder.Pop();
                if (next.row < curcell.row)
                    Console.Write("Up," + " ");
                else if (next.row > curcell.row)
                    Console.Write("Down," + " ");
                else if (next.col > curcell.col)
                    Console.Write("Right," + " ");
                else if (next.col < curcell.col)
                    Console.Write("Left," + " ");

                curcell = next;
            }
            Console.WriteLine();

            printgrid(gridspace, startLoc, path);
            Console.WriteLine("Robot escaped maze in " + count + " moves and searched " + (path.Count - 1) + " cells.");
            Console.WriteLine("Robot completed maze at cell (" + path[path.Count - 1].row + "," + path[path.Count - 1].col + ").");
        }

    }
}
