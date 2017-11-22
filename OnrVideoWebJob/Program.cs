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
                        ONRVideo.ControlerLogger.WriteInformation(String.Format("Soiree doies not exist , retrieving current soiree {0} and saving to DB", soiree));
                        string soireJSON = SaveONRSoireeFromJSon("http://onrvideo.azurewebsites.net/soirees/soiree2281.txt");  //("https://eblais_test:1234qwer@onr-pilote.com/app/api/eveningStatistics?eveningId=2281");
                    }

                }






            }
            catch (Exception ex)
            {
                ONRVideo.ControlerLogger.LogError("WebJob GetSjon Error", ex);
            }


            try
            {
                ///Envoyer les informations à Sezion. Identifier ceux envoyé pour éviter d'envoye plus d'une foi.
            }
            catch (Exception ex)
            {
                ONRVideo.ControlerLogger.LogError("WebJob GetSjon Error", ex);
            }



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
