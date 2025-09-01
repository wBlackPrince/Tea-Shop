using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Seeders;

public class ProductsSeeders: ISeeder
{
    private readonly ProductsDbContext _dbContext;
    private readonly ILogger<ProductsSeeders> _logger;

    private readonly Random _random = new();

    // Константы для количества данных
    private const int USERS_COUNT = 10000;
    private const int PRODUCTS_COUNT = 30;
    private const int TAGS_COUNT = 20;
    private const int ORDERS_COUNT = 70000;

    private static string[] _domains = { "example.com", "example.org", "example.net", "myapp.test" };
    private static Season[] seasons = { Season.SPRING , Season.SUMMER, Season.AUTUMN, Season.WINTER };
    private static string[] _roles = { "USER", "ADMINISTRATOR", "DEVELOPER" };
    private static string[] ingredientsNames =
    {
        // Чайные основы
        "Black tea",
        "Green tea",
        "Oolong tea",
        "White tea",
        "Pu-erh",
        "Rooibos",
        "Herbal base",

        // Травы и листья
        "Peppermint",
        "Lemon balm",
        "Lavender",
        "Lemongrass",
        "Currant leaf",
        "Raspberry leaf",
        "Sage",

        // Цитрусовые
        "Lemon peel",
        "Orange peel",
        "Bergamot oil",
        "Lime peel",

        // Фрукты и ягоды
        "Apple",
        "Pear",
        "Mango",
        "Lychee",
        "Peach",
        "Pineapple",
        "Coconut",
        "Blueberry",
        "Raspberry",
        "Strawberry",
        "Blackberry",
        "Black currant",
        "Lingonberry",
        "Cranberry",
        "Hibiscus",

        // Орехи / семена
        "Almond",
        "Cardamom seeds",

        // Пряности
        "Cardamom",
        "Cinnamon",
        "Clove",
        "Ginger",
        "Turmeric",
        "Black pepper",
        "Anise",
        "Star anise",
        "Fennel",
        "Vanilla",

        // Цветы
        "Jasmine",
        "Chamomile",
        "Rose",
        "Calendula",
        "Cornflower",

        // Дополнительно
        "Roasted rice",
        "Honey granules",
        "Cocoa nibs",
        "Tea oil"
    };

    private static string[] ingredientDescriptions =
    {
        // Чайные основы
        "Классическая основа с насыщенным вкусом.",
        "Свежий и лёгкий вкус зелёного чая.",
        "Полуферментированный чай с богатым ароматом.",
        "Мягкий вкус белого чая с нежными нотами.",
        "Ферментированный чай с глубоким вкусом.",
        "Южноафриканский травяной напиток без кофеина.",
        "Смесь трав и растений для натуральных чаёв.",

        // Травы и листья
        "Освежающий вкус мяты, охлаждающий эффект.",
        "Мягкий лимонный аромат, расслабляющий эффект.",
        "Ароматная трава с лёгкими цветочными нотами.",
        "Травяной ингредиент с цитрусовым ароматом.",
        "Листья смородины с фруктовым послевкусием.",
        "Листья малины с мягким сладковатым вкусом.",
        "Травяная добавка с пряным ароматом.",

        // Цитрусовые
        "Сухая лимонная цедра, придающая кислинку.",
        "Апельсиновая цедра с сладким ароматом.",
        "Эфирное масло бергамота с цитрусовыми нотами.",
        "Цедра лайма с освежающим вкусом.",

        // Фрукты и ягоды
        "Сушёные яблоки с лёгкой сладостью.",
        "Сушёные груши с нежным вкусом.",
        "Сочные кусочки манго для тропического акцента.",
        "Экзотический фрукт с ярким ароматом.",
        "Сушёные персики с мягким вкусом.",
        "Сладкие кусочки ананаса.",
        "Стружка кокоса с нежным ароматом.",
        "Сушёная черника с насыщенным вкусом.",
        "Яркий вкус малины.",
        "Сладкая клубника, дающая аромат.",
        "Сушёная ежевика с терпким вкусом.",
        "Насыщенная чёрная смородина.",
        "Кисловатая брусника.",
        "Яркая клюква с кислинкой.",
        "Кисло-сладкий вкус гибискуса.",

        // Орехи / семена
        "Миндаль с мягким ореховым вкусом.",
        "Кардамоновые семена с пряным ароматом.",

        // Пряности
        "Пряный и ароматный ингредиент.",
        "Сладкая пряность с характерным запахом.",
        "Гвоздика с острым пряным вкусом.",
        "Имбирь с согревающим вкусом.",
        "Куркума с ярким золотым цветом.",
        "Острый вкус чёрного перца.",
        "Ароматный анис с лёгкой сладостью.",
        "Звёздчатый анис с насыщенным вкусом.",
        "Фенхель с мягким сладким вкусом.",
        "Нежная ваниль с тёплым ароматом.",

        // Цветы
        "Жасмин с нежным цветочным ароматом.",
        "Ромашка с мягким успокаивающим вкусом.",
        "Лепестки розы с цветочным ароматом.",
        "Календула с лёгким травяным вкусом.",
        "Василёк, добавляющий декоративности.",

        // Дополнительно
        "Обжаренный рис с ореховым вкусом.",
        "Медовые гранулы с натуральной сладостью.",
        "Какао-крошка с шоколадным вкусом.",
        "Масло чая для усиления аромата."
    };

