namespace DentalClinicApp.Core.Constants
{
    public static class ModelConstant
    {
        public static class User
        {
            public const int MaxUsernameLength = 20;
            public const int MinUsernameLength = 5;

            public const int MaxFirstNameLength = 30;
            public const int MinFirstNameLength = 2;

            public const int MaxLastNameLength = 30;
            public const int MinLastNameLength = 2;

            public const int MaxPasswordLength = 20;
            public const int MinPasswordLength = 5;

            public const int MaxEmaildLength = 60;
            public const int MinEmailLength = 10;
        }

        public static class DentalProblem
        {
            public const int MaxDiseaseName = 50;
            public const int MinDiseaseName = 5;

            public const int MaxDiseaseDescription = 500;
            public const int MinDiseaseDescription = 5;

            public const int MaxAlergyDescription = 50;
            public const int MinAlergyDescription = 5;

            public const int MaxDentalStatus = 50;
            public const int MinDentalStatus = 2;
        }

        public static class Attendance
        {
            public const int MaxClinicRemarksLength = 500;
            public const int MinClinicRemarksLength = 10;

            public const int MaxDiagnosislength = 200;
            public const int MinDiagnosislength = 10;
        }

        public static class DentalProcedure
        {
            public const int MaxProcedureName = 50;
            public const int MinProcedureName = 5;

            public const int MaxProcedureDescription = 500;
            public const int MinProcedureDescription = 10;
            
            public const string MaxPricePerProcedure = "2000";
            public const string MinPricePerProcedure = "0.0";
        }
    }
}
