namespace Ch04_Input
{
    using System;
    using System.Windows.Forms;

    internal class Program
    {
        private static void Main()
        {
            try
            {
                var game = new Game();
                game.Run();
            }
            catch (Exception e)
            {
                MessageBox.Show("An error has occured: " + e.ToString());
            }
        }
    }
}
