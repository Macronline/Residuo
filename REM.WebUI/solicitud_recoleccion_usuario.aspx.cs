using REM.BC2;
using REM.BC2.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class solicitud_recoleccion_usuario : System.Web.UI.Page
{
    [WebMethod]
    public static string WLeerDatosIniciales()
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        //if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
        //    return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            ClienteMunicipio munic = new ClienteMunicipio();
            munic.ReadAll();

            UtilWeb.AddElementSeleccioneALista(munic.Datos);
            UtilWeb util = new UtilWeb();
            string jsonMunic = util.GetJson(munic.Datos);
            
            Lista.Add("Exito");
            Lista.Add(jsonMunic);

        }
        catch (Exception ex)
        {
            Lista.Add("Exception");
            Lista.Add(ex.Message);
            Lista.Add("Hubo un error no controlado en la aplicación, por favor inténtelo nuevamente, si el problema persiste contactarse con el administrador sistemas para revisar el log de eventos del servidor.");
        }
        return scriptSerializer.Serialize(Lista.ToArray());
    }


    [WebMethod]
    
    public static string WGrabarSolicitud(string sNombre, string sRut, string sDireccion, string sCorreo, string sTelefono,
        string sTipo_residuo, string sComentarios, string sIdMunic)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        //Usuario usr = new Usuario();
        //if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
        //    return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            double IdNew = 0;
            Solicitud solicitud = new Solicitud();
            IdNew = solicitud.Create(sNombre, sRut, sDireccion, sCorreo, sTelefono,sTipo_residuo, sComentarios, sIdMunic);

            //WLeerUsuarioConectadoAndMunicipios_Internal(usr, ref html);

            string CorreoString = File.ReadAllText(HttpContext.Current.Server.MapPath("~/templates") + "/correorecepcionsolicitud.txt");
            CorreoString = CorreoString.Replace("[FECHA_RETIRO]", UtilWeb.GetDateNow().ToShortDateString());
            CorreoString = CorreoString.Replace("[NUMERO_FONO_PARA_LLAMAR_CIUDADANO]",
                ConfigurationManager.AppSettings["NUMERO_FONO_PARA_LLAMAR_CIUDADANO"].ToString());

            string CorreoDestino = sCorreo;
            UtilWeb.SendMail(CorreoString, CorreoDestino, "AViSO DE RECEPCIÓN DE SOLICITUD", false);

            Lista.Add("Exito");
            Lista.Add(string.Format("La Solicitud fue grabada con éxito. Folio de Solicitud: {0}", IdNew.ToString()));
        }
        catch (Exception ex)
        {
            Lista.Add("Exception");
            Lista.Add(ex.Message);
            Lista.Add("Hubo un error no controlado en la aplicación, por favor inténtelo nuevamente, si el problema persiste contactarse con el administrador sistemas para revisar el log de eventos del servidor.");
        }
        return scriptSerializer.Serialize(Lista.ToArray());

    }
}