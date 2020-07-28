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
    public class Geocercas : Singular
    {
        public Geocercas(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public double Create(string sNombreGeocerca, string sUbicacion, string sCoordenadas,
            string sObservacion, string sIdClienteMunicipio)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@NombreGeocerca", sNombreGeocerca);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@Ubicacion", sUbicacion);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@Coordenadas", sCoordenadas);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@Observacion", sObservacion);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@IdUserCreate", this.User.Id);
            Parameters.Add(sqlParameter5);
            SqlParameter sqlParameter6 = new SqlParameter("@FechaCreate", DateTime.Now);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@IdClienteMunicipio", sIdClienteMunicipio);
            Parameters.Add(sqlParameter7);

            return commonDalc.ExecuteSQLScalar("INS_GEOCERCA", Parameters);

        }

        public void Read(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_GEOCERCA", parametros);

        }

        public void Update(string sId, string sNombreGeocerca, string sUbicacion, string sCoordenadas,
            string sObservacion, string sIdClienteMunicipio)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter0 = new SqlParameter("@Id", sId);
            Parameters.Add(sqlParameter0);
            SqlParameter sqlParameter1 = new SqlParameter("@NombreGeocerca", sNombreGeocerca);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@Ubicacion", sUbicacion);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@Coordenadas", sCoordenadas);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@Observacion", sObservacion);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@IdUserUpdate", this.User.Id);
            Parameters.Add(sqlParameter5);
            SqlParameter sqlParameter6 = new SqlParameter("@FechaUpdate", DateTime.Now);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@IdClienteMunicipio", sIdClienteMunicipio);
            Parameters.Add(sqlParameter7);


            commonDalc.ExecuteNonQuery("UPD_GEOCERCA", ref Parameters);

        }
        public void Delete(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            DALC.ExecuteNonQuery("DEL_GEOCERCA", ref parametros);

        }

        public void ReadAll()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READALL_GEOCERCA ", parametros);

        }
        
            public void ReadAll_con_Municipio()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READALL_GEOCERCA_CON_MUNICIPIO ", parametros);

        }

        public void ReadAll_Geocerca_del_Municipio(string IdClienteMunicipio)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdClienteMunicipio);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_GEOCERCA_BY_IDMUNICIPIO", parametros);

        }
    }
}
