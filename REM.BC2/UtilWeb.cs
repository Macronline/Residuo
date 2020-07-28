using REM.BC2.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace REM.BC2
{
    public class UtilWeb
    {
        public const string SELECCIONE_STRING = "...Seleccione...";

        public static DateTime GetDateNow()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo myZone = TimeZoneInfo.CreateCustomTimeZone("CHILE", new TimeSpan(-4, 0, 0), "Chile", "Chile");
            DateTime custDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, myZone);
            return custDateTime;
        }

        public string GetJson(DataTable dt)
        {
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (DataColumn column in (InternalDataCollectionBase)dt.Columns)
                    dictionary.Add(column.ColumnName.Trim(), row[column]);
                dictionaryList.Add(dictionary);
            }
            return scriptSerializer.Serialize(dictionaryList);
        }

        public static void AddElementSeleccioneALista(DataTable DT)
        {
            int i = 0;
            DataRow DrSelec = null;
            DrSelec = DT.NewRow();  //Se crea new Row
            for (i = 0; i < DT.Columns.Count; i++)
            {
                if (DT.Columns[i].DataType.ToString() == "System.String")
                {
                    DrSelec[i] = UtilWeb.SELECCIONE_STRING;
                }
                if (DT.Columns[i].ColumnName == "Id")
                {
                    if (DT.Columns[i].DataType.ToString() == "System.String")
                        DrSelec[i] = "";
                    else
                        DrSelec[i] = -1;
                }
            }
            DT.Rows.InsertAt(DrSelec, 0);  //Se suma DataRow
            DrSelec = null;
        }

        public static string GenerateStringSecure(string IdUser, ref string StringOfControl, ref string Serial, ref string Token)
        {
            Serial = Guid.NewGuid().ToString();
            Token = Guid.NewGuid().ToString();
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes;
            byte[] encodedBytes;
            string key = ConfigurationManager.AppSettings["K"].ToString();
            key += IdUser;
            key += Serial;
            key += Token;
            string Res3 = "";
            originalBytes = UTF8Encoding.UTF8.GetBytes(key);
            encodedBytes = md5.ComputeHash(originalBytes);
            foreach (byte b in encodedBytes)
            {
                Res3 += b.ToString("x2");
            }
            StringOfControl = Res3;
            return key;
        }

        public static string GetDatoSingular(string NameColumn, DataTable DT)
        {
            return DT.Rows[0][NameColumn].ToString();
        }

        public static bool CheckSession(List<string> Lista, ref Usuario usr, HttpContext httpContext)
        {
            char[] chArray = new char[1] { char.Parse("=") };
            if (httpContext.Request.Cookies["QS"] == null)
                return false;
            string ClientQueryString = httpContext.Request.Cookies["QS"].Value.ToString();
            ClientQueryString.Trim().Replace("%3d", "=").Split(chArray);
            string IdUser = "";
            string CK1 = "";
            string CK2 = "";
            usr = new Usuario();
            if (!usr.ReadCKOfUser(ClientQueryString, ref CK1, ref CK2, ref IdUser))
                return false;
            usr.Id = IdUser;
            usr.ReadById();
            return true;
        }

        public string GetHtmlTableBasica(DataTable dt, List<ColumnaHtml> lista)  //, string classTr, string classTd
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                //sb.Append(string.Format("<tr class='{0}'>", classTr));
                sb.Append(string.Format("<tr>"));
                for (int i = 0; i < lista.Count; i++)
                {
                    switch (lista[i].Tipo)
                    {
                        case eeTipoColumnaHtml.Label:
                            sb.Append(string.Format("<td>{0}</td>", dr[lista[i].NameColumnForValue].ToString()));
                            break;
                        case eeTipoColumnaHtml.CheckBox:
                            sb.Append(string.Format("<td><input type='checkbox' class='icheck' id='chk{1}_{0}' IdDatoAdicional={0}></td>", dr[lista[i].NameColumnForId].ToString(), lista[i].SufijoToCtrl));
                            break;
                        case eeTipoColumnaHtml.ImageEdit:
                            //<td class='modificar'><i class='fa fa-pencil' style='color: green;' id='img{1}_{0}' onClick='{2}({0}, this)' IdDatoAdicional='{0}'></i></td>
                            sb.Append(string.Format("<td class='modificar'><i class='fa fa-pencil' style='color: green;' id='img{1}_{0}' onClick='{2}({0}, this)' IdDatoAdicional='{0}'></i></td>", 
                                dr[lista[i].NameColumnForId].ToString(), lista[i].SufijoToCtrl, lista[i].NameFunctionJS));
                            break;
                        case eeTipoColumnaHtml.ImageDelete:
                            //<td class='modificar'><i class='fa fa-pencil' style='color: green;' id='img{1}_{0}' onClick='{2}({0}, this)' IdDatoAdicional='{0}'></i></td>
                            sb.Append(string.Format("<td class='borrar'><i class='fa fa-trash' style='color: red;' id='img{1}_{0}' onClick='{2}({0}, this)' IdDatoAdicional='{0}'></i></td>",
                                dr[lista[i].NameColumnForId].ToString(), lista[i].SufijoToCtrl, lista[i].NameFunctionJS));
                            break;
                    }
                }

                sb.Append("</tr>");
            }
            return sb.ToString();
        }

        public static bool IsValidMail(string Mail)
        {
            try
            {
                var addr = new MailAddress(Mail);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool IsValidRut(string RutConDV)
        {
            try
            {
                RutConDV = RutConDV.Replace(".", ""); //Quita puntos si los tiene
                if (RutConDV.Trim().Length == 0)
                    return true;

                if (!RutConDV.Contains("-"))
                    RutConDV = RutConDV.Substring(0, RutConDV.Length - 1) + "-" + RutConDV.Substring(RutConDV.Length - 1);

                int Digito; int Contador; int Multiplo; int Acumulador; string RutDigito;
                Contador = 2;
                Acumulador = 0;
                int rut = int.Parse(RutConDV.Substring(0, RutConDV.IndexOf("-")));
                string dv = RutConDV.Substring(RutConDV.IndexOf("-") + 1);
                while (rut != 0)
                {
                    Multiplo = (rut % 10) * Contador;
                    Acumulador = Acumulador + Multiplo;
                    rut = rut / 10;
                    Contador = Contador + 1;
                    if (Contador == 8)
                    {
                        Contador = 2;
                    }
                }
                Digito = 11 - (Acumulador % 11);
                RutDigito = Digito.ToString().Trim();
                if (Digito == 10) RutDigito = "K";
                if (Digito == 11) RutDigito = "0";
                return (dv.ToUpper() == RutDigito);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsNumeric(string Character, ref int RetNumber)
        {
            bool isNum;
            isNum = int.TryParse(Convert.ToString(Character), System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.InvariantInfo, out RetNumber);
            return isNum;
        }
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

        public static void SendMail(string body, string Destino, string Asunto, bool IsHtml)
        {
            MailMessage objMessage = null;
            SmtpClient objSmtp = null;
            string HostAddress = ConfigurationManager.AppSettings["HostAddress"];
            string MailAddress = ConfigurationManager.AppSettings["CuentaOrigen"];
            string ClaveCorreo = ConfigurationManager.AppSettings["ClaveCorreo"];
            try
            {
                objMessage = new MailMessage();
                objMessage.From = new MailAddress(MailAddress);
                objMessage.To.Add(Destino);

                objMessage.Subject = Asunto;
                objMessage.Body = body;
                objMessage.IsBodyHtml = IsHtml;
                objSmtp = new SmtpClient(HostAddress);

                objSmtp.Host = HostAddress;
                objSmtp.Credentials = new System.Net.NetworkCredential(MailAddress, ClaveCorreo);
                objSmtp.EnableSsl = false;

                objSmtp.Send(objMessage);

                //////////string param1 = ConfigurationManager.AppSettings["param1"];
                //////////string param2 = ConfigurationManager.AppSettings["param2"];
                //////////param2 = "info" + param2.Substring(5) + param2.Substring(4, 1) + param2.Substring(3, 1) + param2.Substring(2, 1) +
                //////////    param2.Substring(1, 1) + param2.Substring(0, 1);
                //////////objSmtp.Credentials = new NetworkCredential(param1, param2);
                //objSmtp.Host = HostAddress;
                //objSmtp.Credentials = new System.Net.NetworkCredential(MailAddress, "123456");
                //objSmtp.EnableSsl = false;

                objSmtp.Send(objMessage);
                objSmtp = null;
                objMessage = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
