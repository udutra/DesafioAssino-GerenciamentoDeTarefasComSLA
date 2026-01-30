using System.Net;
using System.Text.Json;
using DesafioAssino.Api.Models;
using DesafioAssino.Domain.Exceptions;
using FluentValidation;

namespace DesafioAssino.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger){
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Erro de validação.";
                response.Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                break;

            case DomainException domainEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = domainEx.Message;
                break;

            default:
                logger.LogError(exception, "Erro não tratado.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Ocorreu um erro interno no servidor.";
                break;
        }

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
