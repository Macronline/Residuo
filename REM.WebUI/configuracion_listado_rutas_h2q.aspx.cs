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

public partial class configuracion_listado_rutas_h2q : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    private static void WLeerRutas_Internal(Usuario usr, ref string html)
    {
        Ruta rutas = new Ruta(usr);
        //rutas.ReadAll(1);  //TODO: Esta en duro, dinamizar
        rutas.ReadAll_con_Municipio_y_Geocerca();
        //Se genera la tabla HTML
        List<ColumnaHtml> lista = new List<ColumnaHtml>();
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreRuta"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreGeocerca"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutUserMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Coordenadas"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Observacion"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));  //Aca va IMAGEN
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditRutas"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteRutas"));

        UtilWeb util = new UtilWeb();
        html = util.GetHtmlTableBasica(rutas.Datos, lista);

    }
    private static void WLeerRutas_Internal(Usuario usr, ref string html, string IdMunicipio)
    {
        Ruta rutas = new Ruta(usr);
        rutas.ReadAllByMunicipio(IdMunicipio);

        //Se genera la tabla HTML
        List<ColumnaHtml> lista = new List<ColumnaHtml>();
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMunicipio"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreRuta"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Geocerca"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutUserMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Coordenadas"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Observacion"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));  //Aca va IMAGEN
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditRutas"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteRutas"));

        UtilWeb util = new UtilWeb();
        html = util.GetHtmlTableBasica(rutas.Datos, lista);

    }

    private static void WLeerRutas_Geocerca_Internal(Usuario usr, ref string html, string IdMunicipio)
    {
        Ruta rutas = new Ruta(usr);
        rutas.ReadAllRutasyGeocercas_ByIdMunicipio(IdMunicipio);

        //Se genera la tabla HTML
        List<ColumnaHtml> lista = new List<ColumnaHtml>();
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreRuta"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreGeocerca"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutUserMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Coordenadas"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Observacion"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));  //Aca va IMAGEN
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditRutas"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteRutas"));

        UtilWeb util = new UtilWeb();
        html = util.GetHtmlTableBasica(rutas.Datos, lista);

    }
    [WebMethod]
    public static string WLeerUsuarioConectado()
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            ClienteMunicipio mun = new ClienteMunicipio(usr);
            mun.ReadAll();
            UtilWeb.AddElementSeleccioneALista(mun.Datos);
            UtilWeb util = new UtilWeb();
            string jsonMun = util.GetJson(mun.Datos);

            Geocercas geo = new Geocercas(usr);
            geo.ReadAll();
            UtilWeb.AddElementSeleccioneALista(geo.Datos);
             util = new UtilWeb();
            string jsonGeo = util.GetJson(geo.Datos);


            string html = "";
            WLeerRutas_Internal(usr, ref html);


            Lista.Add("Exito");
            Lista.Add(jsonMun);
            Lista.Add(jsonGeo);
            Lista.Add(html);
            //Lista.Add(usr.NombreLargo);

            //Lista.Add(html);
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
    public static string WLeerRutasyGeocercasDelMunicipio(string IdMunicipio)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            //ClienteMunicipio mun = new ClienteMunicipio(usr);
            //mun.ReadAll();
            //UtilWeb.AddElementSeleccioneALista(mun.Datos);
            //UtilWeb util = new UtilWeb();
            //string jsonMun = util.GetJson(mun.Datos);
            UtilWeb util = new UtilWeb();
            Geocercas geo = new Geocercas(usr);
            geo.ReadAll_Geocerca_del_Municipio(IdMunicipio);
            UtilWeb.AddElementSeleccioneALista(geo.Datos);
            util = new UtilWeb();
            string jsonGeo = util.GetJson(geo.Datos);

            
            //Ruta ruta = new Ruta(usr);
            //ruta.ReadAllRutasyGeocercas_ByIdMunicipio(IdMunicipio);
            //UtilWeb.AddElementSeleccioneALista(ruta.Datos);
            //util = new UtilWeb();
            //string jsonRuta = util.GetJson(ruta.Datos);


            string html = "";
            WLeerRutas_Geocerca_Internal(usr, ref html, IdMunicipio);


            Lista.Add("Exito");
            //Lista.Add(jsonRuta);
            Lista.Add(jsonGeo);
            Lista.Add(html);
            //Lista.Add(usr.NombreLargo);

            //Lista.Add(html);
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
    public static string WGrabarRutas(string sId, string sNombreRuta, string sIdClienteMunicipio, string sIdGeocerca)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            Ruta Rutas = new Ruta(usr);
            // if (sId.Trim().Length == 0 && !(sId == "object HTMLSpanElement"))
            if (sId.Trim().Length == 0 || (sId == "[object HTMLSpanElement]"))
                Rutas.Create(sNombreRuta, sIdClienteMunicipio, sIdGeocerca);
            else
                Rutas.Update(sId, sNombreRuta, sIdClienteMunicipio, sIdGeocerca);

            string html = "";
            //WLeerRutas_Internal(usr, ref html, sIdClienteMunicipio);
            WLeerRutas_Geocerca_Internal(usr, ref html, sIdClienteMunicipio);


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
    public static string WLeerParaEditarRutas(string Id)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            //ClienteMunicipio cliente = new ClienteMunicipio(usr);
            Ruta rutas = new Ruta(usr);
            rutas.Read(Id);

            //Camiones camion = new Camiones(usr);
            //camion.Read(Id);
            //cliente.Read(Id);

            Lista.Add("Exito");

            Lista.Add(rutas.Datos.Rows[0]["NombreRuta"].ToString());
            Lista.Add(rutas.Datos.Rows[0]["IdClienteMunicipio"].ToString());
            Lista.Add(rutas.Datos.Rows[0]["IdGeocerca"].ToString());
            //Lista.Add(geo.Datos.Rows[0]["Observacion"].ToString());
            //Lista.Add(geo.Datos.Rows[0]["NombreMunicipio"].ToString());
            //Lista.Add(camion.Datos.Rows[0]["Telefono"].ToString());

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
    public static string WEliminarRutas(string Id)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            if (Id.Trim().Length != 0)
            {
                Ruta rutas = new Ruta(usr);
                rutas.Delete(Id);

                string html = "";
                WLeerRutas_Internal(usr, ref html);

                Lista.Add("Exito");
                Lista.Add(html);
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

}