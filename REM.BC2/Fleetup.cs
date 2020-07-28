using Newtonsoft.Json.Linq;
using REM.BC2.Base;
using REM.BC2.DataAccess;
using REM.BC2.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace REM.BC2
{
    public class Fleetup : Singular
    {
        public Fleetup(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public string LeerGeoCercas()
        {
            ///fencealerts/fence-names
            string token = this.GetTokenActivo();
            string resp = "";
            try
            {
                if (token.Trim().Length != 0)
                {
                    string urlGetGeoCercas = "https://api.fleetup.net/fencealerts/fence-names";
                    string acctId = ConfigurationManager.AppSettings["fleetup_acctId"].ToString();
                    string api_key = ConfigurationManager.AppSettings["fleetup_api_key"].ToString();
                    HttpWebRequest http = (HttpWebRequest)WebRequest.Create(urlGetGeoCercas);
                    http.Method = "POST";
                    http.Headers.Add("x-api-key", api_key);
                    http.Headers.Add("token", token);
                    StreamWriter writer = new StreamWriter(http.GetRequestStream());
                    string xmlString = @"{""" + "acctId" + @""": """ + acctId + @"""}}";
                    writer.Write(xmlString);
                    writer.Close();
                    WebResponse rsp = http.GetResponse();
                    Stream streamnew = rsp.GetResponseStream();
                    StreamReader sr2 = new StreamReader(streamnew);
                    string contentJSON = sr2.ReadToEnd();

                    JObject rootObject = JObject.Parse(contentJSON);
                    JArray resultsArray = (JArray)rootObject["fenceNames"];
                    for (int i = 0; i < resultsArray.Count; i++)
                    {
                        JToken Jtoken = resultsArray[i];
                    }

                    resp = contentJSON;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resp;
        }

        public string LeerAlertasGeoCercas(string devId, DateTime startDate)
        {
            ///fencealerts/fence-names
            string token = this.GetTokenActivo();
            string resp = "";
            try
            {
                if (token.Trim().Length != 0)
                {
                    string startDateString = string.Format("{0}{1}{2}{3}{4}{5}", startDate.Year.ToString(), startDate.Month.ToString("00"),
                        startDate.Day.ToString("00"), startDate.Hour.ToString("00"), startDate.Minute.ToString("00"),
                        startDate.Second.ToString("00"));
                    string urlGetAlertasGeoCercas = "https://api.fleetup.net/fencealerts/geo-fencealerts";
                    string acctId = ConfigurationManager.AppSettings["fleetup_acctId"].ToString();
                    string api_key = ConfigurationManager.AppSettings["fleetup_api_key"].ToString();
                    HttpWebRequest http = (HttpWebRequest)WebRequest.Create(urlGetAlertasGeoCercas);
                    http.Method = "POST";
                    http.Headers.Add("x-api-key", api_key);
                    http.Headers.Add("token", token);
                    StreamWriter writer = new StreamWriter(http.GetRequestStream());
                    string xmlString = @"{""" + "acctId" + @""": """ + acctId + @""", """ +
                        "devId" + @""": """ + devId + @""", """ + "startDate" + @""": """ + startDateString + @"""}";
                    writer.Write(xmlString);
                    writer.Close();
                    WebResponse rsp = http.GetResponse();
                    Stream streamnew = rsp.GetResponseStream();
                    StreamReader sr2 = new StreamReader(streamnew);
                    string contentJSON = sr2.ReadToEnd();

                    JObject rootObject = JObject.Parse(contentJSON);
                    JArray resultsArray = (JArray)rootObject["data"];
                    for (int i = 0; i < resultsArray.Count; i++)
                    {
                        JToken Jtoken = resultsArray[i];
                    }

                    resp = contentJSON;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resp;
        }

        private string GetTokenActivo()
        {
            string res = "";
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter5 = new SqlParameter("@FechaActual", DateTime.Now);
            parametros.Add(sqlParameter5);
            DataTable DT = DALC.ExecuteStoredProcedure("READ_FLEETUP_TOKEN_ACTIVO", parametros);
            if (DT.Rows.Count == 0)
            {
                string acctId = ConfigurationManager.AppSettings["fleetup_acctId"].ToString(); // "21563";
                string secret = ConfigurationManager.AppSettings["fleetup_secret"].ToString(); //"ibi2d5eqabmbxptutvnt803ba54f1u77";
                string api_key = ConfigurationManager.AppSettings["fleetup_api_key"].ToString(); //"u6KbPUKA2w1mzUvg0b2cn5A37wsg8Ecn8iKKGQhL";

                string urlGetToken = "https://api.fleetup.net/token?acctId={0}&secret={1}";
                string contentJSON = "";
                try
                {
                    //https://api.fleetup.net/token?acctId=21563&secret=ibi2d5eqabmbxptutvnt803ba54f1u77
                    urlGetToken = string.Format(urlGetToken, acctId, secret);
                    HttpWebRequest http = (HttpWebRequest)WebRequest.Create(urlGetToken);
                    http.Method = "GET";
                    http.Headers.Add("x-api-key", api_key);
                    WebResponse response = http.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream);
                    contentJSON = sr.ReadToEnd();
                    var details = JObject.Parse(contentJSON);
                    string token = details["token"].ToString();

                    parametros = new ArrayList();
                    SqlParameter sqlParameter1 = new SqlParameter("@TokenString", token);
                    parametros.Add(sqlParameter1);
                    SqlParameter sqlParameter2 = new SqlParameter("@FechaObtencion", DateTime.Now);
                    parametros.Add(sqlParameter2);
                    SqlParameter sqlParameter3 = new SqlParameter("@FechaExpiracion", DateTime.Now.Add(new TimeSpan(1, 0, 0)));
                    parametros.Add(sqlParameter3);
                    SqlParameter sqlParameter4 = new SqlParameter("@contentJSON", contentJSON);
                    parametros.Add(sqlParameter4);
                    double IdToken = DALC.ExecuteSQLScalar("INS_FLEETUP_TOKEN", parametros);

                    parametros = new ArrayList();
                    SqlParameter sqlParameter6 = new SqlParameter("@FechaActual", DateTime.Now);
                    parametros.Add(sqlParameter6);
                    DT = DALC.ExecuteStoredProcedure("READ_FLEETUP_TOKEN_ACTIVO", parametros);
                    if (DT.Rows.Count != 0)
                    {
                        res = DT.Rows[0]["TokenString"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                res = DT.Rows[0]["TokenString"].ToString();
            }
            return res;
        }
    }
}
