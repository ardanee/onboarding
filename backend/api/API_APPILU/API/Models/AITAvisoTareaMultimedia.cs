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
    
    public partial class AITAvisoTareaMultimedia
    {
        public int CodigoAvisoTareaMultimedia { get; set; }
        public int CodigoMultimedia { get; set; }
        public int CodigoAvisoTarea { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }
        public int CodigoUsuarioInserto { get; set; }
        public string UsuarioInserto { get; set; }
        public System.DateTime FechaInserto { get; set; }
        public Nullable<int> CodigoUsuarioModifico { get; set; }
        public string UsuarioModifico { get; set; }
        public Nullable<System.DateTime> FechaModifico { get; set; }
    
        public virtual AITAvisoTarea AITAvisoTarea { get; set; }
        public virtual AITMultimedia AITMultimedia { get; set; }
        public virtual AITUsuario AITUsuario { get; set; }
        public virtual AITUsuario AITUsuario1 { get; set; }
    }
}
