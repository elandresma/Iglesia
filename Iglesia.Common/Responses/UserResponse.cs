using Iglesia.Common.Entities;
using Iglesia.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iglesia.Common.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public Guid ImageId { get; set; }
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://iglesiaamarinh.azurewebsites.net/images/noimage.png"
            : $"https://iglesiamarin.blob.core.windows.net/users/{ImageId}";

        public UserType UserType { get; set; }


        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        public ICollection<AssistancesResponse> Assistances { get; set; }
        public Profession Profession { get; set; }

        public ChurchResponse Church { get; set; }
    }
}
