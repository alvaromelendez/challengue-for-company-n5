## Steps for run the application

1) Open a terminal and go to the main path of the solution.
```sh
../challenge-backend-tl
```
2) Run the following command.
```sh
docker compose up -d
```
3) Go to the following path of the solution.
```sh
cd ../challenge-backend-tl/src/Permission.Infrastructure
```
4) Run the following command. 
```sh
dotnet ef database update --startup-project ../Permission.Api
```
5) To access the database, enter the following credentials in any tools. SqlManagment,     SqlElectron, DBeaver, etc.

| Key | Value |
| ------ | ------ |
| Host | localhost |
| Database/Scheme | master |
| Port | 1433 |
| UserName | sa |
| Password | Pass@word |

6) Insert the following record in the database `PermissionDb`.
```sh
INSERT INTO PermissionDb.dbo.Employees (FirstName, LastName) VALUES('Alvaro', 'Melendez');
```

7) Go to the following link and test the api: http://localhost/swagger/index.html

| Api | Request |
| ------ | ------ |
| /Permission/request-permission | {"employeeId": 1,"datePermission": "2023-04-13T01:39:37.984Z","comment": "permiso1","permissionType": 1} |
| /Permission/get-permissions | 1 |

8) To validate the data persist on Kafka, run the following command:
```sh
docker exec --interactive --tty broker kafka-console-consumer --bootstrap-server broker:9092 --topic requests --from-beginning
```
9) To validate the data persist on Elastic, go to the following link: 
http://localhost/swagger/index.html

| UserName | Password |
| ------ | ------ |
| elastic | password |