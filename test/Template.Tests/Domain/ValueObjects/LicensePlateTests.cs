using Template.Api.Domain.ValueObjects;

namespace Template.Tests.Domain.ValueObjects;

public class LicensePlateTests
{
    public class ContructorTests
    {
        [Test]
        public async Task ThrowsOnNull()
        {
            // Act & Assert
            await Assert.That(() => new LicensePlate(null!)).Throws<FormatException>();
        }

        [Test]
        public async Task ThrowsOnEmpty()
        {
            // Act & Assert
            await Assert.That(() => new LicensePlate("")).Throws<FormatException>();
        }

        [Test]
        public async Task ThrowsOnWhitespace()
        {
            // Act & Assert
            await Assert.That(() => new LicensePlate("   ")).Throws<FormatException>();
        }

        [Test]
        public async Task NormalizesToUpperCase()
        {
            // Act
            var licensePlate = new LicensePlate("abc123");

            // Assert
            await Assert.That(licensePlate.ToJsonValue()).IsEqualTo("ABC123");
        }

        [Test]
        public async Task TrimsWhitespace()
        {
            // Act
            var licensePlate = new LicensePlate("  ABC123  ");

            // Assert
            await Assert.That(licensePlate.ToJsonValue()).IsEqualTo("ABC123");
        }
    }

    public class TryParseTests
    {
        [Test]
        public async Task ReturnsFalseOnNull()
        {
            // Act
            var result = LicensePlate.TryParse(null, null, out var plate);

            // Assert
            await Assert.That(result).IsFalse();
            await Assert.That(plate).IsDefault();
        }

        [Test]
        public async Task SucceedsOnValidInput()
        {
            // Act
            var result = LicensePlate.TryParse("ABC123", null, out var plate);

            // Assert
            await Assert.That(result).IsTrue();
            await Assert.That(plate.Value).IsEqualTo("ABC123");
        }
    }
}
