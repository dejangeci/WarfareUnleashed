namespace Ch05_States
{
    using System.Diagnostics;
    using System.Threading;

    internal class ParallelTask
    {
        private Thread thread;
        private bool finished;
        private Stopwatch elapsedTime;
        private object mutex;

        public ParallelTask()
        {
            thread = new Thread(this.RunTask);
            elapsedTime = new Stopwatch();
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
            return (elapsedTime.ElapsedMilliseconds / 1000f) / 10f;
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
                    if (elapsedTime.Elapsed.Seconds >= 10)
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
