using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Seeders;

public class ProductsSeeders: ISeeder
{
    // Константы для количества данных
    private const int USERS_COUNT = 50000;
    private const int PRODUCTS_COUNT = 100;
    private const int TAGS_COUNT = 20;
    private const int ORDERS_COUNT = 120000;
    private const int REVIEWS_COUNT = 35000;
    private const int COMMENTS_COUNT = 150000;
    private const int BUSKETS_ITEMS_COUNT = 310000;
    private const int KITS_COUNT = 150;
    private const int SUBSCRIPTIONS_COUNT = 24000;

    private static string[] _domains = { "example.com", "example.org", "example.net", "myapp.test" };
    private static Season[] seasons = { Season.SPRING, Season.SUMMER, Season.AUTUMN, Season.WINTER };
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

    private static readonly string[] deliveryAddresses =
    {
        "123 Main St, Springfield",
        "45 Baker St, London",
        "78 Elm Ave, New York",
        "221B Baker St, London",
        "10 Downing St, London",
        "742 Evergreen Terrace, Springfield",
        "1600 Pennsylvania Ave NW, Washington",
        "4 Privet Drive, Little Whinging",
        "99 Wall St, New York",
        "500 Fifth Ave, New York",
        "350 Fifth Ave, New York",
        "1 Infinite Loop, Cupertino",
        "1600 Amphitheatre Parkway, Mountain View",
        "11 Wall St, New York",
        "55 Rue du Faubourg Saint-Honoré, Paris",
        "5th Avenue, Manhattan, New York",
        "Kremlin, Moscow",
        "Red Square 1, Moscow",
        "Unter den Linden 77, Berlin",
        "Friedrichstrasse 43, Berlin",
        "Karl Johans gate 15, Oslo",
        "Rosenborggata 23, Oslo",
        "Nyhavn 20, Copenhagen",
        "Amalienborg Slotsplads 5, Copenhagen",
        "Piazza San Marco 1, Venice",
        "Via Condotti 10, Rome",
        "Gran Via 28, Madrid",
        "Passeig de Gracia 12, Barcelona",
        "Shibuya Crossing, Tokyo",
        "1 Chome-1-2 Oshiage, Tokyo",
    };

    private static readonly PaymentWay[] PaymentWays =
    {
        PaymentWay.CashOnDelivery,
        PaymentWay.CardOnline,
        PaymentWay.CardOnDelivery,
        PaymentWay.BankTransfer,
    };

    private static readonly OrderStatus[] OrderStatuses =
    {
        OrderStatus.Pending,
        OrderStatus.Processing,
        OrderStatus.Shipped,
        OrderStatus.Delivered,
        OrderStatus.Canceled,
    };

    private static readonly string[] ReviewTitles =
    {
        "Отличный товар",
        "Не оправдал ожиданий",
        "Лучшее соотношение цены и качества",
        "Очень доволен покупкой",
        "Не рекомендую",
        "Просто супер!",
        "Хорошее качество",
        "Средний вариант",
        "Лучше, чем ожидал",
        "Разочаровался",
    };

    private static readonly string[] ReviewTexts =
    {
        "Прекрасный продукт, всё соответствует описанию. Пользуюсь уже месяц, всё работает отлично.",
        "К сожалению, качество оказалось хуже, чем ожидал. Больше заказывать не буду.",
        "За свою цену – просто находка! Рекомендую всем.",
        "Товар пришёл быстро, хорошо упакован. Работает без нареканий.",
        "Не стоит своих денег. Ожидал гораздо большего.",
        "Очень доволен покупкой, работает отлично, а внешний вид просто супер!",
        "Хорошее качество материалов, приятно пользоваться.",
        "В целом неплохо, но есть некоторые недочёты.",
        "Превзошёл все мои ожидания. Буду брать ещё.",
        "Товар не понравился, пожалел о покупке.",
    };

    private static readonly string[] CommentTexts =
    {
        "Согласен с автором отзыва!",
        "У меня совсем другой опыт.",
        "Спасибо за полезный отзыв.",
        "Подтверждаю, у меня тоже всё отлично работает.",
        "Мне наоборот не понравилось.",
        "Очень помогло определиться с выбором.",
        "Хороший комментарий, поддерживаю.",
        "Не согласен, думаю по-другому.",
        "Да, тоже обратил внимание на это.",
        "Полезная информация, благодарю!",
    };

    private readonly ProductsDbContext _dbContext;
    private readonly ILogger<ProductsSeeders> _logger;

