using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Commands.DeleteReviewCommand;

public record DeleteReviewCommand(DeleteReviewDto Request): ICommand;