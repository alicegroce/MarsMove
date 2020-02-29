﻿namespace MarsMove
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
            this.glControl1 = new OpenTK.GLControl();
            this.bStartStop = new System.Windows.Forms.Button();
            this.lRadius = new System.Windows.Forms.Label();
            this.lSpeed = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(584, 561);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // bStartStop
            // 
            this.bStartStop.Location = new System.Drawing.Point(250, 520);
            this.bStartStop.Name = "bStartStop";
            this.bStartStop.Size = new System.Drawing.Size(75, 23);
            this.bStartStop.TabIndex = 1;
            this.bStartStop.Text = "Стоп";
            this.bStartStop.UseVisualStyleBackColor = true;
            this.bStartStop.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bStartStop_MouseClick);
            // 
            // lRadius
            // 
            this.lRadius.AutoSize = true;
            this.lRadius.Location = new System.Drawing.Point(15, 15);
            this.lRadius.Name = "lRadius";
            this.lRadius.Size = new System.Drawing.Size(78, 13);
            this.lRadius.TabIndex = 2;
            this.lRadius.Text = "Радиус (км) = ";
            // 
            // lSpeed
            // 
            this.lSpeed.AutoSize = true;
            this.lSpeed.Location = new System.Drawing.Point(15, 28);
            this.lSpeed.Name = "lSpeed";
            this.lSpeed.Size = new System.Drawing.Size(101, 13);
            this.lSpeed.TabIndex = 3;
            this.lSpeed.Text = "Скорость (км/с) = ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.Controls.Add(this.lSpeed);
            this.Controls.Add(this.lRadius);
            this.Controls.Add(this.bStartStop);
            this.Controls.Add(this.glControl1);
            this.MaximumSize = new System.Drawing.Size(600, 600);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "Form1";
            this.Text = "Движение марса";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button bStartStop;
        private System.Windows.Forms.Label lRadius;
        private System.Windows.Forms.Label lSpeed;
    }
}

