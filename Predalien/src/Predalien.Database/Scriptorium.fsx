#r "nuget: Microsoft.EntityFrameworkCore"
#r "nuget: Microsoft.EntityFrameworkCore.SqlServer"

#r "bin/Debug/net6.0/Predalien.Database.dll"

open Predalien.Database
open System

let db = new PredalienDbContext()

let getArticles () = 
    query {
        for article in db.Articles do
        select article
    }
    |> List.ofSeq

let getComments () =
    query {
        for comment in db.Comments do
        select comment
    }
    |> List.ofSeq

let addArticle (article: Article) = 
    article |> db.Add |> ignore
    db.SaveChanges()

let addComment (comment: Comment) =
    comment |> db.Add |> ignore
    db.SaveChanges()

let attachCommentToArticle (articleId: Guid) (comment: Comment) =
    comment.ArticleId <- articleId
    db.Update <| comment |> ignore
    db.SaveChanges()


// => Play

let articleAboutGardetning = Article(Name = "Article About Gardetning", Content = "Gardening is about gardening.")

let okComment = Comment(Text = "Yes I like Gardening")
let noComment = Comment(Text = "Gardening boring")



articleAboutGardetning 
|> addArticle

getArticles ()

let attachCommnetToArticleAboutGardening comment = 
    attachCommentToArticle 
        (Guid("4d648a62-85e6-429b-f6eb-08daa21772b1"))
        comment

attachCommnetToArticleAboutGardening okComment
attachCommnetToArticleAboutGardening noComment

getComments ()

System.Guid.NewGuid()