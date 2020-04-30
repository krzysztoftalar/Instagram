using System.Threading.Tasks;

namespace Application.Services.Profiles
{
    public interface IProfileReader
    {
        Task<Profile> ReadProfile(string username);
    }
}