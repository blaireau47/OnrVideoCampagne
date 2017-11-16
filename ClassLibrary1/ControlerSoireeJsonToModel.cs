using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace ONRVideo
{
    public class ControlerSoireeJsonToModel
    {
        public void RetrieveModelFromJSon(string _soireeJson)
        {

            try
            {

           
            ONRVideo.ONRCampagneVideoEntities onrEntities = new ONRCampagneVideoEntities();


            JObject jsonSoiree = JObject.Parse(_soireeJson);
            ///Recupere campagne et centrale
            

            ///sAVE SOIRÉE
            
            ONRVideo.Soiree curSoiree = new ONRVideo.Soiree();

           

            curSoiree.campagneId = 2;
            curSoiree.central = 0;
            curSoiree.soiree1 = Convert.ToDateTime(jsonSoiree["soiree"].ToString());
            curSoiree.Id = Convert.ToInt32(jsonSoiree["id"]);



            onrEntities.Soirees.Add(curSoiree);
            onrEntities.SaveChanges();

            ///gET sOIRÉE ID
            int soireeID = curSoiree.Id;

                        
            IList<JToken> equipeJSONResults = jsonSoiree["teams"].Children().ToList();
            IList<ONRVideo.Equipe> equipes = new List<ONRVideo.Equipe>();

            foreach (JToken equipe in equipeJSONResults)
            {
                ONRVideo.Equipe curEquipe = equipe.ToObject<ONRVideo.Equipe>();

                curEquipe.SoireeId = soireeID;
                //curSoiree.Equipes.Add(curEquipe);
                onrEntities.Equipes.Add(curEquipe);
                equipes.Add(curEquipe);

            }

            onrEntities.SaveChanges();

            

            IList<JToken> benevolesJSONResults = jsonSoiree["volunteers"].Children().ToList();
            IList<ONRVideo.Volunteer> benevoles = new List<ONRVideo.Volunteer>();

            foreach (JToken benevole in benevolesJSONResults)
            {
                ONRVideo.Volunteer curVolunteer = benevole.ToObject<ONRVideo.Volunteer>();

                var teamID = onrEntities.Equipes.Single(x => x.SoireeId == soireeID && x.teamNumber == curVolunteer.teamNumber).Id;
                curVolunteer.teamID = teamID;
                onrEntities.Volunteers.Add(curVolunteer);
                benevoles.Add(curVolunteer);

            }
            onrEntities.SaveChanges();



            IList<JToken> transportsJSONResults = jsonSoiree["transports"].Children().ToList();
            IList<ONRVideo.Transport> transports = new List<ONRVideo.Transport>();

            foreach (JToken transport in transportsJSONResults)
            {
                ONRVideo.Transport curTransport = transport.ToObject<ONRVideo.Transport>();
                curTransport.teamId = onrEntities.Equipes.Single(x => x.SoireeId == soireeID && x.teamNumber == curTransport.teamNumber).Id;
                onrEntities.Transports.Add(curTransport);
                transports.Add(curTransport);

            }

            onrEntities.SaveChanges();


            }

            catch (SqlException ex)
            {
                ControlerLogger.LogError(_soireeJson, ex);
            }


            catch (Exception ex)
            {

                ControlerLogger.LogError(_soireeJson, ex);
            }


        }

    }
}
