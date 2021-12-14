﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WorkflowLib.Workflow;
using WorkflowRuntime = OptimaJet.Workflow.Core.Runtime.WorkflowRuntime;
namespace WorkFlowDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public WorkflowRuntime Runtime;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var runtime = WorkflowInit.Runtime; //for early init and start
        }
    }
    
}
