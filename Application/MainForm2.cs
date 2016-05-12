using System;

using System.Text;
using System.Net;
using System.Net.Sockets;

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

    public partial class MainForm2 : Form
    {
        #region Private Fields

        /// <summary> Управляет камерой при помощи мыши. </summary>
        private Mouse mouse = new Mouse();

        /// <summary> Управляет камерой при помощи клавиатуры. </summary>
        private Keyboard keyboard = new Keyboard();
        private float xsh = 5, ysh = 0, zsh = 1f,  // начальное положение мяча
                        xc = 5, yc = -5, zc = 4,      // начальное положение камеры
                        rd = 2.5f, ty = 1f, tx = 1f,
                        rad = 1.5839f, cx, cy, cz,
                        size = 50, s, vz, vy, az, t;
        float yu, yu1;
        int nomer,xod;
        float ysh1, xsh1, xc1, yc1;
        string fkob;
        float flag,cx1,cy1,rad1;


        /// <summary> Камера для 'съемки' сцены. </summary>
        private Camera camera = new Camera(new Vector3D(30f, -37f, 2.86f),
                                           new Vector3D(0.00f, -0.1f, 45.49f));

        /// <summary> Положения источников света. </summary>
        private Vector4D[] lights = new Vector4D[] { new Vector4D(5.0f, 5.0f, 5.0f, 1.0f),
													 new Vector4D(-5.0f, -5.0f, 5.0f, 1.0f) };

        /// <summary> Текущее время для управления источниками света. </summary>
        private float time = 0.0f;
        private bool flag1 = false, flag2 = false, flag3 = false, flag4 = false, b3 = false, b4 = false;
        private float startTime = 0;
        private float ang = 30f;
        private int score = 0;
        private bool pol = false;
        private int[] texture = new int[7];

        #endregion

        #region Constructor

        /// <summary> Создает главное окно приложения. </summary>
        public MainForm2()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        #region Настройка визуализации


        private bool LoadGLTextures()
        {
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
            Gl.glVertex3f(10, -25, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(10, -25, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(35, -25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(35, -25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(35, -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(35, -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(35, -25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(35, -25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(35, -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(35, -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(0, -35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(0, -35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(0, -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(0, -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(0, -10, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(0, -10, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(10, -10, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(10, -10, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(10, -25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(10, -25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();


            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(10, -10, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(10, -10, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(30, -10, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(30, -10, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();


            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(30, -10, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(30, -10, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(30, 20, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(30, 20, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(30, 20, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(30, 20, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(20, 20, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(20, 20, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(20, 20, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(20, 20, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(20, 0, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(20, 0, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(10, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(10, 0, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(20, 0, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(20, 0, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(10, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(10, 0, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(10, 25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(10, 25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(10, 25, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(10, 25, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(30, 25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(30, 25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(30, 25, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(30, 25, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(30, 35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(30, 35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(30, 35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(30, 35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-35, 35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-35, 35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-35, 35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-35, 35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-35, 15, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-35, 15, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-35, 15, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-35, 15, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-20, 15, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-20, 15, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-20, 15, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-20, 15, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-20, 25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-20, 25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-20, 25, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-20, 25, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(0, 25, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(0, 25, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(0, 25, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(0, 25, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(0, 0, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(0, 0, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-20, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-20, 0, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(0, 0, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(0, 0, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-20, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-20, 0, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-20, -35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-20, -35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-20, -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-20, -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-10, -35, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-10, -35, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-10, -35, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-10, -35, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(-10, -10, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(-10, -10, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, 0.0f, 0.0f);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[3]);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(255, 255, 255);

            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3f(-10, -10, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3f(-10, -10, size / 20);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3f(0, -10, size / 20);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3f(0, -10, 0);
            Gl.glEnd();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glPopMatrix();

            Glu.GLUquadric quad = Glu.gluNewQuadric();
            Gl.glPushMatrix();
            Gl.glTranslatef(-30f, 22f, 0.1f);
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
            Glu.gluSphere(quad2, 1f, 10, 10);
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
            xsh = xsh + tx * (cx * 0.25f);
            ysh = ysh + ty * (cy * 0.25f);
            xc = xc + tx * (cx * 0.25f);
            yc = yc + ty * (cy * 0.25f);
            cx = cx * 0.96f;
            cy = cy * 0.96f;
            if ((cx < 0.01) && (cx > -0.01))
            {
                cx = 0;
            }
            if ((cy < 0.01) && (cy > -0.01))
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
            if ((cx == 0) && (cy == 0))
            {
                if (flag2 == true)
                {
                    try
                    {
                        SendMessageFromSocket(11000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        Console.ReadLine();
                    }
                    flag2 = false;
                }
                button1.Enabled = true;
            }
           
            if ((xsh < 40) && (xsh > 34) && (ysh < -25) && (ysh > -35)) //1
            {
                tx = tx * -1f;
            }
            if ((xsh < 35) && (xsh > 0) && (ysh < -34) && (ysh > -40))
            {
                ty = ty * -1f;
            }
            if ((xsh < 35) && (xsh > 10) && (ysh < -15) && (ysh > -26))
            {
                ty = ty * -1f;
            }
            if ((xsh < 1) && (xsh > -5) && (ysh < -10) && (ysh > -35))
            {
                tx = tx * -1f;
            }
            if ((xsh < 25) && (xsh > 9) && (ysh < -10) && (ysh > -25)) //5
            {
                tx = tx * -1f;
            }
            if ((xsh < 30) && (xsh > 10) && (ysh < -9) && (ysh > -15))//6
            {
                ty = ty * -1f;
            }
            if ((xsh < 40) && (xsh >30) && (ysh < 20) && (ysh > -10))//7
            {
                tx = tx * -1f;
            }
            if ((xsh < 30) && (xsh > 20) && (ysh < 21) && (ysh > 19))//8
            {
                ty = ty * -1f;
            }
            if ((xsh < 21) && (xsh > 20) && (ysh < 20) && (ysh > 0))//9
            {
                tx = tx * -1f;
            }
            if ((xsh < 20) && (xsh > 10) && (ysh < 2) && (ysh > 0))//10
            {
                ty = ty * -1f;
            }
            if ((xsh < 12) && (xsh > 9) && (ysh < 25) && (ysh > 0))//11
            {
                tx = tx * -1f;
            }
            if ((xsh < 30) && (xsh > 10) && (ysh < 26) && (ysh > 24))//12
            {
                ty = ty * -1f;
            }

            if ((xsh < 35) && (xsh > 29) && (ysh < 35) && (ysh > 25))//13
            {
                tx = tx * -1f;
            }

            if ((xsh < 35) && (xsh > -35) && (ysh < 40) && (ysh > 34))//14

            {
                ty = ty * -1f;
            }
            if ((xsh < -34) && (xsh > -40) && (ysh < 35) && (ysh > 15))//15
            {
                tx = tx * -1f;
            }
            if ((xsh < -20) && (xsh > -35) && (ysh < 15) && (ysh > 10))//16
            {
                ty = ty * -1f;
            }

            if ((xsh < -10) && (xsh > -21) && (ysh < 25) && (ysh > 15))//17
            {
                tx = tx * -1f;
            }

            if ((xsh  >-20) && (xsh < 0) && (ysh < 26) && (ysh > 20))//18
            {
                ty = ty * -1f;
            }

            if ((xsh < 1) && (xsh > -5) && (ysh < 25) && (ysh > 0))//19
            {
                tx = tx * -1f;
            }
            if ((xsh > -20) && (xsh < 0) && (ysh < 4) && (ysh > -1))//20
            {
                ty = ty * -1f;
            }
            if ((xsh < -19) && (xsh > -25) && (ysh < 0) && (ysh > -35))//21
            {
                tx = tx * -1f;
            }
            if ((xsh > -20) && (xsh < -10) && (ysh < -34) && (ysh > -40))//22
            {
                ty = ty * -1f;
            }
            if ((xsh < -5) && (xsh > -9) && (ysh < -10) && (ysh > -35))//23
            {
                tx = tx * -1f;
            }
            if ((xsh > -10) && (xsh < 0) && (ysh < -9) && (ysh > -15))//24
            {
                ty = ty * -1f;
            }

            //лунки   
            if ((xsh < -28) && (xsh > -32) && (ysh > 20) && (ysh < 24))
            {

                xsh = -30;
                ysh = 22;
                xc = -30;
                yc = 18;
                rad = 1.5839f;
                cx = 0;
                cy = 0;
                if (flag == 0) { label6.Text = "ПОБЕДИЛ ИГРОК 2"; }
                else { label6.Text = "ПОБЕДИЛ ИГРОК 1"; };
                label7.Text = "Количество общих ударов:" + Convert.ToString(Convert.ToInt32(yu));
             
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
            flag2=true;
            flag = yu % 2;
            label6.Text = "";
            label7.Text = "";
            tx = 1f;
            ty = 1f;
            cx = xsh - xc;
            cy = ysh - yc;
            pol = true;
            startTime = time;
            button1.Enabled = false;
            if (yu == 0) label1.Text = "";
            if (button1.Enabled == false)
                label1.Text = "";
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
            if ((xsh == xc) && (ysh > yc))
            {
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

        private void toolStripViewport_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        public void askInfoFromSocket(int port)
        {

            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
           

            //Console.Write("Введите сообщение: ");
            //string message = Console.ReadLine();
            string message = Convert.ToString(nomer) ;

            //Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);


            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            int bytesRec = sender.Receive(bytes);

            fkob = Encoding.UTF8.GetString(bytes, 0, bytesRec);


            string[] array = fkob.Split(' ');
            nomer = Convert.ToInt32(array[0]);
            yu1 = Convert.ToInt32(array[1]);
            xsh1 = Convert.ToInt32(array[2]);
            ysh1 = Convert.ToInt32(array[3]);
            xc1 = Convert.ToInt32(array[4]);
            yc1 = Convert.ToInt32(array[5]);
            cx1 = Convert.ToInt32(array[6]);
            cy1 = Convert.ToInt32(array[7]);
            rad1 = Convert.ToInt32(array[8]);
            xod = Convert.ToInt32(array[9]);
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }








        public void SendMessageFromSocket(int port)
        {

            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            //Console.Write("Введите сообщение: ");
            //string message = Console.ReadLine();
            string message = Convert.ToString(Convert.ToInt32(yu)) + ' ' + Convert.ToString(Convert.ToInt32(xsh)) + ' ' +
                Convert.ToString(Convert.ToInt32(ysh)) + ' ' + Convert.ToString(Convert.ToInt32(xc)) + ' ' 
                + Convert.ToString(Convert.ToInt32(yc))+' ' + Convert.ToString(Convert.ToInt32(cx)) + ' '
                + Convert.ToString(Convert.ToInt32(cy)) + ' ' + Convert.ToString(Convert.ToInt32(rad)) + ' ';

            //Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);


            // Отправляем данные через сокет
            int bytesSent = sender.Send(msg);

            // Получаем ответ от сервера


            int bytesRec = sender.Receive(bytes);

            fkob = Encoding.UTF8.GetString(bytes, 0, bytesRec);


            string[] array = fkob.Split(' ');
            nomer = Convert.ToInt32(array[0]); 
            yu1 = Convert.ToInt32(array[1]); 
            xsh1 = Convert.ToInt32(array[2]);
            ysh1 = Convert.ToInt32(array[3]);
            xc1 = Convert.ToInt32(array[4]);
            yc1 = Convert.ToInt32(array[5]);
            cx1 = Convert.ToInt32(array[6]);
            cy1 = Convert.ToInt32(array[7]);
            rad1 = Convert.ToInt32(array[8]);
            xod = Convert.ToInt32(array[9]);
            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            // if (message.IndexOf("<TheEnd>") == -1)
            // SendMessageFromSocket(port);

            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                askInfoFromSocket(11000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
            label2.Text =Convert.ToString(yu1)+' '+Convert.ToString(xsh1)+' '+Convert.ToString(ysh1)+' '+Convert.ToString(xc1)+' '+Convert.ToString(yc1);
            yu = yu1;
            xsh = xsh1;
            ysh = ysh1;
            xc = xc1;
            yc = yc1;

            rad = rad1;
            cx = cx1;
            cy = cy1;
            if (flag == 0)
            {
                label1.Text = "Ходит игрок 2";
            }
            else
            {
                label1.Text = "Ходит игрок 1";
            };
            

        DrawScene();

          //  MoveSphere();


        }
    }
}
 
