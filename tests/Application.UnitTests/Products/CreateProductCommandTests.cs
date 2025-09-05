using Shouldly;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
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
            new CreateIngrindientRequestDto[] { 
                new CreateIngrindientRequestDto(
                    "Ingr 1",
                    10.0f,
                    "Ingr description",
                    false)},
            "prepaation descpription",
            10,
            new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
            new Guid[] { Guid.NewGuid(), Guid.NewGuid() }));

    private readonly CreateProductHandler _handler;

    private readonly IProductsRepository _productRepositoryMock;

    private readonly ILogger<CreateProductHandler> _loggerMock;

    private readonly IValidator<CreateProductRequestDto> _validatorMock;

    public CreateProductCommandTests()
    {
        _productRepositoryMock = Substitute.For<IProductsRepository>();
        _loggerMock = Substitute.For<ILogger<CreateProductHandler>>();
        _validatorMock = Substitute.For<IValidator<CreateProductRequestDto>>();

        _handler = new CreateProductHandler(
            _productRepositoryMock,
            _loggerMock,
            _validatorMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenSeasonIsInvalid()
    {
        // arrange
        CreateProductCommand invalidCommand = Command with
        {
            Request = Command.Request with { Season = "invalid" }
        };

        _validatorMock.ValidateAsync(Arg.Any<CreateProductRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Season",
                    "Season is invalid. Enter SUMMER, AUTUMN, WINTER or SPRING")
            }));

        // act
        var result = await _handler.Handle(invalidCommand, default);

        // assert
        result.Error.ShouldBe(
            Error.Validation("product.create", "validation errors", "Season"));
    }
}