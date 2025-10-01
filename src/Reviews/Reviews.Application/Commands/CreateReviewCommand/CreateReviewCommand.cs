using Reviews.Contracts;
using Shared.Abstractions;

namespace Reviews.Application.Commands.CreateReviewCommand;

public record CreateReviewCommand(CreateReviewRequestDto Request): ICommand;