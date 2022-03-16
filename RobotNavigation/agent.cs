using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class agent
    {
        private grid _gridSpace;
        private cell _startCell;
        private cell _currentCell;

        Queue<Tuple<int, int>> rcq = new Queue<Tuple<int, int>>();
        Stack<Tuple<int, int>> rcs = new Stack<Tuple<int, int>>();

        private bool _reachedEnd;
        private int _moveCount;
        private int _nodesLeftInLayer;
        private int _nodesInNextLayer;

        //up, left, down, right
        private readonly int[] _rd = { -1, 0, 1, 0 };
        private readonly int[] _cd = { 0, -1, 0, 1 };

        public agent(int row, int col, grid gridspace)
        {
            _reachedEnd = false;
            _gridSpace = gridspace;
            _startCell = _gridSpace.area[row][col];
        }

        public void bfsnextmoves(int row, int col)
        {
            /*for (int i = 0; i < 4; i++)
            {
                int nextrow = row + _rd[i];
                int nextcol = col + _cd[i];

                if (nextrow < 0 || nextrow > _gridSpace.rowsize - 1)
                    continue;
                else if (nextcol < 0 || nextcol > _gridSpace.colsize - 1)
                    continue;
                else if (_gridSpace.area[nextrow][nextcol].isObstacle)
                    continue;
                else if (_gridSpace.area[nextrow][nextcol].visited)
                    continue;

                rcq.Enqueue(new Tuple<int, int>(nextrow, nextcol));

                _gridSpace.area[nextrow][nextcol].visited = true;
                _nodesInNextLayer++;
            }*/

        }

        public int bfsTreeTraversal()
        {
            rcq.Enqueue(new Tuple<int, int>(_startCell.row, _startCell.col));

            _gridSpace.area[_startCell.row][_startCell.col].visited = true;

            while(rcq.Count > 0)
            {
                Tuple<int, int> curcell = rcq.Dequeue();
                int r = curcell.Item1;
                int c = curcell.Item2;

                _currentCell = _gridSpace.area[r][c];

                if (_gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                //bfsnextmoves(r, c);

                ////
                for (int i = 0; i < 4; i++)
                {
                    int nextrow = r + _rd[i];
                    int nextcol = c + _cd[i];

                    if (nextrow < 0 || nextrow > _gridSpace.rowsize - 1)
                        continue;
                    else if (nextcol < 0 || nextcol > _gridSpace.colsize - 1)
                        continue;
                    else if (_gridSpace.area[nextrow][nextcol].isObstacle)
                        continue;
                    else if (_gridSpace.area[nextrow][nextcol].visited)
                        continue;

                    rcq.Enqueue(new Tuple<int, int>(nextrow, nextcol));

                    _gridSpace.area[nextrow][nextcol].visited = true;
                    _nodesInNextLayer++;
                    Console.WriteLine("(" + nextrow + "," + nextcol + ")");
                }
                ////
                ///
                _nodesLeftInLayer--;

                if(_nodesLeftInLayer < 1)
                {
                    _nodesLeftInLayer = _nodesInNextLayer;
                    _nodesInNextLayer = 0;
                    _moveCount++;
                }
            }
            if (_reachedEnd)
                return _moveCount;
            return -1;
        }

        public bool isValid(int nextrow, int nextcol)
        {
            if (nextrow < 0 || nextrow > _gridSpace.rowsize - 1)
                return false;
            else if (nextcol < 0 || nextcol > _gridSpace.colsize - 1)
                return false;
            else if (_gridSpace.area[nextrow][nextcol].isObstacle)
                return false;
            else if (_gridSpace.area[nextrow][nextcol].visited)
                return false;

            return true;
        }

        public int dfsTreeTraversal()
        {
            rcs.Push(new Tuple<int, int>(_startCell.row, _startCell.col));

            while (rcs.Count > 0)
            {
                Tuple<int, int> curCell = (Tuple<int, int>)rcs.Pop();

                int r = curCell.Item1;
                int c = curCell.Item2;

                if (!isValid(r, c))
                    continue;

                _currentCell = _gridSpace.area[r][c];

                if (_gridSpace.area[r][c].isGoal)
                {
                    _reachedEnd = true;
                    break;
                }

                _gridSpace.area[r][c].visited = true;
                _moveCount++;
                Console.WriteLine("(" + r + "," + c + ")");

                for(int i = 0; i < 4; i++)
                {
                    int adjx = r + _rd[i];
                    int adjy = c + _cd[i];
                    rcs.Push(new Tuple<int, int>(adjx, adjy));
                }
            }

            if (_reachedEnd)
                return _moveCount;
            return -1;
        }

        public int aStarTreetraversal()
        {



            return -1;
        }

        
        public cell currentcell
        {
            get { return _currentCell; }
        }
    }
}
