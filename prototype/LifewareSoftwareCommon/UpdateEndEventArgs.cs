using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifeware.SoftwareCommon
{
    public class UpdateEndEventArgs : EventArgs
    {
        public SemanticVersion newVersion { get; set; }
        public UpdateEndEventArgs(SemanticVersion nv)
        {
            this.newVersion = nv;
        }
    }
}
