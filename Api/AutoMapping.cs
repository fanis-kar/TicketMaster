using AutoMapper;
using TicketMaster.Api.Model;
using TicketMaster.Data.Model;

namespace TicketMaster
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {

            CreateMap<Act, ActJSON>().ReverseMap();
            CreateMap<Venue, VenueJSON>().ReverseMap();
            CreateMap<Ticket, TicketJSONRequest>().ReverseMap();
            CreateMap<Ticket, TicketJSONReply>().ReverseMap();
            CreateMap<Show, ShowJSON>().ReverseMap();
        }
    }
}
