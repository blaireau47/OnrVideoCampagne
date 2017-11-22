using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONRVideo
{


    public class ModelEmail
    {
        private string subject;
        private string body;
        private string from;
        private string fromName;
           
       
        public string Subject { get => subject; set => subject = value; }
        public string Body { get => body; set => body = value; }
        public string From { get => from; set => from = value; }
        public string FromName { get => fromName; set => fromName = value; }

        public  ModelEmail()
        {
            
        }
    }

    public class ModelTeamEmail : ModelEmail
    {
        private DateTime soiree;
        private string youtubeVideoID;
        private int teamNumber;
        private string emailTemplateId;
        private List<ONRVideo.Volunteer> teamMembers;

        public DateTime Soiree { get => soiree; set => soiree = value; }
        public string YoutubeVideoID { get => youtubeVideoID; set => youtubeVideoID = value; }
        public int TeamNumber { get => teamNumber; set => teamNumber = value; }
        public string EmailTemplateId { get => emailTemplateId; set => emailTemplateId = value; }
        public List<Volunteer> TeamMembers { get => teamMembers; set => teamMembers = value; }

        public  ModelTeamEmail(List<ONRVideo.Volunteer> _volunteers, vEquipesVideoNotSent _infoVideo)
        {
            TeamMembers = _volunteers;
            //Get other details via configuration
            From = System.Configuration.ConfigurationSettings.AppSettings["FROM_EQUIPEVIDEO_EMAIL"];
            FromName = System.Configuration.ConfigurationSettings.AppSettings["FROM_NAME_EQUIPEVIDEO_EMAIL"];
            Subject = System.Configuration.ConfigurationSettings.AppSettings["SUBJECT_EQUIPEVIDEO_EMAIL"];
            
            EmailTemplateId = System.Configuration.ConfigurationSettings.AppSettings["TEMPLATEID_SENDGRID_EQUIPEVIDEO_EMAIL"];
            
            ///Change tokens in body
            this.Soiree = _infoVideo.Soiree;
            this.TeamNumber = _infoVideo.TeamNumber;
            this.youtubeVideoID = _infoVideo.YoutubeVideoID;

        }


    }
}
