using LNkareWeb.Models;
using LNkareWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LNkareWeb.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly IHttpClientFactory _clinetFactory;
        public CategoryRepository(IHttpClientFactory clientFactory) :base(clientFactory)
        {

        }
    }
}
