module DarkMadness2.Core.EventSourceUtils

let eventsSource (source : #IEventSource) = seq {
    while true do
        if source.HasNext () then
            yield source.Next ()
}

let combine (source1 : #IEventSource) (source2 : #IEventSource) =
    { new IEventSource with
        member this.HasNext () = source1.HasNext () || source2.HasNext ()
        member this.Next () = 
            if source1.HasNext () then
                source1.Next ()
            else 
                source2.Next ()
    }