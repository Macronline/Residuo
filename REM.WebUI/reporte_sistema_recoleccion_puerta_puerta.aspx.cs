using Newtonsoft.Json.Linq;
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

public partial class reporte_sistema_recoleccion_puerta_puerta : System.Web.UI.Page
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
            if (!DateTime.TryParse(sFechaDesde, out dtDesde))
            {
                Lista.Add("errorvalidacion");
                Lista.Add(string.Format("Ingrese fecha correcta Desde {0}", sFechaDesde));
                return scriptSerializer.Serialize(Lista.ToArray());
            }
            DateTime dtHasta;
            if (!DateTime.TryParse(sFechaHasta, out dtHasta))
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
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Dia"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Inicio"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "Termino"));
            lista.Add(new ColumnaHtml(eeTipoColumnaHtml.Label, "", "KG_Descargados"));
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
    public static string WLeerDatos(string sIdCamion, string sFechaDesde, string sFechaHasta, string sIdRuta, string sIdOT)
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

            Ot ot = new Ot(usr);
            ot.Read(sIdOT);
            sIdCamion = ot.Datos.Rows[0]["IdCamion"].ToString();
            VehRecolector veh = new VehRecolector(usr);
            veh.Read(sIdCamion);

            DateTime dtIni;
            if (!DateTime.TryParse(ot.Datos.Rows[0]["Fecha"].ToString(), out dtIni))
            {
                Lista.Clear();
                Lista.Add("Error");
                Lista.Add("Fecha OT no válida");
                return scriptSerializer.Serialize(Lista.ToArray());
            }
            if (ot.Datos.Rows[0]["Hora"].ToString().Length < 5)
            {
                Lista.Clear();
                Lista.Add("Error");
                Lista.Add("Hora OT no válida");
                return scriptSerializer.Serialize(Lista.ToArray());
            }
            
            int Hora = 0; int Minuto = 0; int Segundo = 0;
            if (ot.Datos.Rows[0]["Hora"].ToString().Length == 5)
            {
                if (! int.TryParse( ot.Datos.Rows[0]["Hora"].ToString().Substring(0, 2), out Hora ))
                {
                    Lista.Clear();
                    Lista.Add("Error");
                    Lista.Add("Hora de OT no válida");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
                if (!int.TryParse(ot.Datos.Rows[0]["Hora"].ToString().Substring(3, 2), out Minuto))
                {
                    Lista.Clear();
                    Lista.Add("Error");
                    Lista.Add("Hora de OT no válida");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
            }

            if (ot.Datos.Rows[0]["Hora"].ToString().Length == 8)
            {
                if (!int.TryParse(ot.Datos.Rows[0]["Hora"].ToString().Substring(0, 2), out Hora))
                {
                    Lista.Clear();
                    Lista.Add("Error");
                    Lista.Add("Hora de OT no válida");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
                if (!int.TryParse(ot.Datos.Rows[0]["Hora"].ToString().Substring(3, 2), out Minuto))
                {
                    Lista.Clear();
                    Lista.Add("Error");
                    Lista.Add("Hora de OT no válida");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
                if (!int.TryParse(ot.Datos.Rows[0]["Hora"].ToString().Substring(6, 2), out Segundo))
                {
                    Lista.Clear();
                    Lista.Add("Error");
                    Lista.Add("Hora de OT no válida");
                    return scriptSerializer.Serialize(Lista.ToArray());
                }
            }

            dtIni = dtIni.Add(new TimeSpan(Hora, Minuto, Segundo));
            // ot.Datos.Rows[0]["Hora"].ToString()

            //Final del dia
            DateTime dtFin;
            if (!DateTime.TryParse(ot.Datos.Rows[0]["Fecha"].ToString(), out dtFin))
            {
                Lista.Clear();
                Lista.Add("Error");
                Lista.Add("Fecha Hasta no válida");
                return scriptSerializer.Serialize(Lista.ToArray());
            }
            dtFin = dtFin.Add(new TimeSpan(23, 59, 0));

            //Se lee info del Camion desde la data de Fleetup
            //Geocerca, Fecha, Hora Partida o Paso, Ruta, Camion, Chofer, 
            //fence-names
            ///fencealerts/geo-fencealerts

            //Se lee la Ruta
            string nomRutaAux = "";
            sIdRuta = ot.Datos.Rows[0]["IdRuta"].ToString();
            rutas.ReadById(sIdRuta);
            nomRutaAux = rutas.Datos.Rows[0]["NombreRuta"].ToString();

            string nomCamionAux = "";
            camiones.Read(sIdCamion);
            nomCamionAux = camiones.Datos.Rows[0]["NombreCamion"].ToString() + "-" + camiones.Datos.Rows[0]["NombreTagPatente"].ToString();

            string htmlTemp = File.ReadAllText(HttpContext.Current.Server.MapPath("~/templates") + "/template_paso_a_paso_otv2.html");
            string html = "";
            
            double TimeIni = UtilWeb.DateTimeToUnixTimestamp(dtIni);
            double TimeFin = UtilWeb.DateTimeToUnixTimestamp(dtFin);

            string devId = UtilWeb.GetDatoSingular("NombreTagPatente", veh.Datos);  //"213NW2019000039"

            Wialon wialon = new Wialon();

            string urlMapa = "";
            if (wialon.GetExecuteReport(TimeIni, TimeFin, devId, HttpContext.Current.Server.MapPath("~/images"), ref urlMapa))
            {
                if (wialon.Datos != null)
                { 
                    double RegsPorPage = 10;
                    for (int i = 0; i < wialon.Datos.Rows.Count; i++)
                    {
                        ////////JToken Jtoken = arrayAlertas[i];
                        //////string NombreGeoCercaAux = dataFleet.Datos.Rows[i]["fenceNameFleetup"].ToString();
                        //////string acconTimeAux = dataFleet.Datos.Rows[i]["acconTimeFleetup"].ToString();
                        ////////DateTime acconTimeAuxDateTime = new DateTime(acconTimeAux.Substring(0, 4), acconTimeAux.Substring(0, 4))
                        //////string tmTimeAux = dataFleet.Datos.Rows[i]["tmTimeFleetup"].ToString();

                        html += htmlTemp;
                        html = html.Replace("[NUM_INDEX_REG]", (i + 1).ToString());
                        html = html.Replace("[NOMBRE_GEOCERCA]", wialon.Datos.Rows[i]["zone_name"].ToString());
                        html = html.Replace("[DIA_ALERTA_PASO_POR_GEOCERCA]", wialon.Datos.Rows[i]["time_begin"].ToString());
                        html = html.Replace("[HORA_ALERTA_PASO_POR_GEOCERCA]", wialon.Datos.Rows[i]["time_end"].ToString());
                        html = html.Replace("[NOMBRE_RUTA]", nomRutaAux);
                        html = html.Replace("[NOMBRE_CHOFER]", ot.Datos.Rows[0]["NombreChoferes"].ToString());
                        html = html.Replace("[NOMBRE_CAMION] - [PATENTE_CAMION]", nomCamionAux);
                        html = html.Replace("[URL_MAPA]", urlMapa);
                    
                        //if (i > RegsPorPage)
                        //{
                        //    break;
                        //}
                    }
                }
                Lista.Add(html);
                Lista.Add(urlMapa);
            }


            //FleetUpRecordVehPasoGeoCerca dataFleet = new FleetUpRecordVehPasoGeoCerca(usr);
            //dataFleet.ReadAll(sIdCamion, dtIni, dtFin.AddDays(1));

        

        ////Fleetup fleet = new Fleetup(usr);
        ////string strGeo = fleet.LeerGeoCercas();  //Se leen definicion de GeoCercas 
        ////string devId = UtilWeb.GetDatoSingular("Fleetup_devId", veh.Datos);  //"213NW2019000039"
        ////string strGeoAlertasJSON = fleet.LeerAlertasGeoCercas(devId, new DateTime(2020, 6, 16));
        ////JObject rootObject = JObject.Parse(strGeoAlertasJSON);
        ////JArray arrayAlertas = (JArray)rootObject["data"];
        
        //Ot ot = new Ot(usr);
        //ot.Read(sIdOT);

        //double RegsPorPage = 30;
        //for (int i = 0; i < dataFleet.Datos.Rows.Count; i++)
        //{
        //    //JToken Jtoken = arrayAlertas[i];
        //    string NombreGeoCercaAux = dataFleet.Datos.Rows[i]["fenceNameFleetup"].ToString();
        //    string acconTimeAux = dataFleet.Datos.Rows[i]["acconTimeFleetup"].ToString();
        //    //DateTime acconTimeAuxDateTime = new DateTime(acconTimeAux.Substring(0, 4), acconTimeAux.Substring(0, 4))
        //    string tmTimeAux = dataFleet.Datos.Rows[i]["tmTimeFleetup"].ToString();

        //    html += htmlTemp;
        //    html = html.Replace("[NUM_INDEX_REG]", (i + 1).ToString());
        //    html = html.Replace("[NOMBRE_GEOCERCA]", NombreGeoCercaAux);
        //    html = html.Replace("[DIA_ALERTA_PASO_POR_GEOCERCA]", acconTimeAux);
        //    html = html.Replace("[HORA_ALERTA_PASO_POR_GEOCERCA]", tmTimeAux);
        //    html = html.Replace("[NOMBRE_RUTA]", nomRutaAux);
        //    html = html.Replace("[NOMBRE_CHOFER]", ot.Datos.Rows[0]["NombreChoferes"].ToString());
        //    html = html.Replace("[NOMBRE_CAMION] - [PATENTE_CAMION]", nomCamionAux);
        //    if (i > RegsPorPage)
        //    {
        //        break;
        //    }
        //}

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
    public static string WLeerDatosInicialesOLD_MFP(string sIdCamion, string sFechaDesde, string sIdRuta, string sIdOT)
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

            VehRecolector veh = new VehRecolector(usr);
            veh.Read(sIdCamion);

            //Se lee info del Camion desde Fleetup
            //Geocerca, Fecha, Hora Partida o Paso, Ruta, Camion, Chofer, 
            //fence-names
            ///fencealerts/geo-fencealerts

            string htmlTemp = File.ReadAllText(HttpContext.Current.Server.MapPath("~/templates") + "/template_paso_a_paso_ot.html");
            string html = "";
            Fleetup fleet = new Fleetup(usr);

            string strGeo = fleet.LeerGeoCercas();  //Se leen definicion de GeoCercas 

            string devId = UtilWeb.GetDatoSingular("Fleetup_devId", veh.Datos);  //"213NW2019000039"
            string strGeoAlertasJSON = fleet.LeerAlertasGeoCercas(devId, new DateTime(2020, 6, 16));
            JObject rootObject = JObject.Parse(strGeoAlertasJSON);
            JArray arrayAlertas = (JArray)rootObject["data"];

            string nomRutaAux = "";
            rutas.Read(sIdRuta);
            nomRutaAux = rutas.Datos.Rows[0]["NombreRuta"].ToString();

            string nomCamionAux = "";
            camiones.Read(sIdCamion);
            nomCamionAux = camiones.Datos.Rows[0]["NombreCamion"].ToString() + "-" + camiones.Datos.Rows[0]["NombreTagPatente"].ToString();

            Ot ot = new Ot(usr);
            ot.Read(sIdOT);

            for (int i = 0; i < arrayAlertas.Count; i++)
            {
                JToken Jtoken = arrayAlertas[i];
                string NombreGeoCercaAux= Jtoken["fenceName"].ToString();
                string acconTimeAux = Jtoken["acconTime"].ToString();
                //DateTime acconTimeAuxDateTime = new DateTime(acconTimeAux.Substring(0, 4), acconTimeAux.Substring(0, 4))
                string tmTimeAux = Jtoken["tmTime"].ToString();

                html += htmlTemp;
                html = html.Replace("[NUM_INDEX_REG]", (i + 1).ToString());
                html = html.Replace("[NOMBRE_GEOCERCA]", NombreGeoCercaAux);
                html = html.Replace("[DIA_ALERTA_PASO_POR_GEOCERCA]", acconTimeAux);
                html = html.Replace("[HORA_ALERTA_PASO_POR_GEOCERCA]", tmTimeAux);
                html = html.Replace("[NOMBRE_RUTA]", nomRutaAux);
                html = html.Replace("[NOMBRE_CHOFER]", ot.Datos.Rows[0]["NombreChoferes"].ToString());
                html = html.Replace("[NOMBRE_CAMION] - [PATENTE_CAMION]", nomCamionAux);
                
            }
            
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

}