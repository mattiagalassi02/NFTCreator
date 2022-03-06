using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFTCreator
{
    internal class NFTSkin
    {
        private const string FILE_EXT = ".png";
        private string directoryPool;

        //contiene le associazioni livello - numero di elementi per livello
        private Dictionary<string, int> el_level;
        //contiene le associazioni livello - nome della directory per il livello associato
        private Dictionary<string, string> dir_level;
        //contiene le associazioni livello - numero scelto per livello
        private Dictionary<string, int> level_number;
        //lista che determina l'ordine degli elementi
        private List<string> order;

        private string name;
        private string code;
        /// <summary>
        /// Passo alla classe il numero di elementi che dispongono per ogni livello
        /// Se per un livello vengono passatio 0 elementi, si suppone che la skin non 
        /// possieda tael livello
        /// </summary>
        /// <param name="data"></param>
        public NFTSkin(List<string> levelNames, List<int> rangeForLever, List<string> levelOrder)
        {
            if (levelNames.Count != rangeForLever.Count) throw new ArgumentException("Invalid arguments for levels");
            //controllo che l'ordine contenga livelli ammissibili
            if(levelOrder.Count != levelNames.Count) throw new ArgumentException("Invalid arguments for levels");

            foreach (string lo in levelOrder)
            {
                if(!levelNames.Contains(lo)) throw new ArgumentException("Invalid order for levels");
            }

            el_level = new Dictionary<string, int>();

            for (int i = 0; i < levelNames.Count; i++)
            {
                el_level[levelNames[i]] = rangeForLever[i];
            }

            order = new List<string>();

            for (int i = 0; i < levelOrder.Count; i++)
            {
                order.Add(levelOrder[i]);
            }

            setDefaultData(levelNames);
            level_number = new Dictionary<string, int>();
        }
        private void generateNFT()
        {
            Random rnd = new Random();
            //genero casualmente una serie di numeri 
            //interi compresi tra 0 e il numero di oggettia disposizione per livello 
            //concateno tutto in una stringa 
            string value = "";

            //ora vado a generare in ordine

            for (int i = 0; i < order.Count; i++)
            {
                level_number[order[i]] = rnd.Next(1, el_level[order[i]] + 1);
                value += level_number[order[i]];
            }

            code = value;
        }
        public string Code
        {
            get
            {
                generateNFT();
                return code;
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (value != null && value != "")
                    name = value;
            }
        }
        public override string ToString()
        {
            return "CODE:" + code + "\n" + "NAME:" + name + "\n" + getalldir();
        }
        private string getalldir()
        {
            string dir = "";
            foreach(string s in new List<string>(dir_level.Keys))
                dir+=getFileName( dir_level[s])+"\n";
            return dir; 
        }
        public override bool Equals(object obj)
        {
            if (obj is NFTSkin)
            {
                NFTSkin tmp = obj as NFTSkin;
                return tmp.name.Equals(name) && tmp.code.Equals(code);
            }
            return false;
        }
        public override int GetHashCode()
        {
            //faccio la or dei due campi 
            return (name.GetHashCode() | code.GetHashCode());
        }
        //da qui in poi una serie di prorpietà 
        //per ottenere il nome dei file che ne rappresentano i livelli 
        public void setDirectory(string key, string value)
        {
            if (!dir_level.ContainsKey(key)) throw new ArgumentException("Invalid level");
            if(value == null || value == "") throw new ArgumentException("Invalid directory name");

            dir_level[key] = value;
        }

        public string PoolDirectory
        {
            set
            {
                if (value != null && value != "")
                    directoryPool = value;
            }
        }

        public string getFileName(string key)
        {
            if (!dir_level.ContainsKey(key)) throw new ArgumentException("Invalid level");

            //se il nome della cartella ha il percorso assoluto
            //non ci metto il pool
            if(dir_level[key].Contains("\\") && dir_level[key].Contains(":"))
                return dir_level[key] +"\\" +level_number[key] + FILE_EXT;
            else return directoryPool +"\\"+ dir_level[key] + "\\" + level_number[key] + FILE_EXT;
        }

        private void setDefaultData(List<string> levelNames)
        {
            directoryPool = Environment.CurrentDirectory;
            dir_level = new Dictionary<string, string>();

            //per ogni nome di livello associo il nome di cartella uguale 
            //al nome di livello ma con tutte minuscole 
            for (int i = 0; i < levelNames.Count; i++)
            {
                dir_level[levelNames[i]] = levelNames[i].ToLower();
            }
        }
    }
}
