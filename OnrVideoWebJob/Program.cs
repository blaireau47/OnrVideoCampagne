using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;



namespace OnrVideoWebJob
{
    class Program
    {
        static void Main(string[] args)
        {

    





                try
                {
                    ONRVideo.ControlerLogger.WriteInformation("Started this incredible OnrWebJob baby");
                    DateTime soiree = DateTime.Now;
                    ///Valider si la soirée a déjà été sauvé
                    ///Si oui sauter la récupération du JSON

                    using (ONRVideo.ONRCampagneVideoEntities onrEntities = new ONRVideo.ONRCampagneVideoEntities())
                    {
                        //Verifie if soirée already exist
                        List<ONRVideo.Soiree> laSoiree = (from c in onrEntities.Soirees
                                                          where c.soiree1 == soiree
                                                          select c).ToList<ONRVideo.Soiree>();
                        ONRVideo.ControlerLogger.WriteInformation("Checked if soiree exist");
                        ///Check if Soiree exist
                        if (laSoiree.Count == 0)
                        {
                            

                        string fichier = string.Format("http://onrvideo.azurewebsites.net/soirees/soiree{0}{1}{2}.txt",DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
                        ONRVideo.ControlerLogger.WriteInformation(String.Format("Soiree doies not exist , retrieving current soiree {0} and saving to DB looking for file {1}", soiree, fichier));
                        string soireJSON = SaveONRSoireeFromJSon("http://onrvideo.azurewebsites.net/soirees/soiree2281.txt");  //("https://eblais_test:1234qwer@onr-pilote.com/app/api/eveningStatistics?eveningId=2281");
                        //string soireJSON = SaveONRSoireeFromJSon("https://eblais_test:1234qwer@onr-pilote.com/app/api/eveningStatistics?date=2017-08-16&centralLabel=1111");
                        //string soireJSON = SaveONRSoireeFromJSon("https://eblais_test:1234qwer@onr-pilote.com/app/api/eveningStatistics?date=2017-08-16&centralLabel=1296");
                    }

                    }






                }
                catch (Exception ex)
                {
                    ONRVideo.ControlerLogger.LogError("WebJob GetSjon Error", ex);
                }


            //try
            //{
            //    ///Envoyer les informations à Sezion. Identifier ceux envoyé pour éviter d'envoye plus d'une foi.
            //}
            //catch (Exception ex)
            //{
            //    ONRVideo.ControlerLogger.LogError("WebJob GetSjon Error", ex);
            //}



            try
            {
                ///Identifier les courriel à envoyer
                ///La vue utilisé pour récupérer les informations de video a envoyé s'assure que le 
                ///champ SentOn est vide, donc qu'aucun courriel n'a été envoyé

                ///Récupère les info à envoyer directement dans le controleur

                ONRVideo.ControlerSendEmailToEquipes ctlSendEmailToTeams = new ONRVideo.ControlerSendEmailToEquipes();

                ///Le send récupère les Teambers et appel le controleurMail pour gérer l'envoie de courriel               
                ///Le SendVideoToVolunteers est responsable d'indiquer que le courriel est parti en remplissant 
                ///le colonne SentOn de la table des Videos
                ctlSendEmailToTeams.SendVideoToVolunteers();

            }
            catch (Exception ex)
            {
                ONRVideo.ControlerLogger.LogError("WebJob SendEmail to volunteers Error", ex);
            }





        }


        /// <summary>
        /// Récupère le contenu en format JSON et l'insere dans la BD
        /// </summary>
        /// <param name="_url"></param>
        /// <returns></returns>
        static private string SaveONRSoireeFromJSon(string _url)
        {
            string retValue = string.Empty;

            var request = WebRequest.Create(_url);
            request.ContentType = "application/json; charset=utf-8";
            //string authInfo = "eblais_test" + ":" + "1234qwer";
            //authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            //request.Headers["Authorization"] = "Basic " + authInfo;


            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                retValue = sr.ReadToEnd();
            }
            ONRVideo.ControlerSoireeJsonToModel ctlOnrVideoJson = new ONRVideo.ControlerSoireeJsonToModel();

            ///Interprete le JSON et l'insere dans la BD ONR
            ctlOnrVideoJson.RetrieveModelFromJSon(retValue);

            return retValue;

        }



        static private void SendEmailVideoToTeams(DateTime _dateSoiree)
        {

            ///ControlerSendEmailToEquipes gets all the videos to be sent to the teams
            ONRVideo.ControlerSendEmailToEquipes ctlSendEmailToTeams = new ONRVideo.ControlerSendEmailToEquipes();

            ///Validate if videos will be sent. If not raise warning with info date, number of volunteers, numbers of teams etc...


            ///SendVideoToVolunteers will retrieve all the volunteers for each teams 
            ///The volunteers and videoinformation are inserted in a ModelTeamEmail object before 
            ///using the COntrolerMailer
            ctlSendEmailToTeams.SendVideoToVolunteers();



        }
    }
}
////ONRVideo.ControlerMailer ctlMailer = new ONRVideo.ControlerMailer();
////ctlMailer.SendErrorLogMessage("Le sujet du courriel", "Le corpd de courriel");


//string url = "https://sezion.com/api?accountID=573edfa366b9071b5246c037&accountSecret=eYqorVZ4VP6Q_pHzL_OVqX_ESCEiI3n6AeDplqeLwTQ=";
////request.ContentType = "application/json; charset=utf-8";

//JObject joe = new JObject();
//joe["jsonrpc"] = "2.0";
////joe["id"] = "1";
//joe["method"] = "Template_Video_List";
//string s = Newtonsoft.Json.JsonConvert.SerializeObject(joe);
//using (var webClient = new WebClient())
//{
//    // Required to prevent HTTP 401: Unauthorized messages
//    //webClient.Credentials = new NetworkCredential(username, password);
//    // API Doc: http://kodi.wiki/view/JSON-RPC_API/v6
//    //var json = "{\"jsonrpc\":\"2.0\",\"method\":\"GUI.ShowNotification\",\"params\":{\"title\":\"This is the title of the message\",\"message\":\"This is the body of the message\"},\"id\":1}";
//    var response = webClient.UploadString(url, "POST", s);
//}


//var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
////httpWebRequest.ContentType = "application/json-rpc";
//httpWebRequest.Method = "POST";






//    // serialize json for the request
//    byte[] byteArray = Encoding.UTF8.GetBytes(s);
//    httpWebRequest.ContentLength = byteArray.Length;

//    //var json = "{\"jsonrpc\":\"2.0\",\"method\":\"Template_Video_List\",\"params\":{\"templateID\":\"2\"},\"id\":1}";
//    //string response;
//    using (Stream streamWriter = httpWebRequest.GetRequestStream())
//    {
//        // Required to prevent HTTP 401: Unauthorized messages
//        //webClient.Credentials = new NetworkCredential(username, password);


//        streamWriter.Write(byteArray, 0, byteArray.Length);
//    }

//    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
//    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
//    {
//        var responseText = streamReader.ReadToEnd();
//        //Now you have your response.
//        //or false depending on information in the response

//    }

//}
//catch (Exception ex)
//{

//    throw ex;
//}