# WeatherForecastingApp

The WeatherForecastingService is a RESTful service built using .NET Core 8 that fetches weather data from the Visual Crossing Weather API and provides it to clients. This project is designed using a Clean Architecture approach, promoting separation of concerns and ensuring the application is modular, testable, and maintainable. Clients can request weather data for specific cities and dates, offering a reliable way to get current weather information.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

## Default User
There is default User Credentials and Data , If you want to use :
Email : administrator@localhost
Password: Administrator1!



## Features 

- **Fetch Current Weather**: The service fetches current weather data from the Visual Crossing Weather API.
- **Request by City/Date**: Clients can request weather data for a specific city and date.
- **Extendable**: The service is designed to be extendable, allowing for future enhancements and additional features.

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.


## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```


## Conclusion

This project demonstrates a practical application of Clean Architecture principles in a .NET environment. The design choices made aim to balance simplicity with maintainability, ensuring that the application is both functional and easy to extend.

Feel free to reach out if you have any questions or need further clarification!
