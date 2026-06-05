using Template.Api.Domain.ValueObjects;

namespace Template.Tests.Domain.ValueObjects;

public class NonEmptyStringTests
{
    public class ConstructorTests
    {
        [Test]
        public async Task ThrowsOnNull()
        {
            await Assert.That(() => NonEmptyString.Create(null!)).Throws<ArgumentException>();
        }

        [Test]
        public async Task ThrowsOnEmpty()
        {
            await Assert.That(() => NonEmptyString.Create(string.Empty)).Throws<ArgumentException>();
        }

        [Test]
        public async Task ThrowsOnWhitespace()
        {
            await Assert.That(() => NonEmptyString.Create("   ")).Throws<ArgumentException>();
        }

        [Test]
        public async Task ReturnsValueOnValidInput()
        {
            var value = NonEmptyString.Create("hello");

            await Assert.That(value.ToString()).IsEqualTo("hello");
        }
    }

    public class TryParseTests
    {
        [Test]
        public async Task ReturnsFalseOnNull()
        {
            var result = NonEmptyString.TryParse(null, null, out var value);

            await Assert.That(result).IsFalse();
            await Assert.That(value).IsDefault();
        }

        [Test]
        public async Task ReturnsFalseOnEmpty()
        {
            var result = NonEmptyString.TryParse(string.Empty, null, out var value);

            await Assert.That(result).IsFalse();
            await Assert.That(value).IsDefault();
        }

        [Test]
        public async Task SucceedsOnValidInput()
        {
            var result = NonEmptyString.TryParse("hello", null, out var value);

            await Assert.That(result).IsTrue();
            await Assert.That(value.ToString()).IsEqualTo("hello");
        }
    }

    public class ImplicitOperatorTests
    {
        [Test]
        public async Task ConvertsToString()
        {
            NonEmptyString value = "hello";

            string text = value;

            await Assert.That(text).IsEqualTo("hello");
        }

        [Test]
        public async Task ConvertsFromString()
        {
            NonEmptyString value = "hello";

            await Assert.That(value.ToString()).IsEqualTo("hello");
        }

        [Test]
        public async Task ThrowsOnEmpty()
        {
            await Assert.That(() => (NonEmptyString)string.Empty).Throws<ArgumentException>();
        }

        [Test]
        public async Task ThrowsOnWhitespace()
        {
            await Assert.That(() => (NonEmptyString)"   ").Throws<ArgumentException>();
        }
    }
}
