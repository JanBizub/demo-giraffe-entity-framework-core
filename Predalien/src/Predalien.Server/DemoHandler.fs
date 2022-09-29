[<RequireQualifiedAccess>]
module DemoHandler

open System
open Microsoft.AspNetCore.Http
open Giraffe
open Predalien.Database

let getArticles () =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            use db = ctx.GetService<Predalien.Database.PredalienDbContext>()

            let students () = query {
                for article in db.Articles do
                    select article
            }
            
            let result = 
                students ()

            return! json result next ctx
        }


let postArticle () =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! article = ctx.BindModelAsync<Article>()
            use db = ctx.GetService<Predalien.Database.PredalienDbContext>()

            db.Add article |> ignore
            db.SaveChanges () |> ignore

            return! json $"OK {article}" next ctx
        }
