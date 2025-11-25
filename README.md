# Online Casino Bonuses API

Application is hosted on Azure:  
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/swagger/index.html

## Authentication

You can authenticate with any combination of username and password, it will return access token.

**POST:**  
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Auth/login  
https://localhost:7050/api/Auth/login

**Request Body:**
```json
{
    "username": "admin",
    "password": "admin"
}
```

## Predefined Bonus Types

Use the ID in the payloads:

ID: 1, Name: Welcome

ID: 2, Name: Deposit

ID: 3, Name: FreeSpins


## API Endpoints

**Get All Bonuses (with paging)**

GET:

https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Bonus/all?pageNumber=1&pageSize=10

https://localhost:7050/api/Bonus/all?pageNumber=1&pageSize=10

**Create New Bonus for Player**

POST:

https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Bonus/create
https://localhost:7050/api/Bonus/create

Request Body:
```
json
{
  "playerId": 6,
  "type": 3,
  "amount": 10,
  "expiresAt": "2025-11-26T13:51:19.290Z"
}
```

**Update Existing Bonus**

PUT:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Bonus/update/1

https://localhost:7050/api/Bonus/update/1

Request Body:
```
json
{
  "amount": 600,
  "expiresAt": "2025-11-30T02:05:36.190Z"
}
```

**Delete Existing Bonus**

DELETE:

https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/Bonus/delete/1

https://localhost:7050/api/Bonus/delete/1

**List Audit Logs**

GET:
https://onlinecasino-api-hceugqfrhjd7cydh.polandcentral-01.azurewebsites.net/api/AuditLogs/all?pageNumber=1&pageSize=10

https://localhost:7050/api/AuditLogs/all?pageNumber=1&pageSize=10

## Notes

All endpoints (except authentication) require JWT token in Authorization header

Use the token returned from login in format and paste it in Authorize button in Swagger UI and will be used on every next request: Bearer {token}

Audit logs are automatically created for all bonus operations
