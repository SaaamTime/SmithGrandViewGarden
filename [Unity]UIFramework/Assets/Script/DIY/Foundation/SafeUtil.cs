using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIY.Foundation
{
    public static class SafeUtil
    {
        public static bool CheckLegal_Array<T>(T[] target) {
            return target != null && target.Length > 0;
        }

        public static void DoAction(Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        
    }
}
