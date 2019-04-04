using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    class Bullet : BaseObject
    {
        const int xMaxSize = 100, yMaxSize = 65;
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            try
            {
                if (size.Width > xMaxSize) throw new GameBulletException("Пуля не может быть такой длинной!");
                if (size.Height > yMaxSize) throw new GameBulletException("Пуля не может быть такой толстой!");
            }
            catch (GameBulletException ex)
            {
                MessageBox.Show($"{ex.Message} Попытаемся исправить...");
                if (size.Width > xMaxSize)
                {
                    size.Width = xMaxSize;
                }
                if (size.Height > yMaxSize)
                {
                    size.Height = yMaxSize;
                }
                MessageBox.Show("Установлены максимально возможные размеры пули.");
            }
        }

        Image bullet = Image.FromFile(@"Images\bullet.png");

        /// <summary>
        /// Реализация метода базового класса. Вывод через буфер картинки пули на устройство вывода графики
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(bullet, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
        }

        public override void Update()
        {            
            Pos.X += 25;
        }        
    }
}
