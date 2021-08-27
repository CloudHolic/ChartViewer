namespace ChartExtractor

module Types =
    let inline internal (|*|>) (a, b, c, d, e, f, g, h) func = func a b c d e f g h

    // Base type abbreviations
    type FileType = 
        | Osu
        | Bms
        | Quaver
        | O2jam
        | StepMania

    type Name = string
    type FileName = Name
    type PersonName = Name
    type DiffName = Name

    type MilliSeconds = int
    type Bpm = int
    type Meter = int
    type BeatLength = MilliSeconds  // Length of each beat.
    type Multiplier = float
    type Offset = MilliSeconds // Milliseconds from the beginning of the beatmap's audio.
    type Line = int

    type Timing =
        | Absolute of Bpm * BeatLength * Meter
        | Relative of Multiplier

    type TimingPoint =
        {
            Timing  : Timing
            Time    : Offset
        }

    type Note =
        | ShortNote of Line * Offset
        | LongNote of Line * Offset * Offset

    type Beatmap = 
        {
            // Metadata
            FileType    : FileType          // File type like Osu, BMS, Quaver, O2jam, StepMania, and so on.
            FileName    : FileName option   // Audio file name, if exists
            Title       : PersonName option // Song title
            Artist      : PersonName option // Song artist
            Author      : PersonName option // Beatmap creator
            Difficulty  : DiffName option   // Difficulty name
            Lines       : Line              // Total lines   

            // Timing Points
            Timings     : TimingPoint list

            // Notes
            Notes       : Note list
        }       

        static member create filetype filename title artist author difficulty lines timings notes =
            {
                FileType = filetype
                FileName = filename
                Title = title
                Artist = artist
                Author = author
                Difficulty = difficulty
                Lines = lines
                Timings = timings
                Notes = notes
            }