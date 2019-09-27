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
