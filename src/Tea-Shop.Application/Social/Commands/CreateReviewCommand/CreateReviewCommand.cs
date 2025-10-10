using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Social;

namespace Tea_Shop.Application.Social.Commands.CreateReviewCommand;

public record CreateReviewCommand(CreateReviewRequestDto Request): ICommand;