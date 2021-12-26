FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY . App/
WORKDIR /App
ENV DOTNET_EnableDiagnostics=0
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
RUN dotnet publish -c Release
ENTRYPOINT ["dotnet", "./bin/Release/net6.0/publish/house-api.dll"]
