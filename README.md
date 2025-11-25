# Online Casino Bonuses API
Application is hosted on Azure:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/swagger/index.html
You can authenticate with any combination of username and password, it will return access token.
POST:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Auth/login
https://localhost:7050/api/Auth/login
{
    "username" : "admin",
    "password" : "admin"
}

There is already predefined bonus types stored in the enum in the code:
Use the Id in the payloads:
Id: 1, Name: Welcome
Id: 2, Name: Deposit
Id: 3, Name: FreeSpins

Test payloads:
Get all bonuses with paging:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Bonus/all?pageNumber=1&pageSize=10'
https://localhost:7050/api/Bonus/all?pageNumber=1&pageSize=10'

Create new bonus for player:
POST:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Bonus/create
https://localhost:7050/api/api/Bonus/create
{
  "playerId": 6,
  "type": 3,
  "amount": 10,
  "expiresAt": "2025-11-26T13:51:19.290Z"
}

Update existing bonus
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/Bonus/update/1
https://localhost:7050/api/Bonus/update/1
{
  "amount": 600,
  "expiresAt": "2025-11-30T02:05:36.190Z"
}

Delete existing bonus:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/
https://localhost:7050/api/Bonus/delete/1

List audit logs ordered by last changed:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/AuditLogs/all?pageNumber=1&pageSize=10
https://localhost:7050/api/AuditLogs/all?pageNumber=1&pageSize=10
