using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace API.Helpers
{
    public static class Configuration
    {

            //<connectionStrings>
            //  <!--
            //      ****************************************************************************************************************************************************************************************************************************
            //      **********AL PUBLICAR EN PRODUCCION, ESTAS CONECCIONES NO DEBERIAN IR AQUI, TODOS LOS DATOS SENSITIVOS DEBEN ALMACENARSE EN Helpers->Configuration.cs  !!ESTAS CONEXIONES NO SE ENCUENTRAN EN EL ARCHIVO DE PRODUCCIÓN!! ***
            //      ****************************************************************************************************************************************************************************************************************************/-->
            //  <!--PRODUCCION AD-->
            //  <add name = "dbActiveDirectory" connectionString="LDAP://unisercdc01.launion.com.gt:389/DC=launion,DC=com,DC=gt" />
            //<!-- Esto dio clavos LDAP://unisercdc.launion.com.gt:389/DC=launion,DC=com,DC=gt-->
            //<!-- LDAP://unisercdcb.launion.com.gt:389/DC=launion,DC=com,DC=gt -->
            //<!--Este es el mas estable LDAP://unisercdc01.launion.com.gt:389/DC=launion,DC=com,DC=gt -->
            //<add name = "dbAppILUEntities" connectionString="metadata=res://*/Models.dbAppILUDataModel.csdl|res://*/Models.dbAppILUDataModel.ssdl|res://*/Models.dbAppILUDataModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=svrcodesa02;initial catalog=dbAppILU;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
            //<add name = "dbBiosalcMovilEntities" connectionString="metadata=res://*/Models.dbBiosalcMovilModel.csdl|res://*/Models.dbBiosalcMovilModel.ssdl|res://*/Models.dbBiosalcMovilModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=svrcodesa02;initial catalog=BiosalcMovil;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
            //<add name = "dbCSTHotelEntities" connectionString="metadata=res://*/Models.dbCSTHotel.csdl|res://*/Models.dbCSTHotel.ssdl|res://*/Models.dbCSTHotel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=svrcodesa;initial catalog=CSTHotel;persist security info=True;user id=usrConsultaAppILU;password=C0nsu1t4@pp1!U;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
            //<add name = "dbASTILU_SSUEntities" connectionString="metadata=res://*/Models.dbASTILU.csdl|res://*/Models.dbASTILU.ssdl|res://*/Models.dbASTILU.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=192.168.15.211;initial catalog=ASTILU_SSU;user id=usrit;password=UT#18;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
            //<add name = "dbCompensacionEntities" connectionString="metadata=res://*/Models.dbCompensacion.csdl|res://*/Models.dbCompensacion.ssdl|res://*/Models.dbCompensacion.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=uniserdb;initial catalog=dbCompensacion;user id=usrAppILU;password=UsR@99I1U2019;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
            //</connectionStrings>

        public static IDictionary<string, string> data = new Dictionary<string, string>
            {
                //APP SETTINGS -------------------------------------------------------
                /*Desa*/
                    { "DESA_APP_VERSION_ESTABLE_ANDROID", "0.39" },
                    { "DESA_APP_VERSION_MIN_ANDROID", "0.39" },
                    { "DESA_APP_VERSION_ESTABLE_IOS", "0.39" },
                    { "DESA_APP_VERSION_MIN_IOS", "0.39" },
                /*Prod*/
                    { "PROD_APP_VERSION_ESTABLE_ANDROID", "1.00" },
                    { "PROD_APP_VERSION_MIN_ANDROID", "0.39" },
                    { "PROD_APP_VERSION_ESTABLE_IOS", "0.39" },
                    { "PROD_APP_VERSION_MIN_IOS", "0.39" },
                
                //API SETTINGS --------------------------------------------------------    
                /*Desa*/
                    //dbLigaFantasia
                    { "DESA_DB_SERVER_LIGAFANTASIA", "svrcodesa02" },
                    { "DESA_DB_NAME_LIGAFANTASIA", "dbLigaFantasia" },
                    { "DESA_DB_USER_LIGAFANTASIA", "usrLigaFantasia" },
                    { "DESA_DB_PASSWORD_LIGAFANTASIA", "L!g@f4nt@s1@" },

                    //dbAppILU
                    { "DESA_DB_SERVER_APP_ILU", "svrcodesa02" },
                    { "DESA_DB_NAME_APP_ILU", "dbAppILU" },
                    { "DESA_DB_USER_APP_ILU", "usrAppILU" },
                    { "DESA_DB_PASSWORD_APP_ILU", "*d!unbre@@kabl33*%" },


                    //Scilu
                    //{ "DESA_API_SCILU", "https://desapisciludesarrollo.launion.com.gt/" },
                    { "DESA_API_SCILU", "https://apiscilu.launion.com.gt/" },

                    //Imagen Repository
                    { "DESA_IMAGE_REPOSITORY" , @"C:\Users\jabzum\Documents\Git\Bitbucket\ILU\rep_ti_appmovililu-desktop\API\API\API\img"},
                    //{ "DESA_URI_IMAGE_REPOSITORY" , "http://192.168.5.115:45458/img/"},
                    { "DESA_URI_IMAGE_REPOSITORY" , "http://192.168.1.7:45455/img/"},
                    { "DESA_IMAGE_REPOSITORY_AVISOS" , @"\avisos"},
                    { "DESA_URI_IMAGE_REPOSITORY_AVISOS" , "avisos/"},
                    { "DESA_IMAGE_REPOSITORY_AVISOS_TAREA", @"\tareas"},
                    { "DESA_URI_IMAGE_REPOSITORY_AVISOS_TAREA", "tareas/" },

                    //AWS Config.
                    { "DESA_AWS_REGION", "us-east-1" },
                    { "DESA_AWS_PINPOINT_APP_ID", "030fce277dc64ac3bddcc80ceeebcf50" },
                    { "DESA_AWS_ACCESS_KEY","AKIAT6ONWB4HIQVOWGRS" },
                    { "DESA_AWS_SECRET_KEY","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" },

                    //Configuraciones en general
                    { "DESA_PUBLIC_API_AUTH_TOKEN", "PVJ0qbj5w3NwqVGf1myfS6RSNE2RHuYX" }, //Este token sirve para que las apis sin authorize puedan ejecutarse por medio de una URL firmada.

                    //AD
                    { "DESA_AD_USUARIO_ILU", "auNcuvGb6uY+ud5Z0TfE/71Z4Ug6Y87VBcMJGIOz6Gw=" },
                    { "DESA_AD_PASSWORD_ILU", "fl0930llVLBH6YY8KNGRWA==" },

                    //SMTP Outgoing
                    { "DESA_SMTP_MailUser", "hl5VmSqIbsbIG1nh4cCZAw==" },
                    { "DESA_SMTP_MailPassWord", "xsfrvqWOO/OFW5ci7wLQqSPe+B38/9kodDhSDP2zsv0=" },
                    { "DESA_SMTP_MailServer", "caVBtju5C1tq6GFv5rfpa2OtZuzjNfoLMu85J8DFJ0A=" },

                    //Google
                    { "DESA_GOOGLE_MAPS_API_KEY","AIzaSyCIXmnukatFpDU6pkAxuh1O3q7s4TIxrNM" },

                /*QA*/

                /*Prod*/
                    //dbLigaFantasia
                    { "PROD_DB_SERVER_LIGAFANTASIA", "uniserdb" },
                    { "PROD_DB_NAME_LIGAFANTASIA", "dbLigaFantasia" },
                    { "PROD_DB_USER_LIGAFANTASIA", "usrLigaFantasia" },
                    { "PROD_DB_PASSWORD_LIGAFANTASIA", "U$r71g@f4ntaS!A" },

                    //dbAppILU
                    { "PROD_DB_SERVER_APP_ILU", "uniserdb" },
                    { "PROD_DB_NAME_APP_ILU", "dbAppILU" },
                    { "PROD_DB_USER_APP_ILU", "usrAppILU" },
                    { "PROD_DB_PASSWORD_APP_ILU", "*p!unbre@@kabl33*r%" },

                    //Scilu
                    { "PROD_API_SCILU", "https://apiscilu.launion.com.gt/" },

                    //Imagen Repository
                    { "PROD_IMAGE_REPOSITORY" , @"\\unisercapl\sitios$\api\img"},
                    { "PROD_URI_IMAGE_REPOSITORY" , "https://api.launion.com.gt/img/"},
                    { "PROD_IMAGE_REPOSITORY_AVISOS" , @"\avisos"},
                    { "PROD_URI_IMAGE_REPOSITORY_AVISOS" , "avisos/"},
                    { "PROD_IMAGE_REPOSITORY_AVISOS_TAREA", @"\tareas"},
                    { "PROD_URI_IMAGE_REPOSITORY_AVISOS_TAREA", "tareas/" },

                    //AWS Config.
                    { "PROD_AWS_REGION", "us-east-1" },
                    { "PROD_AWS_PINPOINT_APP_ID", "030fce277dc64ac3bddcc80ceeebcf50" },
                    { "PROD_AWS_ACCESS_KEY","AKIAT6ONWB4HIQVOWGRS" },
                    { "PROD_AWS_SECRET_KEY","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" },

                    //Configuraciones en general
                    { "PROD_PUBLIC_API_AUTH_TOKEN", "PVJ0qbj5w3NwqVGf1myfS6RSNE2RHuYX" }, //Este token sirve para que las apis sin authorize puedan ejecutarse por medio de una URL firmada.
                    //AD
                    { "PROD_AD_USUARIO_ILU", "auNcuvGb6uY+ud5Z0TfE/71Z4Ug6Y87VBcMJGIOz6Gw=" },
                    { "PROD_AD_PASSWORD_ILU", "fl0930llVLBH6YY8KNGRWA==" },

            
                    //Google
                    { "PROD_GOOGLE_MAPS_API_KEY","AIzaSyCIXmnukatFpDU6pkAxuh1O3q7s4TIxrNM" },
        };

        public static string PROD_LDAP = "LDAP://unisercdc01.launion.com.gt:389/DC=launion,DC=com,DC=gt";

        public static string getVar(string variable)
        {
            return data[WebConfigurationManager.AppSettings["Environment"].ToUpper()+"_"+variable];

        }
    }
}

