using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public interface IAvatarRepository
    {
        Task<int> CreateAvatarFromUrl(string url);
    }
}
