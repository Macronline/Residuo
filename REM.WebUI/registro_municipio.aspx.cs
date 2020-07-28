using REM.BC2;
using REM.BC2.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registro_municipio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private static void WLeerUsuarioConectadoAndMunicipios_Internal(Usuario usr, ref string html)
    {
        ClienteMunicipio cliente = new ClienteMunicipio(usr);
        cliente.ReadAll();

        //Se genera la tabla HTML
        List<ColumnaHtml> lista = new List<ColumnaHtml>();
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "NombreUserMunic"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "RutUserMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "CorreoUserMunic"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));
        //lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Telefono"));  //Aca va IMAGEN
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageEdit, "Id", "", "Edit", "EditMunicipio"));
        lista.Add(new ColumnaHtml(eeTipoColumnaHtml.ImageDelete, "Id", "", "Delete", "DeleteMunicipio"));

        UtilWeb util = new UtilWeb();
        html = util.GetHtmlTableBasica(cliente.Datos, lista);

    }
    [WebMethod]
    public static string WLeerUsuarioConectadoAndMunicipios()
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            string html = "";
            WLeerUsuarioConectadoAndMunicipios_Internal(usr, ref html);
            
            Lista.Add("Exito");
            Lista.Add(usr.NombreLargo);
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
    public static string WGrabarMunicipio(string sId, string sNombreMunic, string sRutMunic, string sNombreUserMunic, string sRutUserMunic, string sCorreoUserMunic,
        string sClave, string sClave2, string sTelefono, string sLogo)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            if (sId.Trim().Length != 0)
            { 
                if (sClave != sClave2)
                {
                    Lista.Add("CSD");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }

                if (sCorreoUserMunic == "")
                {
                    Lista.Add("CORREO_NO");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
                bool res = UtilWeb.IsValidMail(sCorreoUserMunic);
                if (! res)
                {
                    Lista.Add("CORREO_NO");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }


                if (sRutMunic == "") 
                {
                    Lista.Add("RUT_NO");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
                if (! UtilWeb.IsValidRut(sRutMunic))
                {
                    Lista.Add("RUT_NO");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }

                if (sRutUserMunic == "")
                {
                    Lista.Add("RUT_USER_NO");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
                if (! UtilWeb.IsValidRut(sRutUserMunic))
                {
                    Lista.Add("RUT_USER_NO");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
            }

            ClienteMunicipio cliente = new ClienteMunicipio(usr);
            if ( sId.Trim().Length == 0)
                //cliente.Create(sNombreMunic, sRutMunic, sNombreUserMunic, sRutUserMunic, sCorreoUserMunic, sClave, sTelefono);
                 sId = cliente.Create(sNombreMunic, sRutMunic, sNombreUserMunic, sRutUserMunic, sCorreoUserMunic, sClave, sTelefono).ToString();
            else
                cliente.Update(sId, sNombreMunic, sRutMunic, sNombreUserMunic, sRutUserMunic, sCorreoUserMunic, sClave, sTelefono);


            if (sLogo.Trim().Length != 0 && !sLogo.Contains("image-placeholder.png"))
            {
                //data:image/png;base64,djsjsadkjadjkhadfkadkfakhdfkjhadkjh
                sLogo = sLogo.Substring(sLogo.IndexOf("base64,") + "base64,".Length);
                byte[] bytes = Convert.FromBase64String(sLogo);
                File.WriteAllBytes(string.Format(HttpContext.Current.Server.MapPath("~/images") + "\\MunLog_{0}.jpg", sId), bytes);
            }


            string html = "";
            WLeerUsuarioConectadoAndMunicipios_Internal(usr, ref html);

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
    public static string WLeerParaEditarMunicipio(string Id)
    {
        JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
        List<string> Lista = new List<string>();
        Usuario usr = new Usuario();

        if (!UtilWeb.CheckSession(Lista, ref usr, HttpContext.Current))
            return scriptSerializer.Serialize(Lista.ToArray());

        try
        {
            ClienteMunicipio cliente = new ClienteMunicipio(usr);
            cliente.Read(Id);

            Lista.Add("Exito");
            Lista.Add(cliente.Datos.Rows[0]["NombreMunic"].ToString());
            Lista.Add(cliente.Datos.Rows[0]["RutMunic"].ToString());
            Lista.Add(cliente.Datos.Rows[0]["NombreUserMunic"].ToString());
            Lista.Add(cliente.Datos.Rows[0]["RutUserMunic"].ToString());
            Lista.Add(cliente.Datos.Rows[0]["CorreoUserMunic"].ToString());
            Lista.Add(cliente.Datos.Rows[0]["Telefono"].ToString());

            if (File.Exists(HttpContext.Current.Server.MapPath("images") + string.Format(@"\MunLog_{0}.jpg", Id)))
                Lista.Add(string.Format("/images/MunLog_{0}.jpg", Id));
            else
                Lista.Add("/images/demo/image-placeholder.png");

            Lista.Add(cliente.Datos.Rows[0]["Clave"].ToString());
            Lista.Add(cliente.Datos.Rows[0]["Clave"].ToString());
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
    public static string WEliminarMunicipio(string Id)
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
                ClienteMunicipio cliente = new ClienteMunicipio(usr);
                cliente.Delete(Id);

                string html = "";
                WLeerUsuarioConectadoAndMunicipios_Internal(usr, ref html);

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