    private static readonly string[] preparationDescriptions =
    {
        "Заваривать при 80°C в течение 2 минут.",
        "Залить кипятком 90–95°C, настоять 3 минуты.",
        "Заваривать 4 минуты при 95°C.",
        "Кипятить с молоком и специями 5 минут.",
        "Медленно заваривать в гайвани, 7 проливов по 20 секунд."
    };

    private static readonly string[] productTitles =
    {
        "Assam Breakfast",
        "Earl Grey",
        "Darjeeling First Flush",
        "Jasmine Green",
        "Sencha",
        "Genmaicha",
        "Matcha Classic",
        "Gyokuro",
        "Gunpowder Mint",
        "Tieguanyin Oolong",
        "Milk Oolong",
        "Dong Ding Oolong",
        "White Peony",
        "Silver Needle",
        "Pu-erh Ripe",
        "Pu-erh Raw",
        "Masala Chai",
        "Turmeric Ginger Blend",
        "Lemon Ginger",
        "Hibiscus Berry",
        "Chamomile",
        "Peppermint",
        "Rooibos Vanilla",
        "Citrus Earl Grey",
        "Spiced Apple Black",
        "Mango Green",
        "Lychee Black",
        "Blueberry Oolong",
        "Jasmine Pearls",
        "Coconut Black"
    };

    private static readonly string[] productDescriptions =
    {
        "Классический индийский чёрный чай для бодрого утра.",
        "Чёрный чай с ароматом бергамота.",
        "Лёгкий и ароматный весенний дарджилинг.",
        "Зелёный чай, ароматизированный жасмином.",
        "Японский зелёный чай с травянистым вкусом.",
        "Зелёный чай с жареным рисом.",
        "Традиционный японский порошковый зелёный чай.",
        "Элитный японский зелёный чай с насыщенным вкусом.",
        "Зелёный чай с мятой для свежести.",
        "Китайский улун с цветочными нотами.",
        "Мягкий молочный улун.",
        "Тайваньский улун с глубоким вкусом.",
        "Белый чай с лёгким ароматом.",
        "Элитный белый чай из почек.",
        "Выдержанный пуэр с землянистым вкусом.",
        "Свежий шэн пуэр с фруктовыми нотами.",
        "Пряный индийский чай с молоком и специями.",
        "Пряная смесь куркумы и имбиря.",
        "Освежающая смесь лимона и имбиря.",
        "Гибискус с ягодами и кислинкой.",
        "Успокаивающий травяной чай из ромашки.",
        "Мятный травяной чай с охлаждающим вкусом.",
        "Ройбуш с натуральной ванилью.",
        "Чёрный чай с цитрусовыми нотами.",
        "Чёрный чай с яблоком и специями.",
        "Зелёный чай с кусочками манго.",
        "Чёрный чай с ароматом личи.",
        "Улун с черникой.",
        "Жасминовый зелёный чай в форме жемчужин.",
        "Чёрный чай с кокосом."
    };

    private static readonly float[] productPrices =
    {
        5.5f, 6.0f, 7.0f, 6.5f, 5.8f,
        6.2f, 9.5f, 12.0f, 6.8f, 8.0f,
        8.5f, 9.0f, 7.5f, 10.5f, 8.0f,
        8.5f, 6.5f, 7.0f, 6.8f, 5.9f,
        5.5f, 5.7f, 6.3f, 6.6f, 6.9f,
        7.2f, 7.4f, 8.1f, 9.0f, 6.8f
    };

