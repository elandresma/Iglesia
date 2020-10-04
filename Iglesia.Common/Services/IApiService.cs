using Iglesia.Common.Requests;
using Iglesia.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Iglesia.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
        Task<Response> GetUsersAsync<T>(string urlBase, string servicePrefix, string controller,EmailRequest email);
        

    }
}
