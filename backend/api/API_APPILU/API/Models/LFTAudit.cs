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
    
    public partial class LFTAudit
    {
        public int CodigoAudit { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoIdentidad { get; set; }
        public System.DateTime Fecha { get; set; }
        public string NombreTabla { get; set; }
        public string NombreCampo { get; set; }
        public string Accion { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }
        public Nullable<decimal> Latitud { get; set; }
        public Nullable<decimal> Longitud { get; set; }
    }
}
