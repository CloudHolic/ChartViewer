open System.IO
open FParsec

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    let test p (str: string) =
        str.TrimEnd() |> run p
        |> function
            | Success(result, _, _)     -> printfn "Success: %A" result
            | Failure(errorMsg, _, _)   -> printfn "Failure: %s" errorMsg

    let text = File.ReadAllText @"C:\Users\SANDS\Downloads\373357 Tia - The Glory Days\Tia - The Glory Days (CloudHolic) [Glory].osu"    
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
        let! parser = str_ws key >>. str_ws ":" >>. manySatisfy (fun c -> c <> '\"') |> opt
        match parser with
        | Some(v) ->
            printfn "Key: %s" v
        | None ->            
            do! skipRestOfLine true            
    }

    printfn "%A" (run (metaParser "AudioFileName") text)


    0 // return an integer exit code