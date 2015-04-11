using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.Model;
using timw255.Sitefinity.RestClient.ServiceWrappers.Configuration;

namespace timw255.Sitefinity.PowerShell
{
    [Cmdlet(VerbsCommon.Get, "SFConfigSections")]
    public class Get_SFConfigSections : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string NodeName;

        [Parameter]
        public SitefinityRestClient Client;

        protected override void ProcessRecord()
        {
            Guid runspaceId = Guid.Empty;

            using (var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace))
            {
                runspaceId = ps.Runspace.InstanceId;

                if (!SFClientUtil.Clients.ContainsKey(runspaceId) && Client == null)
                {
                    ErrorRecord err = new ErrorRecord(new ArgumentException(), "", ErrorCategory.InvalidArgument, this);
                    err.ErrorDetails = new ErrorDetails(this, "Resources.DisplayStrings.ResourceManager", "InvalidOrNoSite", null);
                    ThrowTerminatingError(err);
                }

                SitefinityRestClient c = SFClientUtil.Clients[runspaceId];

                ConfigSectionItemsServiceWrapper service = new ConfigSectionItemsServiceWrapper(c);

                IEnumerable<UISectionItem> sectionItems = service.GetConfigSetionItems(NodeName, "", "", "", "Form", "").Items;

                WriteObject(sectionItems);
            }
        }
    }

    [Cmdlet(VerbsCommon.Get, "SFConfigSection")]
    public class Get_SFConfigSection : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string NodeName;

        [Parameter]
        public SitefinityRestClient Client;

        protected override void ProcessRecord()
        {
            Guid runspaceId = Guid.Empty;

            using (var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace))
            {
                runspaceId = ps.Runspace.InstanceId;

                if (!SFClientUtil.Clients.ContainsKey(runspaceId) && Client == null)
                {
                    ErrorRecord err = new ErrorRecord(new ArgumentException(), "", ErrorCategory.InvalidArgument, this);
                    err.ErrorDetails = new ErrorDetails(this, "Resources.DisplayStrings.ResourceManager", "InvalidOrNoSite", null);
                    ThrowTerminatingError(err);
                }

                SitefinityRestClient c = SFClientUtil.Clients[runspaceId];

                ConfigSectionItemsServiceWrapper service = new ConfigSectionItemsServiceWrapper(c);

                IEnumerable<UISectionItem> sectionItems = service.GetConfigSetionItems(NodeName, "", "", "", "Form", "").Items;

                WriteObject(sectionItems);
            }
        }
    }

    [Cmdlet(VerbsCommon.Set, "SFConfigSection")]
    public class Set_SFConfigSection : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string NodeName;

        [Parameter(Mandatory = true)]
        public Hashtable Properties;

        [Parameter]
        public SitefinityRestClient Client;

        protected override void ProcessRecord()
        {
            Guid runspaceId = Guid.Empty;

            using (var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace))
            {
                runspaceId = ps.Runspace.InstanceId;

                if (!SFClientUtil.Clients.ContainsKey(runspaceId) && Client == null)
                {
                    ErrorRecord err = new ErrorRecord(new ArgumentException(), "", ErrorCategory.InvalidArgument, this);
                    err.ErrorDetails = new ErrorDetails(this, "Resources.DisplayStrings.ResourceManager", "InvalidOrNoSite", null);
                    ThrowTerminatingError(err);
                }

                SitefinityRestClient c = SFClientUtil.Clients[runspaceId];

                ConfigSectionItemsServiceWrapper service = new ConfigSectionItemsServiceWrapper(c);

                string[][] properties = HashtableTo2DArray(Properties);

                bool result = service.SaveBatchConfigSection(properties, NodeName, "", "", "", "Form", "");

                WriteObject(result);
            }
        }

        private string[][] HashtableTo2DArray(Hashtable table)
        {
            List<string[]> propertyBag = new List<string[]>();

            foreach (DictionaryEntry d in table)
            {
                propertyBag.Add(new string[] { "Value", (string)d.Value });
                propertyBag.Add(new string[] { "Key", (string)d.Key });
            }

            return propertyBag.ToArray();
        }
    }
}
