﻿using System.ComponentModel.DataAnnotations;

namespace Taskmanager_MVCAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Batch { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
