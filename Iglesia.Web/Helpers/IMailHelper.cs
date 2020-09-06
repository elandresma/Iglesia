using Iglesia.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Helpers
{
        public interface IMailHelper
        {
            Response SendMail(string to, string subject, string body);
        }
}
