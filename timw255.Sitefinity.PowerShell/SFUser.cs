using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.Model;
using timw255.Sitefinity.RestClient.ServiceWrappers.Security;

namespace timw255.Sitefinity.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SFUser")]
    public class New_SFUser : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Username;

        [Parameter(Mandatory = true)]
        public string Password;

        [Parameter(Mandatory = true)]
        public string Email;

        [Parameter(Mandatory = true)]
        public Hashtable ProfileData;

        [Parameter]
        public bool IsApproved;

        [Parameter]
        public Guid UserId;

        [Parameter]
        public string MembershipProvider = String.Empty;

        [Parameter]
        public string ProfileProvider = String.Empty;

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
                    err.ErrorDetails = new ErrorDetails("No Sitefinity Client");
                    ThrowTerminatingError(err);
                }

                SitefinityRestClient c = SFClientUtil.Clients[runspaceId];

                UsersServiceWrapper service = new UsersServiceWrapper(c);

                WcfMembershipUser user = new WcfMembershipUser();

                user.UserName = Username;
                user.Password = Password;
                user.Email = Email;
                user.IsApproved = IsApproved;

                var profile = new Dictionary<string, Object>();

                profile.Add("__type", "Telerik.Sitefinity.Security.Model.SitefinityProfile");

                foreach (DictionaryEntry d in ProfileData)
                {
                    profile.Add((string)d.Key, d.Value);
                }

                profile.Add("Id", Guid.NewGuid());
                profile.Add("LastModified", DateTime.UtcNow);
                profile.Add("DateCreated", DateTime.UtcNow);
                profile.Add("__providerName", ProfileProvider);

                JsonSerializerSettings serializerSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };

                var serializer = JsonSerializer.Create(serializerSettings);

                serializer.Converters.Add(new KeyValuePairConverter());

                string jsonProfileData = String.Empty;

                using (var writer = new StringWriter())
                {
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        serializer.Serialize(jsonWriter, profile);
                        jsonProfileData = writer.ToString();
                    }
                }

                user.ProfileData = "{\"Telerik.Sitefinity.Security.Model.SitefinityProfile\":" + jsonProfileData + "}";

                WcfMembershipUser u = service.CreateUser(user, UserId, MembershipProvider);

                WriteObject(u);
            }
        }
    }
}
