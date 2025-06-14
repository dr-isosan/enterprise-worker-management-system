using NUnit.Framework;
using WorkerApp.Model;

namespace WorkerApp.Tests.Model
{
    [TestFixture]
    public class ProjeTests
    {
        [Test]
        public void Proje_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 6, 1);

            // Act
            var proje = new Proje
            {
                ProjeNo = 1,
                ProjeAdi = "Enterprise System",
                BaslangicTarihi = startDate,
                BitisTarihi = endDate,
                GecikmeMiktari = 5
            };

            // Assert
            Assert.That(proje.ProjeNo, Is.EqualTo(1));
            Assert.That(proje.ProjeAdi, Is.EqualTo("Enterprise System"));
            Assert.That(proje.BaslangicTarihi, Is.EqualTo(startDate));
            Assert.That(proje.BitisTarihi, Is.EqualTo(endDate));
            Assert.That(proje.GecikmeMiktari, Is.EqualTo(5));
        }

        [Test]
        public void Proje_CanHaveNullDelayAmount()
        {
            // Arrange & Act
            var proje = new Proje
            {
                ProjeAdi = "Test Project",
                BaslangicTarihi = DateTime.Now,
                BitisTarihi = DateTime.Now.AddDays(30),
                GecikmeMiktari = null
            };

            // Assert
            Assert.That(proje.GecikmeMiktari, Is.Null);
        }

        [Test]
        public void Proje_Collections_ShouldBeInitializable()
        {
            // Arrange & Act
            var proje = new Proje
            {
                ProjeAdi = "Test Project",
                BaslangicTarihi = DateTime.Now,
                BitisTarihi = DateTime.Now.AddDays(30),
                ProjeCalisanlar = new List<ProjeCalisan>(),
                Gorevler = new List<Gorev>()
            };

            // Assert
            Assert.That(proje.ProjeCalisanlar, Is.Not.Null);
            Assert.That(proje.ProjeCalisanlar, Is.Empty);
            Assert.That(proje.Gorevler, Is.Not.Null);
            Assert.That(proje.Gorevler, Is.Empty);
        }
    }
}
