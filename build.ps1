# This script builds 
$ErrorActionPreference = "Stop"

# In order to run this script, you need to have GIT command line installed and configured to run using SSH (not HTTP)
$RepoUrl = "git@git.aristech.net:server/KorePracticalTest.git"
$RepoBranch = "interview"
$ArchiveName = (Get-Date -Format "yyyy-MM-dd")+"-{revision}-interview-test.zip" # {revision} gets replaced by short SHA1
$ArchivePath = Join-Path $PWD $ArchiveName
$TempDir = [System.IO.Path]::GetTempPath()
$WorkDir = Join-Path $TempDir "PracticalTest"
$GitDir = Join-Path $WorkDir ".git"
$GitExe = "git"
$VersionInfoPath = Join-Path $WorkDir "version-info.txt"

# Prepare WorkDir
Write-Host -ForegroundColor Cyan "Building the PracticalTest package for a candidate"
Write-Host -ForegroundColor Cyan "`nGit clone $RepoUrl branch $RepoBranch into $WorkDir"
if (Test-Path $WorkDir) {
    Write-Host "Clearing $WorkDir..."
    Remove-Item -Force -Recurse $WorkDir
}

# Git clone + checkout
Write-Host "Git clone $RepoUrl $WorkDir..."
& $GitExe clone $RepoUrl $WorkDir
if ($LastExitCode -ne 0) {
    throw "Git exited with $LastExitCode status"
}
Write-Host "Git checkout $RepoBranch"
& $GitExe --git-dir=$GitDir --work-tree=$WorkDir checkout -b $RepoBranch origin/$RepoBranch
if ($LastExitCode -ne 0) {
    throw "Git exited with $LastExitCode status"
}

# Read revision, and determine archive package
Write-Host "Detecting revision" 
$Revision = & $GitExe --git-dir=$GitDir --work-tree=$WorkDir rev-parse --short=7 HEAD
Write-Host "Git revision: $Revision"
if ($LastExitCode -ne 0) {
    throw "Git exited with $LastExitCode status"
}
$ArchiveName = $ArchiveName.replace("{revision}", $Revision)
Write-Host "Full qualified Archive: $ArchiveName"
$ArchivePath = $ArchivePath.replace("{revision}", $Revision)
Write-Host "Full qualified Archive Path: $ArchivePath"

# Delete git history
Write-Host -ForegroundColor Cyan "`nRemoving Git history from Aris Repository"
Write-Host "Deleting .git directory"
Remove-Item -Recurse -Force $GitDir
Write-Host "Git init..."
& $GitExe --git-dir=$GitDir --work-tree=$WorkDir init
if ($LastExitCode -ne 0) {
    throw "Git exited with $LastExitCode status"
}

# Create version info file
Write-Host "Create $VersionInfoPath"
$TimeStamp = (Get-Date).ToUniversalTime().ToString("o")
Write-Output "Aris Interview Test" > $VersionInfoPath
Write-Output "Timestamp: $TimeStamp " >> $VersionInfoPath
Write-Output "Repository: $RepoUrl" >> $VersionInfoPath
Write-Output "Branch: $RepoBranch" >> $VersionInfoPath
Write-Output "Revision: $Revision" >> $VersionInfoPath

# Commit all files
Write-Host "git add --all"
& $GitExe --git-dir=$GitDir --work-tree=$WorkDir add --all
if ($LastExitCode -ne 0) {
    throw "Git exited with $LastExitCode status"
}
Write-Host "git commit"
& $GitExe --git-dir=$GitDir --work-tree=$WorkDir commit -m "Added all interview files." -q --author="Somebody <somebody@aristx.com>"
if ($LastExitCode -ne 0) {
    throw "Git exited with $LastExitCode status"
}

# Create ZIP archive
Write-Host -ForegroundColor Cyan "`nCreating $ArchivePath"
if (Test-Path $ArchivePath) {
    Write-Host "Deleting $ArchivePath..."
    Remove-Item $ArchivePath
}
(Get-Item -Force $GitDir).Attributes = "" # Unhide .git or Compress-Archive will skip it
Compress-Archive -Path "$WorkDir\*" -DestinationPath $ArchivePath

# Create ZIP archive
Write-Host -ForegroundColor Cyan "Clearing $WorkDir"
Remove-Item -Recurse -Force $WorkDir

Write-Host -ForegroundColor Green "`nInterview ZIP Archive Successfully Created"
