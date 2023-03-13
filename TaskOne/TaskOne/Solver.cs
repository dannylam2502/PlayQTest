using System;

namespace TaskOne
{
    public class Solver
    {
        /*
            The idea is to mark a position with a unique value until we loop through the entire list. Since this
        is a looped list, we will see that unique value appear at the end. Steps:
        1. Cache the first position's value, let's call this a ref value
        2. Change the first position to the opposite value, to make sure we will always find at least 1 step
        3. Loop the list, if we see the opposite value, turn it to the ref value, store the counter position to reverse the value later.
        The last time we see the opposite value is the result we're looking for.
        4. We will stop the list if we reach a certain steps without the opposite value. I chose 100 steps because the probability of having
        a 100 times all true/false is 2^100. (note that the random generator in the provided code is using the current time milisecond as the seed, 
        which may cause fast machine generates all the same values during that tick).
        5. Reverse everything to the original state.
        **Edge Cases:
        - The random is met 2^100. Ex: True ... True (x101) .. False
        Time complexity O(n), Space complexity O(n)
        **Worst case:
        - The opposite value is much more dominating than 50/50. Ex: true false false false false false false false true
         */
        public int CountContainerNum(Container container)
        {
            List<int> flagSteps = new List<int>(); // the list to store all the steps we changed the values
            int counter = 0;
            int breakCounter = 0;
            int maxCount = 100; // if we loop maxCount times without seeing any opposite value, and if we've seen at least 1 flag, we stops
            int result = 0; // the result will be the last value of the flagSteps
            bool refValue = container.Value; // cache first value, make it as ref
            container.Value = !refValue; // mark it to the opposite value, so that we will always find one.
            while (true)
            {
                container.MoveForward();
                counter++;
                breakCounter++;
                if (container.Value == !refValue)
                {
                    // store the steps, we will use it to reverse the value to keep the container to its original state
                    flagSteps.Add(counter - result); // store the step needed to move from the last step
                    result = counter; // update the result
                    container.Value = refValue; // change it to the opposite value
                    breakCounter = 0; // reset the breakCounter as this is the condition to break the loop
                }
                // if we get at least 1 steps and
                // if we loop the entire current result plus maxCount steps without seeing opposite value, we break
                if (flagSteps.Count > 0 &&
                    breakCounter > maxCount + result)
                {
                    break;
                }
            }
            // need to reverse the result to keep the original state, start by moving the current to the original
            // the breakCounter is the number of steps we moved to make sure there's no exception.
            for (int i = 0; i < breakCounter; i++)
            {
                container.MoveBackward();
            }
            // each steps is where we need to reverse the value back
            for (int i = 0; i < flagSteps.Count - 1; i++)
            {
                int step = flagSteps[i];
                for (int j = 0; j < step; j++)
                {
                    container.MoveForward();
                }
                container.Value = !refValue;
            }
            // the last one in the steps is the first one that we changed the value
            for (int i = 0; i < flagSteps[flagSteps.Count - 1]; i++)
            {
                container.MoveForward();
                //container.Value = refValue;
            }
            /* Test
            Console.WriteLine();
            for (int i = 0; i < result; i++)
            {
                Console.Write(" " + container.Value.ToString());
                container.MoveForward();
            }
            */
            return result;
        }
    }
}
