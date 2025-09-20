using Shouldly;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Contract.Products;
using Tea_Shop.Shared;

namespace Application.UnitTests.Products;

public class CreateProductCommandTests
{
    private static readonly CreateProductCommand Command = new CreateProductCommand(
        new CreateProductRequestDto(
            "Test Tea",
            100,
            200,
            1000,
            "Test Description",
            "SUMMER",
            new CreateIngrindientRequestDto[]
            {
                new CreateIngrindientRequestDto(
                    "Ingr 1",
                    10.0f,
                    "Ingr description",
                    false),
            },
            "prepaation descpription",
            10,
            new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
            new Guid[] { Guid.NewGuid(), Guid.NewGuid() }));

    private readonly CreateProductHandler _handler;

    private readonly IProductsRepository _productRepositoryMock;

    private readonly ILogger<CreateProductHandler> _loggerMock;

    private readonly IValidator<CreateProductRequestDto> _validatorMock;

    private readonly ITransactionManager _transactionManagerMock;

    public CreateProductCommandTests()
    {
        _productRepositoryMock = Substitute.For<IProductsRepository>();
        _loggerMock = Substitute.For<ILogger<CreateProductHandler>>();
        _validatorMock = Substitute.For<IValidator<CreateProductRequestDto>>();
        _transactionManagerMock = Substitute.For<ITransactionManager>();

        _handler = new CreateProductHandler(
            _productRepositoryMock,
            _loggerMock,
            _validatorMock,
            _transactionManagerMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenSeasonIsInvalid()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with
            {
                Season = "invalid",
            },
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Season",
                    "Season is invalid. Enter SUMMER, AUTUMN, WINTER or SPRING"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation("product.create", "validation errors", "Season"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenTitleIsEmpty()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with
            {
                Title = string.Empty,
            },
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Title",
                    "Title is required"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation(
                "product.create",
                "validation errors",
                "Title"));
    }
    
    [Fact]
    public async Task Handle_Should_ReturnError_WhenTitleIsTooLong()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with
            {
                Title = new string('X', Constants.Limit100 + 1),
            },
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Title",
                    "Title must not exceed 100 characters"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation(
                "product.create",
                "validation errors",
                "Title"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenTitleIsTooShort()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with
            {
                Title = new string('X', Constants.Limit2 - 1),
            },
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Title",
                    "Title must contain at least 2 character"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation(
                "product.create",
                "validation errors",
                "Title"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenDescriptionIsEmpty()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with
            {
                Description = string.Empty,
            },
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Description",
                    "Description is required"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation(
                "product.create",
                "validation errors",
                "Description"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenDescriptionIsTooLong()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with
            {
                Description = new string('X', Constants.Limit2000 + 1),
            },
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Description",
                    "Description must not exceed 2000 characters"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation(
                "product.create",
                "validation errors",
                "Description"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenDescriptionIsTooShort()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with
            {
                Title = new string('X', Constants.Limit2 - 1),
            },
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Description",
                    "Description must contain at least 2 character"),
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation(
                "product.create",
                "validation errors",
                "Description"));
    }
}