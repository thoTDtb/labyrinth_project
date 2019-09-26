﻿namespace labyrinth
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lB_map = new System.Windows.Forms.ListBox();
            this.b_left = new System.Windows.Forms.Button();
            this.b_step = new System.Windows.Forms.Button();
            this.b_right = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.b_step_auto = new System.Windows.Forms.Button();
            this.l_steps_req = new System.Windows.Forms.Label();
            this.l_steps_taken = new System.Windows.Forms.Label();
            this.p_menu = new System.Windows.Forms.Panel();
            this.b_exit = new System.Windows.Forms.Button();
            this.b_start = new System.Windows.Forms.Button();
            this.b_gen_new = new System.Windows.Forms.Button();
            this.b_read_file = new System.Windows.Forms.Button();
            this.p_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lB_map
            // 
            this.lB_map.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lB_map.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lB_map.FormattingEnabled = true;
            this.lB_map.ItemHeight = 41;
            this.lB_map.Location = new System.Drawing.Point(16, 15);
            this.lB_map.Margin = new System.Windows.Forms.Padding(4);
            this.lB_map.Name = "lB_map";
            this.lB_map.Size = new System.Drawing.Size(763, 373);
            this.lB_map.TabIndex = 0;
            // 
            // b_left
            // 
            this.b_left.Location = new System.Drawing.Point(440, 576);
            this.b_left.Margin = new System.Windows.Forms.Padding(4);
            this.b_left.Name = "b_left";
            this.b_left.Size = new System.Drawing.Size(67, 62);
            this.b_left.TabIndex = 1;
            this.b_left.Text = "Turn Left";
            this.b_left.UseVisualStyleBackColor = true;
            this.b_left.Click += new System.EventHandler(this.b_left_Click);
            // 
            // b_step
            // 
            this.b_step.Location = new System.Drawing.Point(515, 507);
            this.b_step.Margin = new System.Windows.Forms.Padding(4);
            this.b_step.Name = "b_step";
            this.b_step.Size = new System.Drawing.Size(67, 62);
            this.b_step.TabIndex = 2;
            this.b_step.Text = "Take a Step";
            this.b_step.UseVisualStyleBackColor = true;
            this.b_step.Click += new System.EventHandler(this.b_step_Click);
            // 
            // b_right
            // 
            this.b_right.Location = new System.Drawing.Point(589, 576);
            this.b_right.Margin = new System.Windows.Forms.Padding(4);
            this.b_right.Name = "b_right";
            this.b_right.Size = new System.Drawing.Size(67, 62);
            this.b_right.TabIndex = 3;
            this.b_right.Text = "Turn Right";
            this.b_right.UseVisualStyleBackColor = true;
            this.b_right.Click += new System.EventHandler(this.b_right_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(181, 400);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 28);
            this.label1.TabIndex = 4;
            this.label1.Text = "Steps Remaining:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 400);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 28);
            this.label2.TabIndex = 5;
            this.label2.Text = "Steps Taken:";
            // 
            // b_step_auto
            // 
            this.b_step_auto.Location = new System.Drawing.Point(515, 576);
            this.b_step_auto.Margin = new System.Windows.Forms.Padding(4);
            this.b_step_auto.Name = "b_step_auto";
            this.b_step_auto.Size = new System.Drawing.Size(67, 62);
            this.b_step_auto.TabIndex = 6;
            this.b_step_auto.Text = "Auto";
            this.b_step_auto.UseVisualStyleBackColor = true;
            // 
            // l_steps_req
            // 
            this.l_steps_req.Location = new System.Drawing.Point(305, 400);
            this.l_steps_req.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.l_steps_req.Name = "l_steps_req";
            this.l_steps_req.Size = new System.Drawing.Size(53, 28);
            this.l_steps_req.TabIndex = 7;
            this.l_steps_req.Text = "0";
            // 
            // l_steps_taken
            // 
            this.l_steps_taken.Location = new System.Drawing.Point(120, 400);
            this.l_steps_taken.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.l_steps_taken.Name = "l_steps_taken";
            this.l_steps_taken.Size = new System.Drawing.Size(53, 28);
            this.l_steps_taken.TabIndex = 8;
            this.l_steps_taken.Text = "0";
            // 
            // p_menu
            // 
            this.p_menu.Controls.Add(this.b_exit);
            this.p_menu.Controls.Add(this.b_start);
            this.p_menu.Controls.Add(this.b_gen_new);
            this.p_menu.Controls.Add(this.b_read_file);
            this.p_menu.Location = new System.Drawing.Point(16, 446);
            this.p_menu.Margin = new System.Windows.Forms.Padding(4);
            this.p_menu.Name = "p_menu";
            this.p_menu.Size = new System.Drawing.Size(255, 353);
            this.p_menu.TabIndex = 9;
            // 
            // b_exit
            // 
            this.b_exit.Location = new System.Drawing.Point(29, 315);
            this.b_exit.Margin = new System.Windows.Forms.Padding(4);
            this.b_exit.Name = "b_exit";
            this.b_exit.Size = new System.Drawing.Size(200, 28);
            this.b_exit.TabIndex = 3;
            this.b_exit.Text = "Exit";
            this.b_exit.UseVisualStyleBackColor = true;
            this.b_exit.Click += new System.EventHandler(this.b_exit_Click);
            // 
            // b_start
            // 
            this.b_start.Enabled = false;
            this.b_start.Location = new System.Drawing.Point(29, 130);
            this.b_start.Margin = new System.Windows.Forms.Padding(4);
            this.b_start.Name = "b_start";
            this.b_start.Size = new System.Drawing.Size(200, 28);
            this.b_start.TabIndex = 2;
            this.b_start.Text = "Start";
            this.b_start.UseVisualStyleBackColor = true;
            this.b_start.Click += new System.EventHandler(this.b_start_Click);
            // 
            // b_gen_new
            // 
            this.b_gen_new.Location = new System.Drawing.Point(29, 68);
            this.b_gen_new.Margin = new System.Windows.Forms.Padding(4);
            this.b_gen_new.Name = "b_gen_new";
            this.b_gen_new.Size = new System.Drawing.Size(200, 28);
            this.b_gen_new.TabIndex = 1;
            this.b_gen_new.Text = "Generate New Labyrinth";
            this.b_gen_new.UseVisualStyleBackColor = true;
            this.b_gen_new.Click += new System.EventHandler(this.b_gen_new_Click);
            // 
            // b_read_file
            // 
            this.b_read_file.Location = new System.Drawing.Point(29, 11);
            this.b_read_file.Margin = new System.Windows.Forms.Padding(4);
            this.b_read_file.Name = "b_read_file";
            this.b_read_file.Size = new System.Drawing.Size(200, 28);
            this.b_read_file.TabIndex = 0;
            this.b_read_file.Text = "Use already existing file";
            this.b_read_file.UseVisualStyleBackColor = true;
            this.b_read_file.Click += new System.EventHandler(this.b_read_file_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 814);
            this.Controls.Add(this.p_menu);
            this.Controls.Add(this.l_steps_taken);
            this.Controls.Add(this.l_steps_req);
            this.Controls.Add(this.b_step_auto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_right);
            this.Controls.Add(this.lB_map);
            this.Controls.Add(this.b_left);
            this.Controls.Add(this.b_step);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.p_menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lB_map;
        private System.Windows.Forms.Button b_left;
        private System.Windows.Forms.Button b_step;
        private System.Windows.Forms.Button b_right;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button b_step_auto;
        private System.Windows.Forms.Label l_steps_req;
        private System.Windows.Forms.Label l_steps_taken;
        private System.Windows.Forms.Panel p_menu;
        private System.Windows.Forms.Button b_exit;
        private System.Windows.Forms.Button b_start;
        private System.Windows.Forms.Button b_gen_new;
        private System.Windows.Forms.Button b_read_file;
    }
}
