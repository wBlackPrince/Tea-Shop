using Reviews.Contracts;
using Shared.Abstractions;

namespace Reviews.Application.Commands.DeleteReviewCommand;

public record DeleteReviewCommand(DeleteReviewDto Request): ICommand;