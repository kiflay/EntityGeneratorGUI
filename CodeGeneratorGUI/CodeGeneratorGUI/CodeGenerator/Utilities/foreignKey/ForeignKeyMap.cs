using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneratorGUI.CodeGenerator.Utilities.foreignKey
{
    public class ForeignKeyMap
    {
        public string FKColumn { get; set; }
        public string FKTable { get; set; }
        public string PKTable { get; set; }
        public string PKColumn { get; set; }


    }
}
