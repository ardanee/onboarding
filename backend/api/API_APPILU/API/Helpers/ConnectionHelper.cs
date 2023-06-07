using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace API.Helpers
{
    public static class ConnectionHelper
    {
        

        //public static string CreateConnectionString(string initialCatalog, string UserID, string Password)
        public static string CreateConnectionString(string nombreServidor, string initialCatalog, string UserID, string Password)
        {
            const string appName = "EntityFramework";
            const string providerName = "System.Data.SqlClient";
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = nombreServidor;
            sqlBuilder.InitialCatalog = initialCatalog;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.UserID = UserID;
            sqlBuilder.Password = Password;
            sqlBuilder.ApplicationName = appName;

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
            //efBuilder.Metadata = @"res://*/Models.ModeloMCC.csdl|res://*/Models.ModeloMCC.ssdl|res://*/Models.ModeloMCC.msl";

            string metadata = "";

            if(initialCatalog == Configuration.getVar("DB_NAME_LIGAFANTASIA"))
            {
                metadata = @"res://*/Models.dbLigaFantasia.csdl|res://*/Models.dbLigaFantasia.ssdl|res://*/Models.dbLigaFantasia.msl";
            }
            else{
                metadata = @"res://*/Models.dbAppILUDataModel.csdl|res://*/Models.dbAppILUDataModel.ssdl|res://*/Models.dbAppILUDataModel.msl";
            }

            efBuilder.Metadata = metadata;
            efBuilder.Provider = providerName;
            efBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;

            return efBuilder.ConnectionString;
        }
    }
}