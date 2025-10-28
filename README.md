# ContractSystem

Учебный проект для изучения разработки приложений на основе C\#, Blazor, Entity Framework.

## Описание

Проект представляет собой систему, позволяющую согласовывать документы и администрировать пользователей.

## Технологии

- C\#
- Blazor
- Entity Framework Core
- ASP.NET Core
- Mapster


## Требования

- .NET SDK (рекомендуется 8 версия)
- Любая СУБД, поддерживаемая Entity Framework Core (например, SQL Server)
- Перед запуском необходимо добавить переменную окружения `ConnectionString` для подключения к базе данных.


## Архитектура

В проекте реализована многослойная архитектура:

- **Api** — [ContractSystem.Api](/ContractSystem.Api) — реализация Api на ASP.NET Core.
- **Frontend** — [ContractSystem.WebApp](/ContractSystem.WebApp) — реализация UI на Blazor.
- **Application Layer** — [ContractSystem.Service](/ContractSystem.Service) — бизнес-логика.
- **Data Access Layer** — [ContractSystem.Repositories](/ContractSystem.Repositories) — работа с БД через Entity Framework Core.
- **Models & Mapping** —  [ContractSystem.Core](/Core) — модели, интерфейсы, преобразование между доменными и DTO-объектами

## Запуск проекта

1. Клонируйте репозиторий:

```
git clone https://github.com/elisevgeniy/ContractSystem.git
```

2. Добавьте переменную окружения с вашей строкой подключения:

Windows
``` 
set ConnectionString=Host=192.168.1.1; Port=1111; Username=user; Password=pass; Database=dbname
```

3. Соберите и запустите проект:

```
dotnet build
```
Запуск API
```
dotnet run --project .\ContractSystem.WebApp\ContractSystem.WebApp
```
Запуск WebApp
```
dotnet run --project .\ContractSystem.Api
```


## Контакты

Автор проекта: [elisevgeniy](https://github.com/elisevgeniy)

