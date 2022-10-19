[<RequireQualifiedAccess>]
module DemoHandler

open Microsoft.AspNetCore.Http
open Giraffe
open Predalien.Database

let getArticles () =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            use db = ctx.GetService<PredalienDbContext>()

            let articles () = query {
                for article in db.Articles do
                    select article
            }
            
            return! json (articles ()) next ctx
        }


let postArticle () =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! article = ctx.BindModelAsync<Article>()
            use db = ctx.GetService<PredalienDbContext>()

            db.Add article |> ignore
            db.SaveChanges () |> ignore

            return! json $"OK {article}" next ctx
        }
