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
    [Serializable]

    public class Persona : Singular
    {
        public Persona(Usuario User)
        {
            base.User = User;
            DataExplorer de = new DataExplorer();
            this.DREntity.ItemArray = de.InitDataRow(this).ItemArray;
        }
        public void ReadAll()
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            this.Datos = DALC.ExecuteStoredProcedure("READ_ALL_PERSONAS", parametros);
        }

        internal void ReadByIdUsuario(string IdUsuario)
        {
            DALCSQLServer DALC = this.GetCommonDalc();
            ArrayList parametros = new ArrayList();
            SqlParameter param = new SqlParameter("@IdUsuario", IdUsuario);
            parametros.Add(param);
            this.Datos = DALC.ExecuteStoredProcedure("READ_PERSONA_BY_IDUSUARIO", parametros);
            if (this.Datos.Rows.Count == 1)
            {
                this.DREntity.ItemArray = this.Datos.Rows[0].ItemArray;
            }
        }

    }
}
