using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;
using API.Helpers;
using API.Controllers.Base;

namespace API.Controllers
{
    [Authorize]
    [RoutePrefix("api/XXXXXXXXApiExample")]
    public class xxxApiExampleController : ApiController
    {

        private dbAppILUEntities db = new dbAppILUEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU")));

        #region v1

        ///<summary>
        ///Función  descripcion
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpPost]
        [Route("postData")]
        public IHttpActionResult postData(string codigo)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            //Parametro de POST
            var param1 = System.Web.HttpContext.Current.Request.Form["param1"];
            

            DateTime fechaActual = DateTime.Now;

            try
            {
                //Opcion 1
                if (true && false)
                {


                    response = true;
                    mensaje = "OK";
                }
                else
                {
                    respuestaApiJson = new int[0];
                    response = false;
                    mensaje = "No se logro la conexión";
                }

                //Opcion 2
                using (var context = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Para actualizar:
                        AITPermisoAgrupacion objeto = db.AITPermisoAgrupacion.Where(w => w.CodigoPermisoAgrupacion == 1).FirstOrDefault();

                        objeto.PermisoAgrupacion = "lol";
                        objeto.Activo = true;
                        objeto.Eliminado = false;
                        objeto.UsuarioInserto = "jabzum";
                        objeto.FechaInserto = fechaActual;

                        //Para insertar:
                        //db.AITPermisoAgrupacion.Add(objeto);
                        db.SaveChanges();
                        context.Commit();


                        response = true;
                        mensaje = "Guardado con éxito!";
                        respuestaApiJson = new { id = 19339339393 };
                    }
                    catch (Exception ex)
                    {
                        context.Rollback();
                        respuestaApiJson = null;
                        response = false;
                        mensaje = ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                respuestaApiJson = null;
                response = false;
                mensaje = ex.Message;
            }

            IDictionary<string, dynamic> data = new Dictionary<string, dynamic>
                {

                    { "response", response },
                    { "mensaje", mensaje },
                    { "data", respuestaApiJson }
                };

            return Ok(data);
        }

        ///<summary>
        ///Función  descripcion
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            //string param1 = System.Web.HttpContext.Current.Request.Form["param1"];
            //DateTime fechaActual = DateTime.Now;

            //Parametro de GET
            var codigoUsuario = System.Web.HttpContext.Current.Request.Params["codigoUsuario"];
            var valor = System.Web.HttpContext.Current.Request.QueryString["valor"];

            try
            {
                using(var context = db)
                {


                    response = true;
                    mensaje = "Ok";
                }

            }
            catch (Exception ex)
            {
                respuestaApiJson = null;
                response = false;
                mensaje = ex.Message;
            }

            IDictionary<string, dynamic> data = new Dictionary<string, dynamic>
                {

                    { "response", response },
                    { "mensaje", mensaje },
                    { "data", respuestaApiJson }
                };

            return Ok(data);
        }

        #endregion

        #region v2

        ///<summary>
        ///Función  descripcion
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public IHttpActionResult GetV2()
        {
            RespuestaHttpGenerica response = new RespuestaHttpGenerica();
            response.response = false;
            response.mensaje = "Error";
            response.data = null;

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            //string param1 = System.Web.HttpContext.Current.Request.Form["param1"];
            //DateTime fechaActual = DateTime.Now;

            //Parametro de GET
            int codigoUsuario;
            int.TryParse(System.Web.HttpContext.Current.Request.Params["codigoUsuario"], out codigoUsuario);
            //var valor = System.Web.HttpContext.Current.Request.QueryString["valor"];
            //int.Parse(codigoUsuario)




            try
            {
                using (var context = db)
                { 
                    
                    var usuarioModificadorCreador = (from u in context.AITUsuario
                                                     where u.CodigoUsuario == codigoUsuario
                                                     select u)
                                                    .FirstOrDefault();
                    response.response = true;
                    response.mensaje = "Ok";
                }

            }
            catch (Exception ex)
            {
                response.data = null;
                response.response = false;
                response.mensaje = ex.Message;
            }

            return Ok(response);
        }

        ///<summary>
        ///Función  descripcion
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpPost]
        [Route("postData")]
        public IHttpActionResult postDataV2(string codigo)
        {
            RespuestaHttpGenerica response = new RespuestaHttpGenerica();
            response.response = false;
            response.mensaje = "Error";
            response.data = null;

            //Parametro de POST
            var param1 = System.Web.HttpContext.Current.Request.Form["param1"];


            DateTime fechaActual = DateTime.Now;

            try
            {
                //Opcion 1
                if (true && false)
                {


                    response.response = true;
                    response.mensaje = "OK";
                }
                else
                {
                    response.data = new int[0];
                    response.response = false;
                    response.mensaje = "No se logro la conexión";
                }

                //Opcion 2
                using (var context = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Para actualizar:
                        AITPermisoAgrupacion objeto = db.AITPermisoAgrupacion.Where(w => w.CodigoPermisoAgrupacion == 1).FirstOrDefault();

                        objeto.PermisoAgrupacion = "lol";
                        objeto.Activo = true;
                        objeto.Eliminado = false;
                        objeto.UsuarioInserto = "jabzum";
                        objeto.FechaInserto = fechaActual;

                        //Para insertar:
                        //db.AITPermisoAgrupacion.Add(objeto);
                        db.SaveChanges();
                        context.Commit();


                        response.response = true;
                        response.mensaje = "Guardado con éxito!";
                        response.data = new { id = 19339339393 };
                    }
                    catch (Exception ex)
                    {
                        context.Rollback();
                        response.data = null;
                        response.response = false;
                        response.mensaje = ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                response.data = null;
                response.response = false;
                response.mensaje = ex.Message;
            }

            return Ok(response);
        }
        #endregion
    }


}
