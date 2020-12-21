using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Net;
using System.Drawing;
using System.Windows.Forms;


namespace iYak.Classes
{
    public class Helpers
    {

        static public string JoinPath(params String[] Args) {

            string rPath = "";

            if (Args.Length <= 0) return rPath;

            foreach (string pathSect in Args) {
                rPath += "\\" + pathSect;
            }

            return rPath.Replace("\\\\", "\\").Substring(1);

        }

        static public List<String> GlobList(String path) {

            List<String> FileList = new List<String>();

            if (Directory.Exists(path)) {
                foreach (string foundFile in Directory.GetFiles(path) ) {
                    FileList.Add(foundFile);
                }
            }

            return FileList;

        }

        static public bool DownloadImage(String path, String savePath) {

            WebClient tClient = new WebClient();

            String fName = path.Substring(path.LastIndexOf('/'));

            tClient.DownloadFile(path, savePath + "\\" + fName);

            return true;

        }


        static public Image LoadImage(string imgPath)
        {

            if (File.Exists(imgPath))
            {

                Bitmap bmpTemp = new Bitmap(imgPath);
                Bitmap MyImage = new Bitmap(bmpTemp);

                return MyImage;

            }

            return new System.Drawing.Bitmap(48, 48);

        }
        static public void Shuffle(ref List<string> theList)
        {
            var r  = new Random();
            int lc = theList.Count();
            int i  = 0;

            for (i = 0; i < lc - 1; i++) {

                int index = r.Next(i, lc);

                if (i != index) {
                    string temp    = theList[i];
                    theList[i]     = theList[index];
                    theList[index] = temp;
                }
            }
        }

        

        public static string LoadFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                string FileData = File.ReadAllText(FilePath).Trim();
                return FileData;
            }

            return "";
        }

        public static void Alert(string message, string caption="Alert") 
        {
            MessageBox.Show(message, caption);
        }

    }
}