    private static readonly float[] productAmounts =
    {
        50, 60, 40, 70, 80,
        65, 30, 25, 55, 60,
        45, 40, 50, 35, 70,
        65, 75, 50, 60, 55,
        80, 70, 65, 55, 60,
        75, 50, 45, 40, 55
    };

    private static float[] ingredientAmounts =
    {
        // Чайные основы
        3.0f, // Black tea
        2.5f, // Green tea
        3.5f, // Oolong tea
        2.0f, // White tea
        4.0f, // Pu-erh
        3.0f, // Rooibos
        2.5f, // Herbal base

        // Травы и листья
        0.5f, // Peppermint
        0.4f, // Lemon balm
        0.3f, // Lavender
        0.5f, // Lemongrass
        0.6f, // Currant leaf
        0.6f, // Raspberry leaf
        0.4f, // Sage

        // Цитрусовые
        0.3f, // Lemon peel
        0.4f, // Orange peel
        0.2f, // Bergamot oil
        0.3f, // Lime peel

        // Фрукты и ягоды
        0.8f, // Apple
        0.7f, // Pear
        0.6f, // Mango
        0.5f, // Lychee
        0.6f, // Peach
        0.7f, // Pineapple
        0.5f, // Coconut
        0.5f, // Blueberry
        0.4f, // Raspberry
        0.6f, // Strawberry
        0.6f, // Blackberry
        0.5f, // Black currant
        0.4f, // Lingonberry
        0.5f, // Cranberry
        0.8f, // Hibiscus

        // Орехи / семена
        0.4f, // Almond
        0.2f, // Cardamom seeds

        // Пряности
        0.2f, // Cardamom
        0.3f, // Cinnamon
        0.1f, // Clove
        0.3f, // Ginger
        0.3f, // Turmeric
        0.1f, // Black pepper
        0.2f, // Anise
        0.3f, // Star anise
        0.3f, // Fennel
        0.2f, // Vanilla

        // Цветы
        0.3f, // Jasmine
        0.4f, // Chamomile
        0.3f, // Rose
        0.4f, // Calendula
        0.2f, // Cornflower

        // Дополнительно
        1.0f, // Roasted rice
        0.5f, // Honey granules
        0.6f, // Cocoa nibs
        0.2f  // Tea oil
    };


    public ProductsSeeders(
        ProductsDbContext dbContext,
        ILogger<ProductsSeeders> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedUsersBatched();
        await SeedTagsBatched();
        await SeedProductsBatched();
    }

