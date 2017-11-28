using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Configuration;


namespace ONRVideo
{

    public class ControlerMailer
    {

        public ControlerMailer()
        {




        }
        private async Task<Response> SendMail(SendGridMessage _msg)
        {

            try
            {

            
            var apiKey = System.Configuration.ConfigurationSettings.AppSettings["SENDGRID_APIKEY"];
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(_msg);
            return response;

            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.TraceError(string.Format("Error when sending  email{0} . Error Information :  {1}, {2}", _msg.HtmlContent, ex.Message, ex.InnerException));
                return null;
            }
        }
        public void SendTeamVidEmail(ONRVideo.ModelTeamEmail _teamModelEmail)
        {
            try
            {

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_teamModelEmail.From, _teamModelEmail.FromName),
                    Subject = _teamModelEmail.Subject
                };
                List<EmailAddress> emailList = new List<EmailAddress>();
                string toNames = string.Empty; //Holds the names that the amil will be adressed to ex: Dear Eric, Mike and Julie

                foreach (Volunteer volunteer in _teamModelEmail.TeamMembers)
                {
                    emailList.Add(new EmailAddress(volunteer.email, string.Format("{0} {1}", volunteer.firstName, volunteer.lastName)));
                    toNames += string.Format("{0}, ", volunteer.firstName);
                }
                msg.AddTos(emailList);

                //msg.AddTo(new EmailAddress("blaireau47@gmail.com", ""));
                msg.SetTemplateId(_teamModelEmail.EmailTemplateId);


                msg.AddSubstitution("ToNames", toNames);
                msg.AddSubstitution("TeamNumber", _teamModelEmail.TeamNumber.ToString());
                msg.AddSubstitution("SoireeDate", _teamModelEmail.Soiree.ToLongDateString());
                msg.AddSubstitution("YoutubeID", _teamModelEmail.YoutubeVideoID);


                msg.SetClickTracking(true, true);

                SendMail(msg).Wait();


            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.TraceError(string.Format("Error when creating email  for team  {0}. Error Information :  {1}, {2}", _teamModelEmail.TeamNumber , ex.Message, ex.InnerException));
            }
        }

        public void SendErrorLogMessage(string _subject, string _FullErrorMessage)
        {

            try
            {
                
                var errorTemplateSendGridId = System.Configuration.ConfigurationSettings.AppSettings["SENDGRID_ERRORTEMPLATEID"];
               
                var fromError = System.Configuration.ConfigurationSettings.AppSettings["SENDGRID_ERROR_EMAILFROM"];

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(fromError, "ONR Video Error Team"),
                    Subject = _subject
                };
                var toError = System.Configuration.ConfigurationSettings.AppSettings["SENDGRID_ERROR_EMAILTO"];
                msg.AddTo(new EmailAddress("blaireau47@gmail.com", ""));
                msg.SetTemplateId(errorTemplateSendGridId);
                msg.AddSubstitution("FullErrorMessage", _FullErrorMessage);


                SendMail(msg).Wait();

                
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.TraceError(string.Format("Error when creating log error emailEmail {0} . Error Information :  {1}, {2}", _subject, ex.Message, ex.InnerException));
            }
            //writeMessage(_message, LogType.Message);
        }


       
    }
}
