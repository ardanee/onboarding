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
    
    public partial class AITReporteBiosalcDetalleVariable
    {
        public int CodigoReporteDetalleVariable { get; set; }
        public string NombreVariable { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }
        public string UsuarioInserto { get; set; }
        public System.DateTime FechaInserto { get; set; }
        public string UsuarioModifico { get; set; }
        public Nullable<System.DateTime> FechaModifico { get; set; }
        public int CodigoReporteDetalle { get; set; }
        public Nullable<decimal> Operacion { get; set; }
        public string NombreEnReporte { get; set; }
        public bool Titulo { get; set; }
        public Nullable<bool> Visible { get; set; }
    
        public virtual AITReporteBiosalcDetalle AITReporteBiosalcDetalle { get; set; }
    }
}
