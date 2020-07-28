using Newtonsoft.Json.Linq;
using REM.BC2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestPlugin : System.Web.UI.Page
{
    public static DateTime UnixTimestampToDateTime(double unixTime)
    {
        DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
        return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
    }
    public static double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
        return (double)unixTimeStampInTicks / TimeSpan.TicksPerSecond;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            txtFecIni.Text = string.Format("{0}-{1}-{2}", DateTime.Now.Day.ToString("00"),
               DateTime.Now.Month.ToString("00"), DateTime.Now.Year.ToString("00"));
            txtFecIniHora.Text = string.Format("{0}:{1}:{2}", "08", "00", "00");

            DateTime Now = UtilWeb.GetDateNow();

            txtFecFin.Text = string.Format("{0}-{1}-{2}", DateTime.Now.Day.ToString("00"),
                DateTime.Now.Month.ToString("00"), DateTime.Now.Year.ToString("00"));
            txtFecFinHora.Text = string.Format("{0}:{1}:{2}", Now.Hour.ToString("00"),
                Now.Minute.ToString("00"), Now.Second.ToString("00"));

            //txtFecIni.Text = string.Format("{0}-{1}-{2}", DateTime.Now.Day.ToString("00"),
            //    DateTime.Now.Month.ToString("00"), DateTime.Now.Year.ToString("00"));
            //txtFecIniHora.Text = string.Format("{0}:{1}:{2}", "08", "00", "00");

            //txtFecFin.Text = string.Format("{0}-{1}-{2}", DateTime.Now.Day.ToString("00"),
            //    DateTime.Now.Month.ToString("00"), DateTime.Now.Year.ToString("00"));
            //txtFecFinHora.Text = string.Format("{0}:{1}:{2}", DateTime.Now.Hour.ToString("00"),
            //    DateTime.Now.Minute.ToString("00"), DateTime.Now.Second.ToString("00"));

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        string urlGetToken = "https://hosting.wialon.com/login.html";
        string urlTokenLoginForGetId = ""; // "https://hosting.wialon.com/wialon/ajax.html?svc=token/login&params={'token':'86fe6d5c99206c57cb445dd5e83b3c71F90F21B1A3DAF982449802E5D1EE3BA5FD2048E1'}";
        urlTokenLoginForGetId = @"https://hst-api.wialon.com/wialon/ajax.html?svc=token/login&params={""" + "token" + @""":""" + @"86fe6d5c99206c57cb445dd5e83b3c71ACE4B0A51A70875BF0D576FE47F3E444E96EC381""" + "}";
        string contentJSON = ""; string contentJSONError = ""; string xmlString = ""; string contentJSONGraboImagen = "";
        try
        {
            //////////DateTime dt2 = UnixTimestampToDateTime(1357938000);
            //////////double reversa = DateTimeToUnixTimestamp(dt2);
            //////////dt2 = UnixTimestampToDateTime(1358715599);
            //////////reversa = DateTimeToUnixTimestamp(dt2);
            DateTime dtIni; DateTime dtFin;
            if (!DateTime.TryParse(txtFecIni.Text + " " + txtFecIniHora.Text, out dtIni))
            {
                Response.Write("Error en Fecha/Hora Inicio ");
                return;
            }
            if (!DateTime.TryParse(txtFecFin.Text + " " + txtFecFinHora.Text, out dtFin))
            {
                Response.Write("Error en Fecha/Hora Final ");
                return;
            }

            double TimeIni = DateTimeToUnixTimestamp(dtIni);
            double TimeFin = DateTimeToUnixTimestamp(dtFin);

            ////PARA OBTENER TOKEN
            //urlGetToken = urlGetToken + "?client_id=MACRONLINE&redirect_uri=http://localhost:11523/TestPlugin.aspx";
            //Response.Redirect(urlGetToken, true);

            //PARA LUEGO OBTENER ID
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(urlTokenLoginForGetId);
            http.Method = "GET";
            WebResponse response = http.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            contentJSON = sr.ReadToEnd();

            string sidsession = @"""" + "eid" + @""":";
            sidsession = contentJSON.Substring(contentJSON.IndexOf(sidsession) + sidsession.Length + 1, 32);

            string urlCleanReport = "https://hst-api.wialon.com/wialon/ajax.html?svc=report/cleanup_result&params={}&sid=[SID_VIALON]";
            urlCleanReport = urlCleanReport.Replace("[SID_VIALON]", sidsession);
            http = (HttpWebRequest)WebRequest.Create(urlCleanReport);
            http.Method = "GET";
            response = http.GetResponse();
            stream = response.GetResponseStream();
            sr = new StreamReader(stream);
            contentJSON = sr.ReadToEnd();

            string urlExecReport = "https://hst-api.wialon.com/wialon/ajax.html?svc=report/exec_report&" +
                                "params={" +
                                            @"""reportResourceId""" + ":21174665, " +
                                    @"""reportTemplateId""" + ":1, " +
                                    @"""reportObjectId""" + ":[Id_Vehiculo], " +
                                    @"""reportObjectSecId""" + ":0, " +
                                    @"""interval""" + ":{ " +
                                                @"""from""" + ":" + TimeIni.ToString() + ", " +
                                                @"""to""" + ":" + TimeFin.ToString() + ", " +
                                                @"""flags""" + ":0 " +
                                                        "}" +
                                        "}" +
                                        "&sid=[SID_VIALON]";
            urlExecReport = urlExecReport.Replace("[SID_VIALON]", sidsession);
            urlExecReport = urlExecReport.Replace("[Id_Vehiculo]", ddlCamiones.SelectedValue);
            http = (HttpWebRequest)WebRequest.Create(urlExecReport);
            http.Method = "GET";
            response = http.GetResponse();
            stream = response.GetResponseStream();
            sr = new StreamReader(stream);
            contentJSON = sr.ReadToEnd();

            JObject rootObject = JObject.Parse(contentJSON);
            JToken tablas = ((Newtonsoft.Json.Linq.JProperty)rootObject.First).Value.SelectToken("tables");
            if (tablas.HasValues)
            {
                //Se pide la tabla
                string urlGetRowsTable = "https://hst-api.wialon.com/wialon/ajax.html?svc=report/get_result_rows&" +
                    @"params={""" + "tableIndex" + @""":0, " +
                   @"""indexFrom""" + @":0, " +
                   @"""indexTo""" + @":30}&sid=[SID_VIALON]";
                urlGetRowsTable = urlGetRowsTable.Replace("[SID_VIALON]", sidsession);
                urlGetRowsTable = urlGetRowsTable.Replace("[Id_Vehiculo]", ddlCamiones.SelectedValue);
                http = (HttpWebRequest)WebRequest.Create(urlGetRowsTable);
                http.Method = "GET";
                response = http.GetResponse();
                stream = response.GetResponseStream();
                sr = new StreamReader(stream);
                contentJSON = sr.ReadToEnd();
                JArray arrayFilas = JArray.Parse(contentJSON);
                DataTable Datos = new DataTable();
                Datos.Columns.Add("zone_name"); Datos.Columns.Add("zone_type"); Datos.Columns.Add("zone_area");
                Datos.Columns.Add("time_begin"); Datos.Columns.Add("time_end"); Datos.Columns.Add("duration_in");
                Datos.Columns.Add("duration_ival");
                for (int p = 0; p < arrayFilas.Count; p++)
                {
                    JObject rss2 = (JObject) arrayFilas[p];
                    
                    DataRow DR = Datos.NewRow();
                    DR["zone_name"] = rss2["c"][0].ToString();
                    DR["zone_type"] = rss2["c"][1].ToString();
                    DR["zone_area"] = rss2["c"][2].ToString();

                    JObject rss3 = (JObject)rss2["c"][3];
                    DR["time_begin"] = rss3["t"].ToString();

                    rss3 = (JObject)rss2["c"][4];
                    DR["time_end"] = rss3["t"].ToString();

                    DR["duration_in"] = rss2["c"][5].ToString();
                    DR["duration_ival"] = rss2["c"][6].ToString();
                }    
                
                //for (int p = 0; p < tablas.Children; p++)
                //{
                //    JToken Jtoken = arrayAlertas[p];

                //}
            }
            string urlget_result_chart = "https://hst-api.wialon.com/wialon/ajax.html?svc=report/get_result_chart&" +
                                    @"params={""" + "attachmentIndex" + @""":0, " +
                                            @"""action" + @""":0,  " +
                                            @"""width" + @""":500,  " +
                                            @"""height" + @""":500, " +
                                            @"""autoScaleY" + @""":0, " +
                                            @"""pixelFrom" + @""":100, " +
                                            @"""pixelTo" + @""":100, " +
                                            @"""flags" + @""":1}" +
                                        "&sid=[SID_VIALON]";
            urlget_result_chart = urlget_result_chart.Replace("[SID_VIALON]", sidsession);
            http = (HttpWebRequest)WebRequest.Create(urlget_result_chart);
            http.Method = "GET";
            response = http.GetResponse();
            stream = response.GetResponseStream();
            //System.IO.Stream responseStream = response.GetResponseStream();

            try
            {
                Bitmap bitmap2 = new Bitmap(stream);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(Server.MapPath("~/images") + @"\png.png", FileMode.Create, FileAccess.ReadWrite))
                    {
                        bitmap2.Save(memory, ImageFormat.Png);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                //bitmap2.Save(Server.MapPath("~/images") + @"\png.png", System.Drawing.Imaging.ImageFormat.Png);                
                contentJSONGraboImagen = "S";
            }
            catch (Exception ex)
            {
                contentJSONGraboImagen = "N";
                contentJSONError = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            sr = new StreamReader(stream);
            contentJSON = sr.ReadToEnd();

            string xml = @"https://hst-api.wialon.com/wialon/ajax.html?svc=report/get_result_map&params={""" + "width" + @""":500," +
                @"""height" + @""":500}&sid=[SID_VIALON]";
            xml = xml.Replace("[SID_VIALON]", sidsession);
            http = (HttpWebRequest)WebRequest.Create(xml);
            http.Method = "GET";
            response = http.GetResponse();
            stream = response.GetResponseStream();
            try
            {
                Bitmap bitmap3 = new Bitmap(stream);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(Server.MapPath("~/images") + @"\map.png", FileMode.Create, FileAccess.ReadWrite))
                    {
                        bitmap3.Save(memory, ImageFormat.Png);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                //bitmap2.Save(Server.MapPath("~/images") + @"\map.png", System.Drawing.Imaging.ImageFormat.Png);
                contentJSONGraboImagen += "S";
            }
            catch (Exception ex)
            {
                contentJSONGraboImagen += "N";
                contentJSONError += ex.Message + Environment.NewLine + ex.StackTrace;
                sr = new StreamReader(stream);
                contentJSON = sr.ReadToEnd();
            }
            //string xml = "https://hst-api.wialon.com/wialon/ajax.html?svc=report/get_result_map&params={"width":600,"height":600"}&sid=<your_sid>}";
            //string xml = "https://hst-api.wialon.com/wialon/ajax.html?svc=report/get_result_map&params={"width":600,"height":600"}&sid =< your_sid >}";

        }
        catch (Exception ex)
        {
            //   throw ex;
            contentJSONError += string.Format("{0}.{1}", ex.Message, ex.StackTrace);
        }

        Response.Write(string.Format("contentJSON: {0}. contentJSONError:{1}. contentJSONGraboImagen: {2}",
                        contentJSON, contentJSONError, contentJSONGraboImagen));

        Image1.ImageUrl = "images/png.png";
        Image2.ImageUrl = "images/map.png";
    }

    protected void Page_Load_FLEETUP(object sender, EventArgs e)
    {


        string urlGetToken = "https://api.fleetup.net/token?acctId={0}&secret={1}";
        string acctId = "21563";
        string secret = "ibi2d5eqabmbxptutvnt803ba54f1u77";
        string api_key = "u6KbPUKA2w1mzUvg0b2cn5A37wsg8Ecn8iKKGQhL";
        string contentJSON = ""; string xmlString = "";
        try
        {
            //https://api.fleetup.net/token?acctId=21563&secret=ibi2d5eqabmbxptutvnt803ba54f1u77
            urlGetToken = string.Format(urlGetToken, acctId, secret);
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(urlGetToken);
            http.Method = "GET";
            //string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            //http.Headers.Add("Authorization", "Basic " + encoded);
            ////http.Credentials = new NetworkCredential("McmTest", "Mcm2019$");
            //http.Credentials = GetCredential(url, username, password);
            http.Headers.Add("x-api-key", api_key);
            WebResponse response = http.GetResponse();
            Stream stream = response.GetResponseStream();
            //MemoryStream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            contentJSON = sr.ReadToEnd();


            //GET DEVICES
            string urlDeviceLastLocation = "https://api.fleetup.net/gpsdata/device-last-location";

            string urlGetDevices = "https://api.fleetup.net/devices";
            var details = JObject.Parse(contentJSON);
            string token = details["token"].ToString();
            //http = (HttpWebRequest)WebRequest.Create(urlDeviceLastLocation);
            http = (HttpWebRequest)WebRequest.Create(urlGetDevices);
            http.Method = "POST";
            http.Headers.Add("x-api-key", api_key);
            http.Headers.Add("token", token);
            StreamWriter writer = new StreamWriter(http.GetRequestStream());
            //string xmlString = "{'{" + acctId + "}': 'string'}";
            //string xmlString = "{'" + acctId + "'}";
            //string xmlString = "{" + acctId + "}";
            //xmlString = @"{{""" + "acctId" + @""": """ + "21563" + @"""}}";
            xmlString = @"{""" + "acctId" + @""": """ + acctId + @"""}}";
            writer.Write(xmlString);
            writer.Close();
            // Send the data to the webserver
            WebResponse rsp = http.GetResponse();
            Stream streamnew = rsp.GetResponseStream();
            //MemoryStream stream = response.GetResponseStream();
            StreamReader sr2 = new StreamReader(streamnew);
            string contentResp = sr2.ReadToEnd();

            //MAPS
            string urlMap = "https://api.fleetup.net/report/get_result_mapdsdsds";
            http = (HttpWebRequest)WebRequest.Create(urlMap);
            http.Method = "POST";
            http.Headers.Add("x-api-key", api_key);
            http.Headers.Add("token", token);
            writer = new StreamWriter(http.GetRequestStream());
            //string xmlString = "{'{" + acctId + "}': 'string'}";
            //string xmlString = "{'" + acctId + "'}";
            //string xmlString = "{" + acctId + "}";
            //xmlString = @"{{""" + "acctId" + @""": """ + "21563" + @"""}}";
            xmlString = @"{""" + "width" + @""": """ + "500" + @""", """ + "height" + @": """ + "500" + @"""}";

            writer.Write(xmlString);
            writer.Close();
            // Send the data to the webserver
            rsp = http.GetResponse();
            streamnew = rsp.GetResponseStream();
            //MemoryStream stream = response.GetResponseStream();
            sr2 = new StreamReader(streamnew);
            contentResp = sr2.ReadToEnd();

            //response = http.GetResponse();
            //stream = response.GetResponseStream();
            //MemoryStream stream = response.GetResponseStream();
            //sr = new StreamReader(stream);
            //contentJSON = sr.ReadToEnd();

        }
        catch (Exception ex)
        {
            //   throw ex;
            contentJSON = string.Format("{0}.{1}", ex.Message, ex.StackTrace);
        }

        Response.Write(contentJSON);
        ////////string urlGetToken = "https://api.fleetup.net/token?acctId={0}&secret={1}";
        ////////string urlGetDevices = "https://api.fleetup.net/devices?token={0}&acctId={1}";

        ////////string acctId = "21563";
        ////////string secret = "ibi2d5eqabmbxptutvnt803ba54f1u77";
        ////////string api_key = "u6KbPUKA2w1mzUvg0b2cn5A37wsg8Ecn8iKKGQhL";

        ////////string contentJSON = "";
        ////////try
        ////////{
        ////////    //https://api.fleetup.net/token?acctId=21563&secret=ibi2d5eqabmbxptutvnt803ba54f1u77
        ////////    urlGetToken = string.Format(urlGetToken, acctId, secret);

        ////////    HttpWebRequest http = (HttpWebRequest)WebRequest.Create(urlGetToken);
        ////////    http.Method = "GET";
        ////////    //string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
        ////////    //http.Headers.Add("Authorization", "Basic " + encoded);
        ////////    ////http.Credentials = new NetworkCredential("McmTest", "Mcm2019$");
        ////////    //http.Credentials = GetCredential(url, username, password);
        ////////    http.Headers.Add("x-api-key", api_key);
        ////////    WebResponse response = http.GetResponse();
        ////////    Stream stream = response.GetResponseStream();
        ////////    //MemoryStream stream = response.GetResponseStream();
        ////////    StreamReader sr = new StreamReader(stream);
        ////////    contentJSON = sr.ReadToEnd();
        ////////    var details = JObject.Parse(contentJSON);
        ////////    string token = details["token"].ToString();


        ////////    //DEVICES
        ////////    urlGetDevices = string.Format(urlGetDevices, token, acctId);
        ////////    http = (HttpWebRequest)WebRequest.Create(urlGetDevices);
        ////////    http.Method = "POST";
        ////////    //string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
        ////////    //http.Headers.Add("Authorization", "Basic " + encoded);
        ////////    ////http.Credentials = new NetworkCredential("McmTest", "Mcm2019$");
        ////////    //http.Credentials = GetCredential(url, username, password);
        ////////    http.Headers.Add("x-api-key", api_key);
        ////////    response = http.GetResponse();
        ////////    stream = response.GetResponseStream();
        ////////    //MemoryStream stream = response.GetResponseStream();
        ////////    sr = new StreamReader(stream);
        ////////    contentJSON = sr.ReadToEnd();

        ////////    //MAPS
        ////////    string urlMap = "https://api.fleetup.net/report/get_result_map";
        ////////    http = (HttpWebRequest)WebRequest.Create(urlMap);
        ////////    http.Method = "POST";
        ////////    http.Headers.Add("x-api-key", api_key);
        ////////    response = http.GetResponse();
        ////////    stream = response.GetResponseStream();
        ////////    sr = new StreamReader(stream);
        ////////    contentJSON = sr.ReadToEnd();
        ////////    contentJSON = contentJSON;
        ////////}
        ////////catch (Exception ex)
        ////////{
        ////////    //   throw ex;
        ////////    contentJSON = string.Format("{0}.{1}", ex.Message, ex.StackTrace);
        ////////}

        ////////Response.Write(contentJSON);
    }


}