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
    public class Ot : Singular
    {
        public Ot(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }
        public double Create(string sNumero, string sFecha, string sIdClienteMunicipio, string sIdRuta, string sIdCamion,
            string sNombreChoferes, string sCorreos, string sHora, string sValorRetiro, string sIdSolicitud)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Numero", sNumero);
            Parameters.Add(sqlParameter1);

            DateTime dt;
            SqlParameter sqlParameter2 = null;
            if (DateTime.TryParse(sFecha, out dt))
                sqlParameter2 = new SqlParameter("@Fecha", dt);
            else
                sqlParameter2 = new SqlParameter("@Fecha", DateTime.Now);

            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@IdClienteMunicipio", sIdClienteMunicipio);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = null;
            if (sIdRuta.Length > 0)
                sqlParameter4 = new SqlParameter("@IdRuta", sIdRuta);
            else
                sqlParameter4 = new SqlParameter("@IdRuta", DBNull.Value);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@IdCamion", sIdCamion);
            Parameters.Add(sqlParameter5);
            SqlParameter sqlParameter6 = new SqlParameter("@NombreChoferes", sNombreChoferes);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@Correos", sCorreos);
            Parameters.Add(sqlParameter7);
            SqlParameter sqlParameter10 = new SqlParameter("@Hora", sHora);
            Parameters.Add(sqlParameter10);
            SqlParameter sqlParameter8 = new SqlParameter("@IdUserCreate", this.User.Id);
            Parameters.Add(sqlParameter8);
            SqlParameter sqlParameter9 = new SqlParameter("@FechaCreate", DateTime.Now);
            Parameters.Add(sqlParameter9);
            SqlParameter sqlParameter11 = new SqlParameter("@ValorRetiro", double.Parse(sValorRetiro));
            Parameters.Add(sqlParameter11);
            SqlParameter sqlParameter12 = new SqlParameter("@IdSolicitud", double.Parse(sIdSolicitud));
            Parameters.Add(sqlParameter12);
            return commonDalc.ExecuteSQLScalar("INS_OT", Parameters);

        }

        public bool Read(string sIdOt)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdOt", sIdOt);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_OT", parametros);
            return (this.Datos.Rows.Count != 0);
        }

        public void Buscar(string IdClienteMunicipio, DateTime dtDesde, DateTime dtHasta, string sIdRuta, string sIdCamion)
        {

            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdClienteMunicipio);
            parametros.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@FechaDesde", dtDesde);
            parametros.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@FechaHasta", dtHasta);
            parametros.Add(sqlParameter3);
            SqlParameter sqlParameter4 = null;

            if (sIdRuta == "-1" )
                sqlParameter4 = new SqlParameter("@IdRuta", DBNull.Value); 
            else
                sqlParameter4 = new SqlParameter("@IdRuta", sIdRuta);
            parametros.Add(sqlParameter4);

            SqlParameter sqlParameter5 = null;
            if (sIdCamion == "-1")
                sqlParameter5 = new SqlParameter("@IdCamion", DBNull.Value);
            else
                sqlParameter5 = new SqlParameter("@IdCamion", sIdCamion);
            parametros.Add(sqlParameter5);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_OTs", parametros);

        }

        public double ReadByIdSolicitud(string sIdSolicitud)
        {
            double res = 0;
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdSolicitud", sIdSolicitud);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_OT_BY_IDSOLICITUD", parametros);
            if (this.Datos.Rows.Count > 0)
            {
                res = double.Parse(this.Datos.Rows[0]["Id"].ToString());
            }
            return res;
        }

        public void Update(string sId, string sNumero, string sFecha, string sIdClienteMunicipio, string sIdRuta, string sIdCamion, 
            string sNombreChoferes, string sCorreos, string sHora, string sValorRetiro, string sIdSolicitud)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            
            DateTime dt;
            SqlParameter sqlParameter2 = null;
            if (DateTime.TryParse(sFecha, out dt))
                sqlParameter2 = new SqlParameter("@Fecha", dt);
            else
                sqlParameter2 = new SqlParameter("@Fecha", DateTime.Now);
            Parameters.Add(sqlParameter2);

            SqlParameter sqlParameter3 = new SqlParameter("@IdClienteMunicipio", sIdClienteMunicipio);
            Parameters.Add(sqlParameter3);

            SqlParameter sqlParameter4 = null;
            if (sIdRuta.Length > 0)
                sqlParameter4 = new SqlParameter("@IdRuta", sIdRuta);
            else
                sqlParameter4 = new SqlParameter("@IdRuta", DBNull.Value);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@IdCamion", sIdCamion);
            Parameters.Add(sqlParameter5);
            SqlParameter sqlParameter6 = new SqlParameter("@NombreChoferes", sNombreChoferes);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@Correos", sCorreos);
            Parameters.Add(sqlParameter7);
            SqlParameter sqlParameter10 = new SqlParameter("@Hora", sHora);
            Parameters.Add(sqlParameter10);
            SqlParameter sqlParameter8 = new SqlParameter("@IdUserUpdate", this.User.Id);
            Parameters.Add(sqlParameter8);
            SqlParameter sqlParameter9 = new SqlParameter("@FechaUpdate", DateTime.Now);
            Parameters.Add(sqlParameter9);
            SqlParameter sqlParameter11 = new SqlParameter("@ValorRetiro", double.Parse(sValorRetiro));
            Parameters.Add(sqlParameter11);
            SqlParameter sqlParameter12 = new SqlParameter("@IdSolicitud", double.Parse(sIdSolicitud));
            Parameters.Add(sqlParameter12);
            commonDalc.ExecuteSQLScalar("UPD_OT", Parameters);
        }
    }
}
