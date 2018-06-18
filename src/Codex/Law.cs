namespace Codex
{
    using Result;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Law : ILaw
    {
        private readonly Func<bool> _predicate;
        private readonly string _errorMessage;

        public static ILaw CreateNotNull<T>(string name, T value) => 
            new Law(() => value != null, $"'{name}' should not be null.");

        public static ILaw CreateNotNullElements<T>(string name, IEnumerable<T> value) => 
            new Law(() => value != null && value.All(it => it != null), $"'{name}' should contain not null elements.");

        public Law(Func<bool> predicate, string errorMessage)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        public IResult<Nothing> Evaluate()
        {
            return _predicate() 
                ? ResultFactory.CreateSuccess() 
                : ResultFactory.CreateError(_errorMessage);
        }
    }
}