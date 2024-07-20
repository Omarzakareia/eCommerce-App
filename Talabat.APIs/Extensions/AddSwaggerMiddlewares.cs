namespace Talabat.APIs.Extensions
{
    public static class AddSwaggerMiddlewares
    {
        public static WebApplication UseSwaggerMiddleWares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
