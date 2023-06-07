using API.Helpers;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SAP.Middleware.Connector;
using System.Web.Http;
using API.Controllers.Base;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/public")]
    public class ApiPublicController : ApiController
    {
        //        public RfcDestination destinoRFC;
        //        public RfcRepository repositorioRFC;
        //        private dbAppILUEntities db = new dbAppILUEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU")));

        //        ///<summary>
        //        ///Función que obtiene las versiones del app y el histórico
        //        ///</summary>
        //        [HttpGet]
        //        [Route("app/version")]
        //        public IHttpActionResult appVersion()
        //        {

        //            List<PublicoHistorico> listadoHistorial = new List<PublicoHistorico>();
        //            listadoHistorial.Add(
        //                new PublicoHistorico()
        //                {
        //                    title = "v.1.00 - Nuevo módulo de Procesos de Gestión",
        //                    content = "- Nuevo módulo de proceso de gestión. Se podrán administrar las licencias mediante la gestión de tareas e interacción con el equipo."
        //                }
        //                );

        //            listadoHistorial.Add(
        //                new PublicoHistorico()
        //                {
        //                    title = "v.0.39 - Bug Fix",
        //                    content = "- Bug fix en los campos tipo fecha en los avisos."
        //                }
        //                );
        //            listadoHistorial.Add(
        //                new PublicoHistorico()
        //                {
        //                    title = "v.0.38 - Mejoras en los campos para Avisos SAP",
        //                    content = "- Se agregaron campos de tipo Hora y Separadores en los formularios de avisos. \n- Se agregó el histórico de cambios del app. \n-Usuarios asignados a una tarea podrán añadir causas. \n- Bug fixes varios."
        //                }
        //                );
        //            listadoHistorial.Add(
        //                new PublicoHistorico()
        //                {
        //                    title = "v.0.37 - Tareas en avisos y optimización de reacciones",
        //                    content = "- Se agregaron tareas como workflows a los avisos. \n - Se mejoró la interacción con las reacciones a un número limitado de emojis. \n- Bug Fixes varios."
        //                }
        //                );

        //            IDictionary<string, dynamic> version = new Dictionary<string, dynamic>() {
        //                                    { "ForceUpdateIos", false },
        //                                    { "LinkIpaIosDistribution", "https://appilu.s3.amazonaws.com/index.html" },
        //                                    { "VersionEstableIos", Helpers.Configuration.getVar("APP_VERSION_ESTABLE_IOS") },
        //                                    { "VersionMinimaIos", Helpers.Configuration.getVar("APP_VERSION_MIN_IOS")},
        //                                    { "ForceUpdateAndroid", false },
        //                                    { "VersionEstableAndroid", Helpers.Configuration.getVar("APP_VERSION_ESTABLE_ANDROID") },
        //                                    { "VersionMinimaAndroid", Helpers.Configuration.getVar("APP_VERSION_MIN_ANDROID")},
        //                                    { "Historial", listadoHistorial },
        //                                    { "response", true },
        //                                    { "mensaje", "Ok" }
        //                                };

        //            return Ok(version);
        //        }





        //        ///<summary>
        //        ///Función que envía notificaciones push a las personas que liberan pedidos y solped. Estas se ejecutan por medio de MS Flow (cuenta de jabzum@launion.com.gt)
        //        ///</summary>
        //        [HttpGet]
        //        [Route("notificaciones/liberaciones/{token}")]
        //        public IHttpActionResult notificacionesPedidos(string token)
        //        {
        //            IDictionary<string, dynamic> respuestaPushNotification = new Dictionary<string, dynamic>();
        //            IDictionary<string, dynamic> respuestaJson = new Dictionary<string, dynamic>();
        //            if (Helpers.Configuration.getVar("PUBLIC_API_AUTH_TOKEN") == token)
        //            {
        //                db.Configuration.LazyLoadingEnabled = false;
        //                db.Configuration.ProxyCreationEnabled = false;

        //                int[] tiposLiberacion = new int[] { 1, 3 }; //1 = Solped, 3 = Pedidos

        //                ApiSAPController sap = new ApiSAPController();

        //                using (var context = db)
        //                {
        //                    foreach (int tipoLiberacion in tiposLiberacion)
        //                    {
        //                        //Obtenmeos los códigos disponibles
        //                        var query = from uad in context.AITUsuarioAtributoDetalle
        //                                    where uad.CodigoUsuarioAtributo == tipoLiberacion && uad.Activo && !uad.Eliminado
        //                                    group uad by new
        //                                    {
        //                                        Valor = uad.Valor
        //                                    }
        //                                    into g
        //                                    select new
        //                                    {
        //                                        Valor = g.Key.Valor
        //                                    };

        //                        foreach (var codigo in query)
        //                        {
        //                            List<dynamic> contenidoSap = new List<dynamic>();
        //                            try
        //                            {
        //                                if (ConexionSAP(Helpers.Configuration.getVar("SAPUser").ToString(), Helpers.Configuration.getVar("SAPPassword").ToString()))
        //                                {
        //                                    //Obtenemos los pedidos pendientes de liberación
        //                                    if (tipoLiberacion == 1)
        //                                    {
        //                                        contenidoSap = sap.getLiberacionesFunc(codigo.Valor, destinoRFC, repositorioRFC);
        //                                    }
        //                                    else
        //                                    {
        //                                        contenidoSap = sap.getLiberacionesPedidosFunc(codigo.Valor, destinoRFC, repositorioRFC);
        //                                    }


        //                                    if (contenidoSap.Count > 0)
        //                                    {
        //                                        //Hay que notificar al grupo.
        //                                        var tokens = from uad in db.AITUsuarioAtributoDetalle
        //                                                     join utd in db.AITUsuarioTokenDispositivo on uad.CodigoUsuario equals utd.CodigoUsuario
        //                                                     where uad.CodigoUsuarioAtributo == tipoLiberacion && uad.Activo && !uad.Eliminado && utd.Activo && !utd.Eliminado
        //                                                     select utd.CodigoUsuarioTokenDispositivo;

        //                                        int[] endpoints = tokens.ToArray();

        //                                        dynamic respuestaAWSPush;

        //                                        if (tipoLiberacion == 1)
        //                                        {
        //                                            //Solped
        //                                            respuestaAWSPush = Helpers.AWSPushNotification.enviarNotificacionAEndpoints(endpoints, "Ingresa al app y autoriza las solicitudes de pedidos pendientes con el código \"" + codigo.Valor + "\"", "Tienes " + contenidoSap.Count + " SolPed por liberar", "appilu://home/liberaciones/solped");
        //                                        }
        //                                        else
        //                                        {
        //                                            //Pedidos
        //                                            respuestaAWSPush = Helpers.AWSPushNotification.enviarNotificacionAEndpoints(endpoints, "Ingresa al app y autoriza los pedidos pendientes con el código \"" + codigo.Valor + "\"", "Tienes " + contenidoSap.Count + " pedidos por liberar", "appilu://home/liberaciones/pedidos");
        //                                        }

        //                                        respuestaPushNotification.Add(tipoLiberacion.ToString(), respuestaAWSPush);

        //                                    }
        //                                    //Obtenemos el listado
        //                                    //response = true;
        //                                }
        //                                else
        //                                {
        //                                    //response = false;
        //                                    //mensaje = "No se pudo conectar a SAP, intente nuevamente";
        //                                    //throw new Exception("No se logró la conexión a SAP.");
        //                                }

        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                //response = false;
        //                                //mensaje = ex.Message;
        //                            }

        //                            try
        //                            {
        //                                RfcSessionManager.EndContext(destinoRFC);
        //                            }
        //                            catch (Exception ex) { }
        //                        }

        //                    }

        //                }
        //                respuestaJson.Add("tipoLiberacion", respuestaPushNotification);
        //                respuestaJson.Add("response", true);
        //            }
        //            else
        //            {
        //                respuestaJson.Add("response", false);
        //                respuestaJson.Add("mensaje", "Authorize is required");
        //            }


        //            return Ok(respuestaJson);
        //        }

        //        ///<summary>
        //        ///Función que envía notificaciones push de los comunicados del área de comunicaciones
        //        ///</summary>
        //        [HttpGet]
        //        [Route("notificaciones/comunicaciones/{token}")]
        //        public IHttpActionResult notificacionesComunicaciones(string token)
        //        {
        //            IDictionary<string, dynamic> respuestaJson = new Dictionary<string, dynamic>();
        //            List<dynamic> respuestaJsonNotificaciones = new List<dynamic>();
        //            if (Helpers.Configuration.getVar("PUBLIC_API_AUTH_TOKEN") == token)
        //            {
        //                db.Configuration.LazyLoadingEnabled = false;
        //                db.Configuration.ProxyCreationEnabled = false;

        //                //Obtenemos las notificaciones pendientes de envío.
        //                using (var context = db.Database.BeginTransaction())
        //                    {
        //                        try
        //                        {
        //                        var notificacionesPendientesQuery = (from n in db.AITNotificacion
        //                                                            where n.CodigoNotificacionEstado == 1 && n.FechaEnvio <= DateTime.Now
        //                                                            select n).ToList<AITNotificacion>();

        //                        if (notificacionesPendientesQuery.Count > 0)
        //                        {
        //                            notificacionesPendientesQuery.ForEach(x => 
        //                            {
        //                                x.CodigoNotificacionEstado = 2;
        //                            });

        //                            db.SaveChanges();

        //                            foreach (AITNotificacion notificacion in notificacionesPendientesQuery)
        //                            {
        //                                //Obtemos los tokens de los usuarios asociados a los grupos de la notificacion.
        //                                int[] tokens = (from ng in db.AITNotificacionGrupo
        //                                              join gu in db.AITGrupoUsuario on ng.CodigoGrupo equals gu.CodigoGrupo
        //                                              where gu.Activo && !gu.Eliminado
        //                                              join utd in db.AITUsuarioTokenDispositivo on gu.CodigoUsuario equals utd.CodigoUsuario
        //                                              where utd.Activo && !utd.Eliminado && ng.CodigoNotificacion == notificacion.CodigoNotificacion
        //                                              select utd.CodigoUsuarioTokenDispositivo)
        //                                              .Distinct() //->>>>>> ESTE DISTINCT HAY QUE VALIDARLO TAMBIÉN.
        //                                              .ToArray();

        //                                //Enviamos notificaciones.
        //                                dynamic respuestaAws =  Helpers.AWSPushNotification.enviarNotificacionAEndpoints(tokens, notificacion.DescripcionCorta, notificacion.Titulo, "appilu://notificaciones/notificacion/" + notificacion.CodigoNotificacion);

        //                                //Enviamos notificaciones en tercer plano.

        //                                //HAY QUE VALIDAR ESTO TODO
        //                                //Task.Run(() => Helpers.AWSPushNotification.enviarNotificacionAEndpoints(tokens, notificacion.DescripcionCorta, notificacion.Titulo, "appilu://notificaciones/notificacion/" + notificacion.CodigoNotificacion));

        //                                //Actualizamos el estado de la notificación a enviado. CodigoNotificacionEstado = 3
        //                                notificacion.FechaFinEnvio = DateTime.Now;
        //                                notificacion.CodigoNotificacionEstado = 3;
        //                                db.SaveChanges();

        //                                IDictionary<string, dynamic> respuestaPush = new Dictionary<string, dynamic>() {
        //                                    { "CodigoNotificacion", notificacion.CodigoNotificacion },
        //                                    { "respuestaPinpoint", respuestaAws }
        //                                };
        //                                respuestaJsonNotificaciones.Add(respuestaPush);
        //                            }
        //                        }


        //                        respuestaJson.Add("response", true);
        //                        respuestaJson.Add("mensaje", "Se ejecuto el proceso correctamente");
        //                        context.Commit();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            //Si falla deshace los cambios
        //                            context.Rollback();
        //                            respuestaJson.Add("response", false);
        //                            respuestaJson.Add("mensaje", "No pudo procesar la transacción en la BD ");
        //                            respuestaJson.Add("respuestaDb", ex.Message);
        //                        }
        //                    }
        //                respuestaJson.Add("notificaciones", respuestaJsonNotificaciones);


        //            }
        //            else
        //            {
        //                respuestaJson.Add("response", false);
        //            }


        //            return Ok(respuestaJson);

        //        }

        //        //[HttpGet]
        //        //[Route("pruebaCompensaciones")]
        //        //public IHttpActionResult pruebaUniserdbCompensaciones()
        //        //{
        //        //    dbCompensacionEntities db = new dbCompensacionEntities(ConnectionHelper.CreateConnectionString("uniserdb", "dbCompensacion", "usrAppILU", "UsR@99I1U2019"));

        //        //    dynamic lol;
        //        //    using(var context = db)
        //        //    {
        //        //        var query = from a in context.SCFEmpleadosPermitidosAUsuario("mmonzon", "559")
        //        //                    select a;

        //        //        //var query = context.Database.SqlQuery<dynamic>("select * from SCFEmpleadosPermitidosAUsuario('mmonzon','559')").ToArray();


        //        //        lol = query.ToArray();

        //        //    }
        //        //    return Ok(lol);
        //        //}




        //        /// <summary>
        //        /// Función para conexión a SAP
        //        /// </summary>
        //        /// <param name="usuario">Usuario de SAP</param>
        //        /// <param name="password">Password del usaurio</param>
        //        /// <returns></returns>
        //        public bool ConexionSAP(string usuario, string password)
        //        {
        //            try
        //            {
        //                RfcConfigParameters parametro = new RfcConfigParameters();
        //                parametro.Add(RfcConfigParameters.Name, Helpers.Configuration.getVar("SAPName").ToString());
        //                parametro.Add(RfcConfigParameters.AppServerHost, Helpers.Configuration.getVar("SAPAppServerHost").ToString());
        //                parametro.Add(RfcConfigParameters.LogonGroup, Helpers.Configuration.getVar("SAPLogonGroup").ToString());
        //                parametro.Add(RfcConfigParameters.SystemNumber, Helpers.Configuration.getVar("SAPSystemNumber").ToString());
        //                parametro.Add(RfcConfigParameters.User, usuario);
        //                parametro.Add(RfcConfigParameters.Password, password);
        //                parametro.Add(RfcConfigParameters.Client, Helpers.Configuration.getVar("SAPClient").ToString());
        //                parametro.Add(RfcConfigParameters.Language, Helpers.Configuration.getVar("SAPLanguage").ToString());
        //                parametro.Add(RfcConfigParameters.PoolSize, Helpers.Configuration.getVar("SAPPoolSize").ToString());
        //                parametro.Add(RfcConfigParameters.MaxPoolSize, Helpers.Configuration.getVar("SAPMaxPoolSize").ToString());
        //                parametro.Add(RfcConfigParameters.IdleTimeout, Helpers.Configuration.getVar("SAPIdleTimeout").ToString());

        //                destinoRFC = RfcDestinationManager.GetDestination(parametro);
        //                repositorioRFC = destinoRFC.Repository;
        //                RfcSessionManager.BeginContext(destinoRFC);

        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                return false;
        //            }
        //        }



        [HttpGet]
        [Route("helloworld")]
        public IHttpActionResult helloworld()
        {
            

            return Ok("hello world");

        }


    }


}