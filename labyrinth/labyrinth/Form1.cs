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

        public Form1()
        {
            InitializeComponent();
            
            label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        }


        //Displays the labyrinth into the listbox, lB_map
        private void DisplayLabyrinth()
        {
            for (int i = 0; i < 9; i++)
            {
                StringBuilder build_line = new StringBuilder();
                for (int j = 0; j < 9; j++)
                {
                    build_line.Append(map_matrix[i, j]);
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
            map_matrix[1, 1] = 'V';
            DisplayLabyrinth();
        }
    }
}
