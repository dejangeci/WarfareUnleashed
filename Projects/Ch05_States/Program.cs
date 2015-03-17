namespace Ch05_States
{
  using System;
  using System.Windows.Forms;

  internal class Program
  {
    private static void Main()
    {
      try
      {
        var app = new Application();
        app.Run();
      }
      catch (Exception e)
      {
        MessageBox.Show("An error has occured: " + e.ToString());
      }
    }
  }
}
