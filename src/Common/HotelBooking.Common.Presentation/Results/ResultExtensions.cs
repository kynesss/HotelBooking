using HotelBooking.Common.Domain;

namespace HotelBooking.Common.Presentation.Results;

public static class ResultExtensions
{
    extension(Result result)
    {
        public TOut Match<TOut>(Func<TOut> onSuccess, Func<Result, TOut> onFailure) =>
            result.IsSuccess ? onSuccess() : onFailure(result);
    }
}
