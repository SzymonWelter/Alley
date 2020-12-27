FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

RUN curl -sL https://deb.nodesource.com/setup_current.x |  bash -
RUN apt-get install -y nodejs
RUN npm --version

WORKDIR /app

COPY ./*.sln  ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore -r linux-x64

COPY . .
RUN dotnet publish ./Alley -c release -r linux-x64 -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:3.1
EXPOSE 80
EXPOSE 443
EXPOSE 8080

WORKDIR /app
COPY --from=build /out/* ./
ENTRYPOINT [ "dotnet", "Alley.dll" ]