using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace ONRVideo
{
    /// <summary>
    /// Ce controleur permet de recupérer les informations utiliser lors dela génération des videos
    /// </summary>
    public class ControlerInfoSoiree
    {
        private List<ONRVideo.Equipe> equipesInfo;
        private List<ONRVideo.vOrganizationTotaux> organizationsInfo;
        private List<ONRVideo.vSoireesTotaux> soireeInfo;

        public List<Equipe> EquipesInfo { get => equipesInfo; set => equipesInfo = value; }
        public List<vOrganizationTotaux> OrganizationsInfo { get => organizationsInfo; set => organizationsInfo = value; }
        public List<vSoireesTotaux> SoireeInfo { get => soireeInfo; set => soireeInfo = value; }
        public DateTime Soiree { get => soiree; }

        private DateTime soiree;


        public ControlerInfoSoiree(DateTime _soiree)
        {
            

            soiree = _soiree;


            

            ///validate if soiree exit if not throw exception Soiree does not exist


            GetSoireeInformation(soiree);
        }

        private void GetSoireeInformation(DateTime _Soiree)
        {

            try
            {


                ONRVideo.ONRCampagneVideoEntities onrEntities = new ONRCampagneVideoEntities();

                ONRVideo.Soiree laSoiree = (from c in onrEntities.Soirees
                                            where c.soiree1 == _Soiree
                                            select c).First<ONRVideo.Soiree>();

                if (laSoiree is null) { throw new Exception(string.Format("La soiréé {0} n'existe pas. Le ControlerInfoSoiree  Impossible de récupérer les informations", _Soiree)); }

                EquipesInfo = (from c in onrEntities.Equipes
                                where c.SoireeId == laSoiree.Id
                               select c).ToList();

                OrganizationsInfo = (from c in onrEntities.vOrganizationTotauxes
                               where c.SoireeID == laSoiree.Id
                                     select c).ToList();

                SoireeInfo = (from c in onrEntities.vSoireesTotauxes
                                     where c.SoireeId == laSoiree.Id
                              select c).ToList();

                

            }
            catch (SqlException ex)
            {
                ControlerLogger.LogError("GetTeamSoireeInformation", ex);
            }


            catch (Exception ex)
            {

                ControlerLogger.LogError("GetTeamSoireeInformation", ex);
            }


        }

    }
}
