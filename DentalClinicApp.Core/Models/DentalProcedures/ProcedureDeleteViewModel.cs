using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.DentalProcedures
{
    public class ProcedureDeleteViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string StartDate { get; set; } = null!;

        public string EndDate { get; set; } = null!;
    }
}
