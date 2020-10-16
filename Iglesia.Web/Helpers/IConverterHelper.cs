using Iglesia.Common.Responses;
using Iglesia.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Helpers
{
    public interface IConverterHelper
    {
        List<AssistancesResponse> ToAssistancesResponseList(List<Assistance> Assistance);


    }
}
