using Newtonsoft.Json;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Net;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using wsTeleavenceService.Global;
using wsTeleavenceService.Models;

namespace wsTeleavenceService.Controllers
{
    public class CloudController : ApiController
    {

        [HttpPost]
        [Route("ListaNuevaContactos")]
        public ResultadoApi AgregarListaNuevaConContactos([FromBody] entradaNuevoContactList request)
        {
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
                #region Generacion de TokenPureCloud
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(VariablesGlobales.idCliente, VariablesGlobales.secretCliente);
                var token_PC = accessTokenInfo.AccessToken;
                Logger.log.Info("Access token=" + token_PC)
                #endregion

                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
                var apiInstance = new OutboundApi();
                var apiInstance2 = new AuthorizationApi();

                var createCL = new ContactList();
                createCL.Name = request.nombreContactList.ToUpper();
                List<string> ColName = new List<string>();
                string namediv = request.division.ToString();
                string DivisionOrg = "";
                AuthzDivisionEntityListing resultdiv = apiInstance2.GetAuthorizationDivisions(name: namediv);
                foreach (AuthzDivision divs in resultdiv.Entities)
                {
                    DivisionOrg = divs.Id.ToString();
                }

                string[] read;
                char[] seperators = { ',' };
                int contador = 0;
                StreamReader sr = new StreamReader(request.rutaCsv, System.Text.Encoding.Default);
                string data;
                string contactListId = "";
                List<Dictionary<string, object>> InfoLista = new List<Dictionary<string, object>>();
                string[] tipoTelefono = request.tipoTelefono.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                string[] campoTelefono = request.campoTelefono.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                List<string> listContactid = new List<string>();

                List<EnvioDataTeleavance> listDataTeleavance = new List<EnvioDataTeleavance>();

                List<List<string>> ListofListEnvioTA = new List<List<string>>();

                List<string> ListEnvioTA = new List<string>();
                string campoIdentificador = null;
                campoIdentificador = request.campoIdentificador.ToString();
                List<WritableDialerContact> body = new List<WritableDialerContact>();
                int sizePurecloud = 0;

                while ((data = sr.ReadLine()) != null)
                {
                    if (contador == 0)
                    {
                        #region Creacion ContactList PureCloud
                        //read = data.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                        read = data.Split(seperators);
                        sizePurecloud = read.Length;
                        DomainEntityRef DomRef = new DomainEntityRef();

                        for (int i = 0; i < sizePurecloud; i++) ColName.Add(read[i].ToString().ToUpper());
                        List<ContactPhoneNumberColumn> PhoneNum = new List<ContactPhoneNumberColumn>();
                        //  ColName.Add(campoIdentificador.ToString().ToUpper());
                        for (int i = 0; i < tipoTelefono.Length; i++)
                        {
                            ContactPhoneNumberColumn PhoneNums = new ContactPhoneNumberColumn();
                            PhoneNums.ColumnName = campoTelefono[i].ToUpper();
                            PhoneNums.Type = tipoTelefono[i];
                            PhoneNum.Add(PhoneNums);
                            //   ColName.Add(campoTelefono[i].ToString().ToUpper());
                        }

                        createCL.ColumnNames = ColName;
                        createCL.PhoneColumns = PhoneNum;
                        DomRef.Id = DivisionOrg;
                        createCL.Division = DomRef;



                        ContactList resultCL = apiInstance.PostOutboundContactlists(createCL);
                        contactListId = resultCL.Id;
                        #endregion
                    }
                    else
                    {
                        #region Asignacion de Datos PureCloud
                        read = data.Split(seperators);
                        Dictionary<string, object> Data2 = new Dictionary<string, object>();
                        InfoLista.Add(new Dictionary<string, object>());
                        var listEnvioTATemp = new List<string>();

                        for (int j = 0; j < sizePurecloud; j++)
                        {

                            InfoLista[(contador - 1)].Add(ColName[j], read[j].ToString());

                        }

                        foreach (string x in ColName)
                        {
                            Data2.Add(x, InfoLista[(contador - 1)][x]);
                        }



                        for (int k = 0; k < read.Length; k++)
                        {

                            listEnvioTATemp.Add(read[k]);
                        }

                        ListEnvioTA = listEnvioTATemp;

                        var contact = new WritableDialerContact();
                        contact.Id = Data2[campoIdentificador].ToString();
                        contact.ContactListId = contactListId;
                        contact.Data = Data2;

                        listContactid.Add(contact.Id);

                        body.Add(contact);

                        ListofListEnvioTA.Add(ListEnvioTA);

                        #endregion
                    }
                    contador++;
                }
                sr.Dispose();
                sr.Close();

                #region Agregado Datos a ContactList PureCloud
                var priority = true;
                var clearSystemData = false;
                List<DialerContact> resultado = null;
                if (contador <= 1000)
                {
                    resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body, priority, clearSystemData);

                }
                else
                {
                    int j = 1000;
                    int tPages = contador / j;
                    for (int i = 0; i <= contador; i += j)
                    {
                        if (i == tPages * j) j = contador - i;

                        var body2 = body.Skip(i).Take(j).ToList();
                        resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body2, priority, clearSystemData);
                    }
                }
                #endregion

