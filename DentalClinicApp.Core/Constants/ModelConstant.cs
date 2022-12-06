﻿namespace DentalClinicApp.Core.Constants
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
            public const int MinDentalStatus = 5;
        }
    }
}
