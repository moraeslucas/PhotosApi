@ECHO OFF

ECHO Initializing the PhotosApi with Swagger interface...
start "" https://localhost:5001/swagger/index.html
ECHO (The web page may take some time to refresh and run properly)
ECHO -------------------------------------------------------------

cd "%CD%"\PhotosApi
dotnet run