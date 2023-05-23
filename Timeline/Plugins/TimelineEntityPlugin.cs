// <copyright file="TimelineEntityPlugin.cs" company="Microsoft">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Microsoft</author>
// <date>12/17/2013 9:53:05 PM</date>
// <summary>Implements the TimelineEntityPlugin Plugin.</summary>
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
// </auto-generated>

using Timeline.Plugins.EntityConfiguration;

namespace Timeline.Plugins
{
    using System;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// TimelineEntityPlugin Plugin.
    /// </summary>    
    public class TimelineEntityPlugin: Plugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineEntityPlugin"/> class.
        /// </summary>
        public TimelineEntityPlugin()
            : base(typeof(TimelineEntityPlugin))
        {
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(10, "Create", "xrmc_timelineentity", Execute));
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(10, "Create", "xrmc_timelineentitylink", Execute));
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(10, "Update", "xrmc_timelineentity", Execute));
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(10, "Update", "xrmc_timelineentitylink", Execute));
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "Create", "xrmc_timelineconfiguration", GenerateEntities));
            RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(40, "Update", "xrmc_timelineconfiguration", GenerateEntities));
        }

        protected void Execute(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            // LICENCEFREE - UNCOMMENT FOR LICENCE FREE VERSION! 
            //return;

            if (new LicenseValidator(localContext.OrganizationService).ValidateLicense() == false)
            {
                if (localContext.PluginExecutionContext.ParentContext == null ||
                    localContext.PluginExecutionContext.ParentContext.PrimaryEntityName != "xrmc_timelineconfiguration" ||
                    localContext.PluginExecutionContext.ParentContext.MessageName != "Create")
                {
                    throw new InvalidPluginExecutionException(
                        "You must purchase a license to enable custom entities or make changes to the default configuration.\r\n\r\nPlease contact xRM Consultancy (sales@xrmconsultancy.com)");
                }
            }
        }

        protected void GenerateEntities(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }
            var timelineConfigurationBuilder = new TimelineConfigurationBuilder(TimelineEntityFactory.CreateDefaultBuilders(localContext.OrganizationService));
            timelineConfigurationBuilder.Construct();
        }
    }
}