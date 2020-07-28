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

public partial class reporte_informacion_consolidada : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    [WebMethod]
    public static string WConsultarOTs(string sFechaDesde, string sFechaHasta, string sIdRuta, string sIdCamion)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            Ot ot = new Ot(usr);
            usr.ReadMunic();
            string sIdClienteMunicipio = UtilWeb.GetDatoSingular("Id", usr.oClienteMunicipio.Datos);
            DateTime dtDesde;
            if (! DateTime.TryParse(sFechaDesde, out dtDesde))
            {
                Lista.Add("errorvalidacion");
                Lista.Add(string.Format("Ingrese fecha correcta Desde {0}", sFechaDesde));
                return scriptSerializer.Serialize(Lista.ToArray());
            }
            DateTime dtHasta;
            if ( ! DateTime.TryParse(sFechaHasta, out dtHasta))
            {
                Lista.Add("errorvalidacion");
                Lista.Add(string.Format("Ingrese fecha correcta Hasta {0}", sFechaHasta));
                return scriptSerializer.Serialize(Lista.ToArray());
            }
            ot.Buscar(sIdClienteMunicipio, dtDesde, dtHasta, sIdRuta, sIdCamion);

            //Se genera la tabla HTML
            List<ColumnaHtml> lista = new List<ColumnaHtml>();
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "StrDesdeHasta"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreRuta"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Numero"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Dia"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Inicio"));
            //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Termino"));
            //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "KG_Descargados"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreCamion"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreChoferes"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditOt"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteOt"));
            UtilWeb util = new UtilWeb();
            string html = util.GetHtmlTableBasica(ot.Datos, lista);

            Lista.Add("Exito");
            Lista.Add(html);

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

}