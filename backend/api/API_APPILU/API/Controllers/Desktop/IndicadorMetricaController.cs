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
    [RoutePrefix("api/metrica")]
    public class IndicadorMetricaController : ApiController
    {
        private dbLigaFantasiaEntities db = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));

        [HttpGet]
        [Route("listarPorLiga/{idLiga}/{codigoUsuarioAdministrador}")]
        public IHttpActionResult listarPorLiga(int idLiga,int codigoUsuarioAdministrador)
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
                    var result = (from metrica in context.LFTMetricaCategoria
                                  join categoria in context.LFTCategoriaJugador on metrica.IdCategoria equals categoria.Id
                                  join liga in context.LFTLiga on categoria.IdLiga equals liga.Id
                                  where metrica.Eliminado == false
                                  && categoria.Eliminado == false
                                  && categoria.IdLiga == idLiga
                                  && liga.CodigoUsuarioAdministrador == codigoUsuarioAdministrador
                                  select new
                                  {
                                      IdMetrica = metrica.Id,
                                      IdCategoria = categoria.Id,
                                      NombreMetrica = metrica.Nombre,
                                      NombreCategoria = categoria.Nombre,
                                      metrica.Descripcion,
                                      metrica.Puntos,
                                      metrica.Positivo,
                                      metrica.Valuacion,
                                      metrica.EsPorcentaje,
                                      metrica.Activo,
                                     
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
        [Route("listarPorId/{id}")]
        public IHttpActionResult listarPorId(int id)
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
                    var result = (from metrica in context.LFTMetricaCategoria
                                  where metrica.Eliminado == false
                                 && metrica.Id == id
                                  select new
                                  {
                                      IdMetrica = metrica.Id,
                                      IdCategoria = metrica.IdCategoria,
                                      NombreMetrica = metrica.Nombre,
                                      metrica.Descripcion,
                                      metrica.Puntos,
                                      metrica.Positivo,
                                      metrica.Valuacion,
                                      metrica.EsPorcentaje,
                                      metrica.Activo,

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
        [Route("listaCategorias/{idLiga}")]
        public IHttpActionResult listaCategorias(int idLiga)
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
                    var result = (from categoria in context.LFTCategoriaJugador
                                  where categoria.Eliminado == false
                                  && categoria.IdLiga == idLiga
                                  && categoria.Activo == true
                                  select new
                                  {
                                      categoria.Id,
                                      categoria.Nombre

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
        [Route("existeLiga/{nombreMetrica}/{idCategoria}")]
        public IHttpActionResult existeLiga(string nombreMetrica, int idCategoria)
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
                    //Obtenemos las cantidades de las metricas para la misma categoria.
                    var result = (from metrica in context.LFTMetricaCategoria
                                  where metrica.Eliminado == false
                                  && metrica.Nombre == nombreMetrica
                                  && metrica.IdCategoria == idCategoria
                                  select metrica.Id
                                  ).Count();

                    if (result > 0)
                    {
                        respuestaApiJson = true;

                    }
                    else
                    {
                        respuestaApiJson = false;
                    }

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
        public IHttpActionResult grabarEditar([FromBody] MetricaPost metricaPost)
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
                    //Valida requeridos
                    if (string.IsNullOrEmpty(metricaPost.Nombre))
                    {
                        throw new Exception("El campo nombre es requerido");
                    }

                    //Valida que la categoría a la que pertenecerá esta métrica corresponda a una liga que tenga como usuario administrador el usuario que se recibe
                    var cat =( from categoria in db.LFTCategoriaJugador
                              join liga in db.LFTLiga on categoria.IdLiga equals liga.Id
                              where categoria.Id == metricaPost.IdCategoria
                              && categoria.Eliminado == false
                              && liga.Eliminado == false
                              && liga.Activo == true
                              && liga.CodigoUsuarioAdministrador == metricaPost.CodigoUsuarioOpera
                              select new { liga.Id ,liga.Nombre}).FirstOrDefault();

                    if (cat == null)
                    {
                        return Unauthorized();
                    }


                   LFTMetricaCategoria metrica = new LFTMetricaCategoria();

                    //Si  no existe, crea una nueva
                    if (metricaPost.Id == null)
                    {
                        //Si es un nuevo problema crea una nueva
                        metrica.IdCategoria = metricaPost.IdCategoria;
                        metrica.Nombre = metricaPost.Nombre;
                        metrica.Descripcion = metricaPost.Descripcion;
                        metrica.Puntos = metricaPost.puntos;
                        metrica.Positivo = metricaPost.positivo;
                        metrica.Valuacion = metricaPost.valuacion;
                        metrica.EsPorcentaje = metricaPost.esPorcentaje;
                        metrica.Activo = true;
                        metrica.Eliminado = false;
                        metrica.CodigoUsuarioInserto = metricaPost.CodigoUsuarioOpera;
                        metrica.UsuarioInserto = metricaPost.UsuarioOpera;
                        metrica.FechaInserto = fechaActual;
                        //Graba nueva  
                        db.LFTMetricaCategoria.Add(metrica);
                        db.SaveChanges();
                    }
                    else
                    {             //busca
                        metrica = db.LFTMetricaCategoria.Where(x => x.Id == metricaPost.Id).FirstOrDefault();
                        if (metrica != null)
                        {
                            metrica.IdCategoria = metricaPost.IdCategoria;
                            metrica.Nombre = metricaPost.Nombre;
                            metrica.Descripcion = metricaPost.Descripcion;
                            metrica.Puntos = metricaPost.puntos;
                            metrica.Valuacion = metricaPost.valuacion;
                            metrica.EsPorcentaje = metricaPost.esPorcentaje;
                            metrica.Positivo = metricaPost.positivo;
                            metrica.Activo = metricaPost.Activo;

                            metrica.CodigoUsuarioModifico = metricaPost.CodigoUsuarioOpera;
                            metrica.UsuarioModifico = metricaPost.UsuarioOpera;
                            metrica.FechaModifico = fechaActual;
                        }
                    }

                    //Graba y hace commit a la transacción
                    db.SaveChanges();
                    context.Commit();

                    metricaPost.Id = metrica.Id;
                    metrica = db.LFTMetricaCategoria.Where(x => x.Id == metricaPost.Id).FirstOrDefault();

                    respuestaApiJson = metrica;
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
        [Route("eliminar")]
        // DELETE: api/ApiEntidad/5
        public IHttpActionResult Delete([FromBody] DeleteObject param)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            using (var context = db.Database.BeginTransaction())
            {

                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;

                try
                {
                    LFTMetricaCategoria metrica = new LFTMetricaCategoria();
                    metrica = db.LFTMetricaCategoria.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (metrica == null)
                    {
                        throw new Exception("El código de la liga no existe");
                    }

                    metrica.Eliminado = true;
                    metrica.CodigoUsuarioModifico = param.CodigoUsuarioOpera;
                    metrica.UsuarioModifico = param.NombreUsuarioOpera;
                    metrica.FechaModifico = DateTime.Now;

                    db.SaveChanges();
                    context.Commit();

                    response = true;
                    mensaje = "OK";
                    respuestaApiJson = metrica.Id;

                }
                catch (Exception ex)
                {
                    context.Rollback();
                    respuestaApiJson = null;
                    response = false;
                    mensaje = ex.Message;
                }

            }

            IDictionary<string, dynamic> data = new Dictionary<string, dynamic>
                {

                    { "response", response },
                    { "mensaje", mensaje },
                    { "asignados", respuestaApiJson }
                };

            return Ok(data);
        }
    }
}
