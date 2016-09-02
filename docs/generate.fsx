#r "../packages/FSharp.Formatting.2.14.4/lib/net40/FSharp.Formatting.Common.dll"
#r "../packages/FSharp.Formatting.2.14.4/lib/net40/FSharp.Markdown.dll"

open System
open System.IO

open FSharp.Markdown
open FSharp.Formatting.Common

let formatting = Path.Combine (__SOURCE_DIRECTORY__, "../../packages/FSharp.Formatting/")
let docTemplate = Path.Combine (formatting, "templates/docpage.cshtml")
let output = Path.Combine (__SOURCE_DIRECTORY__, "../output")

if not <| Directory.Exists output then
    ignore <| Directory.CreateDirectory output

Directory.GetFiles (__SOURCE_DIRECTORY__, "*.md")
|> Seq.map (fun path -> path, File.ReadAllText path)
|> Seq.map (fun (path, text) -> path, Markdown.Parse text)
|> Seq.map (fun (path, markdown) -> path, Markdown.WriteHtml markdown)
|> Seq.iter (fun (path, html) ->
                 let name = Path.GetFileName path
                 let fileName = Path.ChangeExtension (name, "html")
                 let resultPath = Path.Combine (output, fileName)
                 File.WriteAllText (resultPath, html))

// TODO: Parse files as literate Markdown because it supports templates
