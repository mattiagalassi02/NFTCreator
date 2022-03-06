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
            try
            {
                DataFileReader reader = new DataFileReader("conf.txt");

                List<string> levels = reader.Levels;

                List<int> ranges = reader.Ranges;

                List<string> directories = reader.Directories;

                List<string> order = new List<string>(reader.Order);

                int range = reader.NFTRange;

                string saving_folder = reader.SavingFolder;

                string name = reader.Name;

                NFTCollector collector = new NFTCollector(levels, ranges, directories, order, range);

                int counter = 1; 

                while (collector.HasNext)
                {
                    //genero l'nft e gli setto il nome 
                    NFTSkin nft = collector.next();

                    nft.Name = name + counter.ToString();
                    counter++;


                    //mi costruisco un dizionario con le coppie 
                    //nome livello - nome file da usare

                    Dictionary<string, string> setter = new Dictionary<string, string>();

                    foreach (string level in levels)
                        setter[level] = nft.getFileName(level);

                    LevelMerger merger = new LevelMerger(setter, order.ToArray(), saving_folder ,nft.Name);
                    merger.mergeImages();

                    Console.WriteLine("NFT "+(counter-1)+" successfully created!"+"\n");
                    Console.WriteLine("------------------ DATA ------------------");
                    Console.WriteLine(nft);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.WriteLine("Press ENTER to close");

            Console.ReadLine();
           
        }
        
    }
}
