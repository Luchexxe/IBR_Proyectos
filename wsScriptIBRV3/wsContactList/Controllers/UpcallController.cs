using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using wsTeleavenceService.Models;
using wsTeleavenceService.Models.UpdateCallsLocal;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;
using wsTeleavenceService.Global;
using PureCloudPlatform.Client.V2.Extensions;

namespace wsTeleavenceService.Controllers
{
    public class UpcallController : ApiController
    {
        public UpCallResponse Post(UpCallRequest solicitud)
        {
            System.Threading.Thread.Sleep(5000);

            UpCallResponse response = new UpCallResponse();

            int sL_Actual = 0;
            int sL_Destino = 0;
            string sTrama = "";
            string sContactlistId_destino = "";
            string sCampaignId_destino = "";
            string sTabla_destino = "";
            string sOperacion = "";
            string sCampaniaName = "";
            string solWrapupCodeAgent = solicitud.wrapupcodeAgent;
            string solAgentId = solicitud.agentId;
            string solWrapupcodeSystem = solicitud.wrapupcodeSystem??"";
            string solPhonenumber = solicitud.phonenumber;
            string solQueueId = solicitud.queueId;
            string solScriptId = solicitud.scriptId;
            string solParticipantCustId = solicitud.participantCustId;
            string solContactId = solicitud.contactId;
            string solCampaignId = solicitud.campaignId;
            string solContactListId = solicitud.contactlistId;
            string solConversationId = solicitud.conversationId;

            Logger.log.Info("Recibe UpCallControlador:");
            Logger.log.Info("UpCallControlador wrapupAgente: " + solWrapupCodeAgent);
            Logger.log.Info("UpCallControlador contactlistid: " + solContactListId);
            Logger.log.Info("UpCallControlador campaignID: " + solCampaignId);
            Logger.log.Info("UpCallControlador contactid: " + solContactId);
            Logger.log.Info("UpCallControlador wrapupSystem: " + solWrapupcodeSystem);
            Logger.log.Info("UpCallControlador agentid: " + solAgentId);
            Logger.log.Info("UpCallControlador phone: " + solPhonenumber);
            Logger.log.Info("UpCallControlador queueid: " + solQueueId);
            Logger.log.Info("UpCallControlador scriptid: " + solScriptId);
            Logger.log.Info("UpCallControlador participantid: " + solParticipantCustId);
            Logger.log.Info("UpCallControlador ConversationId: " + solConversationId);

            string sLineaContactID;
            string sContactIdDestino = "";
            string sWrapupName = "";
            string dFechaHoraCallback = "";
            string sCampoAux2 = "";
            string sColaId_destino = "";

            try
            {

                #region Generacion de TokenPureCloud
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(VariablesGlobales.idCliente, VariablesGlobales.secretCliente);
                var tokenSource = accessTokenInfo.AccessToken;
                Logger.log.Info("Access token=" + tokenSource);
                #endregion

                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = tokenSource;

                var apiInstanceOutbound = new OutboundApi();
                var conversationsApi = new ConversationsApi();

                //bool statusExist = false;
                string STATUS = "";
                string ATTEMPTS = "";

                //Obtener Contacto
                DialerContact resultDialerContact = apiInstanceOutbound.GetOutboundContactlistContact(solContactListId, solContactId);

                try
                {
                    Dictionary<string, object> DataContactoAnterior = resultDialerContact.Data;
                    ATTEMPTS = DataContactoAnterior["ATTEMPTS"].ToString();
                }
                catch(Exception ex4)
                {
                    Logger.log.Error("Error obteniendo campo ATTEMPTS: " + ex4);
                }

                string wrapupAgentSPGETACTION = "";

                if (solWrapupCodeAgent == "" || solWrapupCodeAgent == null)
                {
                    wrapupAgentSPGETACTION = "123456789";
                    solWrapupCodeAgent = "";
                }
                else
                {
                    wrapupAgentSPGETACTION = solWrapupCodeAgent;
                }

                Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE ENTRADA wrapUpCode: " + wrapupAgentSPGETACTION);

                #region Consulta SP "USP_GET_ACTION_BY_WRAPUPCODE"
                using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "USP_GET_ACTION_BY_WRAPUPCODE";
                    var command = new SqlCommand(query, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@WrapupCodeId", wrapupAgentSPGETACTION);
                    command.Parameters.AddWithValue("@ContactlistId", solContactListId);
                    command.Parameters.AddWithValue("@CampaignId", solCampaignId);

                    command.Parameters.Add("@Trama", SqlDbType.VarChar, 5000);
                    command.Parameters["@Trama"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@ContactlistId_destino", SqlDbType.VarChar, 50);
                    command.Parameters["@ContactlistId_destino"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@CampaignId_destino", SqlDbType.VarChar, 50);
                    command.Parameters["@CampaignId_destino"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@Tabla_destino", SqlDbType.VarChar, 200);
                    command.Parameters["@Tabla_destino"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@Operacion", SqlDbType.VarChar, 20);
                    command.Parameters["@Operacion"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@L_Actual", SqlDbType.Int);
                    command.Parameters["@L_Actual"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@L_Destino", SqlDbType.Int);
                    command.Parameters["@L_Destino"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@CampaignName_actual", SqlDbType.VarChar, 200);
                    command.Parameters["@CampaignName_actual"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@WrapUpCodeName", SqlDbType.VarChar, 500);
                    command.Parameters["@WrapUpCodeName"].Direction = ParameterDirection.Output;

                    command.Parameters.Add("@QueueId_destino", SqlDbType.VarChar, 50);
                    command.Parameters["@QueueId_destino"].Direction = ParameterDirection.Output;

                    try
                    {
                        command.ExecuteNonQuery();

                        sTrama = command.Parameters["@Trama"].Value.ToString();
                        sContactlistId_destino = command.Parameters["@ContactlistId_destino"].Value.ToString();
                        sCampaignId_destino = command.Parameters["@CampaignId_destino"].Value.ToString();
                        sTabla_destino = command.Parameters["@Tabla_destino"].Value.ToString();
                        sOperacion = command.Parameters["@Operacion"].Value.ToString();
                        sL_Actual = (Int32)command.Parameters["@L_Actual"].Value;
                        sL_Destino = (Int32)command.Parameters["@L_Destino"].Value;
                        sCampaniaName = command.Parameters["@CampaignName_actual"].Value.ToString();
                        sWrapupName = command.Parameters["@WrapUpCodeName"].Value.ToString();
                        sColaId_destino = command.Parameters["@QueueId_destino"].Value.ToString();

                        Logger.log.Info("Resultado SP USP_GET_ACTION_BY_WRAPUPCODE");
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE sTrama: " + sTrama);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE sContactlistId_destino: " + sContactlistId_destino);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE sCampaignId_destino: " + sCampaignId_destino);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE sTabla_destino: " + sTabla_destino);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE sOperacion: " + sOperacion);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE sL_Actual: " + sL_Actual);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE sL_Destino: " + sL_Destino);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE Campania Name: " + sCampaniaName);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE wrapUpName: " + sWrapupName);
                        Logger.log.Info("USP_GET_ACTION_BY_WRAPUPCODE wrapUpName: " + sColaId_destino);

                        transaction.Commit();
                        command.Dispose();
                    }
                    catch (Exception ex)
                    {
                        response.respuesta = false;
                        response.detalleRespuesta = ex.Message.ToString();
                        Logger.log.Error("Error: " + ex);

                        transaction.Rollback();

                    }
                    connection.Close();
                    connection.Dispose();
                }
                #endregion

                #region PureCloud
                bool status = false;
                #region Obtener estados de tipificaciones de sistema

                if (solWrapupcodeSystem != "ININ-OUTBOUND-TRANSFERRED-TO-QUEUE")
                {
                    if (solWrapupcodeSystem != "ININ-OUTBOUND-CAMPAIGN-RECYCLE-CANCELLED-RECALL")
                    {
                        string sStatusSystem = "";
                        bool bIncrAttempts = false;
                        using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                        {
                            try
                            {
                                connection.Open();

                                var transaction = connection.BeginTransaction();

                                var query = "USP_GET_BEHAVIOR_BY_WRAPUPSYSTEM";
                                var command = new SqlCommand(query, connection);
                                command.CommandType = CommandType.StoredProcedure;

                                command.Transaction = transaction;
                                command.Parameters.AddWithValue("@sWrapupSystem", solWrapupcodeSystem);


                                command.Parameters.Add("@sStatus", SqlDbType.VarChar, 1);
                                command.Parameters["@sStatus"].Direction = ParameterDirection.Output;

                                command.Parameters.Add("@bIncrAttempts", SqlDbType.Bit);
                                command.Parameters["@bIncrAttempts"].Direction = ParameterDirection.Output;
                                try
                                {
                                    command.ExecuteNonQuery();
                                    sStatusSystem = command.Parameters["@sStatus"].Value.ToString();
                                    bIncrAttempts = (Boolean)command.Parameters["@bIncrAttempts"].Value;

                                    transaction.Commit();
                                    Logger.log.Info("Transacción correcta SP obtener status Tip. System: STATUS =" + sStatusSystem);
                                    Logger.log.Info("Transacción correcta SP obtener status Tip. System: INCREM_INTENTOS= " + bIncrAttempts.ToString());
                                    
                                }
                                catch (Exception ex9)
                                {
                                    Logger.log.Error("Error ejecutar SP obtener status Tip. System: " + ex9);
                                    transaction.Rollback();

                                }

                            }
                            catch (Exception ex6)
                            {
                                Logger.log.Error("Error obteniendo estados de tip. sistema: " + ex6);
                            }

                            

                        }
                        try
                        {
                            DialerContact BodyActualizarContact = new DialerContact();

                            if (sStatusSystem == "U")
                            {
                                if (bIncrAttempts)
                                {
                                    Dictionary<string, object> DataContactoActual = resultDialerContact.Data;
                                    var sATTEMPTS1 = DataContactoActual["ATTEMPTS"].ToString();

                                    int ATTEMPTS1 = Int32.Parse(sATTEMPTS1);

                                    ATTEMPTS1 = ATTEMPTS1 + 1;


                                    Dictionary<string, object> DataContactoActualizar = new Dictionary<string, object>();

                                    DataContactoActualizar.Add("ATTEMPTS", ATTEMPTS1.ToString());

                                    foreach (var y in DataContactoActualizar)
                                    {
                                        foreach (var z in DataContactoActual)
                                        {
                                            if (y.Key == z.Key)
                                            {
                                                DataContactoActual[z.Key] = y.Value;
                                                break;
                                            }
                                        }
                                    }

                                    BodyActualizarContact.Data = DataContactoActual;

                                    ATTEMPTS = ATTEMPTS1.ToString();
                                }

                                BodyActualizarContact.Callable = false;
                                apiInstanceOutbound.PutOutboundContactlistContact(solContactListId, solContactId, BodyActualizarContact);

                                
                            }
                            else if (sStatusSystem == "C")
                            {
                                if (bIncrAttempts)
                                {
                                    Dictionary<string, object> DataContactoActual = resultDialerContact.Data;
                                    var sATTEMPTS1 = DataContactoActual["ATTEMPTS"].ToString();

                                    int ATTEMPTS1 = Int32.Parse(sATTEMPTS1);


                                    Dictionary<string, object> DataContactoActualizar = new Dictionary<string, object>();

                                    DataContactoActualizar.Add("ATTEMPTS", ATTEMPTS1.ToString());

                                    foreach (var y in DataContactoActualizar)
                                    {
                                        foreach (var z in DataContactoActual)
                                        {
                                            if (y.Key == z.Key)
                                            {
                                                DataContactoActual[z.Key] = y.Value;
                                                break;
                                            }
                                        }
                                    }

                                    BodyActualizarContact.Data = DataContactoActual;

                                    ATTEMPTS = ATTEMPTS1.ToString();
                                }

                                BodyActualizarContact.Callable = true;
                                apiInstanceOutbound.PutOutboundContactlistContact(solContactListId, solContactId, BodyActualizarContact);
                            }
                            else
                            {
                                //
                            }
                        }
                        catch(Exception ex8)
                        {
                            Logger.log.Error("Error seteo Estado e Intentos: " + ex8);
                        }

                        status = true;

                    }
                }
                

                #endregion

                    #region obtener datos callback
                    //bool status = false;
                try
                {
                    if (solWrapupcodeSystem != "ININ-OUTBOUND-CAMPAIGN-RECYCLE-CANCELLED-RECALL")
                    {
                        CallbackConversation resultCallback = conversationsApi.GetConversationsCallback(solConversationId);

                        if (resultCallback.Participants != null)
                        {
                            Logger.log.Info("Existen participantes API Callback");

                            foreach (CallbackMediaParticipant x in resultCallback.Participants)
                            {
                                if (x.Purpose.Equals("customer"))
                                {
                                    Logger.log.Info("ParticipantID Callback Customer: " + x.Id);
                                    DateTime dateTime = DateTime.MinValue;
                                    if (x.CallbackScheduledTime != null)
                                    {
                                         dateTime = x.CallbackScheduledTime.Value;
                                    }
                                    dateTime = dateTime.AddHours(-5);
                                    Logger.log.Info("Consulta Callback - HoraCallback: " + dateTime.ToString("yyyy-MM-ddTHH:mm:ss"));


                                    var utilitiesApi = new UtilitiesApi();

                                    var DateTimeGenesysNow = utilitiesApi.GetDate();
                                    var dDateTimeGenesysNow = DateTimeGenesysNow.CurrentDate.Value;

                                    var saveUtcNow = dDateTimeGenesysNow.AddHours(-5);
                                    Logger.log.Info("Consulta Callback - Hora Actual: " + saveUtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));

                                    int res = DateTime.Compare(dateTime, saveUtcNow);

                                    if (res > 0)
                                    {
                                        dFechaHoraCallback = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");

                                        STATUS = "S";
                                        status = true;



                                        if (solWrapupCodeAgent != "")
                                        {
                                            //***************************************************************
                                            // marca para agendamientos
                                            //***************************************************************
                                            var apiInstanceOut = new OutboundApi();
                                            DialerContact contactActualizar = new DialerContact();
                                            contactActualizar = apiInstanceOut.GetOutboundContactlistContact(solContactListId, solContactId);
                                            Dictionary<string, object> DataContactActualizar = contactActualizar.Data;

                                            var Body = new DialerContact();
                                            Body.Callable = true;

                                            Dictionary<string, object> DataNewActualizar = new Dictionary<string, object>();
                                            DataNewActualizar.Add("JERARQUIALLAMADA", "1");

                                            foreach (var y in DataNewActualizar)
                                            {
                                                foreach (var z in DataContactActualizar)
                                                {
                                                    if (y.Key == z.Key)
                                                    {
                                                        DataContactActualizar[z.Key] = y.Value;
                                                        break;
                                                    }
                                                }
                                            }

                                            Body.Data = DataContactActualizar;

                                            DialerContact result = apiInstanceOut.PutOutboundContactlistContact(solContactListId, solContactId, Body);


                                        }


                                    }

                                    //*********************************************************

                                }
                            }
                        }

                    }

                    
                }
                catch (Exception ex5)
                {
                    Logger.log.Error("Error proceso **Obtener datos del callback**: " + ex5);
                }

                #endregion

                if (!status)
                {
                    STATUS = (resultDialerContact.Callable == true) ? "C" : "U";
                }


                if (sOperacion.Equals("INSERT"))
                {

                    var contactIdFront = "";
                    #region Generacion de TokenPureCloud
                    //var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(VariablesGlobales.idCliente, VariablesGlobales.secretCliente);
                    //var token_PC = accessTokenInfo.AccessToken;
                    //Logger.log.Info("Access token=" + tokenSource);

                    //PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = tokenSource;
                    var apiInstance = new OutboundApi();
                    #endregion


                    DialerContact contactAnterior = apiInstance.GetOutboundContactlistContact(solContactListId, solContactId);

                    sLineaContactID = (sL_Actual == 1) ? "CONTACTID2" : "CONTACTID3";
                    sContactIdDestino = Guid.NewGuid().ToString();

                    List<WritableDialerContact> bodyNewContact = new List<WritableDialerContact>();
                    var contactListNuevo = new WritableDialerContact();
                    contactListNuevo.Id = sContactIdDestino;
                    contactListNuevo.ContactListId = sContactlistId_destino;

                    if (sCampaniaName.Substring(sCampaniaName.Length - 1, 1) == "1")
                    {
                        contactAnterior.Data["CONTACIDFRONT"] = solContactId;
                        contactAnterior.Data["ATTEMPTS"] = "0";
                        contactIdFront = solContactId;

                        sCampoAux2 = contactAnterior.Data["AUX2"].ToString();
                    }

                    if (sCampaniaName.Substring(sCampaniaName.Length - 1, 1) == "2")
                    {
                        contactIdFront = contactAnterior.Data["CONTACIDFRONT"].ToString();
                        contactAnterior.Data["ATTEMPTS"] = "0";

                    }

                    contactListNuevo.Data = contactAnterior.Data;
                    bodyNewContact.Add(contactListNuevo);

                    var priority = true;
                    var clearSystemData = true;
                    List<DialerContact> resultado = apiInstance.PostOutboundContactlistContacts(sContactlistId_destino, bodyNewContact, priority, clearSystemData);

                    string sqlInsert = "UPDATE " + sTabla_destino + " SET  " + sLineaContactID + "=\'" + sContactIdDestino + "\' WHERE CONTACTID" + sL_Actual + "=\'" + solContactId + "\';";
                    Logger.log.Info("SQL INSERT: " + sqlInsert);
                    SqlCommand command;

                    #region Envio SQLINSERT
                    using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                    {
                        try
                        {
                            connection.Open();
                            command = new SqlCommand(sqlInsert, connection);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                        catch (Exception ex)
                        {
                            response.respuesta = false;
                            response.detalleRespuesta = ex.Message.ToString();

                            Logger.log.Error("Error SQL INSERT : " + ex);
                        }
                        connection.Close();
                        connection.Dispose();
                    }
                    #endregion

                    #region Consulta SP "USP_UPDATE_TABLES_LISTAS"
                    using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query = "USP_UPDATE_TABLES_LISTAS";
                            var command2 = new SqlCommand(query, connection);
                            command2.CommandType = System.Data.CommandType.StoredProcedure;

                            command2.Transaction = transaction;
                            command2.Parameters.AddWithValue("@contactId", solContactId);
                            command2.Parameters.AddWithValue("@nombre_tabla", sTabla_destino);
                            command2.Parameters.AddWithValue("@conversationId", solConversationId);
                            command2.Parameters.AddWithValue("@agentId", solAgentId);
                            command2.Parameters.AddWithValue("@wrapupcodeAgent", solWrapupCodeAgent);
                            command2.Parameters.AddWithValue("@wrapupcodeSystem", solWrapupcodeSystem);
                            command2.Parameters.AddWithValue("@phonenumber", solPhonenumber);
                            command2.Parameters.AddWithValue("@linea_actual", sL_Actual);
                            command2.Parameters.AddWithValue("@contactlistId", solContactListId);
                            command2.Parameters.AddWithValue("@campaignId", solCampaignId);
                            command2.Parameters.AddWithValue("@queueId", solQueueId);
                            command2.Parameters.AddWithValue("@scriptId", solScriptId);
                            command2.Parameters.AddWithValue("@participantCustId", solParticipantCustId);
                            command2.Parameters.AddWithValue("@custom1", (solicitud.customString1 == null) ? "" : solicitud.customString1);
                            command2.Parameters.AddWithValue("@custom2", (solicitud.customString2 == null) ? "" : solicitud.customString2);
                            command2.Parameters.AddWithValue("@custom3", (solicitud.customString3 == null) ? "" : solicitud.customString3);

                            try
                            {
                                command2.ExecuteNonQuery();
                                transaction.Commit();
                                response.respuesta = true;
                                response.detalleRespuesta = "Recibido satisfactoriamente update TABLE LISTAS";

                            }
                            catch (Exception ex)
                            {
                                response.respuesta = false;
                                response.detalleRespuesta = ex.Message.ToString();

                                Logger.log.Error("Error UPDATE Table Listas: " + ex);
                                transaction.Rollback();
                            }
                        }
                        catch (Exception ex5)
                        {
                            response.detalleRespuesta = ex5.Message.ToString();
                            Logger.log.Error("Error UPDATE Table Listas: " + ex5);
                        }

                    }
                    #endregion


                    #region EnvioTipificacionTeleavance

                    var datenow = DateTime.Now;

                    Logger.log.Info("SP_I_TIPIFICACION - ContactID: " + solContactId);
                    Logger.log.Info("SP_I_TIPIFICACION - Campania: " + sCampaniaName);
                    Logger.log.Info("SP_I_TIPIFICACION - Agente: " + solAgentId);
                    Logger.log.Info("SP_I_TIPIFICACION - STATUS: " + STATUS);
                    Logger.log.Info("SP_I_TIPIFICACION - ATTEMTPS: " + ATTEMPTS);
                    Logger.log.Info("SP_I_TIPIFICACION - vNombreAgente: " + "");
                    Logger.log.Info("SP_I_TIPIFICACION - vDniAgente: " + "");
                    Logger.log.Info("SP_I_TIPIFICACION - Fecha llamada: " + datenow.ToString("yyyy-MM-ddTHH:mm:ss"));
                    Logger.log.Info("SP_I_TIPIFICACION - Tipificacion: " + sWrapupName);
                    Logger.log.Info("SP_I_TIPIFICACION - Nro llamada: " + solPhonenumber);
                    Logger.log.Info("SP_I_TIPIFICACION - ConversationID: " + solConversationId);
                    Logger.log.Info("SP_I_TIPIFICACION - ContactIDFront: " + contactIdFront);
                    Logger.log.Info("SP_I_TIPIFICACION - Tramo0: " + sCampoAux2);

                    using (SqlConnection connection = ConnexionBDTeleavance.connexionBDDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query = "SP_I_TIPIFICACION";
                            command = new SqlCommand(query, connection);
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@nI3_IDENTITY", solContactId);
                            command.Parameters.AddWithValue("@CAMPANA", sCampaniaName);
                            command.Parameters.AddWithValue("@STATUS", STATUS);
                            command.Parameters.AddWithValue("@ATTEMPTS", ATTEMPTS);
                            command.Parameters.AddWithValue("@vUserAgente", solAgentId);
                            command.Parameters.AddWithValue("@vNombreAgente", "");
                            command.Parameters.AddWithValue("@vDniAgente", "");
                            command.Parameters.AddWithValue("@dFechaLlamada", datenow.ToString("yyyy-MM-ddTHH:mm:ss"));
                            command.Parameters.AddWithValue("@vTipificacion", sWrapupName);
                            command.Parameters.AddWithValue("@vNroLlamado", solPhonenumber);
                            command.Parameters.AddWithValue("@vCallidKey", solConversationId);
                            command.Parameters.AddWithValue("@nIDContactFront", contactIdFront);
                            command.Parameters.AddWithValue("@dFechaHoraCallback", dFechaHoraCallback);
                            command.Parameters.AddWithValue("@vTramo", sCampoAux2);

                            command.Parameters.Add("@vmsg", SqlDbType.NVarChar, 255);
                            command.Parameters["@vmsg"].Direction = ParameterDirection.Output;
                            try
                            {
                                command.ExecuteNonQuery();

                                //response.respuesta = true;
                                //response.detalleRespuesta = "Recibido satisfactoriamente";
                                string sMensaje = command.Parameters["@vmsg"].Value.ToString();

                                //Responses.Add(response);
                                transaction.Commit();
                                response.respuesta = true;
                                response.detalleRespuesta = "OK";
                                Logger.log.Info("Transacción correcta SP " + sMensaje);
                                //log.Info("Transacción correcta - Identity = " + response.result_desc);
                            }
                            catch (Exception ex)
                            {
                                //Restore.Debug(Trama);

                                //log.Error(ex.StackTrace);
                                response.respuesta = false;
                                response.detalleRespuesta = ex.Message.ToString();

                                Logger.log.Error("Error Envio INSERT Teleavance: " + ex);
                                //Responses.Add(response);
                                transaction.Rollback();

                            }
                        }
                        catch (Exception ex2)
                        {
                            response.respuesta = false;
                            response.detalleRespuesta = ex2.Message.ToString();

                            Logger.log.Error("Error Envio INSERT Teleavance: " + ex2);
                        }

                    }
                    #endregion
                }
                else if (sOperacion.Equals("UPDATE"))
                {
                    string updateUserID = "";
                    string updateQueueID = "";
                    string updateScriptID = "";
                    string updatePhoneNumber = "";
                    string updateParticipantID = "";
                    string updateConversationID = "";
                    string updateContactID_destino = "";

                    string sColaId_callback = "";

                    var apiInstance = new OutboundApi();

                    #region Consulta SP "USP_GET_DATA_FOR_UPDATE"
                    using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query = "USP_GET_DATA_FOR_UPDATE";
                            var command = new SqlCommand(query, connection);
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@ContactId", solContactId);
                            command.Parameters.AddWithValue("@L_Destino", sL_Destino);
                            command.Parameters.AddWithValue("@L_Actual", sL_Actual);
                            command.Parameters.AddWithValue("@Tabla_destino", sTabla_destino);

                            command.Parameters.Add("@UserID", SqlDbType.VarChar, 50);
                            command.Parameters["@UserID"].Direction = ParameterDirection.Output;

                            command.Parameters.Add("@QueueID", SqlDbType.VarChar, 50);
                            command.Parameters["@QueueID"].Direction = ParameterDirection.Output;

                            command.Parameters.Add("@ScriptID", SqlDbType.VarChar, 50);
                            command.Parameters["@ScriptID"].Direction = ParameterDirection.Output;

                            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 50);
                            command.Parameters["@PhoneNumber"].Direction = ParameterDirection.Output;

                            command.Parameters.Add("@ParticipantID", SqlDbType.VarChar, 50);
                            command.Parameters["@ParticipantID"].Direction = ParameterDirection.Output;

                            command.Parameters.Add("@ConversationID", SqlDbType.VarChar, 50);
                            command.Parameters["@ConversationID"].Direction = ParameterDirection.Output;

                            command.Parameters.Add("@ContactID_destino", SqlDbType.VarChar, 50);
                            command.Parameters["@ContactID_destino"].Direction = ParameterDirection.Output;

                            try
                            {
                                command.ExecuteNonQuery();

                                updateUserID = command.Parameters["@UserID"].Value.ToString();
                                updateQueueID = command.Parameters["@QueueID"].Value.ToString();
                                updateScriptID = command.Parameters["@ScriptID"].Value.ToString();
                                updatePhoneNumber = command.Parameters["@PhoneNumber"].Value.ToString();
                                updateParticipantID = command.Parameters["@ParticipantID"].Value.ToString();
                                updateConversationID = command.Parameters["@ConversationID"].Value.ToString();
                                updateContactID_destino = command.Parameters["@ContactID_destino"].Value.ToString();


                                Logger.log.Info("Resultado SP USP_GET_DATA_FOR_UPDATE");
                                Logger.log.Info("USP_GET_DATA_FOR_UPDATE updateUserID: " + updateUserID);
                                Logger.log.Info("USP_GET_DATA_FOR_UPDATE updateQueueID: " + updateQueueID);
                                Logger.log.Info("USP_GET_DATA_FOR_UPDATE updateScriptID: " + updateScriptID);
                                Logger.log.Info("USP_GET_DATA_FOR_UPDATE updatePhoneNumber: " + updatePhoneNumber);
                                Logger.log.Info("USP_GET_DATA_FOR_UPDATE updateParticipantID: " + updateParticipantID);
                                Logger.log.Info("USP_GET_DATA_FOR_UPDATE updateConversationID: " + updateConversationID);



                                sContactIdDestino = updateContactID_destino;

                                transaction.Commit();
                                command.Dispose();
                            }
                            catch (Exception ex)
                            {
                                response.respuesta = false;
                                response.detalleRespuesta = ex.Message.ToString();
                                Logger.log.Error("Error SP UPDATE: " + ex);

                                transaction.Rollback();

                            }
                            connection.Close();
                            connection.Dispose();
                        }
                        catch (Exception ex6)
                        {
                            Logger.log.Error("Error SP UPDATE: " + ex6);
                        }

                    }
                    #endregion

                    #region Obtener campos Contactlist Actual
                    var campoAux1 = "";
                    try
                    {
                        DialerContact contactIDActual = apiInstance.GetOutboundContactlistContact(solContactListId, solContactId);
                        campoAux1 = contactIDActual.Data["CONTACIDFRONT"].ToString();
                        Logger.log.Info("Axuliar obtenido: " + campoAux1);
                    }
                    catch (Exception ex7)
                    {
                        Logger.log.Error("Error obteniendo campo AUX de contactlist: " + ex7);
                    }
                    #endregion

                    #region actualizacion PureCloud Contacto Callable true 
                    DialerContact contactActualizar = new DialerContact();
                    ContactList contactList = new ContactList();
                    try
                    {
                        contactList = apiInstance.GetOutboundContactlist(sContactlistId_destino, false, false);

                        contactActualizar = apiInstance.GetOutboundContactlistContact(sContactlistId_destino, updateContactID_destino);
                        Dictionary<string, object> DataContactActualizar = contactActualizar.Data;

                        var Body = new DialerContact();
                        Body.Callable = true;

                        Dictionary<string, object> DataNewActualizar = new Dictionary<string, object>();
                        DataNewActualizar.Add("JERARQUIALLAMADA", "1");
                        DataNewActualizar.Add("AUX2", "TRAMO0");

                        foreach (var x in DataNewActualizar)
                        {
                            foreach (var z in DataContactActualizar)
                            {
                                if (x.Key == z.Key)
                                {
                                    DataContactActualizar[z.Key] = x.Value;
                                    break;
                                }
                            }
                        }

                        Body.Data = DataContactActualizar;

                        DialerContact result = apiInstance.PutOutboundContactlistContact(sContactlistId_destino, updateContactID_destino, Body);
                    }
                    catch (Exception ex8)
                    {
                        Logger.log.Error("Error Actualizar Contact (callable) línea anterior: " + ex8);
                    }

                    #endregion

                    #region Obtener Cola Callback

                    using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                    {
                        connection.Open();

                        var transaction = connection.BeginTransaction();

                        var query = "USP_GET_QUEUECALLBACK_BY_CAMPAIGN";
                        var command = new SqlCommand(query, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@CampaignId", solCampaignId);

                        command.Parameters.Add("@QueueId_callback", SqlDbType.VarChar, 50);
                        command.Parameters["@QueueId_callback"].Direction = ParameterDirection.Output;

                        try
                        {
                            command.ExecuteNonQuery();

                            sColaId_callback = command.Parameters["@QueueId_callback"].Value.ToString();

                            Logger.log.Info("Resultado SP USP_GET_QUEUECALLBACK_BY_CAMPAIGN");
                            Logger.log.Info("USP_GET_QUEUECALLBACK_BY_CAMPAIGN QueueID: " + sColaId_callback);

                            transaction.Commit();
                            command.Dispose();
                        }
                        catch (Exception ex2)
                        {
                            Logger.log.Error("Error: " + ex2);

                            transaction.Rollback();

                        }
                        connection.Close();
                        connection.Dispose();
                    }

                    #endregion

                    #region agendamiento Purecloud

                    try
                    {
                        //Lista de Numeros a Llamar
                        List<string> listNumerosLlamar = new List<string>();

                        updatePhoneNumber = updatePhoneNumber.Substring(4, updatePhoneNumber.Length - 4);
                        listNumerosLlamar.Add(updatePhoneNumber);
                        Logger.log.Info("Teléfono a agendar: " + updatePhoneNumber);


                        //Fecha de Agendamiento
                        DateTime horaAgendamiento = DateTime.Now.AddMinutes(1);
                        Logger.log.Info("Hora de agendamiento: " + horaAgendamiento.ToString());

                        //Lista de Agentes Preferidos
                        List<string> listAgente = new List<string>();
                        listAgente.Add(updateUserID);
                        Logger.log.Info("Id Agente A Agendar: " + updateUserID);


                        CreateCallbackOnConversationCommand createCallback = new CreateCallbackOnConversationCommand();

                        RoutingData routingCallback = new RoutingData();
                        routingCallback.QueueId = sColaId_callback;
                        routingCallback.PreferredAgentIds = listAgente;
                        routingCallback.Priority = 0;

                        createCallback.ScriptId = updateScriptID;
                        createCallback.QueueId = sColaId_callback;
                        createCallback.RoutingData = routingCallback;
                        createCallback.CallbackNumbers = listNumerosLlamar;
                        createCallback.CallbackScheduledTime = horaAgendamiento;

                        conversationsApi.PostConversationParticipantCallbacks(updateConversationID, updateParticipantID, createCallback);

                        Logger.log.Info("Ejecutado correctamente: " + horaAgendamiento.ToString());
                    }
                    catch (Exception ex9)
                    {
                        Logger.log.Error("Error Agendamiento de registro: " + ex9);
                    }

                    #endregion


                    Logger.log.Info("Ejecutar USP_UPDATE_TABLES_LISTAS: ");


                    #region Consulta SP "USP_UPDATE_TABLES_LISTAS"
                    using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                    {
                        connection.Open();

                        var transaction = connection.BeginTransaction();

                        var query = "USP_UPDATE_TABLES_LISTAS";
                        var command = new SqlCommand(query, connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@contactId", solContactId);
                        command.Parameters.AddWithValue("@nombre_tabla", sTabla_destino);
                        command.Parameters.AddWithValue("@conversationId", solConversationId);
                        command.Parameters.AddWithValue("@agentId", solAgentId);
                        command.Parameters.AddWithValue("@wrapupcodeAgent", solWrapupCodeAgent);
                        command.Parameters.AddWithValue("@wrapupcodeSystem", solWrapupcodeSystem??"");
                        command.Parameters.AddWithValue("@phonenumber", updatePhoneNumber);
                        command.Parameters.AddWithValue("@linea_actual", sL_Actual);
                        command.Parameters.AddWithValue("@contactlistId", solContactListId);
                        command.Parameters.AddWithValue("@campaignId", solCampaignId);
                        command.Parameters.AddWithValue("@queueId", solQueueId);
                        command.Parameters.AddWithValue("@scriptId", solScriptId);
                        command.Parameters.AddWithValue("@participantCustId", solParticipantCustId);
                        command.Parameters.AddWithValue("@custom1", (solicitud.customString1 == null) ? "" : solicitud.customString1);
                        command.Parameters.AddWithValue("@custom2", (solicitud.customString2 == null) ? "" : solicitud.customString2);
                        command.Parameters.AddWithValue("@custom3", (solicitud.customString3 == null) ? "" : solicitud.customString3);

                        try
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            response.respuesta = true;
                            response.detalleRespuesta = "Recibido satisfactoriamente";

                        }
                        catch (Exception ex)
                        {
                            response.respuesta = false;
                            response.detalleRespuesta = ex.Message.ToString();

                            Logger.log.Error("Error UPDATE Table Listas: " + ex);
                            transaction.Rollback();
                        }
                    }
                    #endregion



                    #region EnvioTipificacionTeleavance

                    var datenow = DateTime.Now;

                    Logger.log.Info("SP_I_TIPIFICACION - ContactID: " + solContactId);
                    Logger.log.Info("SP_I_TIPIFICACION - Campania: " + sCampaniaName);
                    Logger.log.Info("SP_I_TIPIFICACION - Agente: " + solAgentId);
                    Logger.log.Info("SP_I_TIPIFICACION - STATUS: " + STATUS);
                    Logger.log.Info("SP_I_TIPIFICACION - ATTEMPTS: " + ATTEMPTS);
                    Logger.log.Info("SP_I_TIPIFICACION - vNombreAgente: " + "");
                    Logger.log.Info("SP_I_TIPIFICACION - vDniAgente: " + "");
                    Logger.log.Info("SP_I_TIPIFICACION - Fecha llamada: " + datenow.ToString("yyyy-MM-ddTHH:mm:ss"));
                    Logger.log.Info("SP_I_TIPIFICACION - Tipificacion: " + sWrapupName);
                    Logger.log.Info("SP_I_TIPIFICACION - Nro llamada: " + updatePhoneNumber);
                    Logger.log.Info("SP_I_TIPIFICACION - ConversationID: " + solConversationId);
                    Logger.log.Info("SP_I_TIPIFICACION - ContactIDFront: " + campoAux1);
                    Logger.log.Info("SP_I_TIPIFICACION - dFechaHoraCallback: " + dFechaHoraCallback);
                    Logger.log.Info("SP_I_TIPIFICACION - vTramo: " + sCampoAux2);

                    using (SqlConnection connection = ConnexionBDTeleavance.connexionBDDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query = "SP_I_TIPIFICACION";
                            var command = new SqlCommand(query, connection);
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@nI3_IDENTITY", solContactId);
                            command.Parameters.AddWithValue("@CAMPANA", sCampaniaName);
                            command.Parameters.AddWithValue("@STATUS", STATUS);
                            command.Parameters.AddWithValue("@ATTEMPTS", ATTEMPTS);
                            command.Parameters.AddWithValue("@vUserAgente", solAgentId);
                            command.Parameters.AddWithValue("@vNombreAgente", "");
                            command.Parameters.AddWithValue("@vDniAgente", "");
                            command.Parameters.AddWithValue("@dFechaLlamada", datenow.ToString("yyyy-MM-ddTHH:mm:ss"));
                            command.Parameters.AddWithValue("@vTipificacion", sWrapupName);
                            command.Parameters.AddWithValue("@vNroLlamado", updatePhoneNumber);
                            command.Parameters.AddWithValue("@vCallidKey", solConversationId);
                            command.Parameters.AddWithValue("@nIDContactFront", campoAux1);
                            command.Parameters.AddWithValue("@dFechaHoraCallback", dFechaHoraCallback);
                            command.Parameters.AddWithValue("@vTramo", sCampoAux2);

                            command.Parameters.Add("@vmsg", SqlDbType.NVarChar, 255);
                            command.Parameters["@vmsg"].Direction = ParameterDirection.Output;
                            try
                            {
                                command.ExecuteNonQuery();

                                //response.respuesta = true;
                                //response.detalleRespuesta = "Recibido satisfactoriamente";
                                string sMensaje = command.Parameters["@vmsg"].Value.ToString();

                                //Responses.Add(response);
                                transaction.Commit();
                                response.respuesta = true;
                                response.detalleRespuesta = "OK";
                                Logger.log.Info("Transacción correcta SP " + sMensaje);
                                //log.Info("Transacción correcta - Identity = " + response.result_desc);
                            }
                            catch (Exception ex)
                            {
                                //Restore.Debug(Trama);

                                //log.Error(ex.StackTrace);
                                response.respuesta = false;
                                response.detalleRespuesta = ex.Message.ToString();

                                Logger.log.Error("Error UPDATE Teleavance: " + ex);
                                //Responses.Add(response);
                                transaction.Rollback();

                            }
                        }
                        catch (Exception ex2)
                        {
                            response.respuesta = false;
                            response.detalleRespuesta = ex2.Message.ToString();

                            Logger.log.Error("Error Envio INSERT Teleavance: " + ex2);
                        }

                    }
                    #endregion

                }
                else
                {
                    var campoAux1 = "";
                    var wrapup = "";
                    Logger.log.Info("Access token=" + tokenSource);

                    try
                    {
                        //PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = tokenSource;

                        if (solWrapupCodeAgent != "")
                        {
                            //wrapup = solicitud.wrapupcodeAgent;
                            if (solWrapupCodeAgent == "ININ-WRAP-UP-TIMEOUT")
                            {
                                wrapup = "ININ-WRAP-UP-TIMEOUT";
                            }
                            else if (solWrapupCodeAgent.Substring(0, 13) != "ININ-OUTBOUND")
                            {
                                #region Obtener wrapUpCodeName
                                var apiInsRouting = new RoutingApi();

                                WrapupCode wrapupCodeActual = apiInsRouting.GetRoutingWrapupcode(solWrapupCodeAgent);
                                wrapup = wrapupCodeActual.Name;

                                #endregion
                            }

                        }
                        else
                        {
                            if (solWrapupcodeSystem != "")
                            {
                                wrapup = solWrapupcodeSystem;
                            }
                        }

                        if (sCampaniaName != "")
                        {
                            if ((sCampaniaName.Substring(sCampaniaName.Length - 1, 1) == "2") || (sCampaniaName.Substring(sCampaniaName.Length - 1, 1) == "3"))
                            {


                                #region Obtener campos Contactlist Actual
                                //ContactList contactListActual = apiInstance.GetOutboundContactlist(solContactListId, false, false);
                                var apiInstance = new OutboundApi();
                                DialerContact contactIDActual = apiInstance.GetOutboundContactlistContact(solContactListId, solContactId);
                                campoAux1 = contactIDActual.Data["CONTACIDFRONT"].ToString();

                                sCampoAux2 = contactIDActual.Data["AUX2"].ToString();

                                #endregion
                            }
                            else
                            {
                                campoAux1 = solContactId;
                            }
                        }

                        #region EnvioTipificacionTeleavance

                        var datenow = DateTime.Now;



                        Logger.log.Info("SP_I_TIPIFICACION - ContactID: " + solContactId);
                        Logger.log.Info("SP_I_TIPIFICACION - Campania: " + sCampaniaName);
                        Logger.log.Info("SP_I_TIPIFICACION - Agente: " + solAgentId);
                        Logger.log.Info("SP_I_TIPIFICACION - STATUS: " + STATUS);
                        Logger.log.Info("SP_I_TIPIFICACION - ATTEMPTS: " + ATTEMPTS);
                        Logger.log.Info("SP_I_TIPIFICACION - vNombreAgente: " + "");
                        Logger.log.Info("SP_I_TIPIFICACION - vDniAgente: " + "");
                        Logger.log.Info("SP_I_TIPIFICACION - Fecha llamada: " + datenow.ToString("yyyy-MM-ddTHH:mm:ss"));
                        Logger.log.Info("SP_I_TIPIFICACION - Tipificacion: " + wrapup);
                        Logger.log.Info("SP_I_TIPIFICACION - Nro llamada: " + solPhonenumber);
                        Logger.log.Info("SP_I_TIPIFICACION - ConversationID: " + solConversationId);
                        Logger.log.Info("SP_I_TIPIFICACION - ContactIDFront: " + campoAux1);
                        Logger.log.Info("SP_I_TIPIFICACION - Fecha Callback: " + dFechaHoraCallback);
                        Logger.log.Info("SP_I_TIPIFICACION - Tramo0: " + sCampoAux2);


                        using (SqlConnection connection = ConnexionBDTeleavance.connexionBDDTeleavance())
                        {
                            try
                            {
                                connection.Open();

                                var transaction = connection.BeginTransaction();

                                var query = "SP_I_TIPIFICACION";
                                var command = new SqlCommand(query, connection);
                                command.CommandType = CommandType.StoredProcedure;

                                command.Transaction = transaction;
                                command.Parameters.AddWithValue("@nI3_IDENTITY", (solContactId ?? "").ToString());
                                command.Parameters.AddWithValue("@CAMPANA", sCampaniaName);
                                command.Parameters.AddWithValue("@STATUS", STATUS);
                                command.Parameters.AddWithValue("@ATTEMPTS", ATTEMPTS);
                                command.Parameters.AddWithValue("@vUserAgente", (solAgentId ?? "").ToString());
                                command.Parameters.AddWithValue("@vNombreAgente", "");
                                command.Parameters.AddWithValue("@vDniAgente", "");
                                command.Parameters.AddWithValue("@dFechaLlamada", datenow.ToString("yyyy-MM-ddTHH:mm:ss"));
                                command.Parameters.AddWithValue("@vTipificacion", wrapup);
                                command.Parameters.AddWithValue("@vNroLlamado", (solPhonenumber ?? "").ToString());
                                command.Parameters.AddWithValue("@vCallidKey", (solConversationId ?? "").ToString());
                                command.Parameters.AddWithValue("@nIDContactFront", campoAux1);
                                command.Parameters.AddWithValue("@dFechaHoraCallback", dFechaHoraCallback);
                                command.Parameters.AddWithValue("@vTramo", sCampoAux2);

                                command.Parameters.Add("@vmsg", SqlDbType.VarChar, 255);
                                command.Parameters["@vmsg"].Direction = ParameterDirection.Output;
                                try
                                {
                                    command.ExecuteNonQuery();

                                    //response.respuesta = true;
                                    //response.detalleRespuesta = "Recibido satisfactoriamente";
                                    string sMensaje = command.Parameters["@vmsg"].Value.ToString();

                                    //Responses.Add(response);
                                    transaction.Commit();
                                    response.respuesta = true;
                                    response.detalleRespuesta = "OK";
                                    Logger.log.Info("Transacción correcta SP " + sMensaje);
                                }
                                catch (Exception ex)
                                {
                                    //Restore.Debug(Trama);

                                    //log.Error(ex.StackTrace);
                                    response.respuesta = false;
                                    response.detalleRespuesta = ex.Message.ToString();
                                    Logger.log.Error("Error Envio Teleavance: " + ex);

                                    //Responses.Add(response);
                                    transaction.Rollback();

                                }
                            }
                            catch (Exception ex2)
                            {
                                response.respuesta = false;
                                response.detalleRespuesta = ex2.Message.ToString();

                                Logger.log.Error("Error Envio Teleavance: " + ex2);
                            }

                        }
                        #endregion
                    }
                    catch (Exception e4)
                    {
                        response.respuesta = false;
                        response.detalleRespuesta = e4.Message.ToString();

                        Logger.log.Error("Error Envio Teleavance: " + e4);
                    }



                }
                #endregion


            }
            catch (Exception ex0)
            {
                Logger.log.Error("Error en servicio UpCall: " + ex0);
            }



            return response;
        }
    }
}
