Import-Module -Name NetNat -Verbose
Get-NetNat | Remove-NetNat -Confirm:$false -Verbose # https://github.com/docker/for-win/issues/598

python -m pip install --upgrade pip
pip install requests[security]
$env:PATH += ";C:\Users\appveyor\AppData\Roaming\Python\Scripts"
pip install --user aws-sam-cli