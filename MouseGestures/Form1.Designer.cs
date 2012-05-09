namespace MouseGestures
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
            this.controls = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.eventList = new System.Windows.Forms.ListBox();
            this.gestureSurface = new System.Windows.Forms.Panel();
            this.controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // controls
            // 
            this.controls.Controls.Add(this.button1);
            this.controls.Controls.Add(this.saveButton);
            this.controls.Controls.Add(this.eventList);
            this.controls.Dock = System.Windows.Forms.DockStyle.Left;
            this.controls.Location = new System.Drawing.Point(0, 0);
            this.controls.Name = "controls";
            this.controls.Size = new System.Drawing.Size(158, 470);
            this.controls.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(38, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Load Gesture";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(39, 89);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(101, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save Gesture";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // eventList
            // 
            this.eventList.FormattingEnabled = true;
            this.eventList.Location = new System.Drawing.Point(16, 117);
            this.eventList.Name = "eventList";
            this.eventList.Size = new System.Drawing.Size(124, 173);
            this.eventList.TabIndex = 0;
            // 
            // gestureSurface
            // 
            this.gestureSurface.BackColor = System.Drawing.Color.White;
            this.gestureSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gestureSurface.Location = new System.Drawing.Point(158, 0);
            this.gestureSurface.Name = "gestureSurface";
            this.gestureSurface.Size = new System.Drawing.Size(476, 470);
            this.gestureSurface.TabIndex = 1;
            this.gestureSurface.Click += new System.EventHandler(this.gestureSurface_Click);
            this.gestureSurface.Paint += new System.Windows.Forms.PaintEventHandler(this.gestureSurface_Paint);
            this.gestureSurface.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gestureSurface_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 470);
            this.Controls.Add(this.gestureSurface);
            this.Controls.Add(this.controls);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.controls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel controls;
        private System.Windows.Forms.Panel gestureSurface;
        private System.Windows.Forms.ListBox eventList;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button button1;
    }
}

