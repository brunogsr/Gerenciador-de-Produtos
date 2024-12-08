﻿using System.ComponentModel.DataAnnotations;

namespace GerenciadorProdutos.Models.Users
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
    }
}
