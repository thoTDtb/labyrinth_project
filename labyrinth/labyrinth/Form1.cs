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
    public partial class Form1 : Form
    {
        Random r = new Random();

        char[,] map_matrix = new char[9, 9];

        char player_dir = 'V';

        int[] player_pos = new int[2];

        int[] exit = new int[2];

        Stack<int[]> path = new Stack<int[]>();

        public Form1()
        {
            InitializeComponent();

            player_pos[0] = 1;
            player_pos[1] = 1;
            
            label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            lB_map.Font = new Font(FontFamily.GenericMonospace, lB_map.Font.Size);
        }


        //Displays the labyrinth into the listbox, lB_map
        private void DisplayLabyrinth()
        {
            lB_map.Items.Clear();
            for (int y = 0; y < 9; y++)
            {
                StringBuilder build_line = new StringBuilder();
                for (int x = 0; x < 9; x++)
                {
                    char c = map_matrix[y, x];



                    // Draw player if current y,x is player's current pos
                    if (y == player_pos[0] && x == player_pos[1])
                    {
                        build_line.Append(player_dir);
                    }

                    else if (c == 'C')
                        build_line.Append("C");

                    else if (c == 'P')
                        build_line.Append(".");

                    else if (c == '*')
                        build_line.Append("*");
                    //build_line.Append("▮");

                    else if (c == '.')
                        build_line.Append(".");
                    //build_line.Append("▯");

                }
                lB_map.Items.Add(build_line);
            }
        }

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

            DisplayLabyrinth();
        }

        /// <summary>
        /// Generates a 9x9 maze
        /// </summary>
        private void GenerateMaze()
        {
            ClearMaze();

            int max_x = 9;
            int max_y = 9;
            int c_x = 1;
            int c_y = 1;

            bool exit_added = false;

            List<int[]> stack = new List<int[]>();

            bool[,] cells = new bool[max_x, max_y]; // False: Empty, True: Wall
            bool[,] visited = new bool[max_x, max_y]; // False: Not visited, True: Visited

            bool done = false;

            while (!done)
            {
                List<int[]> unvisited = new List<int[]>(); // We will randomly select an unvisited cell

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
                        stack.Add(new int[] { c_y, c_x }); // Add current cell to stack so we can come back later.

                    // Jump to one of the unvisited cells
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
                    if (unvisited.Count == 0 && stack.Count == 0)
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

                            map_matrix[exit[0], exit[1]] = '.';

                            exit_added = true;

                        }

                        // Take the last item from stack, jump to it
                        c_y = stack.Last()[0];
                        c_x = stack.Last()[1];
                    }
                    
                    // Remove current cell from stack if it's in there
                    stack.Remove(stack.Find(cell => cell[0] == c_y && cell[1] == c_x));
                }

                if (c_x == max_x)
                    lB_map.Enabled = false;

            }

            // Done, update listbox
            PathFinding();
            DisplayLabyrinth();
        }

        private void PathFinding()
        {
            int y = player_pos[0];
            int x = player_pos[1];

            // Previous and current direction.
            int prev_dir = 0;
            int dir = 0;

            while (!(y == exit[0] && x == exit[1]))
            {
                List<int[]> possible_paths = new List<int[]>();

                if (map_matrix[y + 1, x] == '.')
                {
                    possible_paths.Add(new int[] { y + 1, x, 1 });
                }
                if (map_matrix[y, x + 1] == '.')
                {
                    possible_paths.Add(new int[] { y, x + 1, 2 });
                }
                if (map_matrix[y - 1, x] == '.')
                {
                    possible_paths.Add(new int[] { y - 1, x, 3 });
                }
                if (map_matrix[y, x - 1] == '.')
                {
                    possible_paths.Add(new int[] { y, x - 1, 4 });
                }

                map_matrix[y, x] = 'P';

                if (possible_paths.Count > 0)
                {
                    // When we change the direction, place a checkpoint
                    dir = possible_paths.First()[2];

                    if (dir != prev_dir)
                        map_matrix[y, x] = 'C';

                    prev_dir = dir;

                    // If there are more than one routes, add it to the stack
                    if (possible_paths.Count > 1)
                    {
                        path.Push(new int[] { y, x });
                    }

                    y = possible_paths.First()[0];
                    x = possible_paths.First()[1];

                }
                else
                {
                    map_matrix[y, x] = 'P';
                    int[] last = path.Pop();
                    y = last[0];
                    x = last[1];
                }
            }
        }


        //Select an existing file to be used
        private void b_read_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_labyrinth_file = new OpenFileDialog();
            if (open_labyrinth_file.ShowDialog() == DialogResult.OK)
            {
                int index_y = 0;
                StreamReader read_labyrinth_file = new StreamReader(open_labyrinth_file.FileName);
                while (!read_labyrinth_file.EndOfStream)
                {
                    int index_x = 0;
                    var temp_char_array = read_labyrinth_file.ReadLine().ToCharArray();
                    foreach (var item in temp_char_array)
                    {
                        map_matrix[index_y, index_x] = item;
                        index_x++;
                    }
                    index_y++;
                }
                read_labyrinth_file.Close();
            }
            b_start.Enabled = true;
            b_gen_new.Enabled = false;


            int max_y = map_matrix.GetLength(0);
            int max_x = map_matrix.GetLength(1);

            for (int y = 0; y < max_y; y++)
            {
                if (map_matrix[y, 0] == '.')
                {
                    exit = new int[] { y, 0 };
                    break;
                }

                if (map_matrix[y, max_x-1] == '.')
                {
                    exit = new int[] { y, max_x-1 };
                    break;
                }
            }

            for (int x = 0; x < max_x; x++)
            {
                if (map_matrix[0, x] == '.')
                {
                    exit = new int[] { 0, x };
                    break;
                }

                if (map_matrix[max_y - 1, x] == '.')
                {
                    exit = new int[] { max_y - 1, x };
                    break;
                }
            }
        }

        //Exits the program
        private void b_exit_Click(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }


        //Starts the game
        private void b_start_Click(object sender, EventArgs e)
        {
            p_menu.Enabled = false;
            p_menu.Visible = false;
            PathFinding();
            DisplayLabyrinth();
        }

        private void b_right_Click(object sender, EventArgs e)
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

            DisplayLabyrinth();
        }

        private void b_left_Click(object sender, EventArgs e)
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

            DisplayLabyrinth();
        }

        private void b_step_Click(object sender, EventArgs e)
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

            DisplayLabyrinth();
        }

        private void b_gen_new_Click(object sender, EventArgs e)
        {
            GenerateMaze();
        }
    }
}
