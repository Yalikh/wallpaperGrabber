$outPath = "$PSScriptRoot\..\publish"
$project = "$PSScriptRoot\..\src\WallpaperGrabber\WallpaperGrabber.csproj"

Get-ChildItem $outPath | Remove-Item -Recurse -Force
dotnet publish $project -o $outPath --no-self-contained -c Release