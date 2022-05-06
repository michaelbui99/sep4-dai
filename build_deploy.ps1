
$ProjectRootDirectory = pwd

iex 'Set-Location ./WebAPI'

iex 'pwd'

if (Test-Path -Path './out'){
    iex 'echo "output directory already exists, removing directory..."'
    iex 'rm -R -Force ./out'
}

iex 'echo "Building deployment"'
Start-Process './publish.bat' -wait
iex 'Set-Location ./out'



iex 'echo "Zipping deployment"'
iex 'Compress-Archive ./* deploy.zip | Out-Null' && iex 'mv ./deploy.zip $ProjectRootDirectory -Force'

iex 'Set-Location $ProjectRootDirectory'



