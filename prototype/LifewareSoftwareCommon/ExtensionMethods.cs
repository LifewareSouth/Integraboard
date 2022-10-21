using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifeware.SoftwareCommon
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adapta el valor de Serial devuelto por servicio
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Lifeware.Serial Adapt(this Lifeware.Validation.Serial s)
        {
            Lifeware.Serial result = new Lifeware.Serial()
            {
                KeyboardCode = s.KeyboardCode,
                ActivationCode = s.ActivationCode,
                PublicKey = s.PublicKey,
                SerialNumber = s.SerialNumber,
                SoftwareGuid = s.SoftwareGuid

            };
            return result;
        }
    }
}
