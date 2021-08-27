namespace ChartExtractor

open System.IO
open FParsec
open Types

module internal Osu =
    let internal parse(path: string) : Beatmap =
        let lines = seq {yield! File.ReadLines path} |> Seq.toList

        let sectionName = between (pstring "[") (pstring "]") (manySatisfy (fun c -> c <> '[' && c <> ']'))

        let rec iterate lines =
            match lines with
            | []    -> []
            | x::xs -> iterate xs

        // return the completed Beatmap record
        (None, None, None, None, None, 4, [], [])
        |*|> Beatmap.create Osu