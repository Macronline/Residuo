using Newtonsoft.Json.Linq;
using REM.BC2.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace REM.BC2
{
    public class Wialon: Singular
    {
        public bool GetExecuteReport(double TimeIni, double TimeFin, string IdVehiculoCodWialon, string PathImages, ref string urlMapaResult)
        {
            string contentJSON = "";
            string urlTokenLoginForGetId = ""; // "https://hosting.wialon.com/wialon/ajax.html?svc=token/login&params={'token':'86fe6d5c99206c57cb445dd5e83b3c71F90F21B1A3DAF982449802E5D1EE3BA5FD2048E1'}";
            urlTokenLoginForGetId = @"https://hst-api.wialon.com/wialon/ajax.html?svc=token/login&params={""" + "token" + @""":""" + @"86fe6d5c99206c57cb445dd5e83b3c71ACE4B0A51A70875BF0D576FE47F3E444E96EC381""" + "}";

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
                                    @"""reportTemplateId""" + ":[WIALON_ID_TEMPLATE_INFORME], " +
                                    @"""reportObjectId""" + ":[Id_Vehiculo], " +
                                    @"""reportObjectSecId""" + ":0, " +
                                    @"""interval""" + ":{ " +
                                                @"""from""" + ":" + TimeIni.ToString() + ", " +
                                                @"""to""" + ":" + TimeFin.ToString() + ", " +
                                                @"""flags""" + ":0 " +
                                                        "}" +
                                        "}" +
                                        "&sid=[SID_VIALON]";
            urlExecReport = urlExecReport.Replace("[WIALON_ID_TEMPLATE_INFORME]", ConfigurationManager.AppSettings["WIALON_ID_TEMPLATE_INFORME"].ToString());
            urlExecReport = urlExecReport.Replace("[SID_VIALON]", sidsession);
            urlExecReport = urlExecReport.Replace("[Id_Vehiculo]", IdVehiculoCodWialon);
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
                urlGetRowsTable = urlGetRowsTable.Replace("[Id_Vehiculo]", IdVehiculoCodWialon);
                http = (HttpWebRequest)WebRequest.Create(urlGetRowsTable);
                http.Method = "GET";
                response = http.GetResponse();
                stream = response.GetResponseStream();
                sr = new StreamReader(stream);
                contentJSON = sr.ReadToEnd();
                JArray arrayFilas = JArray.Parse(contentJSON);
                this.Datos = new DataTable();
                this.Datos.Columns.Add("zone_name"); this.Datos.Columns.Add("zone_type"); this.Datos.Columns.Add("zone_area");
                this.Datos.Columns.Add("time_begin"); this.Datos.Columns.Add("time_end"); this.Datos.Columns.Add("duration_in");
                this.Datos.Columns.Add("duration_ival");
                //"zone_name", "zone_type", "zone_area", "time_begin", "time_end", "duration_in", "duration_ival"

                for (int p = 0; p < arrayFilas.Count; p++)
                {
                    JObject rss2 = (JObject)arrayFilas[p];

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

                    this.Datos.Rows.Add(DR);
                }
                //for (int p = 0; p < tablas.Children; p++)
                //{
                //    JToken Jtoken = arrayAlertas[p];
                //}
            }

            //Obtiene Mapa
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
                    using (FileStream fs = new FileStream(PathImages + @"\map.png", FileMode.Create, FileAccess.ReadWrite))
                    {
                        bitmap3.Save(memory, ImageFormat.Png);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                sr = new StreamReader(stream);
                contentJSON = sr.ReadToEnd();
            }

            urlMapaResult = "images/map.png";

            return true;
        }
    }
}
