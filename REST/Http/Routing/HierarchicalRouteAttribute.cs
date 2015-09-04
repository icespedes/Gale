﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace System.Web.Http
{
    /// <summary>
    /// Override the custom Route for the action (with API version override)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HierarchicalRouteAttribute : Attribute, IDirectRouteFactory
    {
        private string _template = null;

        /// <summary>
        /// Override the custom Route for the action
        /// </summary>
        /// <param name="template">The route template describing the URI pattern to match against.</param>
        public HierarchicalRouteAttribute(string template)
        {
            //FORMAT: {version}/{template}
            _template = template;
        }

        /// <summary>
        /// Route Factory
        /// </summary>
        /// <param name="context">Route Context</param>
        /// <returns></returns>
        public RouteEntry CreateRoute(DirectRouteFactoryContext context)
        {
            string controllerName = context.Actions.FirstOrDefault().ControllerDescriptor.ControllerName;

            //Format: {apiVersion}/{controller}/{template}
            string template = String.Format(
                "{0}/{1}{2}{3}",
                   Gale.REST.Config.GaleConfig.apiVersion,
                   controllerName,
                   (_template.StartsWith("/") ? "" : "/"),
                   _template
           );

            return (new System.Web.Http.RouteAttribute(template)
            {

            } as IDirectRouteFactory).CreateRoute(context);
        }

        /// <summary>
        /// The route template describing the URI pattern to match against.
        /// </summary>
        public string Template
        {
            get { return _template; }
        }
    }
}