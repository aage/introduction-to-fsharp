(*

        The post office has workers which deliver pieces of mail using delivery trucks.
        The model builds from simple types to more complex types.

*)

module PostOfficeDomain

    open System

    type Id = Id of Guid
    type Name = Name of string

    type Address = {
        street:string;
        number:int;
        city:string
        }

    type PostOffice = Id * Address

    type Sender = Name * Address
    type Destination = Name * Address
    type TrackingCode = TrackingCode of Guid
    type Weight = Weight of int
    type DeliveryDate = DeliveryDate of DateTime

    type Mail =
    | Letter of (Sender * Destination)
    | Package of (Sender * Destination * Weight)

    type DeliveryResult =
    | Delivered of Destination * TrackingCode
    | ReturnToPostOffice of PostOffice * TrackingCode
    | ReturnToSender of Sender * TrackingCode

    type Delivery = PostOffice -> TrackingCode -> Mail -> DeliveryDate -> DeliveryResult