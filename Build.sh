@echo off
echo "Linux Docker build"
echo "First Bulid SPA "
cd src/Simpleness.App/ClientApp 
start npm run build
pause
echo "build application"
cd ..
dotnet publish -c Release -o bin/publish/Release/netcoreapp2.1
cd /bin/publish/Release/netcoreapp2.1
echo "ִ�й���Docker"
echo "publish success"