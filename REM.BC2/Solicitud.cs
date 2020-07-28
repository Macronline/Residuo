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
    public class Solicitud : Singular
    {
        public Solicitud()
        {
        }

        public Solicitud(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }
        
        public double Create(string sNombre, string sRut, string sDireccion, string sCorreo, string sTelefono, string sTipo_residuo, 
            string sComentarios, string sIdMunic)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Nombre", sNombre);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter3 = new SqlParameter("@Direccion", sDireccion);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@Correo", sCorreo);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@Telefono", sTelefono);
            Parameters.Add(sqlParameter5);
            SqlParameter sqlParameter6 = new SqlParameter("@TipoResiduo", sTipo_residuo);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@Comentario", sComentarios);
            Parameters.Add(sqlParameter7);
            SqlParameter sqlParameter8;
            if (this.User != null)
                sqlParameter8 = new SqlParameter("@IdUserCreate", this.User.Id);
            else
                sqlParameter8 = new SqlParameter("@IdUserCreate", DBNull.Value);
            Parameters.Add(sqlParameter8);

            SqlParameter sqlParameter9 = new SqlParameter("@FechaCreate", DateTime.Now);
            Parameters.Add(sqlParameter9);
            SqlParameter sqlParameter10 = new SqlParameter("@IdClienteMunicipio", sIdMunic);
            Parameters.Add(sqlParameter10);
            return commonDalc.ExecuteSQLScalar("INS_SOLICITUD", Parameters);

        }

        public bool Read(string sIdSolicitud)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdSolicitud", sIdSolicitud);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_Solicitud", parametros);
            return (this.Datos.Rows.Count != 0);
        }

        public void Buscar(double IdClienteMunicipio)
        {

            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@IdClienteMunicipio", IdClienteMunicipio);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READALL_Solicitud", parametros);

        }

        public void Update(string sId, string sNumero, string sFecha, string sIdClienteMunicipio, string sIdRuta, string sIdCamion, string sNombreChoferes, string sCorreos, string sHora)
        {
            
        }
    }
}
