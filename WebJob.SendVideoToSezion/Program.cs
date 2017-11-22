using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace WebJob.SendVideoToSezion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

            
            //ONRVideo.ControlerMailer ctlMailer = new ONRVideo.ControlerMailer();
            //ctlMailer.SendErrorLogMessage("Le sujet du courriel", "Le corpd de courriel");


            string url = "https://sezion.com/api?accountID=573edfa366b9071b5246c037&accountSecret=eYqorVZ4VP6Q_pHzL_OVqX_ESCEiI3n6AeDplqeLwTQ=";
            //request.ContentType = "application/json; charset=utf-8";


            var json = "{\"jsonrpc\":\"2.0\",\"method\":\"Template_Video_List\",\"params\":{\"templateID\":\"2\"},\"id\":1}";
            //string response;
            using (var webClient = new WebClient())
            {
                // Required to prevent HTTP 401: Unauthorized messages
                //webClient.Credentials = new NetworkCredential(username, password);


               var response = webClient.UploadString(url, "POST", json);
            }

            }
            catch (Exception ex )
            {

                throw ex;
            }
            //var response = (HttpWebResponse)request.GetResponse();
        }
    }
}
