# Самостійна робота №15  
## Аналіз принципів SRP та OCP в open-source проєкті на C#

## Мета роботи
Метою самостійної роботи є дослідження застосування принципів SRP (Single Responsibility Principle) та OCP (Open/Closed Principle) у реальному open-source проєкті на мові програмування C#, а також аналіз їх впливу на архітектуру та підтримуваність коду.

---

## 1. Обраний open-source проєкт
- Назва: ASP.NET Core

ASP.NET Core є великим open-source фреймворком від Microsoft, який активно використовує принципи SOLID та має добре структуровану архітектуру.

---

## 2. Аналіз SRP (Single Responsibility Principle)

### 2.1. Приклади дотримання SRP

#### Клас: `ControllerBase`
- Відповідальність: базова логіка HTTP-контролерів
- Обґрунтування: клас не відповідає за бізнес-логіку або доступ до даних, а лише за обробку HTTP-запитів

```csharp
public abstract class ControllerBase
{
    public HttpContext HttpContext { get; }
}