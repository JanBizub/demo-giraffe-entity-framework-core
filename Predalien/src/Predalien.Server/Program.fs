module Server.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.Extensions.Configuration
open Predalien.Database

let webApp =
    choose [ 
        GET
        >=> route "/api/test"
        >=> DemoHandler.getArticles ()
        
        POST
        >=> route "/api/postarticle"
        >=> DemoHandler.postArticle ()
    ]


/// Ignore the passed value. This is often used to throw away results of a computation.
let (!) f = f |> ignore

let configureServices (context: WebHostBuilderContext) (services: IServiceCollection) =
    let configuration: IConfiguration =
        ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile(sprintf "appsettings.%s.json" context.HostingEnvironment.EnvironmentName, true)
            .Build()
        :> IConfiguration

    !(services.AddCors (fun options ->
        options.AddPolicy(
            name = "default",
            configurePolicy =
                fun policyBuilder ->
                    ! policyBuilder.AllowAnyOrigin().AllowAnyMethod()
                        .AllowAnyHeader()

        )))

    ! services.AddTransient<PredalienDbContext>()

    ! services.AddGiraffe()


let configureApp (app: IApplicationBuilder) =
    let env =
        app.ApplicationServices.GetService<IWebHostEnvironment>()

    (match env.IsDevelopment() with
     | true -> app.UseDeveloperExceptionPage()
     | false ->
         app
             .UseGiraffeErrorHandler(Handler.errorHandler)
             .UseHttpsRedirection())
        .UseCors("default")
        .UseStaticFiles()
        .UseGiraffe(webApp)


let configureLogging (builder: ILoggingBuilder) = ! builder.AddConsole().AddDebug()


[<EntryPoint>]
let main args =
    let contentRoot =
        Directory.GetCurrentDirectory()

    let webRoot =
        Path.Combine(contentRoot, "WebRoot")

    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            ! webHostBuilder
                .UseContentRoot(
                    contentRoot
                )
                .UseWebRoot(
                webRoot
            )
                .Configure(
                Action<IApplicationBuilder> configureApp
            )
                .ConfigureServices(
                configureServices
            )
                .ConfigureLogging(configureLogging))
        .Build()
        .Run()

    0