using log4net;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;
using PureCloudPlatform.Client.V2.Extensions;
using ScriptWSiBR.Filters;
using ScriptWSiBR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Diagnostics;
using System.Web.Http;
using System.Configuration;
using System.IO;
using System.Threading;

namespace ScriptWSiBR.Controllers
{
    public class ScriptController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger("RollingFileLog");

            

        [BasicAuthentication]
        [HttpGet]
        [Route("ObtenerRegistro")]
        public ResponseObtenerRegistro Get(String Terminal)
        {
            var response = new ResponseObtenerRegistro();
            log.Info("****** OBTENER REGISTRO ********");
          
            try
            {
                

                using (SqlConnection connection = GlobalVariable.connexionBD())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "GC_ObtenerRegistro";
                    var command = new SqlCommand(query, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = transaction
                    };

                    command.Parameters.AddWithValue("@CodTerminal", Terminal ?? "");

                    command.Parameters.Add("@TERMINAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@TERMINAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@COMERCIO", SqlDbType.NVarChar, 255);
                    command.Parameters["@COMERCIO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@JEFE_COMERCIAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@JEFE_COMERCIAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@BACK_OFFICE", SqlDbType.NVarChar, 255);
                    command.Parameters["@BACK_OFFICE"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EJECUTIVO_VENDEDOR", SqlDbType.NVarChar, 255);
                    command.Parameters["@EJECUTIVO_VENDEDOR"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EJECUTIVO_ANTERIOR", SqlDbType.NVarChar, 255);
                    command.Parameters["@EJECUTIVO_ANTERIOR"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EJECUTIVO_ACTUAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@EJECUTIVO_ACTUAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELF_EJECUTIVO", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELF_EJECUTIVO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TIPO", SqlDbType.NVarChar, 255);
                    command.Parameters["@TIPO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@RUC", SqlDbType.NVarChar, 255);
                    command.Parameters["@RUC"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@RAZON_SOCIAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@RAZON_SOCIAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@RUBRO", SqlDbType.NVarChar, 255);
                    command.Parameters["@RUBRO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@DIRECCION", SqlDbType.NVarChar, 255);
                    command.Parameters["@DIRECCION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@REFERENCIA", SqlDbType.NVarChar, 255);
                    command.Parameters["@REFERENCIA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@REGION", SqlDbType.NVarChar, 255);
                    command.Parameters["@REGION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@DEPARTAMENTO", SqlDbType.NVarChar, 255);
                    command.Parameters["@DEPARTAMENTO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@PROVINCIA", SqlDbType.NVarChar, 255);
                    command.Parameters["@PROVINCIA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@DISTRITO", SqlDbType.NVarChar, 255);
                    command.Parameters["@DISTRITO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@UBIGEO", SqlDbType.NVarChar, 255);
                    command.Parameters["@UBIGEO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@LATITUD", SqlDbType.NVarChar, 255);
                    command.Parameters["@LATITUD"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@LONGITUD", SqlDbType.NVarChar, 255);
                    command.Parameters["@LONGITUD"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CADENA", SqlDbType.NVarChar, 255);
                    command.Parameters["@CADENA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TITULAR", SqlDbType.NVarChar, 255);
                    command.Parameters["@TITULAR"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@DNI", SqlDbType.NVarChar, 255);
                    command.Parameters["@DNI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FIJO", SqlDbType.NVarChar, 255);
                    command.Parameters["@FIJO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CELULAR_1", SqlDbType.NVarChar, 255);
                    command.Parameters["@CELULAR_1"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CELULAR_2", SqlDbType.NVarChar, 255);
                    command.Parameters["@CELULAR_2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CORREO", SqlDbType.NVarChar, 255);
                    command.Parameters["@CORREO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@STATUS_MIGRACION", SqlDbType.NVarChar, 255);
                    command.Parameters["@STATUS_MIGRACION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@USUARIO", SqlDbType.NVarChar, 255);
                    command.Parameters["@USUARIO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CELULAR", SqlDbType.NVarChar, 255);
                    command.Parameters["@CELULAR"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OPERADOR", SqlDbType.NVarChar, 255);
                    command.Parameters["@OPERADOR"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@RESP_OPER_KINPOS", SqlDbType.NVarChar, 255);
                    command.Parameters["@RESP_OPER_KINPOS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CELULAR_RESP", SqlDbType.NVarChar, 255);
                    command.Parameters["@CELULAR_RESP"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CONTRATO_BIM", SqlDbType.NVarChar, 255);
                    command.Parameters["@CONTRATO_BIM"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@BIMER", SqlDbType.NVarChar, 255);
                    command.Parameters["@BIMER"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@INTERNET", SqlDbType.NVarChar, 255);
                    command.Parameters["@INTERNET"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OPERADOR_INTERNET", SqlDbType.NVarChar, 255);
                    command.Parameters["@OPERADOR_INTERNET"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CONTRATO_INTERNET_BBVA", SqlDbType.NVarChar, 255);
                    command.Parameters["@CONTRATO_INTERNET_BBVA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar, 255);
                    command.Parameters["@STATUS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FECHA_INSTALACION", SqlDbType.NVarChar, 255);
                    command.Parameters["@FECHA_INSTALACION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@GRUPO_INSTALACION", SqlDbType.NVarChar, 255);
                    command.Parameters["@GRUPO_INSTALACION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EQUIPO_INSTALADO", SqlDbType.NVarChar, 255);
                    command.Parameters["@EQUIPO_INSTALADO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ESTADO_FILE", SqlDbType.NVarChar, 255);
                    command.Parameters["@ESTADO_FILE"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@UBICACION_FILE", SqlDbType.NVarChar, 255);
                    command.Parameters["@UBICACION_FILE"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TARIFA_INICIAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@TARIFA_INICIAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@LETRERO_EXTERNO", SqlDbType.NVarChar, 255);
                    command.Parameters["@LETRERO_EXTERNO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FECHA_BAJA", SqlDbType.NVarChar, 255);
                    command.Parameters["@FECHA_BAJA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@STATUS_RECUPERO_EQP", SqlDbType.NVarChar, 255);
                    command.Parameters["@STATUS_RECUPERO_EQP"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@MOTIVO_BAJA", SqlDbType.NVarChar, 255);
                    command.Parameters["@MOTIVO_BAJA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@MES_PASADO_3", SqlDbType.NVarChar, 255);
                    command.Parameters["@MES_PASADO_3"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@MES_PASADO_2", SqlDbType.NVarChar, 255);
                    command.Parameters["@MES_PASADO_2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@MES_PASADO_1", SqlDbType.NVarChar, 255);
                    command.Parameters["@MES_PASADO_1"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@MES_ACTUAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@MES_ACTUAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@SEGMENTACION", SqlDbType.NVarChar, 255);
                    command.Parameters["@SEGMENTACION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ESTADO_3270", SqlDbType.NVarChar, 255);
                    command.Parameters["@ESTADO_3270"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CUENTA_AGENTE", SqlDbType.NVarChar, 255);
                    command.Parameters["@CUENTA_AGENTE"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CUENTA_COMISIONES", SqlDbType.NVarChar, 255);
                    command.Parameters["@CUENTA_COMISIONES"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CODIGO_ACREEDORES", SqlDbType.NVarChar, 255);
                    command.Parameters["@CODIGO_ACREEDORES"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CODIGO_OFICINA", SqlDbType.NVarChar, 255);
                    command.Parameters["@CODIGO_OFICINA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OFICINA_ASIGNADA", SqlDbType.NVarChar, 255);
                    command.Parameters["@OFICINA_ASIGNADA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@DISTRITO2", SqlDbType.NVarChar, 255);
                    command.Parameters["@DISTRITO2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TERRITORIO", SqlDbType.NVarChar, 255);
                    command.Parameters["@TERRITORIO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CAPA", SqlDbType.NVarChar, 255);
                    command.Parameters["@CAPA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EMP", SqlDbType.NVarChar, 255);
                    command.Parameters["@EMP"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EST", SqlDbType.NVarChar, 255);
                    command.Parameters["@EST"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@SEMILLA", SqlDbType.NVarChar, 255);
                    command.Parameters["@SEMILLA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@PRD_MIGRADOS", SqlDbType.NVarChar, 255);
                    command.Parameters["@PRD_MIGRADOS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CAMPANAS", SqlDbType.NVarChar, 255);
                    command.Parameters["@CAMPANAS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CONTOMETRO", SqlDbType.NVarChar, 255);
                    command.Parameters["@CONTOMETRO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ZONA", SqlDbType.NVarChar, 255);
                    command.Parameters["@ZONA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@SUPERVISOR_BBVA", SqlDbType.NVarChar, 255);
                    command.Parameters["@SUPERVISOR_BBVA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EST_REPARTO_CONT", SqlDbType.NVarChar, 255);
                    command.Parameters["@EST_REPARTO_CONT"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FECHA_REPARTO_CONT", SqlDbType.NVarChar, 255);
                    command.Parameters["@FECHA_REPARTO_CONT"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@MIGRA_TARIFA", SqlDbType.NVarChar, 255);
                    command.Parameters["@MIGRA_TARIFA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FECHA_MIGRA_TARIFA", SqlDbType.NVarChar, 255);
                    command.Parameters["@FECHA_MIGRA_TARIFA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TARIFA_ACTUAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@TARIFA_ACTUAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FECHA_ALTA_SIST", SqlDbType.NVarChar, 255);
                    command.Parameters["@FECHA_ALTA_SIST"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EJECUTIVO_TEMPORAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@EJECUTIVO_TEMPORAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELEF_TEMPORAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELEF_TEMPORAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@REGIONES_SIN_IGV", SqlDbType.NVarChar, 255);
                    command.Parameters["@REGIONES_SIN_IGV"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@DIVISION_DIST", SqlDbType.NVarChar, 255);
                    command.Parameters["@DIVISION_DIST"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@RUBRO_ESPECIF", SqlDbType.NVarChar, 255);
                    command.Parameters["@RUBRO_ESPECIF"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FECHA_NAC_TITULAR", SqlDbType.NVarChar, 255);
                    command.Parameters["@FECHA_NAC_TITULAR"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TIPO_AFILIACION", SqlDbType.NVarChar, 255);
                    command.Parameters["@TIPO_AFILIACION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@FECHA_FIRMA_CONTRAT", SqlDbType.NVarChar, 255);
                    command.Parameters["@FECHA_FIRMA_CONTRAT"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar, 255);
                    command.Parameters["@SECTOR"].Direction = ParameterDirection.Output;


                    try
                    {
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        command.Dispose();

                        response.CodigoRespuesta = true;
                        response.DetalleRespuesta = "OK";
                        response.TERMINAL = command.Parameters["@TERMINAL"].Value.ToString();
                        response.COMERCIO = command.Parameters["@COMERCIO"].Value.ToString();
                        response.JEFE_COMERCIAL = command.Parameters["@JEFE_COMERCIAL"].Value.ToString();
                        response.BACK_OFFICE = command.Parameters["@BACK_OFFICE"].Value.ToString();
                        response.EJECUTIVO_VENDEDOR = command.Parameters["@EJECUTIVO_VENDEDOR"].Value.ToString();
                        response.EJECUTIVO_ANTERIOR = command.Parameters["@EJECUTIVO_ANTERIOR"].Value.ToString();
                        response.EJECUTIVO_ACTUAL = command.Parameters["@EJECUTIVO_ACTUAL"].Value.ToString();
                        response.TELF_EJECUTIVO = command.Parameters["@TELF_EJECUTIVO"].Value.ToString();
                        response.TIPO = command.Parameters["@TIPO"].Value.ToString();
                        response.RUC = command.Parameters["@RUC"].Value.ToString();
                        response.RAZON_SOCIAL = command.Parameters["@RAZON_SOCIAL"].Value.ToString();
                        response.RUBRO = command.Parameters["@RUBRO"].Value.ToString();
                        response.DIRECCION = command.Parameters["@DIRECCION"].Value.ToString();
                        response.REFERENCIA = command.Parameters["@REFERENCIA"].Value.ToString();
                        response.REGION = command.Parameters["@REGION"].Value.ToString();
                        response.DEPARTAMENTO = command.Parameters["@DEPARTAMENTO"].Value.ToString();
                        response.PROVINCIA = command.Parameters["@PROVINCIA"].Value.ToString();
                        response.DISTRITO = command.Parameters["@DISTRITO"].Value.ToString();
                        response.LATITUD = command.Parameters["@LATITUD"].Value.ToString();
                        response.LONGITUD = command.Parameters["@LONGITUD"].Value.ToString();
                        response.CADENA = command.Parameters["@CADENA"].Value.ToString();
                        response.TITULAR = command.Parameters["@TITULAR"].Value.ToString();
                        response.DNI = command.Parameters["@DNI"].Value.ToString();
                        response.FIJO = command.Parameters["@FIJO"].Value.ToString();
                        response.CELULAR_1 = command.Parameters["@CELULAR_1"].Value.ToString();
                        response.CELULAR_2 = command.Parameters["@CELULAR_2"].Value.ToString();
                        response.CORREO = command.Parameters["@CORREO"].Value.ToString();
                        response.STATUS_MIGRACION = command.Parameters["@STATUS_MIGRACION"].Value.ToString();
                        response.USUARIO = command.Parameters["@USUARIO"].Value.ToString();
                        response.CELULAR = command.Parameters["@CELULAR"].Value.ToString();
                        response.OPERADOR = command.Parameters["@OPERADOR"].Value.ToString();
                        response.RESP_OPER_KINPOS = command.Parameters["@RESP_OPER_KINPOS"].Value.ToString();
                        response.CELULAR_RESP = command.Parameters["@CELULAR_RESP"].Value.ToString();
                        response.CONTRATO_BIM = command.Parameters["@CONTRATO_BIM"].Value.ToString();
                        response.BIMER = command.Parameters["@BIMER"].Value.ToString();
                        response.INTERNET = command.Parameters["@INTERNET"].Value.ToString();
                        response.OPERADOR_INTERNET = command.Parameters["@OPERADOR_INTERNET"].Value.ToString();
                        response.CONTRATO_INTERNET_BBVA = command.Parameters["@CONTRATO_INTERNET_BBVA"].Value.ToString();
                        response.STATUS = command.Parameters["@STATUS"].Value.ToString();
                        response.FECHA_INSTALACION = command.Parameters["@FECHA_INSTALACION"].Value.ToString();
                        response.GRUPO_INSTALACION = command.Parameters["@GRUPO_INSTALACION"].Value.ToString();
                        response.EQUIPO_INSTALADO = command.Parameters["@EQUIPO_INSTALADO"].Value.ToString();
                        response.ESTADO_FILE = command.Parameters["@ESTADO_FILE"].Value.ToString();
                        response.UBICACION_FILE = command.Parameters["@UBICACION_FILE"].Value.ToString();
                        response.TARIFA_INICIAL = command.Parameters["@TARIFA_INICIAL"].Value.ToString();
                        response.LETRERO_EXTERNO = command.Parameters["@LETRERO_EXTERNO"].Value.ToString();
                        response.FECHA_BAJA = command.Parameters["@FECHA_BAJA"].Value.ToString();
                        response.STATUS_RECUPERO_EQP = command.Parameters["@STATUS_RECUPERO_EQP"].Value.ToString();
                        response.MOTIVO_BAJA = command.Parameters["@MOTIVO_BAJA"].Value.ToString();
                        response.MES_PASADO_3 = command.Parameters["@MES_PASADO_3"].Value.ToString();
                        response.MES_PASADO_2 = command.Parameters["@MES_PASADO_2"].Value.ToString();
                        response.MES_PASADO_1 = command.Parameters["@MES_PASADO_1"].Value.ToString();
                        response.MES_ACTUAL = command.Parameters["@MES_ACTUAL"].Value.ToString();
                        response.SEGMENTACION = command.Parameters["@SEGMENTACION"].Value.ToString();
                        response.ESTADO_3270 = command.Parameters["@ESTADO_3270"].Value.ToString();
                        response.CUENTA_AGENTE = command.Parameters["@CUENTA_AGENTE"].Value.ToString();
                        response.CUENTA_COMISIONES = command.Parameters["@CUENTA_COMISIONES"].Value.ToString();
                        response.CODIGO_ACREEDORES = command.Parameters["@CODIGO_ACREEDORES"].Value.ToString();
                        response.CODIGO_OFICINA = command.Parameters["@CODIGO_OFICINA"].Value.ToString();
                        response.OFICINA_ASIGNADA = command.Parameters["@OFICINA_ASIGNADA"].Value.ToString();
                        response.DISTRITO2 = command.Parameters["@DISTRITO2"].Value.ToString();
                        response.TERRITORIO = command.Parameters["@TERRITORIO"].Value.ToString();
                        response.CAPA = command.Parameters["@CAPA"].Value.ToString();
                        response.EMP = command.Parameters["@EMP"].Value.ToString();
                        response.EST = command.Parameters["@EST"].Value.ToString();
                        response.SEMILLA = command.Parameters["@SEMILLA"].Value.ToString();
                        response.PRD_MIGRADOS = command.Parameters["@PRD_MIGRADOS"].Value.ToString();
                        response.CAMPANAS = command.Parameters["@CAMPANAS"].Value.ToString();
                        response.CONTOMETRO = command.Parameters["@CONTOMETRO"].Value.ToString();
                        response.ZONA = command.Parameters["@ZONA"].Value.ToString();
                        response.SUPERVISOR_BBVA = command.Parameters["@SUPERVISOR_BBVA"].Value.ToString();
                        response.EST_REPARTO_CONT = command.Parameters["@EST_REPARTO_CONT"].Value.ToString();
                        response.FECHA_REPARTO_CONT = command.Parameters["@FECHA_REPARTO_CONT"].Value.ToString();
                        response.MIGRA_TARIFA = command.Parameters["@MIGRA_TARIFA"].Value.ToString();
                        response.FECHA_MIGRA_TARIFA = command.Parameters["@FECHA_MIGRA_TARIFA"].Value.ToString();
                        response.TARIFA_ACTUAL = command.Parameters["@TARIFA_ACTUAL"].Value.ToString();
                        response.FECHA_ALTA_SIST = command.Parameters["@FECHA_ALTA_SIST"].Value.ToString();
                        response.EJECUTIVO_TEMPORAL = command.Parameters["@EJECUTIVO_TEMPORAL"].Value.ToString();
                        response.TELEF_TEMPORAL = command.Parameters["@TELEF_TEMPORAL"].Value.ToString();
                        response.REGIONES_SIN_IGV = command.Parameters["@REGIONES_SIN_IGV"].Value.ToString();
                        response.DIVISION_DIST = command.Parameters["@DIVISION_DIST"].Value.ToString();
                        response.RUBRO_ESPECIF = command.Parameters["@RUBRO_ESPECIF"].Value.ToString();
                        response.FECHA_NAC_TITULAR = command.Parameters["@FECHA_NAC_TITULAR"].Value.ToString();
                        response.TIPO_AFILIACION = command.Parameters["@TIPO_AFILIACION"].Value.ToString();
                        response.FECHA_FIRMA_CONTRAT = command.Parameters["@FECHA_FIRMA_CONTRAT"].Value.ToString();
                        response.SECTOR = command.Parameters["@SECTOR"].Value.ToString();




                        //log.Info("Registro insertado con contactID: " + request.contactId ?? "");

                    }
                    catch (Exception ex2)
                    {
                        log.Error("Error SP: " + ex2.Message);
                        transaction.Rollback();
                        response.CodigoRespuesta = false;
                        response.DetalleRespuesta = "Error SP: " + ex2.Message;

                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = false;
                response.DetalleRespuesta = "Error General: " + ex.Message;
                log.Error("Error: " + ex);
            }


            return response;
        }

        [BasicAuthentication]
        [HttpGet]
        [Route("ObtenerComision")]
        public ResponseComision GetComision(String Terminal, String Mes)
        {
            var response = new ResponseComision();
           
            try
            {


                using (SqlConnection connection = GlobalVariable.connexionBD())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "GC_AE_Comisiones";
                    var command = new SqlCommand(query, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = transaction
                    };

                    command.Parameters.AddWithValue("@COD_TERMINAL", Terminal ?? "");
                    command.Parameters.AddWithValue("@MES", Mes ?? "");

                    command.Parameters.Add("@COMISION", SqlDbType.NVarChar, 255);
                    command.Parameters["@COMISION"].Direction = ParameterDirection.Output;


                    try
                    {
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        command.Dispose();

                        response.CodigoRespuesta = true;
                        response.DetalleRespuesta = "OK";
                        response.comision = command.Parameters["@comision"].Value.ToString();
                        
                        //log.Info("Registro insertado con contactID: " + request.contactId ?? "");

                    }
                    catch (Exception ex2)
                    {
                        log.Error("Error SP: " + ex2.Message);
                        transaction.Rollback();
                        response.CodigoRespuesta = false;
                        response.DetalleRespuesta = "Error SP: " + ex2.Message;

                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = false;
                response.DetalleRespuesta = "Error General: " + ex.Message;
                log.Error("Error: " + ex);
            }


            return response;
        }

        [BasicAuthentication]
        [HttpPut]
        [Route("CrearTicket")]
        public ResponseTicket Put(RequestTicket ticket)
        {
            var response = new ResponseTicket();

            try
            {


                using (SqlConnection connection = GlobalVariable.connexionBD())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "GC_CrearTicket";
                    var command = new SqlCommand(query, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = transaction
                    };
                    command.Parameters.AddWithValue("@conversationId", ticket.conversationId ?? "");
                    command.Parameters.AddWithValue("@codAgente", ticket.codAgente ?? "");
                    command.Parameters.AddWithValue("@nomAgente", ticket.nomAgente ?? "");
                    command.Parameters.AddWithValue("@rucAgente", ticket.rucAgente);
                    command.Parameters.AddWithValue("@telAgente", ticket.telAgente);
                    command.Parameters.AddWithValue("@fecContacto", ticket.fecContacto);
                    command.Parameters.AddWithValue("@direccion ", ticket.Direccion);
                    command.Parameters.AddWithValue("@producto", ticket.Producto); 
                    command.Parameters.AddWithValue("@plazo", ticket.plazo ?? "");
                    command.Parameters.AddWithValue("@userAtencion", ticket.userAtencion ?? "");
                    command.Parameters.AddWithValue("@Observacion", ticket.Observacion ?? "");



                    command.Parameters.Add("@ticket", SqlDbType.NVarChar, 255);
                    command.Parameters["@ticket"].Direction = ParameterDirection.Output;


                    try
                    {
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        command.Dispose();

                        response.CodigoRespuesta = true;
                        response.DetalleRespuesta = "OK";
                        response.ticket = command.Parameters["@ticket"].Value.ToString();
                       



                        //log.Info("Registro insertado con contactID: " + request.contactId ?? "");

                    }
                    catch (Exception ex2)
                    {
                        log.Error("Error SP: " + ex2.Message);
                        transaction.Rollback();
                        response.CodigoRespuesta = false;
                        response.DetalleRespuesta = "Error SP: " + ex2.Message;

                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = false;
                response.DetalleRespuesta = "Error General: " + ex.Message;
                log.Error("Error: " + ex);
            }

            return response;

        }


        [BasicAuthentication]
        [HttpPut]
        [Route("CiabeiEpa")]
        public ResponseCiabeiEPA PutEpa(RequestCiabeiEPA EPA)
        {
            var response = new ResponseCiabeiEPA();

            try
            {


                using (SqlConnection connection = GlobalVariable.connexionBD())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "GC_CiabeiEpa";
                    var command = new SqlCommand(query, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = transaction
                    };

                    command.Parameters.AddWithValue("@ConversationID", EPA.ConversationID ?? "");
                    command.Parameters.AddWithValue("@Origen", EPA.Origen ?? "");
                    command.Parameters.AddWithValue("@Telefono", EPA.Telefono ?? "");
                    command.Parameters.AddWithValue("@Agente", EPA.Agente ?? "");
                    command.Parameters.AddWithValue("@Segmento", EPA.SEGMENTO);
                    command.Parameters.AddWithValue("@PCategoria", EPA.PREGUNTA_CATEGORIA);
                    command.Parameters.AddWithValue("@PDescripcion", EPA.PREGUNTA_DESCRIPCION);
                    command.Parameters.AddWithValue("@PValor", EPA.PREGUNTA_VALOR ?? "");
                    command.Parameters.AddWithValue("@PValor_Descripcion", EPA.PREGUNTA_VALOR_DESCRIPCION ?? "");
                    

                    try
                    {
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        command.Dispose();

                        response.CodigoRespuesta = true;
                        response.DetalleRespuesta = "OK";
                      




                        //log.Info("Registro insertado con contactID: " + request.contactId ?? "");

                    }
                    catch (Exception ex2)
                    {
                        log.Error("Error SP: " + ex2.Message);
                        transaction.Rollback();
                        response.CodigoRespuesta = false;
                        response.DetalleRespuesta = "Error SP: " + ex2.Message;

                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = false;
                response.DetalleRespuesta = "Error General: " + ex.Message;
                log.Error("Error: " + ex);
            }

            return response;

        }

        [BasicAuthentication]
        [HttpPut]
        [Route("CiabeiCrearCliente")]
        public ResponseCrearCliente Crear(RequestCrearCliente cliente)
        {
            var response = new ResponseCrearCliente();

            try
            {


                using (SqlConnection connection = GlobalVariable.connexionBD())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "GC_CiabeiCrearCliente";
                    var command = new SqlCommand(query, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = transaction
                    };

                    command.Parameters.AddWithValue("@CENTRAL", cliente.CENTRAL ?? "");
                    command.Parameters.AddWithValue("@TERRI", cliente.TERRI ?? "");
                    command.Parameters.AddWithValue("@OFICINA", cliente.OFICINA ?? "");
                    command.Parameters.AddWithValue("@OFICINA_N", cliente.OFICINA_N ?? "");
                    command.Parameters.AddWithValue("@GESTOR", cliente.GESTOR ?? "");
                    command.Parameters.AddWithValue("@NDOC_RUC", cliente.NDOC_RUC ?? "");
                    command.Parameters.AddWithValue("@NOMBRE", cliente.NOMBRE ?? "");
                    command.Parameters.AddWithValue("@NETCASH", cliente.NETCASH ?? "");
                    command.Parameters.AddWithValue("@VERSION", cliente.VERSION ?? "");
                    command.Parameters.AddWithValue("@NOMBRE1", cliente.NOMBRE1 ?? "");
                    command.Parameters.AddWithValue("@CARGO1", cliente.CARGO1 ?? "");
                    command.Parameters.AddWithValue("@CORREO1", cliente.CORREO1 ?? "");
                    command.Parameters.AddWithValue("@TELEFONO1", cliente.TELEFONO1 ?? "");
                    command.Parameters.AddWithValue("@CELULAR1", cliente.CELULAR1 ?? "");
                    command.Parameters.AddWithValue("@NOMBRE2", cliente.NOMBRE2 ?? "");
                    command.Parameters.AddWithValue("@CARGO2", cliente.CARGO2 ?? "");
                    command.Parameters.AddWithValue("@CORREO2", cliente.CORREO2 ?? "");
                    command.Parameters.AddWithValue("@TELEFONO2", cliente.TELEFONO2 ?? "");
                    command.Parameters.AddWithValue("@CELULAR2", cliente.CELULAR2 ?? "");
                    command.Parameters.AddWithValue("@NOMBRE3", cliente.NOMBRE3 ?? "");
                    command.Parameters.AddWithValue("@CARGO3", cliente.CARGO2 ?? "");
                    command.Parameters.AddWithValue("@CORREO3", cliente.CORREO3 ?? "");
                    command.Parameters.AddWithValue("@TELEFONO3", cliente.TELEFONO3 ?? "");
                    command.Parameters.AddWithValue("@CELULAR3", cliente.CELULAR3 ?? "");
               

                    try
                    {
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        command.Dispose();

                        response.CodigoRespuesta = true;
                        response.DetalleRespuesta = "OK";
                     




                        //log.Info("Registro insertado con contactID: " + request.contactId ?? "");

                    }
                    catch (Exception ex2)
                    {
                        log.Error("Error SP: " + ex2.Message);
                        transaction.Rollback();
                        response.CodigoRespuesta = false;
                        response.DetalleRespuesta = "Error SP: " + ex2.Message;

                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = false;
                response.DetalleRespuesta = "Error General: " + ex.Message;
                log.Error("Error: " + ex);
            }

            return response;

        }

        [BasicAuthentication]
        [HttpGet]
        [Route("CiabeiBuscarRuc")]
        public ResponseCiabeiRUC BuscarRuc(String Ruc)
        {
            var response = new ResponseCiabeiRUC();
            log.Info("****** OBTENER REGISTRO ********");

            try
            {


                using (SqlConnection connection = GlobalVariable.connexionBD())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "GC_CiabeiBuscarRuc";
                    var command = new SqlCommand(query, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = transaction
                    };

                    command.Parameters.AddWithValue("@RUC", Ruc ?? "");

                    command.Parameters.Add("@CENTRAL", SqlDbType.NVarChar, 255);
                    command.Parameters["@CENTRAL"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TERRI", SqlDbType.NVarChar, 255);
                    command.Parameters["@TERRI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OFICINA", SqlDbType.NVarChar, 255);
                    command.Parameters["@OFICINA"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@OFICINA_N", SqlDbType.NVarChar, 255);
                    command.Parameters["@OFICINA_N"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@GESTOR", SqlDbType.NVarChar, 255);
                    command.Parameters["@GESTOR"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NDOC_RUC", SqlDbType.NVarChar, 255);
                    command.Parameters["@NDOC_RUC"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NOMBRE", SqlDbType.NVarChar, 255);
                    command.Parameters["@NOMBRE"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NETCASH", SqlDbType.NVarChar, 255);
                    command.Parameters["@NETCASH"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@VERSION", SqlDbType.NVarChar, 255);
                    command.Parameters["@VERSION"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NOMBRE1", SqlDbType.NVarChar, 255);
                    command.Parameters["@NOMBRE1"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CARGO1", SqlDbType.NVarChar, 255);
                    command.Parameters["@CARGO1"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CORREO1", SqlDbType.NVarChar, 255);
                    command.Parameters["@CORREO1"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELEFONO1", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELEFONO1"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CELULAR1", SqlDbType.NVarChar, 255);
                    command.Parameters["@CELULAR1"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NOMBRE2", SqlDbType.NVarChar, 255);
                    command.Parameters["@NOMBRE2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CARGO2", SqlDbType.NVarChar, 255);
                    command.Parameters["@CARGO2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CORREO2", SqlDbType.NVarChar, 255);
                    command.Parameters["@CORREO2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELEFONO2", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELEFONO2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CELULAR2", SqlDbType.NVarChar, 255);
                    command.Parameters["@CELULAR2"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NOMBRE3", SqlDbType.NVarChar, 255);
                    command.Parameters["@NOMBRE3"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CARGO3", SqlDbType.NVarChar, 255);
                    command.Parameters["@CARGO3"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CORREO3", SqlDbType.NVarChar, 255);
                    command.Parameters["@CORREO3"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELEFONO3", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELEFONO3"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CELULAR3", SqlDbType.NVarChar, 255);
                    command.Parameters["@CELULAR3"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CARGO_EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@CARGO_EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@REGISTRO_EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@REGISTRO_EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NOMBRE_EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@NOMBRE_EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CORREO_EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@CORREO_EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELEFONO_EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELEFONO_EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@RPM_EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@RPM_EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CEL_EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@CEL_EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@REGISTRO_GI", SqlDbType.NVarChar, 255);
                    command.Parameters["@REGISTRO_GI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NOMBRE_GI", SqlDbType.NVarChar, 255);
                    command.Parameters["@NOMBRE_GI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CORREO_GI", SqlDbType.NVarChar, 255);
                    command.Parameters["@CORREO_GI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELEF_GI", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELEF_GI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@RPM_GI", SqlDbType.NVarChar, 255);
                    command.Parameters["@RPM_GI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CEL_GI", SqlDbType.NVarChar, 255);
                    command.Parameters["@CEL_GI"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@REGISTRO_AO", SqlDbType.NVarChar, 255);
                    command.Parameters["@REGISTRO_AO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@NOMBRE_AO", SqlDbType.NVarChar, 255);
                    command.Parameters["@NOMBRE_AO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@CORREO_AO", SqlDbType.NVarChar, 255);
                    command.Parameters["@CORREO_AO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@TELEF_AO", SqlDbType.NVarChar, 255);
                    command.Parameters["@TELEF_AO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@EGS", SqlDbType.NVarChar, 255);
                    command.Parameters["@EGS"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@GESTOR_INTERNO", SqlDbType.NVarChar, 255);
                    command.Parameters["@GESTOR_INTERNO"].Direction = ParameterDirection.Output;
                    command.Parameters.Add("@ASISTENTE", SqlDbType.NVarChar, 255);
                    command.Parameters["@ASISTENTE"].Direction = ParameterDirection.Output;



                    try
                    {
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        command.Dispose();

                        response.CodigoRespuesta = true;
                        response.DetalleRespuesta = "OK";
                        response.CENTRAL = command.Parameters["@CENTRAL"].Value.ToString();
                        response.TERRI = command.Parameters["@TERRI"].Value.ToString();
                        response.OFICINA = command.Parameters["@OFICINA"].Value.ToString();
                        response.OFICINA_N = command.Parameters["@OFICINA_N"].Value.ToString();
                        response.GESTOR = command.Parameters["@GESTOR"].Value.ToString();
                        response.NDOC_RUC = command.Parameters["@NDOC_RUC"].Value.ToString();
                        response.NOMBRE = command.Parameters["@NOMBRE"].Value.ToString();
                        response.NETCASH = command.Parameters["@NETCASH"].Value.ToString();
                        response.VERSION = command.Parameters["@VERSION"].Value.ToString();
                        response.NOMBRE1 = command.Parameters["@NOMBRE1"].Value.ToString();
                        response.CARGO1 = command.Parameters["@CARGO1"].Value.ToString();
                        response.CORREO1 = command.Parameters["@CORREO1"].Value.ToString();
                        response.TELEFONO1 = command.Parameters["@TELEFONO1"].Value.ToString();
                        response.CELULAR1 = command.Parameters["@CELULAR1"].Value.ToString();
                        response.NOMBRE2 = command.Parameters["@NOMBRE2"].Value.ToString();
                        response.CARGO2 = command.Parameters["@CARGO2"].Value.ToString();
                        response.CORREO2 = command.Parameters["@CORREO2"].Value.ToString();
                        response.TELEFONO2 = command.Parameters["@TELEFONO2"].Value.ToString();
                        response.CELULAR2 = command.Parameters["@CELULAR2"].Value.ToString();
                        response.NOMBRE3 = command.Parameters["@NOMBRE3"].Value.ToString();
                        response.CARGO3 = command.Parameters["@CARGO3"].Value.ToString();
                        response.CORREO3 = command.Parameters["@CORREO3"].Value.ToString();
                        response.TELEFONO3 = command.Parameters["@TELEFONO3"].Value.ToString();
                        response.CELULAR3 = command.Parameters["@CELULAR3"].Value.ToString();
                        response.CARGO_EGS = command.Parameters["@CARGO_EGS"].Value.ToString();
                        response.REGISTRO_EGS = command.Parameters["@REGISTRO_EGS"].Value.ToString();
                        response.NOMBRE_EGS = command.Parameters["@NOMBRE_EGS"].Value.ToString();
                        response.CORREO_EGS = command.Parameters["@CORREO_EGS"].Value.ToString();
                        response.TELEFONO_EGS = command.Parameters["@TELEFONO_EGS"].Value.ToString();
                        response.RPM_EGS = command.Parameters["@RPM_EGS"].Value.ToString();
                        response.CEL_EGS = command.Parameters["@CEL_EGS"].Value.ToString();
                        response.REGISTRO_GI = command.Parameters["@REGISTRO_GI"].Value.ToString();
                        response.NOMBRE_GI = command.Parameters["@NOMBRE_GI"].Value.ToString();
                        response.CORREO_GI = command.Parameters["@CORREO_GI"].Value.ToString();
                        response.TELEF_GI = command.Parameters["@TELEF_GI"].Value.ToString();
                        response.RPM_GI = command.Parameters["@RPM_GI"].Value.ToString();
                        response.CEL_GI = command.Parameters["@CEL_GI"].Value.ToString();
                        response.REGISTRO_AO = command.Parameters["@REGISTRO_AO"].Value.ToString();
                        response.NOMBRE_AO = command.Parameters["@NOMBRE_AO"].Value.ToString();
                        response.CORREO_AO = command.Parameters["@CORREO_AO"].Value.ToString();
                        response.TELEF_AO = command.Parameters["@TELEF_AO"].Value.ToString();
                        response.EGS = command.Parameters["@EGS"].Value.ToString();
                        response.GESTOR_INTERNO = command.Parameters["@GESTOR_INTERNO"].Value.ToString();
                        response.ASISTENTE = command.Parameters["@ASISTENTE"].Value.ToString();





                        //log.Info("Registro insertado con contactID: " + request.contactId ?? "");

                    }
                    catch (Exception ex2)
                    {
                        log.Error("Error SP: " + ex2.Message);
                        transaction.Rollback();
                        response.CodigoRespuesta = false;
                        response.DetalleRespuesta = "Error SP: " + ex2.Message;

                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = false;
                response.DetalleRespuesta = "Error General: " + ex.Message;
                log.Error("Error: " + ex);
            }


            return response;
        }


        [BasicAuthentication]
        [HttpPut]
        [Route("CiabeiActualizarCliente")]
        public ResponseCrearCliente ActualizarCliente(RequestCrearCliente cliente)
        {
            var response = new ResponseCrearCliente();

            try
            {


                using (SqlConnection connection = GlobalVariable.connexionBD())
                {
                    connection.Open();

                    var transaction = connection.BeginTransaction();

                    var query = "GC_CiabeiActualizarCliente";
                    var command = new SqlCommand(query, connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                        Transaction = transaction
                    };

                    command.Parameters.AddWithValue("@CENTRAL", cliente.CENTRAL ?? "");
                    command.Parameters.AddWithValue("@TERRI", cliente.TERRI ?? "");
                    command.Parameters.AddWithValue("@OFICINA", cliente.OFICINA ?? "");
                    command.Parameters.AddWithValue("@OFICINA_N", cliente.OFICINA_N ?? "");
                    command.Parameters.AddWithValue("@GESTOR", cliente.GESTOR ?? "");
                    command.Parameters.AddWithValue("@NDOC_RUC", cliente.NDOC_RUC ?? "");
                    command.Parameters.AddWithValue("@NOMBRE", cliente.NOMBRE ?? "");
                    command.Parameters.AddWithValue("@NETCASH", cliente.NETCASH ?? "");
                    command.Parameters.AddWithValue("@VERSION", cliente.VERSION ?? "");
                    command.Parameters.AddWithValue("@NOMBRE1", cliente.NOMBRE1 ?? "");
                    command.Parameters.AddWithValue("@CARGO1", cliente.CARGO1 ?? "");
                    command.Parameters.AddWithValue("@CORREO1", cliente.CORREO1 ?? "");
                    command.Parameters.AddWithValue("@TELEFONO1", cliente.TELEFONO1 ?? "");
                    command.Parameters.AddWithValue("@CELULAR1", cliente.CELULAR1 ?? "");
                    command.Parameters.AddWithValue("@NOMBRE2", cliente.NOMBRE2 ?? "");
                    command.Parameters.AddWithValue("@CARGO2", cliente.CARGO2 ?? "");
                    command.Parameters.AddWithValue("@CORREO2", cliente.CORREO2 ?? "");
                    command.Parameters.AddWithValue("@TELEFONO2", cliente.TELEFONO2 ?? "");
                    command.Parameters.AddWithValue("@CELULAR2", cliente.CELULAR2 ?? "");
                    command.Parameters.AddWithValue("@NOMBRE3", cliente.NOMBRE3 ?? "");
                    command.Parameters.AddWithValue("@CARGO3", cliente.CARGO2 ?? "");
                    command.Parameters.AddWithValue("@CORREO3", cliente.CORREO3 ?? "");
                    command.Parameters.AddWithValue("@TELEFONO3", cliente.TELEFONO3 ?? "");
                    command.Parameters.AddWithValue("@CELULAR3", cliente.CELULAR3 ?? "");


                    try
                    {
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        command.Dispose();

                        response.CodigoRespuesta = true;
                        response.DetalleRespuesta = "OK";





                        //log.Info("Registro insertado con contactID: " + request.contactId ?? "");

                    }
                    catch (Exception ex2)
                    {
                        log.Error("Error SP: " + ex2.Message);
                        transaction.Rollback();
                        response.CodigoRespuesta = false;
                        response.DetalleRespuesta = "Error SP: " + ex2.Message;

                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                response.CodigoRespuesta = false;
                response.DetalleRespuesta = "Error General: " + ex.Message;
                log.Error("Error: " + ex);
            }

            return response;

        }



        [HttpPost]
        [Route("DownloadRecordings")]
        public ResponseRecordings ObtenerCH(RequestRecordings _request)
        {
            ResponseRecordings _response = new ResponseRecordings();
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
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Queueid,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "f3d5a13e-3b9e-4911-a4be-fb18ab05d6cc"

            }
            );
            /**
            oSegmentDetailQueryPredicate2.Add(new SegmentDetailQueryPredicate()
            {
                Type = SegmentDetailQueryPredicate.TypeEnum.Dimension,
                Dimension = SegmentDetailQueryPredicate.DimensionEnum.Recording,
                _Operator = SegmentDetailQueryPredicate.OperatorEnum.Matches,
                Value = "true"

            }
            );
            **/
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
                Type = SegmentDetailQueryClause.TypeEnum.And,
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
                    //if (conversations.ConversationId == "4502675b-cd34-4d5f-9fb3-31baa6391f47")
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
                      
                        string Telefono = string.Empty;
                       


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
                            String HoraInicio2 = FechaInicioRec.ToString("HHmmss");
                            Conversation result4 = conversationsApi.GetConversation(recording.ConversationId);

                            foreach (Participant oparticipant in result4.Participants)

                            {
                               

                                if (oparticipant.Purpose == "customer")
                                {
                                    Telefono = oparticipant.Ani.ToString();
                                    Telefono = Telefono.Replace(':', '-');
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


                            string filename = fechaInicio + HoraInicio2 + Telefono + recording.ConversationId;




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
                            WriteLog.EscribirLog("Intervalo: " + dates + " || Grabacion : " + fechaInicio + HoraInicio2 +Telefono+ recording.ConversationId);

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
                  //  x = operacionesSql.SPInsertCH(IDInteraccion: audioOgg.Substring(audioOgg.Length - 36), RecordingDate: "", RecordingID: "", ParticipantID: "", PeerID: "", ReceordingMP3: audioOgg);
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

        public class OperacionSQL
        {


            static string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionBD"];
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

    }
}
