using Microsoft.Extensions.Logging;
using Products.Domain;
using Shared.ValueObjects;

namespace Products.Infrastructure.Postgres;

public class ProductsSeeders
{
    private const int PRODUCTS_COUNT = 100;

    private static Season[] seasons = { Season.SPRING, Season.SUMMER, Season.AUTUMN, Season.WINTER };

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
        "Tea oil",
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
        "Масло чая для усиления аромата.",
    };

    private static readonly string[] preparationDescriptions =
    {
        "Заваривать при 80°C в течение 2 минут.",
        "Залить кипятком 90–95°C, настоять 3 минуты.",
        "Заваривать 4 минуты при 95°C.",
        "Кипятить с молоком и специями 5 минут.",
        "Медленно заваривать в гайвани, 7 проливов по 20 секунд.",
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
        "Coconut Black",
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
        "Чёрный чай с кокосом.",
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
        0.2f,  // Tea oil
    };

    private static readonly float[] ProductPrices =
    {
        5.5f, 6.0f, 7.0f, 6.5f, 5.8f,
        6.2f, 9.5f, 12.0f, 6.8f, 8.0f,
        8.5f, 9.0f, 7.5f, 10.5f, 8.0f,
        8.5f, 6.5f, 7.0f, 6.8f, 5.9f,
        5.5f, 5.7f, 6.3f, 6.6f, 6.9f,
        7.2f, 7.4f, 8.1f, 9.0f, 6.8f,
    };

    private static readonly float[] ProductAmounts =
    {
        50, 60, 40, 70, 80,
        65, 30, 25, 55, 60,
        45, 40, 50, 35, 70,
        65, 75, 50, 60, 55,
        80, 70, 65, 55, 60,
        75, 50, 45, 40, 55,
    };

    private readonly ProductsDbContext _dbContext;
    private readonly ILogger<ProductsSeeders> _logger;

    private readonly Random _random = new();

    public ProductsSeeders(
        ProductsDbContext dbContext,
        ILogger<ProductsSeeders> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedProductsBatched();
    }

    private async Task SeedProductsBatched()
    {
        _logger.LogInformation("Seeding products in batches...");

        Guid [] TagsIds = _dbContext.Tags.Select(x => x.Id.Value).ToArray();

        const int batchSize = 15;

        // Отключаем отслеживание изменений для ускорения
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var products = new List<Product>();
        for (var i = 0; i < PRODUCTS_COUNT; i++)
        {
            var title = productTitles[_random.Next(productTitles.Length)];
            var description = productDescriptions[_random.Next(productDescriptions.Length)];
            var price = ProductPrices[_random.Next(ProductPrices.Length)];
            var amount = ProductAmounts[_random.Next(ProductAmounts.Length)];
            var stockQuantity = _random.Next(100, 1000);
            var season = seasons[_random.Next(seasons.Length)];

            Ingrendient[] ingrendients = new Ingrendient[_random.Next(2, 5)];
            for (var j = 0; j < ingrendients.Length; j++)
            {
                ingrendients[j] = new Ingrendient(
                    ingredientAmounts[_random.Next(ingredientAmounts.Length)],
                    ingredientsNames[_random.Next(ingredientsNames.Length)],
                    ingredientDescriptions[_random.Next(ingredientDescriptions.Length)],
                    _random.Next(2) == 0
                );
            }

            string preparationDescription = preparationDescriptions[_random.Next(preparationDescriptions.Length)];
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
}