using FluentValidation;
using FSI.SupportPoint.Application.Dtos.Checkin.Request;

namespace FSI.SupportPointSystem.Application.Validations
{
    public class CheckinRequestValidator : AbstractValidator<CheckinRequest>
    {
        public CheckinRequestValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        }
    }
}

