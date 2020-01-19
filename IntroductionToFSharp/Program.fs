// Learn more about F# at http://fsharp.org

open System
open PostOfficeDomain
open PostOfficeFunctions

[<EntryPoint>]
let main argv =

    // variables

    let sender = Sender(
                    Name "John Smith",
                    {street = "Main street"; number = 1; city = "New York"})

    let destination = Sender(
                        Name "Jane Doe",
                        {street = "5th Avenue"; number = 42; city = "Los Angeles"})

    let postOffice = PostOffice(
                        Id (Guid.NewGuid()),
                        {street = "Manhatten Boulevard"; number = 7; city = "New York"})

    let getCode = fun () -> Guid.NewGuid() |> TrackingCode
    let r = new Random()
    let getMailItem = fun sender destination ->
        let random = r.Next()
        let even = random % 2 = 0
        if even then // package
            let weight = Weight random // use random as weight
            Package (sender,destination, weight)
        else
            Letter (sender,destination)

    // create mail
    printfn "Sending mail..."

    let mail = [1..10]
               |> List.map (fun i -> DateTime.Now.AddDays(float i) |> DeliveryDate)
               |> List.map (fun date ->
                    let code = getCode()
                    let item = getMailItem sender destination
                    (postOffice, code, item, date))
    mail |> List.map (fun m ->
            let (office, code, mail, date) = m
            let (DeliveryDate d) = date
            let mailType = match mail with
                           | Package _ -> "Package"
                           | Letter _ -> "Letter"
            printfn "Sending %s on %A with code %A" mailType date code
            let result = delivery office code mail date
            result)
        |> List.iter (fun r ->
                match r with
                | Delivered (_, code) ->
                    printfn "Item with code %A delivered" code
                | ReturnToPostOffice (_, code) ->
                    printfn "Item with code %A returned to post office" code
                | ReturnToSender (_, code) ->
                    printfn "Item with code %A returned to post office" code
            )

    printfn "Mailing done!"
    Console.ReadKey() |> ignore
    0 // return an integer exit code
