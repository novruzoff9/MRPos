using Store.Application.Common.Models.Branch;

namespace Store.Application.Common.Mappings;

internal class BranchMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Branch, BranchReturnDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Is24Hour, src => src.Is24Hour)
            .Map(dest => dest.Opening, src => src.Opening)
            .Map(dest => dest.Closing, src => src.Closing)
            .Map(dest => dest.ServiceFee, src => src.ServiceFee)
            .Map(dest => dest.TableCount,
                src => src.Tables.AsQueryable().Count())
            .Map(dest => dest.Address, src => new AddressDto(
                src.Address.Street,
                src.Address.City,
                src.Address.State,
                src.Address.ZipCode,
                src.Address.Country,
                src.Address.GoogleMapsLocation
            ));
    }
}
