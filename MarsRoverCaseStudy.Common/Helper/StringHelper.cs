using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRoverCaseStudy.Common.Helper
{
    public interface IStringHelper
    {
        List<string> Split(string input, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries);
    }

    public class StringHelper : IStringHelper
    {
        public List<string> Split(string input, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return input.Split(' ', stringSplitOptions).ToList();
        }
    }
}
