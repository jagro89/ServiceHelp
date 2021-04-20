using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceHelp.ViewModels
{
    public class UserItemViewModels
    {
        public string Id { get; set; }

        [DisplayName("Adres Email")]
        public string Email { get; set; }

        [DisplayName("Nazwa użytkownika")]
        public string Username { get; set; }

        [DisplayName("Uprawnienia")]
        public string[] Roles { get; set; }
    }

    public class ManagementViewModels : List<UserItemViewModels>
    {
    }

    public class UserViewModels
    {
        public UserViewModels()
        {
            Roles = new string[0];
        }

        public string Id { get; set; }

        [DisplayName("Adres Email")]
        public string Email { get; set; }

        [DisplayName("Nazwa użytkownika")]
        public string Username { get; set; }

        [DisplayName("Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Powtórz hasło")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Hasła są niezgodne")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Nr telefonu")]
        public string Phone { get; set; }

        [DisplayName("Uprawnienia")]
        public string[] Roles { get; set; }
    }
}