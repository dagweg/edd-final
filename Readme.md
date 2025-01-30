## House Rental System

The project is a simple house rental system that allows users to list their houses for rent and other users to rent them.

- CRUD for User Management and House Listings
- Authentication and Authorization using JWT
- Global Exception Handling for centralized error handling and logging
- Pagination for Query requests of large data

Test each http request found in `/Requests` folder.

Requests
┣ CreateHouseListing.http
┣ DeleteListingById.http
┣ GetListingById.http
┣ GetListingsByHost.http
┣ GetListingsByName.http
┣ LoginUser.http
┣ PatchListingById.http
┣ RegisterUser.http
┗ RetrieveAllListings.http

These api endpoints are implemented in the project.

Steps to use the api:

1. Register the User. It returns 201 Created.
2. Login the User. It returns 200 OK with JWT token and Expiration Time.
3. Use the JWT token in the Authorization header for the following requests as follows:

```json
Authorization: Bearer <IssuedJwtToken>
```

4. Create a House Listing. It returns 201 Created.
5. Retrieve all House Listings. It returns 200 OK with the list of House Listings.
6. Retrieve a House Listing by Id. It returns 200 OK with the House Listing.
