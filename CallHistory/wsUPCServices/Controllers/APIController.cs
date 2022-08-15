using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Web.Http;
using wsUPCServices.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using System.Data;
using System.Data.SqlClient;



namespace wsUPCServices.Controllers
{




    public class APIController : ApiController
    {


       

        [HttpPost]
        [Route("IntegrationData")]
        public CallHistoryOut ObtenerCH3(CallHistoryIN _request)
        {
            CallHistoryOut _response = new CallHistoryOut();


            var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken("7ef1842a-e392-4bac-8483-622ba55e7c65", "M-zG7w_CVjMYGvvNkeFFJxpQcA-p54A2BwCLO7GJF_w");
            int HorasGMT = Int32.Parse(ConfigurationManager.AppSettings["HoraGMT"]);
            Console.WriteLine("Access token=" + accessTokenInfo.AccessToken);
            var token_PC = accessTokenInfo.AccessToken;
            PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
            var body = new ConversationQuery();
            var apiInstance2 = new AnalyticsApi();
            var apiInstance3 = new RoutingApi();
            var apiInstance4 = new OutboundApi();
            var apiInstance5 = new UsersApi();
            var ConversaID = _request.conversationID;
            

           
           
            
            DateTime HoraInicio = DateTime.ParseExact(_request.startTime, "yyyy-MM-ddTHH:mm:ss", null);
            
            DateTime HoraInicioBig = HoraInicio.AddDays(-7);
             
            //string HoraInicioGMT4 = HoraInicio.AddHours(-4).ToString("dd-MM-yyyy HH:mm:ss");
            string HoraInicioGMT4 = HoraInicio.AddHours(HorasGMT).ToString("dd-MM-yyyy HH:mm:ss");
            DateTime HoraFin = DateTime.ParseExact(_request.endTime, "yyyy-MM-ddTHH:mm:ss", null);
           // DateTime HoraFinBig = HoraFin.AddDays(-7);
            body.Interval = HoraInicioBig.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss");

            var ocPagesize = 100;
            var ocPageNumber = 1;
            var oclPagesize = 100;
            var oclPageNumber = 1;
            var wuPagesize = 100;
            var wuPageNumber = 1;
            //int contador = 0;
            var oCampaignsOutbound = new Dictionary<string, string>();
            var oCampaignsContactList = new Dictionary<string, string>();
            var oUsers = new Dictionary<string, Dictionary<string, string>>();

            var oWrapUpcodes = new Dictionary<string, string>();

            while (1 > 0)
            {
                CampaignEntityListing resultOutboundCampaign = apiInstance4.GetOutboundCampaigns(pageSize: ocPagesize, pageNumber: ocPageNumber);

                foreach (Campaign oEntitiesCamp in resultOutboundCampaign.Entities)
                {
                    oCampaignsOutbound.Add(oEntitiesCamp.Id, oEntitiesCamp.Name);
                    oCampaignsContactList.Add(oEntitiesCamp.Id, oEntitiesCamp.ContactList.Name);

                }
                ocPageNumber++;
                if (ocPageNumber > resultOutboundCampaign.PageCount)
                {
                    break;
                }
            }


            while (1 > 0)
            {
                UserEntityListing resultUsers = apiInstance5.GetUsers(pageSize: oclPagesize, pageNumber: oclPageNumber, state: "active");
                foreach (User oEntitiesUsers in resultUsers.Entities)
                {
                    oUsers.Add(oEntitiesUsers.Id, new Dictionary<string, string>());
                    oUsers[oEntitiesUsers.Id].Add(oEntitiesUsers.Name, oEntitiesUsers.Email);
                }
                oclPageNumber++;
                if (oclPageNumber > resultUsers.PageCount)
                {
                    break;
                }

            }

            while (1 > 0)
            {
                WrapupCodeEntityListing resultWrapUpCode = apiInstance3.GetRoutingWrapupcodes(pageSize: wuPagesize, pageNumber: wuPageNumber);
                foreach (WrapupCode oEntitiesWu in resultWrapUpCode.Entities)
                {
                    oWrapUpcodes.Add(oEntitiesWu.Id, oEntitiesWu.Name);

                }
                wuPageNumber++;
                if (wuPageNumber > resultWrapUpCode.PageCount)
                {
                    break;
                }

            }


            if (ConversaID!=null)
            {
                
               
                List<ConversationDetailQueryFilter> oConversationDetailQuery = new List<ConversationDetailQueryFilter>();
                ConversationDetailQueryFilter oConversationQueries = new ConversationDetailQueryFilter();
                List<ConversationDetailQueryPredicate> oConversationPredicate = new List<ConversationDetailQueryPredicate>();
                oConversationQueries.Type = ConversationDetailQueryFilter.TypeEnum.And;


                oConversationPredicate.Add(new ConversationDetailQueryPredicate()
                {

                    Type = ConversationDetailQueryPredicate.TypeEnum.Dimension,
                    Dimension = ConversationDetailQueryPredicate.DimensionEnum.Conversationid,
                    _Operator = ConversationDetailQueryPredicate.OperatorEnum.Matches,
                    Value = ConversaID

                }
                    );

                oConversationQueries.Predicates = oConversationPredicate;
                oConversationDetailQuery.Add(oConversationQueries);

                body.ConversationFilters = oConversationDetailQuery;
            }


            List<SegmentDetailQueryFilter> oSegmentDetailQuery = new List<SegmentDetailQueryFilter>();
            SegmentDetailQueryFilter oSegmentDetailQueries = new SegmentDetailQueryFilter();
            
            List<SegmentDetailQueryPredicate> oSegmentDetailQueryPredicate = new List<SegmentDetailQueryPredicate>();
            SegmentDetailQueryPredicate oSegmentDetailQueryPredicates = new SegmentDetailQueryPredicate();

            oSegmentDetailQueries.Type = SegmentDetailQueryFilter.TypeEnum.And;


            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmenttype,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "wrapup"
            }
            );

            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmentend,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = HoraInicio.ToString("yyyy-MM-ddTHH:mm:ss") + "/"+HoraFin.ToString("yyyy-MM-ddTHH:mm:ss")
            }
            );

             oSegmentDetailQueries.Predicates = oSegmentDetailQueryPredicate;
            oSegmentDetailQuery.Add(oSegmentDetailQueries);



            body.SegmentFilters = oSegmentDetailQuery;

            int iPageIndex = 1;
            int iPageSize = _request.PageSize;
            int ResultPaging = _request.PageSize;
            int? ContadorInteracciones = 0;

            while (iPageSize != 0)
            {
                try
                {
                    body.Paging = new PureCloudPlatform.Client.V2.Model.PagingSpec(iPageSize, iPageIndex);
                    AnalyticsConversationQueryResponse result = apiInstance2.PostAnalyticsConversationsDetailsQuery(body);
                    ContadorInteracciones = result.TotalHits;
                    WriteLog.EscribirLog("Ejecutando Intervalo: " + _request.startTime + "/" + _request.endTime + " || Total Conversaciones:" + ContadorInteracciones + " || Page: " + iPageIndex);

                    if (ContadorInteracciones == 0)
                    {
                        _response.Estado = "OK";
                        _response.Fecha = DateTime.Now;
                        _response.Contador = ContadorInteracciones;
                        break;

                    }
                    foreach (AnalyticsConversationWithoutAttributes oConversation in result.Conversations)
                    {


                        //  string IDListaContacto = string.Empty;
                        //   string NomIDListaContacto = string.Empty;
                        string IDCampania = string.Empty;
                        string NombreCL = string.Empty;

                        //  string NomIDCAMPA = string.Empty;
                        //  string DatoObs = string.Empty;
                        //string GUIDReg = string.Empty;
                        //    string CANALCALL = string.Empty;
                        //    bool? LLAMABLE = null;
                        string CONVID = string.Empty;
                        string CodigoCC = "IBR";
                        string CodigoCampana = string.Empty;
                        string idCliente = string.Empty;
                        string ANI = string.Empty;
                        string FechaCreacion = string.Empty;
                        string Duracion = string.Empty;
                        string AgentID = string.Empty;
                        string AgentName = string.Empty;
                        string AgentEmail = string.Empty;
                        string AgendaInterna = string.Empty;
                        string DetalleGestion = string.Empty;
                        string TIPIFICACION = string.Empty;
                        string AceptaLlamado = string.Empty;
                        string DialingTime = string.Empty;
                        //string DialingTimeCH = string.Empty;
                      //  Dictionary<string, string> oDialingList = new Dictionary<string, string>();
                        //string Control1 = string.Empty;
                        //string Control2 = string.Empty;
                        // string Control3 = "NERROR";
                        //string Control4 = string.Empty;
                        var x = "";
                        bool keyExists = false;
                        var codConclu = new List<string>();


                        //if (oConversation.ConversationId == "caff5201-4698-4109-a6ba-3a44adfe8d5f")
                        //{
                        //    var d = oConversation.ConversationId;

                        //}
                        //else
                        //{
                        //    continue;

                        //}




                        OperacionSQL operacionesSql = new OperacionSQL();
                        if (oConversation.OriginatingDirection == AnalyticsConversationWithoutAttributes.OriginatingDirectionEnum.Outbound)
                        {

                            CONVID = oConversation.ConversationId;

                            


                            DateTime FCreacion = oConversation.ConversationStart.Value;
                            FCreacion = FCreacion.AddHours(HorasGMT);
                            //FCreacion = FCreacion.AddHours(-4);
                            FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");
                            int contPart = 0;
                            foreach (AnalyticsParticipantWithoutAttributes oParticipant in oConversation.Participants)
                            {
                                contPart++;
                                int totalPart = oConversation.Participants.Count;
                                var ParticipantName = string.Empty;
                                var peerIDParticipant = string.Empty;
                                var ParticipantID = oParticipant.ParticipantId;
                               
                                if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Customer)

                                {
                                    
                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {
                                            
                                            ANI = oSession.Dnis.Remove(0, 7);

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)

                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Dialing)
                                                {
                                                    TimeSpan tiempoDial = Convert.ToDateTime(oSegment.SegmentEnd) - Convert.ToDateTime(oSegment.SegmentStart);

                                                    DialingTime = string.Format(
                                                                            "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                                            tiempoDial.Hours,
                                                                            tiempoDial.Minutes,
                                                                            tiempoDial.Seconds,
                                                                            tiempoDial.Milliseconds);
                                                   
                                                }
                                               
                                            }



                                            if (oSession.OutboundCampaignId == null)

                                            {

                                                continue;
                                            }

                                            IDCampania = oSession.OutboundCampaignId;
                                            idCliente = oSession.OutboundContactId;
                                            keyExists = oCampaignsOutbound.ContainsKey(IDCampania);
                                            if (keyExists)
                                            {
                                                CodigoCampana = oCampaignsOutbound[IDCampania];
                                                NombreCL = oCampaignsContactList[IDCampania];
                                            }
                                            else
                                            {
                                                CodigoCampana = "-";
                                                NombreCL = "-";
                                            }
                                            break;
                                        }

                                        else if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Callback)
                                        {
                                            ANI = oSession.CallbackNumbers[0].Remove(0, 2);

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)

                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Dialing)
                                                {
                                                    TimeSpan tiempoDial = Convert.ToDateTime(oSegment.SegmentEnd) - Convert.ToDateTime(oSegment.SegmentStart);

                                                    DialingTime = string.Format(
                                                                            "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                                            tiempoDial.Hours,
                                                                            tiempoDial.Minutes,
                                                                            tiempoDial.Seconds,
                                                                            tiempoDial.Milliseconds);
                                                }
                                            }

                                            if (oSession.OutboundCampaignId == null)

                                            {

                                                continue;
                                            }

                                            IDCampania = oSession.OutboundCampaignId;
                                            idCliente = oSession.OutboundContactId;
                                            keyExists = oCampaignsOutbound.ContainsKey(IDCampania);
                                            if (keyExists)
                                            {
                                                CodigoCampana = oCampaignsOutbound[IDCampania];
                                                NombreCL = oCampaignsContactList[IDCampania];
                                            }
                                            else
                                            {
                                                CodigoCampana = "-";
                                                NombreCL = "-";
                                            }
                                            continue;

                                        }



                                    }

                                    // if (IDListaContacto == null || IDCampania == null)
                                    if (IDCampania == null)
                                    {
                                        break;
                                    }



                                }

                                else if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Outbound)
                                {

                                    ParticipantName = oParticipant.Purpose.ToString();
                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {
                                            
                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)
                                            {

                                                if (oSession.OutboundCampaignId == null || oSession.OutboundContactListId == null)

                                                {
                                                    break;

                                                }
                                                else

                                                {
                                                    if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.System)
                                                    {
                                                        FCreacion = oSegment.SegmentStart.Value;
                                                        // FCreacion = FCreacion.AddHours(-4);
                                                        FCreacion = FCreacion.AddHours(HorasGMT);
                                                        FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");

                                                    }

                                                        if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                                    {
                                                        if (oSegment.SegmentEnd >= HoraInicio && oSegment.SegmentEnd < HoraFin)
                                                        {
                                                            //DialingTimeCH = oDialingList[oSession.PeerId];
                                                            if (oSegment.WrapUpCode != "ININ-OUTBOUND-TRANSFERRED-TO-QUEUE")
                                                            {
                                                                TIPIFICACION = oSegment.WrapUpCode;


                                                                // Control4 = "FIN";
                                                                //int m = 0;


                                                                x = operacionesSql.SPInsertCHv3(HoraInicioGMT4, CONVID, NombreCL, CodigoCC, CodigoCampana, idCliente, 
                                                                                                ANI, FechaCreacion, Duracion, DialingTime, AgentID, TIPIFICACION, 
                                                                                                AgentName, AgendaInterna, AgentEmail, ParticipantName,peerIDParticipant, ParticipantID);


                                                            }
                                                            else
                                                            {



                                                                continue;



                                                            }

                                                        }
                                                        else

                                                        {
                                                            //  Control3 = "ERROR";
                                                            continue;
                                                        }



                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }

                                                }

                                            }
                                        }
                                        

                                    }


                                }
                                else if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Agent)
                                {
                                    ParticipantName = oParticipant.Purpose.ToString();
                                    peerIDParticipant = "";
                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {
                                            
                                            Duracion = string.Empty;
                                            foreach (AnalyticsSessionMetric oMetrics in oSession.Metrics)
                                            {

                                                if (oMetrics.Name == "tHandle")
                                                {

                                                    TimeSpan t = TimeSpan.FromMilliseconds(Convert.ToDouble(oMetrics.Value));
                                                    Duracion = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                                            t.Hours,
                                                                            t.Minutes,
                                                                            t.Seconds,
                                                                            t.Milliseconds);

                                                }
                                            }

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)
                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Interact)
                                                {
                                                    FCreacion = oSegment.SegmentStart.Value;
                                                    // FCreacion = FCreacion.AddHours(-4);
                                                    FCreacion = FCreacion.AddHours(HorasGMT);
                                                    FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");

                                                }

                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                                {
                                                    if (oSegment.SegmentEnd >= HoraInicio && oSegment.SegmentEnd < HoraFin)
                                                    {
                                                        peerIDParticipant = oSession.PeerId;
                                                       // DialingTimeCH = oDialingList[oSession.PeerId];
                                                        if (oSegment.WrapUpCode.StartsWith("ININ"))
                                                        {
                                                            TIPIFICACION = oSegment.WrapUpCode;

                                                            //_response.resultadoLlamada = oSegment.WrapUpCode;

                                                        }
                                                        else
                                                        {
                                                            keyExists = oWrapUpcodes.ContainsKey(oSegment.WrapUpCode);
                                                            if (keyExists)
                                                            {
                                                                TIPIFICACION = oWrapUpcodes[oSegment.WrapUpCode];
                                                            }
                                                            else
                                                            {
                                                                TIPIFICACION = oSegment.WrapUpCode;
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

                                                keyExists = oUsers.ContainsKey(oParticipant.UserId);
                                                if (keyExists)
                                                {
                                                    AgentID = oParticipant.UserId;
                                                    foreach (KeyValuePair<string, string> dicusers in oUsers[oParticipant.UserId])
                                                    {

                                                        AgentName = dicusers.Key;
                                                        AgentEmail = dicusers.Value;
                                                    }



                                                }
                                                else
                                                {
                                                    AgentID = oParticipant.UserId;
                                                    AgentName = "-";
                                                    AgentEmail = "-";
                                                }

                                                if (contPart < totalPart)
                                                {
                                                    if (oConversation.Participants[contPart].Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Acd)
                                                    {
                                                        foreach (AnalyticsSession oSession2 in oConversation.Participants[contPart].Sessions)
                                                        {
                                                            if (oSession2.MediaType == AnalyticsSession.MediaTypeEnum.Callback)
                                                            {
                                                                if (oSession2.CallbackScheduledTime.HasValue)
                                                                {

                                                                    DateTime FCallB = oSession2.CallbackScheduledTime.Value;
                                                                  //  FCallB.AddHours(-4);
                                                                    FCallB.AddHours(HorasGMT);
                                                                    AgendaInterna = FCallB.ToString("dd-MM-yyy HH:mm:ss");


                                                                }


                                                            }
                                                        }
                                                    }
                                                }



                                                //AgentID = oParticipant.UserId;
                                                x = operacionesSql.SPInsertCHv3(HoraInicioGMT4, CONVID, NombreCL, CodigoCC, CodigoCampana, idCliente, ANI, FechaCreacion, Duracion, DialingTime, AgentID, TIPIFICACION, AgentName, AgendaInterna, AgentEmail,ParticipantName, peerIDParticipant,ParticipantID);




                                                //_response.propietarioLlamada = oParticipant.UserId;


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
                                    continue;
                                }



                            }

                            if (IDCampania == null)
                            {

                                continue;
                            }
                           


                        }

                        else
                        {
                            continue;
                        }



                    }



                    if (result.Conversations.Count < iPageSize)
                    {
                        WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || FINALIZADO || Total Conversaciones: " + ContadorInteracciones);

                        _response.Estado = "OK";
                        _response.Fecha = DateTime.Now;
                        _response.Contador = ContadorInteracciones;
                        break;

                    }


                    else
                    {
                        iPageIndex++;
                        //   Thread.Sleep(5000);
                        continue;
                    }



                }




                catch (PureCloudPlatform.Client.V2.Client.ApiException e)
                {


                    Debug.WriteLine("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery: " + e.Message);
                    _response.Estado = e.Message;
                    _response.Fecha = DateTime.Now;
                    _response.Contador = ContadorInteracciones;
                    WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery: " + e.Message);

                    if (e.ErrorCode == 429)
                    {
                        string dormirSec = "1";

                        if (!e.Headers.TryGetValue("Retry-After", out dormirSec))
                        {
                            WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery | No se obtuvo Retry-After");
                            Thread.Sleep(30000);

                            continue;
                        }
                        int dormir = Int16.Parse(dormirSec) * 1000;
                        WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery | Se reintentará en " + dormir + " segundos");
                        Thread.Sleep(dormir);
                        iPageIndex++;

                        continue;

                    }





                }
            }



            return _response;



        }

        [HttpPost]
        [Route("IntegrationDataRepro")]
        public CallHistoryOut ObtenerCHReproc(CallHistoryIN _request)
        {
            CallHistoryOut _response = new CallHistoryOut();


            var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken("7ef1842a-e392-4bac-8483-622ba55e7c65", "M-zG7w_CVjMYGvvNkeFFJxpQcA-p54A2BwCLO7GJF_w");
            int HorasGMT = Int32.Parse(ConfigurationManager.AppSettings["HoraGMT"]);
            Console.WriteLine("Access token=" + accessTokenInfo.AccessToken);
            var token_PC = accessTokenInfo.AccessToken;
            PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
            var body = new ConversationQuery();
            var apiInstance2 = new AnalyticsApi();
            var apiInstance3 = new RoutingApi();
            var apiInstance4 = new OutboundApi();
            var apiInstance5 = new UsersApi();
            var ConversaID = _request.conversationID;





            DateTime HoraInicio = DateTime.ParseExact(_request.startTime, "yyyy-MM-ddTHH:mm:ss", null);

            DateTime HoraInicioBig = HoraInicio.AddDays(-7);

            //string HoraInicioGMT4 = HoraInicio.AddHours(-4).ToString("dd-MM-yyyy HH:mm:ss");
            string HoraInicioGMT4 = HoraInicio.AddHours(HorasGMT).ToString("dd-MM-yyyy HH:mm:ss");
            DateTime HoraFin = DateTime.ParseExact(_request.endTime, "yyyy-MM-ddTHH:mm:ss", null);
            // DateTime HoraFinBig = HoraFin.AddDays(-7);
            body.Interval = HoraInicioBig.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss");

            var ocPagesize = 100;
            var ocPageNumber = 1;
            var oclPagesize = 100;
            var oclPageNumber = 1;
            var wuPagesize = 100;
            var wuPageNumber = 1;
            //int contador = 0;
            var oCampaignsOutbound = new Dictionary<string, string>();
            var oCampaignsContactList = new Dictionary<string, string>();
            var oUsers = new Dictionary<string, Dictionary<string, string>>();

            var oWrapUpcodes = new Dictionary<string, string>();

            while (1 > 0)
            {
                CampaignEntityListing resultOutboundCampaign = apiInstance4.GetOutboundCampaigns(pageSize: ocPagesize, pageNumber: ocPageNumber);

                foreach (Campaign oEntitiesCamp in resultOutboundCampaign.Entities)
                {
                    oCampaignsOutbound.Add(oEntitiesCamp.Id, oEntitiesCamp.Name);
                    oCampaignsContactList.Add(oEntitiesCamp.Id, oEntitiesCamp.ContactList.Name);

                }
                ocPageNumber++;
                if (ocPageNumber > resultOutboundCampaign.PageCount)
                {
                    break;
                }
            }


            while (1 > 0)
            {
                UserEntityListing resultUsers = apiInstance5.GetUsers(pageSize: oclPagesize, pageNumber: oclPageNumber, state: "active");
                foreach (User oEntitiesUsers in resultUsers.Entities)
                {
                    oUsers.Add(oEntitiesUsers.Id, new Dictionary<string, string>());
                    oUsers[oEntitiesUsers.Id].Add(oEntitiesUsers.Name, oEntitiesUsers.Email);
                }
                oclPageNumber++;
                if (oclPageNumber > resultUsers.PageCount)
                {
                    break;
                }

            }

            while (1 > 0)
            {
                WrapupCodeEntityListing resultWrapUpCode = apiInstance3.GetRoutingWrapupcodes(pageSize: wuPagesize, pageNumber: wuPageNumber);
                foreach (WrapupCode oEntitiesWu in resultWrapUpCode.Entities)
                {
                    oWrapUpcodes.Add(oEntitiesWu.Id, oEntitiesWu.Name);

                }
                wuPageNumber++;
                if (wuPageNumber > resultWrapUpCode.PageCount)
                {
                    break;
                }

            }


            if (ConversaID != null)
            {


                List<ConversationDetailQueryFilter> oConversationDetailQuery = new List<ConversationDetailQueryFilter>();
                ConversationDetailQueryFilter oConversationQueries = new ConversationDetailQueryFilter();
                List<ConversationDetailQueryPredicate> oConversationPredicate = new List<ConversationDetailQueryPredicate>();
                oConversationQueries.Type = ConversationDetailQueryFilter.TypeEnum.And;


                oConversationPredicate.Add(new ConversationDetailQueryPredicate()
                {

                    Type = ConversationDetailQueryPredicate.TypeEnum.Dimension,
                    Dimension = ConversationDetailQueryPredicate.DimensionEnum.Conversationid,
                    _Operator = ConversationDetailQueryPredicate.OperatorEnum.Matches,
                    Value = ConversaID

                }
                    );

                oConversationQueries.Predicates = oConversationPredicate;
                oConversationDetailQuery.Add(oConversationQueries);

                body.ConversationFilters = oConversationDetailQuery;
            }


            List<SegmentDetailQueryFilter> oSegmentDetailQuery = new List<SegmentDetailQueryFilter>();
            SegmentDetailQueryFilter oSegmentDetailQueries = new SegmentDetailQueryFilter();

            List<SegmentDetailQueryPredicate> oSegmentDetailQueryPredicate = new List<SegmentDetailQueryPredicate>();
            SegmentDetailQueryPredicate oSegmentDetailQueryPredicates = new SegmentDetailQueryPredicate();

            oSegmentDetailQueries.Type = SegmentDetailQueryFilter.TypeEnum.And;


            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmenttype,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "wrapup"
            }
            );

            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmentend,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = HoraInicio.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss")
            }
            );

            oSegmentDetailQueries.Predicates = oSegmentDetailQueryPredicate;
            oSegmentDetailQuery.Add(oSegmentDetailQueries);



            body.SegmentFilters = oSegmentDetailQuery;

            int iPageIndex = 1;
            int iPageSize = _request.PageSize;
            int ResultPaging = _request.PageSize;
            int? ContadorInteracciones = 0;

            while (iPageSize != 0)
            {
                try
                {
                    body.Paging = new PureCloudPlatform.Client.V2.Model.PagingSpec(iPageSize, iPageIndex);
                    AnalyticsConversationQueryResponse result = apiInstance2.PostAnalyticsConversationsDetailsQuery(body);
                    ContadorInteracciones = result.TotalHits;
                    WriteLog.EscribirLog("Ejecutando Intervalo: " + _request.startTime + "/" + _request.endTime + " || Total Conversaciones:" + ContadorInteracciones + " || Page: " + iPageIndex);

                    if (ContadorInteracciones == 0)
                    {
                        _response.Estado = "OK";
                        _response.Fecha = DateTime.Now;
                        _response.Contador = ContadorInteracciones;
                        break;

                    }
                    foreach (AnalyticsConversationWithoutAttributes oConversation in result.Conversations)
                    {


                        //  string IDListaContacto = string.Empty;
                        //   string NomIDListaContacto = string.Empty;
                        string IDCampania = string.Empty;
                        string NombreCL = string.Empty;

                        //  string NomIDCAMPA = string.Empty;
                        //  string DatoObs = string.Empty;
                        //string GUIDReg = string.Empty;
                        //    string CANALCALL = string.Empty;
                        //    bool? LLAMABLE = null;
                        string CONVID = string.Empty;
                        string CodigoCC = "IBR";
                        string CodigoCampana = string.Empty;
                        string idCliente = string.Empty;
                        string ANI = string.Empty;
                        string FechaCreacion = string.Empty;
                        string Duracion = string.Empty;
                        string AgentID = string.Empty;
                        string AgentName = string.Empty;
                        string AgentEmail = string.Empty;
                        string AgendaInterna = string.Empty;
                        string DetalleGestion = string.Empty;
                        string TIPIFICACION = string.Empty;
                        string AceptaLlamado = string.Empty;
                        string DialingTime = string.Empty;
                        //string DialingTimeCH = string.Empty;
                        //  Dictionary<string, string> oDialingList = new Dictionary<string, string>();
                        //string Control1 = string.Empty;
                        //string Control2 = string.Empty;
                        // string Control3 = "NERROR";
                        //string Control4 = string.Empty;
                        var x = "";
                        bool keyExists = false;
                        var codConclu = new List<string>();


                        //if (oConversation.ConversationId == "caff5201-4698-4109-a6ba-3a44adfe8d5f")
                        //{
                        //    var d = oConversation.ConversationId;

                        //}
                        //else
                        //{
                        //    continue;

                        //}




                        OperacionSQL operacionesSql = new OperacionSQL();
                        if (oConversation.OriginatingDirection == AnalyticsConversationWithoutAttributes.OriginatingDirectionEnum.Outbound)
                        {

                            CONVID = oConversation.ConversationId;




                            DateTime FCreacion = oConversation.ConversationStart.Value;
                            FCreacion = FCreacion.AddHours(HorasGMT);
                            //FCreacion = FCreacion.AddHours(-4);
                            FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");
                            int contPart = 0;
                            foreach (AnalyticsParticipantWithoutAttributes oParticipant in oConversation.Participants)
                            {
                                contPart++;
                                int totalPart = oConversation.Participants.Count;
                                var ParticipantName = string.Empty;
                                var peerIDParticipant = string.Empty;
                                var ParticipantID = oParticipant.ParticipantId;

                                if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Customer)

                                {

                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {

                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {

                                            ANI = oSession.Dnis.Remove(0, 7);

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)

                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Dialing)
                                                {
                                                    TimeSpan tiempoDial = Convert.ToDateTime(oSegment.SegmentEnd) - Convert.ToDateTime(oSegment.SegmentStart);

                                                    DialingTime = string.Format(
                                                                            "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                                            tiempoDial.Hours,
                                                                            tiempoDial.Minutes,
                                                                            tiempoDial.Seconds,
                                                                            tiempoDial.Milliseconds);

                                                }

                                            }



                                            if (oSession.OutboundCampaignId == null)

                                            {

                                                continue;
                                            }

                                            IDCampania = oSession.OutboundCampaignId;
                                            idCliente = oSession.OutboundContactId;
                                            keyExists = oCampaignsOutbound.ContainsKey(IDCampania);
                                            if (keyExists)
                                            {
                                                CodigoCampana = oCampaignsOutbound[IDCampania];
                                                NombreCL = oCampaignsContactList[IDCampania];
                                            }
                                            else
                                            {
                                                CodigoCampana = "-";
                                                NombreCL = "-";
                                            }
                                            break;
                                        }

                                        else if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Callback)
                                        {
                                            ANI = oSession.CallbackNumbers[0].Remove(0, 2);

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)

                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Dialing)
                                                {
                                                    TimeSpan tiempoDial = Convert.ToDateTime(oSegment.SegmentEnd) - Convert.ToDateTime(oSegment.SegmentStart);

                                                    DialingTime = string.Format(
                                                                            "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                                            tiempoDial.Hours,
                                                                            tiempoDial.Minutes,
                                                                            tiempoDial.Seconds,
                                                                            tiempoDial.Milliseconds);
                                                }
                                            }

                                            if (oSession.OutboundCampaignId == null)

                                            {

                                                continue;
                                            }

                                            IDCampania = oSession.OutboundCampaignId;
                                            idCliente = oSession.OutboundContactId;
                                            keyExists = oCampaignsOutbound.ContainsKey(IDCampania);
                                            if (keyExists)
                                            {
                                                CodigoCampana = oCampaignsOutbound[IDCampania];
                                                NombreCL = oCampaignsContactList[IDCampania];
                                            }
                                            else
                                            {
                                                CodigoCampana = "-";
                                                NombreCL = "-";
                                            }
                                            continue;

                                        }



                                    }

                                    // if (IDListaContacto == null || IDCampania == null)
                                    if (IDCampania == null)
                                    {
                                        break;
                                    }



                                }

                                else if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Outbound)
                                {

                                    ParticipantName = oParticipant.Purpose.ToString();
                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)
                                            {

                                                if (oSession.OutboundCampaignId == null || oSession.OutboundContactListId == null)

                                                {
                                                    break;

                                                }
                                                else

                                                {
                                                    if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.System)
                                                    {
                                                        FCreacion = oSegment.SegmentStart.Value;
                                                        // FCreacion = FCreacion.AddHours(-4);
                                                        FCreacion = FCreacion.AddHours(HorasGMT);
                                                        FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");

                                                    }

                                                    if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                                    {
                                                        if (oSegment.SegmentEnd >= HoraInicio && oSegment.SegmentEnd < HoraFin)
                                                        {
                                                            //DialingTimeCH = oDialingList[oSession.PeerId];
                                                            if (oSegment.WrapUpCode != "ININ-OUTBOUND-TRANSFERRED-TO-QUEUE")
                                                            {
                                                                TIPIFICACION = oSegment.WrapUpCode;


                                                                // Control4 = "FIN";
                                                                //int m = 0;


                                                                x = operacionesSql.SPInsertCHREP(HoraInicioGMT4, CONVID, NombreCL, CodigoCC, CodigoCampana, idCliente,
                                                                                                ANI, FechaCreacion, Duracion, DialingTime, AgentID, TIPIFICACION,
                                                                                                AgentName, AgendaInterna, AgentEmail, ParticipantName, peerIDParticipant, ParticipantID);


                                                            }
                                                            else
                                                            {



                                                                continue;



                                                            }

                                                        }
                                                        else

                                                        {
                                                            //  Control3 = "ERROR";
                                                            continue;
                                                        }



                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }

                                                }

                                            }
                                        }


                                    }


                                }
                                else if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Agent)
                                {
                                    ParticipantName = oParticipant.Purpose.ToString();
                                    peerIDParticipant = "";
                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {

                                            Duracion = string.Empty;
                                            foreach (AnalyticsSessionMetric oMetrics in oSession.Metrics)
                                            {

                                                if (oMetrics.Name == "tHandle")
                                                {

                                                    TimeSpan t = TimeSpan.FromMilliseconds(Convert.ToDouble(oMetrics.Value));
                                                    Duracion = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                                            t.Hours,
                                                                            t.Minutes,
                                                                            t.Seconds,
                                                                            t.Milliseconds);

                                                }
                                            }

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)
                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Interact)
                                                {
                                                    FCreacion = oSegment.SegmentStart.Value;
                                                    // FCreacion = FCreacion.AddHours(-4);
                                                    FCreacion = FCreacion.AddHours(HorasGMT);
                                                    FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");

                                                }

                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                                {
                                                    if (oSegment.SegmentEnd >= HoraInicio && oSegment.SegmentEnd < HoraFin)
                                                    {
                                                        peerIDParticipant = oSession.PeerId;
                                                        // DialingTimeCH = oDialingList[oSession.PeerId];
                                                        if (oSegment.WrapUpCode.StartsWith("ININ"))
                                                        {
                                                            TIPIFICACION = oSegment.WrapUpCode;

                                                            //_response.resultadoLlamada = oSegment.WrapUpCode;

                                                        }
                                                        else
                                                        {
                                                            keyExists = oWrapUpcodes.ContainsKey(oSegment.WrapUpCode);
                                                            if (keyExists)
                                                            {
                                                                TIPIFICACION = oWrapUpcodes[oSegment.WrapUpCode];
                                                            }
                                                            else
                                                            {
                                                                TIPIFICACION = oSegment.WrapUpCode;
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

                                                keyExists = oUsers.ContainsKey(oParticipant.UserId);
                                                if (keyExists)
                                                {
                                                    AgentID = oParticipant.UserId;
                                                    foreach (KeyValuePair<string, string> dicusers in oUsers[oParticipant.UserId])
                                                    {

                                                        AgentName = dicusers.Key;
                                                        AgentEmail = dicusers.Value;
                                                    }



                                                }
                                                else
                                                {
                                                    AgentID = oParticipant.UserId;
                                                    AgentName = "-";
                                                    AgentEmail = "-";
                                                }

                                                if (contPart < totalPart)
                                                {
                                                    if (oConversation.Participants[contPart].Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Acd)
                                                    {
                                                        foreach (AnalyticsSession oSession2 in oConversation.Participants[contPart].Sessions)
                                                        {
                                                            if (oSession2.MediaType == AnalyticsSession.MediaTypeEnum.Callback)
                                                            {
                                                                if (oSession2.CallbackScheduledTime.HasValue)
                                                                {

                                                                    DateTime FCallB = oSession2.CallbackScheduledTime.Value;
                                                                    //  FCallB.AddHours(-4);
                                                                    FCallB.AddHours(HorasGMT);
                                                                    AgendaInterna = FCallB.ToString("dd-MM-yyy HH:mm:ss");


                                                                }


                                                            }
                                                        }
                                                    }
                                                }



                                                //AgentID = oParticipant.UserId;
                                                x = operacionesSql.SPInsertCHREP(HoraInicioGMT4, CONVID, NombreCL, CodigoCC, CodigoCampana, idCliente, ANI, FechaCreacion, Duracion, DialingTime, AgentID, TIPIFICACION, AgentName, AgendaInterna, AgentEmail, ParticipantName, peerIDParticipant, ParticipantID);




                                                //_response.propietarioLlamada = oParticipant.UserId;


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
                                    continue;
                                }



                            }

                            if (IDCampania == null)
                            {

                                continue;
                            }



                        }

                        else
                        {
                            continue;
                        }



                    }



                    if (result.Conversations.Count < iPageSize)
                    {
                        WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || FINALIZADO || Total Conversaciones: " + ContadorInteracciones);

                        _response.Estado = "OK";
                        _response.Fecha = DateTime.Now;
                        _response.Contador = ContadorInteracciones;
                        break;

                    }


                    else
                    {
                        iPageIndex++;
                        //   Thread.Sleep(5000);
                        continue;
                    }



                }




                catch (PureCloudPlatform.Client.V2.Client.ApiException e)
                {


                    Debug.WriteLine("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery: " + e.Message);
                    _response.Estado = e.Message;
                    _response.Fecha = DateTime.Now;
                    _response.Contador = ContadorInteracciones;
                    WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery: " + e.Message);

                    if (e.ErrorCode == 429)
                    {
                        string dormirSec = "1";

                        if (!e.Headers.TryGetValue("Retry-After", out dormirSec))
                        {
                            WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery | No se obtuvo Retry-After");
                            Thread.Sleep(30000);

                            continue;
                        }
                        int dormir = Int16.Parse(dormirSec) * 1000;
                        WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery | Se reintentará en " + dormir + " segundos");
                        Thread.Sleep(dormir);
                        iPageIndex++;

                        continue;

                    }





                }
            }



            return _response;



        }





        [HttpPost]
        [Route("IntegrationData_old")]
        public CallHistoryOut ObtenerCH2(CallHistoryIN _request)
        {
            CallHistoryOut _response = new CallHistoryOut();
            int HorasGMT = Int32.Parse(ConfigurationManager.AppSettings["HoraGMT"]);

            var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken("7ef1842a-e392-4bac-8483-622ba55e7c65", "M-zG7w_CVjMYGvvNkeFFJxpQcA-p54A2BwCLO7GJF_w");

            Console.WriteLine("Access token=" + accessTokenInfo.AccessToken);
            var token_PC = accessTokenInfo.AccessToken;
            PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
            var body = new ConversationQuery();
            var apiInstance2 = new AnalyticsApi();
            var apiInstance3 = new RoutingApi();
            var apiInstance4 = new OutboundApi();
            var apiInstance5 = new UsersApi();
            var ConversaID = _request.conversationID;





            DateTime HoraInicio = DateTime.ParseExact(_request.startTime, "yyyy-MM-ddTHH:mm:ss", null);

            DateTime HoraInicioBig = HoraInicio.AddDays(-7);

            //string HoraInicioGMT4 = HoraInicio.AddHours(-4).ToString("dd-MM-yyyy HH:mm:ss");
            string HoraInicioGMT4 = HoraInicio.AddHours(HorasGMT).ToString("dd-MM-yyyy HH:mm:ss");
            DateTime HoraFin = DateTime.ParseExact(_request.endTime, "yyyy-MM-ddTHH:mm:ss", null);
            // DateTime HoraFinBig = HoraFin.AddDays(-7);
            body.Interval = HoraInicioBig.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss");

            var ocPagesize = 100;
            var ocPageNumber = 1;
            var oclPagesize = 100;
            var oclPageNumber = 1;
            var wuPagesize = 100;
            var wuPageNumber = 1;
            //int contador = 0;
            var oCampaignsOutbound = new Dictionary<string, string>();
            var oCampaignsContactList = new Dictionary<string, string>();
            var oUsers = new Dictionary<string, Dictionary<string, string>>();

            var oWrapUpcodes = new Dictionary<string, string>();

            while (1 > 0)
            {
                CampaignEntityListing resultOutboundCampaign = apiInstance4.GetOutboundCampaigns(pageSize: ocPagesize, pageNumber: ocPageNumber);

                foreach (Campaign oEntitiesCamp in resultOutboundCampaign.Entities)
                {
                    oCampaignsOutbound.Add(oEntitiesCamp.Id, oEntitiesCamp.Name);
                    oCampaignsContactList.Add(oEntitiesCamp.Id, oEntitiesCamp.ContactList.Name);

                }
                ocPageNumber++;
                if (ocPageNumber > resultOutboundCampaign.PageCount)
                {
                    break;
                }
            }


            while (1 > 0)
            {
                UserEntityListing resultUsers = apiInstance5.GetUsers(pageSize: oclPagesize, pageNumber: oclPageNumber, state: "active");
                foreach (User oEntitiesUsers in resultUsers.Entities)
                {
                    oUsers.Add(oEntitiesUsers.Id, new Dictionary<string, string>());
                    oUsers[oEntitiesUsers.Id].Add(oEntitiesUsers.Name, oEntitiesUsers.Email);
                }
                oclPageNumber++;
                if (oclPageNumber > resultUsers.PageCount)
                {
                    break;
                }

            }

            while (1 > 0)
            {
                WrapupCodeEntityListing resultWrapUpCode = apiInstance3.GetRoutingWrapupcodes(pageSize: wuPagesize, pageNumber: wuPageNumber);
                foreach (WrapupCode oEntitiesWu in resultWrapUpCode.Entities)
                {
                    oWrapUpcodes.Add(oEntitiesWu.Id, oEntitiesWu.Name);

                }
                wuPageNumber++;
                if (wuPageNumber > resultWrapUpCode.PageCount)
                {
                    break;
                }

            }


            if (ConversaID != null)
            {


                List<ConversationDetailQueryFilter> oConversationDetailQuery = new List<ConversationDetailQueryFilter>();
                ConversationDetailQueryFilter oConversationQueries = new ConversationDetailQueryFilter();
                List<ConversationDetailQueryPredicate> oConversationPredicate = new List<ConversationDetailQueryPredicate>();
                oConversationQueries.Type = ConversationDetailQueryFilter.TypeEnum.And;


                oConversationPredicate.Add(new ConversationDetailQueryPredicate()
                {

                    Type = ConversationDetailQueryPredicate.TypeEnum.Dimension,
                    Dimension = ConversationDetailQueryPredicate.DimensionEnum.Conversationid,
                    _Operator = ConversationDetailQueryPredicate.OperatorEnum.Matches,
                    Value = ConversaID

                }
                    );

                oConversationQueries.Predicates = oConversationPredicate;
                oConversationDetailQuery.Add(oConversationQueries);

                body.ConversationFilters = oConversationDetailQuery;
            }


            List<SegmentDetailQueryFilter> oSegmentDetailQuery = new List<SegmentDetailQueryFilter>();
            SegmentDetailQueryFilter oSegmentDetailQueries = new SegmentDetailQueryFilter();

            List<SegmentDetailQueryPredicate> oSegmentDetailQueryPredicate = new List<SegmentDetailQueryPredicate>();
            SegmentDetailQueryPredicate oSegmentDetailQueryPredicates = new SegmentDetailQueryPredicate();

            oSegmentDetailQueries.Type = SegmentDetailQueryFilter.TypeEnum.And;


            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmenttype,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "wrapup"
            }
            );

            oSegmentDetailQueryPredicate.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Segmentend,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = HoraInicio.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + HoraFin.ToString("yyyy-MM-ddTHH:mm:ss")
            }
            );

            oSegmentDetailQueries.Predicates = oSegmentDetailQueryPredicate;
            oSegmentDetailQuery.Add(oSegmentDetailQueries);



            body.SegmentFilters = oSegmentDetailQuery;

            int iPageIndex = 1;
            int iPageSize = _request.PageSize;
            int ResultPaging = _request.PageSize;
            int? ContadorInteracciones = 0;

            while (iPageSize != 0)
            {
                try
                {
                    body.Paging = new PureCloudPlatform.Client.V2.Model.PagingSpec(iPageSize, iPageIndex);
                    AnalyticsConversationQueryResponse result = apiInstance2.PostAnalyticsConversationsDetailsQuery(body);
                    ContadorInteracciones = result.TotalHits;
                    WriteLog.EscribirLog("Ejecutando Intervalo: " + _request.startTime + "/" + _request.endTime + " || Total Conversaciones:" + ContadorInteracciones + " || Page: " + iPageIndex);

                    if (ContadorInteracciones == 0)
                    {
                        _response.Estado = "OK";
                        _response.Fecha = DateTime.Now;
                        _response.Contador = ContadorInteracciones;
                        break;

                    }
                    foreach (AnalyticsConversationWithoutAttributes oConversation in result.Conversations)
                    {


                        //  string IDListaContacto = string.Empty;
                        //   string NomIDListaContacto = string.Empty;
                        string IDCampania = string.Empty;
                        string NombreCL = string.Empty;

                        //  string NomIDCAMPA = string.Empty;
                        //  string DatoObs = string.Empty;
                        //string GUIDReg = string.Empty;
                        //    string CANALCALL = string.Empty;
                        //    bool? LLAMABLE = null;
                        string CONVID = string.Empty;
                        string CodigoCC = "IBR";
                        string CodigoCampana = string.Empty;
                        string idCliente = string.Empty;
                        string ANI = string.Empty;
                        string FechaCreacion = string.Empty;
                        string Duracion = string.Empty;
                        string AgentID = string.Empty;
                        string AgentName = string.Empty;
                        string AgentEmail = string.Empty;
                        string AgendaInterna = string.Empty;
                        string DetalleGestion = string.Empty;
                        string TIPIFICACION = string.Empty;
                        string AceptaLlamado = string.Empty;
                        string DialingTime = string.Empty;
                        //string Control1 = string.Empty;
                        //string Control2 = string.Empty;
                        // string Control3 = "NERROR";
                        //string Control4 = string.Empty;
                        var x = "";
                        bool keyExists = false;
                        var codConclu = new List<string>();


                        //if (oConversation.ConversationId == "caff5201-4698-4109-a6ba-3a44adfe8d5f")
                        //{
                        //    var d = oConversation.ConversationId;

                        //}
                        //else
                        //{
                        //    continue;

                        //}




                        OperacionSQL operacionesSql = new OperacionSQL();
                        if (oConversation.OriginatingDirection == AnalyticsConversationWithoutAttributes.OriginatingDirectionEnum.Outbound)
                        {

                            CONVID = oConversation.ConversationId;




                            DateTime FCreacion = oConversation.ConversationStart.Value;
                           // FCreacion = FCreacion.AddHours(-4);
                            FCreacion = FCreacion.AddHours(HorasGMT);
                            FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");
                            int contPart = 0;
                            foreach (AnalyticsParticipantWithoutAttributes oParticipant in oConversation.Participants)
                            {
                                contPart++;
                                int totalPart = oConversation.Participants.Count;

                                var ParticipantID = oParticipant.ParticipantId;
                                if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Customer)

                                {

                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {
                                            ANI = oSession.Dnis.Remove(0, 7);

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)

                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Dialing)
                                                {
                                                    TimeSpan tiempoDial = Convert.ToDateTime(oSegment.SegmentEnd) - Convert.ToDateTime(oSegment.SegmentStart);

                                                    DialingTime = string.Format(
                                                                            "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                                            tiempoDial.Hours,
                                                                            tiempoDial.Minutes,
                                                                            tiempoDial.Seconds,
                                                                            tiempoDial.Milliseconds);
                                                }
                                            }



                                            if (oSession.OutboundCampaignId == null)

                                            {

                                                continue;
                                            }

                                            IDCampania = oSession.OutboundCampaignId;
                                            idCliente = oSession.OutboundContactId;
                                            keyExists = oCampaignsOutbound.ContainsKey(IDCampania);
                                            if (keyExists)
                                            {
                                                CodigoCampana = oCampaignsOutbound[IDCampania];
                                                NombreCL = oCampaignsContactList[IDCampania];
                                            }
                                            else
                                            {
                                                CodigoCampana = "-";
                                                NombreCL = "-";
                                            }
                                            break;
                                        }

                                        else if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Callback)
                                        {
                                            ANI = oSession.CallbackNumbers[0].Remove(0, 2);

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)

                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Dialing)
                                                {
                                                    TimeSpan tiempoDial = Convert.ToDateTime(oSegment.SegmentEnd) - Convert.ToDateTime(oSegment.SegmentStart);

                                                    DialingTime = string.Format(
                                                                            "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                                                                            tiempoDial.Hours,
                                                                            tiempoDial.Minutes,
                                                                            tiempoDial.Seconds,
                                                                            tiempoDial.Milliseconds);
                                                }
                                            }

                                            if (oSession.OutboundCampaignId == null)

                                            {

                                                continue;
                                            }

                                            IDCampania = oSession.OutboundCampaignId;
                                            idCliente = oSession.OutboundContactId;
                                            keyExists = oCampaignsOutbound.ContainsKey(IDCampania);
                                            if (keyExists)
                                            {
                                                CodigoCampana = oCampaignsOutbound[IDCampania];
                                                NombreCL = oCampaignsContactList[IDCampania];
                                            }
                                            else
                                            {
                                                CodigoCampana = "-";
                                                NombreCL = "-";
                                            }
                                            continue;

                                        }



                                    }

                                    // if (IDListaContacto == null || IDCampania == null)
                                    if (IDCampania == null)
                                    {
                                        break;
                                    }



                                }

                                else if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Outbound)
                                {


                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {
                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)
                                            {

                                                if (oSession.OutboundCampaignId == null || oSession.OutboundContactListId == null)

                                                {
                                                    break;

                                                }
                                                else

                                                {
                                                    if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.System)
                                                    {
                                                        FCreacion = oSegment.SegmentStart.Value;
                                                        //FCreacion = FCreacion.AddHours(-4);
                                                        FCreacion = FCreacion.AddHours(HorasGMT);
                                                        FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");

                                                    }

                                                    if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                                    {
                                                        if (oSegment.SegmentEnd >= HoraInicio && oSegment.SegmentEnd < HoraFin)
                                                        {

                                                            if (oSegment.WrapUpCode != "ININ-OUTBOUND-TRANSFERRED-TO-QUEUE")
                                                            {
                                                                TIPIFICACION = oSegment.WrapUpCode;


                                                                // Control4 = "FIN";
                                                                //int m = 0;


                                                                x = operacionesSql.SPInsertCH(HoraInicioGMT4, CONVID, NombreCL, CodigoCC, CodigoCampana, idCliente, ANI, FechaCreacion, Duracion, DialingTime, AgentID, TIPIFICACION, AgentName, AgendaInterna, AgentEmail);


                                                            }
                                                            else
                                                            {



                                                                continue;



                                                            }

                                                        }
                                                        else

                                                        {
                                                            //  Control3 = "ERROR";
                                                            continue;
                                                        }



                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }

                                                }

                                            }
                                        }


                                    }


                                }
                                else if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Agent)
                                {
                                    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                    {
                                        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Voice)
                                        {
                                            Duracion = string.Empty;
                                            foreach (AnalyticsSessionMetric oMetrics in oSession.Metrics)
                                            {

                                                if (oMetrics.Name == "tHandle")
                                                {

                                                    TimeSpan t = TimeSpan.FromMilliseconds(Convert.ToDouble(oMetrics.Value));
                                                    Duracion = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                                                            t.Hours,
                                                                            t.Minutes,
                                                                            t.Seconds,
                                                                            t.Milliseconds);

                                                }
                                            }

                                            foreach (AnalyticsConversationSegment oSegment in oSession.Segments)
                                            {
                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Interact)
                                                {
                                                    FCreacion = oSegment.SegmentStart.Value;
                                                   // FCreacion = FCreacion.AddHours(-4);
                                                    FCreacion = FCreacion.AddHours(HorasGMT);
                                                    FechaCreacion = FCreacion.ToString("dd-MM-yyyy HH:mm:ss");

                                                }

                                                if (oSegment.SegmentType == AnalyticsConversationSegment.SegmentTypeEnum.Wrapup)
                                                {
                                                    if (oSegment.SegmentEnd >= HoraInicio && oSegment.SegmentEnd < HoraFin)
                                                    {
                                                        if (oSegment.WrapUpCode.StartsWith("ININ"))
                                                        {
                                                            TIPIFICACION = oSegment.WrapUpCode;

                                                            //_response.resultadoLlamada = oSegment.WrapUpCode;

                                                        }
                                                        else
                                                        {
                                                            keyExists = oWrapUpcodes.ContainsKey(oSegment.WrapUpCode);
                                                            if (keyExists)
                                                            {
                                                                TIPIFICACION = oWrapUpcodes[oSegment.WrapUpCode];
                                                            }
                                                            else
                                                            {
                                                                TIPIFICACION = oSegment.WrapUpCode;
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

                                                keyExists = oUsers.ContainsKey(oParticipant.UserId);
                                                if (keyExists)
                                                {
                                                    AgentID = oParticipant.UserId;
                                                    foreach (KeyValuePair<string, string> dicusers in oUsers[oParticipant.UserId])
                                                    {

                                                        AgentName = dicusers.Key;
                                                        AgentEmail = dicusers.Value;
                                                    }



                                                }
                                                else
                                                {
                                                    AgentID = oParticipant.UserId;
                                                    AgentName = "-";
                                                    AgentEmail = "-";
                                                }

                                                if (contPart < totalPart)
                                                {
                                                    if (oConversation.Participants[contPart].Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Acd)
                                                    {
                                                        foreach (AnalyticsSession oSession2 in oConversation.Participants[contPart].Sessions)
                                                        {
                                                            if (oSession2.MediaType == AnalyticsSession.MediaTypeEnum.Callback)
                                                            {
                                                                if (oSession2.CallbackScheduledTime.HasValue)
                                                                {

                                                                    DateTime FCallB = oSession2.CallbackScheduledTime.Value;
                                                                    //FCallB.AddHours(-4);
                                                                    FCallB.AddHours(HorasGMT);
                                                                    AgendaInterna = FCallB.ToString("dd-MM-yyy HH:mm:ss");


                                                                }


                                                            }
                                                        }
                                                    }
                                                }



                                                //AgentID = oParticipant.UserId;
                                                x = operacionesSql.SPInsertCH(HoraInicioGMT4, CONVID, NombreCL, CodigoCC, CodigoCampana, idCliente, ANI, FechaCreacion, Duracion, DialingTime, AgentID, TIPIFICACION, AgentName, AgendaInterna, AgentEmail);




                                                //_response.propietarioLlamada = oParticipant.UserId;


                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }

                                }

                                //else if (oParticipant.Purpose == AnalyticsParticipantWithoutAttributes.PurposeEnum.Acd)
                                //{
                                //    foreach (AnalyticsSession oSession in oParticipant.Sessions)
                                //    {
                                //        if (oSession.MediaType == AnalyticsSession.MediaTypeEnum.Callback)
                                //        {
                                //            if (oSession.CallbackScheduledTime.HasValue)
                                //            {

                                //                DateTime FCallB = oSession.CallbackScheduledTime.Value;
                                //                FCallB.AddHours(-4);
                                //                AgendaInterna = FCallB.ToString("dd-MM-yyy HH:mm:ss");


                                //            }


                                //        }
                                //    }
                                //}

                                else
                                {
                                    continue;
                                }



                            }

                            if (IDCampania == null)
                            {

                                continue;
                            }



                        }

                        else
                        {
                            continue;
                        }



                    }



                    if (result.Conversations.Count < iPageSize)
                    {
                        WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || FINALIZADO || Total Conversaciones: " + ContadorInteracciones);

                        _response.Estado = "OK";
                        _response.Fecha = DateTime.Now;
                        _response.Contador = ContadorInteracciones;
                        break;

                    }


                    else
                    {
                        iPageIndex++;
                        //   Thread.Sleep(5000);
                        continue;
                    }



                }




                catch (PureCloudPlatform.Client.V2.Client.ApiException e)
                {


                    Debug.WriteLine("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery: " + e.Message);
                    _response.Estado = e.Message;
                    _response.Fecha = DateTime.Now;
                    _response.Contador = ContadorInteracciones;
                    WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery: " + e.Message);

                    if (e.ErrorCode == 429)
                    {
                        string dormirSec = "1";

                        if (!e.Headers.TryGetValue("Retry-After", out dormirSec))
                        {
                            WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery | No se obtuvo Retry-After");
                            Thread.Sleep(30000);

                            continue;
                        }
                        int dormir = Int16.Parse(dormirSec) * 1000;
                        WriteLog.EscribirLog("Intervalo: " + _request.startTime + "/" + _request.endTime + " || Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery | Se reintentará en " + dormir + " segundos");
                        Thread.Sleep(dormir);
                        iPageIndex++;

                        continue;

                    }





                }
            }



            return _response;



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
                sw.WriteLine(sLogFormat + " CallID: " + CallIdreference + " IDCRM: " + IDCRM+ " Msg: " + Message);
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


            public string SPInsertCH(string StartInterval, string ConversationID, string NombreCL,string CodigoCC, 
                                    string CodCampana, string IDCLIENTE, string ANI, string FechaCall, string DuracionCall, 
                                    string DialingCall,string CodEjecutivo, string Observacion, string NOMBRE_EJECUTIVO, 
                                    string AGENDA_INTERNA, string EMAIL_EJECUTIVO)
            {
                string ret;
                try
                {
                    Object returnValue;
                    //cn.Open();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {

                        conn.Open();

                        SqlCommand conexion = new SqlCommand("GenesysCloud_LoadCallHistory", conn);
                        conexion.CommandType = System.Data.CommandType.StoredProcedure;
                        //coneccion.CommandTimeout = 1;
                        conexion.Parameters.AddWithValue("@START_INTERVAL", StartInterval);
                        conexion.Parameters.AddWithValue("@ID_INTERACCION", ConversationID);
                        conexion.Parameters.AddWithValue("@CODIGO_CALL_CENTER", CodigoCC);
                        conexion.Parameters.AddWithValue("@CODIGO_CAMPANA", CodCampana);
                        conexion.Parameters.AddWithValue("@ID_CLIENTE", IDCLIENTE);
                        conexion.Parameters.AddWithValue("@TELEFONO_GESTIONADO", ANI);
                        conexion.Parameters.AddWithValue("@FECHA_LLAMADO", FechaCall);
                        conexion.Parameters.AddWithValue("@DURACION_LLAMADA", DuracionCall);
                        conexion.Parameters.AddWithValue("@DURACION_DIALING", DialingCall);
                        conexion.Parameters.AddWithValue("@CODIGO_EJECUTIVO", CodEjecutivo);
                        conexion.Parameters.AddWithValue("@OBSERVACION_GESTION", Observacion);
                        conexion.Parameters.AddWithValue("@CONTACTLIST", NombreCL);
                        conexion.Parameters.AddWithValue("@NOMBRE_EJECUTIVO", NOMBRE_EJECUTIVO);
                        conexion.Parameters.AddWithValue("@EMAIL_EJECUTIVO", EMAIL_EJECUTIVO);
                        conexion.Parameters.AddWithValue("@AGENDA_INTERNA", AGENDA_INTERNA);
                 





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
            public string SPInsertCHv3(string StartInterval, string ConversationID, string NombreCL, string CodigoCC, 
                                        string CodCampana, string IDCLIENTE, string ANI, string FechaCall, string DuracionCall, 
                                        string DialingCall, string CodEjecutivo, string Observacion, string NOMBRE_EJECUTIVO, 
                                        string AGENDA_INTERNA, string EMAIL_EJECUTIVO, string NombreParticipante,string peerParticipante, string IDParticipante)
            {
                string ret;
                try
                {
                    Object returnValue;
                    //cn.Open();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {

                        conn.Open();

                        SqlCommand conexion = new SqlCommand("GenesysCloud_LoadCallHistory_V2", conn);
                        conexion.CommandType = System.Data.CommandType.StoredProcedure;
                        //coneccion.CommandTimeout = 1;
                        conexion.Parameters.AddWithValue("@START_INTERVAL", StartInterval);
                        conexion.Parameters.AddWithValue("@ID_INTERACCION", ConversationID);
                        conexion.Parameters.AddWithValue("@CODIGO_CALL_CENTER", CodigoCC);
                        conexion.Parameters.AddWithValue("@CODIGO_CAMPANA", CodCampana);
                        conexion.Parameters.AddWithValue("@ID_CLIENTE", IDCLIENTE);
                        conexion.Parameters.AddWithValue("@TELEFONO_GESTIONADO", ANI);
                        conexion.Parameters.AddWithValue("@FECHA_LLAMADO", FechaCall);
                        conexion.Parameters.AddWithValue("@DURACION_LLAMADA", DuracionCall);
                        conexion.Parameters.AddWithValue("@DURACION_DIALING", DialingCall);
                        conexion.Parameters.AddWithValue("@CODIGO_EJECUTIVO", CodEjecutivo);
                        conexion.Parameters.AddWithValue("@OBSERVACION_GESTION", Observacion);
                        conexion.Parameters.AddWithValue("@CONTACTLIST", NombreCL);
                        conexion.Parameters.AddWithValue("@NOMBRE_EJECUTIVO", NOMBRE_EJECUTIVO);
                        conexion.Parameters.AddWithValue("@EMAIL_EJECUTIVO", EMAIL_EJECUTIVO);
                        conexion.Parameters.AddWithValue("@AGENDA_INTERNA", AGENDA_INTERNA);
                        conexion.Parameters.AddWithValue("@PARTICIPANT_NAME", NombreParticipante);
                        conexion.Parameters.AddWithValue("@PEER_ID", peerParticipante);
                        conexion.Parameters.AddWithValue("@PARTICIPANT_ID", IDParticipante);





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

            public string SPInsertCHREP(string StartInterval, string ConversationID, string NombreCL, string CodigoCC,
                                        string CodCampana, string IDCLIENTE, string ANI, string FechaCall, string DuracionCall,
                                        string DialingCall, string CodEjecutivo, string Observacion, string NOMBRE_EJECUTIVO,
                                        string AGENDA_INTERNA, string EMAIL_EJECUTIVO, string NombreParticipante, string peerParticipante, string IDParticipante)
            {
                string ret;
                try
                {
                    Object returnValue;
                    //cn.Open();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {

                        conn.Open();

                        SqlCommand conexion = new SqlCommand("GenesysCloud_LoadCallHistory_Repro", conn);
                        conexion.CommandType = System.Data.CommandType.StoredProcedure;
                        //coneccion.CommandTimeout = 1;
                        conexion.Parameters.AddWithValue("@START_INTERVAL", StartInterval);
                        conexion.Parameters.AddWithValue("@ID_INTERACCION", ConversationID);
                        conexion.Parameters.AddWithValue("@CODIGO_CALL_CENTER", CodigoCC);
                        conexion.Parameters.AddWithValue("@CODIGO_CAMPANA", CodCampana);
                        conexion.Parameters.AddWithValue("@ID_CLIENTE", IDCLIENTE);
                        conexion.Parameters.AddWithValue("@TELEFONO_GESTIONADO", ANI);
                        conexion.Parameters.AddWithValue("@FECHA_LLAMADO", FechaCall);
                        conexion.Parameters.AddWithValue("@DURACION_LLAMADA", DuracionCall);
                        conexion.Parameters.AddWithValue("@DURACION_DIALING", DialingCall);
                        conexion.Parameters.AddWithValue("@CODIGO_EJECUTIVO", CodEjecutivo);
                        conexion.Parameters.AddWithValue("@OBSERVACION_GESTION", Observacion);
                        conexion.Parameters.AddWithValue("@CONTACTLIST", NombreCL);
                        conexion.Parameters.AddWithValue("@NOMBRE_EJECUTIVO", NOMBRE_EJECUTIVO);
                        conexion.Parameters.AddWithValue("@EMAIL_EJECUTIVO", EMAIL_EJECUTIVO);
                        conexion.Parameters.AddWithValue("@AGENDA_INTERNA", AGENDA_INTERNA);
                        conexion.Parameters.AddWithValue("@PARTICIPANT_NAME", NombreParticipante);
                        conexion.Parameters.AddWithValue("@PEER_ID", peerParticipante);
                        conexion.Parameters.AddWithValue("@PARTICIPANT_ID", IDParticipante);





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

    }
}










