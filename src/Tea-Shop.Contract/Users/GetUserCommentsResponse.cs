using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Contract.Users;

public record GetUserCommentsResponse(CommentDto[] Commnets, int TotalCount);