                #region Sentencia creacion y agregado a SQL

                string sqlCreacionLista = "CREATE TABLE " + request.nombreContactList.ToUpper() + " (ID int IDENTITY(1,1), ROW_INSERT DATETIME NULL DEFAULT(getdate()),ID_INTERACCION varchar(200),CONTACTID varchar(200), ";


                string querySQL = "";

                Logger.log.Info("Creación de Tabla SQL: " + sqlCreacionLista);

                for (int i = 1; i <= 200; i++)
                {
                    sqlCreacionLista += "AUX";
                    sqlCreacionLista += i;
                    sqlCreacionLista += " varchar(200), ";
                }
                sqlCreacionLista = sqlCreacionLista.Substring(0, sqlCreacionLista.Length - 2);  //Eliminar la ultima coma sentencia creacionLista
                sqlCreacionLista += ");\n";

                querySQL = sqlCreacionLista;
                Logger.log.Info("Consulta SQL: " + querySQL);

                #endregion

                #region Envio SQL
                SqlCommand command;
                using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                {
                    try
                    {
                        connection.Open();

                        command = new SqlCommand(querySQL, connection);
                        command.ExecuteNonQuery();

                        command.Dispose();
                        connection.Close();
                        connection.Dispose();

                        resultadoApi.estado = "OK";
                        resultadoApi.mensaje = "Lista Creada Correctamente";
                        resultadoApi.contactListID = contactListId;
                    }
                    catch (Exception ex1)
                    {
                        Logger.log.Error("Error inserción SQL table master: " + ex1);
                        resultadoApi.estado = "Error";
                        resultadoApi.mensaje = ex1.Message;
                        resultadoApi.contactListID = "Error";
                    }

                }
                #endregion

                string fechaGestion = DateTime.Now.ToString("yyyyMM") + "01";

