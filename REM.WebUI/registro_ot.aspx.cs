using REM.BC2;
using REM.BC2.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registro_ot : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string WLeerDatosDeOT(string sIdOt)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            usr.ReadMunic();

            Ot ot = new Ot(usr);
            if (ot.Read(sIdOt))
            {
                Lista.Add("Exito");
                Lista.Add(string.Format("Municipalidad: {0}", UtilWeb.GetDatoSingular("NombreMunic", usr.oClienteMunicipio.Datos)));
                Lista.Add(string.Format("OT: {0}", UtilWeb.GetDatoSingular("Numero", ot.Datos)));
                Lista.Add(UtilWeb.GetDatoSingular("IdRuta", ot.Datos));
                Lista.Add(UtilWeb.GetDatoSingular("IdCamion", ot.Datos));
                Lista.Add(UtilWeb.GetDatoSingular("NombreChoferes", ot.Datos));
                Lista.Add(UtilWeb.GetDatoSingular("Correos", ot.Datos));
                Lista.Add(UtilWeb.GetDatoSingular("Hora", ot.Datos));
                Lista.Add(UtilWeb.GetDatoSingular("Fecha", ot.Datos));

            }
            else {
                Lista.Add("NoExiste");
            }

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
    public static string WLeerDatosIniciales()
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            usr.ReadMunic();

            Ruta rutas = new Ruta(usr);
            rutas.ReadAll(double.Parse(UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos)));
            UtilWeb.AddElementSeleccioneALista(rutas.Datos);
            UtilWeb util = new UtilWeb();
            string jsonRutas = util.GetJson(rutas.Datos);

            VehRecolector camiones = new VehRecolector(usr);
            camiones.ReadAll(double.Parse(UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos)));
            UtilWeb.AddElementSeleccioneALista(camiones.Datos);
            string jsonCamiones = util.GetJson(camiones.Datos);

            Lista.Add("Exito");
            Lista.Add(string.Format("Municipalidad: {0}", UtilWeb.GetDatoSingular("NombreMunic", usr.oClienteMunicipio.Datos)));
            Lista.Add(jsonRutas);
            Lista.Add(jsonCamiones);
            
        }
        catch (Exception ex)
        {
            Lista.Add("Exception");
            Lista.Add(ex.Message);
            Lista.Add("Hubo un error no controlado en la aplicación, por favor inténtelo nuevamente, si el problema persiste contactarse con el administrador sistemas para revisar el log de eventos del servidor.");
        }
        return scriptSerializer.Serialize(Lista.ToArray());

    }

    private static void WLeerDatosIniciales_Private(Usuario usr, ref string html)
    {
        
        ////Se genera la tabla HTML
        //List<ColumnaHtml> lista = new List<ColumnaHtml>();
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreUserMunic"));
        ////lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutUserMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "CorreoUserMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));  //Aca va IMAGEN
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditMunicipio"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteMunicipio"));

        //UtilWeb util = new UtilWeb();
        //html = util.GetHtmlTableBasica(cliente.Datos, lista);

    }

    [WebMethod]
    public static string WGrabarOt(string sId, string sNumero, string sFecha, string sIdClienteMunicipio, string sIdRuta, string sIdCamion,
        string sNombreChoferes, string sCorreos, string sHora)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            if (sCorreos.Trim().Length == 0)
            {
                Lista.Add("error");
                Lista.Add("Ingrese el dato: Correos");
                return scriptSerializer.Serialize(Lista.ToArray());
            }

            if (! UtilWeb.IsValidMail(sCorreos.Trim()) )
            {
                Lista.Add("error");
                Lista.Add("El dato Correos está incorrecto");
                return scriptSerializer.Serialize(Lista.ToArray());
            }

            double IdNew = 0;
            Ot ot = new Ot(usr);
            usr.ReadMunic();
            sIdClienteMunicipio = UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos);

            if (sId.Trim().Length == 0)
                IdNew = ot.Create(sNumero, sFecha, sIdClienteMunicipio, sIdRuta, sIdCamion, sNombreChoferes, sCorreos, sHora, "0", "0");
            else
                ot.Update(sId, sNumero, sFecha, sIdClienteMunicipio, sIdRuta, sIdCamion, sNombreChoferes, sCorreos, sHora, "0", "0");

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
    public static string WLeerLogoMunicipio()
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            usr.ReadMunic();
            string sIdClienteMunicipio = UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos);
            Lista.Add("Exito");
            Lista.Add(string.Format("/images/MunLog_{0}.jpg", sIdClienteMunicipio));
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