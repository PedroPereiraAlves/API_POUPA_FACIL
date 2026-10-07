using API_POUPA_FACIL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace API_POUPA_FACIL.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            _logger.LogDebug("Request was canceled by the client.");
            return true;
        }

        var (status, title, detail) = Map(exception);

        if (status >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unhandled exception for {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path);
        }
        else
        {
            _logger.LogWarning(
                "Request {Method} {Path} failed with {Status}: {Title}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                status,
                title);
        }

        if (status >= StatusCodes.Status500InternalServerError && !_environment.IsDevelopment())
            detail = "Ocorreu um erro inesperado.";

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(
            problem,
            options: null,
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);

        return true;
    }

    private static (int Status, string Title, string Detail) Map(Exception exception)
    {
        if (exception is ConflictException conflict)
            return (StatusCodes.Status409Conflict, "Conflito", conflict.Message);

        var postgres = FindPostgresException(exception);
        if (postgres?.SqlState == PostgresErrorCodes.UniqueViolation)
            return (StatusCodes.Status409Conflict, "Conflito", "O registro informado já existe.");

        return (StatusCodes.Status500InternalServerError, "Erro interno do servidor", exception.Message);
    }

    private static PostgresException? FindPostgresException(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (current is PostgresException postgres)
                return postgres;
        }

        return null;
    }
}
