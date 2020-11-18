# Seed code - Boilerplate for News-App Step5 Assignment

## Assignment Step Description

In this assignment News-App Step 5, we will implement REST-based MicroServices with ASP.NET Core alog with MongoDB (NoSQL implementation) as data store.

In this step, we will create this application as collection of 3 microservices using REST API.

1. UserService
2. NoteService
3. ReminderService

### Problem Statement

In this assignment, we will change our News-App into microservices. To acheive this, we'll develop an application with RESTful microservices which we will allow to perform CRUD operations on User,News and Reminder with the help of URI. Check the correctness of the operations with the help of Postman API.

1. Design the data access layer using MongoDB as data store.
2. Design the microservices using REST API and define endpoints to manipuate the resources (User,News,Reminder).
3. All types of exceptions must be handled by the application and return appropriate status codes.

<b> Note: For detailed clarity on the class files, kindly go thru the Project Structure </b>

### Expected Solution

REST API must expose the endpoints for the following operations:

- Create a new user, update the user, retrieve a single user, delete the user.
- Create a News, delete a news, get all news of a specific userId, adding, update and delete the reminder for a news.
- Create a Reminder, delete a Reminder,update a reminder get all Reminders for specific    user.

### Steps to be followed

    Step 1: Fork and Clone the boilerplate in a specific folder on your local machine.
    Step 2: Implement each Microservice step by step.
    Step 3: See the methods mentioned in the Repository interface.
    Step 4: Implement all the methods of Repository interface.
    Step 5: Test each and every Repository with appropriate test cases.
    Step 6: See the methods mentioned in the service interface.
    Step 7: Implement all the methods of service interface.
    Step 8: Test each and every service with appropriate test cases.
    Step 9: Write controllers to work with RESTful API.  
    Step 10: Test each and every controller with appropriate test cases.
    Step 11: Check all the functionalities using URI's mentioned in the controllers with the help of Postman for final output.

### Project structure

