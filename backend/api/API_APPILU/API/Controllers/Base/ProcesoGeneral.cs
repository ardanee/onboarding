using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using API.Models;
using API.Helpers;

namespace API.Controllers.Base
{
    public class ProcesoGeneral
    {
        private dbAppILUEntities db = new dbAppILUEntities(ConnectionHelper.CreateConnectionString(Helpers.Configuration.getVar("SERVER_APP_ILU"), Helpers.Configuration.getVar("DB_NAME_APP_ILU"), Helpers.Configuration.getVar("DB_USER_APP_ILU"), Helpers.Configuration.getVar("DB_PASSWORD_APP_ILU")));

        //Obtiene la ficha de una persona del API de Scilu
        public string GetApiScilu(string apiName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("apiurl" + apiName);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);

                    var json_serializer = new JavaScriptSerializer();
                    //var item = (IDictionary<string, object>)json_serializer.DeserializeObject(reader.ReadToEnd());
                    //var item = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }

                return null;
                //IDictionary<string, string> response = new Dictionary<string, string>();
                //response["Response"] = "Error"; 

            }
        }

        public List<AIVPermisosPorUsuario> GetPermisos(int codigoPermisoAgrupacion, int codigoUsuario)
        {

            //Permisos por agrupacion y se envia el id de la agrupacion para obtenerlos
            using (var context = db)
            {
                var query = context.AIVPermisosPorUsuario
                    .Where(s => s.CodigoPermisoAgrupacion == codigoPermisoAgrupacion && s.CodigoUsuario == codigoUsuario)
                    .OrderBy(s => s.CodigoPermiso)
                    .ToList();

                return query;
            }


        }

        public List<AIVPermisosPorUsuario> GetPermisos(string permisoAgrupacion, int codigoUsuario)
        {

            //Permisos por agrupacion y se envia el id de la agrupacion para obtenerlos
            using (var context = db)
            {
                var query = context.AIVPermisosPorUsuario
                    .Where(s => s.PermisoAgrupacion == permisoAgrupacion && s.CodigoUsuario == codigoUsuario)
                    .OrderBy(s => s.CodigoPermiso)
                    .ToList();

                return query;
            }


        }


        public List<AIVPermisosPorUsuario> GetPermisosMultiple(int[] codigoPermisoAgrupacion, int codigoUsuario)
        {

            //Permisos por agrupacion y se envia el id de la agrupacion para obtenerlos
            using (var context = db)
            {
                var query = context.AIVPermisosPorUsuario
                    .Where(s => codigoPermisoAgrupacion.Contains(s.CodigoPermisoAgrupacion) && s.CodigoUsuario == codigoUsuario)
                    .OrderBy(s => s.CodigoPermiso)
                    .ToList();

                return query;
            }


        }

        //Obtiene la ficha de una persona del API de Scilu
        public static string fetchGetRequest(string apiName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiName);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);

                    var json_serializer = new JavaScriptSerializer();
                    //var item = (IDictionary<string, object>)json_serializer.DeserializeObject(reader.ReadToEnd());
                    //var item = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());

                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }

                return null;

            }
        }

    }
}