using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace ONRVideo
{
    public class ControlerSendEmailToEquipes
    {
            
        private List<ONRVideo.vEquipesVideoNotSent> teamsVidInfo;
        private ONRVideo.Soiree soiree;
        private ONRVideo.ONRCampagneVideoEntities onrEntities;

        //public List<Equipe> EquipesInfo { get => equipesInfo; set => equipesInfo = value; }
        public List<vEquipesVideoNotSent> TeamsVidInfo { get => teamsVidInfo; set => teamsVidInfo = value; }
        public ONRVideo.Soiree Soiree { get => soiree; set => soiree = value; }
       


        public  ControlerSendEmailToEquipes()
        {
            ONRVideo.ControlerLogger.WriteInformation("Starting to send video to teams that have not received email");
            onrEntities = new ONRCampagneVideoEntities();

            //List<ONRVideo.Soiree> laSoiree = (from c in onrEntities.Soirees
            //                                  where c.soiree1 == _soiree
            //                                  select c).ToList<ONRVideo.Soiree>();
            //ONRVideo.ControlerLogger.WriteInformation("Checked if soiree exist");
            ///Check if Soiree exist
            //if (laSoiree.Count == 0) { throw new Exception(string.Format("La soiréé {0} n'existe pas. Le ControlerSendEmailToEquipes  n'a aucune informaiton pour cette soirée", _soiree)); }

            teamsVidInfo = GetAllTeamsToSendVideo();
        }

        private List<vEquipesVideoNotSent> GetAllTeamsToSendVideo()
        {

            try
            {
                

                teamsVidInfo = (from c in onrEntities.vEquipesVideoNotSents                                           
                                select c).ToList<ONRVideo.vEquipesVideoNotSent>();

            }
            catch (SqlException ex)
            {
                ControlerLogger.LogError("GetTeamSoireeInformation", ex);
            }


            catch (Exception ex)
            {

                ControlerLogger.LogError("GetTeamSoireeInformation", ex);
            }
            return teamsVidInfo;


        }

        public void SendVideoToVolunteers()
        {
            if (this.teamsVidInfo != null)
            {

           

            ControlerMailer ctlMailer = new ControlerMailer();

            foreach (ONRVideo.vEquipesVideoNotSent teamVidInfo in this.teamsVidInfo)
            {

                ///Get volunteers for team
                ONRVideo.ControlerLogger.WriteInformation(string.Format("Sending email to team {0}", teamVidInfo.TeamID));
                List<ONRVideo.Volunteer> teamMembers = (from c in onrEntities.Volunteers
                                                        where c.teamID == teamVidInfo.TeamID
                                          select c).ToList<ONRVideo.Volunteer>();


                ONRVideo.ModelTeamEmail emailInfo = new ModelTeamEmail(teamMembers,teamVidInfo);

                ctlMailer.SendTeamVidEmail(emailInfo);

                ///todo:Update Video that it was sent. Update SentOn column
                ///Get videoinfo for teamid
                VideosEquipe sentVideoInfo = (from c in onrEntities.VideosEquipes
                                                        where c.EquipeId == teamVidInfo.TeamID
                                                        select c).First();
                sentVideoInfo.SentOn = DateTime.Now;

                onrEntities.SaveChanges();
            }
            }
            else
            {

                ONRVideo.ControlerLogger.WriteInformation("No videos where found to be sent to teams.");
            }

        }

    }
}
