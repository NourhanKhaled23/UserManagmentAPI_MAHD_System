

### **Summary**
- The API has endpoints for authentication, user profile management, and admin-specific actions.
- All endpoints (except `/api/Auth/login` and `/api/Auth/register`) require a valid JWT token.
- Admin endpoints require the `Admin` role.
### **API Documentation**

#### **Base URL**
```
https://localhost:7233
```

---

### **Authentication**
All endpoints (except `/api/Auth/login` and `/api/Auth/register`) require a valid JWT token in the `Authorization` header.

#### **Authorization Header**
```
Authorization: Bearer <your_jwt_token>
```

---

### **Endpoints**

#### **1. Authentication Endpoints**

##### **POST /api/Auth/register**
- **Description**: Register a new user.
- **Request Body**:
  ```json
  {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "password": "password123"
  }
  ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "User registered successfully."
    }
    ```
  - **Error (400 Bad Request)**:
    ```json
    {
      "error": "Email is already registered."
    }
    ```

##### **POST /api/Auth/login**
- **Description**: Log in and obtain a JWT token.
- **Request Body**:
  ```json
  {
    "email": "john.doe@example.com",
    "password": "password123"
  }
  ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    }
    ```
  - **Error (401 Unauthorized)**:
    ```json
    {
      "error": "Invalid email or password."
    }
    ```

---

#### **2. User Endpoints**

##### **GET /api/User/profile**
- **Description**: Get the profile of the authenticated user.
- **Authorization**: Requires a valid JWT token.
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "id": 1,
      "firstName": "John",
      "lastName": "Doe",
      "email": "john.doe@example.com",
      "role": "Student"
    }
    ```
  - **Error (401 Unauthorized)**:
    ```json
    {
      "error": "Unauthorized."
    }
    ```

##### **PUT /api/User/profile**
- **Description**: Update the profile of the authenticated user.
- **Authorization**: Requires a valid JWT token.
- **Request Body**:
  ```json
  {
    "firstName": "John",
    "lastName": "Smith"
  }
  ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "Profile updated successfully."
    }
    ```
  - **Error (400 Bad Request)**:
    ```json
    {
      "error": "Failed to update profile."
    }
    ```

##### **PUT /api/User/change-password**
- **Description**: Change the password of the authenticated user.
- **Authorization**: Requires a valid JWT token.
- **Request Body**:
  ```json
  {
    "oldPassword": "password123",
    "newPassword": "newpassword123"
  }
  ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "Password changed successfully."
    }
    ```
  - **Error (400 Bad Request)**:
    ```json
    {
      "error": "Failed to change password. Check your old password."
    }
    ```

##### **DELETE /api/User/delete-account**
- **Description**: Delete the account of the authenticated user.
- **Authorization**: Requires a valid JWT token.
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "Account deleted successfully."
    }
    ```
  - **Error (400 Bad Request)**:
    ```json
    {
      "error": "Failed to delete account."
    }
    ```

---

#### **3. Admin Endpoints**

##### **GET /api/Admin/users**
- **Description**: Get a list of all users (Admin only).
- **Authorization**: Requires a valid JWT token with the `Admin` role.
- **Response**:
  - **Success (200 OK)**:
    ```json
    [
      {
        "id": 1,
        "email": "john.doe@example.com",
        "role": "Student"
      },
      {
        "id": 2,
        "email": "admin@example.com",
        "role": "Admin"
      }
    ]
    ```
  - **Error (403 Forbidden)**:
    ```json
    {
      "error": "Forbidden. Admin access required."
    }
    ```

##### **PUT /api/Admin/set-role/{userId}**
- **Description**: Set the role of a user (Admin only).
- **Authorization**: Requires a valid JWT token with the `Admin` role.
- **Request Body**:
  ```json
  {
    "role": "Admin"
  }
  ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "User role updated successfully."
    }
    ```
  - **Error (400 Bad Request)**:
    ```json
    {
      "error": "Failed to update user role."
    }
    ```

##### **DELETE /api/Admin/delete-user/{userId}**
- **Description**: Delete a user (Admin only).
- **Authorization**: Requires a valid JWT token with the `Admin` role.
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "User deleted successfully."
    }
    ```
  - **Error (400 Bad Request)**:
    ```json
    {
      "error": "Failed to delete user."
    }
    ```

---

### **Testing the Endpoints**

#### **1. Register a New User**
```bash
curl -X 'POST' \
  'https://localhost:7233/api/Auth/register' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "password123"
}'
```

#### **2. Log in as the User**
```bash
curl -X 'POST' \
  'https://localhost:7233/api/Auth/login' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "john.doe@example.com",
  "password": "password123"
}'
```

#### **3. Get User Profile**
```bash
curl -X 'GET' \
  'https://localhost:7233/api/User/profile' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer <your_jwt_token>'
```

#### **4. Update User Profile**
```bash
curl -X 'PUT' \
  'https://localhost:7233/api/User/profile' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer <your_jwt_token>' \
  -H 'Content-Type: application/json' \
  -d '{
  "firstName": "John",
  "lastName": "Smith"
}'
```

#### **5. Change Password**
```bash
curl -X 'PUT' \
  'https://localhost:7233/api/User/change-password' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer <your_jwt_token>' \
  -H 'Content-Type: application/json' \
  -d '{
  "oldPassword": "password123",
  "newPassword": "newpassword123"
}'
```

#### **6. Delete User Account**
```bash
curl -X 'DELETE' \
  'https://localhost:7233/api/User/delete-account' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer <your_jwt_token>'
```

#### **7. Get All Users (Admin Only)**
```bash
curl -X 'GET' \
  'https://localhost:7233/api/Admin/users' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer <admin_jwt_token>'
```

#### **8. Set User Role (Admin Only)**
```bash
curl -X 'PUT' \
  'https://localhost:7233/api/Admin/set-role/1' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer <admin_jwt_token>' \
  -H 'Content-Type: application/json' \
  -d '{
  "role": "Admin"
}'
```

#### **9. Delete User (Admin Only)**
```bash
curl -X 'DELETE' \
  'https://localhost:7233/api/Admin/delete-user/1' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer <admin_jwt_token>'
```

---



