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

            // byte[] superByte = tClient.DownloadData(path);

            // Image MyImage;


            return true;


            //Dim tClient As Net.WebClient = New Net.WebClient



            //Try
            //    Dim superByte() As Byte
            //    superByte = tClient.DownloadData(imgPath)

            //    If superByte.Length > 0 Then
            //        MyImage = Image.FromStream(New MemoryStream(superByte))
            //    Else
            //        Using bmpTemp As New Bitmap(imgPath)
            //            MyImage = New Bitmap(bmpTemp)
            //        End Using
            //    End If

            //Catch ex As WebException
            //    Debug.WriteLine(ex.Message)
            //    Using bmpTemp As New Bitmap(imgPath)
            //        MyImage = New Bitmap(bmpTemp)
            //    End Using
            //End Try


        }


    }
}
