using log4net;
using Newtonsoft.Json.Linq;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebListManagement.Models;

namespace WebListManagement.Controllers
{
    public class CloudController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger("RollingFileLog");
        private static readonly HttpClient client = new HttpClient();
        // GET: Cloud
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListManagement()
        {
            return View();
        }

        public ActionResult ListaExistente()
        {
            try
            {
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(GlobalVariable.idCliente, GlobalVariable.clientSecret);
                System.Console.WriteLine("Access token=" + accessTokenInfo.AccessToken);
                log.Info("Access token=" + accessTokenInfo.AccessToken);
                var token_PC = accessTokenInfo.AccessToken;

                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;

                //Obtener DivisionsId
                var instanceObjectApi = new ObjectsApi();
                int iPageIndexDiv = 1;
                var resultDivisions = instanceObjectApi.GetAuthorizationDivisions(100, iPageIndexDiv);
                var listDivisionsId = new List<string>();

                foreach (var item in resultDivisions.Entities)
                {
                    listDivisionsId.Add(item.Id);
                }

                //


                var instanceOutboundApi = new OutboundApi();
                int iPageIndex = 1;
                var resultContactlist = instanceOutboundApi.GetOutboundContactlists(pageSize: 100, pageNumber: iPageIndex, divisionId: listDivisionsId, sortOrder: "ascending");

                var listContactlists = new List<string>();

                foreach (var item in resultContactlist.Entities)
                {
                    listContactlists.Add(item.Name);
                }

                var selectListContactlists = listContactlists.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

                ViewData["selectListContactlists"] = selectListContactlists;
            }
            catch (Exception ex)
            {
                log.Error("Error cargar Lista Existente: " + ex.ToString());
            }
            return View();
        }

        public ActionResult ListaNueva()
        {
            try
            {
                var accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(GlobalVariable.idCliente, GlobalVariable.clientSecret);
                System.Console.WriteLine("Access token=" + accessTokenInfo.AccessToken);
                log.Info("Access token=" + accessTokenInfo.AccessToken);
                var token_PC = accessTokenInfo.AccessToken;

                PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = token_PC;

                var instanceObjectApi = new ObjectsApi();
                int iPageIndex = 1;
                var resultDivisions = instanceObjectApi.GetAuthorizationDivisions(100, iPageIndex);
                var listDivisions = new List<string>();

                foreach (var item in resultDivisions.Entities)
                {
                    listDivisions.Add(item.Name);
                }

                var selectListDivisions = listDivisions.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();

                ViewData["selectListDivisions"] = selectListDivisions;
            }
            catch (Exception ex5)
            {
                log.Error("Error cargar Lista NUeva: " + ex5.ToString());
            }

            return View();
        }


