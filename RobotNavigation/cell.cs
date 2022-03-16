using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class cell
    {
        private bool _isObastacle;
        private bool _isGoal;
        private int _row;
        private int _col;
        private bool _visited;


        public cell(int row, int col)
        {
            _row = row;
            _col = col;
            isGoal = false;
            isObstacle = false;
        }

        
        public bool isGoal
        {
            get { return _isGoal; }
            set { _isGoal = value; }
        }
        public bool isObstacle
        {
            get { return _isObastacle; }
            set { _isObastacle = value; }
        }
        public int row
        {
            get { return _row; }
        }
        public int col
        {
            get { return _col; }
        }
        public bool visited
        {
            get { return _visited; }
            set { _visited = value; }
        }
    }
}
