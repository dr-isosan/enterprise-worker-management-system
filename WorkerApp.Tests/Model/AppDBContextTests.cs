using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WorkerApp.Model;

namespace WorkerApp.Tests.Model
{
    [TestFixture]
    public class AppDBContextTests
    {
        private AppDBContext _context = null!;
        private DbContextOptions<AppDBContext> _options = null!;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(_options);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void CanCreateDatabase()
        {
            // Act
            var created = _context.Database.EnsureCreated();

            // Assert
            Assert.That(created, Is.True);
        }

        [Test]
        public void CanAddEmployee()
        {
            // Arrange
            var employee = new Calisan
            {
                CalisanAdi = "John",
                CalisanSoyadi = "Doe",
                TamamlananGorevSayisi = 5,
                GecikenGorevSayisi = 2
            };

            // Act
            _context.Calisanlar.Add(employee);
            _context.SaveChanges();

            // Assert
            var savedEmployee = _context.Calisanlar.First();
            Assert.That(savedEmployee.CalisanAdi, Is.EqualTo("John"));
            Assert.That(savedEmployee.CalisanSoyadi, Is.EqualTo("Doe"));
        }

        [Test]
        public void CanAddProject()
        {
            // Arrange
            var project = new Proje
            {
                ProjeAdi = "Test Project",
                BaslangicTarihi = DateTime.Now,
                BitisTarihi = DateTime.Now.AddDays(30),
                GecikmeMiktari = 0
            };

            // Act
            _context.Projeler.Add(project);
            _context.SaveChanges();

            // Assert
            var savedProject = _context.Projeler.First();
            Assert.That(savedProject.ProjeAdi, Is.EqualTo("Test Project"));
        }

        [Test]
        public void CanCreateProjectEmployeeRelationship()
        {
            // Arrange
            var employee = new Calisan { CalisanAdi = "Jane", CalisanSoyadi = "Smith" };
            var project = new Proje { ProjeAdi = "Important Project", BaslangicTarihi = DateTime.Now, BitisTarihi = DateTime.Now.AddDays(60) };

            _context.Calisanlar.Add(employee);
            _context.Projeler.Add(project);
            _context.SaveChanges();

            var projectEmployee = new ProjeCalisan
            {
                ProjeNo = project.ProjeNo,
                CalisanNo = employee.CalisanNo
            };

            // Act
            _context.ProjeCalisanlar.Add(projectEmployee);
            _context.SaveChanges();

            // Assert
            var relationship = _context.ProjeCalisanlar.First();
            Assert.That(relationship.ProjeNo, Is.EqualTo(project.ProjeNo));
            Assert.That(relationship.CalisanNo, Is.EqualTo(employee.CalisanNo));
        }
    }
}
