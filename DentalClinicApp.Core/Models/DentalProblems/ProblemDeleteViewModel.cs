using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.DentalProblems
{
    public class ProblemDeleteViewModel
    {
        public int Id { get; set; }

        public string DiseaseName { get; set; } = null!;

        public string DiseaseDescription { get; set; } = null!;       

    }
}
