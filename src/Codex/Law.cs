namespace Codex
{
    using System;
    using Result;

    public class Law<T>
    {
        private readonly Predicate<T> _predicate;
        private readonly string _errorMessage;

        public Law(Predicate<T> predicate, string errorMessage)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        public IResult<Nothing> Evaluate(T subject)
        {
            return _predicate(subject) 
                ? ResultFactory.CreateSuccess() 
                : ResultFactory.CreateError(_errorMessage);
        }
    }
}