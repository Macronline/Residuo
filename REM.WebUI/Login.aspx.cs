using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using REM.BC2;
using REM.BC2.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    private string GetUserIP()
    {
        string serverVariable = this.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(serverVariable))
            return this.Request.ServerVariables["REMOTE_ADDR"];
        return serverVariable.Split(',')[0];
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (this.txtName.Text.Trim().Length == 0 || this.txtPass.Text.Trim().Length == 0)
            return;

        Usuario usuario = new Usuario();
        string userHostAddress = this.Request.UserHostAddress;
        string userIp = this.GetUserIP();
        bool IsMismIP = false;
        eeResultLogin eeResultLogin = usuario.Login(this.txtName.Text, this.txtPass.Text, "", userHostAddress, userIp, ref IsMismIP);

        switch (eeResultLogin)
        {
            case eeResultLogin.UserNoExiste:
                this.Response.Redirect("UserNoAuthorized.aspx", true);
                break;
            case eeResultLogin.LoginExitoMismaIP:
            case eeResultLogin.LoginExitoDistintaIP:
                if (eeResultLogin == eeResultLogin.LoginExitoDistintaIP)
                {
                    this.Response.Redirect("UserNoAuthorized.aspx", true);
                    break;
                }
                string Token = "";
                string StringOfControl = "";
                string Serial = "";
                string stringSecure = UtilWeb.GenerateStringSecure(usuario.Id, ref StringOfControl, ref Serial, ref Token);
                usuario.CreateSesionHistory(Token, DateTime.Now, userHostAddress, userIp, stringSecure, StringOfControl);
                this.Response.Cookies.Add(new HttpCookie("QS", "s=" + Token)
                {
                    Expires = DateTime.Now.AddMinutes(double.Parse(ConfigurationManager.AppSettings["MinutesSession"].ToString()))
                });

                //Procesa tareas batch
                if (ConfigurationManager.AppSettings["Ejecutar_Carga_Fleetup"].ToString().ToUpper() == "TRUE")
                {
                    FleetUpRecordVehPasoGeoCerca batch = new FleetUpRecordVehPasoGeoCerca(usuario);
                    batch.ProcesarTareasBatchWialon(usuario);
                }

                if (usuario.H2QAccess )
                { 
                    this.Response.Redirect("registro_municipio.html", false);
                    //this.Response.Redirect("RegistroClientes.html", false);
                }
                if (usuario.MunicipioAccess)
                {
                    this.Response.Redirect("municipio_menu.html", false);
                    // this.Response.Redirect("registro_ot_copia.html", false); 
                }
                break;
        }
    }
}