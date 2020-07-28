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
    public class ClienteMunicipio : Singular
    {
        public ClienteMunicipio(Usuario Usr)
        {
            base.User = Usr;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public ClienteMunicipio()
        {
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
            this.Datos = de.GetDataTableModelEmpty(this);
        }

        public double Create(string sNombreMunic, string sRutMunic, string sNombreUserMunic, string sRutUserMunic, string sCorreoUserMunic, 
            string sClave, string sTelefono)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@NombreMunic", sNombreMunic);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@RutMunic", sRutMunic);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@NombreUserMunic", sNombreUserMunic);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@RutUserMunic", sRutUserMunic);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@CorreoUserMunic", sCorreoUserMunic);
            Parameters.Add(sqlParameter5);

            string PassEncripted = Security.Seguridad.EncriptarPass(sClave);
            SqlParameter sqlParameter6 = new SqlParameter("@Clave", PassEncripted);
            Parameters.Add(sqlParameter6);

            SqlParameter sqlParameter7 = new SqlParameter("@IdUserCreate", this.User.Id);
            Parameters.Add(sqlParameter7);
            SqlParameter sqlParameter8 = new SqlParameter("@FechaCreate", DateTime.Now);
            Parameters.Add(sqlParameter8);
            SqlParameter sqlParameter9 = new SqlParameter("@Telefono", sTelefono);
            Parameters.Add(sqlParameter9);
            return commonDalc.ExecuteSQLScalar("INS_CLIENTEMUNICIPIO", Parameters);

        }

        public void ReadAll()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READALL_CLIENTEMUNICIPIO", parametros);

        }

        public void Read(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_CLIENTEMUNICIPIO", parametros);

        }

        internal void ReadByNombreUserMunic()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@NombreUserMunic", this.User.UserNameLogin);
            parametros.Add(sqlParameter1);
            this.Datos = DALC.ExecuteStoredProcedure("READ_CLIENTEMUNIC_BY_NOMBREUSUARIO", parametros);

        }

        public void Update(string sId, string sNombreMunic, string sRutMunic, string sNombreUserMunic, string sRutUserMunic, string sCorreoUserMunic, string sClave, string sTelefono)
        {
            DALCSQLServer commonDalc = this.GetCommonDalc();
            ArrayList Parameters = new ArrayList();
            SqlParameter sqlParameter0 = new SqlParameter("@Id", sId);
            Parameters.Add(sqlParameter0);
            SqlParameter sqlParameter1 = new SqlParameter("@NombreMunic", sNombreMunic);
            Parameters.Add(sqlParameter1);
            SqlParameter sqlParameter2 = new SqlParameter("@RutMunic", sRutMunic);
            Parameters.Add(sqlParameter2);
            SqlParameter sqlParameter3 = new SqlParameter("@NombreUserMunic", sNombreUserMunic);
            Parameters.Add(sqlParameter3);
            SqlParameter sqlParameter4 = new SqlParameter("@RutUserMunic", sRutUserMunic);
            Parameters.Add(sqlParameter4);
            SqlParameter sqlParameter5 = new SqlParameter("@CorreoUserMunic", sCorreoUserMunic);
            Parameters.Add(sqlParameter5);

            //string PassEncripted = Security.Seguridad.EncriptarPass(sClave);
            //SqlParameter sqlParameter6 = new SqlParameter("@Clave", PassEncripted);
            //Parameters.Add(sqlParameter6);

            SqlParameter sqlParameter6 = new SqlParameter("@IdUserUpdate", this.User.Id);
            Parameters.Add(sqlParameter6);
            SqlParameter sqlParameter7 = new SqlParameter("@FechaUpdate", DateTime.Now);
            Parameters.Add(sqlParameter7);
            SqlParameter sqlParameter8 = new SqlParameter("@Telefono", sTelefono);
            Parameters.Add(sqlParameter8);
            commonDalc.ExecuteNonQuery("UPD_CLIENTEMUNICIPIO", ref Parameters);

        }

        public void Delete(string Id)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter sqlParameter1 = new SqlParameter("@Id", Id);
            parametros.Add(sqlParameter1);
            DALC.ExecuteNonQuery("DEL_CLIENTEMUNICIPIO", ref parametros);

        }
    }
}
