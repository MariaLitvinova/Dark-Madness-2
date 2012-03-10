module DarkMadness2.Core.EventSourceUtils

let eventsSource (source : #IEventSource<'a>) = seq {
    while true do
        if source.HasNext () then
            yield source.Next ()
}

let combine (source1 : #IEventSource<'a>) (source2 : #IEventSource<'b>) (combineFunction : Choice<'a, 'b> -> 'c) =
    { new IEventSource<'c> with
        member this.HasNext () = source1.HasNext () || source2.HasNext ()
        member this.Next () = 
            if source1.HasNext () then
                combineFunction (Choice1Of2 <| source1.Next ())
            else 
                combineFunction (Choice2Of2 <| source2.Next ())
    }