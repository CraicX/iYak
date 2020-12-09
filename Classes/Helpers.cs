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


    }
}
