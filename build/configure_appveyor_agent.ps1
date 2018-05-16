Import-Module -Name NetNat
Get-NetNat | Remove-NetNat -Confirm:$false # https://github.com/docker/for-win/issues/598

netstat -ano | findstr "8000" | Select-String -Pattern "\d+$" -AllMatches |  % {$_.Matches } | foreach { tasklist |select-string -Pattern $_.Value } | Get-Unique

python -m pip install --upgrade pip
pip install requests[security]
$env:PATH += ";C:\Users\appveyor\AppData\Roaming\Python\Scripts"
pip install --user aws-sam-cli