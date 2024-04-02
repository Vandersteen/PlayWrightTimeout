FROM mcr.microsoft.com/playwright/dotnet:v1.41.2-jammy

COPY . .

RUN dotnet restore

RUN dotnet build -c Release

RUN dotnet publish -c Release -o /publish

ENTRYPOINT ["sh", "-c", "exec dotnet /publish/PlayWrightTimeout.dll"]
