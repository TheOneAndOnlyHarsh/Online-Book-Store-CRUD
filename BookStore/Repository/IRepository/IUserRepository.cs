using BookStore.Model;
using BookStore.Model.DTO;

namespace BookStore.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string name);
        Task<LoginResponeDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<User> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
