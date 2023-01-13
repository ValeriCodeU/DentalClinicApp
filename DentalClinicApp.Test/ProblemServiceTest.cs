﻿using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Test
{
    public class ProblemServiceTest
	{
        private IRepository repo;
        private IProblemService problemService;
        private DentalClinicDbContext dbContext;

        [SetUp]

        public void Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<DentalClinicDbContext>()
                .UseInMemoryDatabase("DentalDb")
                .Options;

            dbContext = new DentalClinicDbContext(contextOptions);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        [Test]

        public async Task ProblemExistsAsync_ShouldReturnTrue_WithValidId()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);

            await repo.AddAsync(new DentalProblem()
            {
                Id = 1,
                DiseaseName = "Cracked tooth",
                DiseaseDescription = "Playing boxing without a mouth guard",
                AlergyDescription = "drug allergy",
                DentalStatus = "55",
                PatientId = 1,
                IsActive = true,
            });
            await repo.SaveChangesAsync();

            var problemExist = await problemService.ProblemExistsAsync(1);

            Assert.That(problemExist, Is.True);
        }

        [Test]

        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var repo = new Repository(dbContext);
            problemService = new ProblemService(repo);


            var model = new ProblemFormModel()
            {
                DiseaseName = "Cracked tooth",
                DiseaseDescription = "Playing boxing without a mouth guard",
                AlergyDescription = "drug allergy",
                DentalStatus = "55",                
            };

            await problemService.CreateAsync(3, model);
            var problem = await repo.GetByIdAsync<DentalProblem>(1);

            Assert.That(problem.Id, Is.EqualTo(1));
            Assert.That(problem.DentalStatus, Is.EqualTo("55"));
            Assert.That(problem.IsActive, Is.True);
        }


        [TearDown]

        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
