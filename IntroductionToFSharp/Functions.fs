module PostOfficeFunctions

    open PostOfficeDomain

    let getDeliveryDate date = date |> DeliveryDate

    let getMailItem sender destination isLetter = 
        if isLetter then
            Letter (sender,destination)
        else
            let weight = Weight 100 // dummy weight
            Package (sender,destination, weight)

    // The delivery itself
    let delivery:Delivery =
        fun office code mail date ->
            match (date, mail) with
            | (_,Letter letter) -> // check for letters
                let (DeliveryDate day) = date
                if day.Day % 2 = 0 then
                    let _,destination = letter
                    Delivered (destination, code) // letters on even days are delivered
                else
                    let sender, _ = letter
                    ReturnToSender (sender, code) // letters on uneven days are send back to postoffice
            | (_, Package package) -> // check for packages
                let (DeliveryDate day) = date
                if day.Day % 2 <> 0 then
                    let _,destination, _ = package
                    Delivered (destination, code) // packages on uneven days are delivered
                else
                    ReturnToPostOffice (office, code) // packages on even days are returned to post office