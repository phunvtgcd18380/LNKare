﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LNkareWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<T> CreateAsync(string url, T objToCreate);
        Task<bool> UpdateAsync(string url, T objToUpdate);
        Task<bool> DeleteAsync(string url, int id);
        Task<IEnumerable<T>> GetAllInSomeTableAsync(string url, int id);
    }
}
