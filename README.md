# WebAPI_Training
Video  .Net Core 2 but me use .Netcore 3.1 
status : Can not use CORS  
An unhandled exception occurred while processing the request.
InvalidOperationException: Endpoint MyAPI.Controllers.ProductController.GetProducts (MyAPI) contains CORS metadata, but a middleware was not found that supports CORS.
Configure your application startup by adding app.UseCors() inside the call to Configure(..) in the application startup code. The call to app.UseAuthorization() must appear between app.UseRouting() and app.UseEndpoints(...).