using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Renci.SshNet;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using log4net;


namespace EcontactConsoleInteractionAPI
{
    
    public class ConsolaAppDNC
    {
        private static readonly ILog log = LogManager.GetLogger("RollingFileLog");
        static void Main()
        {


            Console.WriteLine("Iniciando Programa");
            log.Info("Programa Iniciado");

            var apiInstance = new OutboundApi();
            var dncListId = "e098cc20-f405-47c3-8b31-de146fe562bc"; // string | DncList ID
           // var rutaCsv = "C:/Test/";
            var rutaCsv = "C:/DNCService/";
        
             List<string> body = new List<string>();

            try
            {
                #region FechaInterval
                DateTime now = DateTime.UtcNow;

                DateTime LastHour = DateTime.UtcNow.AddHours(-1);
                DateTime LastDay = LastHour.AddDays(-1);

                //AÑO
                var NowYear = now.Year.ToString();
                var LHYear = LastHour.Year.ToString();
                var LDYear = LastDay.Year.ToString();

                //MES
                var NowMonth = now.Month.ToString();
                if (NowMonth.Length < 2)
                {
                    NowMonth = "0" + NowMonth;
                }

                var LHMonth = LastHour.Month.ToString();
                if (LHMonth.Length < 2)
                {
                    LHMonth = "0" + LHMonth;
                }

                var LDMonth = LastDay.Month.ToString();
                if (LDMonth.Length < 2)
                {
                    LDMonth = "0" + LDMonth;
                }

                //DIA
                var NowDay = now.Day.ToString();
                if (NowDay.Length < 2)
                {
                    NowDay = "0" + NowDay;
                }

                var LHDay = LastHour.Day.ToString();
                if (LHDay.Length < 2)
                {
                    LHDay = "0" + LHDay;
                }
                var LDDay = LastDay.Day.ToString();
                if (LDDay.Length < 2)
                {
                    LDDay = "0" + LDDay;
                }

                //Hora
                var NowHour = now.Hour.ToString();
                if (NowHour.Length < 2)
                {
                    NowHour = "0" + NowHour;
                }
                var LHHour = LastHour.Hour.ToString();
                if (LHHour.Length < 2)
                {
                    LHHour = "0" + LHHour;
                }

                var LDHour = LastDay.Hour.ToString();
                if (LDHour.Length < 2)
                {
                    LDHour = "0" + LDHour;
                }
                #endregion


                Console.WriteLine("Fecha a tomar: "+ LHYear+LHMonth+LHDay);
                log.Info("Fecha a tomar: " + LHYear + LHMonth + LHDay);

                #region Generacion de TokenPureCloud
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken("6402d508-1da9-48dc-a94d-0137fe4e38a5", "bMD0a_p1D16e8NxX1pWfd8aRYFvQ5zs5AIjq1BVKdS0");
                var token_PC = accessTokenInfo.AccessToken;
                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;

                #endregion
                #region SFTP

                Dictionary<string, string> ListaDNC = new Dictionary<string, string>();
                using (SftpClient cliente = new SftpClient(new PasswordConnectionInfo("sftp.ibrlatam.com", "sftp_cencosud_02", "SZ2JX9tJog")))
                {
                    cliente.Connect();

                    var paths = cliente.ListDirectory("./SERNAC");

                    foreach (var path in paths)
                    {
                        if (path.Name.Contains("BLACKLIST_" + LHYear + LHMonth + LHDay))
                        {
                            //ArchivosDNC.Add(path.FullName);
                            //nombreDNC.Add(path.Name);
                            ListaDNC.Add(path.FullName, path.Name);
                            
                        }
                    }
                    //string serverfile = @".\SERNAC";
                    foreach (var archivDNC in ListaDNC)
                    {
                        string localfile = @"C:\DNCService\" + archivDNC.Value;

                        using (Stream stream = File.OpenWrite(localfile))
                        {

                            //cliente.DownloadFile(archivDNC.Key, stream, x => Console.WriteLine(x));
                             cliente.DownloadFile(archivDNC.Key, stream);
                            Console.WriteLine("Archivo Descargado " + archivDNC.Value);
                            log.Info("Archivo Descargado " + archivDNC.Value);
                        }

                    }
                    cliente.Disconnect();
                }
                #endregion

                string[] read;
                char[] seperators = { '|' };
                int totalNum = 0;
                foreach (var archivDNC in ListaDNC)
                {
                    int contador = 0;

                    StreamReader sr = new StreamReader(rutaCsv + archivDNC.Value, System.Text.Encoding.Default);
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
                Console.WriteLine("Total de contactos encontrados: " + totalNum);
                log.Info("Total de contactos encontrados: " + totalNum);
                if (totalNum < 1000)
                {
                    
                    apiInstance.PostOutboundDnclistPhonenumbers(dncListId, body);
                    Console.WriteLine("Subiendo a GC: " + totalNum);
                    log.Info("Subiendo a GC: " + totalNum);
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
                        Console.WriteLine("Subiendo a GC: " +i+"/" + totalNum);
                        log.Info("Subiendo a GC: " + i + "/" + totalNum);
                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine("Error: " + e.Message);
                log.Info("Error: " + e.Message);
            }


            Console.WriteLine("Finalizando programa");
            log.Info("Finalizando programa");
            Thread.Sleep(10000);
        }

        }
   
   
   
}
