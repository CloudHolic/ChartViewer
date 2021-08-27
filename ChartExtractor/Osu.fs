namespace ChartExtractor

open System.IO
open FParsec
open Types

module internal Osu =
    let internal parse(path: string) : Beatmap =
        let lines = seq {yield! File.ReadLines path}



        // return the completed Beatmap record
        (None, None, None, None, None, 4, [], [])
        |*|> Beatmap.create Osu