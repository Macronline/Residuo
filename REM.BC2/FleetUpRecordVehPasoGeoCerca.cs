using Newtonsoft.Json.Linq;
using REM.BC2.Base;
using REM.BC2.DataAccess;
using REM.BC2.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REM.BC2
{
    public class FleetUpRecordVehPasoGeoCerca: Singular
    {
        public FleetUpRecordVehPasoGeoCerca(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public void ProcesarTareasBatch(Usuario usr)
        {   //Se recorren todos los Camiones. Se lee para los 15 dias hacia atras
            //Si existe al menos 1 registro del dia se pasa a otro dia pues implica que ya pasó
            
            Fleetup fleet = new Fleetup(usr);
            string strGeo = fleet.LeerGeoCercas();  //Se leen definicion de Todas las GeoCercas desde Fleetup

            //Se leen las GeoCercas
            Geocercas geocercasBD = new Geocercas(usr);
            geocercasBD.ReadAll();

            //Se leen los Camiones
            VehRecolector camiones = new VehRecolector(usr);
            camiones.ReadAllForProcess();

            DateTime FechaInicio15Dias = DateTime.Now;
            DateTime FechaComparacionCiclo = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();

            for (int m = 0; m < 4; m++)
            {
                FechaComparacionCiclo = FechaComparacionCiclo.Subtract(new TimeSpan(24, 0, 0));
                if (! this.ExisteRegistroFleetupDelDia(FechaComparacionCiclo, commonDalc))
                {
                    for (int i = 0; i < camiones.Datos.Rows.Count; i++)
                    {
                        string devId = UtilWeb.GetDatoSingular("Fleetup_devId", camiones.Datos);  //"213NW2019000039"
                        string IdCamion = UtilWeb.GetDatoSingular("Id", camiones.Datos);  //"213NW2019000039"
                        string strGeoAlertasJSON = "";
                        try
                        {
                            System.Threading.Thread.Sleep(5000);
                            strGeoAlertasJSON = fleet.LeerAlertasGeoCercas(devId,
                                new DateTime(FechaComparacionCiclo.Year, FechaComparacionCiclo.Month, FechaComparacionCiclo.Day));
                        }
                        catch (Exception ex)
                        {
                            ex = ex;    
                        }
                        JObject rootObject = JObject.Parse(strGeoAlertasJSON);
                        JArray arrayAlertas = (JArray)rootObject["data"];
                        string IdGeoCercaTemp = "";
                        for (int p = 0; p < arrayAlertas.Count; p++)
                        {
                            JToken Jtoken = arrayAlertas[p];
                            string fenceNameAUX = Jtoken["fenceName"].ToString();
                            string fenceIdAUX = Jtoken["fenceId"].ToString();
                            string acconTimeAuxENTRADA = Jtoken["acconTime"].ToString();
                            //DateTime acconTimeAuxDateTime = new DateTime(acconTimeAux.Substring(0, 4), acconTimeAux.Substring(0, 4))
                            string tmTimeAuxSALIDA = Jtoken["tmTime"].ToString();

                            geocercasBD.Datos.DefaultView.RowFilter = string.Format("NombreGeocerca = '{0}'", fenceNameAUX);
                            if (geocercasBD.Datos.DefaultView.Count > 0)
                                IdGeoCercaTemp = geocercasBD.Datos.DefaultView[0]["Id"].ToString();
                            else
                                IdGeoCercaTemp = "";

                            Parameters = new ArrayList();
                            SqlParameter sqlParameter0 = new SqlParameter("@IdVehRecolector", IdCamion);
                            Parameters.Add(sqlParameter0);
                            SqlParameter sqlParameter1 = new SqlParameter("@Fleetup_devId", devId);
                            Parameters.Add(sqlParameter1);
                            SqlParameter sqlParameter2 = new SqlParameter("@fenceNameFleetup", fenceNameAUX);
                            Parameters.Add(sqlParameter2);
                            DateTime dtINIAux = new DateTime(int.Parse(acconTimeAuxENTRADA.Substring(0, 4)), int.Parse(acconTimeAuxENTRADA.Substring(4, 2)),
                                int.Parse(acconTimeAuxENTRADA.Substring(6, 2)), int.Parse(acconTimeAuxENTRADA.Substring(8, 2)),
                                int.Parse(acconTimeAuxENTRADA.Substring(10, 2)), int.Parse(acconTimeAuxENTRADA.Substring(12, 2)));
                            SqlParameter sqlParameter3 = new SqlParameter("@acconTimeFleetup", dtINIAux);
                            Parameters.Add(sqlParameter3);

                            DateTime dtFINAux = new DateTime(int.Parse(tmTimeAuxSALIDA.Substring(0, 4)), int.Parse(tmTimeAuxSALIDA.Substring(4, 2)),
                                int.Parse(tmTimeAuxSALIDA.Substring(6, 2)), int.Parse(tmTimeAuxSALIDA.Substring(8, 2)),
                                int.Parse(tmTimeAuxSALIDA.Substring(10, 2)), int.Parse(tmTimeAuxSALIDA.Substring(12, 2)));
                            SqlParameter sqlParameter33 = new SqlParameter("@tmTimeFleetup", dtFINAux);
                            Parameters.Add(sqlParameter33);
                            SqlParameter sqlParameter4 = new SqlParameter("@fenceIdFleetup", fenceIdAUX);
                            Parameters.Add(sqlParameter4);
                            SqlParameter sqlParameter5 = new SqlParameter("@NombreGeoCerca", fenceNameAUX);
                            Parameters.Add(sqlParameter5);
                            SqlParameter sqlParameter6 = new SqlParameter("@IdGeoCerca", IdGeoCercaTemp);
                            Parameters.Add(sqlParameter6);
                            SqlParameter sqlParameter7 = new SqlParameter("@FechaCreate", DateTime.Now);
                            Parameters.Add(sqlParameter7);
                            commonDalc.ExecuteSQLScalar("INS_FleetUpRecordVehPasoGeoCerca", Parameters);

                        }
                    }
                }
            }
        }

        public void ReadAll(string sIdCamion, DateTime FechaDesde, DateTime FechaHasta)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdCamion", double.Parse(sIdCamion));
            parametros.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@FechaDesde", FechaDesde);
            parametros.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@FechaHasta", FechaHasta);
            parametros.Add(sqlParameter3);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_FleetUpRecordVehPasoGeoCerca", parametros);

        }

        private bool ExisteRegistroFleetupDelDia(DateTime fechaComparacionCiclo, DALCSQLServer commonDalc)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@fechaComparacionCicloINI", fechaComparacionCiclo);
            parametros.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@fechaComparacionCicloFIN", fechaComparacionCiclo.AddDays(1));
            parametros.Add(sqlParameter2);
            DataTable DT = DALC.ExecuteStoredProcedure("EXIST_FleetUpRecordVehPasoGeoCerca_DELDIA", parametros);
            bool res = (DT.Rows.Count != 0);
            return res;
        }

        public void ProcesarTareasBatchWialon(Usuario usr)
        {   //Se recorren todos los Camiones. Se lee para los 15 dias hacia atras
            //Si existe al menos 1 registro del dia se pasa a otro dia pues implica que ya pasó

            Fleetup fleet = new Fleetup(usr);
            string strGeo = fleet.LeerGeoCercas();  //Se leen definicion de Todas las GeoCercas desde Fleetup

            //Se leen las GeoCercas
            Geocercas geocercasBD = new Geocercas(usr);
            geocercasBD.ReadAll();

            //Se leen los Camiones
            VehRecolector camiones = new VehRecolector(usr);
            camiones.ReadAllForProcess();

            DateTime FechaInicio15Dias = DateTime.Now;
            DateTime FechaComparacionCiclo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();

            for (int m = 0; m < 4; m++)
            {
                FechaComparacionCiclo = FechaComparacionCiclo.Subtract(new TimeSpan(24, 0, 0));
                if (!this.ExisteRegistroFleetupDelDia(FechaComparacionCiclo, commonDalc))
                {
                    for (int i = 0; i < camiones.Datos.Rows.Count; i++)
                    {
                        string devId = UtilWeb.GetDatoSingular("Fleetup_devId", camiones.Datos);  //"213NW2019000039"
                        string IdCamion = UtilWeb.GetDatoSingular("Id", camiones.Datos);  //"213NW2019000039"
                        string strGeoAlertasJSON = "";
                        try
                        {
                            System.Threading.Thread.Sleep(5000);
                            strGeoAlertasJSON = fleet.LeerAlertasGeoCercas(devId,
                                new DateTime(FechaComparacionCiclo.Year, FechaComparacionCiclo.Month, FechaComparacionCiclo.Day));
                        }
                        catch (Exception ex)
                        {
                            ex = ex;
                        }
                        JObject rootObject = JObject.Parse(strGeoAlertasJSON);
                        JArray arrayAlertas = (JArray)rootObject["data"];
                        string IdGeoCercaTemp = "";
                        for (int p = 0; p < arrayAlertas.Count; p++)
                        {
                            JToken Jtoken = arrayAlertas[p];
                            string fenceNameAUX = Jtoken["fenceName"].ToString();
                            string fenceIdAUX = Jtoken["fenceId"].ToString();
                            string acconTimeAuxENTRADA = Jtoken["acconTime"].ToString();
                            //DateTime acconTimeAuxDateTime = new DateTime(acconTimeAux.Substring(0, 4), acconTimeAux.Substring(0, 4))
                            string tmTimeAuxSALIDA = Jtoken["tmTime"].ToString();

                            geocercasBD.Datos.DefaultView.RowFilter = string.Format("NombreGeocerca = '{0}'", fenceNameAUX);
                            if (geocercasBD.Datos.DefaultView.Count > 0)
                                IdGeoCercaTemp = geocercasBD.Datos.DefaultView[0]["Id"].ToString();
                            else
                                IdGeoCercaTemp = "";

                            Parameters = new ArrayList();
                            SqlParameter sqlParameter0 = new SqlParameter("@IdVehRecolector", IdCamion);
                            Parameters.Add(sqlParameter0);
                            SqlParameter sqlParameter1 = new SqlParameter("@Fleetup_devId", devId);
                            Parameters.Add(sqlParameter1);
                            SqlParameter sqlParameter2 = new SqlParameter("@fenceNameFleetup", fenceNameAUX);
                            Parameters.Add(sqlParameter2);
                            DateTime dtINIAux = new DateTime(int.Parse(acconTimeAuxENTRADA.Substring(0, 4)), int.Parse(acconTimeAuxENTRADA.Substring(4, 2)),
                                int.Parse(acconTimeAuxENTRADA.Substring(6, 2)), int.Parse(acconTimeAuxENTRADA.Substring(8, 2)),
                                int.Parse(acconTimeAuxENTRADA.Substring(10, 2)), int.Parse(acconTimeAuxENTRADA.Substring(12, 2)));
                            SqlParameter sqlParameter3 = new SqlParameter("@acconTimeFleetup", dtINIAux);
                            Parameters.Add(sqlParameter3);

                            DateTime dtFINAux = new DateTime(int.Parse(tmTimeAuxSALIDA.Substring(0, 4)), int.Parse(tmTimeAuxSALIDA.Substring(4, 2)),
                                int.Parse(tmTimeAuxSALIDA.Substring(6, 2)), int.Parse(tmTimeAuxSALIDA.Substring(8, 2)),
                                int.Parse(tmTimeAuxSALIDA.Substring(10, 2)), int.Parse(tmTimeAuxSALIDA.Substring(12, 2)));
                            SqlParameter sqlParameter33 = new SqlParameter("@tmTimeFleetup", dtFINAux);
                            Parameters.Add(sqlParameter33);
                            SqlParameter sqlParameter4 = new SqlParameter("@fenceIdFleetup", fenceIdAUX);
                            Parameters.Add(sqlParameter4);
                            SqlParameter sqlParameter5 = new SqlParameter("@NombreGeoCerca", fenceNameAUX);
                            Parameters.Add(sqlParameter5);
                            SqlParameter sqlParameter6 = new SqlParameter("@IdGeoCerca", IdGeoCercaTemp);
                            Parameters.Add(sqlParameter6);
                            SqlParameter sqlParameter7 = new SqlParameter("@FechaCreate", DateTime.Now);
                            Parameters.Add(sqlParameter7);
                            commonDalc.ExecuteSQLScalar("INS_FleetUpRecordVehPasoGeoCerca", Parameters);

                        }
                    }
                }
            }
        }
    }
}
