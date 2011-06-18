using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeSolver
{
    public interface IMazeBuilder
    {
        Cell[,] Build(int width, int height);
    }
}
