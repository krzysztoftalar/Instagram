using System.Threading.Tasks;
using Application.Errors;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEmailAccessor
    {
        Task<ResponseError> SendUserEmailVerificationAsync(AppUser user);
    }
}