using System;

using System.Drawing;
using System.Drawing.Imaging;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using Auxiliary.Graphics;
using Auxiliary.MathTools;
using Auxiliary.Raytracing;
using Tao.OpenGl;

namespace SceneEditor
{
	/// <summary> Главное окно приложения. </summary>
    public partial class MainForm : Form
    {
        #region Private Fields
        
        /// <summary> Управляет камерой при помощи мыши. </summary>
        private Mouse mouse = new Mouse();
		
		/// <summary> Управляет камерой при помощи клавиатуры. </summary>
		private Keyboard keyboard = new Keyboard();
		
		/// <summary> Камера для 'съемки' сцены. </summary>
		private Camera camera = new Camera(new Vector3D(29.0f, 11.0f, 11.0f),
		                                   new Vector3D(0.0f, 0.36f, -0.38f));
		
		/// <summary> Положения источников света. </summary>
		private Vector4D[] lights = new Vector4D[] { new Vector4D(5.0f, 5.0f, 5.0f, 1.0f),
													 new Vector4D(-5.0f, -5.0f, 5.0f, 1.0f) };
		
		/// <summary> Текущее время для управления источниками света. </summary>
		private float time = 0.0f;
		
		private int[] texture = new int[1];
		
        #endregion
        
        #region Constructor
        
        /// <summary> Создает главное окно приложения. </summary>
        public MainForm()
        {
            InitializeComponent();
        }        
        
        #endregion
        
        #region Private Methods

        #region Настройка визуализации
        
        private bool LoadGLTextures() {
            bool status = false;                                                // Status Indicator
            Bitmap[] textureImage = new Bitmap[1];                              // Create Storage Space For The Texture

            textureImage[0] = new Bitmap("Tile - 3.png");                // Load The Bitmap
            // Check For Errors, If Bitmap's Not Found, Quit
            if(textureImage[0] != null) {
                status = true;                                                  // Set The Status To True

                Gl.glGenTextures(1, texture);                            // Create The Texture

                textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
                // Rectangle For Locking The Bitmap In Memory
                Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
                // Get The Bitmap's Pixel Data From The Locked Bitmap
                BitmapData bitmapData = textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                // Typical Texture Generation Using Data From The Bitmap
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

                if(textureImage[0] != null) {                                   // If Texture Exists
                    textureImage[0].UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
                    textureImage[0].Dispose();                                  // Dispose The Bitmap
                }
            }

            return status;                                                      // Return The Status
        }        
                
        /// <summary> Настраивает параметры состояния API OpenGL. </summary>
        private void SetupOpenGL()
        {
        	Gl.glEnable(Gl.GL_COLOR_MATERIAL);
        	
        	Gl.glShadeModel(Gl.GL_SMOOTH);
        	
        	Gl.glEnable(Gl.GL_DEPTH_TEST);
        	
        	Gl.glEnable(Gl.GL_LIGHTING);
        	
        	Gl.glEnable(Gl.GL_POINT_SMOOTH);
        	
        	Gl.glEnable(Gl.GL_TEXTURE_2D);
        	
        	Gl.glPointSize(8.0f);  
        	
        	LoadGLTextures();
        }           
        
