using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScriptWSiBR.Models
{
    public class ResponseRecordings
    {
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public int? Contador { get; set; }





    }


    public class ResponseComision
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
        public string comision { get; set; }

    }


    public class ResponseCiabeiRUC
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
        public string CENTRAL { get; set; }
        public string TERRI { get; set; }
        public string OFICINA { get; set; }
        public string OFICINA_N { get; set; }
        public string GESTOR { get; set; }
        public string NDOC_RUC { get; set; }
        public string NOMBRE { get; set; }
        public string NETCASH { get; set; }
        public string VERSION { get; set; }
        public string NOMBRE1 { get; set; }
        public string CARGO1 { get; set; }
        public string CORREO1 { get; set; }
        public string TELEFONO1 { get; set; }
        public string CELULAR1 { get; set; }
        public string NOMBRE2 { get; set; }
        public string CARGO2 { get; set; }
        public string CORREO2 { get; set; }
        public string TELEFONO2 { get; set; }
        public string CELULAR2 { get; set; }
        public string NOMBRE3 { get; set; }
        public string CARGO3 { get; set; }
        public string CORREO3 { get; set; }
        public string TELEFONO3 { get; set; }
        public string CELULAR3 { get; set; }
        public string CARGO_EGS { get; set; }
        public string REGISTRO_EGS { get; set; }
        public string NOMBRE_EGS { get; set; }
        public string CORREO_EGS { get; set; }
        public string TELEFONO_EGS { get; set; }
        public string RPM_EGS { get; set; }
        public string CEL_EGS { get; set; }
        public string REGISTRO_GI { get; set; }
        public string NOMBRE_GI { get; set; }
        public string CORREO_GI { get; set; }
        public string TELEF_GI { get; set; }
        public string RPM_GI { get; set; }
        public string CEL_GI { get; set; }
        public string REGISTRO_AO { get; set; }
        public string NOMBRE_AO { get; set; }
        public string CORREO_AO { get; set; }
        public string TELEF_AO { get; set; }
        public string EGS { get; set; }
        public string GESTOR_INTERNO { get; set; }
        public string ASISTENTE { get; set; }
    }
    public class ResponseCiabeiEPA
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
    }
    public class ResponseCrearCliente
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
    }

    public class ResponseTicket
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
        public string ticket { get; set; }
    }


    public class ResponseUpdateWrapup
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
    }
    public class ResponseInsertLead
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
    }
    public class ResponseObtenerRegistro
    {
        public bool CodigoRespuesta { get; set; }
        public string DetalleRespuesta { get; set; }
        public string TERMINAL { get; set; }
        public string COMERCIO { get; set; }
        public string JEFE_COMERCIAL { get; set; }
        public string BACK_OFFICE { get; set; }
        public string EJECUTIVO_VENDEDOR { get; set; }
        public string EJECUTIVO_ANTERIOR { get; set; }
        public string EJECUTIVO_ACTUAL { get; set; }
        public string TELF_EJECUTIVO { get; set; }
        public string TIPO { get; set; }
        public string RUC { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string RUBRO { get; set; }
        public string DIRECCION { get; set; }
        public string REFERENCIA { get; set; }
        public string REGION { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string PROVINCIA { get; set; }
        public string DISTRITO { get; set; }
        public string LATITUD { get; set; }
        public string LONGITUD { get; set; }
        public string CADENA { get; set; }
        public string TITULAR { get; set; }
        public string DNI { get; set; }
        public string FIJO { get; set; }
        public string CELULAR_1 { get; set; }
        public string CELULAR_2 { get; set; }
        public string CORREO { get; set; }
        public string STATUS_MIGRACION { get; set; }
        public string USUARIO { get; set; }
        public string CELULAR { get; set; }
        public string OPERADOR { get; set; }
        public string RESP_OPER_KINPOS { get; set; }
        public string CELULAR_RESP { get; set; }
        public string CONTRATO_BIM { get; set; }
        public string BIMER { get; set; }
        public string INTERNET { get; set; }
        public string OPERADOR_INTERNET { get; set; }
        public string CONTRATO_INTERNET_BBVA { get; set; }
        public string STATUS { get; set; }
        public string FECHA_INSTALACION { get; set; }
        public string GRUPO_INSTALACION { get; set; }
        public string EQUIPO_INSTALADO { get; set; }
        public string ESTADO_FILE { get; set; }
        public string UBICACION_FILE { get; set; }
        public string TARIFA_INICIAL { get; set; }
        public string LETRERO_EXTERNO { get; set; }
        public string FECHA_BAJA { get; set; }
        public string STATUS_RECUPERO_EQP { get; set; }
        public string MOTIVO_BAJA { get; set; }
        public string MES_PASADO_3 { get; set; }
        public string MES_PASADO_2 { get; set; }
        public string MES_PASADO_1 { get; set; }
        public string MES_ACTUAL { get; set; }
        public string SEGMENTACION { get; set; }
        public string ESTADO_3270 { get; set; }
        public string CUENTA_AGENTE { get; set; }
        public string CUENTA_COMISIONES { get; set; }
        public string CODIGO_ACREEDORES { get; set; }
        public string CODIGO_OFICINA { get; set; }
        public string OFICINA_ASIGNADA { get; set; }
        public string DISTRITO2 { get; set; }
        public string TERRITORIO { get; set; }
        public string CAPA { get; set; }
        public string EMP { get; set; }
        public string EST { get; set; }
        public string SEMILLA { get; set; }
        public string PRD_MIGRADOS { get; set; }
        public string CAMPANAS { get; set; }
        public string CONTOMETRO { get; set; }
        public string ZONA { get; set; }
        public string SUPERVISOR_BBVA { get; set; }
        public string EST_REPARTO_CONT { get; set; }
        public string FECHA_REPARTO_CONT { get; set; }
        public string MIGRA_TARIFA { get; set; }
        public string FECHA_MIGRA_TARIFA { get; set; }
        public string TARIFA_ACTUAL { get; set; }
        public string FECHA_ALTA_SIST { get; set; }
        public string EJECUTIVO_TEMPORAL { get; set; }
        public string TELEF_TEMPORAL { get; set; }
        public string REGIONES_SIN_IGV { get; set; }
        public string DIVISION_DIST { get; set; }
        public string RUBRO_ESPECIF { get; set; }
        public string FECHA_NAC_TITULAR { get; set; }
        public string TIPO_AFILIACION { get; set; }
        public string FECHA_FIRMA_CONTRAT { get; set; }
        public string SECTOR { get; set; }


    }


    public class ResponseFechaOriginal
    {
        public string Anio { get; set; }
        public string Mes { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public string Minuto { get; set; }
        public string Segundo { get; set; }
        public int Edad { get; set; }
        public bool rangoFecha { get; set; }
        public string SiguienteDiaHabil { get; set; }
    }
}