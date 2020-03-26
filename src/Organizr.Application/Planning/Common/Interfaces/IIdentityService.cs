using System.Threading.Tasks;

namespace Organizr.Application.Planning.Common.Interfaces
{
    public interface IIdentityService
    {
        string CurrentUserId { get; }
        Task<bool> UserExistsAsync(string userId);
    }
}