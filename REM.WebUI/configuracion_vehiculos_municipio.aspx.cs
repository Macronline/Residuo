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

public partial class configuracion_vehiculos_municipio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private static void WLeerCamionesMunicipios_Internal(Usuario usr, ref string html, string IdMunicipio)
    {
        //Camiones camion = new Camiones(usr);
        //camion.ReadAll();
        VehRecolector camion = new VehRecolector(usr);
        if (IdMunicipio.Trim().Length == 0)
            camion.ReadAll();
        else
            camion.ReadAll_Camion_del_Municipio(IdMunicipio);
        //Se genera la tabla HTML
        List<ColumnaHtml> lista = new List<ColumnaHtml>();
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreCamion"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMarca"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutUserMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreModelo"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreTagPatente"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));  //Aca va IMAGEN
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditCamion"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteCamion"));

        UtilWeb util = new UtilWeb();
        html = util.GetHtmlTableBasica(camion.Datos, lista);

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
                

            string html = "";
            WLeerCamionesMunicipios_Internal(usr, ref html, "");

            Lista.Add("Exito");
            Lista.Add(jsonMun);
            //Lista.Add(usr.NombreLargo);
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
    public static string WGrabarCamion(string sId, string sNombreCamion, string sMarcaCamion, string sModeloCamion, 
        string sTagCamion, string sMunicipio)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            VehRecolector camion = new VehRecolector(usr);
            if (sId.Trim().Length == 0)
                camion.Create(sNombreCamion, sMarcaCamion, sModeloCamion, sTagCamion, sMunicipio);
            else
                camion.Update(sId, sNombreCamion, sMarcaCamion, sModeloCamion, sTagCamion, sMunicipio);

            string html = "";
            WLeerCamionesMunicipios_Internal(usr, ref html, sMunicipio);

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
    public static string WLeeCamionDelMunicipio(string IdMunicipio)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            VehRecolector camion = new VehRecolector(usr);
            camion.ReadAll_Camion_del_Municipio(IdMunicipio);
            UtilWeb.AddElementSeleccioneALista(camion.Datos);
            UtilWeb util = new UtilWeb();
            string jsonMun = util.GetJson(camion.Datos);


            string html = "";
            WLeerCamionesMunicipios_Internal(usr, ref html, IdMunicipio);

            Lista.Add("Exito");
            //Lista.Add(jsonMun);
            //Lista.Add(usr.NombreLargo);
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
    public static string WLeerParaEditarCamion(string Id)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            //ClienteMunicipio cliente = new ClienteMunicipio(usr);
            VehRecolector camion = new VehRecolector(usr);
            camion.Read(Id);
            //cliente.Read(Id);

            Lista.Add("Exito");
            
            Lista.Add(camion.Datos.Rows[0]["NombreCamion"].ToString());
            Lista.Add(camion.Datos.Rows[0]["NombreMarca"].ToString());
            Lista.Add(camion.Datos.Rows[0]["NombreModelo"].ToString());
            Lista.Add(camion.Datos.Rows[0]["NombreTagPatente"].ToString());
            Lista.Add(camion.Datos.Rows[0]["IdClienteMunicipio"].ToString());
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
    public static string WEliminarCamion(string Id)
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
                VehRecolector camion = new VehRecolector(usr);
                camion.Delete(Id);

                string html = "";
                WLeerCamionesMunicipios_Internal(usr, ref html, "");

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