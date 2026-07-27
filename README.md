# CommsManager 🎨📱

**Профессиональная система управления заказами для творческих профессионалов**
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MAUI](https://img.shields.io/badge/MAUI-Blazor_Hybrid-0078D4?logo=xamarin)](https://learn.microsoft.com/dotnet/maui/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-5C2D91?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![Code Style](https://img.shields.io/badge/code_style-C%23_12-239120?logo=csharp)](https://learn.microsoft.com/dotnet/csharp/)

## 🌟 О проекте

CommsManager — это многоплатформенное решение для художников, крафтеров, фотографов и других творческих профессионалов, которое помогает управлять заказами, клиентами и демонстрировать работы через персонализированную страницу-визитку.

**Ключевые возможности:**

- 📋 Управление заказами с трекингом статусов
- 👥 База клиентов и история взаимодействий
- 🖼️ Портфолио с примерами работ
- 💰 Гибкая система прайс-листов
- 🌐 Личная страница-визитка (а-ля Linktree)
- 📱 QR-код для быстрого доступа к профилю
- 🔄 Онлайн/офлайн синхронизация
- 📊 Аналитика и отчетность

## 📁 Структура решения

```
CommsManager/
├── 📁 CommsManager.Core/           # Ядро приложения
│   ├── Entities/                   # Доменные сущности
│   ├── Models/                     # Модели данных для сущностей и не только
│   ├── Interfaces/                 # Абстракции
│   ├── Services/                   # Доменные сервисы
│   ├── Specifications/             # Спецификации
│   ├── ValueObjects/               # Объекты для значений
│   └── Events/                     # Доменные события
├── 📁 CommsManager.Infrastructure/ # Репозитории, БД
├── 📁 CommsManager.Application/    # Сценарии использования
├── 📁 CommsManager.Web/            # Web-приложение (Blazor WASM)
├── 📁 CommsManager.Maui/           # Мобильное приложение
├── 📁 CommsManager.API/            # Web API (опционально)
├── 📁 Docs/                        # Документация
├── 📄 LICENSE                      # Лицензия
├── 📄 README.md                    # Этот файл
├── 📄 .gitignore                   # Git игнорирование
└── 📄 CommsManager.slnx            # Файл решения
```

## 🛠️ Технологический стек

| Технология | Назначение | Версия |
|------------|------------|---------|
| **.NET 10** | Основная платформа | 10.0+ |
| **MAUI Blazor Hybrid** | Мобильные приложения | 10.0 |
| **Blazor WebAssembly** | Веб-приложение | 10.0 |
| **Entity Framework Core** | ORM и работа с БД | 10.0 |
| **SQL Server** | Базы данных | 2022 |

## 🐳 Запуск с Docker

Для быстрого развёртывания всех сервисов (API, Web, база данных) используйте Docker Compose.

### Предварительные требования
- Docker Desktop (или Docker Engine + Compose)
- (опционально) .NET 10 SDK для локальной разработки

### Настройка
1. Скопируйте файл `.env.example` в `.env` и задайте пароль для БД.
2. Убедитесь, что порты 5000, 5001, 1433 свободны.

### Запуск
```bash
docker-compose up -d --build
```

## 📞 Контакты

**Автор:** [Гоглов Максим Алексеевич]  
**Email:** [max.gog2005@outlook.com]  
**Telegram:** [@maxgog]  
**Issues:** [GitHub Issues](https://github.com/MaxGog/CommsManager/issues)
