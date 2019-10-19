using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RandomStringGenerator_Control
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                RandomStringGenerator stringGenerator = new RandomStringGenerator();
                Console.Write(stringGenerator.Generator(args[0].Trim()));
            }
            else
            {
                Console.Write(Resources.Readme);
            }
        }
    }
}
