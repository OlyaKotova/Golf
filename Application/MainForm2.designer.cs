namespace SceneEditor
{
    partial class MainForm2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm2));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelView = new System.Windows.Forms.Panel();
            this.panelOpenGL = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.toolStripViewport = new System.Windows.Forms.ToolStrip();
            this.labelViewport = new System.Windows.Forms.ToolStripLabel();
            this.labelMouseActive = new System.Windows.Forms.ToolStripLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panelStatus = new System.Windows.Forms.Panel();
            this.labelOrientation = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelView.SuspendLayout();
            this.toolStripViewport.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.White;
            this.imageList.Images.SetKeyName(0, "shere.bmp");
            this.imageList.Images.SetKeyName(1, "rect.bmp");
            this.imageList.Images.SetKeyName(2, "box.bmp");
            this.imageList.Images.SetKeyName(3, "surface.bmp");
            this.imageList.Images.SetKeyName(4, "light.bmp");
            // 
            // panelView
            // 
            this.panelView.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelView.Controls.Add(this.panelOpenGL);
            this.panelView.Controls.Add(this.toolStripViewport);
            this.panelView.Location = new System.Drawing.Point(12, 12);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(526, 478);
            this.panelView.TabIndex = 12;
            // 
            // panelOpenGL
            // 
            this.panelOpenGL.AccumBits = ((byte)(0));
            this.panelOpenGL.AutoCheckErrors = false;
            this.panelOpenGL.AutoFinish = false;
            this.panelOpenGL.AutoMakeCurrent = true;
            this.panelOpenGL.AutoSwapBuffers = true;
            this.panelOpenGL.BackColor = System.Drawing.Color.Black;
            this.panelOpenGL.ColorBits = ((byte)(32));
            this.panelOpenGL.DepthBits = ((byte)(16));
            this.panelOpenGL.Location = new System.Drawing.Point(-2, -2);
            this.panelOpenGL.Name = "panelOpenGL";
            this.panelOpenGL.Size = new System.Drawing.Size(526, 478);
            this.panelOpenGL.StencilBits = ((byte)(0));
            this.panelOpenGL.TabIndex = 8;
            this.panelOpenGL.Load += new System.EventHandler(this.panelOpenGL_Load);
            this.panelOpenGL.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelOpenGLPaint);
            this.panelOpenGL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PanelOpenGLKeyDown);
            this.panelOpenGL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PanelOpenGLKeyUp);
            this.panelOpenGL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelOpenGLMouseDown);
            this.panelOpenGL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelOpenGLMouseMove);
            // 
            // toolStripViewport
            // 
            this.toolStripViewport.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripViewport.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripViewport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelViewport,
            this.labelMouseActive});
            this.toolStripViewport.Location = new System.Drawing.Point(0, 0);
            this.toolStripViewport.Name = "toolStripViewport";
            this.toolStripViewport.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripViewport.Size = new System.Drawing.Size(522, 25);
            this.toolStripViewport.TabIndex = 4;
            this.toolStripViewport.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripViewport_ItemClicked);
            // 
            // labelViewport
            // 
            this.labelViewport.Name = "labelViewport";
            this.labelViewport.Size = new System.Drawing.Size(0, 22);
            // 
            // labelMouseActive
            // 
            this.labelMouseActive.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.labelMouseActive.ForeColor = System.Drawing.Color.Brown;
            this.labelMouseActive.Name = "labelMouseActive";
            this.labelMouseActive.Size = new System.Drawing.Size(23, 22);
            this.labelMouseActive.Text = "8";
            this.labelMouseActive.Visible = false;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.TimerTick);
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.labelOrientation);
            this.panelStatus.Controls.Add(this.labelPosition);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 496);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(912, 13);
            this.panelStatus.TabIndex = 13;
            // 
            // labelOrientation
            // 
            this.labelOrientation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelOrientation.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelOrientation.Location = new System.Drawing.Point(180, 0);
            this.labelOrientation.Name = "labelOrientation";
            this.labelOrientation.Size = new System.Drawing.Size(180, 13);
            this.labelOrientation.TabIndex = 4;
            this.labelOrientation.Text = "---";
            this.labelOrientation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelOrientation.Visible = false;
            // 
            // labelPosition
            // 
            this.labelPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPosition.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPosition.Location = new System.Drawing.Point(0, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(180, 13);
            this.labelPosition.TabIndex = 3;
            this.labelPosition.Text = "---";
            this.labelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPosition.Visible = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(633, 288);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 41);
            this.button1.TabIndex = 14;
            this.button1.Text = "Удар!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(800, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 34);
            this.button2.TabIndex = 18;
            this.button2.Text = "Выход";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(592, 228);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(66, 36);
            this.button4.TabIndex = 23;
            this.button4.Text = "<-";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(800, 228);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(66, 36);
            this.button5.TabIndex = 24;
            this.button5.Text = "->";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(643, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 23);
            this.label6.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(588, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 22);
            this.label7.TabIndex = 26;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(674, 363);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 31);
            this.label1.TabIndex = 27;
            this.label1.Text = "Игрок 1";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Courier New", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(668, 228);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(124, 36);
            this.button3.TabIndex = 28;
            this.button3.Text = "Ход соперника";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(783, 477);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(577, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 30;
            this.label3.Text = "Уровень 3 ";
            // 
            // MainForm2
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(912, 509);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelView);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Гольф";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.panelView.ResumeLayout(false);
            this.panelView.PerformLayout();
            this.toolStripViewport.ResumeLayout(false);
            this.toolStripViewport.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label labelOrientation;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.ToolStripLabel labelMouseActive;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripLabel labelViewport;
        private System.Windows.Forms.ToolStrip toolStripViewport;
        private Tao.Platform.Windows.SimpleOpenGlControl panelOpenGL;

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4; 
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;



    }
}

