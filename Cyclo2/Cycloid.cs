using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyclo2
{
    class Cycloid
    {

        private Dictionary<String,Double> parameters;
        private UInt16 z;
        private Boolean isEpicycloid;

        public ushort Z { get => z; set => z = value; }
        public bool IsEpicycloid { get => isEpicycloid; set => isEpicycloid = value; }

        public Cycloid()
        {
            //da = df = e = lambda = dw = ro = dg = db = g = z = 0;
            
        }
        
    }
}
