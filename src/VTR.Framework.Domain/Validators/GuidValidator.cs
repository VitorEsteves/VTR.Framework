namespace VTR.Framework.Domain.Validators;

public class GuidValidator : AbstractValidator<Guid>
{
    public GuidValidator()
    {
        RuleFor(x => x).NotEqual(Guid.Empty);
    }
}