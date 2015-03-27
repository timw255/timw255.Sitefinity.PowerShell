using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.SitefinityClient;

namespace timw255.Sitefinity.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SFClient")]
    public class New_SFClient : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Username;

        [Parameter(Mandatory = true)]
        public string Password;

        [Parameter(Mandatory = true)]
        public string Url;

        protected override void ProcessRecord()
        {
            Guid runspaceId = Guid.Empty;

            using (var ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace))
            {
                runspaceId = ps.Runspace.InstanceId;

                if (SFClientUtil.Clients.ContainsKey(runspaceId))
                {
                    SFClientUtil.Clients[runspaceId].Dispose();
                }

                SitefinityRestClient c = new SitefinityRestClient(Username, Password, Url);

                SFClientUtil.Clients.Add(runspaceId, c);

                WriteObject(c);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "SFClient")]
    public class Remove_SFClient : PSCmdlet
    {
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

                SFClientUtil.Clients[runspaceId].Dispose();
                SFClientUtil.Clients.Remove(runspaceId);
            }
        }
    }
}
