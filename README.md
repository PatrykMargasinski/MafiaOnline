# MafiaOnline

To run project:

- Make a copy of appsettings.copy.json file and name it appsettings.json.
- Fill missing information in it.
- Create database. i.e. ```CREATE DATABASE MafiaDB```
- Make a migration. In MafiaOnline.DataAccess folder use commend dotnet ```ef --startup-project ../MafiaOnline database update```
