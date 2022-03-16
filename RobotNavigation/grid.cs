using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    
    public class grid
    {
        private List<List<cell>> _area;
        private int _rowsize;
        private int _colsize;

        public grid(int[] gridsize)
        {
            _rowsize = gridsize[0];
            _colsize = gridsize[1];

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

        public List<List<cell>> area
        {
            get { return _area; }
            set { _area = value; }
        }
        public int rowsize
        {
            get { return _rowsize; }
        }
        public int colsize
        {
            get { return _colsize; }
        }
    }
}
