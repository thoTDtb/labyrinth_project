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

        char[,] map_matrix = new char[9, 9];

        char player_dir = 'U';

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
                        if (player_dir == 'U')
                            build_line.Append('⟰');
                        if (player_dir == 'R')
                            build_line.Append('⭆');
                        if (player_dir == 'D')
                            build_line.Append('⟱');
                        if (player_dir == 'L')
                            build_line.Append('⭅');
                    }

                    else if (c == '*')
                        build_line.Append("▮");

                    else if (c == '.')
                        build_line.Append("▯");
                        
                }
                lB_map.Items.Add(build_line);
            }
        }


        //Select and existing file to be used
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
                case 'U':
                    player_dir = 'R';
                    break;
                case 'R':
                    player_dir = 'D';
                    break;
                case 'D':
                    player_dir = 'L';
                    break;
                case 'L':
                    player_dir = 'U';
                    break;
            }

            DisplayLabyrinth();
        }

        private void b_left_Click(object sender, EventArgs e)
        {
            // Turn left
            switch (player_dir)
            {
                case 'U':
                    player_dir = 'L';
                    break;
                case 'L':
                    player_dir = 'D';
                    break;
                case 'D':
                    player_dir = 'R';
                    break;
                case 'R':
                    player_dir = 'U';
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
                case 'U':
                    if (py != 0)
                    {
                        if (map_matrix[py - 1, px] == '.')
                            player_pos[0]--;
                    }
                    break;

                case 'R':
                    if (px != map_matrix.GetLength(1) - 1)
                    {
                        if (map_matrix[py, px + 1] == '.')
                            player_pos[1]++;
                    }
                    break;

                case 'D':
                    if (py != map_matrix.GetLength(0) - 1)
                    {
                        if (map_matrix[py + 1, px] == '.')
                            player_pos[0]++;
                    }
                    break;

                case 'L':
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
