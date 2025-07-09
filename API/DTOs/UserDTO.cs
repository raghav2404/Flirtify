using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

    public class UserDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Token { get; set; }
    }
