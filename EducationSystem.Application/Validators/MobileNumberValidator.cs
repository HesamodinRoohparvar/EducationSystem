using EducationSystem.Application.Validators;
using EducationSystem.Domain.Resources;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace EducationSystem.Application.Validators
{
    public class MobileNumberValidator<T> : PropertyValidator<T, string>, IRegularExpressionValidator
    {
        public override string Name => "PhoneNumberValidator";
        public string Expression => @"^0?9\d{9}$";

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Regex.IsMatch(value, Expression, RegexOptions.Compiled);
            }

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return Resource.FluentValidationInvalidMobileNumberError;
        }
    }
}

public static class PhoneNumberValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> ValidMobileNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var validator = (PropertyValidator<T, string>)new MobileNumberValidator<T>();

        return ruleBuilder.SetValidator(validator);
    }
}
