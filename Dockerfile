FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080

# O comando 'publish' do YAML jogará os arquivos aqui
COPY ./publish .

# Nome exato baseado na sua pasta 'src/FSI.SupportPointSystem.Api'
ENTRYPOINT ["dotnet", "FSI.SupportPointSystem.Api.dll"]