using Microsoft.EntityFrameworkCore;
using PhoneDirectory.Entities.Entities;
using PhoneDirectory.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDirectory.Repository.Repositories
{
    public class InformationRepository : Repository<Information>, IInformationRepository
    {
        public InformationRepository(PhoneDirectoryDbContext context)
              : base(context)
        {

        }



    }
}
