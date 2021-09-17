﻿using Microsoft.EntityFrameworkCore;
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


        public async Task<Information> GetInformationByUserId(int personId)
        {
            try
            {
                return await GetAllAsync().Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> DeleteInformation(int id) // soft delete
        {
            try
            {
                Information information = await GetAllAsync().Where(x => x.Id == id).FirstOrDefaultAsync();

                if (information != null)
                {
                    information.Status = 0;
                    return true;
                }

                else
                    return false;
                    

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}