                #region Envio Registros a BBDD
                for (int l = 0; l < ListofListEnvioTA.Count(); l++)
                {
                    using (SqlConnection connection = ConnexionBDTeleavance.connexionBDDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query2 = "GenesysCloud_LoadData";
                            var command2 = new SqlCommand(query2, connection);
                            command2.CommandType = CommandType.StoredProcedure;

                            command2.Transaction = transaction;

                            command2.Parameters.AddWithValue("@CONTACTID", listContactid[l].ToString());
                            command2.Parameters.AddWithValue("@LISTACONTACTO", request.nombreContactList.ToUpper());

                            for (int k = 1; k <= sizePurecloud; k++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{k}", ListofListEnvioTA[l][k - 1]);
                            }
                            for (int m = sizePurecloud + 1; m <= 50; m++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{m}", "");
                            }




                            //command2.Parameters.AddWithValue("@vAUX50", fechaGestion);

                            command2.Parameters.Add("@vmsg", SqlDbType.NVarChar, 255);
                            command2.Parameters["@vmsg"].Direction = ParameterDirection.Output;
                            try
                            {
                                command2.ExecuteNonQuery();
                                string sMensaje = command2.Parameters["@vmsg"].Value.ToString();
                                transaction.Commit();
                                Logger.log.Info("Transacción correcta SP " + sMensaje);

                            }
                            catch (Exception ex3)
                            {
                                resultadoApi.estado = "Error";
                                resultadoApi.mensaje = ex3.Message.ToString();
                                Logger.log.Error("Envio DATA Teleavance (transaccion): " + ex3);
                                transaction.Rollback();
                            }


                        }
                        catch (Exception ex2)
                        {
                            resultadoApi.estado = "Error";
                            resultadoApi.mensaje = ex2.Message.ToString();

                            Logger.log.Error("Envio DATA Teleavance: " + ex2);
                        }

                    }

                }
                #endregion


            }
            catch (Exception e)
            {
                Logger.log.Error("Error ListaNuevaContactos: " + e);
                resultadoApi.estado = "Error";
                resultadoApi.mensaje = e.Message;
                resultadoApi.contactListID = "Error";
            }
            return resultadoApi;
        }

        [HttpPost]
        [Route("IngresoContactosListaExistente")]
        public ResultadoApi IngresoContactosListaExistente([FromBody] entradaContactoListExistente request)
        {
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
                /*  AGREGADO A PURECLOUD  */
                #region Generacion de TokenPureCloud
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(VariablesGlobales.idCliente, VariablesGlobales.secretCliente);
                var token_PC = accessTokenInfo.AccessToken;
                Logger.log.Info("Access token=" + token_PC);
                #endregion

                string nombreListaContacto = request.nombreContactList.ToString();

                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
                var apiInstance = new OutboundApi();
                ContactListEntityListing result2 = apiInstance.GetOutboundContactlists(name: nombreListaContacto);
                string contactListId = result2.Entities[0].Id;


                List<string> ColName = new List<string>();

                string[] read;
                char[] seperators = { ',' };
                int contador = 0;
                StreamReader sr = new StreamReader(request.rutaCsv, System.Text.Encoding.Default);
                string data;
                string campoIdentificador = null;
                campoIdentificador = request.campoIdentificador.ToString();
                List<Dictionary<string, object>> InfoLista = new List<Dictionary<string, object>>();
                int sizePurecloud = 0;
                List<string> listContactid = new List<string>();

                List<List<string>> ListofListEnvioTA = new List<List<string>>();

                List<string> ListEnvioTA = new List<string>();

                List<WritableDialerContact> body = new List<WritableDialerContact>();

                while ((data = sr.ReadLine()) != null)
                {
                    if (contador == 0)
                    {
                        #region Creacion ContactList PureCloud
                        read = data.Split(seperators);
                        sizePurecloud = read.Length;  //Limitacion de columnas a 50, para ingreso a PureCloud
                        for (int i = 0; i < sizePurecloud; i++) ColName.Add(read[i].ToString().ToUpper());
                        #endregion
                    }
                    else
                    {
                        #region Datos PureCloud
                        read = data.Split(seperators);
                        Dictionary<string, object> Data2 = new Dictionary<string, object>();
                        InfoLista.Add(new Dictionary<string, object>());
                        var listEnvioTATemp = new List<string>();

                        for (int j = 0; j < sizePurecloud; j++)
                        {

                            InfoLista[(contador - 1)].Add(ColName[j], read[j].ToString());
                        }

                        foreach (string x in result2.Entities[0].ColumnNames)
                        {
                            Data2.Add(x, InfoLista[(contador - 1)][x]);
                        }

                        for (int k = 0; k < read.Length; k++)
                        {

                            listEnvioTATemp.Add(read[k]);
                        }

                        var contact = new WritableDialerContact();
                        ListEnvioTA = listEnvioTATemp;

                        contact.Id = Data2[campoIdentificador].ToString();
                        contact.ContactListId = contactListId;
                        contact.Data = Data2;
                        listContactid.Add(contact.Id);

                        body.Add(contact);
                        ListofListEnvioTA.Add(ListEnvioTA);
                        #endregion
                    }
                    contador++;
                }

                sr.Close();

                #region Agregado Datos a ContactList PureCloud
                var priority = true;
                var clearSystemData = false;
                List<DialerContact> resultado = null;
                if (contador <= 1000)
                {
                    resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body, priority, clearSystemData);
                }
                else
                {
                    int j = 1000;
                    int tPages = contador / j;
                    for (int i = 0; i <= contador; i += j)
                    {
                        if (i == tPages * j) j = contador - i;
                        var body2 = body.Skip(i).Take(j).ToList();
                        resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body2, priority, clearSystemData);
                    }
                }
                #endregion



                #region Envio Registros a Teleavance
                string fechaGestion = DateTime.Now.ToString("yyyyMM") + "01";

                for (int l = 0; l < ListofListEnvioTA.Count(); l++)
                {
                    using (SqlConnection connection = ConnexionBDTeleavance.connexionBDDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query2 = "GenesysCloud_LoadData";
                            var command2 = new SqlCommand(query2, connection);
                            command2.CommandType = CommandType.StoredProcedure;

                            command2.Transaction = transaction;

                            command2.Parameters.AddWithValue("@CONTACTID", listContactid[l].ToString());
                            command2.Parameters.AddWithValue("@LISTACONTACTO", nombreListaContacto.ToUpper());




                            for (int k = 1; k <= sizePurecloud; k++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{k}", ListofListEnvioTA[l][k - 1]);
                            }
                            for (int m = sizePurecloud + 1; m <= 50; m++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{m}", "");
                            }

                            //command2.Parameters.AddWithValue("@vAUX50", fechaGestion);


                            command2.Parameters.Add("@vmsg", SqlDbType.NVarChar, 255);
                            command2.Parameters["@vmsg"].Direction = ParameterDirection.Output;
                            try
                            {
                                command2.ExecuteNonQuery();
                                string sMensaje = command2.Parameters["@vmsg"].Value.ToString();
                                transaction.Commit();
                                Logger.log.Info("Transacción correcta SP " + sMensaje);

                            }
                            catch (Exception ex3)
                            {
                                resultadoApi.estado = "Error";
                                resultadoApi.mensaje = ex3.Message.ToString();
                                Logger.log.Error("Envio DATA Teleavance (transaccion): " + ex3);
                                transaction.Rollback();
                            }


                        }
                        catch (Exception ex2)
                        {
                            resultadoApi.estado = "Error";
                            resultadoApi.mensaje = ex2.Message.ToString();

                            Logger.log.Error("Envio DATA Teleavance: " + ex2);
                        }

                    }

                }
                #endregion

                resultadoApi.estado = "OK";
                resultadoApi.mensaje = "Base Agregado Correctamente";
                resultadoApi.contactListID = contactListId;

            }
            catch (Exception e)
            {
                Logger.log.Error("Error IngresoContactosListaExistente :" + e);

                resultadoApi.estado = "Error";
                resultadoApi.mensaje = e.Message;
                resultadoApi.contactListID = "Error";

                Logger.log.Error(e);
            }

            return resultadoApi;
        }

        [HttpPost]
        [Route("DNCList")]
        public ResultadoDNC AddDNCList([FromBody] entradaDNCList request)
        {

            ResultadoDNC resultadoDNC = new ResultadoDNC();
          
            var apiInstance = new OutboundApi();
            var dncListId = ConfigurationManager.AppSettings["DNCListID"];  // string | DncList ID
            var rutaCsv = ConfigurationManager.AppSettings["ruta"];
            List<string> body = new List<string>();
         //   int sizePurecloud = 0;
            
            try
            {
                #region Generacion de TokenPureCloud
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken("821cfc7b-d06a-4a63-aa22-95d5186cfb8d", "LpKfmYNkKqs3nZCIIxveM8gOv3-JCK83WKoy99gWxc4");
                var token_PC = accessTokenInfo.AccessToken;
                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
                Logger.log.Info("Access token=" + token_PC);
                #endregion

                //ACCESO SFTP
                //List<string> ArchivosDNC = new List<string>();
                //List<string> nombreDNC = new List<string>();
                Dictionary<string, string> ListaDNC = new Dictionary<string, string>();
                using (SftpClient cliente = new SftpClient(new PasswordConnectionInfo("sftp.ibrlatam.com", "sftp_cencosud_02", "SZ2JX9tJog")))
                {
                    cliente.Connect();

                    var paths = cliente.ListDirectory("./SERNAC");

                    foreach (var path in paths)
                    {
                        if (path.Name.Contains("BLACKLIST_"+request.FechaActual))
                        {
                            //ArchivosDNC.Add(path.FullName);
                            //nombreDNC.Add(path.Name);
                            ListaDNC.Add(path.FullName, path.Name);
                        }
                    }
                    //string serverfile = @".\SERNAC";
                    foreach (var archivDNC in ListaDNC)
                    {
                        string localfile = @"C:\Test\"+archivDNC.Value;

                    using (Stream stream = File.OpenWrite(localfile))
                    {
                       
                            cliente.DownloadFile(archivDNC.Key, stream, x => Console.WriteLine(x));
                        
                        
                    }
                       
                    }
                    cliente.Disconnect();
                }
                //FIN SFTP


                string[] read;
                char[] seperators = { '|' };
                int totalNum = 0;
                foreach (var archivDNC in ListaDNC)
                {
                    int contador = 0;
                   
                    StreamReader sr = new StreamReader(rutaCsv+archivDNC.Value, System.Text.Encoding.Default);
                    string data;
                    while ((data = sr.ReadLine()) != null)
                    {
                        if (contador == 0)
                        {
                            contador++;
                            continue;
                        }
                        else
                        {
                            #region Datos PureCloud
                            read = data.Split(seperators);
                            if (!body.Contains("+56" + read[0]))
                            {
                                body.Add("+56" + read[0]);
                                totalNum++;
                            }
                           
                            #endregion
                        }
                        contador++;
                    }

                    sr.Close();
                    
                }


                if (totalNum < 1000)
                {
                    apiInstance.PostOutboundDnclistPhonenumbers(dncListId, body);
                }
                else
                {
                    int j = 1000;
                    int tPages = totalNum / j;
                    for (int i = 0; i <= totalNum; i += j)
                    {
                        if (i == tPages * j) j = totalNum - i;
                        var body2 = body.Skip(i).Take(j).ToList();
                        apiInstance.PostOutboundDnclistPhonenumbers(dncListId, body2);
                    }
                }


                resultadoDNC.estado = "OK";
                resultadoDNC.mensaje = "Agregado Correctamente";
            }

            catch (Exception e)
            {
                Logger.log.Error("Error IngresoContactosListaExistente :" + e);

                resultadoDNC.estado = "Error";
                resultadoDNC.mensaje = e.Message;
          

                Logger.log.Error(e);
            }

            return resultadoDNC;
        }

        
        #region OLD
        /**
        [HttpPost]
        [Route("ListaNuevaContactos_old")]
        public ResultadoApi AgregarListaNuevaConContactos_old([FromBody] entradaNuevoContactList request)
        {
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
                #region Generacion de TokenPureCloud
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(VariablesGlobales.idCliente, VariablesGlobales.secretCliente);
                var token_PC = accessTokenInfo.AccessToken;
                Logger.log.Info("Access token=" + token_PC);
                #endregion

                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
                var apiInstance = new OutboundApi();
                var apiInstance2 = new AuthorizationApi();

                var createCL = new ContactList();
                createCL.Name = request.nombreContactList.ToUpper();
                List<string> ColName = new List<string>();
                string namediv = request.division.ToString();
                string DivisionOrg = "";
                AuthzDivisionEntityListing resultdiv = apiInstance2.GetAuthorizationDivisions(name: namediv);
                foreach (AuthzDivision divs in resultdiv.Entities)
                {
                    DivisionOrg = divs.Id.ToString();
                }

                string[] read;
                char[] seperators = { ',' };
                int contador = 0;
                StreamReader sr = new StreamReader(request.rutaCsv, System.Text.Encoding.Default);
                string data;
                string contactListId = "";
                List<Dictionary<string, object>> InfoLista = new List<Dictionary<string, object>>();
                string[] tipoTelefono = request.tipoTelefono.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                string[] campoTelefono = request.campoTelefono.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                List<string> listContactid = new List<string>();

                List<EnvioDataTeleavance> listDataTeleavance = new List<EnvioDataTeleavance>();

                List<List<string>> ListofListEnvioTA = new List<List<string>>();

                List<string> ListEnvioTA = new List<string>();
                string campoIdentificador = null;
                campoIdentificador = request.campoIdentificador.ToString();
                List<WritableDialerContact> body = new List<WritableDialerContact>();
                int sizePurecloud = 0;

                while ((data = sr.ReadLine()) != null)
                {
                    if (contador == 0)
                    {
                        #region Creacion ContactList PureCloud
                        //read = data.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                        read = data.Split(seperators);
                        sizePurecloud = read.Length;
                        DomainEntityRef DomRef = new DomainEntityRef();

                        for (int i = 0; i < sizePurecloud; i++) ColName.Add(read[i].ToString().ToUpper());
                        List<ContactPhoneNumberColumn> PhoneNum = new List<ContactPhoneNumberColumn>();
                      //  ColName.Add(campoIdentificador.ToString().ToUpper());
                        for (int i = 0; i < tipoTelefono.Length; i++)
                        {
                            ContactPhoneNumberColumn PhoneNums = new ContactPhoneNumberColumn();
                            PhoneNums.ColumnName = campoTelefono[i].ToUpper();
                            PhoneNums.Type = tipoTelefono[i];
                            PhoneNum.Add(PhoneNums);
                         //   ColName.Add(campoTelefono[i].ToString().ToUpper());
                        }

                        createCL.ColumnNames = ColName;
                        createCL.PhoneColumns = PhoneNum;
                        DomRef.Id = DivisionOrg;
                        createCL.Division = DomRef;



                        ContactList resultCL = apiInstance.PostOutboundContactlists(createCL);
                        contactListId = resultCL.Id;
                        #endregion
                    }
                    else
                    {
                        #region Asignacion de Datos PureCloud
                        read = data.Split(seperators);
                        Dictionary<string, object> Data2 = new Dictionary<string, object>();
                        InfoLista.Add(new Dictionary<string, object>());
                        var listEnvioTATemp = new List<string>();

                        for (int j = 0; j < sizePurecloud; j++)
                        {

                            InfoLista[(contador - 1)].Add(ColName[j], read[j].ToString());

                        }

                        foreach (string x in ColName)
                        {
                            Data2.Add(x, InfoLista[(contador - 1)][x]);
                        }



                        for (int k = 0; k < read.Length; k++)
                        {

                            listEnvioTATemp.Add(read[k]);
                        }

                        ListEnvioTA = listEnvioTATemp;

                        var contact = new WritableDialerContact();
                        contact.Id = Data2[campoIdentificador].ToString();
                        contact.ContactListId = contactListId;
                        contact.Data = Data2;

                        listContactid.Add(contact.Id);

                        body.Add(contact);

                        ListofListEnvioTA.Add(ListEnvioTA);

                        #endregion
                    }
                    contador++;
                }
                sr.Dispose();
                sr.Close();

                #region Agregado Datos a ContactList PureCloud
                var priority = true;
                var clearSystemData = true;
                List<DialerContact> resultado = null;
                if (contador <= 1000)
                {
                    resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body, priority, clearSystemData);

                }
                else
                {
                    int j = 1000;
                    int tPages = contador / j;
                    for (int i = 0; i <= contador; i += j)
                    {
                        if (i == tPages * j) j = contador - i;

                        var body2 = body.Skip(i).Take(j).ToList();
                        resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body2, priority, clearSystemData);
                    }
                }
                #endregion

                #region Sentencia creacion y agregado a SQL
                string sqlCreacionLista = "CREATE TABLE " + request.nombreContactList.ToUpper() + " (ID int IDENTITY(1,1), CONTACTID varchar(200), ";

                string sqlIngresoLisa = "";
                string querySQL = "";

                Logger.log.Info("Creación de Tabla SQL: " + sqlCreacionLista);

                for (int i = 1; i <= 200; i++)
                {
                    sqlCreacionLista += "AUX";
                    sqlCreacionLista += i;
                    sqlCreacionLista += " varchar(200), ";
                }
                sqlCreacionLista = sqlCreacionLista.Substring(0, sqlCreacionLista.Length - 2);  //Eliminar la ultima coma sentencia creacionLista
                sqlCreacionLista += ");\n";


                foreach (string x in listContactid)
                {
                    sqlIngresoLisa += "INSERT INTO " + request.nombreContactList.ToUpper() + " (CONTACTID) VALUES (\'" + x + "\');\n";
                }


                querySQL = sqlCreacionLista + sqlIngresoLisa;
                Logger.log.Info("Consulta SQL: " + querySQL);
                #endregion

                #region Envio SQL
                SqlCommand command;
                using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                {
                    try
                    {
                        connection.Open();

                        command = new SqlCommand(querySQL, connection);
                        command.ExecuteNonQuery();

                        command.Dispose();
                        connection.Close();
                        connection.Dispose();

                        resultadoApi.estado = "OK";
                        resultadoApi.mensaje = "Lista Creada Correctamente";
                        resultadoApi.contactListID = contactListId;
                    }
                    catch (Exception ex1)
                    {
                        Logger.log.Error("Error inserción SQL table master: " + ex1);
                        resultadoApi.estado = "Error";
                        resultadoApi.mensaje = ex1.Message;
                        resultadoApi.contactListID = "Error";
                    }

                }
                #endregion

                string fechaGestion = DateTime.Now.ToString("yyyyMM") + "01";

                #region Envio Registros a BBDD
                for (int l = 0; l < ListofListEnvioTA.Count(); l++)
                {
                    using (SqlConnection connection = ConnexionBDTeleavance.connexionBDDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query2 = "SP_I_CARGABASE";
                            var command2 = new SqlCommand(query2, connection);
                            command2.CommandType = CommandType.StoredProcedure;

                            command2.Transaction = transaction;

                            command2.Parameters.AddWithValue("@CONTACTID", listContactid[l].ToString());
                            command2.Parameters.AddWithValue("@LISTACONTACTO", request.nombreContactList.ToUpper());

                            for (int k = 1; k <= sizePurecloud; k++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{k}", ListofListEnvioTA[l][k - 1]);
                            }
                            for (int m = sizePurecloud + 1; m <= 200; m++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{m}", "");
                            }




                            //command2.Parameters.AddWithValue("@vAUX50", fechaGestion);

                            command2.Parameters.Add("@vmsg", SqlDbType.NVarChar, 255);
                            command2.Parameters["@vmsg"].Direction = ParameterDirection.Output;
                            try
                            {
                                command2.ExecuteNonQuery();
                                string sMensaje = command2.Parameters["@vmsg"].Value.ToString();
                                transaction.Commit();
                                Logger.log.Info("Transacción correcta SP " + sMensaje);

                            }
                            catch (Exception ex3)
                            {
                                resultadoApi.estado = "Error";
                                resultadoApi.mensaje = ex3.Message.ToString();
                                Logger.log.Error("Envio DATA Teleavance (transaccion): " + ex3);
                                transaction.Rollback();
                            }


                        }
                        catch (Exception ex2)
                        {
                            resultadoApi.estado = "Error";
                            resultadoApi.mensaje = ex2.Message.ToString();

                            Logger.log.Error("Envio DATA Teleavance: " + ex2);
                        }

                    }

                }
                #endregion


            }
            catch (Exception e)
            {
                Logger.log.Error("Error ListaNuevaContactos: " + e);
                resultadoApi.estado = "Error";
                resultadoApi.mensaje = e.Message;
                resultadoApi.contactListID = "Error";
            }
            return resultadoApi;
        }
        **/
        /**
        [HttpPost]
        [Route("IngresoContactosListaExistente_old")]
        public ResultadoApi IngresoContactosListaExistente_old([FromBody] entradaContactoListExistente request)
        {
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
                
                #region Generacion de TokenPureCloud
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(VariablesGlobales.idCliente, VariablesGlobales.secretCliente);
                var token_PC = accessTokenInfo.AccessToken;
                Logger.log.Info("Access token=" + token_PC);
                #endregion

                string nombreListaContacto = request.nombreContactList.ToString();

                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;
                var apiInstance = new OutboundApi();
                ContactListEntityListing result2 = apiInstance.GetOutboundContactlists(name: nombreListaContacto);
                string contactListId = result2.Entities[0].Id;


                List<string> ColName = new List<string>();
                
                string[] read;
                char[] seperators = { ',' };
                int contador = 0;
                StreamReader sr = new StreamReader(request.rutaCsv, System.Text.Encoding.Default);
                string data;
                string campoIdentificador = null;
                campoIdentificador = request.campoIdentificador.ToString();
                List<Dictionary<string, object>> InfoLista = new List<Dictionary<string, object>>();
                int sizePurecloud = 0;
                List<string> listContactid = new List<string>();

                List<List<string>> ListofListEnvioTA = new List<List<string>>();

                List<string> ListEnvioTA = new List<string>();

                List<WritableDialerContact> body = new List<WritableDialerContact>();

                while ((data = sr.ReadLine()) != null)
                {
                    if (contador == 0)
                    {
                        #region Creacion ContactList PureCloud
                        read = data.Split(seperators);
                        sizePurecloud = read.Length;  //Limitacion de columnas a 50, para ingreso a PureCloud
                        for (int i = 0; i < sizePurecloud; i++) ColName.Add(read[i].ToString().ToUpper());
                        #endregion
                    }
                    else
                    {
                        #region Datos PureCloud
                        read = data.Split(seperators);
                        Dictionary<string, object> Data2 = new Dictionary<string, object>();
                        InfoLista.Add(new Dictionary<string, object>());
                        var listEnvioTATemp = new List<string>();

                        for (int j = 0; j < sizePurecloud; j++)
                        {

                            InfoLista[(contador - 1)].Add(ColName[j], read[j].ToString());
                        }

                        foreach (string x in result2.Entities[0].ColumnNames)
                        {
                            Data2.Add(x, InfoLista[(contador - 1)][x]);
                        }

                        for (int k = 0; k < read.Length; k++)
                        {

                            listEnvioTATemp.Add(read[k]);
                        }

                        var contact = new WritableDialerContact();
                        ListEnvioTA = listEnvioTATemp;

                        contact.Id = Data2[campoIdentificador].ToString();
                        contact.ContactListId = contactListId;
                        contact.Data = Data2;
                        listContactid.Add(contact.Id);

                        body.Add(contact);
                        ListofListEnvioTA.Add(ListEnvioTA);
                        #endregion
                    }
                    contador++;
                }

                sr.Close();

                #region Agregado Datos a ContactList PureCloud
                var priority = true;
                var clearSystemData = true;
                List<DialerContact> resultado = null;
                if (contador <= 1000)
                {
                    resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body, priority, clearSystemData);
                }
                else
                {
                    int j = 1000;
                    int tPages = contador / j;
                    for (int i = 0; i <= contador; i += j)
                    {
                        if (i == tPages * j) j = contador - i;
                        var body2 = body.Skip(i).Take(j).ToList();
                        resultado = apiInstance.PostOutboundContactlistContacts(contactListId, body2, priority, clearSystemData);
                    }
                }
                #endregion

                
                string sqlIngresoLisa = "";


                foreach (string x in listContactid)
                {
                    sqlIngresoLisa += "INSERT INTO " + nombreListaContacto.ToUpper() + " (CONTACTID) VALUES (\'" + x + "\');\n";
                }

                SqlCommand command;

                Logger.log.Info("Consulta total: " + sqlIngresoLisa);

                #region Envio SQL
                using (SqlConnection connection = ConnexionBD.connexionBDTeleavance())
                {
                    connection.Open();

                    command = new SqlCommand(sqlIngresoLisa, connection);
                    command.ExecuteNonQuery();

                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
                #endregion

                #region Envio Registros a Teleavance
                string fechaGestion = DateTime.Now.ToString("yyyyMM") + "01";

                for (int l = 0; l < ListofListEnvioTA.Count(); l++)
                {
                    using (SqlConnection connection = ConnexionBDTeleavance.connexionBDDTeleavance())
                    {
                        try
                        {
                            connection.Open();

                            var transaction = connection.BeginTransaction();

                            var query2 = "SP_I_CARGABASE";
                            var command2 = new SqlCommand(query2, connection);
                            command2.CommandType = CommandType.StoredProcedure;

                            command2.Transaction = transaction;

                            command2.Parameters.AddWithValue("@CONTACTID", listContactid[l].ToString());
                            command2.Parameters.AddWithValue("@LISTACONTACTO", nombreListaContacto.ToUpper());




                            for (int k = 1; k <= sizePurecloud; k++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{k}", ListofListEnvioTA[l][k - 1]);
                            }
                            for (int m = sizePurecloud + 1; m <= 200; m++)
                            {
                                command2.Parameters.AddWithValue($"@vAUX{m}", "");
                            }

                            //command2.Parameters.AddWithValue("@vAUX50", fechaGestion);


                            command2.Parameters.Add("@vmsg", SqlDbType.NVarChar, 255);
                            command2.Parameters["@vmsg"].Direction = ParameterDirection.Output;
                            try
                            {
                                command2.ExecuteNonQuery();
                                string sMensaje = command2.Parameters["@vmsg"].Value.ToString();
                                transaction.Commit();
                                Logger.log.Info("Transacción correcta SP " + sMensaje);

                            }
                            catch (Exception ex3)
                            {
                                resultadoApi.estado = "Error";
                                resultadoApi.mensaje = ex3.Message.ToString();
                                Logger.log.Error("Envio DATA Teleavance (transaccion): " + ex3);
                                transaction.Rollback();
                            }


                        }
                        catch (Exception ex2)
                        {
                            resultadoApi.estado = "Error";
                            resultadoApi.mensaje = ex2.Message.ToString();

                            Logger.log.Error("Envio DATA Teleavance: " + ex2);
                        }

                    }

                }
                #endregion

                resultadoApi.estado = "OK";
                resultadoApi.mensaje = "Base Agregado Correctamente";
                resultadoApi.contactListID = contactListId;

            }
            catch (Exception e)
            {
                Logger.log.Error("Error IngresoContactosListaExistente :" + e);

                resultadoApi.estado = "Error";
                resultadoApi.mensaje = e.Message;
                resultadoApi.contactListID = "Error";

                Logger.log.Error(e);
            }

            return resultadoApi;
        }

      **/
        #endregion
    }
}