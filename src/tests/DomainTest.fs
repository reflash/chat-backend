module DomainTest

open NUnit
open NUnit.Framework
open FsUnit

open ChatBackend.Domain

[<Literal>]
let validEmail = "abc@mail.ru"
[<Literal>]
let invalidEmail = "asd"

[<Literal>]
let validPassword = "somepass"
[<Literal>]
let invalidPassword = "shortpp"

[<TestCase(validEmail)>]
let ``When creating valid email should return Some email `` email =
    let email_ =  Email email
    createEmail validEmail |> should equal (Some email_)

[<TestCase(invalidEmail)>]
let ``When creating invalid email should return None `` email =
    createEmail email |> should equal None

[<TestCase(validPassword)>]
let ``When creating valid password should return Some pass `` pass =
    let pass_ =  Password pass
    createPassword pass |> should equal (Some pass_)

[<TestCase(invalidEmail)>]
let ``When creating invalid password should return None `` pass =
    createPassword pass |> should equal None

[<TestCase(validEmail, validPassword)>]
let ``When creating valid user should return Some user `` email pass =
    let user =  { email = Email email; password = Password pass; }
    createUser email pass |> should equal (Some user)

[<TestCase(invalidEmail, validPassword)>]
let ``When creating user with invalid email and valid pass should return None `` email pass =
    createUser email pass |> should equal None

[<TestCase(validEmail, invalidPassword)>]
let ``When creating user with valid email and invalid pass should return None `` email pass =
    createUser email pass |> should equal None

[<TestCase(invalidEmail, invalidPassword)>]
let ``When creating user with invalid email and invalid pass should return None `` email pass =
    createUser email pass |> should equal None

[<Test>]
let ``When trying to get token for registered user should return Some token ``() =
    let email = Email validEmail
    let pass = Password validPassword
    let user = { email = email; password = pass }
    getToken (fun e -> Some user) email pass |> Option.isSome |> should equal true

[<Test>]
let ``When trying to get token for non-registered user should return None ``() =
    let email = Email validEmail
    let pass = Password validPassword
    getToken (fun e -> None) email pass |> should equal None
