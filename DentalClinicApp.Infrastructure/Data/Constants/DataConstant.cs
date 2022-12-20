namespace DentalClinicApp.Infrastructure.Data.Constants
{
    public static class DataConstant
    {
        public static class ApplicationUser
        {
            public const int MaxUserFirstName = 50;          
            public const int MaxUserLastName = 50;          
        }      

        public static class DentalProblem
        {
            public const int MaxDiseaseName = 50;
            public const int MaxDiseaseDescription = 500;
            public const int MaxAlergyDescription = 50;
            public const int MaxDentalStatus = 5;
        }

        public static class DentalProcedure
        {
            public const int MaxProcedureName = 50;
            public const int MaxProcedureDescription = 500;
            public const int MaxProcedureNote = 100;
            public const int ProcedurePrecisionDecimal = 18;
            public const int ProcedureScaleDecimal = 2;
        }

        public static class Apppointment
        {
            public const int MaxDetailsLength = 100;
        }

        public static class Attendance
        {
            public const int MaxClinicRemarksLength = 500;
            public const int MaxDiagnosislength = 200;
        }

    }
}
