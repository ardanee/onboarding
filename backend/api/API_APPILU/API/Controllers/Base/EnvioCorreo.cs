using API.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace API_MonitoreoRTK.Controllers.Base
{
    public class EnvioCorreo
    {
        private const string MatchEmailPattern = @"^([a-zA-Z0-9_\-])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$";
        Encriptador encript = new Encriptador();

        /// <summary>
        /// Evalua si el valor enviado es numerico o no
        /// </summary>
        public static bool EsNumero(object expresion)
        {
            bool isNum;
            double retNum;

            isNum = Double.TryParse(Convert.ToString(expresion), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        /// <summary>
        /// Evalua si el valor enviado es numerico o no
        /// </summary>  
        public static bool EsNumerico(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";

            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) && !objTwoDotPattern.IsMatch(strNumber) && !objTwoMinusPattern.IsMatch(strNumber) && objNumberPattern.IsMatch(strNumber);

        }

        /// <summary>
        /// Evalua si el valor enviado es positivo entero o real
        /// </summary>        
        public static bool EsNumeroPositivo(String strNumber)
        {
            Regex objNotPositivePattern = new Regex("[^0-9.]");
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");

            return !objNotPositivePattern.IsMatch(strNumber) && objPositivePattern.IsMatch(strNumber) && !objTwoDotPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// Evalua si el valor enviado es fecha o no
        /// </summary>
        public static bool EsFecha(object expresion)
        {

            bool isFecha;

            DateTime retFecha;

            isFecha = DateTime.TryParse(Convert.ToString(expresion), out retFecha);

            return isFecha;

        }

        /// <summary>
        /// Evalua si el valor enviado es un correo o no
        /// </summary>
        public static bool EsCorreoValido(String _correo)
        {
            if (_correo != null) return Regex.IsMatch(_correo, MatchEmailPattern);
            else return false;

        }

        /// <summary>
        /// Envía correo texto
        /// </summary>
        /// <param name="destinatario"></param>
        /// <param name="remitente"></param>
        /// <param name="mensaje"></param>
        /// <param name="asunto"></param>
        public static void EnviarCorreo(string destinatario, string remitente, string mensaje, string asunto)
        {
            try
            {
                /*-------------------------MENSAJE DE CORREO----------------------*/

                //Creamos un nuevo Objeto de mensaje
                MailMessage mmsg = new MailMessage();

                //Direccion de correo electronico a la que queremos enviar el mensaje
                foreach (var email in destinatario.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mmsg.To.Add(new MailAddress(email));

                }

                //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario

                //Asunto
                mmsg.Subject = asunto;
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

                //Direccion de correo electronico que queremos que reciba una copia del mensaje
                //mmsg.Bcc.Add("destinatariocopia@servidordominio.com"); //Opcional

                //Cuerpo del Mensaje
                mmsg.Body = mensaje;
                mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                mmsg.IsBodyHtml = false; //Si no queremos que se envíe como HTML

                //Correo electronico desde la que enviamos el mensaje
                mmsg.From = new MailAddress(remitente);


                /*-------------------------CLIENTE DE CORREO----------------------*/
                string Servidor = Encriptador.Desencriptar(API.Helpers.Configuration.getVar("SMTP_MailServer"), "L@Union2015Mcc");

                string Usuario = Encriptador.Desencriptar(API.Helpers.Configuration.getVar("SMTP_MailUser"), "L@Union2015Mcc");
                string Con = Encriptador.Desencriptar(API.Helpers.Configuration.getVar("SMTP_MailPassWord"), "L@Union2015Mcc");

                //Creamos un objeto de cliente de correo
                SmtpClient cliente = new SmtpClient(Servidor, 25);
                System.Net.NetworkCredential SMTPUserInfo = null;

                SMTPUserInfo = new System.Net.NetworkCredential(Usuario, Con);
                cliente.UseDefaultCredentials = false;

                cliente.Credentials = SMTPUserInfo;
                cliente.Send(mmsg);


            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Envía correo html
        /// </summary>
        /// <param name="destinatario"></param>
        /// <param name="remitente"></param>
        /// <param name="mensaje"></param>
        /// <param name="asunto"></param>
        public static bool EnviarCorreoHtml(string destinatario, string remitente, string mensaje, string asunto)
        {
            try
            {
                /*-------------------------MENSAJE DE CORREO----------------------*/
                MailMessage mmsg = new MailMessage();

                //Direccion de correo electronico a la que queremos enviar el mensaje
                foreach (var email in destinatario.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mmsg.To.Add(new MailAddress(email));

                }

                //Asunto
                mmsg.Subject = asunto;
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

                //Cuerpo del Mensajea
                mmsg.Body = mensaje;
                mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML

                //Correo electronico desde la que enviamos el mensaje
                mmsg.From = new MailAddress(remitente);

                /*-------------------------CLIENTE DE CORREO----------------------*/
                string Servidor = Encriptador.Desencriptar(API.Helpers.Configuration.getVar("SMTP_MailServer"), "L@Union2015Mcc");

                string Usuario = Encriptador.Desencriptar(API.Helpers.Configuration.getVar("SMTP_MailUser"), "L@Union2015Mcc");
                string Con = Encriptador.Desencriptar(API.Helpers.Configuration.getVar("SMTP_MailPassWord"), "L@Union2015Mcc");

                //Creamos un objeto de cliente de correo
                SmtpClient cliente = new SmtpClient(Servidor, 25);
                System.Net.NetworkCredential SMTPUserInfo = null;

                SMTPUserInfo = new System.Net.NetworkCredential(Usuario, Con);
                cliente.UseDefaultCredentials = false;

                cliente.Credentials = SMTPUserInfo;
                cliente.Send(mmsg);
                return true;
            }
            catch (SmtpException ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
    }
}