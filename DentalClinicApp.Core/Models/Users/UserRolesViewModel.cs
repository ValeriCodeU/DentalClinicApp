﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Users
{
    public class UserRolesViewModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string[]? RoleNames { get; set; }
    }
}
