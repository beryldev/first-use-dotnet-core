using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Wrhs.Core;

namespace Wrhs.Tests
{
    public abstract class ValidatorTestsBase<T>
    {  
        protected IValidator<T> validator;
         
        protected IEnumerable<ValidationResult> Act(T command)
        {
            return validator.Validate(command);
        }

        protected void AssertSingleError(IEnumerable<ValidationResult> results, string field)
        {
            results.Should().HaveCount(1);
            results.Select(x=>x.Field).Should().Contain(field);
        }   
    }
}