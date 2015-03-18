namespace Ch05_States
{
  using SFML.System;
  using System.Threading;

  internal class ParallelTask
  {
    private Thread thread;
    private bool finished;
    private Clock elapsedTime;
    private object mutex;

    public ParallelTask()
    {
      thread = new Thread(this.RunTask);
      elapsedTime = new Clock();
      mutex = new object();
    }

    public void Execute()
    {
      finished = false;
      elapsedTime.Restart();
      thread.Start();
    }

    public bool IsFinished()
    {
      lock (mutex)
      {
        return finished;
      }
    }

    public float GetCompletion()
    {
      // 100% at 10 seconds of elapsed time
      return elapsedTime.ElapsedTime.AsSeconds() / 10f;
    }

    private void RunTask()
    {
      // Dummy task - stall 10 seconds
      bool ended = false;
      while (!ended)
      {
        // Protect the clock
        lock (mutex)
        {
          if (elapsedTime.ElapsedTime.AsSeconds() >= 10)
          {
            ended = true;
          }
        }
      }

      // mFinished may be accessed from multiple threads, protect it
      lock (mutex)
      {
        finished = true;
      }
    }
  }
}
