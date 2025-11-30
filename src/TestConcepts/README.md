# Test Concepts

This project, **TestConcepts**, is designed to demonstrate the use of xUnit for unit testing in a .NET 9 environment. 

## Purpose

The primary goal of this project is to provide a simple example of how to set up and run unit tests using the xUnit framework. It serves as a foundation for understanding unit testing principles and practices.

## Project Structure

- **TestConcepts.sln**: The solution file that contains references to the projects within the solution.
- **TestConcepts.Tests**: The xUnit test project that includes:
  - **TestConcepts.Tests.csproj**: The project file specifying the target framework and dependencies.
  - **UnitTest1.cs**: A sample test class containing test methods.
  - **Usings.cs**: Global using directives for cleaner code.

## Running the Tests

To run the tests in this project, follow these steps:

1. Ensure you have the .NET SDK installed on your machine.
2. Open a terminal and navigate to the `TestConcepts` directory.
3. Run the following command to restore the dependencies:
   ```
   dotnet restore
   ```
4. Execute the tests using the command:
   ```
   dotnet test
   ```

This will build the project and run all the tests defined in the `TestConcepts.Tests` project. 

## Conclusion

This project serves as a basic introduction to unit testing with xUnit in .NET 9. You can expand upon this foundation by adding more tests and exploring advanced testing techniques.