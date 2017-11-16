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
       


        public  ControlerSendEmailToEquipes(DateTime _soiree)
        {

            onrEntities = new ONRCampagneVideoEntities();

            ONRVideo.Soiree soiree = (from c in onrEntities.Soirees
                                        where c.soiree1 == _soiree
                                      select c).First<ONRVideo.Soiree>();

            if (soiree is null) { throw new Exception(string.Format("La soiréé {0} n'existe pas. Le ControlerInfoSoiree  Impossible de récupérer les informations", _soiree)); }

            teamsVidInfo = GetVideosToSend(soiree);
        }

        private List<vEquipesVideoNotSent> GetVideosToSend(ONRVideo.Soiree _Soiree)
        {

            try
            {
                List<ONRVideo.vEquipesVideoNotSent> teamsVidInfo;

                teamsVidInfo = (from c in onrEntities.vEquipesVideoNotSents
                                            where c.Soiree == _Soiree.soiree1
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

            ControlerMailer ctlMailer = new ControlerMailer();

            foreach (ONRVideo.vEquipesVideoNotSent teamVidInfo in this.teamsVidInfo)
            {

                ///Get volunteers for team

                List<ONRVideo.Volunteer> teamMembers = (from c in onrEntities.Volunteers
                                                        where c.teamID == teamVidInfo.TeamID
                                          select c).ToList<ONRVideo.Volunteer>();


                ONRVideo.ModelTeamEmail emailInfo = new ModelTeamEmail(teamMembers,teamVidInfo);

                ctlMailer.SendTeamVidEmail(emailInfo);
            }


        }

    }
}
