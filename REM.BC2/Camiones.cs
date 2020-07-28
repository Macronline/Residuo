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
    public class Camiones : Singular
    {
        public Camiones(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public double Create(string sNombreCamion, string sMarcaCamion, string sModeloCamion, 
            string sTagCamion, string sMunicipio)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Nombre", sNombreCamion);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@Marca", sMarcaCamion);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@Modelo", sModeloCamion);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@Tag", sTagCamion);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@Municipio", sMunicipio);
            Parameters.Add(sqlParameter5);
            //SqlParameter sqlParameter6 = new SqlParameter("@Clave", sClave);
            //Parameters.Add(sqlParameter6);
            //SqlParameter sqlParameter7 = new SqlParameter("@IdUserCreate", this.User.Id);
            //Parameters.Add(sqlParameter7);
            //SqlParameter sqlParameter8 = new SqlParameter("@FechaCreate", DateTime.Now);
            //Parameters.Add(sqlParameter8);
            return commonDalc.ExecuteSQLScalar("INS_CAMION", Parameters);

        }

        public void Read(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_CAMION", parametros);

        }

        public void Update(string sId, string sNombreCamion, string sMarcaCamion, string sModeloCamion,
            string sTagCamion, string sMunicipio)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter0 = new SqlParameter("@Id", sId);
            Parameters.Add(sqlParameter0);
            SqlParameter sqlParameter1 = new SqlParameter("@Nombre", sNombreCamion);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@Marca", sMarcaCamion);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@Modelo", sModeloCamion);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@Tag", sTagCamion);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@Municipio", sMunicipio);
            Parameters.Add(sqlParameter5);

          
            commonDalc.ExecuteNonQuery("UPD_CAMION", ref Parameters);

        }
        public void Delete(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            DALC.ExecuteNonQuery("DEL_CAMION", ref parametros);

        }

        public void ReadAll()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READALL_CAMION", parametros);

        }
    }
}
