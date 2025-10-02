using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Seeders;

public class ProductsSeeders: ISeeder
{
    // Константы для количества данных
    private const int TAGS_COUNT = 20;
    private const int ORDERS_COUNT = 120000;
    private const int REVIEWS_COUNT = 35000;
    private const int COMMENTS_COUNT = 150000;
    private const int BUSKETS_ITEMS_COUNT = 310000;
    private const int KITS_COUNT = 50;
    private const int SUBSCRIPTIONS_COUNT = 24000;

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


    private static readonly string[] kitsNames =
    {
        "Шёлковый путь", "Английское утро", "Императорская коллекция", "Серебряные почки", "Сады Кенсингтона",
        "Нефритовый рассвет", "Карманный аристократ", "Пять королевских сортов", "Эрл Грей Монументаль",
        "Венеция Блюз", "Бабушкин сундук", "У камина", "Домик в деревне", "Тёплые вечера", "Медовое настроение",
        "Пряный русский", "Самоварные сказки", "Яблочный спас", "Лесная поляна", "Душевная беседа",
        "Шёлк и пряности", "Шепот сакуры", "Гейша", "Путь дзен", "Гималайская легенда", "Улунский дракон",
        "Сады Киото", "Бирюзовое озеро", "Мастер чайной церемонии", "Запретный город", "Летний сад",
        "Цитрусовая феерия", "Вишнёвый карнавал", "Тропический микс", "Ягодный джем", "Персиковый нектар",
        "Малиновое варенье", "Экзотик-коктейль", "Лесные дары", "Фруктовый всплеск", "Чайный путешественник",
        "Набор гурмана", "Рождественское настроение", "Поэзия чая", "Вокруг света за 5 чашек", "Золотой век",
        "Магия Востока", "Сказки Шахерезады", "Арабские ночи", "Коллекционер",
    };

    string[] teaSetDescriptions =
    {
        "Набор из шести изысканных чаев с добавлением лепестков роз и ванили. Идеален для романтического вечера.",
        "Коллекция крепких утренних чаев с мальвой и бергамотом. Помогает проснуться и зарядиться энергией.",
        "Подарочный набор в деревянной шкатулке с десятью элитными сортами чая из разных провинций Китая.",
        "Набор зеленых чаев с нежными типсами. Каждый чай обладает тонким ароматом и светло-золотистым настоем.",
        "Четыре классических английских чая в элегантной жестяной коробке с изображением лондонского сада.",
        "Ранний сбор зеленого чая с добавлением лепестков жасмина. Дарит ощущение свежести и гармонии.",
        "Миниатюрный набор из пяти популярных чаев в индивидуальных пакетиках. Удобно брать в путешествие.",
        "Отборные сорта чая, которые столетиями поставлялись к королевскому двору. Роскошь в каждой чашке.",
        "Набор с различными вариациями чая Эрл Грей - от классического до современного с цитрусовыми нотами.",
        "Эксклюзивная смесь черного чая с васильками и лавандой. Навевает мысли о венецианских каналах.",
        "Традиционные русские чаи с душистыми травами, собранными в деревенских садах. Ностальгия по детству.",
        "Набор согревающих чаев с корицей, гвоздикой и апельсином. Создан для холодных вечеров у огня.",
        "Простой и душевный набор чаев с ягодными и фруктовыми добавками. Домашний уют в каждой чашке.",
        "Пять вечерних чаев с мягким вкусом без кофеина. Помогает расслабиться после долгого дня.",
        "Сладкие чаи с натуральным медом и цветочными добавками. Напиток для хорошего настроения.",
        "Яркие смеси с имбирем, кардамоном и перцем. Напоминает о традициях русского чаепития.",
        "Набор сказочных чаев с яблоком, корицей и лесными ягодами. Для семейных вечеров с детьми.",
        "Фруктовые чаи с яблоком, грушей и летними ягодами. Вкус русского лета в любой сезон.",
        "Чайные смеси с хвоей, брусникой и дикими травами. Переносит на солнечную лесную поляну.",
        "Набор для неторопливой беседы с друзьями. Нежные улуны и ароматные травяные сборы.",
        "Восточные чаи с корицей, бадьяном и лепестками роз. Воссоздает атмосферу древнего каравана.",
        "Нежные зеленые чаи с лепестками сакуры и сливы. Японская эстетика в каждой детали.",
        "Утонченные белые чаи и легкие улуны. Изысканность и гармония для настоящих ценителей.",
        "Минималистичный набор медитативных чаев. Помогает найти внутреннее равновесие и спокойствие.",
        "Чай с высокогорных плантаций Непала и Дарджилинга. Мощный характер и насыщенный вкус.",
        "Коллекция тайваньских и китайских улунов разной степени ферментации. От нежных до терпких.",
        "Японские сенча и матча в традиционной упаковке. Точность и красота японской чайной церемонии.",
        "Набор бирюзовых улунов с цветочно-медовыми нотами. Чистота горного озера в вашей чашке.",
        "Полный комплект для чайной церемонии: чай, посуда и инструкция. Погружение в древнюю традицию.",
        "Элитные чаи, которые когда-то были доступны только императорской семье. Историческая ценность.",
        "Светлые чаи с клубникой, персиком и мятой. Напоминает о теплых днях в загородном саду.",
        "Энергичные смеси с апельсином, лимоном и лаймом. Взрыв свежести и витаминов в каждой чашке.",
        "Яркие красные чаи с вишней и шоколадом. Напоминает веселый праздник с друзьями.",
        "Экзотические сочетания с манго, маракуйей и ананасом. Путешествие по тропическим островам.",
        "Сладкие черные чаи с малиной, клубникой и черникой. Вкус любимого детского лакомства.",
        "Нежные белые чаи с персиком и абрикосом. Сочетание сладости и цветочной свежести.",
        "Традиционные русские чаи с малиной и медом. Лучшее средство от осенней хандры.",
        "Смеси с папайей, кокосом и имбирем. Для тех, кто любит экспериментировать со вкусами.",
        "Чайные сборы с черникой, брусникой и земляникой. Аромат прогулки по летнему лесу.",
        "Фруктовые инфузии с апельсином, яблоком и грушей. Бодрящий вкус для начала дня.",
        "Набор чаев из десяти разных стран мира. Географическое путешествие через вкусы и ароматы.",
        "Отборные редкие сорта для истинных ценителей. Каждый чай - это отдельная история.",
        "Новогодние смеси с мандарином, корицей и гвоздикой. Создает волшебную атмосферу праздника.",
        "Чайные композиции, вдохновленные классическими произведениями. Искусство в каждой чашке.",
        "Пять уникальных чаев с разных континентов. Возможность познакомиться с мировыми традициями.",
        "Винтажные чаи, собранные и обработанные по старинным технологиям. Возвращение в прошлое.",
        "Таинственные восточные смеси с сандалом и пачулями. Погружает в мир восточных сказок.",
        "Ароматные чаи с кардамоном, шафраном и фисташками. Напоминает истории из Тысячи и одной ночи.",
        "Ночные чаи с ванилью, миндалем и финиками. Для вечерних бесед под звездным небом.",
        "Эксклюзивный набор редких сортов, который пополняется каждый сезон. Для настоящих коллекционеров."
    };