        [HttpPost]
        public ActionResult _PartialList1(HttpPostedFileBase file, string nombre, string telefonos, string tipostelefono, string identificador, string selectListDivisions)
        {
            try
            {
                if (file == null)
                {
                    var messageText = "No ha seleccionado un archivo.";
                    return RedirectToAction("Error", new { message = messageText });
                }
                else
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                         if(fileName.Substring(fileName.Length - 3, 3) == "csv")
                        {
                            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                            file.SaveAs(path);


                            #region ConsumirAPICrearLista

                            string PathStatic = "C:/inetpub/wwwroot/iBR/AppListManagement/App_Data/uploads/";

                            try
                            {
                                string URI = "https://gcloudws1.ibrlatam.com/AppScriptService/ListaNuevaContactos";

                                var values = new Dictionary<string, string>
                                    {
                                        { "nombreContactList", nombre.ToUpper() },
                                        { "campoTelefono", telefonos.ToUpper() },
                                        { "tipoTelefono", tipostelefono.ToUpper() },
                                        { "campoIdentificador", identificador.ToUpper() },
                                        { "division", selectListDivisions },
                                        { "rutaCsv", PathStatic+fileName }
                                    };

                                var content = new FormUrlEncodedContent(values);
                                var successcode = false;
                                var statuscode = "";
                                Task.Run(async () =>
                                {
                                    var response = await client.PostAsync(URI, content);
                                    var responseString = await response.Content.ReadAsStringAsync();
                                    log.Info($"Respuesta WS Lista Existente es: {responseString.ToString()}");

                                    successcode = response.IsSuccessStatusCode;
                                    statuscode = response.StatusCode.ToString();
                                }).GetAwaiter().GetResult();

                                if (successcode)
                                {
                                    return RedirectToAction("Success");
                                }
                                else
                                {
                                    var messageText = "Error - WS Status Code: " + statuscode;
                                    return RedirectToAction("Error", new { message = messageText });
                                }
                            }
                            catch (Exception ex3)
                            {
                                log.Error("Error: " + ex3.ToString());
                                var messageText = ex3.ToString();
                                return RedirectToAction("Error", new { message = messageText });
                                
                            }

                            #endregion

                            
                        }
                        else
                        {
                            log.Error("Error: No es el formato de archivo solicitado.");
                            var messageText = "No es el formato de archivo solicitado.";
                            return RedirectToAction("Error", new { message = messageText });
                        }

                        
                    }
                    else
                    {
                        log.Error("Error: El archivo no contiene datos.");
                        var messageText = "El archivo no contiene datos.";
                        return RedirectToAction("Error", new { message = messageText });
                    }

                    
                }
            }
            catch(Exception ex2)
            {
                log.Error("Error: " + ex2.ToString());
                var messageText = ex2.ToString();
                return RedirectToAction("Error", new { message = messageText });
            }

            
        }

        [HttpPost]
        public ActionResult SeleccionAccion(string SelectionModo)
        {
            if (SelectionModo != null)
            {
                if (SelectionModo == "1")
                {
                    return RedirectToAction("ListaNueva");
                }
                else if (SelectionModo == "2")
                {
                    return RedirectToAction("ListaExistente");
                }
                else
                {
                    var messageText = "Error.";
                    return RedirectToAction("Error", new { message = messageText });
                }
                
            }
            else
            {
                var messageText = "No ha seleccionado una opción.";
                return RedirectToAction("Error", new { message = messageText });
            }
            
        }

        [HttpPost]
        public ActionResult _PartialList2(HttpPostedFileBase file, string identificador, string selectListContactlists)
        {
            try
            {
                if (file == null)
                {
                    var messageText = "No ha seleccionado un archivo.";
                    return RedirectToAction("Error", new { message = messageText });
                }
                else
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        if (fileName.Substring(fileName.Length - 3, 3) == "csv")
                        {
                            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                            file.SaveAs(path);

                            #region ConsumirApiListaExistente
                            string PathStatic = "C:/inetpub/wwwroot/iBR/AppListManagement/App_Data/uploads/";

                            try
                            {
                                string URI = "https://gcloudws1.ibrlatam.com/AppScriptService/IngresoContactosListaExistente";

                                var values = new Dictionary<string, string>
                                    {
                                        { "nombreContactList", selectListContactlists },
                                        { "campoIdentificador", identificador.ToUpper() },
                                        { "rutaCsv", PathStatic+fileName }
                                    };

                                var content = new FormUrlEncodedContent(values);
                                var successcode = false;
                                var statuscode = "";
                                Task.Run(async () =>
                                {
                                    var response = await client.PostAsync(URI, content);
                                    var responseString = await response.Content.ReadAsStringAsync();
                                    log.Info($"Respuesta WS Lista Existente es: {responseString.ToString()}");

                                    successcode = response.IsSuccessStatusCode;
                                    statuscode = response.StatusCode.ToString();

                                }).GetAwaiter().GetResult();

                                if (successcode)
                                {
                                    return RedirectToAction("Success");
                                }
                                else
                                {
                                    var messageText = "Error - WS Status Code: "+statuscode;
                                    return RedirectToAction("Error", new { message = messageText });
                                }

                            }
                            catch (Exception ex3)
                            {
                                log.Error("Error: " + ex3.ToString());
                                var messageText = ex3.ToString();
                                return RedirectToAction("Error", new { message = messageText });

                            }

                            #endregion

                            
                        }
                        else
                        {
                            log.Error("Error: No es el formato de archivo solicitado." );
                            var messageText = "No es el formato de archivo solicitado.";
                            return RedirectToAction("Error", new { message = messageText });
                        }


                    }
                    else
                    {
                        log.Error("Error: El archivo no contiene datos.");
                        var messageText = "El archivo no contiene datos.";
                        return RedirectToAction("Error", new { message = messageText });
                    }


                }
            }
            catch (Exception ex2)
            {
                log.Error("Error: " + ex2.ToString());
                var messageText = ex2.ToString();
                return RedirectToAction("Error", new { message = messageText });
            }


        }


        public ActionResult Error(string message)
        {
            ViewBag.message = message;

            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}