    private readonly Random _random = new();

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
        // await SeedKitsBatched();
        await SeedBusketItems();
        await SeedOrdersBatched();
        await SeedReviewsBatched();
        await SeedCommentsBatched();
    }

    private async Task SeedKitsBatched()
    {
        _logger.LogInformation("Seeding kits in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var usersIds = _dbContext.UsersRead.Select(u => u.Id.Value).ToArray();
        var reviewsIds = _dbContext.ReviewsRead.Select(r => r.Id.Value).ToArray();

        const int batchSize = 1000;
        List<Comment> comments = [];

        DateTime startDate = new DateTime(2023, 1, 1);
        DateTime endDate = new DateTime(2025, 9, 8);

        DateTime createdAt;
        DateTime updatedAt;

        Comment? comment = null;
        Comment? childComment = null;

        for (int i = 0; i < COMMENTS_COUNT; i++)
        {
            createdAt = GetRandomDate(startDate, endDate).ToUniversalTime();
            updatedAt = createdAt.AddDays(_random.Next(0, 25)).ToUniversalTime();

            comment = new Comment(
                new CommentId(Guid.NewGuid()),
                new UserId(usersIds[_random.Next(0, usersIds.Length)]),
                new ReviewId(reviewsIds[_random.Next(0, reviewsIds.Length)]),
                CommentTexts[_random.Next(0, CommentTexts.Length)],
                startDate.ToUniversalTime(),
                updatedAt.ToUniversalTime(),
                null);

            childComment = new Comment(
                new CommentId(Guid.NewGuid()),
                new UserId(usersIds[_random.Next(0, usersIds.Length)]),
                new ReviewId(reviewsIds[_random.Next(0, reviewsIds.Length)]),
                CommentTexts[_random.Next(0, CommentTexts.Length)],
                startDate.AddHours(_random.Next(1, 12)).ToUniversalTime(),
                updatedAt.AddHours(_random.Next(13, 24)).ToUniversalTime(),
                null);

            childComment.ParentId = childComment.Id;

            comments.Add(comment);
            comments.Add(childComment);

            if (i % batchSize == 0)
            {
                _logger.LogInformation($"Saved {i} comments...");
                _dbContext.Comments.AddRange();
                await _dbContext.SaveChangesAsync();
                comments.Clear();
            }
        }

        if (comments.Any())
        {
            _dbContext.Comments.AddRange(comments);
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedCommentsBatched()
    {
        _logger.LogInformation("Seeding comments in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var usersIds = _dbContext.UsersRead.Select(u => u.Id.Value).ToArray();
        var reviewsIds = _dbContext.ReviewsRead.Select(r => r.Id.Value).ToArray();

        const int batchSize = 1000;
        List<Comment> comments = [];

        DateTime startDate = new DateTime(2023, 1, 1);
        DateTime endDate = new DateTime(2025, 9, 8);

        DateTime createdAt;
        DateTime updatedAt;

        Comment? comment = null;
        Comment? childComment = null;

        for (int i = 0; i < COMMENTS_COUNT; i++)
        {
            createdAt = GetRandomDate(startDate, endDate).ToUniversalTime();
            updatedAt = createdAt.AddDays(_random.Next(0, 25)).ToUniversalTime();

            comment = new Comment(
                new CommentId(Guid.NewGuid()),
                new UserId(usersIds[_random.Next(0, usersIds.Length)]),
                new ReviewId(reviewsIds[_random.Next(0, reviewsIds.Length)]),
                CommentTexts[_random.Next(0, CommentTexts.Length)],
                startDate.ToUniversalTime(),
                updatedAt.ToUniversalTime(),
                null);

            childComment = new Comment(
                new CommentId(Guid.NewGuid()),
                new UserId(usersIds[_random.Next(0, usersIds.Length)]),
                new ReviewId(reviewsIds[_random.Next(0, reviewsIds.Length)]),
                CommentTexts[_random.Next(0, CommentTexts.Length)],
                startDate.AddHours(_random.Next(1, 12)).ToUniversalTime(),
                updatedAt.AddHours(_random.Next(13, 24)).ToUniversalTime(),
                null);

            childComment.ParentId = childComment.Id;

            comments.Add(comment);
            comments.Add(childComment);

            if (i % batchSize == 0)
            {
                _logger.LogInformation($"Saved {i} comments...");
                _dbContext.Comments.AddRange();
                await _dbContext.SaveChangesAsync();
                comments.Clear();
            }
        }

        if (comments.Any())
        {
            _dbContext.Comments.AddRange(comments);
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedReviewsBatched()
    {
        _logger.LogInformation("Seeding reviews in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var usersIds = _dbContext.UsersRead.Select(u => u.Id.Value).ToArray();
        var productIds = _dbContext.ProductsRead.Select(u => u.Id.Value).ToArray();

        const int batchSize = 1000;
        List<Review> reviews = [];

        DateTime startDate = new DateTime(2023, 1, 1);
        DateTime endDate = new DateTime(2025, 9, 8);

        DateTime createdAt;
        DateTime updatedAt;

        Review? review = null;

        for (int i = 0; i < REVIEWS_COUNT; i++)
        {
            createdAt = GetRandomDate(startDate, endDate).ToUniversalTime();
            updatedAt = createdAt.AddDays(_random.Next(0, 25)).ToUniversalTime();

            review = new Review(
                new ReviewId(Guid.NewGuid()),
                new ProductId(productIds[_random.Next(0, productIds.Length)]),
                new UserId(usersIds[_random.Next(0, usersIds.Length)]),
                _random.Next(1, 5),
                ReviewTitles[_random.Next(0, ReviewTitles.Length)],
                ReviewTexts[_random.Next(0, ReviewTexts.Length)],
                startDate.ToUniversalTime(),
                updatedAt.ToUniversalTime());

            reviews.Add(review);

            if (i % batchSize == 0)
            {
                _logger.LogInformation($"Saved {i} reviews...");
                _dbContext.Reviews.AddRange(reviews);
                await _dbContext.SaveChangesAsync();
                reviews.Clear();
            }
        }

        if (reviews.Any())
        {
            _dbContext.Reviews.AddRange(reviews);
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedOrdersBatched()
    {
        _logger.LogInformation("Seeding orders in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var usersIds = _dbContext.UsersRead.Select(u => u.Id.Value).ToArray();
        var productIds = _dbContext.ProductsRead.Select(u => u.Id.Value).ToArray();

        int orderCount;
        int quantity;
        Guid userId;
        Guid productId;
        string deliveryAddress;
        PaymentWay paymentWay;
        OrderStatus orderStatus;
        DateTime createdAt;
        DateTime updatedAt;
        DateTime expectedTimeDelivery;
        DateTime startDate = new DateTime(2023, 1, 1);
        DateTime endDate = new DateTime(2025, 9, 8);
        OrderItem[] orderItems;
        Order order;

        const int batchSize = 1000;
        List<Order> orders = [];

        for (int i = 0; i < ORDERS_COUNT; i++)
        {
            orderCount = _random.Next(1, 10);
            userId = usersIds[_random.Next(0, usersIds.Length)];
            deliveryAddress = deliveryAddresses[_random.Next(0, deliveryAddresses.Length)];
            paymentWay = PaymentWays[_random.Next(0, PaymentWays.Length)];
            createdAt = GetRandomDate(startDate, endDate).ToUniversalTime();
            updatedAt = createdAt.AddDays(_random.Next(0, 25)).ToUniversalTime();
            expectedTimeDelivery = updatedAt.AddDays(_random.Next(0, 25)).ToUniversalTime();
            orderItems = new OrderItem[orderCount];

            for (int j = 0; j < orderCount; j++)
            {
                productId = productIds[_random.Next(0, productIds.Length)];
                quantity = _random.Next(1, 10);

                orderItems[j] = OrderItem.Create(
                    new OrderItemId(Guid.NewGuid()),
                    new ProductId(productId),
                    quantity).Value;
            }

            order = new Order(
                new OrderId(Guid.NewGuid()),
                new UserId(userId),
                deliveryAddress,
                paymentWay,
                expectedTimeDelivery,
                orderItems,
                createdAt,
                updatedAt);

            orders.Add(order);

            if (i % batchSize == 0)
            {
                _logger.LogInformation($"Saved {i} orders...");
                _dbContext.Orders.AddRange(orders);
                await _dbContext.SaveChangesAsync();
                orders.Clear();
            }
        }

        if (orders.Any())
        {
            _dbContext.Orders.AddRange(orders);
            await _dbContext.SaveChangesAsync();
        }
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
            "Sweet",
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
            "Сладкие десертные чаи.",
        };


        // Отключаем отслеживание изменений для ускорения
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var tags = new List<Tag>();
        for (var i = 0; i < TAGS_COUNT; i++)
        {
            string tagName = tagNames[_random.Next(tagNames.Length)];
            string tagDescription = tagDescriptions[_random.Next(tagDescriptions.Length)];

            tags.Add(new Tag(
                new TagId(Guid.NewGuid()),
                tagName,
                tagDescription));

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

    private async Task SeedBusketItems()
    {
        _logger.LogInformation("Seeding busket items in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var usersIds = _dbContext.UsersRead.Select(u => u.Id.Value).ToArray();
        var productIds = _dbContext.ProductsRead.Select(u => u.Id.Value).ToArray();

        List<BasketItem> busketItems = new List<BasketItem>();

        int quantity = 0;
        UserId userId;
        ProductId productId;
        BasketId basketId;

        const int batchSize = 3000;

        for (int i = 0; i < BUSKETS_ITEMS_COUNT; i++)
        {
            quantity = _random.Next(1, 20);
            userId = new UserId(usersIds[_random.Next(usersIds.Length)]);
            basketId = _dbContext.UsersRead.First(u => u.Id == userId).BasketId;
            productId = new ProductId(productIds[_random.Next(productIds.Length)]);

            busketItems.Add(new BasketItem(
                new BasketItemId(Guid.NewGuid()),
                basketId,
                productId,
                quantity));

            if (i % batchSize == 0)
            {
                _logger.LogInformation($"Saved {i} busket items...");
                _dbContext.BusketsItems.AddRange(busketItems);
                await _dbContext.SaveChangesAsync();
                busketItems.Clear();
            }
        }

        if (busketItems.Any())
        {
            _dbContext.BusketsItems.AddRange(busketItems);
            await _dbContext.SaveChangesAsync();
        }
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

    DateTime GetRandomDate(DateTime start, DateTime end)
    {
        var range = (end - start).Days;
        return start.AddDays(_random.Next(range))
            .AddHours(_random.Next(0, 24))
            .AddMinutes(_random.Next(0, 60))
            .AddSeconds(_random.Next(0, 60));
    }
}