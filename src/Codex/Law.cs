namespace Codex
{
    using System;
    using Result;

    public interface ILaw<in T>
    {
        IResult<Nothing> Evaluate(T subject);
    }

    public class Law<T> : ILaw<T>
    {
        private readonly Predicate<T> _predicate;
        private readonly string _errorMessage;

        public static Law<T> CreateNotNull(string name) => 
            new Law<T>(x => x != null, $"'{name}' should not be null.");

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