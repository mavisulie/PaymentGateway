## How to run

1. Open solution and run the app - making sure PaymentGateway.Api is set as Startup project
2. Copy Swagger/Open Api url from the console in the browser (eg. http://localhost:7024/api/swagger/ui)
3. Test first function(ProcessPayment) from OpenApi doc with test data and copy paymentId to be used in the retrieval of the second Function(GetPayment)
4. Use second function to retrieve payment

## Areas of improvement

- Add component / Api tests
- Replace client mock with real client
- Log response errors to AppInsights
- Data layer to use a proper mechanism instead of InMemory mechanism
- Proper documentation of flow of the system, function input/output json examples

## Technologies
- .NET 7
- Function app - lightweight and easy to deploy
- EntityFrameworkCore.InMemory - to make testing easy