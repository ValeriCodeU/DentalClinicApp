using DentalClinicApp.Infrastructure.Data.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Test
{
	public class PatientServiceTest
	{
        private IRepository repo;

        [SetUp]

        public void Setup()
        {
            var repoMock = new Mock<IRepository>();
        }

        [TearDown]

        public void TearDown()
        {

        }
    }
}
