using System.Collections.Generic;

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

        public class NotNullLawTests
        {
            [Fact]
            public void GivenNull_ReturnsError()
            {
                var result = Law<object>.CreateNotNull("subjectName").Evaluate(null);
                Assert.False(result.IsSuccess());
                Assert.Equal("'subjectName' should not be null.", result.GetError());
            }

            [Fact]
            public void GivenObject_ReturnsSuccess()
            {
                var result = Law<object>.CreateNotNull(string.Empty).Evaluate(new object());
                Assert.True(result.IsSuccess());
            }
        }

        public class NotNullElementsLaw
        {
            [Fact]
            public void GivenAllNullElements_ReturnsError()
            {
                var collectionWithNullValue = new object[] { null, null };
                var result = Law<object>.CreateNotNullElements("subjectName").Evaluate(collectionWithNullValue);
                Assert.False(result.IsSuccess());
                Assert.Equal("'subjectName' should contain not null elements.", result.GetError());
            }

            [Fact]
            public void GivenAllNotNullElements_ReturnsError()
            {
                var testCollection = new[] { "subject1", "subject2" };
                var result = Law<object>.CreateNotNullElements(string.Empty).Evaluate(testCollection);
                Assert.True(result.IsSuccess());
            }
        }
    }
}