The folders and files you see in this repositories, is how it is expected to be in projects, which are submitted for automated evaluation by Hobbes
```
📦News-Step-5
 ┣ 📂NewsService //Microservice to handle news data
 ┃ ┣ 📂Controllers
 ┃ ┃ ┗ 📜NewsController.cs //REST API controller to define endpoints for News
 ┃ ┣ 📂Exceptions //custom exception classes
 ┃ ┃ ┣ 📜NewsAlreadyExistsException.cs
 ┃ ┃ ┣ 📜NoNewsFoundException.cs
 ┃ ┃ ┗ 📜NoReminderFoundException.cs
 ┃ ┣ 📂Models
 ┃ ┃ ┣ 📜News.cs
 ┃ ┃ ┣ 📜NewsContext.cs //class to define Mongo Collection and configuring MongoClient
 ┃ ┃ ┣ 📜Reminder.cs
 ┃ ┃ ┗ 📜UserNews.cs
 ┃ ┣ 📂Properties
 ┃ ┃ ┗ 📜launchSettings.json
 ┃ ┣ 📂Repository
 ┃ ┃ ┣ 📜INewsRepository.cs //Interface to define contract for News
 ┃ ┃ ┗ 📜NewsRepository.cs //Implementation of INewsRepository
 ┃ ┣ 📂Services
 ┃ ┃ ┣ 📜INewsService.cs //Interface to define Business Rules
 ┃ ┃ ┗ 📜NewsService.cs //Implementation of INewsService
 ┃ ┣ 📜appsettings.Development.json
 ┃ ┣ 📜appsettings.json
 ┃ ┣ 📜NewsService.csproj
 ┃ ┣ 📜Program.cs
 ┃ ┗ 📜Startup.cs
 ┣ 📂ReminderService //Microservice to handle reminder data
 ┃ ┣ 📂Controllers
 ┃ ┃ ┗ 📜ReminderController.cs //REST API controller to define endpoints for Reminder
 ┃ ┣ 📂Exceptions //custom exception classes
 ┃ ┃ ┣ 📜NoReminderFoundException.cs
 ┃ ┃ ┗ 📜ReminderAlreadyExistsException.cs
 ┃ ┣ 📂Models
 ┃ ┃ ┣ 📜Reminder.cs
 ┃ ┃ ┣ 📜ReminderContext.cs //class to define Mongo Collection and configuring MongoClient
 ┃ ┃ ┗ 📜UserReminder.cs
 ┃ ┣ 📂Properties
 ┃ ┃ ┗ 📜launchSettings.json
 ┃ ┣ 📂Repository
 ┃ ┃ ┣ 📜IReminderRepository.cs //Interface to define contract for Reminder
 ┃ ┃ ┗ 📜ReminderRepository.cs //Implementation of IReminderRepository
 ┃ ┣ 📂Services
 ┃ ┃ ┣ 📜IReminderService.cs //Interface to define Business Rules
 ┃ ┃ ┗ 📜ReminderService.cs //Implementation of IReminderService
 ┃ ┣ 📜appsettings.Development.json
 ┃ ┣ 📜appsettings.json
 ┃ ┣ 📜Program.cs
 ┃ ┣ 📜ReminderService.csproj
 ┃ ┗ 📜Startup.cs
 ┣ 📂UserService //Microservice to handle user data
 ┃ ┣ 📂Controllers
 ┃ ┃ ┗ 📜UserController.cs //REST API controller to define endpoints for User
 ┃ ┣ 📂Exceptions //custom exception classes
 ┃ ┃ ┣ 📜UserAlreadyExistsException.cs
 ┃ ┃ ┗ 📜UserNotFoundException.cs
 ┃ ┣ 📂Models
 ┃ ┃ ┣ 📜UserContext.cs //class to define Mongo Collection and configuring MongoClient
 ┃ ┃ ┗ 📜UserProfile.cs
 ┃ ┣ 📂Properties
 ┃ ┃ ┗ 📜launchSettings.json
 ┃ ┣ 📂Repository
 ┃ ┃ ┣ 📜IUserRepository.cs //Interface to define contract for User
 ┃ ┃ ┗ 📜UserRepository.cs //Implementation of IUserRepository
 ┃ ┣ 📂Services
 ┃ ┃ ┣ 📜IUserService.cs //Interface to define Business Rules
 ┃ ┃ ┗ 📜UserService.cs //Implementation of IUserService
 ┃ ┣ 📜appsettings.Development.json
 ┃ ┣ 📜appsettings.json
 ┃ ┣ 📜Program.cs
 ┃ ┣ 📜Startup.cs
 ┃ ┗ 📜UserService.csproj
 ┣ 📂test //Test project having unit test and integration test
 ┃ ┣ 📂ControllerTests
 ┃ ┃ ┣ 📂IntegrationTest
 ┃ ┃ ┃ ┣ 📜CustomWebApplicationFactory.cs
 ┃ ┃ ┃ ┣ 📜NewsControllerTest.cs
 ┃ ┃ ┃ ┣ 📜ReminderControllerTest.cs
 ┃ ┃ ┃ ┗ 📜UserControllerTest.cs
 ┃ ┃ ┗ 📂UnitTest
 ┃ ┃ ┃ ┣ 📜NewsControllerTest.cs
 ┃ ┃ ┃ ┣ 📜ReminderControllerTest.cs
 ┃ ┃ ┃ ┗ 📜UserControllerTest.cs
 ┃ ┣ 📂InfraSetup
 ┃ ┃ ┣ 📜NewsDbFixture.cs
 ┃ ┃ ┣ 📜ReminderDbFixture.cs
 ┃ ┃ ┗ 📜UserDbFixture.cs
 ┃ ┣ 📂RepositoryTests
 ┃ ┃ ┣ 📜NewsRepositoryTest.cs
 ┃ ┃ ┣ 📜ReminderRepositoryTest.cs
 ┃ ┃ ┗ 📜UserRepositoryTest.cs
 ┃ ┣ 📂ServiceTests
 ┃ ┃ ┣ 📜NewsServiceTest.cs
 ┃ ┃ ┣ 📜ReminderServiceTest.cs
 ┃ ┃ ┗ 📜UserServiceTest.cs
 ┃ ┣ 📜appsettings-integration.json
 ┃ ┣ 📜appsettings.json
 ┃ ┣ 📜PriorityOrderer.cs
 ┃ ┣ 📜test.csproj
 ┃ ┗ 📜test.csproj.user
 ┗ 📜News-Step-5.sln
```
