using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labyrinth.Classes
{
    /// <summary>
    /// Stores and generates mazes.
    /// </summary>
    class Maze
    {
        #region variables
        /* Private variables */
        /// <summary>
        /// Maze's map matrix
        /// </summary>
        private char[,] maze_matrix;

        /// <summary>
        /// Exit's coordinates
        /// </summary>
        private int[] exit;

        /// <summary>
        /// Random number generator
        /// </summary>
        private Random r;

        /* Private variables */
        /// <summary>
        /// X size of the maze
        /// </summary>
        public int max_x { get; set; }

        /// <summary>
        /// Y size of the maze
        /// </summary>
        public int max_y { get; set; }
        #endregion

        #region constructor

        /// <summary>
        /// Constructor of the Maze object
        /// </summary>
        /// <param name="size_y">Y size of the maze</param>
        /// <param name="size_x">X size of the maze</param>
        public Maze(int size_y, int size_x)
        {
            max_y = size_y;
            max_x = size_x;

            maze_matrix = new char[size_y, size_x];
        }

        #endregion

        #region functions

        /// <summary>
        /// Generates a maze
        /// </summary>
        public void Generate()
        {
            this.Clear();
            
            int c_y = 1, c_x = 1;       // Current position
            bool exit_made = false;     // Was an exit made?
            bool done = false;          // Is the generation done?

            // Store junctions in a stack
            Stack<int[]> junction_stack = new Stack<int[]>();

            // Make sure we know what cells we visited already
            bool[,] visited = new bool[max_x, max_y];

            // Store the neighbouring unvisited
            // cells for a random path choice
            List<int[]> unvisited = new List<int[]>();

            while (!done)
            {
                unvisited.Clear();

                visited[c_y, c_x] = true;
                maze_matrix[c_y, c_x] = '.';

                // Check neighbours and add them if they are unvisited
                if (c_y - 2 > 0)
                {
                    // Up
                    if (!visited[c_y - 2, c_x])
                        unvisited.Add(new int[] { c_y - 2, c_x });
                }
                if (c_x - 2 > 0)
                {
                    // Left
                    if (!visited[c_y, c_x - 2])
                        unvisited.Add(new int[] { c_y, c_x - 2 });
                }
                if (c_y != max_y - 2)
                {
                    // Down
                    if (!visited[c_y + 2, c_x])
                        unvisited.Add(new int[] { c_y + 2, c_x });
                }
                if (c_x != max_x - 2)
                {
                    // Right
                    if (!visited[c_y, c_x + 2])
                        unvisited.Add(new int[] { c_y, c_x + 2 });
                }

                if (unvisited.Count > 0) // There are unvisited neighbours
                {
                    if (unvisited.Count > 1)
                        junction_stack.Push(new int[] { c_y, c_x }); // Add current cell to stack so we can come back later.

                    // Jump to one of the random unvisited cells
                    int[] random_unvisited = unvisited[r.Next(unvisited.Count)];

                    int old_y = c_y;
                    int old_x = c_x;

                    c_y = random_unvisited[0];
                    c_x = random_unvisited[1];

                    // Remove wall between the two cells, very odd way of doing it but it works :)

                    // Vertical
                    if (old_y < c_y)
                        maze_matrix[old_y + 1, c_x] = '.';
                    else if (old_y > c_y)
                        maze_matrix[old_y - 1, c_x] = '.';

                    // Horizontal
                    else if (old_x < c_x)
                        maze_matrix[c_y, old_x + 1] = '.';
                    else if (old_x > c_x)
                        maze_matrix[c_y, old_x - 1] = '.';
                }
                else
                {
                    if (unvisited.Count == 0 && junction_stack.Count == 0)
                        done = true;

                    else if (unvisited.Count == 0)
                    {
                        if (!exit_made)
                        {
                            // Check for a possible exit
                            if (c_y + 1 == max_y - 1)
                                exit = new int[] { c_y + 1, c_x };

                            else if (c_x + 1 == max_x - 1)
                                exit = new int[] { c_y, c_x + 1 };

                            else if (c_y - 1 == 0)
                                exit = new int[] { c_y - 1, c_x };

                            else if (c_x - 1 == 0)
                                exit = new int[] { c_y, c_x - 1 };

                            // Make sure it's not on wrong coordinates
                            if (exit[0] != 0 || exit[1] != 0)
                            {
                                maze_matrix[exit[0], exit[1]] = '.';
                                exit_made = true;
                            }
                        }

                        // Pop the last item from stack, jump to it
                        int[] last_junction = junction_stack.Pop();

                        c_y = last_junction[0];
                        c_x = last_junction[1];
                    }
                }
            }
        }

        /// <summary>
        /// Fills the maze with walls, thus clearing it
        /// </summary>
        public void Clear()
        {

        }

        #endregion
    }
}
