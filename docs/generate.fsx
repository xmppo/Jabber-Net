#load "../packages/FSharp.Formatting.2.14.4/FSharp.Formatting.fsx"

open System
open System.IO

open FSharp.Literate
open FSharp.Markdown

let input = Path.Combine (__SOURCE_DIRECTORY__, "content")
let output = Path.Combine (__SOURCE_DIRECTORY__, "output")
let formatting = Path.Combine (__SOURCE_DIRECTORY__, "../packages/FSharp.Formatting.2.14.4")
let templates = Path.Combine (formatting, "templates")
let docTemplate = Path.Combine (templates, "docpage.cshtml")

if not <| Directory.Exists output then
    ignore <| Directory.CreateDirectory output

let projectInfo =
    [ "project-author", "Friedrich von Never"
      "project-github", "https://github.com/ForNeVeR/Jabber-Net"
      "project-name", "Jabber-Net"
      "project-nuget", "https://www.nuget.org/packages/jabber-net/"
      "project-summary", "A set of .NET classes for sending and receiving Extensible Messaging and Presence Protocol (XMPP), also known as the Jabber."
      "root", "." ]

let transformUrl url =
    match Uri.TryCreate (url, UriKind.Relative) with
    | (true, uri) when url.EndsWith(".md") -> url.Substring (0, url.Length - 2) + "html"
    | _ -> url


let transformSpan : MarkdownSpan -> MarkdownSpan = function
| DirectLink (body, (url, smth)) -> DirectLink (body, (transformUrl url, smth))
| other -> other

let rec transformParagraph : MarkdownParagraph -> MarkdownParagraph = function
| Span s -> Span (List.map transformSpan s)
| ListBlock (kind, paragraphs) -> ListBlock (kind, List.map (List.map transformParagraph) paragraphs)
| other -> other

let customizeDocument (p : ProcessingContext) (ld : LiterateDocument) : LiterateDocument =
    ld.With(List.map transformParagraph ld.Paragraphs)

Literate.ProcessDirectory (input,
                           docTemplate,
                           output,
                           replacements = projectInfo,
                           layoutRoots = [templates],
                           customizeDocument = customizeDocument)
