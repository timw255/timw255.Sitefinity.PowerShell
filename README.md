# timw255.Sitefinity.PowerShell
Tools to administer Sitefinity via PowerShell

## Example
```PowerShell
# import module
Import-Module C:\<some path>\timw255.Sitefinity.PowerShell.dll;

# login
New-SFClient -Username admin -Password password -Url http://sf805700.local/;

# create user
New-SFUser -Username jsmith -Password p@ssword123 -Email jsmith@somedomain.com -ProfileData @{FirstName="John"; LastName="Smith"}

# get config
Get-SFConfigSection -NodeName "ServicesPaths_8,systemConfig_0";

# update config
Set-SFConfigSection -NodeName "ServicesPaths_8,systemConfig_0" -Properties @{WorkflowBaseUrl="http://sf735619.local/"};

# logout
Remove-SFClient
```
