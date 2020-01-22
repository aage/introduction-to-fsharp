// F# SYNTAX

(*
    How to run scripts ('.fsx'):
        1. Send to 'F# Interactive' (Eaiest)
        2. From 'fsi.exe' (in Developer Command Prompt)
        3. Many other ways

    Code in 'F# Interactive' and execute by ending with double semi-colon (';;')
*)

// ===== Printing to console

printfn "%s" "hello"
printfn "It is the year %i" 2020
// printfn "It is the year %i" "2020" // compile error
printfn "%A" "hello" // use pretty printer
printfn "%O" "world" // use object.ToString()

// ===== Declare variables

    // (immutable and type inferred) variables (send to interactive to figure out types)

let age = 37
let pi = 3.14
let name = "Aage"

// ===== Types (records)

type Person = { age:int; name:string } // use semi-colon for separating fields
let me = {age= 37; name = "Aage" } // immutable!
let nextWeek = { me with age = 38 } // update copy method

    // when two types have the same labels the compiler picks the last:
    // https://fsharpforfunandprofit.com/posts/records/

    // Aliasses

type Age = Age of int
type Name = Name of string
type PersonTypeSafe = {age:Age;name:Name}

let age' = Age 37
let name' = Name "Aage"
let me' = {age= age'; name= name'}

type Length = Length of int
let length = Length 200
// let meAswell = {name=name'; age= length} // compile error

    // Type composition

type FirstName = FirstName of string
type LastName = LastName of string
type FullName = FirstName * LastName
// type FullName = {first:FirstName;last:LastName} // alternative
let name'' = FirstName "Aage", LastName "ten Hengel"

let printName (name:FullName) =
    let (first,last) = name // deconstruct
    printfn "First: %A Last: %A" first last

printName name''

// ===== Discriminated Unions

    // A Discriminated Union is a type of enum but:
        // 1. values point to types
        // 2. is exhaustive (unlike abstract classes)

type Money =
    | Dollar of decimal
    | Euro of decimal

let payMe money = // type inferrence
    match money with // pattern matching
    | Dollar d -> printfn "Payed with dollars, amount: %A" d
    | Euro e -> printfn "Payed with euros, amount: %A" e

let dollars = Dollar 42.75M
payMe dollars

// ===== Collections

    // For more info on differences: https://fsharpforfunandprofit.com/posts/list-module-functions/
    // Or: https://stackoverflow.com/questions/10814203/when-to-use-a-sequence-in-f-as-opposed-to-a-list

// == Lists -> 
    // 1. Use when you need pattern matching (recursive functions)
    // 2. Small collections
    // 3. Appending and prepending

let numbers = [1;2;3;4;5;6;7;8;9;10]
let numbers' = [1..10]
numbers = numbers' // true

    // Arrays -> Use for performance reasons with large sets

let alphabet = [|'A'..'Z'|]

    // Sequences (lazy) -> 'F#' equivalent for 'IEnumerable<T>'

let squares = seq { for i in 1..10 -> i * i }
let squares' = seq { for i = 0 to 10 do yield i * i } // alternative
squares |> Seq.iter (fun i -> printfn "%i" i)

// ====== Functions
    
    // regular syntax

let add x y = x + y

    // lambdas

let add' = fun x y -> x + y

    // explicitly type params (when type inferrence doesn't work)

let add'' (x:int, y:int) = x + y

    // compose functions with '|>'

type Human = {age:int}
let peoples = [40..50] |> List.map (fun age -> {age=age})
let averageAge = peoples
                 |> List.map (fun p -> p.age)
                 |> fun ages ->
                    let total = ages |> List.sum
                    let count = ages |> List.length
                    total / count
averageAge = 45 // true

    // partial application

let add1 = add 1
let three = add1 2
three = 3 // true

let add2 = add 2
let add3 = add1 >> add2
let four = add3 1

    // sentence naming

let ``when I add one and one together it should be two`` () =
    let oneAndOne = 1 + 1
    2 = oneAndOne