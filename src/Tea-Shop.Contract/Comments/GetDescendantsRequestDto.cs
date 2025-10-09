namespace Tea_Shop.Contract.Comments;

public record GetDescendantsRequestDto(Guid CommentId, int Depth);