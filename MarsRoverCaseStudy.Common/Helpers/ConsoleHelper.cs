using System;

namespace MarsRoverCaseStudy.Common.Helpers
{
    public interface IConsoleHelper
    {
        string ReadLine();
        void WriteLine(string input);
    }

    public class ConsoleHelper : IConsoleHelper
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string input)
        {
            Console.WriteLine(input);
        }
    }
}
