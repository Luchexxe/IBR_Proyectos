using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using wsUPCServices.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;



namespace wsUPCServices.Controllers
{




    public class APIController : ApiController
    {




        [HttpPost]
        [Route("DownloadRecordings_Segementado")]
        public CallHistoryOut ObtenerCH(CallHistoryIN _request)
        {
            CallHistoryOut _response = new CallHistoryOut();
            List<string> audiosogg = new List<string>();
            //Dictionary<string,Dictionary<string,string>> audiosogg = new Dictionary<string, Dictionary<string, string>>();
            int? Counter = 0;
            int HorasGMT = Int32.Parse(ConfigurationManager.AppSettings["HoraGMT"]);
            var x = "";

            WriteLog.EscribirLog("Ejecutando Intervalo: " + _request.startTime + "/" + _request.endTime);
            #region Autenticacion
            string clientId = ConfigurationManager.AppSettings["clientID"];
            string clientSecret = ConfigurationManager.AppSettings["clientPass"];

            //Set Region
            PureCloudRegionHosts region = PureCloudRegionHosts.us_east_1;
            PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath(region);

            // Configure SDK Settings
            var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(clientId, clientSecret);
            PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = accessTokenInfo.AccessToken;
            #endregion
            // Create API instances
            var conversationsApi = new ConversationsApi();
            var recordingApi = new RecordingApi();
            var outboundApi = new OutboundApi();
            //List<BatchDownloadRequest> batchDownloadRequestList = new List<BatchDownloadRequest>();
            //BatchDownloadJobSubmission batchRequestBody = new BatchDownloadJobSubmission();

            DateTime HoraInicio = DateTime.ParseExact(_request.startTime, "yyyy-MM-ddTHH:mm:ss", null);
            DateTime HoraInicioBig = HoraInicio.AddDays(-7);
            DateTime HoraFin = DateTime.ParseExact(_request.endTime, "yyyy-MM-ddTHH:mm:ss", null);

            string dates = HoraInicioBig.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss");



            WriteLog.EscribirLog("Intervalo: " + dates + " || Inicio de proceso");
            //BatchDownloadJobStatusResult completedBatchStatus = new BatchDownloadJobStatusResult();

            // Process and build the request for downloading the recordings
            // Get the conversations within the date interval and start adding them to batch request
            int iPageIndex = 1;
            int iPageSize = 80;
            List<SegmentDetailQueryFilter> oSegmentDetailQuery = new List<SegmentDetailQueryFilter>();
            //List<ConversationDetailQueryFilter> oConversationFilter = new List<ConversationDetailQueryFilter>();

            List<SegmentDetailQueryPredicate> oSegmentDetailQueryPredicate = new List<SegmentDetailQueryPredicate>();
            List<SegmentDetailQueryPredicate> oSegmentDetailQueryPredicate2 = new List<SegmentDetailQueryPredicate>();
            List<SegmentDetailQueryClause> oSegmentDetailQueryClause = new List<SegmentDetailQueryClause>();


            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "be66ad76-4270-441c-b27a-31a9d5159c33"

            }
            );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "283b4aa2-aa7f-4c2f-8eb0-b6529f60c24d"

            }
            );
            /// new

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "ce1959bb-f550-4e9e-a3ba-2adc4b95b99e"

            }
           );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "f0e3eb82-a0a8-48cc-894c-0d7fffd78509"

            }
           );

            //new 270422

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "41ea5d8c-33cf-4d69-b555-f740f2ce946f"

            }
           );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "6a2549ed-673b-4c28-bedf-97519e1eeac5"

            }
           );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "23138a71-2ec6-41b6-bc7d-c81980a4b03f"

            }
);
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "12c1747c-c51b-4a7d-ae2a-084032b93eaf"

            }
);
            //NEW 12052022
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "208b73d3-bddc-4952-8168-0067f7f032cf"

            }
);
            //NEW 26072022
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "264b096d-6639-443d-a40e-a0d6308bc7d8"

            }
);
            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmentend,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = HoraInicio.ToString("yyyy-MM-ddTHH:mm:ss") + ".000Z/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss" + ".000Z")

            }
           );

            oSegmentDetailQueryClause.Add(new SegmentDetailQueryClause()
            {
                Type = SegmentDetailQueryClause.TypeEnum.Or,
                Predicates = oSegmentDetailQueryPredicate2

            }


                );

            oSegmentDetailQuery.Add(new SegmentDetailQueryFilter()
            {
                Type = SegmentDetailQueryFilter.TypeEnum.And,
                Predicates = oSegmentDetailQueryPredicate,
                Clauses = oSegmentDetailQueryClause

            }
             );


            Dictionary<int, string> ListIdInteracion = new Dictionary<int, string>()
                {


{0,"be030e80-ab3b-4e58-bd7b-3d8ab5e65225"},
{1,"9226673f-df46-4b87-9c7b-02874a6a3176"},
{2,"c2b5c269-8b06-4f01-96e8-be42bafd05f7"},
{3,"d9ff6b89-26ec-4419-b1d0-4693e8ce2ad2"},
{4,"628a8bac-ac99-4b3f-9646-94033e5d356a"},
{5,"c8c1f9a4-d7c3-4ac4-9baf-18bf68c65bd6"},
{6,"f7e86a78-167c-43dc-8d40-dd5b73e13a12"},
{7,"abde389e-3e29-41ca-80ef-32f3b68614c7"},
{8,"06dd4c02-11d3-4869-b03f-5fe14c211508"},
{9,"5fc91f37-ba12-48d3-9353-6a08545fa21f"},
{10,"b1ca6671-70be-4be2-b3a6-389a6b5037af"},
{11,"01d11809-0eee-4ed0-a032-07e005777e66"},
{12,"b9999ce5-ff9b-4908-a3e0-c87788a49f25"},
{13,"f39b14a3-f7b1-46d2-9396-e43341d45152"},
{14,"967e77c6-72ad-4234-ae2c-cb0f4b99593f"},
{15,"3d8d5365-f5f6-4bca-8027-2aaea9b342b6"},
{16,"20ab8afe-abb6-4b15-8eb7-d8a124be432e"},
{17,"98859dd1-dcec-4312-ae2e-93a5946cbc1f"},
{18,"797b09c4-c745-4150-8df0-0065debfef15"},
{19,"ad02374f-20b0-4ee1-b63f-c2945ef5727e"},
{20,"b3762f11-c108-4ff4-9423-ecdeebafb60a"},
{21,"30dbf114-e2d1-484f-a37e-d5f911bfcc34"}


            };


            OperacionSQL operacionesSql = new OperacionSQL();
            while (iPageSize != 0)
            {
                var Paginacion = new PureCloudPlatform.Client.V2.Model.PagingSpec(iPageSize, iPageIndex);
                AnalyticsConversationQueryResponse conversationDetails = conversationsApi.PostAnalyticsConversationsDetailsQuery(new ConversationQuery(Interval: dates, SegmentFilters: oSegmentDetailQuery, Paging: Paginacion));
                Counter = conversationDetails.TotalHits;

                Dictionary<string, string> Listrecordings = new Dictionary<string, string>();

               

             


                WriteLog.EscribirLog("Intervalo: " + dates + " || Pagina: " + iPageIndex + " || Interacciones: " + Counter);
                if (Counter == 0)
                {
                    break;
                }
               
                foreach (var conversations in conversationDetails.Conversations)
                    {

                    foreach (KeyValuePair<int,string> par in ListIdInteracion)
                    {
                        if ((conversations.ConversationId == par.Value))
                        {

                            var y = "";
                        }
                        else
                        {
                            continue;
                        }
                   

                        foreach (var participants in conversations.Participants)
                        {

                            if (participants.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Agent)
                            {

                                foreach (var Sessions in participants.Sessions)

                                {
                                    var peerID = Sessions.PeerId;
                                    if (Sessions.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                    {
                                        foreach (var segment in Sessions.Segments)
                                        {
                                            if (segment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                            {
                                                if (segment.WrapUpCode == "be66ad76-4270-441c-b27a-31a9d5159c33" || segment.WrapUpCode == "283b4aa2-aa7f-4c2f-8eb0-b6529f60c24d" || segment.WrapUpCode == "ce1959bb-f550-4e9e-a3ba-2adc4b95b99e" || segment.WrapUpCode == "f0e3eb82-a0a8-48cc-894c-0d7fffd78509" || segment.WrapUpCode == "41ea5d8c-33cf-4d69-b555-f740f2ce946f" || segment.WrapUpCode == "6a2549ed-673b-4c28-bedf-97519e1eeac5" || segment.WrapUpCode == "23138a71-2ec6-41b6-bc7d-c81980a4b03f" || segment.WrapUpCode == "12c1747c-c51b-4a7d-ae2a-084032b93eaf" || segment.WrapUpCode == "208b73d3-bddc-4952-8168-0067f7f032cf" || segment.WrapUpCode == "264b096d-6639-443d-a40e-a0d6308bc7d8")
                                                {
                                                    if (segment.SegmentEnd >= HoraInicio && segment.SegmentEnd < HoraFin)
                                                    {

                                                        List<RecordingMetadata> recordingsData = recordingApi.GetConversationRecordingmetadata(conversations.ConversationId);

                                                        //   Recording recordingsData = recordingApi.GetConversationRecording(conversationId);
                                                        // Iterate through every result, check if there are one or more recordingIds in every conversation
                                                        foreach (var recording in recordingsData)
                                                        {

                                                            if (peerID == recording.SessionId)
                                                            {
                                                                Listrecordings.Add(recording.Id, recording.ConversationId);

                                                                WriteLog.EscribirLog("Intervalo: " + dates + " || Added: " + recording.ConversationId);

                                                            }


                                                            else
                                                            {

                                                                continue;


                                                            }

                                                        }

                                                    }
                                                    else
                                                    {

                                                        continue;

                                                    }

                                                }
                                                else
                                                {
                                                    continue;
                                                }


                                            }
                                            else
                                            {
                                                continue;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        peerID = "";
                                        continue;
                                    }
                                    peerID = "";
                                }

                            }
                            else
                            {
                                continue;
                            }

                        }
                    }
                }
                
              


                int conteo = 0;
                while (Listrecordings.Count > conteo)
                {

                    BatchDownloadJobSubmission batchRequestBody = new BatchDownloadJobSubmission();
                    List<BatchDownloadRequest> batchDownloadRequestList = new List<BatchDownloadRequest>();
                    var lista = Listrecordings.Skip(conteo).Take(100);
                    foreach (KeyValuePair<string, string> kvp in lista)
                    {
                        BatchDownloadRequest batchRequest = new BatchDownloadRequest();
                        batchRequest.ConversationId = kvp.Value;
                        batchRequest.RecordingId = kvp.Key;
                        batchDownloadRequestList.Add(batchRequest);
                        batchRequestBody.BatchDownloadRequestList = batchDownloadRequestList;

                    }

                    BatchDownloadJobSubmissionResult result = recordingApi.PostRecordingBatchrequests(batchRequestBody);

                    BatchDownloadJobStatusResult result2 = recordingApi.GetRecordingBatchrequest(result.Id);

                  

                    while (result2.ExpectedResultCount != result2.ResultCount)
                    {
                        result2 = recordingApi.GetRecordingBatchrequest(result.Id);
                        WriteLog.EscribirLog("Intervalo: " + dates + " ||Batch Result Status: " + result2.ResultCount + " / " + result2.ExpectedResultCount);

                        

                        // Simple polling through recursion
                        Thread.Sleep(5000);

                    }


                    //completedBatchStatus = getRecordingStatus(result);

                    // Start downloading the recording files individually
                    foreach (var recording in result2.Results)
                    {
                        string RUT = string.Empty;
                        string DV = string.Empty;
                        object RUT2;
                        object DV2;
                        string nombreCola = string.Empty;
                        string codRatif = "99";
                        if (recording.ResultUrl == null || recording.ResultUrl == String.Empty)
                        {
                            WriteLog.EscribirLog("Intervalo: " + dates + " || Conversationid: " + recording.ConversationId + " || Recordingid: " + recording.RecordingId + " || No tiene grabacion");
                        }
                        else
                        {
                            RecordingMetadata result3 = recordingApi.GetConversationRecordingmetadataRecordingId(recording.ConversationId, recording.RecordingId);
                            DateTime FechaInicioRec = HoraInicio;

                            if (result3.StartTime != null)
                            {
                                FechaInicioRec = DateTime.ParseExact(result3.StartTime.Substring(0, 19), "yyyy-MM-ddTHH:mm:ss", null);
                                
                            }

                            FechaInicioRec = FechaInicioRec.AddHours(HorasGMT);
                            //FechaInicioRec = FechaInicioRec.AddHours(-4);



                            //String fechaInicio = FechaInicioRec.ToString().Substring(6, 4) + FechaInicioRec.ToString().Substring(3, 2) + FechaInicioRec.ToString().Substring(0, 2);
                            String fechaInicio = FechaInicioRec.ToString("yyyyMMdd");

                            //String HoraInicio2 = FechaInicioRec.ToString().Substring(11, 2) + FechaInicioRec.ToString().Substring(14, 2);
                            String HoraInicio2 = FechaInicioRec.ToString("HHmm");
                            Conversation result4 = conversationsApi.GetConversation(recording.ConversationId);

                            foreach (Participant oparticipant in result4.Participants)

                            {

                                string idcontacto;
                                string contactlistID;
                                string[] separator = new string[] { "-" };
                                string[] ArrayRes;
                                string[] ArrayQueue;
                                string aux119 = string.Empty;
                               
                                if (oparticipant.Purpose == "customer")
                                {
                                    if (oparticipant.Attributes.TryGetValue("dialerContactId", out idcontacto) && oparticipant.Attributes.TryGetValue("dialerContactListId", out contactlistID))
                                    {
                                        DialerContact resultContacto = outboundApi.GetOutboundContactlistContact(contactlistID, idcontacto);
                                        if (resultContacto.Data.TryGetValue("RUT", out RUT2))
                                        {
                                            RUT = RUT2.ToString();
                                        }
                                        if (resultContacto.Data.TryGetValue("DV", out DV2))
                                        {
                                            DV = DV2.ToString();
                                        }
                                    }

                                    ArrayRes = RUT.Split(separator, StringSplitOptions.None);

                                    if (ArrayRes[0].Length == 7)
                                    {
                                        RUT = "0" + RUT;
                                    }

                                    nombreCola = oparticipant.QueueName;
                                    ArrayQueue = oparticipant.QueueName.Split(separator, StringSplitOptions.None);

                                    nombreCola = ArrayQueue[0];


                                    //break;
                                }
                                else if (oparticipant.Purpose == "agent")
                                {      
                                    if (oparticipant.WrapupRequired==true) {
                                        if (oparticipant.Wrapup.Code == "be66ad76-4270-441c-b27a-31a9d5159c33" || oparticipant.Wrapup.Code == "283b4aa2-aa7f-4c2f-8eb0-b6529f60c24d" || oparticipant.Wrapup.Code == "ce1959bb-f550-4e9e-a3ba-2adc4b95b99e" || oparticipant.Wrapup.Code == "f0e3eb82-a0a8-48cc-894c-0d7fffd78509"  || oparticipant.Wrapup.Code == "41ea5d8c-33cf-4d69-b555-f740f2ce946f" || oparticipant.Wrapup.Code == "6a2549ed-673b-4c28-bedf-97519e1eeac5" || oparticipant.Wrapup.Code == "23138a71-2ec6-41b6-bc7d-c81980a4b03f" || oparticipant.Wrapup.Code == "12c1747c-c51b-4a7d-ae2a-084032b93eaf" || oparticipant.Wrapup.Code == "208b73d3-bddc-4952-8168-0067f7f032cf" || oparticipant.Wrapup.Code == "264b096d-6639-443d-a40e-a0d6308bc7d8")
                                        {
                                            if (oparticipant.Attributes.TryGetValue("vAux119", out aux119))
                                            {
                                                if (aux119 != "")
                                                {
                                                    codRatif = aux119;
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            String targetDirectory = ConfigurationManager.AppSettings["ruta"]; ;

                            // If there is an errorMsg skip the recording download
                            if (recording.ErrorMsg != null)
                            {
                                WriteLog.EscribirLog("Intervalo: " + dates + " || Error: " + recording.ErrorMsg);

                            }

                            string contentType = recording.ContentType;

                            // Split the text and gets the extension that will be used for the recording
                            string ext = contentType.Split('/').Last();

                            // For the JSON special case
                            if (ext.Length >= 4)
                            {
                                ext = ext.Substring(0, 4);
                            }



                            string filename = fechaInicio + HoraInicio2 + RUT + DV + "T603903T" + nombreCola + "0050" + codRatif + recording.ConversationId;

                            //audiosogg.Add(filename, new Dictionary<string, string>());
                            //for (var w = 0; w <= 5; w++)
                            //{
                            //    switch(w)
                            //    {
                            //    case 0:
                            //    audiosogg[filename].Add("ID_Interaccion", recording.ConversationId);
                            //    continue;
                            //    case 1:
                            //    audiosogg[filename].Add("recordingDate", FechaInicioRec.ToString("yyyy-MM-dd HH:mm:ss"));
                            //    continue;
                            //    case 2:
                            //    audiosogg[filename].Add("RecordingID", recording.Id);
                            //    continue;
                            //    case 3:
                            //    audiosogg[filename].Add("ParticipantID", recording.ConversationId);
                            //    continue;



                            //    }





                            //}


                            audiosogg.Add(filename);
                            WriteLog.EscribirLog("Intervalo: " + dates + " || Grabacion : " + fechaInicio + HoraInicio2 + RUT + DV + "T603903T" + nombreCola + "0050" + codRatif + recording.ConversationId);

                            using (WebClient wc = new WebClient())
                                wc.DownloadFile(recording.ResultUrl, targetDirectory + "\\" + filename + "." + ext);
                        }







                    }


                    conteo += 100;
                }


                // Send a batch request and start polling for updates


                if (conversationDetails.Conversations.Count < iPageSize)
                {

                    break;

                }


                else
                {
                    iPageIndex++;
                    //   Thread.Sleep(5000);
                    continue;
                }


            }


            if (Counter > 0)
            {
                foreach (var audioOgg in audiosogg)
                {
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();
                   
                    //cmd.StandardInput.WriteLine("C:/Users/reyes/Desktop/ffmpeg/ffmpeg/bin/ffmpeg -i C:/Users/reyes/Desktop/IBRProyecto/wsIBR/Grabaciones/"+ oggrecording + ".ogg -codec:a libmp3lame -qscale:a 2 -b:a 320000 C:/Users/reyes/Desktop/IBRProyecto/wsIBR/Grabacionesmp3/"+ oggrecording + ".mp3");
                    cmd.StandardInput.WriteLine(ConfigurationManager.AppSettings["rutapp"] + " -i " + ConfigurationManager.AppSettings["rutaogg"] + audioOgg + ".ogg -codec:a libmp3lame -qscale:a 2 -b:a 320000 " + ConfigurationManager.AppSettings["rutamp3"] + audioOgg + ".mp3");
                    x = operacionesSql.SPInsertCH(IDInteraccion: audioOgg.Substring(audioOgg.Length-36), RecordingDate: "", RecordingID: "", ParticipantID: "", PeerID: "", ReceordingMP3: audioOgg);
                   cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                }
            }

            WriteLog.EscribirLog("Intervalo: " + dates + " || Estado: Finalizado" + " || total: " + Counter);

            _response.Estado = "Finalizado";
            _response.Fecha = DateTime.Now;
            _response.Contador = Counter;
            return _response;

        }

        [HttpPost]
        [Route("DownloadRecordings")]
        public CallHistoryOut ObtenerCHv2(CallHistoryIN _request)
        {
            CallHistoryOut _response = new CallHistoryOut();
            List<string> audiosogg = new List<string>();
            //Dictionary<string,Dictionary<string,string>> audiosogg = new Dictionary<string, Dictionary<string, string>>();
            int? Counter = 0;
            int HorasGMT = Int32.Parse(ConfigurationManager.AppSettings["HoraGMT"]);
            var x = "";

            WriteLog.EscribirLog("Ejecutando Intervalo: " + _request.startTime + "/" + _request.endTime);
            #region Autenticacion
            string clientId = ConfigurationManager.AppSettings["clientID"];
            string clientSecret = ConfigurationManager.AppSettings["clientPass"];

            //Set Region
            PureCloudRegionHosts region = PureCloudRegionHosts.us_east_1;
            PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath(region);

            // Configure SDK Settings
            var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(clientId, clientSecret);
            PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = accessTokenInfo.AccessToken;
            #endregion
            // Create API instances
            var conversationsApi = new ConversationsApi();
            var recordingApi = new RecordingApi();
            var outboundApi = new OutboundApi();
            //List<BatchDownloadRequest> batchDownloadRequestList = new List<BatchDownloadRequest>();
            //BatchDownloadJobSubmission batchRequestBody = new BatchDownloadJobSubmission();

            DateTime HoraInicio = DateTime.ParseExact(_request.startTime, "yyyy-MM-ddTHH:mm:ss", null);
            DateTime HoraInicioBig = HoraInicio.AddDays(-7);
            DateTime HoraFin = DateTime.ParseExact(_request.endTime, "yyyy-MM-ddTHH:mm:ss", null);

            string dates = HoraInicioBig.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss");



            WriteLog.EscribirLog("Intervalo: " + dates + " || Inicio de proceso");
            //BatchDownloadJobStatusResult completedBatchStatus = new BatchDownloadJobStatusResult();

            // Process and build the request for downloading the recordings
            // Get the conversations within the date interval and start adding them to batch request
            int iPageIndex = 1;
            int iPageSize = 80;
            List<SegmentDetailQueryFilter> oSegmentDetailQuery = new List<SegmentDetailQueryFilter>();
            //List<ConversationDetailQueryFilter> oConversationFilter = new List<ConversationDetailQueryFilter>();

            List<SegmentDetailQueryPredicate> oSegmentDetailQueryPredicate = new List<SegmentDetailQueryPredicate>();
            List<SegmentDetailQueryPredicate> oSegmentDetailQueryPredicate2 = new List<SegmentDetailQueryPredicate>();
            List<SegmentDetailQueryClause> oSegmentDetailQueryClause = new List<SegmentDetailQueryClause>();


            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "be66ad76-4270-441c-b27a-31a9d5159c33"

            }
            );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "283b4aa2-aa7f-4c2f-8eb0-b6529f60c24d"

            }
            /// new
            );
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "ce1959bb-f550-4e9e-a3ba-2adc4b95b99e"

            }
           );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "f0e3eb82-a0a8-48cc-894c-0d7fffd78509"

            }
           );
            //new 270422

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "41ea5d8c-33cf-4d69-b555-f740f2ce946f"

            }
           );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "6a2549ed-673b-4c28-bedf-97519e1eeac5"

            }
           );

            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "23138a71-2ec6-41b6-bc7d-c81980a4b03f"

            }
);
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "12c1747c-c51b-4a7d-ae2a-084032b93eaf"

            }
);

            //NEW 12052022
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "208b73d3-bddc-4952-8168-0067f7f032cf"

            }
);
            //NEW 26072022
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Wrapupcode,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "264b096d-6639-443d-a40e-a0d6308bc7d8"

            }
);
            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmentend,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = HoraInicio.ToString("yyyy-MM-ddTHH:mm:ss") + ".000Z/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss" + ".000Z")

            }
           );

            oSegmentDetailQueryClause.Add(new SegmentDetailQueryClause()
            {
                Type = SegmentDetailQueryClause.TypeEnum.Or,
                Predicates = oSegmentDetailQueryPredicate2

            }


                );

            oSegmentDetailQuery.Add(new SegmentDetailQueryFilter()
            {
                Type = SegmentDetailQueryFilter.TypeEnum.And,
                Predicates = oSegmentDetailQueryPredicate,
                Clauses = oSegmentDetailQueryClause

            }
             );

       
            OperacionSQL operacionesSql = new OperacionSQL();
            while (iPageSize != 0)
            {
                var Paginacion = new PureCloudPlatform.Client.V2.Model.PagingSpec(iPageSize, iPageIndex);
                AnalyticsConversationQueryResponse conversationDetails = conversationsApi.PostAnalyticsConversationsDetailsQuery(new ConversationQuery(Interval: dates, SegmentFilters: oSegmentDetailQuery, Paging: Paginacion));
                Counter = conversationDetails.TotalHits;

                Dictionary<string, string> Listrecordings = new Dictionary<string, string>();






                WriteLog.EscribirLog("Intervalo: " + dates + " || Pagina: " + iPageIndex + " || Interacciones: " + Counter);
                if (Counter == 0)
                {
                    break;
                }

                foreach (var conversations in conversationDetails.Conversations)
                {


                    //if ((conversations.ConversationId == "8c81328d-4efc-4f2b-9543-ebea7c8f06c2"))
                    //{

                    //    var y = "";
                    //}
                    //else
                    //{
                    //    continue;
                    //}

                    foreach (var participants in conversations.Participants)
                        {

                            if (participants.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Agent)
                            {

                                foreach (var Sessions in participants.Sessions)

                                {
                                    var peerID = Sessions.PeerId;
                                    if (Sessions.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                    {
                                        foreach (var segment in Sessions.Segments)
                                        {
                                            if (segment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                            {
                                                 if (segment.WrapUpCode == "be66ad76-4270-441c-b27a-31a9d5159c33" || segment.WrapUpCode == "283b4aa2-aa7f-4c2f-8eb0-b6529f60c24d" || segment.WrapUpCode == "ce1959bb-f550-4e9e-a3ba-2adc4b95b99e" || segment.WrapUpCode == "f0e3eb82-a0a8-48cc-894c-0d7fffd78509" || segment.WrapUpCode == "41ea5d8c-33cf-4d69-b555-f740f2ce946f" || segment.WrapUpCode == "6a2549ed-673b-4c28-bedf-97519e1eeac5" || segment.WrapUpCode == "23138a71-2ec6-41b6-bc7d-c81980a4b03f" || segment.WrapUpCode == "12c1747c-c51b-4a7d-ae2a-084032b93eaf" || segment.WrapUpCode == "208b73d3-bddc-4952-8168-0067f7f032cf" || segment.WrapUpCode == "264b096d-6639-443d-a40e-a0d6308bc7d8")
                                                 {
                                                    if (segment.SegmentEnd >= HoraInicio && segment.SegmentEnd < HoraFin)
                                                    {

                                                        List<RecordingMetadata> recordingsData = recordingApi.GetConversationRecordingmetadata(conversations.ConversationId);

                                                        //   Recording recordingsData = recordingApi.GetConversationRecording(conversationId);
                                                        // Iterate through every result, check if there are one or more recordingIds in every conversation
                                                        foreach (var recording in recordingsData)
                                                        {

                                                            if (peerID == recording.SessionId)
                                                            {
                                                                Listrecordings.Add(recording.Id, recording.ConversationId);

                                                                WriteLog.EscribirLog("Intervalo: " + dates + " || Added: " + recording.ConversationId);

                                                            }


                                                            else
                                                            {

                                                                continue;


                                                            }

                                                        }

                                                    }
                                                    else
                                                    {

                                                        continue;

                                                    }

                                                }
                                                else
                                                {
                                                    continue;
                                                }


                                            }
                                            else
                                            {
                                                continue;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        peerID = "";
                                        continue;
                                    }
                                    peerID = "";
                                }

                            }
                            else
                            {
                                continue;
                            }

                        }
                    
                }




                int conteo = 0;
                while (Listrecordings.Count > conteo)
                {

                    BatchDownloadJobSubmission batchRequestBody = new BatchDownloadJobSubmission();
                    List<BatchDownloadRequest> batchDownloadRequestList = new List<BatchDownloadRequest>();
                    var lista = Listrecordings.Skip(conteo).Take(100);
                    foreach (KeyValuePair<string, string> kvp in lista)
                    {
                        BatchDownloadRequest batchRequest = new BatchDownloadRequest();
                        batchRequest.ConversationId = kvp.Value;
                        batchRequest.RecordingId = kvp.Key;
                        batchDownloadRequestList.Add(batchRequest);
                        batchRequestBody.BatchDownloadRequestList = batchDownloadRequestList;

                    }

                    BatchDownloadJobSubmissionResult result = recordingApi.PostRecordingBatchrequests(batchRequestBody);

                    BatchDownloadJobStatusResult result2 = recordingApi.GetRecordingBatchrequest(result.Id);



                    while (result2.ExpectedResultCount != result2.ResultCount)
                    {
                        result2 = recordingApi.GetRecordingBatchrequest(result.Id);
                        WriteLog.EscribirLog("Intervalo: " + dates + " ||Batch Result Status: " + result2.ResultCount + " / " + result2.ExpectedResultCount);



                        // Simple polling through recursion
                        Thread.Sleep(5000);

                    }


                    //completedBatchStatus = getRecordingStatus(result);

                    // Start downloading the recording files individually
                    foreach (var recording in result2.Results)
                    {
                        string RUT = string.Empty;
                        string DV = string.Empty;
                        object RUT2;
                        object DV2;
                        string nombreCola = string.Empty;
                        string codRatif = "99";
                        
                        if (recording.ResultUrl == null || recording.ResultUrl == String.Empty)
                        {
                            WriteLog.EscribirLog("Intervalo: " + dates + " || Conversationid: " + recording.ConversationId + " || Recordingid: " + recording.RecordingId + " || No tiene grabacion");
                        }
                        else
                        {
                            RecordingMetadata result3 = recordingApi.GetConversationRecordingmetadataRecordingId(recording.ConversationId, recording.RecordingId);
                            DateTime FechaInicioRec = HoraInicio;

                            if (result3.StartTime != null)
                            {
                                FechaInicioRec = DateTime.ParseExact(result3.StartTime.Substring(0, 19), "yyyy-MM-ddTHH:mm:ss", null);

                            }

                            FechaInicioRec = FechaInicioRec.AddHours(HorasGMT);
                            //FechaInicioRec = FechaInicioRec.AddHours(-4);



                            //String fechaInicio = FechaInicioRec.ToString().Substring(6, 4) + FechaInicioRec.ToString().Substring(3, 2) + FechaInicioRec.ToString().Substring(0, 2);
                            String fechaInicio = FechaInicioRec.ToString("yyyyMMdd");

                            //String HoraInicio2 = FechaInicioRec.ToString().Substring(11, 2) + FechaInicioRec.ToString().Substring(14, 2);
                            String HoraInicio2 = FechaInicioRec.ToString("HHmm");
                            Conversation result4 = conversationsApi.GetConversation(recording.ConversationId);

                            foreach (Participant oparticipant in result4.Participants)

                            {

                                string idcontacto;
                                string contactlistID;
                                string[] separator = new string[] { "-" };
                                string[] ArrayRes;
                                string[] ArrayQueue;
                                string aux119 = string.Empty;
                                if (oparticipant.Purpose == "customer")
                                {
                                    if (oparticipant.Attributes.TryGetValue("dialerContactId", out idcontacto) && oparticipant.Attributes.TryGetValue("dialerContactListId", out contactlistID))
                                    {
                                        DialerContact resultContacto = outboundApi.GetOutboundContactlistContact(contactlistID, idcontacto);
                                        if (resultContacto.Data.TryGetValue("RUT", out RUT2))
                                        {
                                            RUT = RUT2.ToString();
                                        }
                                        if (resultContacto.Data.TryGetValue("DV", out DV2))
                                        {
                                            DV = DV2.ToString();
                                        }
                                    }

                                    ArrayRes = RUT.Split(separator, StringSplitOptions.None);

                                    if (ArrayRes[0].Length == 7)
                                    {
                                        RUT = "0" + RUT;
                                    }

                                    nombreCola = oparticipant.QueueName;
                                    ArrayQueue = oparticipant.QueueName.Split(separator, StringSplitOptions.None);

                                    nombreCola = ArrayQueue[0];


                                    //break;
                                }
                                else if (oparticipant.Purpose == "agent") 
                                {
                                    if (oparticipant.WrapupRequired == true)
                                    {
                                        if (oparticipant.Wrapup.Code == "be66ad76-4270-441c-b27a-31a9d5159c33" || oparticipant.Wrapup.Code == "283b4aa2-aa7f-4c2f-8eb0-b6529f60c24d" || oparticipant.Wrapup.Code == "ce1959bb-f550-4e9e-a3ba-2adc4b95b99e" || oparticipant.Wrapup.Code == "f0e3eb82-a0a8-48cc-894c-0d7fffd78509" || oparticipant.Wrapup.Code == "41ea5d8c-33cf-4d69-b555-f740f2ce946f" || oparticipant.Wrapup.Code == "6a2549ed-673b-4c28-bedf-97519e1eeac5" || oparticipant.Wrapup.Code == "23138a71-2ec6-41b6-bc7d-c81980a4b03f" || oparticipant.Wrapup.Code == "12c1747c-c51b-4a7d-ae2a-084032b93eaf" || oparticipant.Wrapup.Code == "208b73d3-bddc-4952-8168-0067f7f032cf" || oparticipant.Wrapup.Code == "264b096d-6639-443d-a40e-a0d6308bc7d8")
                                        {
                                            if (oparticipant.Attributes.TryGetValue("vAux119", out aux119))
                                            {
                                                if (aux119 != "")
                                                {
                                                    codRatif = aux119;
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            String targetDirectory = ConfigurationManager.AppSettings["ruta"]; ;

                            // If there is an errorMsg skip the recording download
                            if (recording.ErrorMsg != null)
                            {
                                WriteLog.EscribirLog("Intervalo: " + dates + " || Error: " + recording.ErrorMsg);

                            }

                            string contentType = recording.ContentType;

                            // Split the text and gets the extension that will be used for the recording
                            string ext = contentType.Split('/').Last();

                            // For the JSON special case
                            if (ext.Length >= 4)
                            {
                                ext = ext.Substring(0, 4);
                            }



                            string filename = fechaInicio + HoraInicio2 + RUT + DV + "T603903T" + nombreCola + "0050" + codRatif + recording.ConversationId;

                            //audiosogg.Add(filename, new Dictionary<string, string>());
                            //for (var w = 0; w <= 5; w++)
                            //{
                            //    switch(w)
                            //    {
                            //    case 0:
                            //    audiosogg[filename].Add("ID_Interaccion", recording.ConversationId);
                            //    continue;
                            //    case 1:
                            //    audiosogg[filename].Add("recordingDate", FechaInicioRec.ToString("yyyy-MM-dd HH:mm:ss"));
                            //    continue;
                            //    case 2:
                            //    audiosogg[filename].Add("RecordingID", recording.Id);
                            //    continue;
                            //    case 3:
                            //    audiosogg[filename].Add("ParticipantID", recording.ConversationId);
                            //    continue;



                            //    }





                            //}


                            audiosogg.Add(filename);
                            WriteLog.EscribirLog("Intervalo: " + dates + " || Grabacion : " + fechaInicio + HoraInicio2 + RUT + DV + "T603903T" + nombreCola + "0050" + codRatif + recording.ConversationId);

                            using (WebClient wc = new WebClient())
                                wc.DownloadFile(recording.ResultUrl, targetDirectory + "\\" + filename + "." + ext);
                        }







                    }


                    conteo += 100;
                }


                // Send a batch request and start polling for updates


                if (conversationDetails.Conversations.Count < iPageSize)
                {

                    break;

                }


                else
                {
                    iPageIndex++;
                    //   Thread.Sleep(5000);
                    continue;
                }


            }


            if (Counter > 0)
            {
                foreach (var audioOgg in audiosogg)
                {
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    //cmd.StandardInput.WriteLine("C:/Users/reyes/Desktop/ffmpeg/ffmpeg/bin/ffmpeg -i C:/Users/reyes/Desktop/IBRProyecto/wsIBR/Grabaciones/"+ oggrecording + ".ogg -codec:a libmp3lame -qscale:a 2 -b:a 320000 C:/Users/reyes/Desktop/IBRProyecto/wsIBR/Grabacionesmp3/"+ oggrecording + ".mp3");
                    cmd.StandardInput.WriteLine(ConfigurationManager.AppSettings["rutapp"] + " -i " + ConfigurationManager.AppSettings["rutaogg"] + audioOgg + ".ogg -codec:a libmp3lame -qscale:a 2 -b:a 320000 " + ConfigurationManager.AppSettings["rutamp3"] + audioOgg + ".mp3");
                    x = operacionesSql.SPInsertCH(IDInteraccion: audioOgg.Substring(audioOgg.Length - 36), RecordingDate: "", RecordingID: "", ParticipantID: "", PeerID: "", ReceordingMP3: audioOgg);
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                }
            }

            WriteLog.EscribirLog("Intervalo: " + dates + " || Estado: Finalizado" + " || total: " + Counter);

            _response.Estado = "Finalizado";
            _response.Fecha = DateTime.Now;
            _response.Contador = Counter;
            return _response;

        }



    }

    public class OperacionSQL
    {


        static string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionBDIBR"];
        public SqlConnection cn;
        public DataSet ds = new DataSet();
        public SqlDataAdapter da;

        public SqlCommand comando { get; set; }
        private void conectar()
        {
            cn = new SqlConnection(connectionString);
        }

        public OperacionSQL()
        {
            conectar();
        }
        public DataSet _return(String qry, string nametableds)
        {
            cn.Open();
            da = new SqlDataAdapter(qry, cn);
            da.Fill(ds, nametableds);
            cn.Close();
            return ds;

        }

        public string SPInsertCH(string IDInteraccion, string RecordingDate, string RecordingID, string ParticipantID,
                                string PeerID, string ReceordingMP3)
        {
            string ret;
            try
            {
                Object returnValue;
                //cn.Open();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    SqlCommand conexion = new SqlCommand("GenesysCloud_LoadRecordings", conn);
                    conexion.CommandType = System.Data.CommandType.StoredProcedure;
                    //coneccion.CommandTimeout = 1;
                    conexion.Parameters.AddWithValue("@ID_INTERACTION ", IDInteraccion);
                    conexion.Parameters.AddWithValue("@RECORDINGDATE_UTC ", RecordingDate);
                    conexion.Parameters.AddWithValue("@RECORDING_ID ", RecordingID);
                    conexion.Parameters.AddWithValue("@PARTICIPANT_ID ", ParticipantID);
                    conexion.Parameters.AddWithValue("@PEER_ID ", PeerID);
                    conexion.Parameters.AddWithValue("@RECORDINGNAME_MP3 ", ReceordingMP3);
                    
                    returnValue = conexion.ExecuteScalar();
                    //cn.Close();

                    ret = "OK";
                    //return Convert.ToInt16(returnValue);
                }
            }
            catch (Exception ex)
            {
                ret = "ERROR EN OPERACION SQL: " + ex.Message;


            }

            return ret;
        }
       
    }




    class WriteLog
    {
        public static void EscribirLog(String Message)
        {


            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            StreamWriter sw = CreateLogFiles();
            sw.WriteLine(sLogFormat + " " + Message);
            sw.Flush();
            sw.Close();

        }
        public static void EscribirLog(String CallIdreference, String IDCRM, String Message)
        {


            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            StreamWriter sw = CreateLogFiles();
            sw.WriteLine(sLogFormat + " CallID: " + CallIdreference + " IDCRM: " + IDCRM + " Msg: " + Message);
            sw.Flush();
            sw.Close();

        }

        private static StreamWriter CreateLogFiles()
        {

            StreamWriter sfile = null;
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
            string sDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string sTime = sYear + sMonth + sDay;
            string ruta = HttpContext.Current.Server.MapPath("~/");
            string sLogFile = @ruta + @"\" + "Log_" + sTime + ".txt";
            //string sLogFile = "Log_" + sTime + ".txt";
            if (!File.Exists(sLogFile))
            {
                sfile = new StreamWriter(sLogFile);
                sfile.WriteLine("******************      Log   " + sTime + "       ******************");
                sfile.Flush();
                sfile.Close();

            }




            int NumberOfRetries = 3;
            int DelayOnRetry = 1000;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    // Do stuff with file
                    sfile = new StreamWriter(sLogFile, true);
                    break; // When done we can break loop
                }
                catch (IOException e)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    if (i == NumberOfRetries) // Last one, (re)throw exception and exit
                        throw new Exception("Se ha producido un error en el metodo writelog()", e);

                    Thread.Sleep(DelayOnRetry);
                }
            }

            return sfile;
        }

    }

    public class WriteNavLog
    {

        public static void EscribirLog(String Message)
        {


            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            StreamWriter sw = CreateLogFiles();
            sw.WriteLine(sLogFormat + " " + Message);
            sw.Flush();
            sw.Close();

        }
        public static void EscribirLog(String CallIdreference, String Message)
        {


            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            StreamWriter sw = CreateLogFiles();
            sw.WriteLine(sLogFormat + " CallID: " + CallIdreference + " Msg: " + Message);
            sw.Flush();
            sw.Close();

        }

        private static StreamWriter CreateLogFiles()
        {

            StreamWriter sfile = null;
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
            string sDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string sTime = sYear + sMonth + sDay;
            string ruta = HttpContext.Current.Server.MapPath("~/");
            string sLogFile = @ruta + @"\" + "LogNavegacion_" + sTime + ".txt";
            //string sLogFile = "Log_" + sTime + ".txt";
            if (!File.Exists(sLogFile))
            {
                sfile = new StreamWriter(sLogFile);
                sfile.WriteLine("******************      Log   " + sTime + "       ******************");
                sfile.Flush();
                sfile.Close();

            }




            int NumberOfRetries = 3;
            int DelayOnRetry = 1000;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    // Do stuff with file
                    sfile = new StreamWriter(sLogFile, true);
                    break; // When done we can break loop
                }
                catch (IOException e)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    if (i == NumberOfRetries) // Last one, (re)throw exception and exit
                        throw new Exception("Se ha producido un error en el metodo writelog()", e);

                    Thread.Sleep(DelayOnRetry);
                }
            }

            return sfile;
        }

    }


}











