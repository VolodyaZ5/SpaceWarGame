using System;
using System.Drawing;

namespace MyGame
{
    abstract class BaseObject : ICollision
    {
        protected Point Pos; //Позиция
        protected Point Dir; //Направление
        protected Size Size; //Размер

        /// <summary>
        /// Делегат для использования событий
        /// </summary>
        public delegate void Message();

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        /// <summary>
        /// Реализация интерфейса ICollision, для определения пересечения
        /// объектов, реализующих этот интерфейс
        /// </summary>        
        public bool Collision(ICollision obj) => obj.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);

        /// <summary>
        /// Вывод через буфер кружочков на устройство вывода графики
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Обновление позиции на экране
        /// </summary>
        public abstract void Update();

        public void PrintLog()
        {
            Console.WriteLine($"Сбит объект!");
        }
    }
}
