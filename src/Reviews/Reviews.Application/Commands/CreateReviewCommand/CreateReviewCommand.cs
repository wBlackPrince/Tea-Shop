using Reviews.Contracts;
using Reviews.Contracts.Dtos;
using Shared.Abstractions;

namespace Reviews.Application.Commands.CreateReviewCommand;

public record CreateReviewCommand(CreateReviewRequestDto Request): ICommand;