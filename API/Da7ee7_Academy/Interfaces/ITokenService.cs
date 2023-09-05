using Da7ee7_Academy.Entities;

namespace Da7ee7_Academy.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
