namespace SceneEditor
{
    partial class MainForm
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
        	this.panelView.Size = new System.Drawing.Size(516, 541);
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
        	this.panelOpenGL.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.panelOpenGL.Location = new System.Drawing.Point(0, 25);
        	this.panelOpenGL.Name = "panelOpenGL";
        	this.panelOpenGL.Size = new System.Drawing.Size(512, 512);
        	this.panelOpenGL.StencilBits = ((byte)(0));
        	this.panelOpenGL.TabIndex = 8;
        	this.panelOpenGL.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelOpenGLPaint);
        	this.panelOpenGL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelOpenGLMouseMove);
        	this.panelOpenGL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PanelOpenGLKeyUp);
        	this.panelOpenGL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelOpenGLMouseDown);
        	this.panelOpenGL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PanelOpenGLKeyDown);
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
        	this.toolStripViewport.Size = new System.Drawing.Size(512, 25);
        	this.toolStripViewport.TabIndex = 4;
        	// 
        	// labelViewport
        	// 
        	this.labelViewport.Name = "labelViewport";
        	this.labelViewport.Size = new System.Drawing.Size(89, 22);
        	this.labelViewport.Text = "Окно просмотра";
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
        	this.timer.Interval = 20;
        	this.timer.Tick += new System.EventHandler(this.TimerTick);
        	// 
        	// panelStatus
        	// 
        	this.panelStatus.Controls.Add(this.labelOrientation);
        	this.panelStatus.Controls.Add(this.labelPosition);
        	this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.panelStatus.Location = new System.Drawing.Point(0, 566);
        	this.panelStatus.Name = "panelStatus";
        	this.panelStatus.Size = new System.Drawing.Size(540, 21);
        	this.panelStatus.TabIndex = 13;
        	// 
        	// labelOrientation
        	// 
        	this.labelOrientation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        	this.labelOrientation.Dock = System.Windows.Forms.DockStyle.Left;
        	this.labelOrientation.Location = new System.Drawing.Point(180, 0);
        	this.labelOrientation.Name = "labelOrientation";
        	this.labelOrientation.Size = new System.Drawing.Size(180, 21);
        	this.labelOrientation.TabIndex = 4;
        	this.labelOrientation.Text = "---";
        	this.labelOrientation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        	// 
        	// labelPosition
        	// 
        	this.labelPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        	this.labelPosition.Dock = System.Windows.Forms.DockStyle.Left;
        	this.labelPosition.Location = new System.Drawing.Point(0, 0);
        	this.labelPosition.Name = "labelPosition";
        	this.labelPosition.Size = new System.Drawing.Size(180, 21);
        	this.labelPosition.TabIndex = 3;
        	this.labelPosition.Text = "---";
        	this.labelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        	// 
        	// MainForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.SystemColors.Control;
        	this.ClientSize = new System.Drawing.Size(540, 587);
        	this.Controls.Add(this.panelStatus);
        	this.Controls.Add(this.panelView);
        	this.ForeColor = System.Drawing.SystemColors.ControlText;
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        	this.MaximizeBox = false;
        	this.Name = "MainForm";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Raytracing Tutorial";
        	this.Load += new System.EventHandler(this.MainFormLoad);
        	this.panelView.ResumeLayout(false);
        	this.panelView.PerformLayout();
        	this.toolStripViewport.ResumeLayout(false);
        	this.toolStripViewport.PerformLayout();
        	this.panelStatus.ResumeLayout(false);
        	this.ResumeLayout(false);
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



    }
}

