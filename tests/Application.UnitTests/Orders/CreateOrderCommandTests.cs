using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Orders;
using Tea_Shop.Application.Orders.Commands.CreateOrderCommand;
using Tea_Shop.Application.Users;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Shared;


namespace Application.UnitTests.Orders;

public class CreateOrderCommandTests
{
    private static readonly OrderItemRequestDto[] Items =
    {
        new OrderItemRequestDto(Guid.NewGuid(), 5),
        new OrderItemRequestDto(Guid.NewGuid(), 7),
        new OrderItemRequestDto(Guid.NewGuid(), 8),
    };

    private static readonly CreateOrderCommand Command = new CreateOrderCommand(
        new CreateOrderRequestDto(
            Guid.NewGuid(),
            "Cheboksary, Sovchoznya",
            "CardOnline",
            "Pickup",
            10,
            Items));

    private readonly CreateOrderHandler _handler;

    private readonly IOrdersRepository _ordersRepositoryMock;

    private readonly IUsersRepository _usersRepositoryMock;

    private readonly IReadDbContext _readDbContextMock;

    private readonly ILogger<CreateOrderHandler> _loggerMock;

    private readonly IValidator<CreateOrderRequestDto> _validatorMock;

    private ITransactionManager _transactionManager;

    public CreateOrderCommandTests()
    {
        _ordersRepositoryMock = Substitute.For<IOrdersRepository>();
        _usersRepositoryMock = Substitute.For<IUsersRepository>();
        _readDbContextMock = Substitute.For<IReadDbContext>();
        _loggerMock = Substitute.For<ILogger<CreateOrderHandler>>();
        _validatorMock = Substitute.For<IValidator<CreateOrderRequestDto>>();
        _transactionManager = Substitute.For<ITransactionManager>();

        _handler = new CreateOrderHandler(
            _ordersRepositoryMock,
            _usersRepositoryMock,
            _readDbContextMock,
            _loggerMock,
            _validatorMock,
            _transactionManager);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenDeliveryAddressIsEmpty()
    {
        // arrange
        CreateOrderCommand invalidCommand = Command with
        {
            Request = Command.Request with { DeliveryAddress = string.Empty, },
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateOrderRequestDto>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "DeliveryAddress",
                    "Delivery address is required"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        result.Error.ShouldBe(Error.Validation(
            "order.create",
            "Validation failed Delivery address is required",
            "DeliveryAddress"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenPaymentMethodIsInvalid()
    {
        // arrange
        CreateOrderCommand invalidCommand = Command with
        {
            Request = Command.Request with { PaymentMethod = "invalid", },
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateOrderRequestDto>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "PaymentMethod",
                    "Payment method is invalid"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        result.Error.ShouldBe(Error.Validation(
            "order.create",
            "Validation failed Payment method is invalid",
            "PaymentMethod"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenDeliveryWayIsInvalid()
    {
        // arrange
        CreateOrderCommand invalidCommand = Command with
        {
            Request = Command.Request with { DeliveryWay = "invalid", },
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateOrderRequestDto>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "DeliveryWay",
                    "Delivery way is invalid"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        result.Error.ShouldBe(Error.Validation(
            "order.create",
            "Validation failed Delivery way is invalid",
            "DeliveryWay"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenUsedBonusesBelowZero()
    {
        // arrange
        CreateOrderCommand invalidCommand = Command with
        {
            Request = Command.Request with { UsedBonuses = -1, },
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateOrderRequestDto>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "UsedBonuses",
                    "Used bonus count must be greater than 0"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        result.Error.ShouldBe(Error.Validation(
            "order.create",
            "Validation failed Used bonus count must be greater than 0",
            "UsedBonuses"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenOrderItemsQuantityEqualsOrBelowZero() {
        // arrange
        OrderItemRequestDto[] invalidItems = new OrderItemRequestDto[3];
        Array.Copy(Items, invalidItems, Items.Length);

        CreateOrderCommand invalidCommand = Command with
        {
            Request = Command.Request with { Items = invalidItems, },
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateOrderRequestDto>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Items",
                    "Quantity of order item must be greater than zero"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        result.Error.ShouldBe(Error.Validation(
            "order.create",
            "Validation failed Quantity of order item must be greater than zero",
            "Items"));
    }
}