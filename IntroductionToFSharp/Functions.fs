module PostOfficeFunctions

    open PostOfficeDomain

    let delivery:Delivery =
        fun office code mail date ->
            match (date, mail) with
            | (_,Letter letter) when
                // letters on even days are delivered
                let (DeliveryDate day) = date
                day.Day % 2 = 0 ->
                    let _,destination = letter
                    Delivered (destination, code)
            | (d, Package p) when
                // packages on uneven days are returned to postoffice
                let (DeliveryDate day) = d
                day.Day % 2 <> 0 ->
                    ReturnToPostOffice (office, code)
            | (_,_) -> // rest are returned to sender
                match mail with
                | Letter letter ->
                    let sender, _ = letter
                    ReturnToSender (sender, code)
                | Package package ->
                    let sender,_,_ = package
                    ReturnToSender (sender, code)
