rm approot -Recurse -Force -Confirm:$false
dotnet publish --configuration Release --output ./approot
cd approot;
dotnet ShamirApp.dll