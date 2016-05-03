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
		        private float xsh = 30, ysh = -30f, zsh = 1f,xc = 30,yc = -35,zc = 4,u, rd = 2.5f,ty = 1f,tx = 1f,rad = 1.5839f,cx,cy,cz, yu, size = 50,s,vz,vy,az,t;
       
		/// <summary> Камера для 'съемки' сцены. </summary>
                private Camera camera = new Camera(new Vector3D(30f, -37f, 2.86f),
                                                   new Vector3D(0.00f, -0.1f, 45.49f));
		
		/// <summary> Положения источников света. </summary>
		private Vector4D[] lights = new Vector4D[] { new Vector4D(5.0f, 5.0f, 5.0f, 1.0f),
													 new Vector4D(-5.0f, -5.0f, 5.0f, 1.0f) };
		
		/// <summary> Текущее время для управления источниками света. </summary>
		private float time = 0.0f;
        private bool flag1=false,flag2=false,flag3=false,flag4=false,b3=false,b4=false;
        private float startTime=0;
        private float ang = 30f;
        private int score = 0;
        private bool pol = false;
		private int[] texture = new int[7];
		
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
        
                                                  
        private bool LoadGLTextures(){
            bool status = false;                                                // Status Indicator
            Bitmap[] textureImage = new Bitmap[7];                             // Create Storage Space For The Texture
            textureImage[0] = new Bitmap("Tile6.jpg");                // Load The Bitmap
            textureImage[1] = new Bitmap("Tile - 8.jpg");
            textureImage[2] = new Bitmap("Tile - 7.jpg");
            textureImage[3] = new Bitmap("Tile - 10.jpg");
            textureImage[4] = new Bitmap("Tile - 4.jpg");
            textureImage[5] = new Bitmap("Tile - 9.png");
            textureImage[6] = new Bitmap("Tile62.jpg");
            // Check For Errors, If Bitmap's Not Found, Quit
            Gl.glGenTextures(6, texture);
            for (int i = 0; i < 7; i++)
            {
                if ((textureImage[i] != null))
                {
                    status = true;                                                 // Set The Status To True

                    // Create The Texture
                    //texture for floor
                    textureImage[i].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
                    // Rectangle For Locking The Bitmap In Memory
                    Rectangle rectangle = new Rectangle(0, 0, textureImage[i].Width, textureImage[i].Height);
                    // Get The Bitmap's Pixel Data From The Locked Bitmap
                    BitmapData bitmapData = textureImage[i].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    // Typical Texture Generation Using Data From The Bitmap
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[i]);
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[i].Width, textureImage[i].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

                    if ((textureImage[i] != null))
                    {                                   // If Texture Exists
                        textureImage[i].UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
                        textureImage[i].Dispose();                                  // Dispose The Bitmap
                    }

                }
            }
            return status;                                                      // Return The Status
        }       
                
        
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
        
        
        private void SetupLights()
        {
        	Gl.glEnable(Gl.GL_LIGHT0);        	
        	
        	Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, new float[] { 0.0f, 0.0f, 0.0f });
        	Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, new float[] { 0.8f, 0.8f, 0.8f });        	
        	Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[] { 0.0f, 0.0f, 0.0f });
            lights[0].X = 0f;
            lights[0].Y = 0f;
            lights[0].Z = size;

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lights[0].ToArray());       	        	
        }
        
        #endregion
        
        #region Визуализация сцены
        
        /// <summary> Отрисовывает комнату. </summary>
        private void DrawBottom()
        {
            //пол
            Gl.glPushMatrix();
                Gl.glTranslatef(0.0f, 0.0f, 0.0f);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[1]);
                Gl.glBegin(Gl.GL_QUADS);
                    Gl.glColor3f(255, 255, 255);

             // комната
                        // нижняя грань
                    Gl.glTexCoord2f(0, 0);
                    Gl.glVertex3f(-size, -size, 0f);
                    Gl.glTexCoord2f(1, 0);
                    Gl.glVertex3f(size, -size, 0f);
                    Gl.glTexCoord2f(1, 1);
                    Gl.glVertex3f(size, size, 0f);
                    Gl.glTexCoord2f(0, 1);
                    Gl.glVertex3f(-size, size, 0f);
                    Gl.glEnd();
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
                    Gl.glPopMatrix();

                        //стены
                    Gl.glPushMatrix();
                    Gl.glTranslatef(0.0f, 0.0f, 0.0f);
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glColor3f(255, 255, 255);
                        
                        // левая грань    
                    Gl.glTexCoord2f(0, 0);
                    Gl.glVertex3f(-size, -size, -0);
                    Gl.glTexCoord2f(1, 0);
                    Gl.glVertex3f(-size, size, -0);
                    Gl.glTexCoord2f(1, 1);
                    Gl.glVertex3f(-size, size, size);
                    Gl.glTexCoord2f(0, 1);
                    Gl.glVertex3f(-size, -size, size);
                    
                        // правая грань
                    Gl.glTexCoord2f(1, 0);
                    Gl.glVertex3f(size, -size, -0);
                    Gl.glTexCoord2f(1, 1);
                    Gl.glVertex3f(size, -size, size);
                    Gl.glTexCoord2f(0, 1);
                    Gl.glVertex3f(size, size, size);
                    Gl.glTexCoord2f(0, 0);
                    Gl.glVertex3f(size, size, -0);                               

                        // передняя грань           
                    Gl.glPushMatrix();
                    Gl.glTranslatef(0.0f, 0.0f, 0.0f);
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glColor3f(255, 255, 255);
                    Gl.glTexCoord2f(1, 0);
                    Gl.glVertex3f(-size, size, -0);
                    Gl.glTexCoord2f(1, 1);
                    Gl.glVertex3f(-size, size, size);
                    Gl.glTexCoord2f(0, 1);
                    Gl.glVertex3f(size, size, size);
                    Gl.glTexCoord2f(0, 0);
                    Gl.glVertex3f(size, size, -0);            
                    Gl.glEnd();
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
                    Gl.glPopMatrix();

                        //задняя грань
                    Gl.glPushMatrix();
                    Gl.glTranslatef(0.0f, 0.0f, 0.0f);
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[6]);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glColor3f(255, 255, 255);
                    Gl.glTexCoord2f(1, 0);
                    Gl.glVertex3f(-size, -size, -0);
                    Gl.glTexCoord2f(1, 1);
                    Gl.glVertex3f(-size, -size, size);
                    Gl.glTexCoord2f(0, 1);
                    Gl.glVertex3f(size, -size, size);
                    Gl.glTexCoord2f(0, 0);
                    Gl.glVertex3f(size, -size, -0);
                    Gl.glEnd();
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
                    Gl.glPopMatrix();

                //потолок
                    Gl.glPushMatrix();
                    Gl.glTranslatef(0.0f, 0.0f, 0.0f);
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[2]);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glColor3f(255, 255, 255);
                            
                    Gl.glTexCoord2f(0, 0);
                    Gl.glVertex3f(-size, -size, size);
                    Gl.glTexCoord2f(1, 0);
                    Gl.glVertex3f(size, -size, size);
                    Gl.glTexCoord2f(1, 1);
                    Gl.glVertex3f(size, size, size);
                    Gl.glTexCoord2f(0, 1);
                    Gl.glVertex3f(-size, size, size);
                    Gl.glEnd();
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
                    Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

        // бортики   
      
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(35, -35, -0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(35, -35, size/20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(25, -35, size/20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(25, -35, -0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);
                   
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(35 , -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(35 , -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(35, 25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(35, 25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);
                    
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(25, -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(25, -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(25, 10, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(25, 10, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);
                     
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(25, 10, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(25, 10, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-15, 10, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-15, 10, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();


            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);
                    
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(35, 25, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(35, 25, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-25, 25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-25, 25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();


            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);
                     
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-15, 10, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-15, 10, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-15, -35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-15, -35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);
                    
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-25, 25, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-25, 25, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-25, -35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-25, -35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);
                  
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-15, -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-15, -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-25, -35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-25, -35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Glu.GLUquadric quad = Glu.gluNewQuadric();
            Gl.glPushMatrix();
            Gl.glTranslatef(-20f, -30f, 0.1f );
            Gl.glRotated(180, 1, 0, 0);
            Gl.glColor3f(0f, 0f, 0f);
            Glu.gluQuadricTexture(quad, Gl.GL_TRUE);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Glu.gluDisk(quad, 0f, 1.0, 75, 75);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();
            Glu.gluDeleteQuadric(quad);
        }
                 
        private void DrawALL()
        {

            Glu.GLUquadric quad2 = Glu.gluNewQuadric();
                Gl.glPushMatrix();
                Gl.glTranslatef(xsh, ysh, zsh);
                Gl.glColor3f(1.0f, 1.0f, 1.0f);
                Glu.gluQuadricTexture(quad2, Gl.GL_TRUE);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[4]);
                Glu.gluSphere(quad2, 1f, 50, 50);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
                Gl.glPopMatrix();
            Glu.gluDeleteQuadric(quad2);			

            Gl.glDisable(Gl.GL_LIGHTING);
			    Gl.glBegin(Gl.GL_POINTS);
			    {
				    Gl.glColor3fv(new float[] { 1f, 1f, 1f });			
				    Gl.glVertex3fv(lights[0].ToArray());				
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
	        	DrawALL();
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
            panelOpenGL.Invalidate();
        	SetupOpenGL();        	
        	SetupLights();
        }
        private void MoveSphere()
        {
             t = (time - startTime) * 0.01f;
             xsh = xsh + tx*(cx * 0.25f);
             ysh = ysh + ty*(cy * 0.25f);
             xc =xc + tx*(cx * 0.25f);
             yc =yc + ty*(cy * 0.25f);
             cx = cx*0.98f;
             cy = cy*0.98f;
             if ((cx < 0.01)&&(cx > -0.01)) {
                 cx = 0;
             }
             if ((cy < 0.01)&&(cy > -0.01))
             {
                 cy = 0;
             }
        }
        private void PanelOpenGLPaint(object sender, PaintEventArgs e)
        {
            if ((cx != 0) || (cy != 0))
            {
                camera = new Camera(new Vector3D(xsh, ysh - 10f, 30), new Vector3D(0.00f, 1.2f, 1.5839f));
            }
            else
            {
                camera = new Camera(new Vector3D(xc, yc, zc), new Vector3D(0.00f, 0.2f, rad));
            }
            if ((cx == 0) && (cy == 0)) {
                button1.Enabled = true;
            }
            if ((xsh < 26) && (xsh > 20) && (ysh < 10) && (ysh > -35)) {
                tx = tx*-1f;
            }
            if ((xsh > 34) && (ysh < 24) && (ysh > -35))
            {
                tx = tx * -1f;
            }
            if ((xsh > -16) && (xsh < -10) && (ysh < 10) && (ysh > -35))
            {
                tx = tx * -1f;
            }
            if ((xsh < -24) && (ysh < 24) && (ysh > -35))
            {
                tx = tx * -1f;
            }
            if ((ysh < -34) && (xsh < 35) && (xsh > 25))
            {
                ty = ty * -1f;
            }
            if ((ysh < -34) && (xsh < -15) && (xsh > -25))
            {
                ty = ty * -1f;
            }
            if ((ysh > 24) && (xsh > -25) && (xsh < 35))
            {
                ty = ty * -1f;
            }
            if ((ysh < 11)&& (ysh > 10) && (xsh < 25) && (xsh > -15))
            {
                ty = ty * -1f;
            }
            if ((xsh < -19) && (xsh > -21) && (ysh > -31) && (ysh < -29))
            {

                xsh = 30;
                ysh = -30f;
                xc = 30;
                yc = -35;
                rad = 1.5839f;
                cx = 0;
                cy = 0;
                label6.Text = "Excellent! Try again";
                label7.Text = "Number of strokes:" + Convert.ToString(Convert.ToInt32(yu));
                yu = 0;
            }
            DrawScene();
            if (pol)
            {
                MoveSphere();
            }
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
            panelOpenGL.Invalidate();                   
        }
        
        #endregion       

        private void panelOpenGL_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yu++;
            label6.Text = "";
            label7.Text = "";
            tx = 1f;
            ty = 1f;
            cx = xsh - xc;       
            cy = ysh - yc;
           pol = true;
           startTime = time;
           button1.Enabled = false;
           if ((xsh < 35) && (xsh > 25) && (ysh < 10) && (ysh > -35))
           {
               xc = xsh;
               yc = ysh - 5f;
               rad = 1.5839f;
           }
           if ((xsh < 35) && (xsh > -15) && (ysh < 25) && (ysh > 10))
           {
               xc = xsh + 5f;
               yc = ysh;
               rad = 0f;
           }
           if ((xsh < -15) && (xsh > -25) && (ysh < 25) && (ysh > -35))
           {
               xc = xsh;
               yc = ysh + 5f;
               rad = -1.5839f;
           }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //button3.Enabled = false;
            //b3 = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            b4 = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            b4 = false;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if ((xsh == xc) && (ysh > yc)) {
                xc += 0.5f;
                yc += 0.5f;
                rad -= 0.15839f;
                return;
            }
            if ((xsh < xc) && (ysh > yc))
            {
                xc += 0.5f;
                yc += 0.5f;
                rad -= 0.15839f;
                return;
            }
            if ((xsh < xc) && (ysh == yc))
            {
                xc -= 0.5f;
                yc += 0.5f;
                rad -= 0.15839f;
                return;
            }
            if ((xsh < xc) && (ysh < yc))
            {
                xc -= 0.5f;
                yc += 0.5f;
                rad -= 0.15839f;
                return;
            }
            if ((xsh == xc) && (ysh < yc))
            {
                xc -= 0.5f;
                yc -= 0.5f;
                rad -= 0.15839f;
                return;
            }
            if ((xsh > xc) && (ysh < yc))
            {
                xc -= 0.5f;
                yc -= 0.5f;
                rad -= 0.15839f;
                return;
            }
            if ((xsh > xc) && (ysh == yc))
            {
                xc += 0.5f;
                yc -= 0.5f;
                rad -= 0.15839f;
                return;
            }
            if ((xsh > xc) && (ysh > yc))
            {
                xc += 0.5f;
                yc -= 0.5f;
                rad -= 0.15839f;
                return;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if ((xsh == xc) && (ysh > yc))
            {
                xc -= 0.5f;
                yc += 0.5f;
                rad += 0.15839f;
                return;
            }
            if ((xsh < xc) && (ysh > yc))
            {
                xc -= 0.5f;
                yc -= 0.5f;
                rad += 0.15839f;
                return;
            }
            if ((xsh < xc) && (ysh == yc))
            {
                xc -= 0.5f;
                yc -= 0.5f;
                rad += 0.15839f;
                return;
            }
            if ((xsh < xc) && (ysh < yc))
            {
                xc += 0.5f;
                yc -= 0.5f;
                rad += 0.15839f;
                return;
            }
            if ((xsh == xc) && (ysh < yc))
            {
                xc += 0.5f;
                yc -= 0.5f;
                rad += 0.15839f;
                return;
            }
            if ((xsh > xc) && (ysh < yc))
            {
                xc += 0.5f;
                yc += 0.5f;
                rad += 0.15839f;
                return;
            }
            if ((xsh > xc) && (ysh == yc))
            {
                xc += 0.5f;
                yc += 0.5f;
                rad += 0.15839f;
                return;
            }
            if ((xsh > xc) && (ysh > yc))
            {
                xc -= 0.5f;
                yc += 0.5f;
                rad += 0.15839f;
                return;
            }
        }

        private void panelOpenGL1_Load(object sender, EventArgs e)
        {

        }
    }
}
 
