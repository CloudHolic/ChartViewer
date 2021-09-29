open System.IO
open FParsec

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let sample = @"osu file format v14
AudioFilename: theglorydays.mp3
AudioLeadIn: 0
PreviewTime: 149255
Countdown: 0
SampleSet: Soft
StackLeniency: 0.7
Mode: 3
LetterboxInBreaks: 0
SpecialStyle: 0
WidescreenStoryboard: 0
"

[<EntryPoint>]
let main argv =
    let test p (str: string) =
        str.TrimEnd() |> run p
        |> function
            | Success(result, _, _)     -> printfn "Success: %A" result
            | Failure(errorMsg, _, _)   -> printfn "Failure: %s" errorMsg

    let text = File.ReadAllText @"C:\Users\SANDS\Downloads\Glory.osu"    
    let VersionParser = pstring "osu file format v" >>. pint32

    let str_ws s = pstring s >>. spaces    
    let sectionName = between (pstring "[") (pstring "]") (manySatisfy (fun c -> c <> '[' && c <> ']'))
    // let metaParser key = spaces >>. str_ws key >>. str_ws ":"
    //let strMetaParser key = metaParser key >>. manySatisfy (fun c -> c <> '\"')
    //let intMetaParser key = metaParser key >>. pint32

    //test sectionName @"[General]"
    // test (strMetaParser "AudioFileName") text
    //test VersionParser text
    //test (strMetaParser "Title") "Title : ???"
    //test (strMetaParser "Artist") "  Artist:    frolica"
    //test (strMetaParser "Creator") ("Creator:        loli god       ")
    //test (strMetaParser "Difficulty") "Difficulty:SHD   "
    //test (intMetaParser "CircleSize") "CircleSize:     5"

    let rec metaParser key = parse {
        do! spaces
        let! parser = str_ws key >>. str_ws ":" >>. manySatisfy (fun c -> c <> '\"')
        match parser with
        | "" ->
            do! skipRestOfLine true >>. metaParser key
        | v ->
            do! skipRestOfLine true
    }

    let osuParser = parse {
        do! spaces
        let! version = pstring "osu file format v" >>. pint32
        match version with
        | 14 ->
            do! metaParser "AudioFilename" >>. metaParser "Title" >>. metaParser "Artist" >>. metaParser "Creator" >>. metaParser "Difficulty"
        | _ ->
            printfn "It's an old format."
    }

    match run osuParser sample with
    | Success (v, _, _) -> printfn $"Success %A{v}"
    | Failure (msg, err, _) -> printfn $"%A{msg}"

    0 // return an integer exit code