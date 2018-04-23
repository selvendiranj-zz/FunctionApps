$requestBody = Get-Content $req -Raw | ConvertFrom-Json

# Set Service Principal credentials
# SP_PASSWORD, SP_USERNAME, TENANTID are app settings
$secpasswd = ConvertTo-SecureString $env:SP_PASSWORD -AsPlainText -Force;
$mycreds = New-Object System.Management.Automation.PSCredential ($env:SP_USERNAME, $secpasswd)
Add-AzureRmAccount -ServicePrincipal -Tenant $env:TENANTID -Credential $mycreds;
$context = Get-AzureRmContext;
Set-AzureRmContext -Context $context;

# Start VM
Start-AzureRmVM -ResourceGroupName "identityadfsgroup" `
                -Name "identityadfs" `
                | Out-String

# Get VM
$Result = (Get-AzureRmVM -ResourceGroupName "identityadfsgroup" `
                         -Name "identityadfs" `
                         -Status) | Out-String
Out-File -Encoding Ascii -FilePath $res -inputObject $Result