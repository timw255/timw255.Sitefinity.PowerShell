using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.RestClient;

namespace timw255.Sitefinity.PowerShell
{
    public static class SFClientUtil
    {
        public static readonly Dictionary<Guid, SitefinityRestClient> Clients = new Dictionary<Guid, SitefinityRestClient>();
    }
}
