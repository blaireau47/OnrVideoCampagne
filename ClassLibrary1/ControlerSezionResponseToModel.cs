using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ONRVideo
{
    public class ControlerSezionResponseToModel
    {

        /// <summary>
        /// Reads the value sent by Sezion
        /// </summary>
        /// <param name="_sezionJSON"> 
        /// EX: Of the JSON structure
        // {
        //    "accountID":"533154e50fa92a6755582c8c",
        //    "description":"Test video for webhoook",
        //    "duration":2000,
        //    "name":"Test video",
        //    "templateID":"533155ec0fa92a6755582c8e",
        //    "outFiles":[
        //      { 
        //        "name":"5331669c0fa92a6755582c9e.MP4_H264_AAC_640x480.mp4",
        //        "profile":"MP4_H264_AAC_640x480_24fps",
        //        "size":27841,
        //        "storage":{
        //          "sezionID":"5331669c0fa92a6755582c9e.MP4_H264_AAC_640x480_24fps"
        //        }
        //}
        //    ],
        //    "status": {"code":"done"},
        //    "date":"2014-03-25T11:21:00.047Z",
        //    "id":"5331669c0fa92a6755582c9e",
        //    "data" : "String passed as data parameter to Video Object when Template_VideoNew was called"
        //  }
        /// </param>
        /// <returns>Bool value if value was successfully saved</returns>
        public bool RetrieveSezionInfo(string _sezionJSON)
        {
            bool retValue = false;
            JObject jsonSezionVidInfo = JObject.Parse(_sezionJSON);
            string videoType = jsonSezionVidInfo["data"].ToString();

            switch (videoType)
            {
                case "Equipe":
                    retValue = SaveVideoEquipe(jsonSezionVidInfo);
                    break;

                default:
                    ControlerLogger.LogErrror(new Exception(string.Format("VideoType received from Seizion is invalaide format",_sezionJSON)));
                    break;
            }

            



            return retValue;

        }

        private bool SaveVideoEquipe(JObject _jsonSezionVidInfo)
        {
            bool retValue = false;
            ONRVideo.ONRCampagneVideoEntities onrEntities = new ONRCampagneVideoEntities();
            ONRVideo.VideosEquipe vidInfoFromSezion = new VideosEquipe();

            ////Retrieve info from Data. Information is not passt ass JSON but as String

            //vidInfoFromSezion.Equipe = _jsonSezionVidInfo["data"].ToString();
            //vidInfoFromSezion.EquipeId = jsonSezionVidInfo["data"].ToString();

            vidInfoFromSezion.dateAdded = DateTime.Now;
            vidInfoFromSezion.VideoUrl = _jsonSezionVidInfo["outFiles"]["Name"].ToString();

            onrEntities.VideosEquipes.Add(vidInfoFromSezion);
            onrEntities.SaveChanges();

            return retValue;
        }
    }
}
