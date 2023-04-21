using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dynamic_Modeling
{
    public struct AssemblyDepartment
    {
        public int[] ADetailsAmount;
        public int[] BDetailsAmount;

        public AssemblyDepartment(int modelingTime, int startADetailsAmount, int startBDetailsAmount)
        {
            ADetailsAmount = new int[modelingTime];
            BDetailsAmount = new int[modelingTime];

            ADetailsAmount[0] = startADetailsAmount;
            BDetailsAmount[0] = startBDetailsAmount;
        }
    }
}
