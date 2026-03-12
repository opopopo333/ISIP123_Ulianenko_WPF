using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class assembly_
    {
        public decimal TotalSum
        {
            get
            {
                return this.partassembly_.Sum(pa => pa.basepart_.price);
            }
        }
    }
}
