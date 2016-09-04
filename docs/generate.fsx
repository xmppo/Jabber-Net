#load "../packages/FSharp.Formatting/FSharp.Formatting.fsx"

open System
open System.IO

open FSharp.Literate
open FSharp.Markdown
open FSharp.MetadataFormat

let content = Path.Combine (__SOURCE_DIRECTORY__, "content")
let output = Path.Combine (__SOURCE_DIRECTORY__, "output")
let jabberNetTemplates = Path.Combine (__SOURCE_DIRECTORY__, "templates")
let formatting = Path.Combine (__SOURCE_DIRECTORY__, "../packages/FSharp.Formatting")
let formattingTemplates = Path.Combine (formatting, "templates")
let referenceTemplates = Path.Combine (formattingTemplates, "reference")
let docTemplate = Path.Combine (formattingTemplates, "docpage.cshtml")

let mkdir name =
    if not <| Directory.Exists name then
        ignore <| Directory.CreateDirectory name

mkdir output

let root = defaultArg (Option.ofObj <| Environment.GetEnvironmentVariable "JABBER_NET_ROOT") "."
let projectInfo =
    [ "project-author", "Friedrich von Never"
      "project-github", "https://github.com/ForNeVeR/Jabber-Net"
      "project-name", "Jabber-Net"
      "project-nuget", "https://www.nuget.org/packages/jabber-net/"
      "project-summary", "A set of .NET classes for sending and receiving Extensible Messaging and Presence Protocol (XMPP), also known as the Jabber."
      "root", root ]

let transformUrl url =
    match Uri.TryCreate (url, UriKind.Relative) with
    | (true, _) when url.EndsWith(".md") -> url.Substring (0, url.Length - 2) + "html"
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

Literate.ProcessDirectory
    (content,
     docTemplate,
     output,
     replacements = projectInfo,
     layoutRoots = [jabberNetTemplates; formattingTemplates],
     customizeDocument = customizeDocument)

let bin = Path.Combine (__SOURCE_DIRECTORY__, "..", "bin", "Debug")
let reference = Path.Combine (output, "reference")
let library = Path.Combine (bin, "JabberNet.dll")

mkdir reference

MetadataFormat.Generate
    (library,
     reference,
     [jabberNetTemplates; formattingTemplates; referenceTemplates],
     parameters = projectInfo,
     markDownComments = false)
