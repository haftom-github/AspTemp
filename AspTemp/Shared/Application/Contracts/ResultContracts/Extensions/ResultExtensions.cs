namespace AspTemp.Shared.Application.Contracts.ResultContracts.Extensions;

public static class ResultExtensions
{
    extension<TIn>(Result<TIn> result)
    {
        public Result<TOut> Map<TOut>(Func<TIn, TOut> mapper)
            => result.IsSuccess
                ? new Success<TOut>(
                    mapper(result.Success!.Value), 
                    result.Success.Message, 
                    result.Success.Details!.ToDictionary()
                )
                : result.Failure!;

        public async Task<Result<TOut>> MapAsync<TOut>(Func<TIn, Task<TOut>> mapper)
            => result.IsSuccess
                ? new Success<TOut>(
                    await mapper(result.Success!.Value),
                    result.Success.Message,
                    result.Success.Details?.ToDictionary()
                )
                : result.Failure!;

        public Result<TOut> Bind<TOut>(Func<TIn, Result<TOut>> binder)
            => result.IsSuccess
                ? binder(result.Success!.Value)
                : result.Failure!;

        public async Task<Result<TOut>> BindAsync<TOut>(Func<TIn, Task<Result<TOut>>> binder)
            => result.IsSuccess
                ? await binder(result.Success!.Value)
                : result.Failure!;

        public IResult ToHttpResult()
        {
            return result.IsSuccess 
                ? Results.Json(result.Success, statusCode:200) 
                : Results.Json(result.Failure!.ToProblemDetails(), statusCode: result.Failure!.FailureType.GetStatus());
        }
    }
}