namespace Ch05_States
{
  using System.Collections.Generic;

  internal class CommandQueue
  {
    private Queue<Command> queue = new Queue<Command>();

    public void Push(Command command)
    {
      queue.Enqueue(command);
    }

    public Command Pop()
    {
      return queue.Dequeue();
    }

    public bool IsEmpty()
    {
      return queue.Count == 0;
    }
  }
}
