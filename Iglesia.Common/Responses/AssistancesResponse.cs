
namespace Iglesia.Common.Responses
{
  public class AssistancesResponse
    {
        public int Id { get; set; }


        public UserResponse User { get; set; }


        public MeetingResponse Meeting { get; set; }


        public bool IsPresent { get; set; }

    }
}
