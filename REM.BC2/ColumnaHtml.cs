using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REM.BC2
{
    public enum eeTipoColumnaHtml
    {
        Label,
        CheckBox,
        ImageEdit,
        ImageDelete
    }
    public class ColumnaHtml
    {
        public eeTipoColumnaHtml Tipo;
        public string NameColumnForValue = "";
        public string NameColumnForId = "";
        public string SufijoToCtrl = "";
        public string NameFunctionJS = "";

        public ColumnaHtml(eeTipoColumnaHtml tipo, string nameColumnForId, string nameColumnForValue)
        {
            this.Tipo = tipo;
            this.NameColumnForValue = nameColumnForValue;
            this.NameColumnForId = nameColumnForId;
        }
        public ColumnaHtml(eeTipoColumnaHtml tipo, string nameColumnForId, string nameColumnForValue, string SufijoToCtrl)
        {
            this.Tipo = tipo;
            this.NameColumnForValue = nameColumnForValue;
            this.NameColumnForId = nameColumnForId;
            this.SufijoToCtrl = SufijoToCtrl;
        }
        public ColumnaHtml(eeTipoColumnaHtml tipo, string nameColumnForId, string nameColumnForValue, string SufijoToCtrl, string NameFunctionJS)
        {
            this.Tipo = tipo;
            this.NameColumnForValue = nameColumnForValue;
            this.NameColumnForId = nameColumnForId;
            this.SufijoToCtrl = SufijoToCtrl;
            this.NameFunctionJS = NameFunctionJS;
        }
    }
}
