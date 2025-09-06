using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Reviews.Commands.DeleteReviewCommand;

public record DeleteReviewCommand(DeleteReviewDto Request): ICommand;