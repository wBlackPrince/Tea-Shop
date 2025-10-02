using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Commands.CreateReviewCommand;

public record CreateReviewCommand(CreateReviewRequestDto Request): ICommand;