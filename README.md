# InfoTrackTest

Post Route to Place request for settlement Booking 

Route - /api/Settlement/Booking
If Settlement Booking Request 
  - Invalid Name (Null or Empty) then return Bad Request
  - Invalid Time or out of office hours time then return Bad request
  - Valid Request and no 4 concurrent bookings then Book and return success
  - Valid Request and 4 concurrent bookings present in system then return Conflict

Sample Request 
  {
  "bookingTime": "string",
  "name": "string"
  }

Sample Success Response 
  {
  "status": "string",
  "data": {
    "bookingId": "string"
  },
  "message": "string"
}

Please refer to Launch settings for port info based on what we run the app on 

Running Locally (Docker not included)

  IISExpress -> https://localhost:44358/api/Settlement/Booking

  Http -> http://localhost:5282/api/Settlement/Booking
  
  https -> https://localhost:7002/api/Settlement/Booking
