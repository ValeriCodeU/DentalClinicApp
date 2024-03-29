﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Users
{
	public class UserEditViewModel
	{
		public Guid Id { get; set; }

		[Required]
        [Display(Name = "First Name")]

        public string? FirstName { get; set; }

		[Required]
        [Display(Name = "Last Name")]

        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Last Name")]

        public string Username { get; set; }

        [Required]
        [Display(Name = "Email")]

        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }
	}
}
