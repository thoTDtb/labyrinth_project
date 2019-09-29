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

        /// <summary>
        /// Steps taken from start
        /// </summary>
        int steps_taken = 0;

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

                    //else if (path_c == 'C') line += ".";
                    else if (c == '*') line += "*";
                    else if (c == '.') line += ".";

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
        /// Copies a char matrix and returns it
        /// </summary>
        /// <param name="matrix">The matrix to copy</param>
        /// <returns>The copied matrix, with a different pointer</returns>
        private char[,] CopyMatrix(char[,] matrix)
        {
            char[,] new_matrix = new char[matrix.GetLength(0), matrix.GetLength(1)];
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    new_matrix[y, x] = matrix[y, x];
                }
            }

            return new_matrix;
        }

        /// <summary>
        /// Copies an int[] list and returns it
        /// </summary>
        /// <param name="list">The list to copy</param>
        /// <returns>The copied list, with a different pointer</returns>
        private List<int[]> CopyList(List<int[]> list)
        {
            List<int[]> new_list = new List<int[]>();
            foreach (var item in list)
            {
                new_list.Add(item);
            }

            return new_list;
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

            Stack<int[]> junction_stack = new Stack<int[]>(); // path stack

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

                    // If we change direction or come to a junction, add a checkpoint
                    if (dir != prev_dir || possible_paths.Count > 1)
                    {
                        path_to_checkpoint.Add(new int[] { y, x });

                        Checkpoint cpoint = new Checkpoint();

                        cpoint.coordinates = new int[] { y, x };
                        cpoint.path = CopyList(path_to_checkpoint);

                        checkpoints.Add(cpoint);
                    }

                    prev_dir = dir;

                    // If there are more than one routes, add it to the stack
                    if (possible_paths.Count > 1)
                    {
                        junction_stack.Push(new int[] { y, x });
                    }

                    y = random_choice[0];
                    x = random_choice[1];

                }
                else
                {
                    path_matrix[y, x] = 'P';

                    // Jump back to the previous saved junction
                    if (junction_stack.Count > 0)
                    {
                        int[] last = junction_stack.Pop();
                        y = last[0];
                        x = last[1];

                        // Find the checkpoint associated and set it as the current path
                        Checkpoint cpoint = checkpoints.Find(c => c.coordinates[0] == y && c.coordinates[1] == x);
                        path_to_checkpoint = CopyList(cpoint.path);
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

            ClearPathFinding();
            foreach (var cpoint in path_to_checkpoint)
            {
                // Remove checkpoint from player's position
                if (cpoint[0] == player_pos[0] && cpoint[1] == player_pos[1])
                    path_matrix[cpoint[0], cpoint[1]] = (char)0;
                else
                    path_matrix[cpoint[0], cpoint[1]] = 'C';
            }

            // Put checkpoint on exit
            path_matrix[exit[0], exit[1]] = 'C';
        }

        /// <summary>
        /// Does pathfinding and updates the listbox
        /// </summary>
        private void UpdateGame()
        {
            PathFinding(map_matrix, exit, player_pos[0], player_pos[1]);
            DisplayMaze(lB_map);
            l_steps_taken.Text = steps_taken.ToString();
            l_steps_req.Text = GetSteps().ToString();
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

            if (py != player_pos[0] || px != player_pos[1])
                steps_taken++;
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
        private void CheckFinish(int[] player_pos, int[] exit)
        {
            if (player_pos[0] == exit[0] && player_pos[1] == exit[1])
            {
                p_menu.Enabled = true;
                p_menu.Visible = true;
                b_start.Enabled = false;
                
                timer_auto.Enabled = false;

                DialogResult result = MessageBox.Show("Congratulations!\nYou reached the exit!", "Finish!", MessageBoxButtons.OK);

                Reset_Game(exit, player_pos, player_dir);

            }
        }

        /// <summary>
        /// Returns the amount of steps required to the exit
        /// </summary>
        /// <returns>Amount of steps required to the exit</returns>
        private int GetSteps()
        {
            int steps = 0;

            int c_y = player_pos[0];
            int c_x = player_pos[1];

            char[,] new_path_matrix = CopyMatrix(path_matrix);
            int[] next_cpoint;

            while (true)
            {
                next_cpoint = LookForCheckpoint(new int[] { c_y, c_x }, map_matrix, new_path_matrix);

                if (next_cpoint[0] == -1 || next_cpoint[1] == -1)
                    break;

                if (next_cpoint[0] != c_y)
                    steps += Math.Abs(next_cpoint[0] - c_y);
                else if (next_cpoint[1] != c_x)
                    steps += Math.Abs(next_cpoint[1] - c_x);

                new_path_matrix[next_cpoint[0], next_cpoint[1]] = (char)0;

                c_y = next_cpoint[0];
                c_x = next_cpoint[1];
            }

            return steps;
        }

        /// <summary>
        /// Makes the player step towards the exit automatically
        /// </summary>
        private void StepPlayerToExit()
        {
            int p_y = player_pos[0];
            int p_x = player_pos[1];
            char new_player_dir = 'V';

            // Look for a checkpoint and turn that way
            int[] cpoint_next = LookForCheckpoint(player_pos, map_matrix, path_matrix);

            // Failed to find checkpoint
            if (cpoint_next[0] == -1 || cpoint_next[1] == -1)
                return;
            
            if (p_y < cpoint_next[0])
            {
                new_player_dir = 'V';
            }
            else if (p_y > cpoint_next[0])
            {
                new_player_dir = '^';
            }
            else if (p_x > cpoint_next[1])
            {
                new_player_dir = '<';
            }
            else if (p_x < cpoint_next[1])
            {
                new_player_dir = '>';
            }

            // We first turn, then actually step when we're in the right direction
            if (new_player_dir == player_dir)
                StepForward();
            else
                player_dir = new_player_dir;

            CheckFinish(player_pos, exit);
        }

        /// <summary>
        /// Checks in every direction to find a checkpoint (until it hits a wall)
        /// </summary>
        /// <param name="pos">Position to start from</param>
        /// <returns>checkpoint's position</returns>
        private int[] LookForCheckpoint(int[] pos, char[,] map, char[,] path_map)
        {
            int p_y = pos[0];
            int p_x = pos[1];

            // Right
            for (int x = p_x; x < map_x; x ++)
            {
                if (map[p_y, x] == '*')
                    break;

                if (path_map[p_y, x] == 'C')
                {
                    return new int[] { p_y, x };
                }
            }

            // Left
            for (int x = p_x; x >= 0; x--)
            {
                if (map[p_y, x] == '*')
                    break;

                if (path_map[p_y, x] == 'C')
                {
                    return new int[] { p_y, x };
                }
            }

            // Up
            for (int y = p_y; y >= 0; y--)
            {
                if (map[y, p_x] == '*')
                    break;

                if (path_map[y, p_x] == 'C')
                {
                    return new int[] { y, p_x };
                }
            }

            // Down
            for (int y = p_y; y < map_y; y++)
            {
                if (map[y, p_x] == '*')
                    break;

                if (path_map[y, p_x] == 'C')
                {
                    return new int[] { y, p_x };
                }
            }

            return new int[] { -1, -1 };
        }

        /// <summary>
        /// Resets the program to its starting phase
        /// </summary>
        /// <param name="exit_pos">Exit's loaction</param>
        /// <param name="player_position">Player's position</param>
        /// <param name="player_direction">Player's direction</param>
        private void Reset_Game(int[] exit_pos, int[] player_position, char player_direction)
        {
            p_menu.Enabled = true;
            p_menu.Visible = true;
            b_start.Enabled = false;
            ClearMaze();
            ClearPathFinding();
            player_position[0] = 1;
            player_position[1] = 1;
            player_direction = 'V';
            exit_pos = new int[] { 0, 0 };
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
            CheckFinish(player_pos, exit);
        }

        //Exits the program
        private void b_exit_Click(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }

        //Re-enables the menu panel
        private void b_back_to_menu_Click(object sender, EventArgs e)
        {
            Reset_Game(exit, player_pos, player_dir);
        }

        /// <summary>
        /// Enables auto timer
        /// </summary>
        private void b_step_auto_Click(object sender, EventArgs e)
        {
            if (!timer_auto.Enabled)
            {
                timer_auto.Enabled = true;
                b_left.Enabled = false;
                b_right.Enabled = false;
                b_step.Enabled = false;
            }
            else
            {
                timer_auto.Enabled = false;
                b_left.Enabled = true;
                b_right.Enabled = true;
                b_step.Enabled = true;
            }
        }

        /// <summary>
        /// Gets called every 500 miliseconds
        /// </summary>
        private void timer_auto_Tick(object sender, EventArgs e)
        {
            StepPlayerToExit();
            UpdateGame();
        }

        #endregion
    }
}
