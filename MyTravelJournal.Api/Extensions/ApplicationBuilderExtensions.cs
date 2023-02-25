using MyTravelJournal.Api.Middleware;

namespace MyTravelJournal.Api.Extensions;

public static class ApplicationBuilderExtensions
{
   public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
   {
      return applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
   }
}