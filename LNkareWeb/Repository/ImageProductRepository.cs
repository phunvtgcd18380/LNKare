using LNkareWeb.Models;
using LNkareWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LNkareWeb.Repository
{
    public class ImageProductRepository : Repository<ImageProduct>, IImageProductRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public ImageProductRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {

        }
    }
}
