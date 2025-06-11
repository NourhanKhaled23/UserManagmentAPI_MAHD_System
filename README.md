📖 User Management System

An ASP.NET Core Web API for secure user authentication, profile management, and role-based access control.
It uses Entity Framework Core for data persistence, JWT tokens for authentication, and SMTP for email notifications.


📦 System Components Summary
Component	Purpose
AuthController	Handles registration, login, tokens
UserController	Profile actions, password changes
AdminController	User listing, role changes, deletions
Database	User data, hashed passwords, roles
JWT Middleware	Token validation, role-based access
API Key Auth	Protects unauthenticated routes



✅ Summary
All public routes require X-Api-Key

All protected routes require Bearer JWT

Role-check middleware guards admin endpoints

Refresh token flow prevents re-login fatigue

Password reset is tokenized and time-bound

Clean separation: AuthController, UserController, AdminController




📑 Overview
The system supports:

User registration, login, password reset, and profile management.

Role-based access control for Admin, Instructor, and Student roles.

Administrative operations (view users, assign roles, delete users).

Security features like API key validation, JWT authentication, rate limiting, and input validation.

🏗️ System Architecture
Layered Architecture:

Presentation Layer: API controllers (AuthController, UserController, AdminController).

Application Layer: Services (AuthService, UserService, EmailService) and DTOs.

Domain Layer: Entities (User, UserSettings, RefreshToken) and enums (UserRoles).

Infrastructure Layer: Repositories (UserRepository, SettingsRepository) and AppDbContext.

Middleware: Custom middleware for API key enforcement, JWT validation, error handling, and logging.

Utilities: PasswordHelper (hashing) and JwtHelper (token generation).

📦 Key Components
Entities
User: Core user information.

UserSettings: User preferences (e.g., notifications).

RefreshToken: Manages token refreshing.

Enums
UserRoles: Defines roles (Admin, Instructor, Student).

Services
AuthService, UserService, EmailService, AdminService (stub), ExternalAuthService (placeholder).

Middleware
API key validation.

JWT validation.

Error handling.

Logging.

Database
SQL Server via AppDbContext.

Database initialization via DbInitializer.

🔐 Security Features
JWT Authentication with issuer and audience validation.

API key check via X-Api-Key header.

Rate limiting (e.g., 3 requests/hour for forgot-password).

Password hashing with PasswordHelper.

Input validation using FluentValidation.

Structured logging using Serilog.

 📊 UMS System Flow Overview
👥 Actors:
User (Student/Instructor/Admin)

API Gateway / Frontend

UMS API (backend)
→ AuthController
→ UserController
→ AdminController

Database (User records, roles, tokens)

📈 Full System Interaction Flow
🔑 Registration & Login
User → Frontend → UMS API (AuthController)

Register
→ User submits email/password
→ API validates, hashes password
→ Stores in DB, sends confirmation

Login
→ User submits credentials
→ API verifies credentials
→ Generates Access Token (short-lived JWT) + Refresh Token (long-lived)
→ Returns both to user

🔐 Token Verification (for protected routes)
Frontend/API Gateway sends JWT token in Authorization header

API verifies:

Token validity

Token expiry

User role (if Admin endpoint)

If valid → process request
If invalid/expired → respond 401 Unauthorized

🔄 Refresh Token Flow
User → Frontend → API

Access token expires

Frontend sends refreshToken to /refresh-token

API validates token, issues a new Access+Refresh token pair

Frontend resumes secure actions

📖 User Profile Flow
User → Frontend → UMS API (UserController)

GET /profile → returns profile data

PUT /profile → updates email/name

PUT /change-password → validates current password, hashes new one

DELETE /delete-account → deletes user data

🛠️ Admin Management Flow
Admin User (with Admin role JWT) → Frontend → UMS API (AdminController)

GET /users → lists all users

PUT /set-role/{userId} → change user role

DELETE /delete-user/{userId} → remove user from system

API middleware enforces role-check for these routes

📝 Password Reset Flow
User → Frontend → UMS API (AuthController)

POST /forgot-password
→ Generates token, sends reset link via email

User clicks reset link

POST /reset-password with token & new password
→ Validates token
→ Updates password

📉 Flowchart-style Text Diagram
 
 **every API endpoint** 


## 📖 Full API Documentation

---

## 🔑 Authentication APIs (`/UMS/api/Auth`)

**No JWT required — only X-Api-Key**

---

### **Register User**

* **POST** `/UMS/api/Auth/register`
* **Headers**:

  * `X-Api-Key: your_api_key`
* **Body:**

```json
{
  "email": "user@example.com",
  "password": "YourPassword123!"
}
```

* **Response:**

```json
{
  "message": "Registration successful. Please login."
}
```

* **Errors:**
* `400 BadRequest` if email exists or invalid input.

---

### **Login**

* **POST** `/UMS/api/Auth/login`
* **Headers**:

  * `X-Api-Key: your_api_key`
* **Body:**

```json
{
  "email": "user@example.com",
  "password": "YourPassword123!"
}
```

* **Response:**

```json
{
  "accessToken": "<JWT token>",
  "refreshToken": "<refresh token>"
}
```

* `401 Unauthorized` for invalid credentials.

---

### **Refresh Token**

* **POST** `/UMS/api/Auth/refresh-token`
* **Headers:**

  * `X-Api-Key: your_api_key`
* **Body:**

```json
{
  "refreshToken": "<refresh token>"
}
```

* **Response:**

