using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Controllers.ModeloPosts
{
    public class Ejemplo
    {
        public int CodigoAviso { get; set; }
        public int CodigoProblema { get; set; }
        public dynamic Solicitante { get; set; }
        public DateTime FechaInserto { get; set; }
        public string FechaInsertoAmigable { get; set; }
        public AITEstado Estado { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public AITProblema Problema { get; set; }
        public List<string> Asignados { get; set; }

    }

    public class DeleteObject
    {
        public int Id { get; set; }
        public int CodigoUsuarioOpera  { get; set; }
        public string NombreUsuarioOpera { get; set; }
    }
    public class LigaPost
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public int CodigoUsuarioAdministrador { get; set; }
        public int IdInstitucion { get; set; }
        public bool Publico { get; set; }
        public DateTime? FechaInicio { get; set; }
        public int? CantidadRecurrencia { get; set; }
        public string UnidadRecurrencia { get; set; }
        public bool Activo { get; set; }
        public int? CantidadRondas { get; set; }
        public bool Editable { get; set; }
        public bool EditarPlantilla { get; set; }
        public decimal PresupuestoInicial { get; set; }
        public string Password { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }



    public class MetricaPost
    {
        public int? Id { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal puntos { get; set; }
        public bool positivo { get; set; }
        public decimal valuacion { get; set; }
        public bool esPorcentaje { get; set; }
        public bool Activo { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }

    public class ResultadoRondaPost
    {
        public int IdRonda { get; set; }
        public int IdJugadorLiga { get; set; }
        public int IdMetrica { get; set; }
        public decimal Resultado { get; set; }
     }

    public class resultadoPost
    {
        public List<ResultadoRondaPost> resultado { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }

    public class BuscarLigaPost
    {
        public int CodigoUsuario { get; set; }
        public int IdInstitucion { get; set; }
        public string NombreLiga { get; set; }
        public int? Activa { get; set; }
    }

    public class UnirseLigaPost
    {
        public int CodigoUsuario { get; set; }
        public int IdLiga { get; set; }
    }

    public class ValidarPassPost
    {
        public int IdLiga { get; set; }
        public string Pass { get; set; }
    }

    public class TopJugadoresPost
    {
        public int IdLiga { get; set; }
        public int? IdRonda { get; set; }
        public int? IdCategoria { get; set; }
    }

    public class TopJugadoresDetallePost
    {
        public int IdJugadorLiga { get; set; }
        public int? IdRonda { get; set; }
    }

    public class CierrePost
    {
        public int IdRonda { get; set; }
        public int? IdLiga { get; set; }
        public int? CodigoUsuarioOpera { get; set; }
    }

    public class CrearCuentaObject
    {
        public int? CodigoEmpleado { get; set; }
        public string DPI { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string CorreoElectronico { get; set; }
        public int? NumeroTelefono { get; set; }
    }


    #region CategoriaJugador
    public class CategoriaJugadorGuardar
    {
        public int? Id { get; set; }
        public int IdLiga { get; set; }
        public string Nombre { get; set; }
        public int MinimoJugadores { get; set; }
        public int MaximoJugadores { get; set; }
        public int IdInstitucion { get; set; }
        public bool Activo { get; set; }
        public bool Eliminado { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }
    #endregion

    #region Jugador de una liga (Admon)
    public class JugadorLigaGuardar
    {
        public int? Id { get; set; }
        public int? IdCategoriaJugador { get; set; }
        public int? IdJugador { get; set; }
        public int IdLiga { get; set; }
        public decimal Precio { get; set; }
        public bool? Activo { get; set; }
        public bool? Eliminado { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }

    #endregion

    #region Plantilla
    public class AgregarJugadorPlantillaPost
    {
        public int IdJugador { get; set; }
        public int IdEquipo { get; set; }
        public int IdLiga { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }

    public class EliminarJugadorPlantillaPost
    {
        public int IdMiembroEquipoLiga { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }
    #endregion

    #region Ronda
    public class RondaCerrarPost
    {
        public int Id { get; set; }
        public int CodigoUsuarioOpera { get; set; }
        public string UsuarioOpera { get; set; }
    }
    #endregion
}