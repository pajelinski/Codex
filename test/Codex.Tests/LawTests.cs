namespace Codex.Tests
{
    using System;
    using Xunit;

    public class LawTests
    {
        private readonly Func<bool> _falsePredicate = () => false;
        private readonly Func<string,ILaw> _createIsNotNullLaw;

        public LawTests()
        {
            _createIsNotNullLaw = x => new Law(() => !string.IsNullOrEmpty(x), string.Empty);

        }

        [Fact]
        public void Evaluate_GivenFailingPredicate_ReturnsErrorMessage()
        {
            const string expectedErrorMessage = "errorMessage";
            var result = new Law(_falsePredicate, expectedErrorMessage).Evaluate();
            Assert.Equal(expectedErrorMessage, result.GetError());
        }

        [Fact]
        public void Evaluate_GivenIsNotNullPredicate_WhenEvaluatedValueIsNull_ReturnsError() => 
            Assert.False(_createIsNotNullLaw(string.Empty).Evaluate().IsSuccess());

        [Fact]
        public void Evaluate_GivenIsNotNullPredicate_WhenEvaluatedValueIsNotNull_ReturnsSuccess() => 
            Assert.True(_createIsNotNullLaw("string").Evaluate().IsSuccess());

        [Fact]
        public void WhenPredicateIsNull_ThrowsArgumentNullException() => 
            Assert.Throws<ArgumentNullException>("predicate", () => new Law(null, string.Empty));

        [Fact]
        public void WhenErrorMessageIsNull_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>("errorMessage", () => new Law(_falsePredicate, null));

        public class NotNullLawTests
        {
            [Fact]
            public void GivenNull_ReturnsError()
            {
                var result = Law.CreateNotNull<object>("subjectName", null).Evaluate();
                Assert.False(result.IsSuccess());
                Assert.Equal("'subjectName' should not be null.", result.GetError());
            }

            [Fact]
            public void GivenObject_ReturnsSuccess()
            {
                var result = Law.CreateNotNull(string.Empty, new object()).Evaluate();
                Assert.True(result.IsSuccess());
            }
        }

        public class NotNullElementsLaw
        {
            [Fact]
            public void GivenAllNullElements_ReturnsError()
            {
                var collectionWithNullValue = new object[] { null, null };
                var result = Law.CreateNotNullElements("subjectName", collectionWithNullValue).Evaluate();
                Assert.False(result.IsSuccess());
                Assert.Equal("'subjectName' should contain not null elements.", result.GetError());
            }

            [Fact]
            public void GivenAllNotNullElements_ReturnsError()
            {
                var testCollection = new[] { "subject1", "subject2" };
                var result = Law.CreateNotNullElements(string.Empty, testCollection).Evaluate();
                Assert.True(result.IsSuccess());
            }
        }
    }
}