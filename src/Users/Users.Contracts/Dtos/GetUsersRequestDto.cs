using Shared.Dto;

namespace Users.Contracts.Dtos;

public record GetUsersRequestDto
{
    public string? SearchFirstName { get; init; }

    public string? SearchLastName { get; init; }

    public string? SearchMiddleName { get; init; }

    public string? Role { get; init; }

    public bool? IsActive { get; init; }

    public PaginationRequest Pagination { get; init; }
}