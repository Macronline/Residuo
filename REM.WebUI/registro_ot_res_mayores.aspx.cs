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

public partial class registro_ot_res_mayores : System.Web.UI.Page
{
    [WebMethod]
    public static string WLeerDatosIniciales(string sIdSolicitud)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            usr.ReadMunic();

            //Ruta rutas = new Ruta(usr);
            //rutas.ReadAll(double.Parse(UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos)));
            //UtilWeb.AddElementSeleccioneALista(rutas.Datos);
            UtilWeb util = new UtilWeb();
            //string jsonRutas = util.GetJson(rutas.Datos);

            VehRecolector camiones = new VehRecolector(usr);
            camiones.ReadAll(double.Parse(UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos)));
            UtilWeb.AddElementSeleccioneALista(camiones.Datos);
            string jsonCamiones = util.GetJson(camiones.Datos);

            Solicitud solicitud = new Solicitud(usr);
            if (solicitud.Read(sIdSolicitud))
            {
                Lista.Add("Exito");
                if (solicitud.Datos.Rows[0]["NumeroOT"].ToString().Length > 0)
                    Lista.Add(string.Format("OT: {0}", solicitud.Datos.Rows[0]["NumeroOT"].ToString()));
                else
                    Lista.Add("OT: POR ASIGNAR");

                Lista.Add(jsonCamiones);
                Lista.Add(solicitud.Datos.Rows[0]["IdCamion"].ToString());
                Lista.Add(solicitud.Datos.Rows[0]["NombreChoferes"].ToString());
                Lista.Add(solicitud.Datos.Rows[0]["CorreosOT"].ToString());
                Lista.Add(solicitud.Datos.Rows[0]["FechaOT"].ToString());
                Lista.Add(solicitud.Datos.Rows[0]["HoraOT"].ToString());

                Lista.Add(solicitud.Datos.Rows[0]["Nombre"].ToString());
                Lista.Add(solicitud.Datos.Rows[0]["Direccion"].ToString());
                Lista.Add(solicitud.Datos.Rows[0]["TipoResiduo"].ToString());
                Lista.Add("");
                Lista.Add(solicitud.Datos.Rows[0]["ValorRetiroOT"].ToString());
               
            }
            
            //Lista.Add(jsonRutas);
            //Lista.Add(jsonCamiones);

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
    public static string WGrabarOt(string sIdSolicitud, string sNumero, string sFecha, string sIdCamion, string sNombreChoferes, string sCorreos, 
        string sHora, string sValorRetiro)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            double IdNew = 0;
            usr.ReadMunic();
            string sIdClienteMunicipio = UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos);

            double IdOT = 0;
            double IdSoltemp = 0;
            Ot ot = new Ot(usr);
            if (double.TryParse(sIdSolicitud, out IdSoltemp))
            {
                IdOT = ot.ReadByIdSolicitud(sIdSolicitud);
            }

            if (IdOT == 0)
                IdNew = ot.Create(sNumero, sFecha, sIdClienteMunicipio, "", sIdCamion, sNombreChoferes, sCorreos, sHora, sValorRetiro, sIdSolicitud);
            else
                ot.Update(IdOT.ToString(), sNumero, sFecha, sIdClienteMunicipio, "", sIdCamion, sNombreChoferes, sCorreos, sHora, sValorRetiro, sIdSolicitud);
            
            //WLeerUsuarioConectadoAndMunicipios_Internal(usr, ref html);

            Lista.Add("Exito");
            Lista.Add(string.Format("OT: {0}", IdNew.ToString()));
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
    public static string WEnviarCorreoSolicitud(string sIdSolicitud, string sNumero, string sFecha, string sIdCamion, string sNombreChoferes, string sCorreos,
    string sHora, string sValorRetiro)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            double IdNew = 0;
            usr.ReadMunic();
            string sIdClienteMunicipio = UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos);

            double IdOT = 0;
            double IdSoltemp = 0;
            Solicitud solic = new Solicitud(usr);
            if (solic.Read(sIdSolicitud))
            {
                Ot ot = new Ot(usr);
                if (double.TryParse(sIdSolicitud, out IdSoltemp))
                {
                    IdOT = ot.ReadByIdSolicitud(sIdSolicitud);
                }
                VehRecolector camion = new VehRecolector(usr);
                camion.Read(sIdCamion);

                //Viene leida la OT
                string CorreoString = File.ReadAllText(HttpContext.Current.Server.MapPath("~/templates") + "/correoavisoaciudadano.txt");
                CorreoString = CorreoString.Replace("[FECHA_RETIRO]", sFecha);
                CorreoString = CorreoString.Replace("[HORA_RETIRO]", sHora);
                CorreoString = CorreoString.Replace("[PATENTE_CAMION_RETIRO]", camion.Datos.Rows[0]["NombreTagPatente"].ToString());
                CorreoString = CorreoString.Replace("[CHOFER_CAMION_RETIRO]", sNombreChoferes);
                CorreoString = CorreoString.Replace("[NUMERO_FONO_PARA_LLAMAR_CIUDADANO]", 
                    ConfigurationManager.AppSettings["NUMERO_FONO_PARA_LLAMAR_CIUDADANO"].ToString());

                string CorreoDestino = sCorreos;
                UtilWeb.SendMail(CorreoString, CorreoDestino, "ASIGNACIÓN DE RETIRO", false);

            }
            
            Lista.Add("Exito");
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