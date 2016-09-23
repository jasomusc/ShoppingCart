# ShoppingCart - README

-------------------------------------------
Documentation on how the task has been tackled
-------------------------------------------

1. RESTful API
-------------------------------------------
  - Design Database
  - Build Database
  - Connect to database using EntityFramework
  - Research on connecting to SQLITE using Entityframework (for some reason didn't work with .NET Framework 4.6 so had to switch project back to .NET Framework 4.5.2)
  - Develop class to log all errors in Error_Logging table
  - Develop API Methods

-------------------------------------------

2. Unit Tests
-------------------------------------------
  - Add project for Unit Tests to solution
  - Develop Unit Test Methods
  - Research issue with database connection (result: needed to copy configSections, appsettings and connectionstrings tags from Web.config of API into App.config of Test Unit project)
  - Research issue with accepting HttpResponseMessage from TestMethod (result need to add { Request = new HttpRequestMessage() } when creating instead of Controller)
  - Test all API methods developed

-------------------------------------------
  
3. Web Interface
-------------------------------------------

  - Add MVC Project default template to solution
  - Modify UI of Home page
  - Develop methods for Get Items and Get Cart Items methods
  - Develop methods for Add, Remove and Clear Items methods
  - Test Web Interface
  - Code cleaning

-------------------------------------------
