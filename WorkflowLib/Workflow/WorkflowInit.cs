using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using OptimaJet.Workflow.Core.Builder;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using OptimaJet.Workflow.DbPersistence;
using OptimaJet.Workflow.Plugins;

namespace WorkflowLib.Workflow
{
    public static class WorkflowInit
    {
        private static Lazy<WorkflowRuntime> _runtime = new Lazy<WorkflowRuntime>(InitRuntime);
        
        public static WorkflowRuntime Runtime => _runtime.Value;

        private static WorkflowRuntime InitRuntime()
        {
            var loopPlugin = new LoopPlugin();
            var basicPlugin = new BasicPlugin();
            
            basicPlugin.UsersInRoleAsync += UsersInRoleAsync;
            basicPlugin.CheckPredefinedActorAsync += CheckPredefinedActorAsync;
            basicPlugin.GetPredefinedIdentitiesAsync += GetPredefinedIdentitiesAsync;
            basicPlugin.UpdateDocumentStateAsync += UpdateDocumentStateAsync;
            basicPlugin.WithActors(new List<string>() {"Manager", "Author"});

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            var dbProvider = new MSSQLProvider(connectionString);
            
            var builder = new WorkflowBuilder<XElement>(
                dbProvider,
                new OptimaJet.Workflow.Core.Parser.XmlWorkflowParser(),
                dbProvider
            ).WithDefaultCache();
            
            WorkflowRuntime.RegisterLicense("DEV-REVWOjA3LjI2LjIwMjU6ZXlKTllYaE9kVzFpWlhKUFprRmpkR2wyYVhScFpYTWlPaTB4TENKTllYaE9kVzFpWlhKUFpsUnlZVzV6YVhScGIyNXpJam90TVN3aVRXRjRUblZ0WW1WeVQyWlRZMmhsYldWeklqb3RNU3dpVFdGNFRuVnRZbVZ5VDJaVWFISmxZV1J6SWpvdE1Td2lUV0Y0VG5WdFltVnlUMlpEYjIxdFlXNWtjeUk2TFRGOTp2OWZ0bWpvRnBSSTI0S2RIeUlPQVhlZ1NyQ2dXRzc5bEh4R2dkdDg0c00rVGRxTWRyMlo5QzM0SHZOZ0VmMG1VZXQvalNJcHdadVpkYTNMd29EVHcwWEtTdHYzQitCWGx1VnNzdExjVFMvQ1RsQ0l5djY5NGRmUVIvNnZlUXV0SGVVZXg2TmhMekUyQTd5SE4rd3RmdkZIWkRBUU85NHZya3d0V0tVSy91VFU9");
            var runtime = new WorkflowRuntime()
                .WithBuilder(builder)
                // .WithActionProvider()
                // .WithRuleProvider()
                //.WithDesignerAutocompleteProvider()
                .WithPersistenceProvider(dbProvider)
                .SwitchAutoUpdateSchemeBeforeGetAvailableCommandsOn()
                .RegisterAssemblyForCodeActions(Assembly.GetExecutingAssembly())
                .WithPlugins(null, basicPlugin, loopPlugin)
                //.WithExternalParametersProvider()
                .CodeActionsDebugOn()
                .AsSingleServer()
                .Start();

            return runtime;

        }

        private static async Task UpdateDocumentStateAsync(ProcessInstance processinstance, string statename, string localizedstatename)
        {
            return;
        }

        private static async Task<IEnumerable<string>> GetPredefinedIdentitiesAsync(ProcessInstance processinstance, WorkflowRuntime runtime, string parameter)
        {
            return new List<string>();
        }

        private static async Task<IEnumerable<string>> UsersInRoleAsync(string rolename, ProcessInstance processinstance)
        {
            return new List<string>();
        }

        private static async Task<bool> CheckPredefinedActorAsync(ProcessInstance processinstance, WorkflowRuntime runtime, string identityid, string parameter)
        {
            return false;
        }
        
        
    }
}
