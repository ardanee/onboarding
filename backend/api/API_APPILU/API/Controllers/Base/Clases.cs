using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Controllers.Base
{

    public class RespuestaGenerica
    {
        public bool response { get; set; }
        public string mensaje { get; set; }
        public string data { get; set; }
    }

    public class RespuestaHttpGenerica
    {
        public bool response { get; set; }
        public string mensaje { get; set; }
        public dynamic data { get; set; }
    }

    #region Plantilla
    public class PlantillaRondaQuery
    {
        public int IdRonda { get; set; }
        public int IdLiga { get; set; }
        public decimal? Presupuesto { get; set; }
        public int? NumeroRonda { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? EstadoRonda { get; set; }
    }
    
    #endregion

}