using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.Data;
using System.Web.Configuration;
using System.DirectoryServices;
using System.Diagnostics;
using System.Collections.Generic;
using API.Helpers;

namespace API.Models
{
    public class ManejoActiveDirectory
    {
        #region Variables

        private string DominioCEDILU = "cedilu.edu.gt";
        private string DefaultOuCEDILU = "DC=cedilu,DC=edu,DC=gt";
        private string DefaultRootOuCEDILU = "DC=cedilu,DC=edu,DC=gt";
        private string UsuarioCEDILU = Encriptador.Desencriptar(Configuration.getVar("AD_USUARIO_ILU"));
        private string PasswordCEDILU = Encriptador.Desencriptar(Configuration.getVar("AD_USUARIO_ILU"));

        private string DominioILU = "launion.com.gt";
        private string DefaultOuILU = "DC=launion,DC=com,DC=gt";
        private string DefaultRootOuILU = "DC=test,DC=com";
        private string UsuarioILU = Encriptador.Desencriptar(Configuration.getVar("AD_USUARIO_ILU"));
        private string PasswordILU = Encriptador.Desencriptar(Configuration.getVar("AD_PASSWORD_ILU"));

        private string Dominio = "";
        private string DefaultOU = "";
        private string DefaultRootOu;
        private string Usuario = "";
        private string Password = "";

        public ManejoActiveDirectory()
        {
            //String Dominio = "";

            //if (WebConfigurationManager.AppSettings["Sitio"].ToString() == "ILU")
            if(true)
            {
                Dominio = DominioILU;
                DefaultOU = DefaultOuILU;
                DefaultRootOu = DefaultRootOuILU;
                Usuario = UsuarioILU;
                Password = PasswordILU;
            }
            else
            {
                Dominio = DominioCEDILU;
                DefaultOU = DefaultOuCEDILU;
                DefaultRootOu = DefaultRootOuCEDILU;
                Usuario = UsuarioCEDILU;
                Password = PasswordCEDILU;
            }
        }

        #endregion
        #region Validate Methods

        /// <summary>
        /// Valida el usuario y contraseña del usuario
        /// </summary>
        /// <param name="sUserName">Nombre de usuario a validar</param>
        /// <param name="sPassword">La contraseña a validar</param>
        /// <returns>Retorna verdadero si el usuario y contraseña es valido</returns>
        public bool ValidarCredenciales(string sUserName, string sPassword)
        {
            PrincipalContext oPrincipalContext = GetPrincipalContext();
            return oPrincipalContext.ValidateCredentials(sUserName, sPassword);

        }

