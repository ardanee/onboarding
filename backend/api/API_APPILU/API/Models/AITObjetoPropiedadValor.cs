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
    
    public partial class AITObjetoPropiedadValor
    {
        public int Id { get; set; }
        public Nullable<int> IdObjeto { get; set; }
        public Nullable<int> IdPropiedad { get; set; }
        public string Valor { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<bool> Eliminado { get; set; }
        public Nullable<int> CodigoUsuarioInserto { get; set; }
        public string UsuarioInserto { get; set; }
        public Nullable<System.DateTime> FechaInserto { get; set; }
        public Nullable<int> CodigoUsuarioModifico { get; set; }
        public string UsuarioModifico { get; set; }
        public Nullable<System.DateTime> FechaModifico { get; set; }
    
        public virtual AITObjeto AITObjeto { get; set; }
        public virtual AITPropiedadTipoObjeto AITPropiedadTipoObjeto { get; set; }
    }
}
