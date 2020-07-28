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

public partial class registro_geocercas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private static void WLeerGeocerca_Internal(Usuario usr, ref string html, string IdMunicipio)
    {
        Geocercas geo = new Geocercas(usr);
        //geo.ReadAll();
        if (IdMunicipio.Trim().Length == 0)
            geo.ReadAll_con_Municipio();
        else
            geo.ReadAll_Geocerca_del_Municipio(IdMunicipio);

        //Se genera la tabla HTML
        List<ColumnaHtml> lista = new List<ColumnaHtml>();
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreGeocerca"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Ubicacion"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutUserMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Coordenadas"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Observacion"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));  //Aca va IMAGEN
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditGeocerca"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteGeocerca"));

        UtilWeb util = new UtilWeb();
        html = util.GetHtmlTableBasica(geo.Datos, lista);

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
            ClienteMunicipio mun = new ClienteMunicipio(usr);
            mun.ReadAll();
            UtilWeb.AddElementSeleccioneALista(mun.Datos);
            UtilWeb util = new UtilWeb();
            string jsonMun = util.GetJson(mun.Datos);
            
            string html = "";
            WLeerGeocerca_Internal(usr, ref html, "");

            Lista.Add("Exito");
            Lista.Add(jsonMun);
            Lista.Add(html);
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
    public static string WLeerGeocercasDelMunicipio(string IdMunicipio)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            Geocercas geo = new Geocercas(usr);
            geo.ReadAll_Geocerca_del_Municipio(IdMunicipio);
            UtilWeb.AddElementSeleccioneALista(geo.Datos);
            UtilWeb util = new UtilWeb();
            string jsonMun = util.GetJson(geo.Datos);

            string html = "";
            WLeerGeocerca_Internal(usr, ref html, IdMunicipio);

            Lista.Add("Exito");
            //Lista.Add(jsonMun);
            Lista.Add(html);
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
    public static string WGrabarGeocerca(string sId, string sNombreGeocerca, string sUbicacion, string sCoordenadas,
        string sObservacion, string sIdClienteMunicipio)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            Geocercas geo = new Geocercas(usr);
            if (sId.Trim().Length == 0)
                geo.Create(sNombreGeocerca, sUbicacion, sCoordenadas, sObservacion, sIdClienteMunicipio);
            else
                geo.Update(sId, sNombreGeocerca, sUbicacion, sCoordenadas, sObservacion, sIdClienteMunicipio);

            string html = "";
            WLeerGeocerca_Internal(usr, ref html, sIdClienteMunicipio);

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
    public static string WLeerParaEditarGeocerca(string Id)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            //ClienteMunicipio cliente = new ClienteMunicipio(usr);
            Geocercas geo = new Geocercas(usr);
            geo.Read(Id);

            //Camiones camion = new Camiones(usr);
            //camion.Read(Id);
            //cliente.Read(Id);

            Lista.Add("Exito");

            Lista.Add(geo.Datos.Rows[0]["NombreGeocerca"].ToString());
            Lista.Add(geo.Datos.Rows[0]["Ubicacion"].ToString());
            Lista.Add(geo.Datos.Rows[0]["Coordenadas"].ToString());
            Lista.Add(geo.Datos.Rows[0]["Observacion"].ToString());
            Lista.Add(geo.Datos.Rows[0]["IdClienteMunicipio"].ToString());
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
    public static string WEliminarGeocerca(string Id)
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
                Geocercas geo = new Geocercas(usr);
                geo.Delete(Id);

                string html = "";
                WLeerGeocerca_Internal(usr, ref html, "");

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