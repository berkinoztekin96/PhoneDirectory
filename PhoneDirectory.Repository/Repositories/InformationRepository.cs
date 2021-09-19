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
        private readonly PhoneDirectoryDbContext dbContext;
        public InformationRepository(PhoneDirectoryDbContext context)
            : base(context)
        {
            dbContext = context;
        }


        public async Task<Information> GetInformationByUserId(int personId)
        {
            try
            {
                return await FindBy(x => x.PersonId == personId).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Information> DeleteInformation(int id) // soft delete
        {
            try
            {
                Information information = await dbContext.Informations.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (information != null)
                {
                    information.Status = 0;
                    return information;
                }

                else
                    return null;
                    

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