        /// <summary> Настраивает источники света. </summary>
        private void SetupLights()
        {
        	Gl.glEnable(Gl.GL_LIGHT0);        	
        	
        	Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, new float[] { 0.1f, 0.1f, 0.1f });
        	Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, new float[] { 0.5f, 0.2f, 0.2f });        	
        	Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[] { 0.8f, 0.8f, 0.8f });
        	
        	Gl.glEnable(Gl.GL_LIGHT1);        	
        	
        	Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, new float[] { 0.1f, 0.1f, 0.1f });
        	Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, new float[] { 0.2f, 0.5f, 0.2f });        	
        	Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPECULAR, new float[] { 0.8f, 0.8f, 0.8f });
        }
        
        /// <summary> Перемещает истоники всета по сцене. </summary>
        private void MoveLights()
        {
        	lights[0].X = (float) (5.0 * Math.Sin(time / 10.0));
        	lights[0].Y = (float) (5.0 * Math.Cos(time / 10.0));
        	lights[0].Z = 5.0f;
        	
        	Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lights[0].ToArray());
        	
        	lights[1].X = (float) (5.0 * Math.Cos(time / 20.0));
        	lights[1].Y = (float) (5.0 * Math.Sin(time / 20.0));
        	lights[1].Z = (float) (-2.0 + Math.Cos(time / 5.0));
        	
        	Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, lights[1].ToArray());  
        }
        
        #endregion
        
        #region Визуализация сцены
        
        /// <summary> Отрисовывает пол комнаты. </summary>
        private void DrawBottom()
        {
        	Gl.glPushMatrix();
        	
        	Gl.glTranslatef(0.0f, 0.0f, -5.0f);
        	
        	Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
        	
        	Gl.glBegin(Gl.GL_QUADS);
        	
        	Gl.glColor3f(1.0f, 1.0f, 1.0f);
				
			Gl.glNormal3f(0.0f, 0.0f, 1.0f);
			
					Gl.glTexCoord2f(0, 0); Gl.glVertex2f(-8, -8);

					Gl.glTexCoord2f(0, 1); Gl.glVertex2f(-8, 8);

					Gl.glTexCoord2f(1, 1); Gl.glVertex2f(8, 8);
					
					Gl.glTexCoord2f(1, 0);Gl.glVertex2f(8, -8);
			
			Gl.glEnd();
			
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
			
			Gl.glPopMatrix();
        }
        
        /// <summary> Рисует гирлянду из сфер. </summary>
        private void DrawSpheres()
        {
        	Glu.GLUquadric quad = Glu.gluNewQuadric();
        	
        	for (int i = 0; i < 50; i++)
        	{
        		Vector3D position =
        			new Vector3D((float) ((2.0 + 3.0 * i / 50.0) * Math.Sin(i)),
        			             (float) ((2.0 + 3.0 * i / 50.0) * Math.Cos(i)),
        			             (float) (-4.0 + 9.0* i / 50.0));
        		
        		Gl.glPushMatrix();
	        	
        		Gl.glTranslatef(position.X * (float) (Math.Sin(time / 100.0)),
        		                position.Y * (float) (Math.Cos(time / 100.0)),
        		                position.Z * (float) (0.5 + 0.5 * Math.Sin(time / 100.0)));
        		                
        		Gl.glColor3fv(Vector3D.Abs(position / 5.0f).ToArray());
        		                
	        	Glu.gluSphere(quad, 0.3f, 20, 20);        		
	        	
	        	Gl.glPopMatrix();
        	}
        	
        	Glu.gluDeleteQuadric(quad);
        }

        private void DrawCylinders()
        {
        	Glu.GLUquadric quad = Glu.gluNewQuadric();
        	
        	for (int i = 1; i < 12; i++)
        	{
        		Vector3D position =
        			new Vector3D((float) (6.5 * Math.Sin(i * time / 50.0)),
        			             (float) (6.5 * Math.Cos(i * time / 50.0)),
        			             -5.0f);
        		
        		Gl.glPushMatrix();
	        	
        		Gl.glTranslatef(position.X, position.Y, position.Z);
        		                
        		Gl.glColor3fv(Vector3D.Abs(Vector3D.Sin(time * position / 100.0f)).ToArray());
        		                
	        	Glu.gluCylinder(quad, 0.5f, 0.0f, 1.5f, 20, 20);        		
	        	
	        	Gl.glPopMatrix();
        	}
        	
        	Glu.gluDeleteQuadric(quad);        	
        }
         
        /// <summary> Отрисовывает источники света. </summary>
        private void DrawLights()
        {
			Gl.glDisable(Gl.GL_LIGHTING);
			
			Gl.glBegin(Gl.GL_POINTS);
			{
				Gl.glColor3fv(new float[] { 0.5f, 0.2f, 0.2f });
			
				Gl.glVertex3fv(lights[0].ToArray());
				
				Gl.glColor3fv(new float[] { 0.2f, 0.5f, 0.2f });
			
				Gl.glVertex3fv(lights[1].ToArray());
			}
			Gl.glEnd();
			
			Gl.glEnable(Gl.GL_LIGHTING);
        }
        
        /// <summary> Отрисовывает сцену в выбранном режиме. </summary>
        private void DrawScene()
        {        
        	{
		        mouse.Apply(camera);
		        	
		        keyboard.Apply(camera);
		        	            
		        camera.Setup();
        	}
        	
	        Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
	        	
	        {
	        	DrawBottom();
	        	
	        	DrawSpheres();
	        	
	        	DrawCylinders();
	        	
	        	DrawLights();
	        }
	        	
	        {
		        labelPosition.Text = "Положение: " + camera.Position.ToString();
		        	
		        labelOrientation.Text = "Ориентация: " + camera.Orientation.ToString();
	        }
        } 
 
        #endregion
        
        #endregion
        
        #region Event Handlers
        
        private void MainFormLoad(object sender, EventArgs e)
        {
        	panelOpenGL.InitializeContexts();
        	
        	SetupOpenGL();
        	
        	SetupLights();
        }
        
        private void PanelOpenGLPaint(object sender, PaintEventArgs e)
        {
        	DrawScene();
        }
        
        private void PanelOpenGLKeyDown(object sender, KeyEventArgs e)
        {
        	keyboard.OnKeyDown(e);
        }
        
        private void PanelOpenGLKeyUp(object sender, KeyEventArgs e)
        {
        	keyboard.OnKeyUp(e);
        }
        
        private void PanelOpenGLMouseDown(object sender, MouseEventArgs e)
        {
        	mouse.OnMouseDown(e);
        	
        	labelMouseActive.Visible = mouse.Active;
        }
        
        private void PanelOpenGLMouseMove(object sender, MouseEventArgs e)
        {
        	mouse.OnMouseMove(e);
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
        	time++;
        	
        	MoveLights();
        	
        	panelOpenGL.Invalidate();
        }
        
        #endregion       
    }
}
 
