using API.Controllers.Base;
using API.Controllers.ModeloPosts;
using API.Helpers;
using API.Models;
using API_MonitoreoRTK.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/cuenta")]
    public class CuentaController : ApiController
    {
        private dbLigaFantasiaEntities db = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));
        private dbAppILUEntities dbAppILU = new dbAppILUEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU")));
                

        [HttpPost]
        [Route("crear")]
        public IHttpActionResult crearCuenta(CrearCuentaObject postObject)
        {
            bool response = false;
            string mensaje = "Error";
            dynamic respuestaApiJson = null;

            DateTime fechaActual = DateTime.Now;

            try
            {
                //Obtenemos los datos del Api de Scilu del usuario.
                string respuestaApiString = ApiScilu.GetApiScilu("Api/Empleado/PorDPI/" + postObject.DPI);

                if (respuestaApiString != null && respuestaApiString != "{}" && respuestaApiString != "[]")
                {
                    //Existe en Scilu
                    var jsonApiScilu = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(respuestaApiString);

                    var empleado = jsonApiScilu[0];

                    using (var context = db.Database.BeginTransaction())
                    {

                        string empleadoUsuario = (string)empleado.UsuarioDominio;
                        try
                        {
                            //Verificamos si el usuario ya existe en la BD..
                            var queryUsuario = (from u in db.LFTUsuario
                                                where u.Activo && !u.Eliminado
                                                && (u.Usuario == postObject.DPI)
                                                select u).FirstOrDefault();

                            if (queryUsuario != null)
                            {
                                //El usuario ya existe.
                                response = false;
                                mensaje = "Parece que el usuario ya existe, intenta iniciar sesión con tu DPI.";
                            }
                            else
                            {
                                //Verificar si el usuario existe en AD.


                                ManejoActiveDirectory manejoActiveDirectory = new ManejoActiveDirectory();

                                //UserPrincipal usuario = manejoActiveDirectory.ObtenerUsuarioAD(empleadoUsuario);

                                //Verificamos si el ID de scilu existe en la tabla de AD.
                                int codigoEmpleadoScilu = (int)empleado.CodigoEmpleado;
                                var usuarioAD = (from ad in dbAppILU.ActiveDirectory
                                                 where ad.DPI == postObject.DPI
                                                 select ad
                                                ).FirstOrDefault();

                                //try
                                //{

                                //}catch(Exception e)
                                //{
                                //    mensaje = "Su usuario es su DPI y su contraseña es la misma con la que ingresa a windows.";
                                //    response = true;
                                //}




                                if (usuarioAD == null)
                                {
                                    //El usuario no existe en AD.
                                    string fechaNacimiento = (string)empleado.FechaNacimiento;
                                    string nombreCompletoUsuario = (string)empleado.NombreEmpleado;

                                    //DateTime d2 = DateTime.Parse(fechaNacimiento, null, System.Globalization.DateTimeStyles.RoundtripKind);
                                    //DateTime d2 = DateTime.ParseExact(fechaNacimiento, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    DateTime d2 = DateTime.ParseExact(fechaNacimiento, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                                    if (d2.Year == postObject.FechaNacimiento.Value.Year && d2.Month == postObject.FechaNacimiento.Value.Month && d2.Day == postObject.FechaNacimiento.Value.Day)
                                    {
                                        //Crear usuario...
                                        string clave = postObject.FechaNacimiento.Value.Year + "" + ((postObject.FechaNacimiento.Value.Month < 10) ? "0" : "") + postObject.FechaNacimiento.Value.Month + "" + ((postObject.FechaNacimiento.Value.Day < 10) ? "0" : "") + postObject.FechaNacimiento.Value.Day;

                                        LFTUsuario nuevoUsuario = new LFTUsuario();
                                        nuevoUsuario.Activo = true;
                                        nuevoUsuario.Eliminado = false;
                                        nuevoUsuario.Usuario = postObject.DPI;
                                        nuevoUsuario.Clave = Encriptador.Encriptar(clave);
                                        nuevoUsuario.UsuarioInserto = "APPLIGAFANTASIA";
                                        nuevoUsuario.FechaInserto = DateTime.Now;
                                        nuevoUsuario.NombreCompletoUsuario = (nombreCompletoUsuario == null) ? "" : nombreCompletoUsuario;
                                        nuevoUsuario.CorreoElectronico = "";
                                        nuevoUsuario.AutenticacionWindows = false;
                                        nuevoUsuario.IdInstitucion = 1; //Ingenio la union

                                        var respuestaCreacionUsuario = crearUsuarioBD(nuevoUsuario);

                                        if (respuestaCreacionUsuario["response"])
                                        {
                                            dynamic usuarioRes = respuestaCreacionUsuario["usuario"];
                                            LFTJugador nuevoJugador = new LFTJugador();
                                            nuevoJugador.IdInstitucion = 1; //Ingenio la union
                                            nuevoJugador.Nombre = (nombreCompletoUsuario == null) ? "" : nombreCompletoUsuario;
                                            nuevoJugador.CodigoUsuario = (int)usuarioRes.id;
                                            nuevoJugador.Activo = true;
                                            nuevoJugador.Eliminado = false;
                                            nuevoJugador.FechaInserto = DateTime.Now;

                                            db.LFTJugador.Add(nuevoJugador);
                                            db.SaveChanges();
                                            context.Commit();
                                        }

                                        //db.AITUsuario.Add(nuevoUsuario);
                                        //db.SaveChanges();
                                        //context.Commit();

                                        mensaje = "Tu usuario es tu DPI y contraseña es: " + clave;
                                        response = true;
                                    }
                                    else
                                    {
                                        mensaje = "Sus datos no coinciden, por favor intenta de nuevo.";
                                        response = false;
                                    }


                                }
                                else
                                {
                                    //El usuario existe en AD, por lo tanto se usa su clave como acceso..

                                    //string destinatario = (string)postObject.CorreoElectronico; //El destinatario lo debemos obtener del de AD. TODO

                                    string destinatario = (from ad in dbAppILU.ActiveDirectory
                                                           where ad.DPI == postObject.DPI
                                                           select ad.Correo).FirstOrDefault();

                                    try
                                    {

                                        try
                                        {


                                            var queryPlantilla = (from p in db.LFTPlantillaCorreo
                                                                  where p.Activo && !p.Eliminado
                                                                  && p.CodigoPlantillaCorreo == 1
                                                                  select p).FirstOrDefault();

                                            if (queryPlantilla != null)
                                            {
                                                string remitente = queryPlantilla.Remitente;

                                                if (destinatario != null & destinatario != "")
                                                {


                                                    using (HostingEnvironment.Impersonate())
                                                    {
                                                        if (!string.IsNullOrEmpty(destinatario))
                                                        {
                                                            string _mensajes = queryPlantilla.Plantilla;
                                                            _mensajes = _mensajes.Replace("{{{usuario}}}", postObject.DPI);
                                                            _mensajes = _mensajes.Replace("{{{password}}}", "Su contraseña es la misma de Windows.");

                                                            string asunto = queryPlantilla.Asunto;

                                                            bool respuestaSMTP = false; // EnvioCorreo.EnviarCorreoHtml(destinatario, remitente, _mensajes, asunto);

                                                            if (respuestaSMTP)
                                                            {
                                                                mensaje = "Verifica en tu buzón los datos de acceso para ingresar.";
                                                                response = true;

                                                            }
                                                            else
                                                            {
                                                                mensaje = "Su usuario es su DPI y su contraseña es la misma de windows.";
                                                                response = true;

                                                            }

                                                        }

                                                    }


                                                }
                                                else
                                                {
                                                    mensaje = "Su usuario es su DPI y su contraseña es la misma de windows.";
                                                    response = true;
                                                }
                                            }
                                            else
                                            {
                                                mensaje = "Su usuario es su DPI y su contraseña es la misma de windows.";
                                                response = true;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            mensaje = "Su usuario es su DPI y su contraseña es la misma con la que ingresa a windows.";
                                            response = false;
                                        }
                                        string nombreCompletoUsuario = (string)empleado.NombreEmpleado;

                                        LFTUsuario nuevoUsuario = new LFTUsuario();
                                        nuevoUsuario.Activo = true;
                                        nuevoUsuario.Eliminado = false;
                                        nuevoUsuario.Usuario = postObject.DPI;
                                        nuevoUsuario.Clave = Encriptador.Encriptar("");
                                        nuevoUsuario.UsuarioInserto = "APPLIGAFANTASIA";
                                        nuevoUsuario.FechaInserto = DateTime.Now;
                                        nuevoUsuario.NombreCompletoUsuario = (nombreCompletoUsuario == null) ? "" : nombreCompletoUsuario;
                                        nuevoUsuario.CorreoElectronico = usuarioAD.Correo; //Poner el correo del usuario del AD...
                                        nuevoUsuario.AutenticacionWindows = true;
                                        nuevoUsuario.UsuarioAD = usuarioAD.Usuario;
                                        nuevoUsuario.IdInstitucion = 1; //Ingenio la union

                                        dynamic respuestaCreacionUsuario = crearUsuarioBD(nuevoUsuario);

                                        if (respuestaCreacionUsuario["response"])
                                        {
                                            dynamic usuarioRes = respuestaCreacionUsuario["usuario"];
                                            LFTJugador nuevoJugador = new LFTJugador();
                                            nuevoJugador.IdInstitucion = 1; //Ingenio la union
                                            nuevoJugador.Nombre = (nombreCompletoUsuario == null) ? "" : nombreCompletoUsuario;
                                            nuevoJugador.CodigoUsuario = (int)usuarioRes.id;
                                            nuevoJugador.Activo = true;
                                            nuevoJugador.Eliminado = false;
                                            nuevoJugador.FechaInserto = DateTime.Now;

                                            db.LFTJugador.Add(nuevoJugador);
                                            db.SaveChanges();
                                            context.Commit();
                                            response = true;
                                        }else
                                        {
                                            response = false;
                                            mensaje = "No se pudieron enviar tus credenciales, contacta a tu administrador.";
                                        }

                                        //mensaje = "Verifica en tu buzón o teléfono los datos de acceso para ingresar.";
                                    }
                                    catch (Exception e)
                                    {
                                        mensaje = "No se pudieron enviar tus credenciales, contacta a tu administrador.";
                                        response = false;
                                    }
                                }
                            }


                            //response = true;
                            //mensaje = "Guardado con éxito!";
                            //respuestaApiJson = new { id = 19339339393 };
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
                else
                {
                    //No existe, no crear usuario. No es empleado.
                    response = false;
                    mensaje = "Su identidad no fue encontrada, comuniquese con su administrador o a la extensión 1515 para habilitar su usuario.";
                    respuestaApiJson = new int[0];
                }


            }
            catch (Exception ex)
            {
                respuestaApiJson = null;
                response = false;
                mensaje = "Estamos presentando inconvenientes. Contacta a TI a la extensión 1515 para resolver tu problema.";
            }

            IDictionary<string, dynamic> data = new Dictionary<string, dynamic>
                {

                    { "response", response },
                    { "mensaje", mensaje },
                    { "data", respuestaApiJson }
                };

            return Ok(data);
        }


        public static IDictionary<string, dynamic> crearUsuarioBD(LFTUsuario usuario)
        {
            IDictionary<string, dynamic> response = new Dictionary<string, dynamic>();
           // dbAppILUEntities db = new dbAppILUEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU")));
             dbLigaFantasiaEntities dbx = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA")));

            try
            {
                DateTime fechaActual = DateTime.Now;

                if(string.IsNullOrEmpty(usuario.CorreoElectronico))
                {
                    usuario.CorreoElectronico = "";
                }

                dbx.LFTUsuario.Add(usuario);
                dbx.SaveChanges();

                //Asignandole el rol al nuevo usuario
                LFTRolUsuario rolUsuarioNuevo = new LFTRolUsuario();
                rolUsuarioNuevo.Activo = true;
                rolUsuarioNuevo.Eliminado = false;
                rolUsuarioNuevo.UsuarioInserto = "APPLIGAFANTASIA";
                rolUsuarioNuevo.FechaInserto = fechaActual;
                rolUsuarioNuevo.CodigoUsuario = usuario.CodigoUsuario;
                rolUsuarioNuevo.CodigoRol = 2; //Rol default
                

                dbx.LFTRolUsuario.Add(rolUsuarioNuevo);
                dbx.SaveChanges();

                ////Asignar usuario red social. (De momento solo de google connect)
                //if (usuario.CorreoElectronico != null && usuario.CorreoElectronico != "")
                //{
                //    AITUsuarioRedSocial redSocialUsuario = new AITUsuarioRedSocial();
                //    redSocialUsuario.CodigoUsuario = usuario.CodigoUsuario;
                //    redSocialUsuario.CodigoRedSocial = 1; //Google 
                //    redSocialUsuario.Token = "";
                //    redSocialUsuario.CorreoElectronico = usuario.CorreoElectronico;
                //    redSocialUsuario.Activo = true;
                //    redSocialUsuario.Eliminado = false;
                //    redSocialUsuario.UsuarioInserto = "APPILU";
                //    redSocialUsuario.FechaInserto = fechaActual;

                //    dbx.TUsuarioRedSocial.Add(redSocialUsuario);
                //    dbx.SaveChanges();
                //}

                ////Agregamos a todos los grupos de notificaciones al usuario (Notificaciones de área de comunicaciones)
                //List<AITGrupoUsuario> usuarioGrupos = new List<AITGrupoUsuario>();
                //var queryGrupos = (from g in db.AITGrupo
                //                   where g.Activo && !g.Eliminado
                //                   && (g.CodigoGrupoTipo == null || g.CodigoGrupoTipo == 1)
                //                   select g.CodigoGrupo).ToList();

                //foreach (int codigoGrupo in queryGrupos)
                //{
                //    AITGrupoUsuario grupoUsuario = new AITGrupoUsuario();
                //    grupoUsuario.CodigoGrupo = codigoGrupo;
                //    grupoUsuario.CodigoUsuario = usuario.CodigoUsuario;
                //    grupoUsuario.Activo = true;
                //    grupoUsuario.Eliminado = false;
                //    grupoUsuario.UsuarioInserto = usuario.UsuarioInserto;
                //    grupoUsuario.FechaInserto = fechaActual;

                //    usuarioGrupos.Add(grupoUsuario);

                //}

                //db.AITGrupoUsuario.AddRange(usuarioGrupos);
                //dbx.SaveChanges();

                response.Add("response", true);
                response.Add("mensaje", "Ok");
                response.Add("usuario", new { id = usuario.CodigoUsuario });
                //response = true;
                //mensaje = "OK";
                //respuestaApiJson = new { id = usuario.CodigoUsuario };
            }
            catch (Exception ex)
            {
                response.Add("response", false);
                response.Add("mensaje", ex.Message);
                response.Add("usuario", new int[0]);
                //mensaje = ex.Message;
            }
            return response;

        }
    }
}
