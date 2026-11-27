using HotelBooking.Common.Domain;

namespace HotelBooking.Common.Presentation.Results;

public static class ResultValueExtensions
{
    extension<TIn>(Result<TIn> result)
    {
        public TOut Match<TOut>(Func<TIn, TOut> onSuccess, Func<Result<TIn>, TOut> onFailure) =>
            result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}
