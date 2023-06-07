
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.Rest;
using System.Data;
using System.DirectoryServices.AccountManagement;

namespace API.Helpers
{
    public class AdAuthenticationService
    {
        public class AuthenticationResult
        {
            public AuthenticationResult(string mensajeAUsuario = null)
            {
                MensajeAUsuario = mensajeAUsuario;
            }

            public String MensajeAUsuario { get; set; }
            public Boolean EsExitoso { get; set; }
        }
        public AuthenticationResult InicioDeSesion(String username, String password)
        {
            AuthenticationResult resultado = new AuthenticationResult();
            //#if DEBUG
            //            // authenticates against your local machine - for development time

            //#else
            //            // authenticates against your Domain AD
            //            ContextType authenticationType = ContextType.Domain;
            //#endif

            ContextType authenticationType = ContextType.Domain;

            PrincipalContext principalContext = new PrincipalContext(authenticationType);
            bool isAuthenticated = false;
            resultado.EsExitoso = false;
            UserPrincipal userPrincipal = null;
            try
            {
                isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                if (isAuthenticated)
                {
                    userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
                    resultado.MensajeAUsuario = "Inicio de sesión exitoso";
                    resultado.EsExitoso = true;


                }
            }
            catch (Exception)
            {
                isAuthenticated = false;
                userPrincipal = null;
            }

            if (!isAuthenticated || userPrincipal == null)
            {

                resultado.MensajeAUsuario = "Usuario o contraseña incorrecta";


            }

            if (userPrincipal != null && userPrincipal.IsAccountLockedOut())
            {
                resultado.MensajeAUsuario = "Cuenta bloqueada";
                // here can be a security related discussion weather it is worth 
                // revealing this information

            }

            if (userPrincipal != null && userPrincipal.Enabled.HasValue && userPrincipal.Enabled.Value == false)
            {
                resultado.MensajeAUsuario = "Cuenta deshabilitada";
                // here can be a security related discussion weather it is worth 
                // revealing this information
                //return new AuthenticationResult("Cuenta deshabilitada");
            }

            return resultado;
        }


    }
}