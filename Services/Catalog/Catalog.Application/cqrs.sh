#!/bin/bash

# Kullanıcıdan sınıf adı ve çoğul hali alın
read -p "Sınıf adını girin: " class_name
read -p "Sınıfın çoğul halini girin: " plural_class_name

# Kök dizin (ana klasör)
root_directory="$plural_class_name"

# Dizin oluşturma fonksiyonu
create_directory() {
    mkdir -p "$1"
}

# Dosya oluşturma fonksiyonu
create_file() {
    cat > "$1" <<EOL
$2
EOL
}

# Komutlar için klasör yapısı
commands_directory="$root_directory/Commands"
create_directory "$commands_directory"
create_directory "$commands_directory/Create${class_name}Command"
create_directory "$commands_directory/Delete${class_name}Command"
create_directory "$commands_directory/Edit${class_name}Command"

# Sorgular için klasör yapısı
queries_directory="$root_directory/Queries"
create_directory "$queries_directory"
create_directory "$queries_directory/Get${plural_class_name}Query"
create_directory "$queries_directory/Get${class_name}Query"

# Create.cs içerik
create_command_file="$commands_directory/Create${class_name}Command/Create${class_name}.cs"
create_file "$create_command_file" "
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.${plural_class_name}.Commands.Create${class_name}Command;

public record Create${class_name}(string Name, string Description, decimal Price) : IRequest<bool>;

public class Create${class_name}CommandHandler : IRequestHandler<Create${class_name}, bool>
{
    private readonly IApplicationDbContext _context;

    public Create${class_name}CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(Create${class_name} request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(Create${class_name}));

        var ${class_name,,} = new ${class_name}
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        await _context.${plural_class_name}.AddAsync(${class_name,,}, cancellationToken);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}
"

# Edit.cs içerik
edit_command_file="$commands_directory/Edit${class_name}Command/Edit${class_name}.cs"
create_file "$edit_command_file" "
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.${plural_class_name}.Commands.Edit${class_name}Command;

public record Edit${class_name}(int Id, string Name, string Description, decimal Price) : IRequest<bool>;

public class Edit${class_name}CommandHandler : IRequestHandler<Edit${class_name}, bool>
{
    private readonly IApplicationDbContext _context;

    public Edit${class_name}CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(Edit${class_name} request, CancellationToken cancellationToken)
    {
        Guard.Against.NotFound(request.Id, nameof(request));
        Guard.Against.NotFound(request.Name, nameof(request));
        Guard.Against.NotFound(request.Description, nameof(request));
        Guard.Against.NegativeOrZero(request.Price, nameof(request));

        var ${class_name,,} = await _context.${plural_class_name}.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (${class_name,,} == null) { return false; }

        ${class_name,,}.Name = request.Name;
        ${class_name,,}.Description = request.Description;
        ${class_name,,}.Price = request.Price;
        ${class_name,,}.Modified = DateTime.UtcNow;

        _context.${plural_class_name}.Update(${class_name,,});

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
"

# Delete.cs içerik
delete_command_file="$commands_directory/Delete${class_name}Command/Delete${class_name}.cs"
create_file "$delete_command_file" "
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.${plural_class_name}.Commands.Delete${class_name}Command;

public record Delete${class_name}(int Id) : IRequest<bool>;

public class Delete${class_name}CommandHandler : IRequestHandler<Delete${class_name}, bool>
{
    private readonly IApplicationDbContext _context;

    public Delete${class_name}CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(Delete${class_name} request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.Id, nameof(request.Id));

        var ${class_name,,} = await _context.${plural_class_name}.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (${class_name,,} == null) { return false; }
        _context.${plural_class_name}.Remove(${class_name,,});

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
"

# Get.cs içerik
get_query_file="$queries_directory/Get${plural_class_name}Query/Get${plural_class_name}.cs"
create_file "$get_query_file" "
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.${plural_class_name}.Queries.Get${plural_class_name}Query;

public record Get${plural_class_name} : IRequest<List<${class_name}>>;

public class Get${plural_class_name}QueryHandler : IRequestHandler<Get${plural_class_name}, List<${class_name}>>
{
    private readonly IApplicationDbContext _context;

    public Get${plural_class_name}QueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<${class_name}>> Handle(Get${plural_class_name} request, CancellationToken cancellationToken)
    {
        var ${plural_class_name,,} = await _context.${plural_class_name}.ToListAsync(cancellationToken);
        return ${plural_class_name,,};
    }
}
"

# Get.cs içerik
get_single_query_file="$queries_directory/Get${class_name}Query/Get${class_name}.cs"
create_file "$get_single_query_file" "
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

namespace Catalog.Application.${plural_class_name}.Queries.Get${class_name}Query;

public record Get${class_name}(int Id) : IRequest<${class_name}>;

public class Get${class_name}QueryHandler : IRequestHandler<Get${class_name}, ${class_name}>
{
    private readonly IApplicationDbContext _context;

    public Get${class_name}QueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<${class_name}> Handle(Get${class_name} request, CancellationToken cancellationToken)
    {
        var ${class_name,,} = await _context.${plural_class_name}.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return ${class_name,,};
    }
}
"

# İşlem tamamlandığında bilgi mesajı
echo "Folder structure created and files for $class_name have been added."
