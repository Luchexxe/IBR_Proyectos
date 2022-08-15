using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;



namespace EcontactConsoleInteractionAPI
{

    class ApiSendData
    {
       public string startTime { get; set; }
        public string endTime { get; set; }
        public int PageSize { get; set; }

    }

    class Respuesta
    {
        public string Estado { get; set; }
        public string Fecha { get; set; }


    }

    class Program
    {

static HttpClient client = new HttpClient();

       
        static async Task<Uri> DownloadRecordings(ApiSendData _request)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("Grabaciones/DownloadRecordings", _request);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Uri> CallHistory(ApiSendData _request)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("CallHistory/IntegrationData", _request);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        static async Task<Uri> CallHistoryRepro(ApiSendData _request)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("CallHistory/IntegrationDataRepro", _request);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }


        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            Console.WriteLine("Iniciando Programa");
          
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client.BaseAddress = new Uri("https://gcloudws1.ibrlatam.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

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


                // Create a new product
                ApiSendData cuerpo = new ApiSendData
                {
                    startTime = LHYear + "-" + LHMonth + "-" + LHDay + "T" + LHHour + ":00:00",
                    endTime = NowYear + "-" + NowMonth + "-" + NowDay + "T" + NowHour + ":00:00",
                    PageSize = 100
                    //startTime = "2021-08-09T00:00:00",
                    //endTime = "2021-08-09T23:00:00",
                    //PageSize = 100

                };

                ApiSendData cuerpo2 = new ApiSendData
                {
                    startTime = LHYear + "-" + LHMonth + "-" + LHDay + "T" + LHHour + ":00:00",
                    endTime = NowYear + "-" + NowMonth + "-" + NowDay + "T" + NowHour + ":00:00",
                    //startTime = "2021-08-09T00:00:00",
                    //endTime = "2021-08-09T23:00:00",
                };
                ApiSendData cuerpo3 = new ApiSendData
                {
                    startTime = LDYear + "-" + LDMonth + "-" + LDDay + "T" + "03:00:00",
                    endTime = NowYear + "-" + NowMonth + "-" + NowDay + "T" + "03:00:00",
                    //startTime = "2021-08-09T00:00:00",
                    //endTime = "2021-08-09T23:00:00",
                };


                Console.WriteLine("iniciando Servicios");

                if (LHHour == "05")
                {
                    await Task.WhenAll(
                       new Task[] {
                            CallHistory(cuerpo),
                            DownloadRecordings(cuerpo2),
                            CallHistoryRepro(cuerpo3)
                       }
                   );
                    Console.WriteLine("Finalizado");
                }
                else
                {
                    await Task.WhenAll(
                           new Task[] {
                            CallHistory(cuerpo),
                            DownloadRecordings(cuerpo2)
                           }
                       );
                    Console.WriteLine("Finalizado");
                }
               

                //Console.WriteLine("CallHistory||El intervalo será desde " + cuerpo.startTime + " hasta " + cuerpo.endTime);
                //var url2 = await CallHistory(cuerpo);

                //Console.WriteLine("Download Recordings||El intervalo será desde " + cuerpo.startTime + " hasta " + cuerpo.endTime);
                //var url = await DownloadRecordings(cuerpo2);





            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Finalizando programa");
            //Console.ReadLine();
            Thread.Sleep(10000);
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        RunAsync().Wait(60000);

    //    }


    //    static async Task RunAsync()
    //    {
    //        Console.WriteLine("Iniciando Programa");
    //        try
    //        {

    //            DateTime now = DateTime.UtcNow.AddHours(-1);

    //            var Curyear = now.Year.ToString();

    //            var Curmonth = now.Month.ToString();

    //            if (Curmonth.Length < 2)
    //            {
    //                Curmonth = "0" + Curmonth;
    //            }

    //            var Curday = now.Day.ToString();
    //            if (Curday.Length < 2)
    //            {
    //                Curday = "0" + Curday;
    //            }


    //            var CurHour = now.Hour.ToString();
    //            if (CurHour.Length < 2)
    //            {
    //                CurHour = "0" + CurHour;
    //            }
    //            using (var client2 = new HttpClient())
    //            {
    //                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
    //                Console.WriteLine("Intentando abrir URL...");
    //                client2.BaseAddress = new Uri("https://gcloudws1.ibrlatam.com/CallHistory/");
    //                //client.BaseAddress = new Uri("http://localhost:49637/");

    //                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth","xyz");
    //                client2.DefaultRequestHeaders.Accept.Clear();
    //                client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



    //                var cuerpo = new ApiSendData()
    //                {

    //                    startTime = Curyear + "-" + Curmonth + "-" + Curday + "T" + CurHour + ":00:00",
    //                    endTime = Curyear + "-" + Curmonth + "-" + Curday + "T" + CurHour + ":59:59",
    //                    PageSize = 100
    //                };

    //                Console.WriteLine("El intervalo será desde " + cuerpo.startTime + " hasta " + cuerpo.endTime);

    //                try
    //                {

    //                    var response = await client2.PostAsJsonAsync("IntegrationData", cuerpo);


    //                }
    //                catch (Exception e)
    //                {

    //                    Console.WriteLine("Estado: ERROR ||" + "Fecha: " + DateTime.Now + " || Exception: " + e.Message);
    //                    Thread.Sleep(10000);
    //                }


    //            }
    //            using (var client = new HttpClient())
    //            {
    //                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
    //                Console.WriteLine("Intentando abrir URL...");
    //                client.BaseAddress = new Uri("http://172.20.1.231:8081/");
    //                //client.BaseAddress = new Uri("http://localhost:49637/");

    //                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth","xyz");
    //                client.DefaultRequestHeaders.Accept.Clear();
    //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


    //                var cuerpo = new ApiSendData()
    //                {

    //                     startTime = Curyear + "-" + Curmonth + "-" + Curday + "T" + CurHour + ":00:00",
    //                     endTime = Curyear + "-" + Curmonth + "-" + Curday + "T" + CurHour+":59:59",

    //                };

    //                Console.WriteLine("El intervalo será desde " + cuerpo.startTime + " hasta " + cuerpo.endTime);

    //                try
    //                {

    //                    var response = await client.PostAsJsonAsync("DownloadRecordings", cuerpo);


    //                }
    //                catch (Exception e)
    //                {

    //                    Console.WriteLine("Estado: ERROR ||" + "Fecha: " + DateTime.Now + " || Exception: "+e.Message);
    //                    Thread.Sleep(10000);
    //                }


    //            }


    //        }

    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.Message);
    //            Thread.Sleep(60000);
    //        }


    //    }



    //}
}
