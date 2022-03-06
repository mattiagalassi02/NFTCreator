using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFTCreator
{
    internal class NFTCollector
    {
        private List<string> codeList;
        private List<string> levelNames;
        private List<int> levelRange;
        private List<string> order;

        private List<string> directories;
        private int range; 


        public NFTCollector(List<string> levels, List<int> rangeForLevel, List<string> levelDirectories , List<string> levelOrder, int rangeCollection)
        {
            if (levels.Count != rangeForLevel.Count) throw new ArgumentException("Invalid lists");
            if (levels.Count != levelDirectories.Count) throw new ArgumentException("Invalid lists");
            if (levels.Count != levelOrder.Count) throw new ArgumentException("Invalid lists");
            if (rangeCollection < 0 ) throw new ArgumentException("Invalid range");
            codeList = new List<string>();
            levelNames = levels; 
            levelRange = rangeForLevel;
            range = rangeCollection;
            order = levelOrder; 
            directories = levelDirectories;
        }
        /// <summary>
        /// Genero i codici casuali degli nft
        /// Se è già presente una combinazione non la inserisco 
        /// e continuo finchè non ne ho una valida 
        /// </summary>
        /// <returns>Ritorno nft creato oppure null se ho finito il range</returns>
        public NFTSkin next()
        {
            NFTSkin nft = null;

                if (range > 0)
                {
                    bool find = false;

                    nft = new NFTSkin(levelNames, levelRange, order);
                    do
                    {
                        string code = nft.Code;

                        if (!codeList.Contains(code))
                        {
                            codeList.Add(code);
                            find = true;
                            range--;
                        }

                    } while (!find) ;

                }
                //aggiungo le cartelle all'nft
                for(int i = 0; i < levelNames.Count; i++)
                {
                    nft.setDirectory(levelNames[i], directories[i]); 
                }
          

                return nft;
        }
        /// <summary>
        /// Ritorno vero se ho un prossimo nft (non ho finito il range)
        /// e falso se ho terminato la generazione
        /// </summary>
        public bool HasNext
        {
            get { return range > 0; }
        }
        public int Range
        { 
            get { return range; } 
            set
            { if(value > 0) range = value; }
        }
        public override string ToString()
        {
            string res = "";
            foreach (string code in codeList)
                res += code+"\n"; 
            return res;
        }

    }
}
