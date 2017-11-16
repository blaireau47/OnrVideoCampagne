using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace WH.Sezion.NewVideo
{
    public static class ReceiveNewSezionVideo
    {
        [FunctionName("ReceiveNewSezionVideo")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            
            log.Info($"Webhook was triggered again!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            //dynamic data = JsonConvert.DeserializeObject(jsonContent);
            ONRVideo.ControlerLogger.WriteInformation("sent data = " + jsonContent);
            



            //if (data.first == null || data.last == null)
            //{
            //    return req.CreateResponse(HttpStatusCode.BadRequest, new
            //    {
            //        error = "Please pass first/last properties in the input object"

            //    });
            //
            ONRVideo.ControlerSezionResponseSaveVideo objCtlSaveSezion = new ONRVideo.ControlerSezionResponseSaveVideo();
            objCtlSaveSezion.RetrieveSezionInfo(jsonContent);

            ///Send a empty response ok to sezion. Sezion only waits for a Status OK
            return req.CreateResponse(HttpStatusCode.OK, new
            {


            });
        }


       
    }
}
