# MRPos

**MRPos** (Minimalist Restoran Satış və İdarəetmə Sistemi) restoran və restoran şəbəkələrinin satış, inventar və işçi idarəetməsini asanlaşdırmaq üçün hazırlanmış veb əsaslı bir tətbiqdir. Bu sistem, restoranların məhsul məlumatlarını idarə etməsinə, satış əməliyyatlarını qeyd etməsinə və vizual hesabatlar yaratmasına imkan verir.

Layihə Microservice arxitekturası əsasında hazırlanmışdır. İşçilərin rol və vəzifələrinin idarə olunması üçün `IdentityServer` vardır ki, onun məlumatları ayrı bir databazada saxlanılır. Bu server vasitəsilə giriş edən şəxs öz məlumatlarına əsasən token əldə edir və özünə uyğun səlahiyyətləri olur.

`JWT token` vasitəsilə giriş edən şəxsin aid olduğu olduğu şirkət, işlədiyi filial və rolu kimi məlumatları saxlanılır və hər hansı bir `endpoint`-ə sorğu göndərilən zaman onun şirkətinə və filialına aid olan məlumatlar gətirilir. 

`OrderService` daxilində verilən sifarişlərin hər an izlənilməsi üçün `SignalR` texnologiyası istifadə olunmuşdur. Bunun vasitəsilə müştəri resoran daxilində ofisiantı çağırmadan öz sifarişini verdikdə, və ya Mətbəxdə hər hansısa sifariş hazırlandıqda ofisiantlara anındaca məlumat ötürülür.

Digər Servislər isə ümumi bir databazada saxlanılır. Müxtəlif növlərdə arxitekturalara sahib olan servislər vardır.

## Xüsusiyyətlər

- **Məhsul idarəetməsi**: Məhsul əlavə etmə, redaktə etmə və silmə funksiyaları.
- **Filial İdarəetməsi**: Restoran şəbəkəsinin daxili filiallarını və onların öz menyu və işçilərinin idarə olunması
- **Satış əməliyyatları**: Satışların qeyd edilməsi və izlənməsi.
- **Hesabatlar**: Satış məlumatlarına əsaslanan qrafik və cədvəl şəklində hesabatların yaradılması.
- **Müştəriyönümlülük**: Hər bir restoran şəbəkəsinin öz məlumatlarına əsasən müştərilərin hər bir filial və qidalar barədə məlumat ala bilməsi üçün avtomatik hazırlanan reklam səhifələri

## Kitabxanalar
- **MediatR**: CQRS nümunəsini tətbiq etmək üçün istifadə olunur.
- **Entity Framework Core**: Verilənlər bazası əməliyyatları üçün istifadə olunur
- **Mapster**: Dto-lar və entitylər arasında maplama üçün istifadə olunur.
- **FluentValidation**: Sorğuların və əmrlərin doğrulanması üçün istifadə olunur.
- **SignalR**: Real-time məlumat ötürülməsi üçün istifadə olunur.


## Texnologiyalar

- **Backend**: C# ASP.NET Core
- **Verilənlər Bazası**: MsSql
- **Digər texnologiyalar**: JWT token, CQRS, SignalR, Toast