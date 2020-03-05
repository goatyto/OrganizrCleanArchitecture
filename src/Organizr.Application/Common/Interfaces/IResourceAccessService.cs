using System;
using System.Threading.Tasks;

namespace Organizr.Application.Common.Interfaces
{
    public interface IResourceAccessService
    {
        bool CanAccess(Guid resourceId, string userId);
    }
}