using AuthECAPI.Models;

namespace AuthECAPI.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                //app.UseSwaggerUI();
                app.UseSwaggerUI(options =>
                {
                    //options.SwaggerEndpoint("/swagger/PaymentAPI/swagger.json", "Payment API v1");
                    options.SwaggerEndpoint("/openapi/v1.json", "api");
                });
                app.UseSwagger();
            }
            return app;
        }
    }
}
