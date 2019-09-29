using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace labyrinth
{
    /// <summary>
    /// Checkpoint for turns, used for pathfinding.
    /// </summary>
    struct Checkpoint
    {
        /// <summary>
        /// Checkpoint's location
        /// </summary>
        public int[] coordinates;

        /// <summary>
        /// Path to this checkpoint's location, consists of coordinates
        /// </summary>
        public List<int[]> path;
    }

    public partial class Form1 : Form
    {
        Random r = new Random();
        
        // Map size
        int map_x = 9;
        int map_y = 9;

        /// <summary>
        /// Map data
        /// </summary>
        char[,] map_matrix = new char[9, 9];

        /// <summary>
        /// Pathfinding data
        /// </summary>
        char[,] path_matrix = new char[9, 9];

        /// <summary>
        /// Player's current Y and X coordinates
        /// </summary>
        int[] player_pos = new int[2];

        /// <summary>
        /// Exit's coordinates
        /// </summary>
        int[] exit = new int[2];

        /// <summary>
        /// Current player direction
        /// </summary>
        char player_dir = 'V';

        public Form1()
        {
            InitializeComponent();

            player_pos[0] = 1;
            player_pos[1] = 1;
            
            label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            lB_map.Font = new Font(FontFamily.GenericMonospace, lB_map.Font.Size);
        }


        //Displays the maze into the desired listbox
        private void DisplayMaze(ListBox lb)
        {
            lb.Items.Clear();
            for (int y = 0; y < 9; y++)
            {
                string line = "";
                for (int x = 0; x < 9; x++)
                {
                    char c = map_matrix[y, x];
                    char path_c = path_matrix[y, x];

                    // Draw player if current y,x is player's current pos
                    if (y == player_pos[0] && x == player_pos[1])
                        line += player_dir;

                    else if (path_c == 'C') line += "c";
                    else if (c == 'P') line += " ";
                    else if (c == '*') line += "*";
                    else if (c == '.') line += " ";

                }
                lb.Items.Add(line);
            }
        }

        /// <summary>
        /// Clears the map data, thus resetting the maze.
        /// </summary>
        private void ClearMaze()
        {
            map_matrix = new char[9, 9];

            for (int y = 0; y < map_matrix.GetLength(0); y++)
            {
                for (int x = 0; x < map_matrix.GetLength(1); x++)
                {
                    map_matrix[y, x] = '*';
                }
            }

            ClearPathFinding();
        }

        /// <summary>
        /// Clears pathfinding data
        /// </summary>
        private void ClearPathFinding()
        {
            path_matrix = new char[9, 9];
        }

        /// <summary>
        /// Generates a 9x9 maze
        /// </summary>
        /// <param name="max_x">x size of the map</param>
        /// <param name="max_y">y size of the map</param>
        private void GenerateMaze(int max_x, int max_y)
        {
            ClearMaze();
            
            int c_x = 1; // Current x position
            int c_y = 1; // Current y position

            bool exit_added = false; // Make sure we have an exit placed.
            exit = new int[2]; // The exit's coordinates

            Stack<int[]> junction_stack = new Stack<int[]>();
            
            bool[,] visited = new bool[max_x, max_y]; // False: Not visited, True: Visited

            bool done = false;

            while (!done)
            {
                // Store the neighbouring unvisited cells for later
                List<int[]> unvisited = new List<int[]>();

                // Set the current cell as visited
                visited[c_y, c_x] = true;
                map_matrix[c_y, c_x] = '.';

                // Check neighbours
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
                        unvisited.Add(new int[] { c_y, c_x - 2});
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

                    if (old_y < c_y) // 5 5  7 5  = 6 5
                        map_matrix[old_y + 1, c_x] = '.';
                    else if (old_y > c_y)
                        map_matrix[old_y - 1, c_x] = '.';

                    else if (old_x < c_x)
                        map_matrix[c_y, old_x + 1] = '.';
                    else if (old_x > c_x)
                        map_matrix[c_y, old_x - 1] = '.';
                }
                else
                {
                    if (unvisited.Count == 0 && junction_stack.Count == 0)
                        done = true;

                    else if (unvisited.Count == 0)
                    {
                        if (!exit_added)
                        {
                            if (c_y + 1 == max_y - 1)
                                exit = new int[] { c_y + 1, c_x };

                            else if (c_x + 1 == max_x - 1)
                                exit = new int[] { c_y, c_x + 1 };

                            else if (c_y - 1 == 0)
                                exit = new int[] { c_y - 1, c_x };

                            else if (c_x - 1 == 0)
                                exit = new int[] { c_y, c_x - 1 };

                            if (exit[0] != 0 || exit[1] != 0)
                            {
                                map_matrix[exit[0], exit[1]] = '.';
                                exit_added = true;
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
        /// Copies an int[] array and returns it
        /// </summary>
        /// <param name="array">The array to copy</param>
        /// <returns>The copied array, with a different pointer</returns>
        private int[] CopyArray(int[] array)
        {
            int[] new_array = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                new_array[i] = array[i];
            }

            return new_array;
        }

        /// <summary>
        /// Attempts to find a path to the specified exit.
        /// </summary>
        /// <param name="map">Map matrix, where * are walls and . are paths</param>
        /// <param name="exit_pos">Exit's position</param>
        /// <param name="start_y">Starting Y coordiante</param>
        /// <param name="start_x">Starting X coordiante</param>
        private void PathFinding(char[,] map, int[] exit_pos, int start_y, int start_x)
        {
            ClearPathFinding();

            Stack<int[]> path = new Stack<int[]>(); // path stack

            List<int[]> path_to_checkpoint = new List<int[]>(); // Path to the current checkpoint
            List<Checkpoint> checkpoints = new List<Checkpoint>(); // Checkpoints, first is the coordinates, second is the path to it's position

            int y = start_y;
            int x = start_x;

            // Previous and current direction.
            int prev_dir = 0;
            int dir = 0;

            while (!(y == exit_pos[0] && x == exit_pos[1]))
            {
                List<int[]> possible_paths = new List<int[]>();

                // If the neighbours aren't walls and they weren't visited before, mark them as a possible path
                if (map[y + 1, x] == '.' && path_matrix[y + 1, x] == 0) possible_paths.Add(new int[] { y + 1, x, 1 });
                if (map[y, x + 1] == '.' && path_matrix[y, x + 1] == 0) possible_paths.Add(new int[] { y, x + 1, 2 });
                if (map[y - 1, x] == '.' && path_matrix[y - 1, x] == 0) possible_paths.Add(new int[] { y - 1, x, 3 });
                if (map[y, x - 1] == '.' && path_matrix[y, x - 1] == 0) possible_paths.Add(new int[] { y, x - 1, 4 });

                // Mark as visited
                path_matrix[y, x] = 'P';

                if (possible_paths.Count > 0)
                {
                    // Go in a random direction
                    int[] random_choice = possible_paths[r.Next(possible_paths.Count)];

                    // When we change the direction, place a checkpoint
                    dir = random_choice[2];

                    if (dir != prev_dir)
                    {
                        Checkpoint cpoint = new Checkpoint();

                        cpoint.coordinates = new int[] { y, x };
                        

                        /*current_path.Add(new int[] { y, x });

                        List<int[]> temp_path = new List<int[]>();

                        foreach (var checkpoint in current_path)
                        {
                            temp_path.Add(new int[] { checkpoint[0], checkpoint[1] });
                        }

                        checkpoints.Add(new int[,] { { y, x },  });*/
                        path_matrix[y, x] = 'C';
                    }

                    prev_dir = dir;

                    // If there are more than one routes, add it to the stack
                    if (possible_paths.Count > 1)
                    {
                        path.Push(new int[] { y, x });
                    }

                    y = random_choice[0];
                    x = random_choice[1];

                }
                else
                {
                    path_matrix[y, x] = 'P';

                    if (path.Count > 0)
                    {
                        l_steps_taken.Text = "OK";
                        int[] last = path.Pop();
                        y = last[0];
                        x = last[1];
                    }
                    else
                    {
                        // Pathfinding failed for unknown reasons.
                        break;
                        
                        ClearPathFinding();
                        y = 1;
                        x = 1;
                    }
                }
            }

            
        }

        /// <summary>
        /// Updates several game features
        /// </summary>
        private void UpdateGame()
        {
            PathFinding(map_matrix, exit, player_pos[0], player_pos[1]);
            DisplayMaze(lB_map);
        }

        /// <summary>
        /// Opens file dialog and loads the map from the file
        /// </summary>
        private void LoadFileIntoMap()
        {
            OpenFileDialog maze_file = new OpenFileDialog();

            if (maze_file.ShowDialog() == DialogResult.OK)
            {
                int index_y = 0;
                int index_x = 0;

                StreamReader fstream = new StreamReader(maze_file.FileName);
                while (!fstream.EndOfStream)
                {
                    string line = fstream.ReadLine();
                    index_x = 0;

                    foreach (char c in line) map_matrix[index_y, index_x++] = c;

                    index_y++;
                }
                fstream.Close();

                b_start.Enabled = true;
            }
        }

        /// <summary>
        /// Turns the player left
        /// </summary>
        private void TurnLeft()
        {
            // Turn left
            switch (player_dir)
            {
                case '^':
                    player_dir = '<';
                    break;
                case '<':
                    player_dir = 'V';
                    break;
                case 'V':
                    player_dir = '>';
                    break;
                case '>':
                    player_dir = '^';
                    break;
            }
        }

        /// <summary>
        /// Turns the player right
        /// </summary>
        private void TurnRight()
        {
            // Turn right
            switch (player_dir)
            {
                case '^':
                    player_dir = '>';
                    break;
                case '>':
                    player_dir = 'V';
                    break;
                case 'V':
                    player_dir = '<';
                    break;
                case '<':
                    player_dir = '^';
                    break;
            }
        }

        /// <summary>
        /// Steps the player forward
        /// </summary>
        private void StepForward()
        {
            int py = player_pos[0];
            int px = player_pos[1];

            switch (player_dir)
            {
                case '^':
                    if (py != 0)
                    {
                        if (map_matrix[py - 1, px] == '.')
                            player_pos[0]--;
                    }
                    break;

                case '>':
                    if (px != map_matrix.GetLength(1) - 1)
                    {
                        if (map_matrix[py, px + 1] == '.')
                            player_pos[1]++;
                    }
                    break;

                case 'V':
                    if (py != map_matrix.GetLength(0) - 1)
                    {
                        if (map_matrix[py + 1, px] == '.')
                            player_pos[0]++;
                    }
                    break;

                case '<':
                    if (px != 0)
                    {
                        if (map_matrix[py, px - 1] == '.')
                            player_pos[1]--;
                    }
                    break;
            }
        }

        /// <summary>
        /// Find the exit in the maze
        /// </summary>
        /// <param name="map">Map array, where * are walls and . are paths</param>
        /// <returns>Returns the position of the exit</returns>
        //This was necessary because the "PathFinding" method required the exit's coordinates which were 0, 0 by default so the program crashed 
        private int[] FindExit(char[,] map)
        {
            int[] exit_pos = new int[2];
            for (int y = 0; y < map_y; y++)
            {
                for (int x = 0; x < map_x; x++)
                {
                    if ((y == 0 || y == 8) && (map[y, x] == '.'))
                        exit_pos = new int[] { y, x };
                    else if ((x == 0 || x == 8) && (map[y, x] == '.'))
                        exit_pos = new int[] { y, x };
                }
            }

            return exit_pos;
        }

        /// <summary>
        /// Checks if the player is at the exit 
        /// </summary>
        /// <param name="player_pos">Player's current position</param>
        /// <param name="exit">Exit's position</param>
        private void Finish(int[] player_pos, int[] exit)
        {
            if (player_pos[0] == exit[0] && player_pos[1] == exit[1])
            {
                DialogResult result = MessageBox.Show("Congratulations!\nYou reached the exit!", "Finish!", MessageBoxButtons.OK);
                lB_map.Items.Clear();
                p_menu.Enabled = true;
                p_menu.Visible = true;
                b_start.Enabled = false;
                ClearMaze();
                ClearPathFinding();
                player_pos[0] = 1;
                player_pos[1] = 1;
                player_dir = 'V';
                exit = new int[] { 0, 0 };
            }
        }

        #region Form elements

        private void b_read_file_Click(object sender, EventArgs e)
        {
            LoadFileIntoMap();
            DisplayMaze(lB_map);
            exit = FindExit(map_matrix);
        }

        private void b_gen_new_Click(object sender, EventArgs e)
        {
            GenerateMaze(map_x, map_y);
            UpdateGame();
            b_start.Enabled = true;
        }

        //Starts the game
        private void b_start_Click(object sender, EventArgs e)
        {
            p_menu.Enabled = false;
            p_menu.Visible = false;
            DisplayMaze(lB_map);
        }

        private void b_right_Click(object sender, EventArgs e)
        {
            TurnRight();
            UpdateGame();
        }

        private void b_left_Click(object sender, EventArgs e)
        {
            TurnLeft();
            UpdateGame();
        }

        private void b_step_Click(object sender, EventArgs e)
        {
            StepForward();
            UpdateGame();
            Finish(player_pos, exit);
        }

        //Exits the program
        private void b_exit_Click(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }

        //Re-enables the menu panel
        private void b_back_to_menu_Click(object sender, EventArgs e)
        {
            p_menu.Enabled = true;
            p_menu.Visible = true;
            b_start.Enabled = false;
            ClearMaze();
            ClearPathFinding();
            player_pos[0] = 1;
            player_pos[1] = 1;
            player_dir = 'V';
            exit = new int[] { 0, 0 };
        }

        #endregion
    }
}
