using System;
using System.Drawing;

namespace MyGame
{
    class Sputnik : BaseObject 
    {
        public Sputnik(Point pos, Point dir, Size size) : base(pos, dir, size){}

        Image sputnik = Image.FromFile(@"Images\sputnik.png");

        /// <summary>
        /// Реализация метода базового класса. Вывод через буфер картинок спутника на устройство вывода графики
        /// </summary>
        public override void Draw()
        {            
            Game.Buffer.Graphics.DrawImage(sputnik, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Переопределение базового метода. Обновление позиции на экране
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width;
        }
    }
}
