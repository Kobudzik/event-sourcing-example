FROM mcr.microsoft.com/dotnet/sdk:5.0 AS buildStage
COPY /src /app
WORKDIR /app
RUN dotnet build WebUI/WebUI.csproj -c Release -o buildOutputDir

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=buildStage /app/buildOutputDir .
COPY aspnetapp.pfx .
EXPOSE 81
ENTRYPOINT ["dotnet", "CommandsRegistry.WebUI.dll"]