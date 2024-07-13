namespace Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, ToClientCompany>()
            .ForMember((ToClientCompany c) => c.FullAddress, 
                memberOptions: (IMemberConfigurationExpression<Company, ToClientCompany, string> opts) => 
                    opts.MapFrom((Company c) => string.Join(",", c.Address, c.Country)));
    }
}