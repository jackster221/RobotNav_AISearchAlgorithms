using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class robot
    {
        public int[] currentloc { get; set; }
        public grid gridspace { get; set; }
        public agent aiagent { get; set; }
        

        public robot(grid gridspace, int[] loc)
        {
            this.gridspace = gridspace;
            aiagent = new agent(loc[1], loc[0], gridspace);
            currentloc = loc;
        }

        public List<cell> exitmaze(int selection)
        {
            switch (selection)
            {
                case 1:
                    return aiagent.bfsTreeTraversal();
                case 2:
                    return aiagent.dfsTreeTraversal();
                case 3:
                    gridspace.makegridinformed();
                    return aiagent.gbfsTreeTraversal();
                case 4:
                    gridspace.makegridinformed();
                    return aiagent.aStarTreetraversal();
            }
            return null;
        }
        
        

    }
}
