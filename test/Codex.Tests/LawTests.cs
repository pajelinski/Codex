namespace Codex.Tests
{
    using System;
    using Result;
    using Xunit;

    public class LawTests
    {
        private readonly Nothing _nothing = new Nothing();
        private readonly Predicate<Nothing> _falsePredicate = x => false;
        private readonly Law<string> _isNotNullLaw;

        public LawTests()
        {
            _isNotNullLaw = new Law<string>(x => !string.IsNullOrEmpty(x), string.Empty);

        }

        [Fact]
        public void Evaluate_GivenFailingPredicate_ReturnsErrorMessage()
        {
            const string expectedErrorMessage = "errorMessage";
            var result = new Law<Nothing>(_falsePredicate, expectedErrorMessage).Evaluate(_nothing);
            Assert.Equal(expectedErrorMessage, result.GetError());
        }

        [Fact]
        public void Evaluate_GivenIsNotNullPredicate_WhenEvaluatedValueIsNull_ReturnsError() => 
            Assert.False(_isNotNullLaw.Evaluate(string.Empty).IsSuccess());

        [Fact]
        public void Evaluate_GivenIsNotNullPredicate_WhenEvaluatedValueIsNotNull_ReturnsSuccess() => 
            Assert.True(_isNotNullLaw.Evaluate("string").IsSuccess());

        [Fact]
        public void WhenPredicateIsNull_ThrowsArgumentNullException() => 
            Assert.Throws<ArgumentNullException>("predicate", () => new Law<Nothing>(null, string.Empty));

        [Fact]
        public void WhenErrorMessageIsNull_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>("errorMessage", () => new Law<Nothing>(_falsePredicate, null));
    }
}