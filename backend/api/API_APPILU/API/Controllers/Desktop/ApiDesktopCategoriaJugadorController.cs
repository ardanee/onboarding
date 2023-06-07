using API.Controllers.Base;
using API.Controllers.ModeloPosts;
using API.Helpers;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers.Desktop
{
    [Authorize]
    [RoutePrefix("api/desktop/categoriajugador")]
    public class ApiDesktopCategoriaJugadorController : ApiController
    {
        private dbLigaFantasiaEntities db = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));

        ///<summary>
        ///Obtenemos las categorías de una liga específica
        ///</summary>
        /// <param name="id">El id de la liga</param>
        /// <returns></returns>
        [HttpGet]
        [Route("liga/{idLiga}")]
        public IHttpActionResult liga(int idLiga)
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

                    var categorias = (from cj in context.LFTCategoriaJugador
                                                     where cj.IdLiga == idLiga
                                                     && cj.Activo == true && cj.Eliminado == false
                                                     select cj
                                                     )
                                                    .ToList();
                    response.data = categorias;
                    response.response = true;
                    response.mensaje = "Ok";
                }

            }
            catch (Exception ex)
            {
                response.data = new List<string>();
                response.response = false;
                response.mensaje = ex.Message;
            }

            return Ok(response);
        }


        [HttpPost]
        [Route("guardar")]
        public IHttpActionResult grabarEditar([FromBody] CategoriaJugadorGuardar categoriaPost)
        {
            RespuestaHttpGenerica response = new RespuestaHttpGenerica();
            response.response = false;
            response.mensaje = "Error";
            response.data = null;
            LFTCategoriaJugador categoria = new LFTCategoriaJugador();

            DateTime fechaActual = DateTime.Now;

            using (var context = db.Database.BeginTransaction())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                try
                {
                    //Valida requeridos
                    if (string.IsNullOrEmpty(categoriaPost.Nombre))
                    {
                        throw new Exception("El campo nombre es requerido");
                    }

                    var usuarioOpera = db.LFTUsuario.Where(w => w.CodigoUsuario == categoriaPost.CodigoUsuarioOpera).FirstOrDefault();

                    

                    //Si la plantilla no existe, crea una nueva
                    if (categoriaPost.Id == null)
                    {
                        //Si es un nuevo problema crea una nueva plantilla que corresponda al problema
                        categoria.IdLiga = categoriaPost.IdLiga;
                        categoria.Nombre = categoriaPost.Nombre;
                        categoria.MinimoJugadores = categoriaPost.MinimoJugadores;
                        categoria.MaximoJugadores = categoriaPost.MaximoJugadores;
                        categoria.Activo = true;
                        categoria.Eliminado = false;
                        categoria.CodigoUsuarioInserto = categoriaPost.CodigoUsuarioOpera;
                        categoria.UsuarioInserto = usuarioOpera.Usuario;
                        categoria.FechaInserto = fechaActual;

                        //Graba nueva liga
                        db.LFTCategoriaJugador.Add(categoria);

                    }
                    else
                    {   
                        //Registro editar
                        categoria = db.LFTCategoriaJugador.Where(x => x.Id == categoriaPost.Id).FirstOrDefault();
                        if (categoria != null)
                        {
                            categoria.Nombre = categoriaPost.Nombre;
                            categoria.MinimoJugadores = categoriaPost.MinimoJugadores;
                            categoria.MaximoJugadores = categoriaPost.MaximoJugadores;
                            categoria.CodigoUsuarioModifico = categoriaPost.CodigoUsuarioOpera;
                            categoria.UsuarioModifico = usuarioOpera.Usuario;
                            categoria.FechaModifico = fechaActual;
                            
                        }
                    }

                    //Graba y hace commit a la transacción
                    db.SaveChanges();
                    context.Commit();

                    response.data = categoria;
                    response.response = true;
                    response.mensaje = "Guardado con éxito!";
                    
                }
                catch (Exception ex)
                {
                    response.data = null;
                    response.response = false;
                    response.mensaje = ex.Message;
                }

                return Ok(response);
            }
        }

        ///<summary>
        ///Objtenemos las categorias.
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCategorias/{idLiga}")]
        public IHttpActionResult getJugadores(int idLiga)
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

                    var categorias = (from j in context.LFTCategoriaJugador
                                      where j.IdLiga == idLiga
                                     select new { Id = j.Id, Nombre = j.Nombre })
                                                    .ToList();
                    response.data = categorias;
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
    }
}
