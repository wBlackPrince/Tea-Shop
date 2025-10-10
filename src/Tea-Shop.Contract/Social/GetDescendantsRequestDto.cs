namespace Tea_Shop.Contract.Social;

public record GetDescendantsRequestDto(Guid CommentId, int Depth);