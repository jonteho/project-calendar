﻿using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
		[Required]
		[EmailAddress]
        public string Email { get; set; }
    }
}