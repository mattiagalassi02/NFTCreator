using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing; 

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

            Console.WriteLine(collector);
            //Dictionary<string, string> t = new Dictionary<string, string>();
            //t.Add("1", "1.jpg");
            //t.Add("2", "2.png");
            //t.Add("3", "3.png");

            //string[] order = { "2", "1", "3" };

            //LevelMerger merger = new LevelMerger(t, order, "res.png"); 
            //merger.mergeImages();


            Console.ReadLine();
           
        }
        
    }
}
