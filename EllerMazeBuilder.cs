using System;

namespace MazeSolver
{
    /*public class EllerMazeBuilder : IMazeBuilder
    {
        #region Implementation of IMazeBuilder

        public Cell[,] Build(int width, int height)
        {
            var rnd = new Random();
            var maze = new Cell[width,height];
            var rowSets = new int[width,height];

            int lastSet = 0;

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    maze[i, j] = new Cell(i, j);

            rowSets[0,0] = 
            for (int i = 0; i < width; i++ )
            {
                if (i > 0 && rnd.Next(2) == 0)
                {
                    maze[i - 1, 0].East = maze[i,0];
                    maze[i, 0].West = maze[i - 1, 0];

                }
                else
                {
                    lastSet++;
                }

                rowSets[i, 0] = lastSet++;

            }

            for (int j = 1; j < height - 1; j++)
            {

            }


            return maze;
        }

        #endregion
    }*/
}
