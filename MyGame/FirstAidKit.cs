using System;
using System.Drawing;

namespace MyGame
{
    class FirstAidKit : BaseObject
    {
        public FirstAidKit(Point pos, Point dir, Size size) : base(pos, dir, size){}

        Image firstAidKit = Image.FromFile(@"Images\firstAidKit.png");

        /// <summary>
        /// Вывод через буфер картинок аптечки на устройства вывода графики
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(firstAidKit, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
        }

        /// <summary>
        /// Обновление позиции на экране
        /// </summary>
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0)
            {
                Pos.X = Game.Width;
            }
        }
    }
}
