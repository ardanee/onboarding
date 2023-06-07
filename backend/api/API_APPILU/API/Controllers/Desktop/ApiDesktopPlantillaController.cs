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
    [RoutePrefix("api/desktop/plantilla")]
    public class ApiDesktopPlantillaController : ApiController
    {
        private dbLigaFantasiaEntities db = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));

        ///<summary>
        ///Se obtiene la información del equipo de la liga por ronda por un entrenador
        ///</summary>
        /// <param name="idLiga">Id de la liga</param>
        /// <param name="codigoUsuario">Enviamos el codigo del usuario logeado, pues es el entrenador.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{idLiga}")]
        public IHttpActionResult porLiga(int idLiga)
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
            int codigoUsuarioEntrenador;
            int.TryParse(System.Web.HttpContext.Current.Request.Params["codigoUsuario"], out codigoUsuarioEntrenador);

            string idRondaString = System.Web.HttpContext.Current.Request.Params["idRonda"];
            string numeroRondaString = System.Web.HttpContext.Current.Request.Params["numeroRonda"];

            //var valor = System.Web.HttpContext.Current.Request.QueryString["valor"];
            //int.Parse(codigoUsuario)




            try
            {
                using (var context = db)
                {


                    //Obtenemos la ronda
                    var rondaQuery = (from rl in context.LFTRondaLiga
                                      join er in context.LFTEquipoRonda on rl.Id equals er.IdRonda
                                      where er.CodigoUsuarioEntrenador == codigoUsuarioEntrenador
                                      && rl.IdLiga == idLiga
                                      && rl.Activo == true && rl.Eliminado == false && er.Activo == true && er.Eliminado == false
                                      select new PlantillaRondaQuery
                                      {
                                          IdRonda = rl.Id,
                                          IdLiga = rl.IdLiga,
                                          Presupuesto = er.Presupuesto,
                                          NumeroRonda= rl.NumeroRonda,
                                          FechaInicio = rl.FechaInicio,
                                          FechaFin = rl.FechaFin,
                                          EstadoRonda = rl.Estado
                                      }
                                 );

                    if (numeroRondaString != null)
                    {
                        int numeroRonda = int.Parse(numeroRondaString);

                        rondaQuery = rondaQuery.Where(w => w.NumeroRonda == numeroRonda);

                    }else //Obtiene la ronda activa
                    {
                        rondaQuery = rondaQuery.Where(w => w.EstadoRonda == 4); //En estado de edición de plantilla
                    }

                    rondaQuery = rondaQuery.OrderByDescending(o => o.FechaInicio);



                    PlantillaRondaQuery ronda = rondaQuery.FirstOrDefault();


                    
                    if(ronda == null)
                    {
                        //Si no hay rondas activas, obtiene la última ronda.

                        ronda = (from rl in context.LFTRondaLiga
                                 join er in context.LFTEquipoRonda on rl.Id equals er.IdRonda
                                 where er.CodigoUsuarioEntrenador == codigoUsuarioEntrenador
                                 && rl.Estado == 2 //Estado en curso
                                 && rl.IdLiga == idLiga
                                 && rl.Activo == true && rl.Eliminado == false && er.Activo == true && er.Eliminado == false
                                 orderby rl.NumeroRonda descending
                                 select new PlantillaRondaQuery
                                 {
                                     IdRonda = rl.Id,
                                     IdLiga = rl.IdLiga,
                                     Presupuesto = er.Presupuesto,
                                     NumeroRonda = rl.NumeroRonda,
                                     FechaInicio = rl.FechaInicio,
                                     FechaFin = rl.FechaFin,
                                     EstadoRonda = rl.Estado
                                 }
                                 ).FirstOrDefault();

                        if(ronda == null)
                        {
                            ronda = (from rl in context.LFTRondaLiga
                                     join er in context.LFTEquipoRonda on rl.Id equals er.IdRonda
                                     where er.CodigoUsuarioEntrenador == codigoUsuarioEntrenador
                                     && rl.IdLiga == idLiga
                                     && rl.Activo == true && rl.Eliminado == false && er.Activo == true && er.Eliminado == false
                                     orderby rl.NumeroRonda descending
                                     select new PlantillaRondaQuery
                                     {
                                         IdRonda = rl.Id,
                                         IdLiga = rl.IdLiga,
                                         Presupuesto = er.Presupuesto,
                                         NumeroRonda = rl.NumeroRonda,
                                         FechaInicio = rl.FechaInicio,
                                         FechaFin = rl.FechaFin,
                                         EstadoRonda = rl.Estado
                                     }
                                 ).FirstOrDefault();
                        }
                    }

                    //Si ronda == null quiere decir que no esta en la liga..

                    if(ronda != null)
                    {
                        //Obtenemos las categorías disponibles de la liga.

                        var categoriasQuery = (from cl in context.LFTCategoriaJugador
                                               where cl.IdLiga == idLiga
                                               && cl.Activo == true && cl.Eliminado == false
                                               select
                                               new
                                               {
                                                   cl.Id,
                                                   cl.Nombre,
                                                   cl.MinimoJugadores,
                                                   cl.MaximoJugadores
                                               }
                                               ).ToList();

                        //Obtenemos a los jugadores relacionados al equipo.
                        var jugadoresQuery = (from er in context.LFTEquipoRonda
                                              join mel in context.LFTMiembroEquipoLiga on er.Id equals mel.IdEquipo
                                              where er.IdRonda == ronda.IdRonda
                                              && er.CodigoUsuarioEntrenador == codigoUsuarioEntrenador
                                              && er.Activo == true && er.Eliminado == false
                                              && mel.Activo == true && mel.Eliminado == false
                                              select new
                                              {
                                                  Id = mel.Id,
                                                  Jugador = new { Id = mel.LFTJugadorLiga.IdJugador, Nombre = mel.LFTJugadorLiga.LFTJugador.Nombre },
                                                  CategoriaJugador = new { Id = mel.LFTJugadorLiga.IdCategoriaJugador, Nombre = mel.LFTJugadorLiga.LFTCategoriaJugador.Nombre },
                                                  Precio = mel.Precio,
                                                  PrecioActual = mel.LFTJugadorLiga.Precio
                                              }
                                              ).ToList();
                        //Equipo
                        var equipo = (from e in context.LFTEquipoRonda
                                      where e.CodigoUsuarioEntrenador == codigoUsuarioEntrenador
                                      && e.IdRonda == ronda.IdRonda
                                      select e).FirstOrDefault();


                        response.data = new
                        {
                            Ronda = ronda,
                            Categorias = categoriasQuery,
                            JugadoresActivos = jugadoresQuery,
                            Equipo = equipo
                        };
                    }else
                    {
                        response.data = null;
                    }

                    
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
        ///Obtiene a los jugadores por categoría de una liga.
        ///</summary>
        /// <param name="idLiga">id de la liga</param>
        /// <param name="idCategoria">El id de la categoria</param>
        /// <returns></returns>
        [HttpGet]
        [Route("jugadoresPorCategoria/{idLiga}/categoria/{idCategoria}")]
        public IHttpActionResult jugadoresPorCategoria(int idLiga, int idCategoria)
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

                    var jugadores = (from j in context.LFTJugadorLiga
                                     where j.Activo == true && j.Eliminado == false
                                     && j.IdLiga == idLiga
                                     && j.IdCategoriaJugador == idCategoria
                                     select new
                                     {
                                         Id = j.IdJugador,
                                         Nombre = j.LFTJugador.Nombre
                                     })
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
        ///Agregamos a un jugador al equipo de una ronda de un entrenador.
        ///</summary>
        /// <param name="idLiga">id de la liga</param>
        /// <param name="idJugador">El id del jugador</param>
        /// <returns></returns>
        [HttpPost]
        [Route("agregarJugador")]
        public IHttpActionResult agregarJugador(AgregarJugadorPlantillaPost postData)
        {
            RespuestaHttpGenerica response = new RespuestaHttpGenerica();
            response.response = false;
            response.mensaje = "Error";
            response.data = null;

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            LFTUsuario usuario = new LFTUsuario();
            //string param1 = System.Web.HttpContext.Current.Request.Form["param1"];
            DateTime fechaActual = DateTime.Now;

            //Parametro de GET

            //var valor = System.Web.HttpContext.Current.Request.QueryString["valor"];
            //int.Parse(codigoUsuario)

            try
            {

                using (var context = db.Database.BeginTransaction())
                {
                    try
                    {
                        



                        usuario = db.LFTUsuario.Where(w => w.CodigoUsuario == postData.CodigoUsuarioOpera).FirstOrDefault();

                        //Obtenemos los detalles del jugador....;
                        var jugadorLiga = (from j in db.LFTJugadorLiga
                                       where j.Activo == true && j.Eliminado == false
                                       && j.IdJugador == postData.IdJugador
                                       && j.IdLiga == postData.IdLiga
                                       select new
                                       {
                                           Id = j.Id,
                                           IdJugador = j.IdJugador,
                                           IdLiga = j.IdLiga,
                                           LFTJugador = j.LFTJugador,
                                           Precio = j.Precio,
                                           Categoria = j.LFTCategoriaJugador,
                                           LFTCategoriaJugador = j.LFTCategoriaJugador
                                       }
                                         )
                                         .FirstOrDefault();

                        if (jugadorLiga == null)
                        {
                            throw new Exception("El jugador no existe.");
                        }

                        //Validamos que el Jugador no haya sido agregado..
                        int idJugadorLigaExistente = jugadorLiga.Id;

                        var queryJugadorAgregadoAntes = (from mel in db.LFTMiembroEquipoLiga
                                                         where mel.IdEquipo == postData.IdEquipo
                                                         && mel.IdJugadorLiga == idJugadorLigaExistente
                                                         && mel.Activo == true && mel.Eliminado == false
                                                         select mel
                                                         ).Count();

                        if(queryJugadorAgregadoAntes >0)
                        {
                            throw new Exception("Ya tienes a este jugador en tu equipo.");
                        }

                        //Si el entrenador esta eligiendo a más jugadores de lo permitido, debe decirle que no puede hacer eso.

                        try
                        {
                            LFTCategoriaJugador categoriaDelJugadorNuevo = jugadorLiga.Categoria;

                            var jugadoresExistentesEnCategoria = (from mel in db.LFTMiembroEquipoLiga
                                                                  join jl in db.LFTJugadorLiga on mel.IdJugadorLiga equals jl.Id
                                                                  join c in db.LFTCategoriaJugador on jl.IdCategoriaJugador equals c.Id
                                                                  where mel.Activo == true && mel.Eliminado == false
                                                                  && c.Id == categoriaDelJugadorNuevo.Id
                                                                  && mel.IdEquipo == postData.IdEquipo
                                                                  select mel
                                                                  ).Count();

                            if(categoriaDelJugadorNuevo.MaximoJugadores <= jugadoresExistentesEnCategoria)
                            {
                                throw new Exception("Ya superaste la cantidad máxima.");
                            }

                        }catch(Exception e)
                        {
                            throw new Exception(e.Message);
                        }

                        //Validamos si el usuario se va a sobrepasar de su presupuesto.
                        var totalGastado = (from mel in db.LFTMiembroEquipoLiga
                                                where mel.IdEquipo == postData.IdEquipo
                                                && mel.Activo == true && mel.Eliminado == false
                                                select mel.Precio
                                                ).Sum();

                        var totalGastadoFinal = totalGastado + jugadorLiga.Precio;

                        var presupuesto = (from e in db.LFTEquipoRonda
                                           where e.Id == postData.IdEquipo
                                           select e.Presupuesto
                                           ).FirstOrDefault();

                        if((presupuesto - totalGastadoFinal) < 0)
                        {
                            throw new Exception("Te estas pasando de tu presupuesto, intenta con otro jugador.");
                        }

                        //Para actualizar:
                        LFTMiembroEquipoLiga objeto = new LFTMiembroEquipoLiga();

                        objeto.IdJugadorLiga = jugadorLiga.Id;
                        objeto.IdEquipo = postData.IdEquipo;
                        objeto.Precio = jugadorLiga.Precio;

                        objeto.Activo = true;
                        objeto.Eliminado = false;
                        objeto.CodigoUsuarioInserto = usuario.CodigoUsuario;
                        objeto.UsuarioInserto = usuario.Usuario;
                        objeto.FechaInserto = fechaActual;

                        //Para insertar:
                        db.LFTMiembroEquipoLiga.Add(objeto);
                        db.SaveChanges();
                        context.Commit();

                        var jugadorRespuesta = new
                        {
                            Id = objeto.Id,
                            Jugador = new { Nombre = jugadorLiga.LFTJugador.Nombre },
                            Precio = jugadorLiga.Precio,
                            PrecioActual = jugadorLiga.Precio,
                            CategoriaJugador = new { Id = jugadorLiga.LFTCategoriaJugador.Id, Nombre = jugadorLiga.LFTCategoriaJugador.Nombre }
                        };


                        response.response = true;
                        response.mensaje = "Guardado con éxito!";
                        response.data = jugadorRespuesta;
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

        ///<summary>
        ///Función  descripcion
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpPost]
        [Route("eliminarJugador")]
        public IHttpActionResult eliminarJugador(EliminarJugadorPlantillaPost postData)
        {
            RespuestaHttpGenerica response = new RespuestaHttpGenerica();
            response.response = false;
            response.mensaje = "Error";
            response.data = null;
            


            DateTime fechaActual = DateTime.Now;

            try
            {
                
                using (var context = db.Database.BeginTransaction())
                {
                    LFTUsuario usuario = db.LFTUsuario.Where(w => w.CodigoUsuario == postData.CodigoUsuarioOpera).FirstOrDefault();
                    try
                    {
                        //Para actualizar:
                        LFTMiembroEquipoLiga objeto = db.LFTMiembroEquipoLiga.Where(w => w.Id == postData.IdMiembroEquipoLiga).FirstOrDefault();
                        objeto.Activo = false;
                        objeto.Eliminado = true;
                        objeto.CodigoUsuarioModifico = postData.CodigoUsuarioOpera;
                        objeto.UsuarioModifico = usuario.Usuario;
                        objeto.FechaModifico = fechaActual;

                        //Re calculamos el presupuesto del equipo del entrenador, ya que un jugador pudo haber sido valuado o devaluado.

                        LFTEquipoRonda equipoRonda = db.LFTEquipoRonda.Where(w => w.Id == objeto.IdEquipo).FirstOrDefault();
                        LFTJugadorLiga jugadorLiga = db.LFTJugadorLiga.Where(w => w.Id == objeto.IdJugadorLiga).FirstOrDefault();

                        equipoRonda.Presupuesto = equipoRonda.Presupuesto + (jugadorLiga.Precio - objeto.Precio);



                        //Para insertar:
                        //db.AITPermisoAgrupacion.Add(objeto);
                        db.SaveChanges();
                        context.Commit();


                        response.response = true;
                        response.mensaje = "Eliminado con éxito!";
                        response.data = new { Id = objeto.Id, PresupuestoEquipo = equipoRonda.Presupuesto };
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
    }
}
