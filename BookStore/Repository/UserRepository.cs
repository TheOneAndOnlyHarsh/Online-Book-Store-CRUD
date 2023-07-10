using BookStore.Data;
using BookStore.Model;
using BookStore.Model.DTO;
using BookStore.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Repository
{
    public class UserRepository : IUserRepository
    {
        public ApplicationDbContext _db { get; set; }
        private string secretKey;
        public UserRepository(ApplicationDbContext db , IConfiguration configuration  ) { 

            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");


        }

        public bool isUniqueUser(string name)
        {
            var user = _db.Users.FirstOrDefault(b => b.Name == name);
            if (user == null)
            {

                return true;
            }
            return false;
        }

        public async Task<LoginResponeDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            // validation of Name and password..

           
            var user = _db.Users.FirstOrDefault(b=> b.Name.ToLower()== loginRequestDTO.Name.ToLower()
            
            && b.Password == loginRequestDTO.Password);

            if (user == null) 
            {
                return null;
            }

            // then generating token if user was found , here a secreat key is used to authenticate a token

            var tokenHandle = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                // if we have multiple roles then we can add multiple claims

                Subject = new ClaimsIdentity(new Claim[]
                {

                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)


                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandle.CreateToken(tokenDescriptor);
            LoginResponeDTO loginResponeDTO = new LoginResponeDTO()
            {

                Token = tokenHandle.WriteToken(token),
                User = user,
            };
            return loginResponeDTO;
        }

        public async Task<User> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            // we will add new user here:

            User user = new User()
            {

                Name = registrationRequestDTO.Name,
                Email = registrationRequestDTO.Email,
                Password = registrationRequestDTO.Password,
                Role = registrationRequestDTO.Role

            };
             _db.Users.Add(user);
             await _db.SaveChangesAsync();
             user.Password = " ";
             return user;
        }
    }
}
