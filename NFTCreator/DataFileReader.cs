﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace NFTCreator
{
    internal class DataFileReader
    {
        private const char COMMENT_RECOGNIZER = '#';
        private const char DATA_RECOGNIZER = '='; 
        private const char DATA_SEPARATOR = ';';
        private const int DEFAULT_RANGE = 6;
        private const string NAME = "EVE_";
        private string SAVING_FOLDER; 

        //variabile booleana che mi serve per capire se,
        //una volta generato il file di defualt non devo effettuare la lettura 
        private bool dataAvaiable;
        //liste dati
        private List<string> levels; 
        private List<int> ranges;
        private List<string> directories;
        private List<string> order; 
        private int rangeTot;
        private string name; 

        /// <summary>
        /// Controllo se il file esiste e ne eseguo la lettura
        /// Se non esiste ne genero uno di default 
        /// </summary>
        /// <param name="fileName"></param>
        public DataFileReader(string fileName)
        {
            levels = new List<string>();
            directories = new List<string>();
            ranges = new List<int>(); 
            order = new List<string>();

            if (!File.Exists(fileName))
                generateDefaultFile(fileName);

            if (!dataAvaiable)
                readData(fileName); 
        }
        private void generateDefaultFile(string fileName)
        { 

            //la cartella dove mettere gli nft generati 
            SAVING_FOLDER = Environment.CurrentDirectory + "\\nft"; 
            //-------------------------------------------------------
            string currentdir = Environment.CurrentDirectory;
            //creo una lista di cartelle prelevando quelle locali 
            List<string> curr_dir = new List<string>(Directory.GetDirectories(currentdir));
            //ora per ogni cartella vado a leggere il numero di file interni e 
            //creo il record da scrivere su file 
            StreamWriter writer = new StreamWriter(new FileStream("conf.txt", FileMode.Create));

            //scrivo il trailer del file 

            writer.WriteLine(@"#I dati seguenti servono a configurare il programma
#formattare i dati nel seguente modo:
#nome_livello=range_livello;nome_cartella_file_livello
#si può specificare il percorso assoluto della cartella o solo il nome
#(in tal caso viene preso di default la cartella corrente)
#il cancelletto è il carattere per i commenti 
#usare la parola range per spcificare il numero di nft 
#separare ventuali dati con il ';'
"); 

            string dir = ""; 

            for (int i = 0; i < curr_dir.Count; i++)
            {
                dir = curr_dir[i];
                //da questo processo escludo la cartella nft che assumo possa essere quella out di defult 
                if (dir!=SAVING_FOLDER)
                {
                    //prelevo numero file della cartella
                    int numFiles = Directory.GetFiles(dir).Length;
                    ranges.Add(numFiles);
                    //dir è il percorso assoluto e mi serve solo la cartella relativa
                    string[] tmp = dir.Split('\\');
                    dir = tmp[tmp.Length - 1];

                    levels.Add(dir);
                    directories.Add(dir);

                    writer.WriteLine(dir + DATA_RECOGNIZER + numFiles + DATA_SEPARATOR + dir);
                }

            }
            //aggiungo la riga del range 
            writer.WriteLine("range"+DATA_RECOGNIZER+DEFAULT_RANGE);
            //aggiungo la riga del nome 
            writer.WriteLine("name" + DATA_RECOGNIZER + NAME);
            name = NAME;
            //aggiungo la riga dell'out folder
            writer.WriteLine("out_folder" + DATA_RECOGNIZER + SAVING_FOLDER);
            //aggiungo la riga dell'ordine 

            writer.Write("order" + DATA_RECOGNIZER);
            defaultOrder();
            for (int i = 0;i < order.Count;i++)
            {
                writer.Write(order[i]); 

                if(i != (order.Count - 1))//se non è l'ultimo metto la virgola 
                    writer.Write(DATA_SEPARATOR); 
            }


            writer.Close();
            rangeTot = DEFAULT_RANGE;
            dataAvaiable = true;
        }
        private void defaultOrder()
        {
            foreach(string level in levels)
                order.Add(level);
        }
        private void readData(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            try
            {
                //continuo a leggere fino alla fine del file 
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    //se non ho un commento faccio la lettura
                    if (!line.Contains(COMMENT_RECOGNIZER) && !line.Equals(""))
                    {
                        //splitto sul carattere dei sepatatore di dati e livelli
                        //cioè nomeLivello=dati
                        string[] vs = line.Split(DATA_RECOGNIZER);

                        //eliminio eventuali spazi
                        removeSpaces(vs);
                        //se la parola chiave è range lo salvo
                        if(vs[0].ToLower().Equals("range"))
                        {
                            rangeTot = Int32.Parse(vs[1]);
                        }
                        else if(vs[0].ToLower().Equals("order"))//se la parola è ordine lo salvo 
                        {

                            string[] orderlevel = vs[1].Split(DATA_SEPARATOR); 
                            foreach(string level in orderlevel)
                                order.Add(level.ToLower());

                        }
                        else if (vs[0].ToLower().Equals("name"))//se la parola è nome lo salvo 
                        {
                            name = vs[1].Trim();
                        }
                        else if (vs[0].ToLower().Equals("out_folder"))//se la parola è nome lo salvo 
                        {
                            SAVING_FOLDER = vs[1].Trim();
                        }
                        else
                        {
                            levels.Add(vs[0]);

                            string[] data = vs[1].Split(DATA_SEPARATOR);
                            removeSpaces(data);
                            ranges.Add(Int32.Parse(data[0]));

                            directories.Add(data[1]);
                        }

                    }
                }
            }
            catch (Exception e)
            { throw new Exception("Bad data format");  }
            finally
            { reader.Close(); }
 
        }
        private void removeSpaces(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
                words[i] = words[i].Trim();
        }
        //Accesso ai dati
        //non ritorno le liste interne ma creo delle copie 
        public List<string> Levels
        {
            get
            {
                List<string> tmp = new List<string>();
                foreach (string s in levels)
                    tmp.Add(s);
                return tmp;
            }
        }
        public string[] Order
        {
            get
            {
                return order.ToArray();
            }
        }
        public List<string> Directories
        {
            get
            {
                List<string> tmp = new List<string>();
                foreach (string s in directories)
                    tmp.Add(s);
                return tmp;
            }
        }
        public List<int> Ranges
        {
            get
            {
                List<int> tmp = new List<int>();
                foreach (int n in ranges)
                    tmp.Add(n);
                return tmp;
            }
        }
        public int NFTRange
        { get { return rangeTot; } }

        public string Name 
        { get { return name; } }

        public string SavingFolder
        { get { return SAVING_FOLDER; } }

    }
}
