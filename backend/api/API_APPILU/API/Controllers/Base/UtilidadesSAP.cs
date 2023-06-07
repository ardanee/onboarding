using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;
using System.Configuration;

using System.Data;
using System.Xml;
using System.IO;

namespace API.Controllers.Base
{
    public class UtilidadesSAP
    {

        public static RfcDestination destinoRFC;
        public static RfcRepository repositorioRFC;



        public UtilidadesSAP()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool ObtenerConexionSAP(string usuario, string password)
        {
            bool existeConexion = false;

            try
            {
                RfcConfigParameters parametro = new RfcConfigParameters();
                parametro.Add(RfcConfigParameters.Name, Helpers.Configuration.getVar("SAPName").ToString());
                parametro.Add(RfcConfigParameters.AppServerHost, Helpers.Configuration.getVar("SAPAppServerHost").ToString());
                parametro.Add(RfcConfigParameters.LogonGroup, Helpers.Configuration.getVar("SAPLogonGroup").ToString());
                parametro.Add(RfcConfigParameters.SystemNumber, Helpers.Configuration.getVar("SAPSystemNumber").ToString());
                parametro.Add(RfcConfigParameters.User, usuario);
                parametro.Add(RfcConfigParameters.Password, password);
                parametro.Add(RfcConfigParameters.Client, Helpers.Configuration.getVar("SAPClient").ToString());
                parametro.Add(RfcConfigParameters.Language, Helpers.Configuration.getVar("SAPLanguage").ToString());
                parametro.Add(RfcConfigParameters.PoolSize, Helpers.Configuration.getVar("SAPPoolSize").ToString());
                parametro.Add(RfcConfigParameters.MaxPoolSize, Helpers.Configuration.getVar("SAPMaxPoolSize").ToString());
                parametro.Add(RfcConfigParameters.IdleTimeout, Helpers.Configuration.getVar("SAPIdleTimeout").ToString());

                destinoRFC = RfcDestinationManager.GetDestination(parametro);
                repositorioRFC = destinoRFC.Repository;
                RfcSessionManager.BeginContext(destinoRFC);

                existeConexion = true;
            }
            catch (Exception ex)
            {
                existeConexion =  false;
            }
            return existeConexion;


        }








        public static void DesbloqueoPassword(String usuario, String password, out String mensaje, out String tipo)
        {
            //oSalidaMercaderia = String.Empty;

            try
            {
                mensaje = String.Empty;
                tipo = String.Empty;

                //Instanciar BAPI
                IRfcFunction funcionRFC = repositorioRFC.CreateFunction("ZBAPI_LIBERA_USUARIO");


                funcionRFC.SetValue("USERNAME", usuario);

                if (!password.Equals(String.Empty))
                {

                    funcionRFC.SetValue("CLAVENUEVA", password);
                }


                //Invocando a la funcion
                funcionRFC.Invoke(destinoRFC);



                IRfcTable tablaRETURN = funcionRFC.GetTable("RETURN");

                if (tablaRETURN.RowCount > 0)
                {
                    //oSalidaMercaderia = funcionRFC.GetString("SALIDA_MERCANCIA");
                    tipo = tablaRETURN.GetString("TYPE");
                    mensaje = tablaRETURN.GetString("MESSAGE");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
