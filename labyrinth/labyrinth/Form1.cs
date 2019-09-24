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

namespace labyrinth
{
    public partial class Form1 : Form
    {
        Random r = new Random();

        char[,] map_matrix = new char[9, 9];

        char player_dir = 'V';

        int[] player_pos = new int[2];

        public Form1()
        {
            InitializeComponent();

            player_pos[0] = 1;
            player_pos[1] = 1;
            
            label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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

                    else if (c == '*')
                        build_line.Append("▮");

                    else if (c == '.')
                        build_line.Append("▯");
                        
                }
                lB_map.Items.Add(build_line);
            }
        }

        /// <summary>
        /// Generates a 9x9 maze
        /// </summary>
        private void GenerateMaze()
        {
            int max_x = 7; // 7 because walls on 2 sides shouldn't be included
            int max_y = 7; // ^^^
            int c_x = 0;
            int c_y = 0;

            List<int[]> stack = new List<int[]>();

            bool[,] cells = new bool[max_x, max_y]; // False: Empty, True: Wall
            bool[,] visited = new bool[max_x, max_y]; // False: Not visited, True: Visited

            bool done = false;

            while (!done)
            {
                List<int[]> unvisited = new List<int[]>(); // We will randomly select an unvisited cell

                // Check neighbours
                if (c_y != 0)
                {
                    // Up
                    if (!visited[c_y - 1, c_x])
                        unvisited.Add(new int[] { c_y - 1, c_x });
                }
                else if (c_x != 0)
                {
                    // Left
                    if (!visited[c_y, c_x - 1])
                        unvisited.Add(new int[] { c_y, c_x - 1});
                }
                else if (c_y != max_y-1)
                {
                    // Down
                    if (!visited[c_y + 1, c_x])
                        unvisited.Add(new int[] { c_y + 1, c_x });
                }
                else if (c_x != max_x - 1)
                {
                    // Right
                    if (!visited[c_y, c_x + 1])
                        unvisited.Add(new int[] { c_y, c_x + 1 });
                }
                
                if (unvisited.Count != 0) // There are unvisited neighbours
                {
                    stack.Add(new int[] { c_y, c_x }); // Add current cell to stack so we can come back later.

                    // Jump to one of the unvisited cells
                    int[] random_unvisited = unvisited[r.Next(unvisited.Count)];

                    c_y = random_unvisited[0];
                    c_x = random_unvisited[1];
                }
                else
                {
                    // Remove current cell from stack if it's there
                    stack.Remove(stack.Find(cell => cell[0] == c_y && cell[1] == c_x));

                    if (stack.Count == 0)
                        done = true;
                    else
                    {
                        // Take the last item from stack, jump to it
                        c_y = stack.Last()[0];
                        c_x = stack.Last()[1];
                    }
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
    }
}
