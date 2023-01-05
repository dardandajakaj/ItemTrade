using API.Entity;

namespace API.Interfaces
{
    public interface ITokenServiceProvider
    {
        string CreateToken(User user);
    }
}