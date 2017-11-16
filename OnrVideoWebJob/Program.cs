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

            try
            {
                //string soireJSON = GetONRSoireeJSon("http://onrvideo.azurewebsites.net/soirees/soiree2281.txt");  //("https://eblais_test:1234qwer@onr-pilote.com/app/api/eveningStatistics?eveningId=2281");

                ONRVideo.ControlerInfoSoiree infoSoir = new ONRVideo.ControlerInfoSoiree(new DateTime("2017-01-01"));

                infoSoir.GetSoireeInformation(36);

            }
            catch (Exception ex)
            {
                ONRVideo.ControlerLogger.LogError("WebJob GetSjon Error", ex);
            }



            //string allText = System.IO.File.ReadAllText(@"C:\projets\onrvideo\sezionReturnEx.json");

            //ONRVideo.ControlerSezionResponseSaveVideo ctlSaveVideoInfo = new ONRVideo.ControlerSezionResponseSaveVideo();

            //ctlSaveVideoInfo.RetrieveSezionInfo(allText);


        }


        static private string GetONRSoireeJSon(string _url)
        {
            string retValue = string.Empty;

            var request = WebRequest.Create(_url);
            request.ContentType = "application/json; charset=utf-8";


            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                retValue = sr.ReadToEnd();
            }
            ONRVideo.ControlerSoireeJsonToModel ctlOnrVideoJson = new ONRVideo.ControlerSoireeJsonToModel();

            ctlOnrVideoJson.RetrieveModelFromJSon(retValue);

            return retValue;

        }
    }
}
