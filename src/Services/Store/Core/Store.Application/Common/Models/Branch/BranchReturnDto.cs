namespace Store.Application.Common.Models.Branch;

public record BranchReturnDto
(
    string Id,
    string Name,
    string PhoneNumber,
    string? Description,
    bool Is24Hour,
    TimeOnly? Opening,
    TimeOnly? Closing,
    decimal ServiceFee,
    string CompanyId,
    AddressDto Address
);

public record AddressDto
(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country,
    string GoogleMapsLocation
);