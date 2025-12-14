namespace Shared.Models.General;

public record LookupDto<TId>(TId Id, string Name);

public record LookupDto(string Id, string Name)
    : LookupDto<string>(Id, Name);
