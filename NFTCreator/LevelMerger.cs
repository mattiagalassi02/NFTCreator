using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO; 

namespace NFTCreator
{
    internal class LevelMerger
    {
        private Dictionary<string, string> filesLevel;
        private string[] order;
        private ImageFormat format;
        private string name;
        private string directory;
        private string saving_folder; 

        public LevelMerger(Dictionary<string, string> fileNames, string[] _order, string savingFolder,string resName )
        {
            //controllo che la lunghezza dei livelli sia la stessa dell'ordine
            if (fileNames.Keys.Count != _order.Length || resName == null) throw new ArgumentException("Illegal Parameters");

            filesLevel = fileNames; 
            order = _order;
            //controllo che i livelli ordinati siano tutti presenti 
            foreach(string level in order)
            {
                if(!filesLevel.ContainsKey(level))
                    throw new ArgumentException("Illegal Parameters");
            }
            name = resName;

            //controllo se esiste la cartella di out altrimenti la creo 

            directory = savingFolder; 
            DirectoryInfo dir = new DirectoryInfo(directory);
            if(!dir.Exists)
                dir.Create();

            format = ImageFormat.Png;
        }

        public ImageFormat Format
        {
            get { return format; }
            set { 
                if(value!=null)
                format = value; 
            }
        }
        public string Name
        {
            get { return name; }
        }
        public string Directory
        {
            get { return directory; }
            set
            {
                if(value != null)
                directory = value;
            }
        }
        public void mergeImages()
        {
            //creo il bitmap dell'timmagine
            Bitmap image = CombineBitmap();
            //salvo l'immagine con gli argomenti specificati 
            image.Save(directory +"\\"+ name +".png", format);
        }
        private Bitmap CombineBitmap()
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string level in order)
                {
                    string image = filesLevel[level];
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    //with the largest and the highest 
                    if(bitmap.Width > width)
                        width = bitmap.Width;
                    if (bitmap.Height > height)
                        height = bitmap.Height; 

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);
                    g.Clear(System.Drawing.Color.Transparent);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image,new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        //offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
    }
}
