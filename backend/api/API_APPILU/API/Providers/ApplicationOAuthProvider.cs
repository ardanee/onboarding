using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using API.Models;
using API.Helpers;
using System.DirectoryServices.Protocols;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;

namespace API
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private enum AuthTypeClient
        {
            Normal = 1,
            Google = 2
        }

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials
        (OAuthGrantResourceOwnerCredentialsContext context)
        {
            var formData = await context.Request.ReadFormAsync();
            var contextAppILU = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU")));


            /*** Replace below user authentication code as per your Entity Framework Model ****/
            int CodigoUsuario = 0;
            string UserName = context.UserName;
            bool autenticado = false;
            int IdInstitucion = 0;
          
            using (var obj = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA"))))
            {

                //AITUsuario entry = obj.AITUsuario.Where
                //<AITUsuario>(record => 
                //record.Usuario == context.UserName && 
                //record.Clave == context.Password).FirstOrDefault();

                try
                {

                


                if (int.Parse(formData["auth_type"]) == (int)AuthTypeClient.Normal)
                {
                    LFTUsuario entry = obj.LFTUsuario.Where
                    <LFTUsuario>(record =>
                    record.Usuario == context.UserName && record.Activo && !record.Eliminado).FirstOrDefault();

                    if (entry != null)
                    {
                        if (entry.AutenticacionWindows)
                        {
                            autenticado = AuthenticateWindows(entry.UsuarioAD, context.Password);

                        }
                        else if (entry.Clave == Encriptador.Encriptar(context.Password)) //Hacer validación de encriptación
                        {
                            autenticado = true;
                        }
                        else
                        {
                            autenticado = false;
                        }

                        CodigoUsuario = entry.CodigoUsuario;
                        IdInstitucion = (int)entry.IdInstitucion;
                        

                    }else if (false) //ESTO YA NO SE EJECUTA..... PORQUE YA NO SE CREA USUARIO DESDE AQUÍ
                    {
                        ////Consultar a Scilu si existe el usuario y Crear usuario en BD.
                        ////string empleado = Controllers.Base.ApiScilu.GetApiScilu("Api/Empleado/EmpleadoPorUsuarioDominio/" + UserName); //SE comenta porque ahora es por DPI
                        //string empleado = Controllers.Base.ApiScilu.GetApiScilu("Api/Empleado/PorDPI/" + UserName);

                        ////empleado = "[{\"CodigoEmpleado\":10001704,\"NombreEmpleado\":\"JOSE MAURICIO ABZUM MENDEZ \",\"CodigoEmpleadoAnterior\":\"3435\",\"CodigoPuestoLaboral\":355,\"PuestoLaboral\":\"Gestor de Proyectos\",\"CodigoEmpresa\":3,\"Empresa\":\"INGENIO LA UNION, S.A.\",\"CodigoAreaEmpresa\":46,\"AreaEmpresa\":\"Desarrollo\",\"CodigoDepartamentoEmpresa\":\"No Aplica\",\"DepartamentoEmpresa\":\"No Aplica\",\"EstadoEmpleado\":\"Alta\"}]"; 
                        //if (empleado !=null && empleado != "{}")
                        //{

                            
                        //    dynamic respuestaApiJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(empleado);

                        //    if(respuestaApiJson[0].EstadoEmpleado == "Alta") { 
                        //        AITUsuario usuarioNuevo = new AITUsuario();
                        //        usuarioNuevo.NombreCompletoUsuario = respuestaApiJson[0].NombreEmpleado;
                        //        usuarioNuevo.Usuario = context.UserName.ToLower().Trim();
                        //        usuarioNuevo.AutenticacionWindows = false;
                        //        usuarioNuevo.Activo = true;
                        //        usuarioNuevo.Eliminado = false;
                        //        usuarioNuevo.UsuarioInserto = "APPILU";
                        //        usuarioNuevo.FechaInserto = DateTime.Now;
                        //        usuarioNuevo.NecesitaCambioDeClave = false;

                        //        if(respuestaApiJson[0].CodigoEmpresa == 3)
                        //        {
                        //            int codigoEmpleadoScilu = (int) respuestaApiJson[0].CodigoEmpleado;
                        //            ActiveDirectory ad = (from a in obj.ActiveDirectory
                        //                                  where a.DPI == UserName
                        //                                  select a)
                        //                                  .FirstOrDefault();
                        //            if(ad != null)
                        //            {
                        //                usuarioNuevo.CorreoElectronico = ad.Correo;
                        //                usuarioNuevo.UsuarioAD = ad.Usuario.ToLower();
                        //                usuarioNuevo.AutenticacionWindows = true;
                        //            }
                        //            else
                        //            {
                        //                usuarioNuevo.CorreoElectronico = "";
                        //            }
                                    
                        //        }else
                        //        {
                        //            usuarioNuevo.CorreoElectronico = "";
                        //        }

                        //        try
                        //        {
                        //            //TODO heredar de appilu
                        //            //IDictionary<string, dynamic> usuarioCreacion = Controllers.Desktop.ApiDesktopUsuarioController.crearUsuarioBD(usuarioNuevo);
                        //            //obj.AITUsuario.Add(usuarioNuevo);
                        //            //obj.SaveChanges();
                        //            //CodigoUsuario = usuarioCreacion["usuario"].id;                                    

                        //            autenticado = AuthenticateWindows(context.UserName, context.Password);
                        //        }
                        //        catch(Exception e)
                        //        {
                        //            string mensaje = e.Message;
                        //        }
                                
                        //    }
                            

                            
                        //}

                    }


                }
                else if (int.Parse(formData["auth_type"]) == (int)AuthTypeClient.Google && false) //ESTO NO SE VA A UTILIZAR AÚN, DESCOMENTAR SI SE QUIERE REDES SOCIALES.
                {
                    //AITUsuarioRedSocial entry = obj.AITUsuarioRedSocial.Where
                    //<AITUsuarioRedSocial>(record =>
                    //record.CorreoElectronico == context.UserName && record.Activo && !record.Eliminado).FirstOrDefault();

                    //if (entry != null)
                    //{
                    //    autenticado = true;
                    //    UserName = entry.AITUsuario.Usuario;
                    //    CodigoUsuario = entry.CodigoUsuario;
                    //}
                    //else
                    //{
                    //    autenticado = false;
                    //}
                }


                if (!autenticado)
                {
                    context.SetError("invalid_grant",
                    "The user name or password is incorrect.");
                    return;
                }

                }
                catch (Exception e)
                {
                    string message = e.Message;
                }


            }
            

            ClaimsIdentity oAuthIdentity =
            new ClaimsIdentity(context.Options.AuthenticationType);
            ClaimsIdentity cookiesIdentity =
            new ClaimsIdentity(context.Options.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(UserName, CodigoUsuario,IdInstitucion);
            AuthenticationTicket ticket =
            new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string,
            string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication
        (OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri
        (OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, int codigoUsuario,int idInstitucion)
        {
            //Obtenemos los datos del Api de Scilu del empleado.
            string empleado = Controllers.Base.ApiScilu.GetApiScilu("Api/Empleado/PorDPI/" + userName);
            ActiveDirectory ad = new ActiveDirectory();
            string usuarioAD = "";

            if (empleado != null && empleado != "{}" && empleado != "[]")
            {
                //using (var context = new dbAppILUEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU"))))
                using (var context = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA"))))
                {
                    dynamic empleadoArray = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(empleado);

                    int codigoEmpleadoScilu = (int) empleadoArray[0].CodigoEmpleado;

                    try
                    {
                        //usuarioAD = (string)empleadoArray[0].UsuarioDominio;

                        usuarioAD = (from u in context.LFTUsuario
                                    where u.CodigoUsuario == codigoUsuario
                                    select u.UsuarioAD).FirstOrDefault();

                        if(usuarioAD == null)
                        {
                            usuarioAD = "";
                        }

                    }
                    catch(Exception e)
                    {
                        using (var contextDbAppILU = new dbAppILUEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU"))))
                        {
                            ad = (from a in contextDbAppILU.ActiveDirectory
                                  where a.DPI == userName
                                  select a)
                              .FirstOrDefault();

                            if (ad.Usuario != null)
                            {
                                usuarioAD = ad.Usuario;
                            }
                            else
                            {
                                usuarioAD = "";
                            }
                        }

                    }

                    
                }
            }


            //Datos de la institución a la que pertenece
            string NombreInstitucion = string.Empty;
            string UrlLogoInstitucion = string.Empty;
            using (var obj = new dbLigaFantasiaEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("DB_SERVER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_NAME_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_USER_LIGAFANTASIA"), Helpers.Configuration.getVar("DB_PASSWORD_LIGAFANTASIA"))))
            {
                var registroInstitucion = (from institucion in obj.LFTInstitucion
                                           where institucion.Eliminado == false && institucion.Activo == true && institucion.Id == idInstitucion
                                           select new { institucion.Nombre, institucion.LogoUrl }).FirstOrDefault();
                if (registroInstitucion != null)
                {
                    NombreInstitucion = registroInstitucion.Nombre;
                    UrlLogoInstitucion = string.IsNullOrEmpty(registroInstitucion.LogoUrl)? "_": registroInstitucion.LogoUrl;
                }

            }

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "codigoUsuario", codigoUsuario.ToString() },
                { "empleado", empleado },
                { "usuarioAD", usuarioAD },
                { "IdInstitucion", idInstitucion.ToString() },
                { "NombreInstitucion", NombreInstitucion },
                { "UrlLogoInstitucion", UrlLogoInstitucion }
            };
            return new AuthenticationProperties(data);
        }

        public bool AuthenticateWindows(string username, string password)
        {
            bool validation;
            try
            {
                //string ldapEndpoint = ConfigurationManager.ConnectionStrings["dbActiveDirectory"].ConnectionString;  //Desde el Web.config
                //string ldapEndpoint = Helpers.Configuration.PROD_LDAP;  //Desde el Helper de Connection

                //LdapConnection lcon = new LdapConnection
                //        (new LdapDirectoryIdentifier((string)null, false, false));
                //NetworkCredential nc = new NetworkCredential(username,
                //                       password, ldapEndpoint);
                //lcon.Credential = nc;
                //lcon.AuthType = AuthType.Negotiate;
                //// user has authenticated at this point,
                //// as the credentials were used to login to the dc.
                //lcon.Bind(nc);
                //validation = true;

                AdAuthenticationService autenticador = new AdAuthenticationService();
                AdAuthenticationService.AuthenticationResult resultado = autenticador.InicioDeSesion(username, password);

                validation = resultado.EsExitoso;
            }
            catch (LdapException)
            {
                validation = false;
            }
            return validation;

        }
    }
}