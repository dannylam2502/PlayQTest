using System;
public class Container
{
    private class Node
    {
        public Node Next;
        public Node Prev;
        public bool Value;
        public Node(Node prev)
        {
            var randomGen = new Random(DateTime.Now.Millisecond);
            Value = randomGen.Next(2) < 1;
            //Console.Write(" " + Value.ToString());
            Prev = prev;
        }
    }
    private Node current;
    public Container(int count = 0)
    {
        if (count < 1)
        {
            var randomGen = new Random(DateTime.Now.Millisecond);
            count = randomGen.Next(1, 20); //Could be up to Int32.MaxValue, reduced for sake of test memory
        }
        //Console.WriteLine("Count = {0}", count); // for testing purpose only
        Node prev = null;
        for (int i = 0; i < count; i++)
        {
            var currentNode = new Node(prev);
            if (prev != null)
            {
                prev.Next = currentNode;
            }
            if (current == null)
            {

                current = currentNode;
            }
            prev = currentNode;
        }
        prev.Next = current;
        current.Prev = prev;
    }
    public bool Value
    {
        get { return current.Value; }
        set { current.Value = value; }
    }
    public void MoveForward()
    {
        current = current.Next;
    }
    public void MoveBackward()
    {
        current = current.Prev;
    }
}