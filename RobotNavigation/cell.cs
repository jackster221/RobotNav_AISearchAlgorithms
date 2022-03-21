using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class cell
    {
        public int row { get; }
        public int col { get; }
        public bool isObstacle { get; set; }
        public bool isGoal { get; set; }
        public bool Visited { get; set; }
        public int distanceToNearestGoal { get; set; }


        public cell(int currow, int curcol)
        {
            distanceToNearestGoal = -1;
            row = currow;
            col = curcol;
            isGoal = false;
            isObstacle = false;
        }

    }
}
