using Reviews.Contracts;
using Reviews.Contracts.Dtos;
using Shared.Abstractions;

namespace Reviews.Application.Commands.DeleteReviewCommand;

public record DeleteReviewCommand(DeleteReviewDto Request): ICommand;