        /// <summary>
        /// Chequea si la cuenta de usuario ha expirado
        /// </summary>
        /// <param name="sUserName">El nombre de usuario a chequear</param>
        /// <returns>Retorna verdadero si el usuario expiro</returns>
        public bool UsuarioExpirado(string sUserName)
        {
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
            if (oUserPrincipal.AccountExpirationDate != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Chequea si existe el usuario en el active directory
        /// </summary>
        /// <param name="sUserName">El nombre de usuario a chequear</param>
        /// <returns>Retorna verdadero si existe el usuario a chequear</returns>
        public bool ExisteUsuarioEnAD(string sUserName)
        {
            if (ObtenerUsuarioAD(sUserName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Chequea si la cuenta de usuario esta bloqueada
        /// </summary>
        /// <param name="sUserName">el nombre de usuario a chequear</param>
        /// <returns>Regresa verdadero si la cuenta de usuario esta bloqueada</returns>
        public bool EstaLaCuentaDeUsuarioBloqueada(string sUserName)
        {
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
            return oUserPrincipal.IsAccountLockedOut();
        }
        #endregion

        #region Search Methods

        /// <summary>
        /// Gets a certain user on Active Directory
        /// </summary>
        /// <param name="sUserName">The username to get</param>
        /// <returns>Returns the UserPrincipal Object</returns>
        public UserPrincipal ObtenerUsuarioAD(string sUserName)
        {
            PrincipalContext oPrincipalContext = GetPrincipalContext();

            UserPrincipal oUserPrincipal = UserPrincipal.FindByIdentity(oPrincipalContext, sUserName);
            return oUserPrincipal;
        }


        public class UsuarioActiveDirectory
        {
            public string Usuario { get; set; }

            public string NombreCompleto { get; set; }

            public string Company { get; set; }

            public string Deparment { get; set; }

            public string JobTitle { get; set; }

            public string Email { get; set; }

            public string Phone { get; set; }

            public string Mobile { get; set; }
        }

        public List<UsuarioActiveDirectory> ObtenerUsuariosAD()
        {
            List<UsuarioActiveDirectory> rst = new List<UsuarioActiveDirectory>();

            string DomainPath = "LDAP://" + DefaultOuILU;

            DirectoryEntry adSearchRoot = new DirectoryEntry(DomainPath);
            DirectorySearcher adSearcher = new DirectorySearcher(adSearchRoot);

            adSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
            adSearcher.PropertiesToLoad.Add("samaccountname");
            adSearcher.PropertiesToLoad.Add("title");
            adSearcher.PropertiesToLoad.Add("mail");
            adSearcher.PropertiesToLoad.Add("usergroup");
            adSearcher.PropertiesToLoad.Add("company");
            adSearcher.PropertiesToLoad.Add("department");
            adSearcher.PropertiesToLoad.Add("telephoneNumber");
            adSearcher.PropertiesToLoad.Add("mobile");
            adSearcher.PropertiesToLoad.Add("displayname");
            SearchResult result;
            SearchResultCollection iResult = adSearcher.FindAll();

            UsuarioActiveDirectory item;
            if (iResult != null)
            {
                for (int counter = 0; counter < iResult.Count; counter++)
                {
                    result = iResult[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        item = new UsuarioActiveDirectory();

                        item.Usuario = (String)result.Properties["samaccountname"][0];

                        if (result.Properties.Contains("displayname"))
                        {
                            item.NombreCompleto = (String)result.Properties["displayname"][0];
                        }

                        if (result.Properties.Contains("mail"))
                        {
                            item.Email = (String)result.Properties["mail"][0];
                        }

                        if (result.Properties.Contains("company"))
                        {
                            item.Company = (String)result.Properties["company"][0];
                        }

                        if (result.Properties.Contains("title"))
                        {
                            item.JobTitle = (String)result.Properties["title"][0];
                        }

                        if (result.Properties.Contains("department"))
                        {
                            item.Deparment = (String)result.Properties["department"][0];
                        }

                        if (result.Properties.Contains("telephoneNumber"))
                        {
                            item.Phone = (String)result.Properties["telephoneNumber"][0];
                        }

                        if (result.Properties.Contains("mobile"))
                        {
                            item.Mobile = (String)result.Properties["mobile"][0];
                        }
                        rst.Add(item);
                    }
                }
            }

            adSearcher.Dispose();
            adSearchRoot.Dispose();

            return rst;
        }



        /// <summary>
        /// Gets a certain group on Active Directory
        /// </summary>
        /// <param name="sGroupName">The group to get</param>
        /// <returns>Returns the GroupPrincipal Object</returns>
        public GroupPrincipal GetGroup(string sGroupName)
        {
            PrincipalContext oPrincipalContext = GetPrincipalContext();

            GroupPrincipal oGroupPrincipal = GroupPrincipal.FindByIdentity(oPrincipalContext, sGroupName);
            return oGroupPrincipal;
        }

        #endregion

        #region User Account Methods

        /// <summary>
        /// Setea una contraseña de usuario
        /// </summary>
        /// <param name="sUserName">Usuario</param>
        /// <param name="sNewPassword">Nueva contraseña</param>
        /// <param name="sMessage">Mensaje de salida en caso exista algun error</param>
        public void SetearContraseñaUsuarioAD(string sUserName, string sNewPassword, out string sMessage)
        {
            try
            {
                UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
                oUserPrincipal.SetPassword(sNewPassword);
                sMessage = "";
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }

        }

        /// <summary>
        /// Habilita una cuenta de usuario
        /// </summary>
        /// <param name="sUserName">The username to enable</param>
        public void HabilitaCuentaDeUsuario(string sUserName)
        {
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
            oUserPrincipal.Enabled = true;
            oUserPrincipal.Save();
        }

        /// <summary>
        /// Forza deshabilitar una cuenta de usuario
        /// </summary>
        /// <param name="sUserName">El usuario a deshabilitar/param>
        public void DeshabilitarCuentaUsuario(string sUserName)
        {
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
            oUserPrincipal.Enabled = false;
            oUserPrincipal.Save();
        }

        /// <summary>
        /// Forza a expirar la contraseña del usuario
        /// </summary>
        /// <param name="sUserName">el usuario para expirar la contraseña/param>
        public void ExpirarContraseñaDeUsuario(string sUserName)
        {
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
            oUserPrincipal.ExpirePasswordNow();
            oUserPrincipal.Save();

        }

        /// <summary>
        /// Desbloquea una cuenta de usuario
        /// </summary>
        /// <param name="sUserName">Nombre de usuario a desbloquear</param>
        public void DesbloquearCuentaDeUsuario(string sUserName)
        {
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
            oUserPrincipal.UnlockAccount();
            oUserPrincipal.Save();
        }



        /// <summary>
        /// Crea un nuevo usuario en el active directory
        /// </summary>
        /// <param name="sOU">The OU location you want to save your user</param>
        /// <param name="sUserName">El usuario del nuevo usuario</param>
        /// <param name="sPassword">La contraseña del nuevo usuario</param>
        /// <param name="sGivenName">El nombre personal del nuevo usuario</param>
        /// <param name="sSurname">El apellido del nuevo usuario</param>
        /// <returns>Regrese el objetvo del nuevo usuario creado</returns>
        public UserPrincipal CrearNuevoUsuarioEnAD(string sOU, string sUserName, string sPassword, string sGivenName, string sSurname)
        {
            if (!ExisteUsuarioEnAD(sUserName))
            {
                PrincipalContext oPrincipalContext = GetPrincipalContext(sOU);

                UserPrincipal oUserPrincipal = new UserPrincipal(oPrincipalContext, sUserName, sPassword, true /*Enabled or not*/);

                //User Log on Name
                oUserPrincipal.UserPrincipalName = sUserName;
                oUserPrincipal.GivenName = sGivenName;
                oUserPrincipal.Surname = sSurname;
                oUserPrincipal.Save();

                return oUserPrincipal;
            }
            else
            {
                return ObtenerUsuarioAD(sUserName);
            }
        }

        /// <summary>
        /// Eliminar Usuario en active directory
        /// </summary>
        /// <param name="sUserName">El usuario que deseas eliminar</param>
        /// <returns>Regresa verdadero si realmente es utilizado</returns>
        public bool EliminarUsuario(string sUserName)
        {
            try
            {
                UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);

                oUserPrincipal.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Group Methods

        /// <summary>
        /// Creates a new group in Active Directory
        /// </summary>
        /// <param name="sOU">The OU location you want to save your new Group</param>
        /// <param name="sGroupName">The name of the new group</param>
        /// <param name="sDescription">The description of the new group</param>
        /// <param name="oGroupScope">The scope of the new group</param>
        /// <param name="bSecurityGroup">True is you want this group to be a security group, false if you want this as a distribution group</param>
        /// <returns>Retruns the GroupPrincipal object</returns>
        public GroupPrincipal CrearNuevoGrupoEnAD(string sOU, string sGroupName, string sDescription, GroupScope oGroupScope, bool bSecurityGroup)
        {
            PrincipalContext oPrincipalContext = GetPrincipalContext(sOU);

            GroupPrincipal oGroupPrincipal = new GroupPrincipal(oPrincipalContext, sGroupName);
            oGroupPrincipal.Description = sDescription;
            oGroupPrincipal.GroupScope = oGroupScope;
            oGroupPrincipal.IsSecurityGroup = bSecurityGroup;
            oGroupPrincipal.Save();

            return oGroupPrincipal;
        }

        /// <summary>
        /// Adds the user for a given group
        /// </summary>
        /// <param name="sUserName">The user you want to add to a group</param>
        /// <param name="sGroupName">The group you want the user to be added in</param>
        /// <returns>Returns true if successful</returns>
        public bool AñadirUsuarioAGrupoEnAD(string sUserName, string sGroupName)
        {
            try
            {
                UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
                GroupPrincipal oGroupPrincipal = GetGroup(sGroupName);
                if (oUserPrincipal != null && oGroupPrincipal != null)
                {
                    if (!EsUsuarioMiembroDeGrupo(sUserName, sGroupName))
                    {
                        oGroupPrincipal.Members.Add(oUserPrincipal);
                        oGroupPrincipal.Save();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes user from a given group
        /// </summary>
        /// <param name="sUserName">The user you want to remove from a group</param>
        /// <param name="sGroupName">The group you want the user to be removed from</param>
        /// <returns>Returns true if successful</returns>
        public bool EliminarUsuarioDeGrupoEnAD(string sUserName, string sGroupName)
        {
            try
            {
                UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
                GroupPrincipal oGroupPrincipal = GetGroup(sGroupName);
                if (oUserPrincipal != null && oGroupPrincipal != null)
                {
                    if (EsUsuarioMiembroDeGrupo(sUserName, sGroupName))
                    {
                        oGroupPrincipal.Members.Remove(oUserPrincipal);
                        oGroupPrincipal.Save();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if user is a member of a given group
        /// </summary>
        /// <param name="sUserName">The user you want to validate</param>
        /// <param name="sGroupName">The group you want to check the membership of the user</param>
        /// <returns>Returns true if user is a group member</returns>
        public bool EsUsuarioMiembroDeGrupo(string sUserName, string sGroupName)
        {
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);
            GroupPrincipal oGroupPrincipal = GetGroup(sGroupName);

            if (oUserPrincipal != null && oGroupPrincipal != null)
            {
                return oGroupPrincipal.Members.Contains(oUserPrincipal);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a list of the users group memberships
        /// </summary>
        /// <param name="sUserName">The user you want to get the group memberships</param>
        /// <returns>Returns an arraylist of group memberships</returns>
        public ArrayList ObtenerUsuariosDeGrupos(string sUserName)
        {
            ArrayList myItems = new ArrayList();
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);

            PrincipalSearchResult<Principal> oPrincipalSearchResult = oUserPrincipal.GetGroups();

            foreach (Principal oResult in oPrincipalSearchResult)
            {
                myItems.Add(oResult.Name);
            }
            return myItems;
        }

        /// <summary>
        /// Gets a list of the users authorization groups
        /// </summary>
        /// <param name="sUserName">The user you want to get authorization groups</param>
        /// <returns>Returns an arraylist of group authorization memberships</returns>
        public ArrayList ObtenerGruposAutorizadosDelUsuarioEnAD(string sUserName)
        {
            ArrayList myItems = new ArrayList();
            UserPrincipal oUserPrincipal = ObtenerUsuarioAD(sUserName);

            PrincipalSearchResult<Principal> oPrincipalSearchResult = oUserPrincipal.GetAuthorizationGroups();

            foreach (Principal oResult in oPrincipalSearchResult)
            {
                myItems.Add(oResult.Name);
            }
            return myItems;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the base principal context
        /// </summary>
        /// <returns>Retruns the PrincipalContext object</returns>
        public PrincipalContext GetPrincipalContext()
        {


            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, Dominio, DefaultOU, ContextOptions.SimpleBind, Usuario, Password);
            return oPrincipalContext;
        }

        /// <summary>
        /// Gets the principal context on specified OU
        /// </summary>
        /// <param name="sOU">The OU you want your Principal Context to run on</param>
        /// <returns>Retruns the PrincipalContext object</returns>
        public PrincipalContext GetPrincipalContext(string sOU)
        {
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain, Dominio, sOU, ContextOptions.SimpleBind, Usuario, Password);
            return oPrincipalContext;
        }

        #endregion
    }
}