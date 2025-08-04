using FinShark.DataAccess.Models;
using WebApi.DTOs;

namespace WebApi.Mappers;

public static class CommentMapper
{
    public static CommentRead? ToCommentRead(this Comment comment)
    {
        if (comment is null) return null;

        var commentRead = new CommentRead
        {
            Id = comment.Id,
            Title = comment.Title,
            Content = comment.Content,
            Created = comment.Created,
            StockId = comment.StockId
        };

        return commentRead;
    }

    public static ICollection<CommentRead> ToCommentReadCollection(this IEnumerable<Comment> comments)
    {
        if (comments is null || !comments.Any())
            return new List<CommentRead>();

        var commentReadCollection = comments.Select(comment => comment.ToCommentRead()).ToList();

        return commentReadCollection;
    }

    public static Comment? ToComment(this CommentCreate commentCreate, int stockId)
    {
        if (commentCreate is null) return null;

        var comment = new Comment
        {
            Title = commentCreate.Title,
            Content = commentCreate.Content,
            StockId = stockId
        };

        return comment;
    }
    public static Comment? ToComment(this CommentUpdate commentDTO)
    {
        if (commentDTO is null) return null;

        var comment = new Comment
        {
            Title = commentDTO.Title,
            Content = commentDTO.Content
        };

        return comment;
    }

    public static bool Update(this Comment comment, CommentUpdate dto)
    {
        if (comment is null || dto is null) return false;

        comment.Title = dto.Title;
        comment.Content = dto.Content;

        return true;
    }
}
