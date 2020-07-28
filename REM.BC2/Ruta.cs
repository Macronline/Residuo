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
    public class Ruta : Singular
    {
        public Ruta(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public double Create(string sNombreRuta, string sIdMunicipio, string sIdGeocerca)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@NombreRuta", sNombreRuta);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@IdClienteMunicipio", sIdMunicipio);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@IdGeocerca", sIdGeocerca);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@IdUserCreate", this.User.Id);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@FechaCreate", DateTime.Now);
            Parameters.Add(sqlParameter5);
            return commonDalc.ExecuteSQLScalar("INS_Ruta", Parameters);
        }

        public void ReadAll(double IdClienteMunicipio)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdClienteMunicipio);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_Ruta", parametros);

        }

        public void Read(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_Ruta", parametros);

        }
        public void ReadById(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_RUTA_BY_ID", parametros);

        }
        public void ReadAllByMunicipio(string IdMunicipio)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdMunicipio);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_RUTA_BY_CLIENTE", parametros);

        }

        public void ReadAll_con_Municipio_y_Geocerca()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READALL_Ruta_Geo_Municipio ", parametros);

        }

        public void ReadAllRutasyGeocercas_ByIdMunicipio(string IdClienteMunicipio)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdClienteMunicipio);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_Ruta_Geo_Municipio_BY_IDMUNICIPIO", parametros);

        }
        public void Update(string sId, string sNombreRuta, string sIdMunicipio, string sIdGeocerca)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@NombreRuta", sNombreRuta);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@IdlienteMunicipio", sIdMunicipio);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@IdGeocerca", sIdGeocerca);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@IdUserUpdate", this.User.Id);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@FechaUpdate", DateTime.Now);
            Parameters.Add(sqlParameter5);
            commonDalc.ExecuteNonQuery("UPD_Ruta", ref Parameters);

        }

        public void Delete(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            DALC.ExecuteNonQuery("DEL_Ruta", ref parametros);

        }
    }
}
