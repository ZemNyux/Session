using System.ComponentModel.DataAnnotations;

namespace Session.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Поле має бути встановлене")]
        [Display(Name = "Ім'я користувача: ")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Поле має бути встановлене")]
        [Display(Name = "Пароль: ")]
        public string? Password { get; set; }
    }
}