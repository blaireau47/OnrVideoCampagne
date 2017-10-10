using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace OnrVideoWebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            ONRVideo.ControlerJsonToModel ctlOnrVideo = new ONRVideo.ControlerJsonToModel();
            string soireJSON = GetJSon("http://onrvideo.azurewebsites.net/soirees/soiree2281.txt");  //("https://eblais_test:1234qwer@onr-pilote.com/app/api/eveningStatistics?eveningId=2281");

            ctlOnrVideo.RetrieveModelFromJSon(soireJSON);
            
        }


        static private string GetJSon(string _url)
        {
            string retValue = string.Empty;

            var request = WebRequest.Create(_url);
            request.ContentType = "application/json; charset=utf-8";


            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                retValue = sr.ReadToEnd();
            }
            ONRVideo.ControlerJsonToModel ctlOnrVideoJson = new ONRVideo.ControlerJsonToModel();

            ctlOnrVideoJson.RetrieveModelFromJSon(retValue);

            return retValue;

        }
    }
}
