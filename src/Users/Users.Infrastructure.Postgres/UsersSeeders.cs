using System.Security.Cryptography;
using Baskets.Domain;
using Microsoft.Extensions.Logging;
using Shared.ValueObjects;
using Users.Domain;

namespace Users.Infrastructure.Postgres;

public class UsersSeeders
{
    private const int USERS_COUNT = 50000;
    private static string[] _roles = { "USER", "ADMINISTRATOR", "DEVELOPER" };
    private static string[] _domains = { "example.com", "example.org", "example.net", "myapp.test" };

    private readonly UsersDbContext _dbContext;
    private readonly ILogger<UsersSeeders> _logger;
    private readonly Random _random = new();

    public UsersSeeders(
        UsersDbContext dbContext,
        ILogger<UsersSeeders> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedUsersBatched();
    }

    private async Task SeedUsersBatched()
    {
        _logger.LogInformation("Seeding users in batches...");

        const int batchSize = 1000;
        var firstNames = new[]
        {
            "Александр", "Елена", "Дмитрий", "Анна", "Михаил", "Ольга", "Сергей", "Наталья", "Владимир", "Татьяна"
        };
        var lastNames = new[]
        {
            "Иванов", "Петров", "Сидоров", "Козлов", "Новиков", "Морозов", "Петухов", "Обухов", "Калинин", "Лебедев"
        };

        // Отключаем отслеживание изменений для ускорения
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var users = new List<User>();
        var buskets = new List<Basket>();
        User? user = null;
        Basket? busket = null;

        for (var i = 0; i < USERS_COUNT; i++)
        {
            var firstName = firstNames[_random.Next(firstNames.Length)];
            var lastName = lastNames[_random.Next(lastNames.Length)];
            var email = RandomEmail();
            var phoneNumber = RandomPhone();
            var role = _roles[_random.Next(_roles.Length)];

            user = new User(
                new UserId(Guid.NewGuid()),
                "qwerty",
                firstName,
                lastName,
                email,
                phoneNumber,
                (Role)Enum.Parse(typeof(Role), role),
                new BasketId(Guid.NewGuid()));

            busket = new Basket(
                new BasketId(Guid.NewGuid()),
                user.Id);

            user.BasketId = busket.Id;

            users.Add(user);
            buskets.Add(busket);

            if (users.Count < batchSize)
                continue;

            _dbContext.Set<User>().AddRange(users);
            _dbContext.Set<Basket>().AddRange(buskets);
            await _dbContext.SaveChangesAsync();
            users.Clear();
            buskets.Clear();
        }

        if (users.Count != 0)
        {
            _dbContext.Set<User>().AddRange(users);
            _dbContext.Set<Basket>().AddRange(buskets);
            await _dbContext.SaveChangesAsync();
            users.Clear();
            buskets.Clear();
        }

        _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        _logger.LogInformation("Seeded {UsersCount} users.", USERS_COUNT);
    }

    static string RandomEmail(int minLen = 6, int maxLen = 12)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        int len = RandomNumberGenerator.GetInt32(minLen, maxLen + 1);

        Span<char> local = stackalloc char[len];
        for (int i = 0; i < len; i++)
            local[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];

        string domain = _domains[RandomNumberGenerator.GetInt32(_domains.Length)];
        return new string(local) + "@" + domain;
    }

    static string RandomPhone()
    {
        // +7 и дальше 10 цифр
        var digits = new char[10];
        for (int i = 0; i < 10; i++)
            digits[i] = (char)('0' + RandomNumberGenerator.GetInt32(10));

        return $"+7{new string(digits)}";
    }
}