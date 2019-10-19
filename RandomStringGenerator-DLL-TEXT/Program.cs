using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RandomStringGenerator
{
    class Program
    {
        [DllImport("RandomStringGenerator.dll")]
        public static extern string Generator(string Expression);
        static void Main(string[] args)
        {
            Console.WriteLine(Generator("[0-9](5)"));
            Console.ReadKey();
        }
    }
}
