using API.Controllers.Base;
using API.Helpers;
using API.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace API.Controllers.Desktop
{
    [AllowAnonymous]
    [RoutePrefix("api/desktop/topentrenador")]
    public class ApiDesktopTopEntrenadorController : ApiController
    {
        private dbLigaFantasiaEntities db = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));

        ///<summary>
        ///Función  descripcion
        ///</summary>
        [HttpGet]
        [Route("{idLiga}")]
        public IHttpActionResult getTopentrenadores(int idLiga)
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
            string numeroRonda = System.Web.HttpContext.Current.Request.Params["numeroRonda"];
            //int.Parse(codigoUsuario)




            try
            {
                using (var context = db)
                {

                    
                    if(!string.IsNullOrEmpty(numeroRonda))
                    {
                        int numeroRondaInt = Int32.Parse(numeroRonda);

                        var query = (from l in context.LFTLiga
                                     join rl in context.LFTRondaLiga on l.Id equals rl.IdLiga
                                     where l.Id == idLiga && rl.NumeroRonda == numeroRondaInt && rl.Estado == 3 //Ronda debe estar cerrada
                                     join er in context.LFTEquipoRonda on rl.Id equals er.IdRonda
                                     where er.Activo && !er.Eliminado
                                     join u in context.LFTUsuario on er.CodigoUsuarioEntrenador equals u.CodigoUsuario
                                     join mel in context.LFTMiembroEquipoLiga on er.Id equals mel.IdEquipo
                                     where mel.Activo == true && mel.Eliminado == false
                                     join rr in context.LFTResultadoRonda on mel.IdJugadorLiga equals rr.IdJugadorLiga
                                     where rr.IdRonda == rl.Id && mel.IdJugadorLiga == rr.IdJugadorLiga && rr.Eliminado == false
                                     join mc in context.LFTMetricaCategoria on rr.IdMetrica equals mc.Id
                                     group new { l, u, rr, rl, mc }
                                     by new
                                     {
                                         IdLiga = l.Id,
                                         NombreLiga = l.Nombre,
                                         CodigoUsuarioEntrenador = u.CodigoUsuario,
                                         NombreEntrenador = u.NombreCompletoUsuario
                                     }
                                                     into g
                                     orderby g.Sum(x => (x.rr.Resultado / 100) * (x.mc.Puntos * (x.mc.Positivo == true ? 1 : -1))) descending
                                     select new
                                     {
                                         g.Key.IdLiga,
                                         g.Key.NombreLiga,
                                         g.Key.CodigoUsuarioEntrenador,
                                         g.Key.NombreEntrenador,
                                         Resultado = g.Sum(x => (x.rr.Resultado / 100) * (x.mc.Puntos * (x.mc.Positivo == true ? 1 : -1)))
                                     }
                                    )
                                .ToList();
                        response.data = query;
                    }
                    else
                    {
                        //Todas las rondas
                        var query = (from l in context.LFTLiga
                                     join rl in context.LFTRondaLiga on l.Id equals rl.IdLiga
                                     where l.Id == idLiga && rl.Estado == 3 //Ronda debe estar cerrada
                                     join er in context.LFTEquipoRonda on rl.Id equals er.IdRonda
                                     where er.Activo && !er.Eliminado
                                     join u in context.LFTUsuario on er.CodigoUsuarioEntrenador equals u.CodigoUsuario
                                     join mel in context.LFTMiembroEquipoLiga on er.Id equals mel.IdEquipo
                                     where mel.Activo == true && mel.Eliminado == false
                                     join rr in context.LFTResultadoRonda on mel.IdJugadorLiga equals rr.IdJugadorLiga
                                     where rr.IdRonda == rl.Id && mel.IdJugadorLiga == rr.IdJugadorLiga && rr.Eliminado == false
                                     join mc in context.LFTMetricaCategoria on rr.IdMetrica equals mc.Id
                                     group new { l, u, rr, mc }
                                     by new
                                     {
                                         IdLiga = l.Id,
                                         NombreLiga = l.Nombre,
                                         CodigoUsuarioEntrenador = u.CodigoUsuario,
                                         NombreEntrenador = u.NombreCompletoUsuario
                                     }
                                                     into g
                                     orderby g.Sum(x => (x.rr.Resultado / 100) * (x.mc.Puntos * (x.mc.Positivo == true ? 1 : -1))) descending
                                     select new
                                     {
                                         g.Key.IdLiga,
                                         g.Key.NombreLiga,
                                         g.Key.CodigoUsuarioEntrenador,
                                         g.Key.NombreEntrenador,
                                         Resultado = g.Sum(x => (x.rr.Resultado / 100) * (x.mc.Puntos * (x.mc.Positivo == true ? 1 : -1)))
                                     }
                                    )
                                .ToList();

                        //query = from q in query

                        response.data = query;
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
        ///Función para obtener las métricas de una ronda por entrenador.
        ///</summary>
        /// <param name="usuario">Usuario de SAP</param>
        /// <param name="password">Password del usaurio</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getMetricasPorEntrenador/{idLiga}/{idEntrenador}/{numeroRonda}")]
        public IHttpActionResult getMetricasPorEntrenador(int idLiga, int idEntrenador, int numeroRonda)
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

                    var queryPreproceso = (from l in context.LFTLiga
                                 join rl in context.LFTRondaLiga on l.Id equals rl.IdLiga
                                 where l.Id == idLiga && rl.Estado == 3 //La ronda debe estar cerrada
                                 join er in context.LFTEquipoRonda on rl.Id equals er.IdRonda
                                 where er.CodigoUsuarioEntrenador == idEntrenador && er.Activo && !er.Eliminado
                                 join u in context.LFTUsuario on er.CodigoUsuarioEntrenador equals u.CodigoUsuario
                                 join mel in context.LFTMiembroEquipoLiga on er.Id equals mel.IdEquipo
                                 where mel.Activo == true && mel.Eliminado == false
                                 join jl in context.LFTJugadorLiga on mel.IdJugadorLiga equals jl.Id
                                 join j in context.LFTJugador on jl.IdJugador equals j.Id
                                 join ujl in context.LFTUsuario on j.CodigoUsuario equals ujl.CodigoUsuario
                                 join rr in context.LFTResultadoRonda on mel.IdJugadorLiga equals rr.IdJugadorLiga
                                 where rr.IdRonda == rl.Id && mel.IdJugadorLiga == rr.IdJugadorLiga && rr.Eliminado == false
                                 join mc in context.LFTMetricaCategoria on rr.IdMetrica equals mc.Id

                                 select new
                                 {
                                     IdLiga = l.Id,
                                     NombreLiga = l.Nombre,
                                     CodigoUsuarioEntrenador = u.CodigoUsuario,
                                     NombreEntrenador = u.NombreCompletoUsuario,
                                     IdRonda = rl.Id,
                                     NumeroRonda = rl.NumeroRonda,
                                     CodigoJugador = ujl.CodigoUsuario,
                                     NombreJugador = ujl.NombreCompletoUsuario,
                                     IdMetrica = mc.Id,
                                     NombreMetrica = mc.Nombre,
                                     Resultado = ((rr.Resultado / 100) * (mc.Puntos * (mc.Positivo == true? 1 : -1)))
                                 }
                                    );

                    if(numeroRonda != -1)
                    {
                        //Ronda específica. -1 quiere decir todas las rondas.
                        queryPreproceso = queryPreproceso.Where(w => w.NumeroRonda == numeroRonda);
                    }
                                
                    var query = queryPreproceso.ToList();

                    var metricaAgrupacion = (from data in query
                                             group data by new { data.IdMetrica, data.NombreMetrica } into g
                                             select new
                                             {
                                                 IdMetrica = g.Key.IdMetrica,
                                                 Metrica = g.Key.NombreMetrica,
                                                 Resultado = g.Sum(x => x.Resultado)
                                             }).ToList();

                    var jugadoresAgrupacion = (from data in query
                                             group data by new { data.CodigoJugador, data.NombreJugador } into g
                                             select new
                                             {
                                                 CodigoJugador = g.Key.CodigoJugador,
                                                 Jugador = g.Key.NombreJugador,
                                                 Resultado = g.Sum(x => x.Resultado)
                                             }).ToList();


                    response.data = new
                    {
                        Metricas = metricaAgrupacion,
                        Jugadores = jugadoresAgrupacion
                    };
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
