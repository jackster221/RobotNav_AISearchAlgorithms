using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class robot
    {
        private int[] _currentloc;
        private grid _gridspace;
        private agent _aiagent;
        

        private List<cell> _botpath;

        public robot(grid gridspace, int[] loc)
        {
            _aiagent = new agent(loc[0], loc[1], gridspace);
            currentloc = loc;
            _botpath = null;
        }

        public void exitmaze(int selection)
        {
            int moves = 0;
            switch (selection)
            {
                case 1:
                    moves = _aiagent.bfsTreeTraversal();
                    break;
                case 2:
                    moves = _aiagent.dfsTreeTraversal();
                    break;
                case 3:
                    break;
            }

            if (moves > 0)
            {
                Console.WriteLine("Robot escaped maze in " + moves + " moves.");
                Console.WriteLine("Robot completed maze at cell (" + _aiagent.currentcell.row + "," + _aiagent.currentcell.col + ").");
            }
            else
                Console.WriteLine("Robot could not complete the puzzle.");
            
        }
        
        

        public grid gridspace
        {
            get { return _gridspace; }
            set { _gridspace = value; }
        }
        public int[] currentloc
        {
            get { return _currentloc; }
            set { _currentloc = value; }
        }
        public int totalmoves
        {
            get { return _botpath.Count; }
        }
        public List<cell> botpath
        {
            get { return _botpath; }
        }
    }
}
