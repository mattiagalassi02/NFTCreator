using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace NFTCreator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataFileReader reader = new DataFileReader("conf.txt");

            List<string> level = reader.Levels; 

            List<int> ranges = reader.Ranges;

            NFTCollector collector = new NFTCollector(level, ranges, 20);

            while (collector.HasNext)
                Console.WriteLine(collector.next());

            //Console.WriteLine(collector);
            Console.ReadLine();

        }
    }
}