    private async Task SeedOrdersBatched()
    {
        // string tagName = tagNames[_random.Next(tagNames.Length)];
        _logger.LogInformation("Seeding orders in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

        // for (int i = 0; i < ORDERS_COUNT; i++)
        // {
        //     var user = _dbContext.Users.Select().
        // }
    }

    private async Task SeedTagsBatched()
    {
        _logger.LogInformation("Seeding tags in batches...");

        const int batchSize = 100;

        string[] tagNames =
        {
            "Black",
            "Green",
            "White",
            "Oolong",
            "Pu-erh",
            "Herbal",
            "Rooibos",
            "Spiced",
            "Fruity",
            "Floral",
            "Citrus",
            "Berry",
            "Mint",
            "Organic",
            "Premium",
            "Decaf",
            "Seasonal",
            "Wellness",
            "Classic",
            "Sweet"
        };

        string[] tagDescriptions =
        {
            "Чёрные чаи с насыщенным вкусом и ароматом.",
            "Зелёные чаи со свежим травянистым вкусом.",
            "Белые чаи с мягким и нежным вкусом.",
            "Улуны с богатым цветочным и молочным ароматом.",
            "Пуэры с выдержанным и глубоким вкусом.",
            "Травяные смеси без кофеина.",
            "Ройбуш — южноафриканский чай без кофеина.",
            "Пряные чаи с добавлением специй.",
            "Чаи с фруктовыми нотами.",
            "Чаи с добавлением лепестков цветов.",
            "Чаи с цитрусовыми ингредиентами.",
            "Чаи с ягодными добавками.",
            "Чаи с мятой для свежести.",
            "Органические чаи без химии и ароматизаторов.",
            "Премиальные сорта и редкие коллекции.",
            "Чаи без кофеина.",
            "Сезонные смеси для зимы, весны, лета или осени.",
            "Чаи для здоровья и расслабления.",
            "Классические вкусы для ежедневного чаепития.",
            "Сладкие десертные чаи."
        };


        // Отключаем отслеживание изменений для ускорения
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var tags = new List<Tag>();
        for (var i = 0; i < TAGS_COUNT; i++)
        {
            string tagName = tagNames[_random.Next(tagNames.Length)];
            string tagDescription = tagDescriptions[_random.Next(tagDescriptions.Length)];

            tags.Add(new Tag
            (
                new TagId(Guid.NewGuid()),
                tagName,
                tagDescription
            ));

            if (tags.Count < batchSize)
                continue;

            _dbContext.Set<Tag>().AddRange(tags);
            await _dbContext.SaveChangesAsync();
            tags.Clear();
        }

        if (tags.Count != 0)
        {
            _dbContext.Set<Tag>().AddRange(tags);
            await _dbContext.SaveChangesAsync();
        }

        _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        _logger.LogInformation("Seeded {TagsCount} users.", TAGS_COUNT);
    }

    private async Task SeedProductsBatched()
    {
        _logger.LogInformation("Seeding products in batches...");

        Guid [] TagsIds = _dbContext.Tags.Select(x => x.Id.Value).ToArray();

        const int batchSize = 15;

        // Отключаем отслеживание изменений для ускорения
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var products = new List<Product>();
        for (var i = 0; i < USERS_COUNT; i++)
        {
            var title = productTitles[_random.Next(productTitles.Length)];
            var description = productDescriptions[_random.Next(productDescriptions.Length)];
            var price = productPrices[_random.Next(productPrices.Length)];
            var amount = productAmounts[_random.Next(productAmounts.Length)];
            var stockQuantity = _random.Next(100, 1000);
            var season = seasons[_random.Next(seasons.Length)];

            Ingrendient[] ingrendients = new Ingrendient[_random.Next(2, 5)];
            for (var j = 0; j < ingrendients.Length; j++)
            {
                ingrendients[j] = new Ingrendient(
                    ingredientAmounts[_random.Next(ingredientAmounts.Length)],
                    ingredientsNames[_random.Next(ingrendients.Length)],
                    ingredientDescriptions[_random.Next(ingredientDescriptions.Length)],
                    _random.Next(2) == 0
                );
            }

            string preparationDescription = preparationDescriptions[_random.Next(_roles.Length)];
            int preparationTime = _random.Next(10, 30);

            Guid[] photosIds = new Guid[_random.Next(2, 5)];
            for (var j = 0; j < photosIds.Length; j++)
            {
                photosIds[j] = Guid.NewGuid();
            }

            Guid[] tagsIds = new Guid[_random.Next(2, 8)];
            for (var j = 0; j < tagsIds.Length; j++)
            {
                tagsIds[j] = TagsIds[_random.Next(tagsIds.Length)];
            }


            products.Add(new Product
            (
                new ProductId(Guid.NewGuid()),
                title,
                description,
                price,
                amount,
                stockQuantity,
                season,
                ingrendients,
                tagsIds,
                preparationDescription,
                preparationTime,
                photosIds
            ));

            if (products.Count < batchSize)
                continue;

            _dbContext.Set<Product>().AddRange(products);
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            products.Clear();
        }

        if (products.Count != 0)
        {
            _dbContext.Set<Product>().AddRange(products);
            await _dbContext.SaveChangesAsync();
        }

        _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        _logger.LogInformation("Seeded {ProductCount} product.", PRODUCTS_COUNT);
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
        for (var i = 0; i < USERS_COUNT; i++)
        {
            var firstName = firstNames[_random.Next(firstNames.Length)];
            var lastName = lastNames[_random.Next(lastNames.Length)];
            var email = RandomEmail();
            var phoneNumber = RandomPhone();
            var role = _roles[_random.Next(_roles.Length)];

            users.Add(new User
            (
                new UserId(Guid.NewGuid()),
                "qwerty",
                firstName,
                lastName,
                email,
                phoneNumber,
                (Role)Enum.Parse(typeof(Role), role)
            ));

            if (users.Count < batchSize)
                continue;

            _dbContext.Set<User>().AddRange(users);
            await _dbContext.SaveChangesAsync();
            users.Clear();
        }

        if (users.Count != 0)
        {
            _dbContext.Set<User>().AddRange(users);
            await _dbContext.SaveChangesAsync();
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