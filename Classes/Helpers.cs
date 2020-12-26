//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     Helpers.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  Static class for misc functions
//
//
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
using System.Text.RegularExpressions;


namespace iYak.Classes
{
    public static class Helpers
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

        public static AudioFile GetAudioFileInfo(string Filename)
        {
            AudioFile audiof = new AudioFile()
            {
                FilePath = Filename,
                FileDate = File.GetCreationTime(Filename).ToShortDateString()
            };

            var info = new FileInfo(Filename);

            audiof.FileSize = (int) info.Length / 1000;

            return audiof;

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

        public static string GenerateFileName(Voice voice)
        {

            string FileName = voice.Nickname + "_";

            string words = Regex.Replace(voice.Speech, "[^a-zA-Z]", " ").Replace("  ", " ");

            if (words.Length > 20) words = words.Substring(0, 20);

            string timestamp = DateTime.UtcNow.Ticks.ToString();

            timestamp = timestamp.Substring(timestamp.Length - 5);

            FileName += words + "_" + timestamp;

            FileName = FileName.Replace(" ", "-").ToLower();

            if (FileName.Substring(FileName.Length - 1) == "-") FileName = FileName.Substring(0, FileName.Length - 2);

            return FileName;

        }

        public static Object GetPropValue(this Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

    }


    public class HttpPost
    {

        public WebHeaderCollection Headers = new WebHeaderCollection();
        public string Method = "POST";


        public string Request(string accessUri)
        {
            // Prepare OAuth request
            WebRequest webRequest = WebRequest.Create(accessUri);
            webRequest.Method = Method;
            webRequest.ContentLength = 0;
            if(Headers.Count >= 1) webRequest.Headers = Headers;

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (Stream stream = webResponse.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] waveBytes = null;
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = stream.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (stream.CanRead && count > 0);

                        waveBytes = ms.ToArray();

                        return Encoding.UTF8.GetString(waveBytes);
                    }
                }
            }
        }
    }
}
