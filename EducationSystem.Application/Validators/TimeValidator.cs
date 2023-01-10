using EducationSystem.Domain.Resources;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EducationSystem.Application.Validators
{
    public class TimeValidator<T> : PropertyValidator<T, string>, IRegularExpressionValidator
    {
        public string Expression => @"\d{2}:\d{2}:\d{2}";
        public override string Name => "TimeValidator";

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
            return Resource.FluentValidationInvalidTimeError;
        }
    }

    public static class TimeValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidTime<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var validator = (PropertyValidator<T, string>)new TimeValidator<T>();

            return ruleBuilder.SetValidator(validator);
        }
    }
}
