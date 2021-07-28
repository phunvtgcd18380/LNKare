using LNkareWeb.Models;
using LNkareWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LNkareWeb.Repository
{
    public class ProductRepository : Repository<Product> , IProductRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public ProductRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
