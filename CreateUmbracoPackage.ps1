param([string]$packageDirectory, [string]$buildConfiguration)

$workingPackageFilePath = Join-Path $PSScriptRoot "package.xml"
Copy-Item -Path (Join-Path $packageDirectory "package.xml") -Destination $workingPackageFilePath

$workingPackageFile = [xml](Get-Content $workingPackageFilePath)

$xpathForFiles = "//file"
$xpathForName = "//info/package/name"
$xpathForVersion = "//info/package/version"

$fileNodes = $workingPackageFile.SelectNodes($xpathForFiles)
$nameNode = $workingPackageFile.SelectSingleNode($xpathForName)
$versionNode = $workingPackageFile.SelectSingleNode($xpathForVersion)

$packageName = $nameNode.InnerText

$filepaths = @($workingPackageFilePath)

foreach ($fileNode in $fileNodes)
{
	$nameNode = $fileNode["orgName"]
	$pathNode = $fileNode["orgPath"]
	
	$name = $nameNode.InnerText
	$path = $pathNode.InnerText.Replace("/", "\").Replace("\bin", ("\bin\" + $buildConfiguration))
	$filePath = Join-Path $packageDirectory ($path + "\" + $name)
	
	if($filePath.Contains($packageName + ".dll") -and [string]::IsNullOrWhiteSpace($versionFilePath))
	{
		$fileStream = ([System.IO.FileInfo] (Get-Item $filePath)).OpenRead()
		$assemblyBytes = new-object byte[] $fileStream.Length
		$fileStream.Read($assemblyBytes, 0, $fileStream.Length)
		$fileStream.Close()

		$assemblyLoaded = [System.Reflection.Assembly]::Load($assemblyBytes)
		$version = $assemblyLoaded.GetName().Version.ToString()
		$versionNode.InnerText = $version
	}

	$filepaths += $filePath
}

$workingPackageFile.Save($workingPackageFilePath)

Compress-Archive -LiteralPath $filepaths -CompressionLevel Optimal -DestinationPath ($packageName + ".zip") -Update

Remove-Item -Path $workingPackageFilePath