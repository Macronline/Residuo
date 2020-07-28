using REM.BC2.Base;
using REM.BC2.DataAccess;
using REM.BC2.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REM.BC2
{
    public class VehRecolector : Singular
    {
        public VehRecolector(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public double Create(string sNombreCamion, string sNombreMarca, string sNombreModelo, string sNombreTagPatente, string sIdMunicipio)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@NombreCamion", sNombreCamion);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2= new SqlParameter("@NombreMarca", sNombreMarca);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@NombreModelo", sNombreModelo);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@NombreTagPatente", sNombreTagPatente);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@IdUserCreate", this.User.Id);
            Parameters.Add(sqlParameter5);
            SqlParameter sqlParameter6 = new SqlParameter("@FechaCreate", DateTime.Now);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@IdClienteMunicipio", sIdMunicipio);
            Parameters.Add(sqlParameter7);
            return commonDalc.ExecuteSQLScalar("INS_VehRecolector", Parameters);
        }

        public void ReadAll(double IdClienteMunicipio)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdClienteMunicipio);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_VehRecolector", parametros);

        }
        public void ReadAll_Camion_del_Municipio(string IdClienteMunicipio)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdClienteMunicipio);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_VehRecolector_BY_IDMUNICIPIO", parametros);

        }
        public void ReadAll()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READALL_VehRecolector_CON_MUNICIPIO", parametros);

        }

        public void ReadAllForProcess()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READALL_VehRecolector_For_PROCESSMASSIVE", parametros);

        }

        public void Read(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_VehRecolector", parametros);

        }
        
        public void Update(string sId, string sNombreVehRecolector, string sNombreMarca, string sNombreModelo, string sNombreTagPatente, string sIdMunicipio)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter0 = new SqlParameter("@Id", sId);
            Parameters.Add(sqlParameter0);
            SqlParameter sqlParameter1 = new SqlParameter("@NombreCamion", sNombreVehRecolector);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@NombreMarca", sNombreMarca);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@NombreModelo", sNombreModelo);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@NombreTagPatente", sNombreTagPatente);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@IdUserUpdate", this.User.Id);
            Parameters.Add(sqlParameter5);
            SqlParameter sqlParameter6 = new SqlParameter("@FechaUpdate", DateTime.Now);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@IdClienteMunicipio", sIdMunicipio);
            Parameters.Add(sqlParameter7);
            commonDalc.ExecuteNonQuery("UPD_VehRecolector", ref Parameters);

        }

        public void Delete(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            DALC.ExecuteNonQuery("DEL_VehRecolector", ref parametros);

        }
    }
}
