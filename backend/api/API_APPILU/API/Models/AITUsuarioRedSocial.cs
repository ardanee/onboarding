//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AITUsuarioRedSocial
    {
        public int CodigoUsuarioRedSocial { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoRedSocial { get; set; }
        public string Token { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }
        public string UsuarioInserto { get; set; }
        public System.DateTime FechaInserto { get; set; }
        public string UsuarioModifico { get; set; }
        public Nullable<System.DateTime> FechaModifico { get; set; }
    
        public virtual AITRedSocial AITRedSocial { get; set; }
        public virtual AITUsuario AITUsuario { get; set; }
    }
}
