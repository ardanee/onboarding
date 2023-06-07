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
    [RoutePrefix("api/desktop/jugadorliga")]
    public class ApiDesktopJugadorLigaController : ApiController
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

                    var jugadores = (from jl in context.LFTJugadorLiga
                                      where jl.IdLiga == idLiga
                                      && jl.Activo == true && jl.Eliminado == false
                                      select new
                                      {
                                          Jugador = jl.LFTJugador,
                                          CategoriaJugador = jl.LFTCategoriaJugador,
                                          jl.Id,
                                          jl.Precio
                                          
                                      }
                                                     )
                                                    .ToList();
                    response.data = jugadores;
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

        ///<summary>
        ///Objtenemos los usuarios disponibles de una liga.
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getJugadores/{idLiga}")]
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

                    var jugadores = (from j in context.LFTJugador
                                                     select new { Id = j.Id, Nombre=j.Nombre } )
                                                    .ToList();
                    response.data = jugadores;
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
        ///Guardamos a un jugador en la liga.
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpPost]
        [Route("guardar")]
        public IHttpActionResult grabarEditar([FromBody] JugadorLigaGuardar jugadorPost)
        {
            RespuestaHttpGenerica response = new RespuestaHttpGenerica();
            response.response = false;
            response.mensaje = "Error";
            response.data = null;
            LFTJugadorLiga jugador = new LFTJugadorLiga();

            DateTime fechaActual = DateTime.Now;

            using (var context = db.Database.BeginTransaction())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                try
                {
                    //Valida requeridos
                    if (jugadorPost.Precio <=0)
                    {
                        throw new Exception("El jugador debe tener un precio mayor a cero.");
                    }

                    if (jugadorPost.IdJugador == null)
                    {
                        throw new Exception("");
                    }

                    if (jugadorPost.IdCategoriaJugador == null)
                    {
                        throw new Exception("El jugador debe tener un precio mayor a cero.");
                    }

                    var usuarioOpera = db.LFTUsuario.Where(w => w.CodigoUsuario == jugadorPost.CodigoUsuarioOpera).FirstOrDefault();


                    int idJugador = (int)jugadorPost.IdJugador;
                    int idCategoriaJugador = (int)jugadorPost.IdCategoriaJugador;

                    //Si la plantilla no existe, crea una nueva
                    if (jugadorPost.Id == null)
                    {
                        //Verificamos si el jugador ya existe en la liga, de lo contrario si lo insertamos
                        var jugadorExiste = (from jl in db.LFTJugadorLiga
                                             where jl.IdLiga == jugadorPost.IdLiga
                                             && jl.IdJugador == jugadorPost.IdJugador
                                             && jl.Activo == true && jl.Eliminado == false
                                             select jl
                                             ).FirstOrDefault();

                        if(jugadorExiste != null)
                        {
                            throw new Exception("El jugador ya existe en esta liga.");
                        }

                        //Nuevo registro
                        jugador.Precio = jugadorPost.Precio;
                        jugador.PrecioInicial = jugadorPost.Precio;

                        jugador.IdJugador = idJugador;
                        jugador.IdCategoriaJugador = idCategoriaJugador;
                        jugador.IdLiga = jugadorPost.IdLiga;
                        
                        jugador.Activo = true;
                        jugador.Eliminado = false;
                        jugador.CodigoUsuarioInserto = jugadorPost.CodigoUsuarioOpera;
                        jugador.UsuarioInserto = usuarioOpera.Usuario;
                        jugador.FechaInserto = fechaActual;

                        //Graba nueva liga
                        db.LFTJugadorLiga.Add(jugador);

                    }
                    else
                    {
                        //Registro editar
                        jugador = db.LFTJugadorLiga.Where(x => x.Id == jugadorPost.Id).FirstOrDefault();
                        if (jugador!= null)
                        {
                            jugador.IdCategoriaJugador = idCategoriaJugador;
                            if(jugadorPost.Activo != null){
                                jugador.Activo = jugadorPost.Activo;
                            }

                            if (jugadorPost.Eliminado != null)
                            {
                                jugador.Eliminado = jugadorPost.Eliminado;
                            }

                            jugador.Precio = jugadorPost.Precio;
                            jugador.PrecioInicial = jugadorPost.Precio;

                            jugador.UsuarioModifico = usuarioOpera.Usuario;
                            jugador.FechaModifico = fechaActual;

                        }
                    }

                    

                    //Graba y hace commit a la transacción
                    db.SaveChanges();
                    context.Commit();

                    var jugadorAlmacenado = (from jl in db.LFTJugadorLiga
                                                 where jl.Id == jugador.Id
                                                 select new
                                                 {
                                                     Jugador = jl.LFTJugador,
                                                     CategoriaJugador = jl.LFTCategoriaJugador,
                                                     jl.Id,
                                                     jl.Precio

                                                 }
                                                )
                                            .FirstOrDefault();

                    response.data = jugadorAlmacenado;
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
    }
}
