using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract;

namespace Tea_Shop.Application.Social.Commands.UpdateReviewCommand;

public record UpdateReviewCommand(Guid ReviewId, UpdateEntityRequestDto Request): ICommand;