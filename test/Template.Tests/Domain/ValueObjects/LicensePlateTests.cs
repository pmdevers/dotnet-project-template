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
            await Assert.That(() => LicensePlate.Create(null!)).Throws<ArgumentException>();
        }

        [Test]
        public async Task ThrowsOnEmpty()
        {
            // Act & Assert
            await Assert.That(() => LicensePlate.Create("")).Throws<ArgumentException>();
        }

        [Test]
        public async Task ThrowsOnWhitespace()
        {
            // Act & Assert
            await Assert.That(() => LicensePlate.Create("   ")).Throws<ArgumentException>();
        }

        [Test]
        public async Task NormalizesToUpperCase()
        {
            // Act
            var licensePlate = LicensePlate.Create("abc123");

            // Assert
            await Assert.That(licensePlate.ToString()).IsEqualTo("ABC123");
        }

        [Test]
        public async Task TrimsWhitespace()
        {
            // Act
            var licensePlate = LicensePlate.Create("  ABC123  ");

            // Assert
            await Assert.That(licensePlate.ToString()).IsEqualTo("ABC123");
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
            await Assert.That(plate.ToString()).IsEqualTo("ABC123");
        }
    }
}
