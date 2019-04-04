using System;
using System.Windows.Forms;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MAX_FORM_WIDTH = 1000, MAX_FORM_HEIGHT = 800;
            
            Form form = new Form();
            try
            {
                form.Width = MAX_FORM_WIDTH;
                form.Height = MAX_FORM_HEIGHT;
                if (form.Width > MAX_FORM_WIDTH) throw new ArgumentOutOfRangeException("Превышена максимальная ширина экрана!");                
                if (form.Height > MAX_FORM_HEIGHT) throw new ArgumentOutOfRangeException("Превышена максимальная высота экрана!");                
            }
            catch (ArgumentOutOfRangeException ex)
            {                
                MessageBox.Show($"{ex.Message} Попытаемся исправить...");
                if (form.Width > MAX_FORM_WIDTH) form.Width = MAX_FORM_WIDTH;
                if (form.Height > MAX_FORM_HEIGHT) form.Height = MAX_FORM_HEIGHT;
                MessageBox.Show($"Установлены максимально возможные размеры экрана.");                
            }
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}
