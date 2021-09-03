namespace ChartExtractor

open System.IO
open FParsec
open Types

module Osu =
    let internal parse(path: string) : Beatmap =
        let txt = File.ReadAllText path        
        let clearNone v = List.fold (fun x y -> match y with None -> x | Some(z) -> z :: x) [] v

        let str_ws s = pstring s >>. spaces
        let sectionName = between (pstring "[") (pstring "]") (manySatisfy (fun c -> c <> '[' && c <> ']'))
        let metaParser key = str_ws key >>. str_ws ":"
        let strMetaParser key = metaParser key >>. manySatisfy (fun c -> c <> '\"')
        let intMetaParser key = metaParser key >>. pint32

        let LinesParser = intMetaParser "CircleSize"

        // let TimingParser =

        // let NoteParser = 

        let rec metaParser key = parse {
            do! spaces
            let! parser = str_ws key >>. str_ws ":" >>. manySatisfy (fun c -> c <> '\"')
            match parser with
            | "" ->
                do! skipRestOfLine true
                do! metaParser key
            | v ->
                printfn $"Key: %s{v}"
        }
        
        let osuParser = parse {
            do! spaces
            let! version = pstring "osu file format v" >>. pint32
            match version with
            | 14 ->
                do! skipRestOfLine true
                do! pipe5 (metaParser "AudioFileName") (metaParser "Title") (metaParser "Artist") (metaParser "Creator") (metaParser "Difficulty")
            | _ ->
                printfn "It's an old format."
        }

        match run osuParser txt with
        | Success (v, _, _) -> printfn "Success"
        | Failure (msg, err, _) -> printfn $"%A{msg}"

        //match run (many ) txt with
        //| Success (v, _, _) -> Beatmap.create Osu
        //| Failure (msg, err, _) -> Beatmap.create Osu

        // return the completed Beatmap record
        (None, None, None, None, None, 4, [], [])
        |*|> Beatmap.create Osu