using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Helpers
{
    public class Helpers
    {
        public static string obtenerFechaAmigable(DateTime d)
        {
            
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if(dayDiff>= 31 && dayDiff <= 360)
            {
                return d.ToString("dd MMM");
            }

            if (dayDiff >360)
            {
                return d.ToString("dd MMM yyyy");
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "ahora";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "hace 1 min";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("hace {0} min",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "hace 1 hr";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("hace {0} hrs",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "ayer";
            }
            if (dayDiff < 7)
            {
                return string.Format("hace {0} días",
                    dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("hace {0} sem.",
                    Math.Ceiling((double)dayDiff / 7));
            }

            return d.ToString("dd MMM yyyy");
        }

        public static string obtenerFechaAmigable2(DateTime d)
        {

            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            if (d.Date == DateTime.Today)
            {
                return "Hoy";
            }

            if(DateTime.Today - d.Date == TimeSpan.FromDays(1))
            {
                return "Ayer";
            }

            // 4.
            // Don't allow out of range values.
            if (dayDiff >= 1 && dayDiff <= 360)
            {
                return d.ToString("dd MMM");
            }

            if (dayDiff > 360)
            {
                return d.ToString("dd MMM yyyy");
            }

            // 5.
            // Handle same-day times.

            return d.ToString("dd MMM yyyy");
        }

        public static string obtenerFechaAmigable3(DateTime d)
        {
            // 
            string hora = "";
            string minuto = "";
            minuto = (d.Minute < 10) ? "0" + d.Minute : d.Minute.ToString();
            hora = d.Hour.ToString();
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            DateTime fechaHoy = DateTime.Now;

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff >= 31 && dayDiff <= 360)
            {
                return "el "+d.ToString("dd MMM")+ " a las "+hora+":"+ minuto;
            }

            if (dayDiff > 360)
            {
                return "el "+d.ToString("dd MMM yyyy")+ " a las "+hora + ":" + minuto;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "ahora";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "hace 1 min";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("hace {0} min",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.

                if (fechaHoy.Year == d.Year && fechaHoy.Month == d.Month && fechaHoy.Day == d.Day)
                {
                    return "hoy a las " + hora + ":" + minuto;
                }
                else
                {
                    return "ayer a las " + hora + ":" + minuto;
                }
            }
            //// 6.
            //// Handle previous days.
            //if (dayDiff == 1)
            //{
            //    return "ayer a las "+ hora + ":" + minuto; ;
            //}
            if (dayDiff < 7)
            {
                //return string.Format("hace {0} días",
                //    dayDiff);
                return "el " + d.ToString("dd MMM") + " a las " + hora + ":" + minuto;
            }
            if (dayDiff < 31)
            {
                //return string.Format("hace {0} sem.",
                //    Math.Ceiling((double)dayDiff / 7));
                return "el " + d.ToString("dd MMM") + " a las " + hora + ":" + minuto;
            }

            return "el " + d.ToString("dd MMM yyyy") + " a las " + hora + ":" + minuto;
            //return d.ToString("dd MMM yyyy");
        }

        public static string obtenerDiaMesAmigable(int dia, int mes)
        {
            string diaString = dia.ToString();
            string[] meses = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            if (dia < 10)
            {
                diaString = "0" + dia;
            }

            



            return diaString+" "+meses[mes-1];

        }

        public static string obtenerMes(int mes)
        {
            string[] meses = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            return meses[mes - 1];

        }
    }
}