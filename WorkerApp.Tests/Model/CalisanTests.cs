using NUnit.Framework;
using WorkerApp.Model;

namespace WorkerApp.Tests.Model
{
    [TestFixture]
    public class CalisanTests
    {
        [Test]
        public void Calisan_Properties_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var calisan = new Calisan
            {
                CalisanNo = 1,
                CalisanAdi = "Ali",
                CalisanSoyadi = "Veli",
                TamamlananGorevSayisi = 10,
                GecikenGorevSayisi = 2
            };

            // Assert
            Assert.That(calisan.CalisanNo, Is.EqualTo(1));
            Assert.That(calisan.CalisanAdi, Is.EqualTo("Ali"));
            Assert.That(calisan.CalisanSoyadi, Is.EqualTo("Veli"));
            Assert.That(calisan.TamamlananGorevSayisi, Is.EqualTo(10));
            Assert.That(calisan.GecikenGorevSayisi, Is.EqualTo(2));
        }

        [Test]
        public void Calisan_CanHaveNullTaskCounts()
        {
            // Arrange & Act
            var calisan = new Calisan
            {
                CalisanAdi = "Test",
                CalisanSoyadi = "User",
                TamamlananGorevSayisi = null,
                GecikenGorevSayisi = null
            };

            // Assert
            Assert.That(calisan.TamamlananGorevSayisi, Is.Null);
            Assert.That(calisan.GecikenGorevSayisi, Is.Null);
        }

        [Test]
        public void Calisan_ProjeCalisanlar_ShouldBeInitializable()
        {
            // Arrange & Act
            var calisan = new Calisan
            {
                CalisanAdi = "Test",
                CalisanSoyadi = "User",
                ProjeCalisanlar = new List<ProjeCalisan>()
            };

            // Assert
            Assert.That(calisan.ProjeCalisanlar, Is.Not.Null);
            Assert.That(calisan.ProjeCalisanlar, Is.Empty);
        }
    }
}
