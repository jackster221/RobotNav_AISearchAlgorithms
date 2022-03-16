using System;
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

            robot bot = new robot(gridspace, convertCoords(lines[1]));


            string[] splitline = lines[2].Split('|');

            for(int i = 0; i < splitline.Length; i++)
            {
                gridspace.applygoalstate(convertCoords(splitline[i]), true);
            }

            for(int i = 3; i < lines.Length; i++)
            {
                gridspace.applyobstaclestate(convertCoords(lines[i]), true);
            }

            printgrid(gridspace);

            Console.WriteLine("Select option:" + Environment.NewLine + "1 - BFS" + Environment.NewLine + "2 - DFS");
            int result = Convert.ToInt32(Console.ReadLine());

            bot.exitmaze(result);
        }

        static void printgrid(grid gridspace)
        {
            string result = "";
            for(int i = 0; i < gridspace.rowsize; i++)
            {
                for(int j = 0; j < gridspace.colsize; j++)
                {
                    if(gridspace.area[i][j].isObstacle)
                        result += "X" + " ";
                    else
                        result += "O" + " ";
                }
                result += System.Environment.NewLine;
            }
            Console.WriteLine(result);
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
    }
}
