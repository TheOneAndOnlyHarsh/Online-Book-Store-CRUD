using Microsoft.AspNetCore.Mvc;

namespace BookStore.Model.DTO
{
    public class LoginResponeDTO


    {
        public User User { get; set; }  

        public string Token { get; set; }   
    }
}
