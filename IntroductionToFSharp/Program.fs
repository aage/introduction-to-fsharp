// Learn more about F# at http://fsharp.org

open System
open PostOfficeDomain
open PostOfficeFunctions

[<EntryPoint>]
let main argv =

    // ====== Variables

    let senderAddress = {street = "Main street"; number = 1; city = "New York"}
    let sender = Sender(Name "John Smith", senderAddress)

    let destinationAddress = {street = "5th Avenue"; number = 42; city = "Los Angeles"}
    let destination = Sender(Name "Jane Doe", destinationAddress)

    let postOfficeAddress = {street = "Manhatten Boulevard"; number = 7; city = "New York"}
    let id = Guid.NewGuid() |> Id
    let postOffice = PostOffice(id, postOfficeAddress)
    
    printfn "Getting mail..."
    let r = new Random()
    let mail =      [1..15] // get 15 pieces of mail
                    |> List.map (float >> DateTime.Now.AddDays >> getDeliveryDate) // compose functions with '>>'
                    |> List.map (fun date ->
                         let code = Guid.NewGuid() |> TrackingCode
                         let isLetter = r.Next() % 2 = 0
                         let item = getMailItem sender destination isLetter
                         (postOffice, code, item, date))

    printfn "Delivering mail..."

    let results =   mail
                    |> List.map(fun del -> // monitor info printing (side effect)
                        let (_, code, mail, date) = del
                        let mailType = match mail with
                                       | Package _ -> "Package"
                                       | Letter _ -> "Letter"
                        printfn "Sending %s on %A with code %A" mailType date code
                        del)
                    |> List.map (fun del -> // attempt delivery
                        let (office, code, mail, date) = del
                        let result = delivery office code mail date
                        result)

    printfn "Mail handled."

    results         |> List.iter (fun result ->  // print results (side effects)
                        match result with
                        | Delivered (_, code) ->
                            printfn "Item with code %A delivered" code
                        | ReturnToPostOffice (_, code) ->
                            printfn "Item with code %A returned to post office" code
                        | ReturnToSender (_, code) ->
                            printfn "Item with code %A returned to sender" code)

    printfn "Mailing done. Press key to exit."
    Console.ReadKey() |> ignore
    0 // return an integer exit code
