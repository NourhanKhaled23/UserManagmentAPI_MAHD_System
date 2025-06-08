

## **User Management System (UMS) Documentation**

### **1. Overview**
The **User Management System (UMS)** is a backend service built using **ASP.NET Core** and **Entity Framework Core**. It handles user-related operations such as:
- **User Registration & Login**
- **Profile Management** (view, update, delete)
- **Role Management** (Admin, Instructor, Student)
- **Password Management** (change password)
- **Admin Operations** (view all users, update roles, delete users)

The system is designed with **Clean Architecture**, which separates the application into distinct layers for better maintainability and scalability.

---

### **2. Key Features**
1. **User Registration & Login**:
   - Users can register with their email, password, and basic details.
   - Passwords are securely hashed using **BCrypt**.
   - Users can log in and receive a **JWT token** for authentication.

2. **Profile Management**:
   - Users can view and update their profile (e.g., first name, last name).
   - Users can change their password.
   - Users can delete their account.

3. **Role Management**:
   - Admins can view all users, update user roles, and delete users.
   - Users are assigned roles (e.g., Admin, Instructor, Student).

4. **Security**:
   - Passwords are hashed using **BCrypt**.
   - Authentication is done using **JWT tokens**.
   - Sensitive data (e.g., JWT secret) is stored securely in configuration files.

5. **Logging & Error Handling**:
   - The system uses **Serilog** for structured logging.
   - Global error handling middleware ensures consistent error responses.

6. **API Versioning**:
   - The API supports versioning to allow future updates without breaking existing clients.

---

### **3. Architecture**
The system is divided into **4 layers**:

#### **1. Domain Layer**
- Contains the core business logic and entities (e.g., `User`, `UserSettings`).
- Defines interfaces for repositories (e.g., `IUserRepository`).

#### **2. Application Layer**
- Implements business logic (e.g., `AuthService`, `UserService`).
- Uses **DTOs (Data Transfer Objects)** to transfer data between layers.
- Handles use cases like registration, login, profile updates, etc.

#### **3. Infrastructure Layer**
- Implements data access using **Entity Framework Core**.
- Contains repositories (e.g., `UserRepository`, `SettingsRepository`).
- Provides security helpers (e.g., `PasswordHelper`, `JwtHelper`).

#### **4. Presentation Layer (UserManagementAPI)**
- Exposes RESTful API endpoints.
- Handles HTTP requests and responses.
- Uses **Swagger** for API documentation.

---

### ** Security**
- **Password Hashing**: Passwords are hashed using **BCrypt** before being stored in the database.
- **JWT Authentication**: Users receive a JWT token upon login, which is used for authentication in subsequent requests.
- **Role-Based Access Control**: Endpoints are protected based on user roles (e.g., only Admins can access `/api/Admin/users`).

---

### ** Logging & Error Handling**
- **Logging**: The system uses **Serilog** for structured logging. Logs are written to the console and can be configured to write to files or other sinks.
- **Error Handling**: Global error handling middleware catches unhandled exceptions and returns consistent error responses.

---

### ** Testing**
- **Unit Tests**: The system is tested using **xUnit** and **Moq** to ensure that service methods behave as expected.
- **Integration Tests**: Tests are written to verify that the API endpoints work correctly.

---

### ** API Versioning**
- The API supports versioning to allow future updates without breaking existing clients.
- Example: `/api/v1/Auth/register` for version 1 of the API.



---

### ** Required Packages**
The system uses the following NuGet packages:
- **Microsoft.EntityFrameworkCore.SqlServer**: For database access.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: For JWT authentication.
- **BCrypt.Net-Next**: For password hashing.
- **Serilog.AspNetCore**: For structured logging.
- **Swashbuckle.AspNetCore**: For API documentation (Swagger).
- **xUnit & Moq**: For unit testing.

---

### ** Folder Structure**
The project is organized into the following folders:
- **Domain**: Contains core entities and interfaces.
- **Application**: Contains business logic and DTOs.
- **Infrastructure**: Contains data access and security helpers.
- **UserManagementAPI**: Contains API controllers and middleware.
- **Tests**: Contains unit and integration tests.

---

### ** How to Run the Project**
1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   ```
2. **Set up the database**:
   - Update the connection string in `appsettings.json`.
   - Run database migrations:
     ```bash
     dotnet ef database update
     ```
3. **Run the application**:
   ```bash
   dotnet run
   ```
4. **Access the API**:
   - Open Swagger UI at `https://localhost:7233/swagger`.