```json
{
  "accessToken": "<new JWT>",
  "refreshToken": "<new refresh token>"
}
```

* `400 BadRequest` if invalid token.

---

### **Forgot Password**

* **POST** `/UMS/api/Auth/forgot-password`
* **Headers:**

  * `X-Api-Key: your_api_key`
* **Body:**

```json
{
  "email": "user@example.com"
}
```

* **Response:**

```json
{
  "message": "Password reset link sent."
}
```

* Rate limited: 3 requests/hour.

---

### **Reset Password**

* **POST** `/UMS/api/Auth/reset-password`
* **Headers:**

  * `X-Api-Key: your_api_key`
* **Body:**

```json
{
  "email": "user@example.com",
  "newPassword": "NewPassword123!",
  "resetToken": "token-from-email"
}
```

* **Response:**

```json
{
  "message": "Password reset successful."
}
```

* `400 BadRequest` if token invalid/expired.

---

## 👤 User APIs (`/UMS/api/User`)

**Requires JWT in Authorization header**

---

### **Get User Profile**

* **GET** `/UMS/api/User/profile`
* **Headers:**

  * `Authorization: Bearer <token>`
* **Response:**

```json
{
  "id": 1,
  "email": "user@example.com",
  "role": "Student"
}
```

---

### **Update User Profile**

* **PUT** `/UMS/api/User/profile`
* **Headers:**

  * `Authorization: Bearer <token>`
* **Body:**

```json
{
  "email": "updated@example.com"
}
```

* **Response:**

```json
{
  "message": "Profile updated."
}
```

---

### **Change Password**

* **PUT** `/UMS/api/User/change-password`
* **Headers:**

  * `Authorization: Bearer <token>`
* **Body:**

```json
{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword456!"
}
```

* **Response:**

```json
{
  "message": "Password changed successfully."
}
```

* `400 BadRequest` for invalid current password.

---

### **Delete Account**

* **DELETE** `/UMS/api/User/delete-account`
* **Headers:**

  * `Authorization: Bearer <token>`
* **Response:**

```json
{
  "message": "Account deleted."
}
```

---

## 🛠️ Admin APIs (`/UMS/api/Admin`)

**Requires Admin role JWT**

---

### **Get All Users**

* **GET** `/UMS/api/Admin/users`
* **Headers:**

  * `Authorization: Bearer <token>`
* **Response:**

```json
[
  {
    "id": 1,
    "email": "user1@example.com",
    "role": "Student"
  },
  ...
]
```

---

### **Set User Role**

* **PUT** `/UMS/api/Admin/set-role/{userId}`
* **Headers:**

  * `Authorization: Bearer <token>`
* **Body:**

```json
{
  "role": "Instructor"
}
```

* **Response:**

```json
{
  "message": "Role updated."
}
```

* `404 NotFound` if user ID missing.

---

### **Delete User**

* **DELETE** `/UMS/api/Admin/delete-user/{userId}`
* **Headers:**

  * `Authorization: Bearer <token>`
* **Response:**

```json
{
  "message": "User deleted."
}
```

---

## 📑 Common Error Responses

* `400 BadRequest`: Validation failure
* `401 Unauthorized`: Missing/invalid JWT or API key
* `403 Forbidden`: Insufficient role permissions
* `404 NotFound`: Resource not found
* `429 Too Many Requests`: Rate limit exceeded
* `500 Internal Server Error`: Unhandled exception

---

## 📌 Example Curl Request

```bash
curl -X POST "http://localhost:5003/UMS/api/Auth/login" \
-H "Content-Type: application/json" \
-H "X-Api-Key: your_api_key" \
-d '{"email": "user@example.com", "password": "YourPassword123!"}'
```

---

Would you like me to generate a **Swagger-compatible OpenAPI spec YAML/JSON** for this too? 📄✨

⚙️ Setup Instructions
bash
نسخ
تحرير
# 1. Clone the repository
git clone <repository-url>

# 2. Install dependencies
dotnet restore

# 3. Update appsettings.json
# - SQL Server connection string
# - SMTP Email settings
# - API key for X-Api-Key

# 4. Set environment variables
- JWT_SECRET_KEY
- JWT_ISSUER
- JWT_AUDIENCE

# 5. Apply migrations
dotnet ef database update

# 6. Run the application
dotnet run

# 7. Access Swagger UI
http://localhost:5003/swagger

![image](https://github.com/user-attachments/assets/54fd69ba-4b6e-4e30-abf9-41294eccccbd)
![Screenshot 2025-06-11 193436](https://github.com/user-attachments/assets/294ec1ea-39e0-456f-8e9b-1ba532e43cfc)
![Screenshot 2025-06-11 225601](https://github.com/user-attachments/assets/6f214107-70b1-441a-8bad-3fd25f9e7e19)
![Screenshot 2025-06-11 193715](https://github.com/user-attachments/assets/e72c2dc1-86af-4950-91c6-bab0edeb301f)
![Screenshot 2025-06-11 193755](https://github.com/user-attachments/assets/a142a4d6-fd8a-4f40-b7b8-78775d5816ff)

![Screenshot 2025-06-11 194137](https://github.com/user-attachments/assets/da878da9-8ec4-4a85-a46c-c91bdad0eafc)

![Screenshot 2025-06-11 193654](https://github.com/user-attachments/assets/62936248-dfc8-42f4-a3ba-8c4113c89b8d)

Password Reset Flow Documentation

📎 License
This project is open-source and available under the MIT License.
