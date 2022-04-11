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

        Queue<cell> rcq;
        Stack<cell> rcs;

        private bool _reachedEnd;
        private int _moveCount;


        public int moveCount
        {
            get { return _moveCount; }
        }

        //up, left, down, right priority order
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
            int nodesLeftInLayer = 0;
            int nodesInNextLayer = 0;
            rcq = new Queue<cell>();
            rcq.Enqueue(gridSpace.area[startCell.row][startCell.col]);

            gridSpace.area[startCell.row][startCell.col].Visited = true;

            while (rcq.Count > 0)
            {
                cell currentCell = rcq.Dequeue();
                int r = currentCell.row;
                int c = currentCell.col;

                searchedcells.Add(currentCell);

                if (currentCell.isGoal)
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
                    gridSpace.area[nextrow][nextcol].parentCell = currentCell;
                    rcq.Enqueue(gridSpace.area[nextrow][nextcol]);

                    gridSpace.area[nextrow][nextcol].Visited = true;
                    nodesInNextLayer++;

                }

                nodesLeftInLayer--;

                if (nodesLeftInLayer < 1)
                {
                    nodesLeftInLayer = nodesInNextLayer;
                    nodesInNextLayer = 0;
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
            rcs = new Stack<cell>();
            rcs.Push(gridSpace.area[startCell.row][startCell.col]);

            while (rcs.Count > 0)
            {
                cell currentCell = (cell)rcs.Pop();

                int r = currentCell.row;
                int c = currentCell.col;

                searchedcells.Add(currentCell);

                if (gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                currentCell.Visited = true;
                _moveCount++;


                for (int i = 3; i >= 0; i--)
                {
                    int adjx = r + _rd[i];
                    int adjy = c + _cd[i];

                    if (!isValid(adjx, adjy))
                        continue;

                    gridSpace.area[adjx][adjy].parentCell = currentCell;
                    rcs.Push(gridSpace.area[adjx][adjy]);
                }
            }

            if (_reachedEnd)
                return searchedcells;
            return null;
        }

        public List<cell> gbfsTreeTraversal()
        {
            rcs = new Stack<cell>();
            rcs.Push(gridSpace.area[startCell.row][startCell.col]);

            while (rcs.Count > 0)
            {
                cell currentCell = (cell)rcs.Pop();

                searchedcells.Add(currentCell);

                if (gridSpace.area[currentCell.row][currentCell.col].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                currentCell.Visited = true;
                _moveCount++;

                List<cell> cellstoadd = new List<cell>();

                for (int i = 3; i >= 0; i--)
                {
                    int adjr = currentCell.row + _rd[i];
                    int adjc = currentCell.col + _cd[i];

                    if (!isValid(adjr, adjc))
                        continue;
                    gridSpace.area[adjr][adjc].parentCell = currentCell;
                    cellstoadd.Add(gridSpace.area[adjr][adjc]);
                }

                IEnumerable<cell> sortedcells =
                    from curcell in cellstoadd
                    orderby curcell.distanceToNearestGoal descending
                    select curcell;

                foreach (cell surroundingcell in sortedcells)
                    rcs.Push(surroundingcell);

            }

            if (_reachedEnd)
                return searchedcells;
            return null;
        }
        public List<cell> aStarTreetraversal()
        {
            List<cell>openlist = new List<cell>();
            openlist.Add(gridSpace.area[startCell.row][startCell.col]);



            while (openlist.Count > 0)
            {
                cell currentCell = openlist[0];
                for(int i = 1; i < openlist.Count(); i++)
                {
                    if (currentCell.totaldistance > openlist[i].totaldistance)
                        currentCell = openlist[i];
                }
                openlist.Remove(currentCell);

                int r = currentCell.row;
                int c = currentCell.col;

                searchedcells.Add(currentCell);


                if (gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                gridSpace.area[r][c].Visited = true;
                _moveCount++;

                List<cell> children = new List<cell>();
                for (int i = 3; i >= 0; i--)
                {
                    int adjr = r + _rd[i];
                    int adjc = c + _cd[i];

                    if (!isValid(adjr, adjc))
                        continue;

                    gridSpace.area[adjr][adjc].parentCell = currentCell;
                    children.Add(gridSpace.area[adjr][adjc]);
                }

                IEnumerable<cell> sortedcells =
                    from curcell in children
                    orderby curcell.distanceToNearestGoal descending
                    select curcell;

                foreach (cell childCell in children)
                {
                    bool skip = false;
                    childCell.distanceTostart = currentCell.distanceTostart + 1;
                    childCell.totaldistance = childCell.distanceTostart + childCell.distanceToNearestGoal;
                    childCell.parentCell = currentCell;


                    foreach (cell cell in openlist)
                    {
                        if (childCell.row == cell.row && childCell.col == cell.col)
                        {
                            if (childCell.distanceTostart > gridSpace.area[cell.row][cell.col].distanceTostart)
                                skip = true;
                        }
                    }

                    if(!skip)
                        openlist.Add(childCell);

                }


            }

            if (_reachedEnd)
                return searchedcells;
            return null;
        }


    }
}
