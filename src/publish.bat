echo "Remove release directory"
if exist "./VoicemodPowertools/bin/Release" rd /s /q "./VoicemodPowertools/bin/Release"

echo "Starting to publish"
::asdasd asdasd
::dotnet publish -p:PublishProfile="./run/Publish FrameworkDependent.run.xml"
dotnet build -c Release -p:PublishProfile="./run/Publish FrameworkDependent.run.xml"