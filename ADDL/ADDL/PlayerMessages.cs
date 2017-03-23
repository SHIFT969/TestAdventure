using System;

namespace ADDL
{
    public static class PM // Player Messages
    {
        // takes alternating number of params. consider using overloaded methods for performance
        public static void OutputMessage(params Object[] objects) 
        {
            try
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if ((i + 1) % 2 == 1)  // every even number suppose to be a color
                    {
                        Console.ForegroundColor = (ConsoleColor)objects[i];
                    }
                    else if ((i + 1) % 2 == 0)  // every uneven number suppose to be a string
                    {
                        Console.Write((String)objects[i]);
                    }
                }
            }
            catch
            {
                throw new Exception("Wrong order for string output params. Algorythm error.");
            }
        }
    }
}
