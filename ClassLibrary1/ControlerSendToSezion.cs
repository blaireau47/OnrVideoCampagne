using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ONRVideo
{
    public class ControlerSendToSezion
    {

        /// <summary>
        /// Reads the value sent by Sezion
        /// </summary>
        /// <param name="_sezionJSON"> 
        /// EX: Of the JSON structure
        // 
//         {  
//           "accountID":"573edfa366b9071b5246c037",
//           "credits":37,
//           "data":"equipeID|23",
//           "description":"une super description incroyable",
//           "duration":36650,
//           "keywords":"Key word baby",
//           "name":"Test de data recue",
//           "templateID":"583cf4c5ec5d8ed49ba96a2c",
//           "formData":[

//           ],
//           "analytics":{  
//              "playDetails":[

//              ]
//          },
//           "playerInfo":{  
//              "CTAForm":{  
//                 "inputFields":[

//                 ]
//},
//              "CTAAllowClose":true,
//              "CTAShowAtEnd":true,
//              "CTAPauseOnShow":true,
//              "CTAShowOnPause":true,
//              "CTAOpenOnClick":true,
//              "CTAType":"text"
//           },
//           "outFiles":[
//              {  
//                 "name":"59fc95ce3f6b88f56e7a3e31.MP4_H264_AAC_852x480.mp4",
//                 "profile":"MP4_H264_AAC_852x480_24fps",
//                 "size":6691296,
//                 "storage":{  
//                    "youtubeID":"3iu9xbfnk3w",
//                    "youtubeAccountID":"113601026647381656544"
//                 }
//              }
//           ],
//           "status":{  
//              "code":"done"
//           },
//           "date":"2017-11-03T16:14:06.612Z",
//           "id":"59fc95ce3f6b88f56e7a3e31"
//        }
        /// </param>
        /// <returns>Bool value if value was successfully saved</returns>
        public bool RetrieveSezionInfo(string _sezionJSON)
        {
            bool retValue = false;
            JObject jsonSezionVidInfo = JObject.Parse(_sezionJSON);
            //string videoType = jsonSezionVidInfo["videoType"].ToString();

            string videoType = "Equipe";

            switch (videoType)
            {
                case "Equipe":
                    retValue = SaveVideoEquipe(jsonSezionVidInfo);
                    break;

                default:
                    ControlerLogger.LogError(_sezionJSON, new Exception(string.Format("VideoType received from Seizion is invalid format or empty")));
                    break;
            }





            return retValue;

        }

        private bool SaveVideoEquipe(JObject _jsonSezionVidInfo)
        {
            try
            {

            
            bool retValue = false;
            ONRVideo.ONRCampagneVideoEntities onrEntities = new ONRCampagneVideoEntities();
            ONRVideo.VideosEquipe vidInfoFromSezion = new VideosEquipe();

            ////Retrieve info from Data. Information is  pass in JSON propertie as a list of Key Value pair

            //vidInfoFromSezion.Equipe = _jsonSezionVidInfo["data"].ToString();
            string[] equipeIdInfo = _jsonSezionVidInfo["data"].ToString().Split('|');
            int intEquipeId;
            if (equipeIdInfo[0] == "equipeID" & int.TryParse(equipeIdInfo[1], out intEquipeId))
            {
                vidInfoFromSezion.EquipeId = intEquipeId;
            }
            else throw new Exception("Erreur lors du retour de Sezion Equipeid INVALID OU INTROUVABLE", new Exception(_jsonSezionVidInfo.ToString()));
            

            vidInfoFromSezion.dateAdded = DateTime.Now;
            vidInfoFromSezion.VideoUrl = _jsonSezionVidInfo.SelectToken("outFiles[0].storage.youtubeID").ToString();

            onrEntities.VideosEquipes.Add(vidInfoFromSezion);
            onrEntities.SaveChanges();

                return retValue;

            }
            catch (Exception ex)
            {

                ONRVideo.ControlerLogger.LogError("erreur lors de la sauvegarde du video", ex);

                return false;
            }


        }

    }
}
        //private string testData = "{\"accountID\":\"573edfa366b9071b5246c037\",\"credits\":37,\"data\":\"equipeID|23\",\"description\":\"une super description incroyable\",\"duration\":36650,\"keywords\":\"Key word baby\",\"name\":\"Test de data recue\",\"templateID\":\"583cf4c5ec5d8ed49ba96a2c\",\"formData\":[],\"analytics\":{\"playDetails\":[]},\"playerInfo\":{\"CTAForm\":{\"inputFields\":[]},\"CTAAllowClose\":true,\"CTAShowAtEnd\":true,\"CTAPauseOnShow\":true,\"CTAShowOnPause\":true,\"CTAOpenOnClick\":true,\"CTAType\":\"text\"},\"outFiles\":[{\"name\":\"59fc95ce3f6b88f56e7a3e31.MP4_H264_AAC_852x480.mp4\",\"profile\":\"MP4_H264_AAC_852x480_24fps\",\"size\":6691296,\"storage\":{\"youtubeID\":\"3iu9xbfnk3w\",\"youtubeAccountID\":\"113601026647381656544\"}}],\"status\":{\"code\":\"done\"},\"date\":\"2017-11-03T16:14:06.612Z\",\"id\":\"59fc95ce3f6b88f56e7a3e31\"}";
