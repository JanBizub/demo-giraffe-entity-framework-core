namespace rec Predalien.Domain

open System

type ArticleR =
    {
    ArticleId : Guid
    Name      : string
    Content   : string
    Comments  : CommentR list
    }

type CommentR =
    {
    CommentId: Guid
    Text : string
    }