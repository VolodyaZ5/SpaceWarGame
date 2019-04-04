using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Ship : BaseObject
    {
        private int _energy = 100; //Поле запаса энергии        
        private int _score = 0; //Очки за сбитие объектов

        public int Energy => _energy; //Свойство запаса энергии
        public int Score => _score; //Свойство очков за сбитие объектов


        Image ship = Image.FromFile(@"Images\ship.png");

        public static event Message MessageDie; //событие гибели корабля

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        /// <summary>
        /// Вывод через буфер картинки корабля на устройство вывода графики
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(ship, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// Уменьшение энергии
        /// </summary>
        /// <param name="n">Размер уменьшения</param>
        public void LowEnergy(int n)
        {
            _energy -= n;
        }

        /// <summary>
        /// Увеличение энергии
        /// </summary>
        /// <param name="n">Размер увеличения</param>
        public void UpEnergy(int n)
        {
            _energy += n;
        }

        /// <summary>
        /// Увеличение счетчика очков
        /// </summary>
        /// <param name="s">Размер увеличения</param>
        public void UpScore(int s)
        {
            _score += s;
        }

        /// <summary>
        /// Уменьшение счетчика очков за сбитие аптечки
        /// </summary>
        /// <param name="a">Величина штрафа</param>
        public void DownScore(int a)
        {
            _score -= a;
        }

        /// <summary>
        /// Обнуление очков при переходе на новый уровень
        /// </summary>
        public void ResetScore()
        {
            _score = 0;
        }

        /// <summary>
        /// Обнуление уровня жизни корабля при переходе на новый уровень
        /// </summary>
        public void ResetEnergy()
        {
            _energy = 100;
        }
        /// <summary>
        /// Обновление позиции на экране
        /// </summary>
        public override void Update()
        {

        }

        /// <summary>
        /// Движение корабля вверх
        /// </summary>
        public void Up()
        {
            if (Pos.Y > 0)
            {
                Pos.Y -= Dir.Y;
            }
        }

        /// <summary>
        /// Движение корабля вниз
        /// </summary>
        public void Down()
        {
            if (Pos.Y < Game.Height - Size.Height * 2)
            {
                Pos.Y += Dir.Y;
            }
        }

        /// <summary>
        /// Обработчик события гибели корабля
        /// </summary>
        public void Die()
        {
            MessageDie?.Invoke();
        }
    }
}
