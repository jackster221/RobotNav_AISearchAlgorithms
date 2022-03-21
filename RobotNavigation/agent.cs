using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class agent
    {
        public grid gridSpace { get; set; }
        public cell startCell { get; set; }
        public cell currentCell { get; set; }

        public List<cell> searchedcells { get; set; }

        Queue<Tuple<int, int>> rcq = new Queue<Tuple<int, int>>();
        Stack<Tuple<int, int>> rcs = new Stack<Tuple<int, int>>();

        private bool _reachedEnd;
        private int _moveCount;
        private int _nodesLeftInLayer;
        private int _nodesInNextLayer;

        public int moveCount
        {
            get { return _moveCount; }
        }

        //up, left, down, right
        private readonly int[] _rd = { -1, 0, 1, 0 };
        private readonly int[] _cd = { 0, -1, 0, 1 };

        public agent(int row, int col, grid gridspace)
        {
            searchedcells = new List<cell>();
            _reachedEnd = false;
            gridSpace = gridspace;
            startCell = gridSpace.area[row][col];
        }

        public bool isValid(int nextrow, int nextcol)
        {
            if (nextrow < 0 || nextrow > gridSpace.rowsize - 1)
                return false;
            else if (nextcol < 0 || nextcol > gridSpace.colsize - 1)
                return false;
            else if (gridSpace.area[nextrow][nextcol].isObstacle)
                return false;
            else if (gridSpace.area[nextrow][nextcol].Visited)
                return false;

            return true;
        }

        public List<cell> bfsTreeTraversal()
        {
            rcq.Enqueue(new Tuple<int, int>(startCell.row, startCell.col));

            gridSpace.area[startCell.row][startCell.col].Visited = true;

            while(rcq.Count > 0)
            {
                Tuple<int, int> curcell = rcq.Dequeue();
                int r = curcell.Item1;
                int c = curcell.Item2;

                currentCell = gridSpace.area[r][c];
                Console.WriteLine("(" + r + "," + c + ")");
                searchedcells.Add(currentCell);

                if (gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }
   
                for (int i = 0; i < 4; i++)
                {
                    int nextrow = r + _rd[i];
                    int nextcol = c + _cd[i];

                    if (!isValid(nextrow, nextcol))
                        continue;
                    
                    rcq.Enqueue(new Tuple<int, int>(nextrow, nextcol));

                    gridSpace.area[nextrow][nextcol].Visited = true;
                    _nodesInNextLayer++;
                    
                }
                
                _nodesLeftInLayer--;

                if(_nodesLeftInLayer < 1)
                {
                    _nodesLeftInLayer = _nodesInNextLayer;
                    _nodesInNextLayer = 0;
                    _moveCount++;
                }
            }
            if (_reachedEnd)
            {
                return searchedcells;
            }
            return null;
        }

        public List<cell> dfsTreeTraversal()
        {
            rcs.Push(new Tuple<int, int>(startCell.row, startCell.col));

            while (rcs.Count > 0)
            {
                Tuple<int, int> curCell = (Tuple<int, int>)rcs.Pop();

                int r = curCell.Item1;
                int c = curCell.Item2;

                if (!isValid(r, c))
                    continue;

                currentCell = gridSpace.area[r][c];
                searchedcells.Add(currentCell);
                Console.WriteLine("(" + r + "," + c + ")");

                if (gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                gridSpace.area[r][c].Visited = true;
                _moveCount++;
                

                for(int i = 3; i >= 0; i--)
                {
                    int adjx = r + _rd[i];
                    int adjy = c + _cd[i];
                    rcs.Push(new Tuple<int, int>(adjx, adjy));
                }
            }

            if (_reachedEnd)
                return searchedcells;
            return null;
        }

        public List<cell> gbfsTreeTraversal()
        {
            rcs.Push(new Tuple<int, int>(startCell.row, startCell.col));

            while (rcs.Count > 0)
            {
                Tuple<int, int> curCell = (Tuple<int, int>)rcs.Pop();

                int r = curCell.Item1;
                int c = curCell.Item2;

               
                currentCell = gridSpace.area[r][c];
                searchedcells.Add(currentCell);
                Console.WriteLine("(" + r + "," + c + ")");

                if (gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                gridSpace.area[r][c].Visited = true;
                _moveCount++;

                List<cell> cellstoadd = new List<cell>();

                for (int i = 3; i >= 0; i--)
                {
                    int adjr = r + _rd[i];
                    int adjc = c + _cd[i];

                    if (!isValid(adjr, adjc))
                        continue;
                    cellstoadd.Add(gridSpace.area[adjr][adjc]);
                }

                IEnumerable<cell> sortedcells =
                    from curcell in cellstoadd
                    orderby curcell.distanceToNearestGoal descending
                    select curcell;

                foreach(cell surroundingcell in sortedcells)
                    rcs.Push(new Tuple<int, int>(surroundingcell.row, surroundingcell.col));
                
            }

            if (_reachedEnd)
                return searchedcells;
            return null;
        }
        public List<cell> aStarTreetraversal()
        {
            rcs.Push(new Tuple<int, int>(startCell.row, startCell.col));

            while (rcs.Count > 0)
            {
                Tuple<int, int> curCell = (Tuple<int, int>)rcs.Pop();

                int r = curCell.Item1;
                int c = curCell.Item2;


                currentCell = gridSpace.area[r][c];
                searchedcells.Add(currentCell);
                Console.WriteLine("(" + r + "," + c + ")");

                if (gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                gridSpace.area[r][c].Visited = true;
                _moveCount++;

                List<cell> cellstoadd = new List<cell>();

                for (int i = 3; i >= 0; i--)
                {
                    int adjr = r + _rd[i];
                    int adjc = c + _cd[i];

                    if (!isValid(adjr, adjc))
                        continue;
                    cellstoadd.Add(gridSpace.area[adjr][adjc]);
                }

                IEnumerable<cell> sortedcells =
                    from curcell in cellstoadd
                    orderby curcell.distanceToNearestGoal descending
                    select curcell;

                foreach (cell surroundingcell in sortedcells)
                    rcs.Push(new Tuple<int, int>(surroundingcell.row, surroundingcell.col));

            }

            if (_reachedEnd)
                return searchedcells;
            return null;
        }

        
        
    }
}
