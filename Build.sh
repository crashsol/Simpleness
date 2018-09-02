@echo off
echo "Linux Docker build"
echo "First Bulid SPA "
cd src/Simpleness.App/ClientApp 
call npm run build
echo "build application"
cd ..
dotnet publish -c Release -o bin/publish/Release/netcoreapp2.1
cd /bin/publish/Release/netcoreapp2.1
echo "Ö´ÐÐ¹¹½¨Docker"
echo "publish success"