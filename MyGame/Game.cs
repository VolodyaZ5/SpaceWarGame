using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Media;
using System.IO;
using System.Text;

namespace MyGame
{   
    static class Game
    {
        //Высота и ширина игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }

        //Список объектов фона
        static List<BaseObject> objectInSpace = new List<BaseObject>();
        //Список объектов пуль
        static List<Bullet> bullets = new List<Bullet>();

        static Random rnd = new Random();
        private static Timer timer = new Timer { Interval = 50 };

        ///Количество целей
        private static int countGoals = 15;
        

        //Объект класса корабля
        static Ship ship;
        
        static SoundPlayer shot; //Звук выстрела
        static SoundPlayer crash; //Звук столкновения

        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
                
        public delegate void Message(string msg);

        static Game()
        {            
            ship = new Ship(new Point(0, 400), new Point(5, 20), new Size(32, 18));
            shot = new SoundPlayer(@"Sounds\gun.wav");
            crash = new SoundPlayer(@"Sounds\crash.wav");
        }
        public static void Init(Form form)
        {            
            //Устройство для вывода графики
            Graphics g;

            //Предоставляет доступ к главному буферу графического контекста для 
            //текущего приложения
            _context = BufferedGraphicsManager.Current;

            //Создаем объект и связываем его с формой
            g = form.CreateGraphics();

            //Запоминаем размеры формы            
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            //Связываем буфер в памяти с графическим объектом, чтобы рисовать в памяти
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            form.KeyDown += Form_KeyDown;

            Load(countGoals);            
            
            timer.Start();
            timer.Tick += (s, e) => { Draw(); Update(); };

            Ship.MessageDie += Finish;
        }

        /// <summary>
        /// Обработчик события нажатия на клавишу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bullets.Add(new Bullet(new Point(ship.Rect.Right, ship.Rect.Y + 3), new Point(25, 0), new Size(50, 15)));
                shot.Play();
            }
            if (e.KeyCode == Keys.Up)
            {
                ship.Up();
            }
            if (e.KeyCode == Keys.Down)
            {
                ship.Down();
            }
        }


        /// <summary>
        /// Вывод через буфер на устройство вывода графики
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in objectInSpace)
            {
                obj?.Draw();
            }

            foreach (Bullet b in bullets)
            {
                b.Draw();
            }

            ship?.Draw();
            if (ship != null)
            {
                Buffer.Graphics.DrawString($"Energy: {ship.Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString($"Score: {ship.Score}", SystemFonts.DefaultFont, Brushes.White, 80, 0);
                Buffer.Graphics.DrawString($"Left goals: {objectInSpace.Count}", SystemFonts.DefaultFont, Brushes.White, 150, 0);
            }

            Buffer.Render();
        }


        /// <summary>
        /// Обновление позиции на экране для каждого из элементов массива объектов фона
        /// </summary>
        public static void Update()
        {
            Message mess = new Message(PrintLog);
            foreach (BaseObject obj in objectInSpace)
            {
                obj.Update();
            }
            foreach (Bullet b in bullets)
            {
                b.Update();
            }
            for (int i = 0; i < objectInSpace.Count; i++)
            {                
                objectInSpace[i].Update();

                if (ship.Collision(objectInSpace[i]))
                {
                    ship?.LowEnergy(rnd.Next(1, 10));
                    if (ship.Energy <= 0)
                    {
                        ship?.Die();
                    }
                    if (objectInSpace[i] as FirstAidKit != null)
                    {
                        ship?.UpEnergy(rnd.Next(5, 10));
                    }
                }
                for (int j = 0; j < bullets.Count && objectInSpace.Count > 0; j++)
                {
                    try
                    {
                        if (bullets[j].Collision(objectInSpace[i]))
                        {
                            mess?.Invoke(objectInSpace[i].GetType().ToString());
                            crash.Play();
                            if (objectInSpace[i] as FirstAidKit != null)
                            {
                                ship.DownScore(200);
                            }
                            else
                            {
                                ship.UpScore(100);
                            }
                            objectInSpace.RemoveAt(i);
                            bullets.RemoveAt(j);                            
                        }
                    }
                    catch
                    {
                        j = bullets.Count;
                    }
                }

                if (objectInSpace.Count == 0)
                {
                    Load(++countGoals);
                    ship.ResetScore();
                    ship.ResetEnergy();
                }                
            }            
        }
        

        /// <summary>
        /// Создает объекты фона
        /// </summary>
        public static void Load(int count)
        {
            objectInSpace = new List<BaseObject>();
            for (int i = 0; i < count; i++)
            {
                int r = rnd.Next(5, 30);

                switch (rnd.Next(0, 5))
                {
                    case 0: objectInSpace.Add(new Asteroid(new Point(800, rnd.Next(0, Game.Height - 20)), new Point(-r, r), new Size(20, 20))); break;
                    case 1: objectInSpace.Add(new Star(new Point(600, rnd.Next(0, Game.Height - 20)), new Point(-r / 5, r), new Size(20, 20))); break;
                    case 2: objectInSpace.Add(new Sputnik(new Point(300, rnd.Next(0, Game.Height - 20)), new Point(-r / 2, r), new Size(30, 25))); break;
                    case 3: objectInSpace.Add(new NyanCat(new Point(700, rnd.Next(0, Game.Height - 20)), new Point(-r / 3, r), new Size(60, 25))); break;
                    case 4: objectInSpace.Add(new FirstAidKit(new Point(800, rnd.Next(Game.Height - 20)), new Point(-r / 5, r), new Size(25, 25))); break;
                }
            }            
        }

        /// <summary>
        /// Завершение игры вследствие гибели корабля
        /// </summary>
        public static void Finish()
        {
            timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 325, 250);
            Buffer.Render();
        }

        /// <summary>
        /// Печать лога в окно консоли и сохранение в файл
        /// </summary>
        /// <param name="msg">Сообщение для вывода в консоль и сохранения в файл</param>
        public static void PrintLog(string msg)
        {           
            Console.WriteLine($"- В {DateTime.Now} сбит {msg}!");

            StreamWriter sw = new StreamWriter("log.txt", true);
            sw.WriteLine($"- В {DateTime.Now} сбит {msg}!");
            sw.Close();
        }

        
    }
}
