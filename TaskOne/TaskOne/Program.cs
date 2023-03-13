// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using TaskOne;

Solver solver = new Solver();
Container container = new Container();
// get the count first, then use the num count to test
var stopwatch = new Stopwatch();
stopwatch.Start();
int count = solver.CountContainerNum(container);
stopwatch.Stop();
Console.WriteLine("Result = {0}, time = {1}ms", count, stopwatch.ElapsedMilliseconds);
Console.WriteLine("Time complexity O(n), space complexity O(n)");
/*
var randomGen = new Random(DateTime.Now.Millisecond);
for (int i = 1; i <= 10; i++)
{
    Console.WriteLine("Test number {0}", i);
    // randomly move forward or backward
    int randomMove = randomGen.Next(count);
    for (int j = 0; j < randomMove; j++)
    {
        bool r = randomGen.Next(2) < 1;
        if (r)
        {
            container.MoveForward();
        }
        else
        {
            container.MoveBackward();
        }
    }
    Console.WriteLine("Result is {0}", solver.CountContainerNum(container));
}
*/
