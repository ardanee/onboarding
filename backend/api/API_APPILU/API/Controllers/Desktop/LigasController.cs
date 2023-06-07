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
    [RoutePrefix("api/liga")]
    public class LigasController : ApiController
    {
        private dbLigaFantasiaEntities db = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));

        [HttpGet]
        [Route("listarPorAdministrador/{codigoUsuario}/{activa}")]
        public IHttpActionResult listarPorAdministrador(int codigoUsuario, int activa)
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
                    bool boolValue = activa != 0;
                    //Obtenemos las cantidades de las ligas creados por el usuario recibido en el parámetro.
                    var result = (from liga in context.LFTLiga
                                  join usuario in context.LFTUsuario on liga.CodigoUsuarioAdministrador equals usuario.CodigoUsuario
                                  where liga.Eliminado == false
                                  && liga.CodigoUsuarioAdministrador == codigoUsuario
                                  && liga.Activo == (activa == 2 ? liga.Activo : boolValue)
                                  select new
                                  {
                                      liga.Id,
                                      liga.Nombre,
                                      usuario.NombreCompletoUsuario,
                                      liga.Publico,
                                      liga.CantidadRondas,
                                      liga.Editable,
                                      liga.EditarPlantilla,
                                      liga.PresupuestoInicial,
                                      liga.Password,
                                      liga.Activo,
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
        [Route("listarPorEntrenador/{codigoUsuario}/{activa}")]
        public IHttpActionResult listarPorEntrenador(int codigoUsuario, int activa)
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
                    bool boolValue = activa != 0;
                    //Obtenemos las cantidades de las ligas creados por el usuario recibido en el parámetro.
                    var result = (from liga in context.LFTLiga
                                  join ronda in context.LFTRondaLiga on liga.Id equals ronda.IdLiga
                                  join equipo in context.LFTEquipoRonda on ronda.Id equals equipo.IdRonda
                                  join usuario in context.LFTUsuario on liga.CodigoUsuarioAdministrador equals usuario.CodigoUsuario
                                  where liga.Eliminado == false
                                  && equipo.Eliminado == false
                                  && liga.Activo == (activa == 2 ? liga.Activo :boolValue)
                                  && equipo.Activo == true
                                  && equipo.CodigoUsuarioEntrenador == codigoUsuario
                                  select new
                                  {
                                      liga.Id,
                                      liga.Nombre,
                                      usuario.NombreCompletoUsuario,
                                      liga.Publico,
                                      liga.CantidadRondas,
                                      liga.Editable,
                                      liga.EditarPlantilla,
                                      liga.PresupuestoInicial,
                                      liga.Activo,
                                  }
                                  ).Distinct()
                                  .ToList();



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
        [Route("listarPorId/{id}/{codigoUsuarioAdministrador}")]
        public IHttpActionResult listarPorId(int id, int codigoUsuarioAdministrador)
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
                    var result = (from liga in context.LFTLiga
                                  where liga.Eliminado == false
                                  && liga.Id == id
                                  && liga.CodigoUsuarioAdministrador == codigoUsuarioAdministrador
                                  select new
                                  {
                                      liga.Id,
                                      liga.Nombre,
                                      liga.CodigoUsuarioAdministrador,
                                      liga.IdInstitucion,
                                      liga.Publico,
                                      liga.FechaInicio,
                                      liga.CantidadRecurrencia,
                                      liga.UnidadRecurrencia,
                                      liga.Activo,
                                      liga.CantidadRondas,
                                      liga.Editable,
                                      liga.EditarPlantilla,
                                      liga.PresupuestoInicial,
                                      liga.Password,
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
        [Route("usuarioEnrolado/{idLiga}/{codigoUsuario}")]
        public IHttpActionResult usuarioEnrolado(int idLiga, int codigoUsuario)
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
                    //valida si un usuario ya es entrenador en una liga
                    var result = (from liga in context.LFTLiga
                                  join ronda in context.LFTRondaLiga on liga.Id equals ronda.IdLiga
                                  join equipo in context.LFTEquipoRonda on ronda.Id equals equipo.IdRonda
                                  where liga.Eliminado == false
                                  && liga.Id == idLiga
                                  && equipo.CodigoUsuarioEntrenador == codigoUsuario
                                  select new
                                  {
                                      equipo.Id,
                                     }
                                  ).Count();

                    bool estaEnrolado = result > 0 ? true : false;


                    respuestaApiJson = estaEnrolado;

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
        [Route("listarPorIdOpen/{id}")]
        public IHttpActionResult listarPorIdOpen(int id)
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
                    var result = (from liga in context.LFTLiga
                                  where liga.Eliminado == false
                                  && liga.Id == id
                                  select new
                                  {
                                      liga.Id,
                                      liga.Nombre,
                                      liga.CodigoUsuarioAdministrador,
                                      liga.IdInstitucion,
                                      liga.Publico,
                                      liga.FechaInicio,
                                      liga.CantidadRecurrencia,
                                      liga.UnidadRecurrencia,
                                      liga.Activo,
                                      liga.CantidadRondas,
                                      liga.Editable,
                                      liga.EditarPlantilla,
                                      liga.PresupuestoInicial,
                                      liga.Password,
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

        [HttpPost]
        [Route("existeLiga")]
        public IHttpActionResult existeLiga([FromBody] BuscarLigaPost criterios)
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
                    var result = (from liga in context.LFTLiga
                                  where liga.Eliminado == false
                                  && liga.Nombre == criterios.NombreLiga
                                  && liga.IdInstitucion == criterios.IdInstitucion
                                  select liga.Id
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
        [Route("listarDisponiblesParaEntrenar")]
        public IHttpActionResult listarDisponiblesParaEntrenar(BuscarLigaPost busqueda)
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
                    if (busqueda.NombreLiga.Trim() == "&#")
                    {
                        busqueda.NombreLiga = string.Empty;
                    }
                    //Obtenemos las cantidades de las ligas creados por el usuario recibido en el parámetro.
                    var result =db.LFPLigasDisponiblesEntrenadorListar(busqueda.IdInstitucion,busqueda.CodigoUsuario,busqueda.NombreLiga,busqueda.Activa).ToList();
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
        public IHttpActionResult grabarEditar([FromBody] LigaPost ligaPost)
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
                    if (string.IsNullOrEmpty(ligaPost.Nombre))
                    {
                        throw new Exception("El campo nombre es requerido");
                    }

                    LFTLiga liga = new LFTLiga();

                    //Si la plantilla no existe, crea una nueva
                    if (ligaPost.Id == null)
                    {
                        //Si es un nuevo problema crea una nueva plantilla que corresponda al problema
                        liga.Nombre = ligaPost.Nombre;
                        liga.CodigoUsuarioAdministrador = ligaPost.CodigoUsuarioAdministrador;
                        liga.IdInstitucion = ligaPost.IdInstitucion;
                        liga.FechaInicio = null;
                        liga.CantidadRecurrencia = null;
                        liga.UnidadRecurrencia = null;
                        liga.Editable = true;
                        liga.EditarPlantilla = true;
                        liga.Activo = true;
                        liga.Eliminado = false;
                        liga.Publico = true;
                        liga.CodigoUsuarioInserto = ligaPost.CodigoUsuarioOpera;
                        liga.UsuarioInserto = ligaPost.UsuarioOpera;
                        liga.FechaInserto = fechaActual;
                        //Graba nueva liga
                        db.LFTLiga.Add(liga);
                        db.SaveChanges();
                    }
                    else
                    {   //Si liga ya existe
                        liga = db.LFTLiga.Where(x => x.Id == ligaPost.Id).FirstOrDefault();
                        if (liga != null)
                        {
                            //Valida que el usuario que intenta modificar el registro sea el administrador 
                            if (liga.CodigoUsuarioAdministrador != ligaPost.CodigoUsuarioOpera)
                            {
                                return NotFound();
                            }

                            liga.Nombre = ligaPost.Nombre;
                            liga.Activo = ligaPost.Activo;
                            liga.CodigoUsuarioModifico = ligaPost.CodigoUsuarioOpera;
                            liga.UsuarioModifico = ligaPost.UsuarioOpera;
                            liga.FechaModifico = fechaActual;

                            //Si la contraseña es diferente
                            if (ligaPost.Password != liga.Password)
                            {
                                if (string.IsNullOrEmpty(ligaPost.Password))
                                {
                                    liga.Password = string.Empty;
                                }
                                else

                                    //si la contraseña no es caracter vacío, debe encriptar
                                    liga.Password = Encriptador.Encriptar(ligaPost.Password);
                            }
                        }


                        //Si la liga aún es editable
                        if (liga.Editable == true)
                        {
                            liga.CodigoUsuarioAdministrador = ligaPost.CodigoUsuarioAdministrador;
                            liga.IdInstitucion = ligaPost.IdInstitucion;
                            liga.Publico = ligaPost.Publico;
                            liga.FechaInicio = ligaPost.FechaInicio;
                            liga.CantidadRecurrencia = ligaPost.CantidadRecurrencia;
                            liga.UnidadRecurrencia = ligaPost.UnidadRecurrencia;
                            liga.CantidadRondas = ligaPost.CantidadRondas;
                            liga.Editable = false;
                            liga.PresupuestoInicial = ligaPost.PresupuestoInicial;

                            //Graba liga
                            db.SaveChanges();

                            DateTime fechaInicio = (DateTime)liga.FechaInicio;
                            DateTime fechaFin;

                            //Graba todas las rondas
                            for (int i = 1; i <= ligaPost.CantidadRondas; i++)
                            {
                                //la primera ronda la marca como activa(2), las demás como peniente(1)
                                int estado = i == 1 ? 4 : 1; //1:pendiente, 2:en curso, 3:cerrada, 4:edicion de plantilla

                                //FechaInicio
                                if (i == 1)
                                {
                                    fechaInicio = (DateTime)liga.FechaInicio;
                                }
                                else
                                {
                                    fechaInicio = this.addTime(fechaInicio, liga.UnidadRecurrencia, (int)liga.CantidadRecurrencia);
                                }

                                fechaFin = this.addTime(fechaInicio, liga.UnidadRecurrencia, (int)liga.CantidadRecurrencia - 1);

                                LFTRondaLiga ronda = new LFTRondaLiga();
                                ronda.IdLiga = liga.Id;
                                ronda.NumeroRonda = i;
                                ronda.Estado = estado;
                                ronda.FechaInicio = fechaInicio;
                                ronda.FechaFin = fechaFin;
                                ronda.Activo = true;
                                ronda.Eliminado = false;
                                ronda.CodigoUsuarioInserto = liga.CodigoUsuarioInserto;
                                ronda.UsuarioInserto = liga.UsuarioInserto;
                                ronda.FechaInserto = DateTime.Now;

                                //Graba ronda
                                db.LFTRondaLiga.Add(ronda);
                            }

                        }
                    }

                    //Graba y hace commit a la transacción
                    db.SaveChanges();
                    context.Commit();

                    ligaPost.Id = liga.Id;
                    liga = db.LFTLiga.Where(x => x.Id == ligaPost.Id).FirstOrDefault();

                    respuestaApiJson = liga;
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
        [Route("unirseLiga")]
        public IHttpActionResult unirseLiga([FromBody] UnirseLigaPost unirseLiga)
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

                    //recorre cada ronda de la liga y verifica si el jugador ya existe como entrenador de un equipo
                    //Si no existe se crea el registro

                    var presupuestoLiga = db.LFTLiga.Where(x => x.Id == unirseLiga.IdLiga).Select(x => x.PresupuestoInicial).FirstOrDefault();

                    var rondas = db.LFTRondaLiga.Where(x => x.IdLiga == unirseLiga.IdLiga && x.Activo == true && x.Eliminado == false).ToList();

                    foreach (LFTRondaLiga ronda in rondas)
                    {
                        //Verfica si existe entrenador
                        var registros = (
                            from equipo in db.LFTEquipoRonda
                            join round in db.LFTRondaLiga on equipo.IdRonda equals round.Id
                            where equipo.IdRonda == ronda.Id
                            && equipo.CodigoUsuarioEntrenador == unirseLiga.CodigoUsuario
                            && equipo.Eliminado == false
                            select equipo.Id
                            ).Count();
                        if (registros < 1)
                        {
                            //Si no existe crea el registro
                            LFTEquipoRonda nuevoEquipo = new LFTEquipoRonda();
                            nuevoEquipo.IdRonda = ronda.Id;
                            nuevoEquipo.CodigoUsuarioEntrenador = unirseLiga.CodigoUsuario;
                            nuevoEquipo.Presupuesto = presupuestoLiga;
                            nuevoEquipo.Eliminado = false;
                            nuevoEquipo.Activo = true;
                            nuevoEquipo.CodigoUsuarioInserto = unirseLiga.CodigoUsuario;
                            nuevoEquipo.UsuarioInserto = (from usuario in db.LFTUsuario where usuario.CodigoUsuario == unirseLiga.CodigoUsuario select usuario.Usuario).FirstOrDefault();
                            nuevoEquipo.FechaInserto = DateTime.Now;

                            db.LFTEquipoRonda.Add(nuevoEquipo);
                            db.SaveChanges();
                        }
                    }


                    //Graba y hace commit a la transacción
                    db.SaveChanges();
                    context.Commit();



                    respuestaApiJson = "ok";
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
        [Route("validarPass")]
        public IHttpActionResult validarPass([FromBody] ValidarPassPost validacion)
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

                    var passEncriptado = Helpers.Encriptador.Encriptar(validacion.Pass);
                    var registro = db.LFTLiga.Where(x => x.Id == validacion.IdLiga && x.Password == passEncriptado && x.Eliminado==false).Count();
                    if (registro < 1)
                    {
                        respuestaApiJson = "fail";
                    }else
                    {
                        respuestaApiJson = "success";
                    }
                    
                                       
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
                    LFTLiga liga = new LFTLiga();
                    liga = db.LFTLiga.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (liga == null)
                    {
                        throw new Exception("El código de la liga no existe");
                    }

                    liga.Eliminado = true;
                    liga.CodigoUsuarioModifico = param.CodigoUsuarioOpera;
                    liga.UsuarioModifico = param.NombreUsuarioOpera;
                    liga.FechaModifico = DateTime.Now;

                    db.SaveChanges();
                    context.Commit();

                    response = true;
                    mensaje = "OK";
                    respuestaApiJson = liga.Id;

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


        private DateTime addTime(DateTime fecha, string unidadTiempo, int cantidad)
        {
            DateTime respuesta = fecha;
            try
            {
                switch (unidadTiempo)
                {
                    case "d":
                        {
                            respuesta = fecha.AddDays(cantidad);
                            break;
                        }
                    case "m":
                        {
                            respuesta = fecha.AddMonths(cantidad);
                            break;
                        }
                    case "y":
                        {
                            respuesta = fecha.AddYears(cantidad);
                            break;
                        }
                }
                return respuesta;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        #region Ronda (Funciones relacionadas a las rondas)

        ///<summary>
        ///Función  que cierra una ronda y duplica a los equipos de los entrenadores para la otra ronda.
        ///</summary>
        /// <returns></returns>
        [HttpPost]
        [Route("cerrarRonda/{idRonda}")]
        public IHttpActionResult cerrarRonda(int idRonda, RondaCerrarPost rondaCerrarPost)
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
                        //Obtenemos los datos de la liga a partir de la ronda
                        var liga = (from r in db.LFTRondaLiga
                                    join l in db.LFTLiga on r.IdLiga equals l.Id
                                    where r.Id == idRonda
                                    select new {
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
                                                            MiembrosEquipo = er.LFTMiembroEquipoLiga.Where(w => w.Activo ==true && w.Eliminado == false).ToList()
                                                            
                                                        }
                                               ).ToList();

                            //Si un entrenador tiene dos
                            //var equiposNuevosDuplicados = datosRondasADuplicar.GroupBy(g => new { g.IdEquipo, g.CodigoEntrenador, g.Presupuesto})
                            //    .Select(s => new { s.Key.CodigoEntrenador, s.Key.IdEquipo, s.Key.Presupuesto }).ToList();

                            foreach(var equipoNuevo in datosRondasADuplicar)
                            {
                                LFTEquipoRonda nuevoEquipoRow = new LFTEquipoRonda();
                                nuevoEquipoRow.IdRonda = proximaRonda.Id;
                                nuevoEquipoRow.CodigoUsuarioEntrenador = equipoNuevo.CodigoEntrenador;
                                nuevoEquipoRow.Presupuesto = equipoNuevo.Presupuesto;
                                nuevoEquipoRow.Activo = true;
                                nuevoEquipoRow.Eliminado = false;
                                nuevoEquipoRow.FechaInserto = fechaActual;
                                nuevoEquipoRow.CodigoUsuarioInserto = rondaCerrarPost.CodigoUsuarioOpera;
                                nuevoEquipoRow.UsuarioInserto = rondaCerrarPost.UsuarioOpera;

                                //var miembrosDeEquipo = datosRondasADuplicar.Where(w => w.IdEquipo == equipoNuevo.IdEquipo);

                            

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

                                db.LFTEquipoRonda.Add(nuevoEquipoRow);
                            
                            }

                            

                            db.SaveChanges();


                        }

                        //Pasamos la ronda viejita de estado en curso a cerrado. Y la nueva ronda a: Plantilla editable.
                        ronda.Estado = 3;
                        if (proximaRonda != null)
                            proximaRonda.Estado = 4;

                        db.SaveChanges();
                        context.Commit();

                        response.response = true;
                        response.mensaje = "Guardado con éxito!";
                        response.data = new {
                            RondaCerrada = new { IdRonda = ronda.Id, Estado= ronda.Estado },
                            RondaEnCurso = (proximaRonda != null)? new { IdRonda = proximaRonda.Id, Estado = proximaRonda.Estado }: null
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

            return Ok(response);
        }

        ///<summary>
        ///Función  que iniciar una ronda (Solo cambia de estado).
        ///</summary>
        /// <returns></returns>
        [HttpPost]
        [Route("iniciarRonda/{idRonda}")]
        public IHttpActionResult iniciarRonda(int idRonda, RondaCerrarPost rondaCerrarPost)
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

                        var ronda = (from r in db.LFTRondaLiga
                                     where r.Id == idRonda
                                     select r).FirstOrDefault();

                        //Pasamos la ronda a en curso.
                        ronda.Estado = 2;

                        db.SaveChanges();
                        context.Commit();

                        response.response = true;
                        response.mensaje = "Guardado con éxito!";
                        response.data = new List<dynamic>()
                        {
                            new {
                                IdLiga = ronda.IdLiga,
                                IdRonda = ronda.Id,
                                NombreLiga = "",
                                ronda.NumeroRonda,
                                ronda.Estado,
                                NombreEstado = "",
                                ronda.FechaInicio,
                                ronda.FechaFin,
                                ronda.Activo,
                            }
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

            return Ok(response);
        }


        #endregion

    }
}
