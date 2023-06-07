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
    [RoutePrefix("api/resultado")]
    public class ResultadoController : ApiController
    {
        private dbLigaFantasiaEntities db = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));

        [HttpGet]
        [Route("listarRondasLiga/{idLiga}/{codigoUsuarioAdministrador}")]
        public IHttpActionResult listarRondasLiga(int idLiga,int codigoUsuarioAdministrador)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;


            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                using (var context = db)
                {
                    //Obtenemos las cantidades de las ligas creados por el usuario recibido en el parámetro.
                    var result = (from Rondaliga in context.LFTRondaLiga
                                  join liga in context.LFTLiga on Rondaliga.IdLiga equals liga.Id
                                  join estado in context.LFTEstadoRonda on Rondaliga.Estado equals estado.Id
                                  where liga.Eliminado == false
                                  && liga.Activo == true
                                  && Rondaliga.Eliminado == false
                                  && liga.CodigoUsuarioAdministrador == codigoUsuarioAdministrador
                                  && liga.Id==idLiga
                                  select new
                                  {
                                      IdLiga = liga.Id,
                                      IdRonda= Rondaliga.Id,
                                      NombreLiga = liga.Nombre,
                                      Rondaliga.NumeroRonda,
                                      Rondaliga.Estado,
                                      NombreEstado = estado.Nombre,
                                      Rondaliga.FechaInicio,
                                      Rondaliga.FechaFin,
                                      Rondaliga.Activo,                                     
                                  }
                                  ).ToList();
                    respuestaApiJson = result;

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
                    { "data", respuestaApiJson },

                };

            return Ok(data);
        }

        [HttpGet]
        [Route("listarRondasOpen/{idLiga}")]
        public IHttpActionResult listarRondasOpen(int idLiga)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;


            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                using (var context = db)
                {
                    //Obtenemos las cantidades de las ligas creados por el usuario recibido en el parámetro.
                    var result = (from Rondaliga in context.LFTRondaLiga
                                  join liga in context.LFTLiga on Rondaliga.IdLiga equals liga.Id
                                  join estado in context.LFTEstadoRonda on Rondaliga.Estado equals estado.Id
                                  where liga.Eliminado == false
                                   && Rondaliga.Eliminado == false
                                  && liga.Id == idLiga
                                  select new
                                  {
                                      IdLiga = liga.Id,
                                      IdRonda = Rondaliga.Id,
                                      NombreLiga = liga.Nombre,
                                      Rondaliga.NumeroRonda,
                                      Rondaliga.Estado,
                                      NombreEstado = estado.Nombre,
                                      Rondaliga.FechaInicio,
                                      Rondaliga.FechaFin,
                                      Rondaliga.Activo,
                                  }
                                  ).ToList();
                    respuestaApiJson = result;

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
                    { "data", respuestaApiJson },

                };

            return Ok(data);
        }

        [HttpGet]
        [Route("listarJugadoresLiga/{idLiga}/{codigoUsuarioAdministrador}")]
        public IHttpActionResult listarJugadoresLiga(int idLiga, int codigoUsuarioAdministrador)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;


            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                using (var context = db)
                {
                    //Obtenemos las cantidades de las ligas creados por el usuario recibido en el parámetro.
                    var result = (from jugadoresLiga in context.LFTJugadorLiga
                                  join jugador in context.LFTJugador on jugadoresLiga.IdJugador equals jugador.Id
                                  join liga in context.LFTLiga on jugadoresLiga.IdLiga equals liga.Id
                                  join categoria in context.LFTCategoriaJugador on jugadoresLiga.IdCategoriaJugador equals categoria.Id
                                
                                  where liga.Eliminado == false
                                  && liga.Activo == true
                                  && jugadoresLiga.Eliminado == false
                                  && jugadoresLiga.Activo == true
                                  && jugador.Eliminado == false
                                  && jugador.Activo == true 
                                  && liga.CodigoUsuarioAdministrador == codigoUsuarioAdministrador
                                  && liga.Id == idLiga
                                  select new
                                  {
                                      IdLiga = liga.Id,
                                      IdJugadorLiga = jugadoresLiga.Id,
                                      IdJugador = jugador.Id,
                                      IdCategoria = jugadoresLiga.IdCategoriaJugador,
                                      Nombre = jugador.Nombre,
                                      jugador.SobreMi,
                                      NombreCategoria = categoria.Nombre,
                                      
                                  }
                                  ).ToList();
                    respuestaApiJson = result;

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
                    { "data", respuestaApiJson },

                };

            return Ok(data);
        }


        [HttpGet]
        [Route("listarResultado/{idRonda}/{idJugadorLiga}")]
        public IHttpActionResult listarResultado(int idRonda, int idJugadorLiga)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;


            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                using (var context = db)
                {
                    //Obtenemos las cantidades de las ligas creados por el usuario recibido en el parámetro.
                    var result = db.LFPListarResultadoJugadorRonda(idRonda, idJugadorLiga).ToList();
                    respuestaApiJson = result;

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
                    { "data", respuestaApiJson },

                };

            return Ok(data);
        }

        [HttpPost]
        [Route("grabarEditar")]
        public IHttpActionResult grabarEditar([FromBody] resultadoPost resultadoPost )
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            DateTime fechaActual = DateTime.Now;

            using (var context = db.Database.BeginTransaction())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                try
                {

                    //Valida que el resultado que se va a ingresar sea de la liga en la que el usuario sea administrador
                    var validacion = (from ronda in db.LFTRondaLiga
                                      join league in db.LFTLiga on ronda.IdLiga equals league.Id
                                      where ronda.Estado == 2 //En curso
                                      && ronda.Eliminado == false
                                      && league.Eliminado == false
                                      && league.Activo == true
                                      && league.CodigoUsuarioAdministrador == resultadoPost.CodigoUsuarioOpera
                                      select new { id = league.Id, league.Nombre }
                                      ).FirstOrDefault();
                    if (validacion == null)
                    {
                        return Unauthorized();
                    }


                    //Recorre los items de la data recibida como parámetro
                    foreach(ResultadoRondaPost resultadoRonda in resultadoPost.resultado)
                    {
                        var resultado = db.LFTResultadoRonda.Where(x => x.IdRonda == resultadoRonda.IdRonda
                                                                    && x.IdMetrica == resultadoRonda.IdMetrica
                                                                    && x.IdJugadorLiga == resultadoRonda.IdJugadorLiga).FirstOrDefault();
                        //Si no existe el resultado ronda, crea uno nuevo
                        if (resultado == null)
                        {
                            LFTResultadoRonda nuevoResultadoRonda = new LFTResultadoRonda();
                            nuevoResultadoRonda.IdJugadorLiga = resultadoRonda.IdJugadorLiga;
                            nuevoResultadoRonda.IdRonda = resultadoRonda.IdRonda;
                            nuevoResultadoRonda.IdMetrica = resultadoRonda.IdMetrica;
                            nuevoResultadoRonda.Resultado = resultadoRonda.Resultado;
                            nuevoResultadoRonda.CodigoUsuarioInserto = resultadoPost.CodigoUsuarioOpera;
                            nuevoResultadoRonda.UsuarioInserto = resultadoPost.UsuarioOpera;
                            nuevoResultadoRonda.FechaInserto = DateTime.Now;

                            db.LFTResultadoRonda.Add(nuevoResultadoRonda);
                            db.SaveChanges();

                        }else
                        {
                            //Si ya existe modificar el resultado
                            resultado.Resultado = resultadoRonda.Resultado;
                            resultado.CodigoUsuarioModifico = resultadoPost.CodigoUsuarioOpera;
                            resultado.UsuarioModifico = resultadoPost.UsuarioOpera;
                            resultado.FechaModifico = DateTime.Now;
                            db.SaveChanges();
                        }
                    }

                   

                    //Graba y hace commit a la transacción
                    db.SaveChanges();
                    context.Commit();
                   

                    respuestaApiJson = "Ok";
                    response = true;
                    mensaje = "Ok";
                }
                catch (Exception ex)
                {
                    context.Rollback();
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
        }

        [HttpPost]
        [Route("ListarTopJugadores")]
        public IHttpActionResult ListarTopJugadores([FromBody] TopJugadoresPost post)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            DateTime fechaActual = DateTime.Now;

                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                try
                {

                var result = db.LFPTopJugadoresListar(post.IdLiga, post.IdRonda, post.IdCategoria).ToList();
                    
                    respuestaApiJson = result;
                    response = true;
                    mensaje = "Ok";
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


        [HttpPost]
        [Route("ListarTopJugadoresDetalle")]
        public IHttpActionResult ListarTopJugadoresDetalle([FromBody] TopJugadoresDetallePost post)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            DateTime fechaActual = DateTime.Now;

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {

                var result = db.LFPTopDetalleResultadoJugadoresListar(post.IdJugadorLiga,post.IdRonda).ToList();

                respuestaApiJson = result;
                response = true;
                mensaje = "Ok";
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
        ///Función  que valida si la ronda esta lista para ser cerrada, esta función retorna si hay métricas aún por llenar.
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpGet]
        [Route("validarRondaACerrar/{idRondaLiga}")]
        public IHttpActionResult validarRondaACerrar(int idRondaLiga)
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


                    //var usuarioModificadorCreador = (from u in context.AITUsuario
                    //                                 where u.CodigoUsuario == codigoUsuario
                    //                                 select u)
                    //                                .FirstOrDefault();

                    var query = (from l in context.LFTLiga
                                 join rl in context.LFTRondaLiga on l.Id equals rl.IdLiga
                                 where rl.Id == idRondaLiga && rl.Activo  == true && rl.Eliminado == false
                                 join cj in context.LFTCategoriaJugador on l.Id equals cj.IdLiga
                                 where cj.Activo == true && cj.Eliminado == false
                                 join mc in context.LFTMetricaCategoria on cj.Id equals mc.IdCategoria
                                 where mc.Activo == true && mc.Eliminado == false
                                 join jl in context.LFTJugadorLiga on cj.Id equals jl.IdCategoriaJugador
                                 where jl.Activo == true && jl.Eliminado == false
                                 join rr in context.LFTResultadoRonda on mc.Id equals rr.IdMetrica into LJrr
                                 from rr in LJrr.Where(w => w.IdJugadorLiga == jl.Id && w.IdRonda == rl.Id).DefaultIfEmpty()
                                 where rr == null
                                 select new
                                 {
                                     rl.NumeroRonda,
                                     jl.IdJugador,
                                     NombreMetrica = mc.Nombre,
                                     Resultado = rr.Resultado.ToString()
                                 }
                                 ).Count();

                    response.data = query;
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

        [HttpPost]
        [Route("validarCierre")]
        public IHttpActionResult validarCierre([FromBody] CierrePost post)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            DateTime fechaActual = DateTime.Now;

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {

                var result = db.LFPValidacionCierreRondaListar( post.IdRonda,post.IdLiga).ToList();

                respuestaApiJson = result;
                response = true;
                mensaje = "Ok";
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

        [HttpPost]
        [Route("cerrarRonda")]
        public IHttpActionResult cerrarRonda([FromBody] CierrePost post)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            DateTime fechaActual = DateTime.Now;

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {

               var result = db.LFPCerrarRonda(post.IdRonda,post.CodigoUsuarioOpera);

                //Esto hay que trasladarlo al SP LFPCerrarRonda
                var duplicoEquipos = cerrarRondaDuplicarEquipos(post.IdRonda, new RondaCerrarPost() { CodigoUsuarioOpera = (int)post.CodigoUsuarioOpera });
                
                respuestaApiJson = "Ok";
                response = true;
                mensaje = "Ok";
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

        private RespuestaHttpGenerica cerrarRondaDuplicarEquipos(int idRonda, RondaCerrarPost rondaCerrarPost)
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


                    try
                    {
                        var usuarioOpera = (from u in db.LFTUsuario
                                           where u.CodigoUsuario == rondaCerrarPost.CodigoUsuarioOpera
                                           select u)
                                           .FirstOrDefault();

                        //Obtenemos los datos de la liga a partir de la ronda
                        var liga = (from r in db.LFTRondaLiga
                                    join l in db.LFTLiga on r.IdLiga equals l.Id
                                    where r.Id == idRonda
                                    select new
                                    {
                                        Ronda = r,
                                        Liga = l
                                    })
                                   .FirstOrDefault();

                        var ronda = (from r in db.LFTRondaLiga
                                     where r.Id == idRonda
                                     select r).FirstOrDefault();

                        //Obtenemos la próxima ronda..
                        var proximaRonda = (from r in db.LFTRondaLiga
                                            where r.IdLiga == liga.Liga.Id
                                            && r.NumeroRonda == liga.Ronda.NumeroRonda + 1
                                            select r
                                            ).FirstOrDefault();

                        if (proximaRonda != null)
                        {

                            var datosRondasADuplicar = (from er in db.LFTEquipoRonda
                                                        where er.IdRonda == idRonda
                                                        && er.Activo && !er.Eliminado
                                                        select new
                                                        {
                                                            IdEquipo = er.Id,
                                                            CodigoEntrenador = er.CodigoUsuarioEntrenador,
                                                            Presupuesto = er.Presupuesto,
                                                            MiembrosEquipo = er.LFTMiembroEquipoLiga.Where(w => w.Activo == true && w.Eliminado == false).ToList()

                                                        }
                                               ).ToList();

                            //Si un entrenador tiene dos
                            //var equiposNuevosDuplicados = datosRondasADuplicar.GroupBy(g => new { g.IdEquipo, g.CodigoEntrenador, g.Presupuesto})
                            //    .Select(s => new { s.Key.CodigoEntrenador, s.Key.IdEquipo, s.Key.Presupuesto }).ToList();

                            foreach (var equipoNuevo in datosRondasADuplicar)
                            {

                                //Se crea un equipo nuevo, pero se comenta, ya que cuando un entrenador se une, el proceso crea todos sus equipos.

                                //LFTEquipoRonda nuevoEquipoRow = new LFTEquipoRonda();
                                //nuevoEquipoRow.IdRonda = proximaRonda.Id;
                                //nuevoEquipoRow.CodigoUsuarioEntrenador = equipoNuevo.CodigoEntrenador;
                                //nuevoEquipoRow.Presupuesto = equipoNuevo.Presupuesto;
                                //nuevoEquipoRow.Activo = true;
                                //nuevoEquipoRow.Eliminado = false;
                                //nuevoEquipoRow.FechaInserto = fechaActual;
                                //nuevoEquipoRow.CodigoUsuarioInserto = rondaCerrarPost.CodigoUsuarioOpera;
                                //nuevoEquipoRow.UsuarioInserto = usuarioOpera.Usuario;

                                //var miembrosDeEquipo = datosRondasADuplicar.Where(w => w.IdEquipo == equipoNuevo.IdEquipo);


                                LFTEquipoRonda nuevoEquipoRow = (from er in db.LFTEquipoRonda
                                                                 where er.CodigoUsuarioEntrenador == equipoNuevo.CodigoEntrenador
                                                                 && er.IdRonda == proximaRonda.Id
                                                                 select er).FirstOrDefault();
                                nuevoEquipoRow.Presupuesto = equipoNuevo.Presupuesto;


                                foreach (var miembro in equipoNuevo.MiembrosEquipo)
                                {
                                    LFTMiembroEquipoLiga nuevoMiembroClonado = new LFTMiembroEquipoLiga();
                                    nuevoMiembroClonado.IdJugadorLiga = miembro.IdJugadorLiga;
                                    nuevoMiembroClonado.Precio = miembro.Precio;
                                    nuevoMiembroClonado.Activo = true;
                                    nuevoMiembroClonado.Eliminado = false;
                                    nuevoMiembroClonado.CodigoUsuarioInserto = rondaCerrarPost.CodigoUsuarioOpera;
                                    nuevoMiembroClonado.FechaInserto = fechaActual;
                                    nuevoMiembroClonado.UsuarioInserto = rondaCerrarPost.UsuarioOpera;

                                    nuevoEquipoRow.LFTMiembroEquipoLiga.Add(nuevoMiembroClonado);


                                }

                                //db.LFTEquipoRonda.Add(nuevoEquipoRow);

                            }



                            db.SaveChanges();


                        }

                        //Pasamos la ronda viejita de estado en curso a cerrado. Y la nueva ronda a: Plantilla editable.
                        //ronda.Estado = 3;
                        //if (proximaRonda != null)
                        //    proximaRonda.Estado = 4;

                        db.SaveChanges();
                        context.Commit();

                        response.response = true;
                        response.mensaje = "Guardado con éxito!";
                        response.data = new
                        {
                            RondaCerrada = new { IdRonda = ronda.Id, Estado = ronda.Estado },
                            RondaEnCurso = (proximaRonda != null) ? new { IdRonda = proximaRonda.Id, Estado = proximaRonda.Estado } : null
                        };
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

            return response;
        }

    }
}
