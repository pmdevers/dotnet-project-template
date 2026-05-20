namespace Template.Api.Domain.Abstractions;

public class Results<T, TError>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public TError? Error { get; }

    private Results(bool isSuccess, T? value, TError? error)
    {
        if (isSuccess)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (error is not null)
            {
                throw new ArgumentException("Successful result cannot have an error.", nameof(error));
            }
        }
        else
        {
            ArgumentNullException.ThrowIfNull(error);
            if (value is not null)
            {
                throw new ArgumentException("Failed result cannot have a value.", nameof(value));
            }
        }

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Results<T, TError> Success(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new(true, value, default);
    }

    public static Results<T, TError> Failure(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(false, default, error);
    }
}

public static class Results
{
    public static Results<T, TError> Success<T, TError>(T value) => Results<T, TError>.Success(value);
    public static Results<T, TError> Failure<T, TError>(TError error) => Results<T, TError>.Failure(error);

    extension<T, TError>(Results<T, TError> results)
    {
        public bool IsFailure() => results.IsFailure;

        public Results<T2, TError> Map<T2>(Func<T, T2> mapFunc)
            => results.IsSuccess
                ? Success<T2, TError>(mapFunc(results.Value!))
                : Failure<T2, TError>(results.Error!);

        public Results<T2, TError> Bind<T2>(Func<T, Results<T2, TError>> bind)
            => results.IsSuccess
                ? bind(results.Value!)
                : Failure<T2, TError>(results.Error!);

        public Results<T2, TError> Bind<T2>(Func<Results<T, TError>, Results<T2, TError>> bind)
            => bind(results);

        public Task<Results<T2, TError>> BindAsync<T2>(Func<T, Task<Results<T2, TError>>> bind)
            => results.IsSuccess
                ? bind(results.Value!)
                : Task.FromResult(Failure<T2, TError>(results.Error!));

        public Results<T, TError2> MapError<TError2>(Func<TError, TError2> mapFunc)
            => results.IsSuccess
                ? Success<T, TError2>(results.Value!)
                : Failure<T, TError2>(mapFunc(results.Error!));

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<TError, TResult> onFailure)
            => results.IsSuccess
                ? onSuccess(results.Value!)
                : onFailure(results.Error!);

        public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccessAsync, Func<TError, Task<TResult>> onFailureAsync)
            => results.IsSuccess
                ? await onSuccessAsync(results.Value!)
                : await onFailureAsync(results.Error!);

        public async Task<TResult> MatchAsync<TResult>(Func<T, TResult> onSuccess, Func<TError, Task<TResult>> onFailureAsync)
            => results.IsSuccess
                ? onSuccess(results.Value!)
                : await onFailureAsync(results.Error!);

        public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccessAsync, Func<TError, TResult> onFailure)
            => results.IsSuccess
                ? await onSuccessAsync(results.Value!)
                : onFailure(results.Error!);
    }

    extension<T, TError>(Task<Results<T, TError>> resultsTask)
    {
        public async Task<bool> IsFailureAsync()
        {
            var result = await resultsTask;
            return result.IsFailure;
        }

        public async Task<Results<T2, TError>> MapAsync<T2>(Func<T, T2> mapFunc)
        {
            var result = await resultsTask;
            return result.IsSuccess
                ? Success<T2, TError>(mapFunc(result.Value!))
                : Failure<T2, TError>(result.Error!);
        }

        public async Task<Results<T2, TError>> BindAsync<T2>(Func<T, Results<T2, TError>> bind)
        {
            var result = await resultsTask;
            return result.IsSuccess
                ? bind(result.Value!)
                : Failure<T2, TError>(result.Error!);
        }

        public async Task<Results<T2, TError>> BindAsync<T2>(Func<T, Task<Results<T2, TError>>> bind)
        {
            var result = await resultsTask;
            return result.IsSuccess
                ? await bind(result.Value!)
                : Failure<T2, TError>(result.Error!);
        }

        public async Task<Results<T, TError2>> MapErrorAsync<TError2>(Func<TError, TError2> mapFunc)
        {
            var result = await resultsTask;
            return result.IsSuccess
                ? Success<T, TError2>(result.Value!)
                : Failure<T, TError2>(mapFunc(result.Error!));
        }

        public async Task<TResult> MatchAsync<TResult>(Func<T, TResult> onSuccess, Func<TError, Task<TResult>> onFailureAsync)
        {
            var result = await resultsTask;
            return await result.MatchAsync(onSuccess, onFailureAsync);
        }

        public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccessAsync, Func<TError, TResult> onFailure)
        {
            var result = await resultsTask;
            return await result.MatchAsync(onSuccessAsync, onFailure);
        }

        public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccessAsync, Func<TError, Task<TResult>> onFailureAsync)
        {
            var result = await resultsTask;
            return await result.MatchAsync(onSuccessAsync, onFailureAsync);
        }

        public async Task<TResult> MatchAsync<TResult>(Func<T, TResult> onSuccess, Func<TError, TResult> onFailure)
        {
            var result = await resultsTask;
            return result.Match(onSuccess, onFailure);
        }
    }
}
