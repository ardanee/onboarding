using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace API.Controllers.Base
{
    public class ApiScilu
    {
        static string apiUrl = Helpers.Configuration.getVar("API_SCILU");

        //Obtiene la ficha de una persona del API de Scilu
        public static string GetApiScilu(string apiName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl + apiName);
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

                return "{}";

            }
        }
    }
}