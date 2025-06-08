

### **4. Database Schema**
The system uses a relational database 

#### **Users Table**
- Stores user information:
  - `Id` (Primary Key)
  - `FirstName`
  - `LastName`
  - `Email` (Unique)
  - `PasswordHash` (Hashed password)
  - `Role` (e.g., Admin, Instructor, Student)
  - `CreatedAt` (Timestamp for account creation)
  - `UpdatedAt` (Timestamp for last update)

#### **UserSettings Table**
- Stores additional user settings:
  - `Id` (Primary Key)
  - `UserId` (Foreign Key to Users table)
  - `ReceiveNotifications` (Boolean flag for notifications)
  - `PrivacyLevel` (e.g., Public, Private)
