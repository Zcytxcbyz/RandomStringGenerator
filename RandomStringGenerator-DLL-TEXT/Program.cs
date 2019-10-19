using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomStringGenerator;

namespace RandomStringGenerator_DLL_TEXT
{
    class Program
    {
        static void Main(string[] args)
        {
            RandomStringGenerator.RandomStringGenerator randomStringGenerator = new RandomStringGenerator.RandomStringGenerator();
            Console.WriteLine(randomStringGenerator.Generator("[0-9](5)"));
            Console.ReadKey();
        }
    }
}