    public ProductsSeeders(
        Products dbContext,
        ILogger<ProductsSeeders> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        //await SeedUsersBatched();
        //await SeedTagsBatched();
        //await SeedKitsBatched();
        await SeedSubscriptions();
        await SeedBusketItems();
        await SeedOrdersBatched();
        await SeedReviewsBatched();
        await SeedCommentsBatched();
    }

    private async Task SeedSubscriptions()
    {
        _logger.LogInformation("Seeding subscriptions in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var usersIds = _dbContext.UsersRead.Select(u => u.Id).ToArray();
        var kits = _dbContext.Kits.ToArray();

        const int batchSize = 1000;
        List<Subscription> subscriptions = [];

        Subscription subscription;

        for (int i = 0; i < SUBSCRIPTIONS_COUNT; i++)
        {
            subscription = new Subscription(
                new SubscriptionId(Guid.NewGuid()),
                usersIds[_random.Next(0, usersIds.Length)],
                _random.Next(1, 12),
                kits[_random.Next(0, kits.Length)]);

            subscriptions.Add(subscription);

            if (i % batchSize == 0)
            {
                _logger.LogInformation($"Saved {i} subscriptions...");
                _dbContext.Subscriptions.AddRange(subscriptions);
                await _dbContext.SaveChangesAsync();
                subscriptions.Clear();
            }
        }

        if (subscriptions.Any())
        {
            _dbContext.Subscriptions.AddRange(subscriptions);
            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedKitsBatched()
    {
        _logger.LogInformation("Seeding kits in batching...");
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        var productsIds = _dbContext.ProductsRead.Select(p => p.Id).ToArray();

        const int batchSize = 10;
        List<Kit> kits = [];

        Kit? kit = null;
        KitItem[] kitItems;
        KitId kitId;

        for (int i = 0; i < KITS_COUNT; i++)
        {
            kitItems = new KitItem[_random.Next(2, 7)];

            kitId = new KitId(Guid.NewGuid());

            for (int j = 0; j < kitItems.Length; j++)
            {
                kitItems[j] = new KitItem(
                    new KitItemId(Guid.NewGuid()),
                    kitId,
                    productsIds[_random.Next(productsIds.Length)],
                    _random.Next(2, 7));
            }

            kit = new Kit(
                kitId,
                kitsNames[_random.Next(0, kitsNames.Length)],
                _random.Next(1, 10),
                teaSetDescriptions[_random.Next(0, teaSetDescriptions.Length)],
                kitItems);

            kits.Add(kit);

            if (i % batchSize == 0)
            {
                _logger.LogInformation($"Saved {i} kits...");
                _dbContext.Kits.AddRange(kits);
                await _dbContext.SaveChangesAsync();
                kits.Clear();
            }
        }

        if (kits.Any())
        {
            _dbContext.Kits.AddRange(kits);
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

    DateTime GetRandomDate(DateTime start, DateTime end)
    {
        var range = (end - start).Days;
        return start.AddDays(_random.Next(range))
            .AddHours(_random.Next(0, 24))
            .AddMinutes(_random.Next(0, 60))
            .AddSeconds(_random.Next(0, 60));
    }
}