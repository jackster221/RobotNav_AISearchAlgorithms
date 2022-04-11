using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    
    public class grid
    {
        public List<List<cell>> area { get; set; }
        public int rowsize { get; }
        public int colsize { get; }
        public List<cell> goalcells { get; set; }

        public grid(int[] gridsize)
        {
            rowsize = gridsize[0];
            colsize = gridsize[1];
            goalcells = new List<cell>();
            area = new List<List<cell>>();

            for (int i = 0; i < rowsize; i++)
            {
                area.Add(new List<cell>());
                for(int j = 0; j < colsize; j++)
                {
                    area[i].Add(new cell(i, j));
                }
            }

        }

        public void applygoalstate(int[] gridloc, bool state)
        {
            if (area[gridloc[1]][gridloc[0]].isObstacle && state == true)
            {
                Console.WriteLine("Cannot assign goal state while obstacle is present");
            }
            else
            {
                area[gridloc[1]][gridloc[0]].isGoal = state;
                goalcells.Add(area[gridloc[1]][gridloc[0]]);
            }
        }

        public void applyobstaclestate(int[] obsgrid, bool state)
        {
            for (int i = obsgrid[1]; i < (obsgrid[1] + obsgrid[3]); i++)
            {
                for (int j = obsgrid[0]; j < (obsgrid[0] + obsgrid[2]); j++)
                {
                    area[i][j].isObstacle = state;
                }
            }
        }

        //calculate manhattan distance from each cell to nearest endpoint 
        public void makegridinformed()
        {
            foreach(cell gcell in goalcells)
            {
                for(int i = 0; i < rowsize; i++)
                {
                    for(int j = 0; j < colsize; j++)
                    {
                        int currowsize = area[i][j].row;
                        int curcolsize = area[i][j].col;

                        if (currowsize > gcell.row)
                            currowsize -= gcell.row;
                        else
                            currowsize = gcell.row - currowsize;

                        if (curcolsize > gcell.col)
                            curcolsize -= gcell.col;
                        else
                            curcolsize = gcell.col - curcolsize;

                        int dist = currowsize + curcolsize;

                        if (area[i][j].distanceToNearestGoal == -1)
                            area[i][j].distanceToNearestGoal = dist;
                        else if (area[i][j].distanceToNearestGoal > dist)
                            area[i][j].distanceToNearestGoal = dist;

                    }
                }
            }
        }

    }
}
