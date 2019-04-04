using System;
using System.Drawing;

namespace MyGame
{
    class Asteroid : BaseObject, ICloneable, IComparable<Asteroid>
    {
        public int Power { get; set; } = 3;
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        Image asteroid = Image.FromFile(@"Images\asteroid.png");

        /// <summary>
        /// Реализация метода базового класса. Вывод через буфер картинок астероида на устройство вывода графики
        /// </summary>
        public override void Draw()
        {            
            Game.Buffer.Graphics.DrawImage(asteroid, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
        }

        /// <summary>
        /// Переопределение базового метода. Обновление позиции на экране
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width;            
        }

        /// <summary>
        /// Копирование объекта астероида
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y),
                new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height))
            {
                Power = Power
            };
            return asteroid;
        }

        /// <summary>
        /// Сравнение астероидов
        /// </summary>
        /// <param name="obj">Сравниваемый объект астероида</param>
        /// <returns></returns>
        int IComparable<Asteroid>.CompareTo(Asteroid obj)
        {
            if (Power > obj.Power)
            {
                return 1;
            }
            if (Power < obj.Power)
            {
                return -1;
            }
            return 0;
        }
    }
}
