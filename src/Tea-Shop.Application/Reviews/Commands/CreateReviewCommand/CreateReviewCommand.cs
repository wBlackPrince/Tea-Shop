using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Reviews.Commands.CreateReviewCommand;

public record CreateReviewCommand(CreateReviewRequestDto Request): ICommand;