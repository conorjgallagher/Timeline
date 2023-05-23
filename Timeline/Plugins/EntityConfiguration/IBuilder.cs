using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration
{
    public interface IBuilder
    {
        Entity Target { get; }
        void Construct();
    }
}
