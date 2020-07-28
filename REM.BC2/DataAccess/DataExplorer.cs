using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using REM.BC2;
using REM.BC2.Base;
using REM.BC2.Security;

namespace REM.BC2.DataAccess
{
    [Serializable]
    public class DataExplorer : Singular
    {
        private static SortedList<string, DataTable> Model = new SortedList<string, DataTable>();

        public DataExplorer()
        {
        }
        public DataTable GetDataTableModelEmpty(object type)
        {
            DataTable DT = null;
            try
            {
                if (Model.ContainsKey(type.ToString()))
                {
                    DT = Model[type.ToString()];
                }
                else
                {
                    DALCSQLServer DALC = this.GetCommonDalc();
                    string NameTable = "";
                    string SQL = "";
                    if (type is Usuario)
                    {
                        NameTable = "TBL_BUS_Usuario";
                        SQL = "SELECT A.*, B.* FROM " + NameTable + " AS A INNER JOIN TBL_BUS_UsrVigencia AS B " +
                            " ON A.Id = B.IdUsuario WHERE A.Id = -1 ";
                    }
                    if (type is Persona)
                    {
                        NameTable = "TBL_BUS_Persona";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is ClienteMunicipio)
                    {
                        NameTable = "TBL_BUS_ClienteMunicipio";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Ruta)
                    {
                        NameTable = "TBL_BUS_Ruta";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is VehRecolector)
                    {
                        NameTable = "TBL_BUS_VehRecolector";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is VehRecolector)
                    {
                        NameTable = "TBL_BUS_CAMION";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Ot)
                    {
                        NameTable = "TBL_BUS_Ot";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Fleetup)
                    {
                        NameTable = "TBL_BUS_Fleetup";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is FleetUpRecordVehPasoGeoCerca)
                    {
                        NameTable = "TBL_BUS_FleetUpRecordVehPasoGeoCerca";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Solicitud)
                    {
                        NameTable = "TBL_BUS_Solicitud";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }

                    if (type is Rol)
                    {
                        NameTable = "TBL_BUS_Rol";
                        SQL = String.Format("SELECT * FROM {0} WHERE Id = {1}", NameTable, -1);
                    }
                   
                    if (! Model.ContainsKey(type.ToString()))
                    {
                        DT = DALC.ExecuteSQLDirect(SQL);
                        Model.Add(type.ToString(), DT);
                    }
                    else
                    {
                        DT = Model[type.ToString()];
                    }    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return DT;
        }
        public DataRow InitDataRow(object type)
        {
            DataRow DR = null;
            try
            {
                if (Model.ContainsKey(type.ToString()))
                {
                    DR = Model[type.ToString()].NewRow();
                }
                else
                {
                    DALCSQLServer DALC = this.GetCommonDalc();
                    DataTable DT = null;
                    string NameTable = "";
                    string SQL = "";
                    if (type is Usuario)
                    {
                        NameTable = "TBL_BUS_Usuario";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Persona)
                    {
                        NameTable = "TBL_BUS_Persona";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is ClienteMunicipio)
                    {
                        NameTable = "TBL_BUS_ClienteMunicipio";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Ruta)
                    {
                        NameTable = "TBL_BUS_Ruta";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is VehRecolector)
                    {
                        NameTable = "TBL_BUS_VehRecolector";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Camiones)
                    {
                        NameTable = "TBL_BUS_Camion";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Geocercas)
                    {
                        NameTable = "TBL_BUS_Geocerca";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Ot)
                    {
                        NameTable = "TBL_BUS_Ot";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Fleetup)
                    {
                        NameTable = "TBL_BUS_Fleetup";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is FleetUpRecordVehPasoGeoCerca)
                    {
                        NameTable = "TBL_BUS_FleetUpRecordVehPasoGeoCerca";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }
                    if (type is Solicitud)
                    {
                        NameTable = "TBL_BUS_Solicitud";
                        SQL = "SELECT A.* FROM " + NameTable + " AS A " +
                            "WHERE A.Id = -1 ";
                    }

                    if (type is Rol)
                    {
                        NameTable = "TBL_BUS_Rol";
                        SQL = String.Format("SELECT * FROM {0} WHERE Id = {1}", NameTable, -1);
                    }
                    if (! Model.ContainsKey(type.ToString()))
                    {
                        DT = DALC.ExecuteSQLDirect(SQL);
                        Model.Add(type.ToString(), DT);
                        DR = DT.NewRow();
                    }
                    else
                    { 
                        DR = Model[type.ToString()].NewRow();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return DR;
        }

    }
}
