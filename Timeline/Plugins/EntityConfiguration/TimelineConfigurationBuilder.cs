using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration
{
    public class TimelineConfigurationBuilder
    {
        private readonly List<IBuilder> builders;

        public TimelineConfigurationBuilder(List<IBuilder> builders)
        {
            this.builders = builders;
        }

        public void Construct()
        {
            foreach (var builder in builders)
            {
                builder.Construct();
            }
        }
    }
}