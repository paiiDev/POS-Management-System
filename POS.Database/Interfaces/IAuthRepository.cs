using POS.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetDataByUsernameAsync(string username);

    }
}
