using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahc.Health.Common.Constant
{/// <summary>
 /// Enumeration represents possible genders for the session subject.
 /// </summary>
    [Serializable]
    public enum EnumStatus
    {
        /// <summary>
        /// Open status
        /// </summary>
        Open = 0,

        /// <summary>
        /// completed record
        /// </summary>
        Completed = 1

    }

    public enum EnumHealthMetric
    {
        /// <summary>
        ///heart rate
        /// </summary>
        HEARTRATE = 1,

        /// <summary>
        /// Female gender
        /// </summary>
        TOTSLEEPS = 2

    }

}
