namespace ChatBackend
open System
open System.Text.RegularExpressions


module Domain =
  open ChatBackend.Helpers

  type Email = Email of string
  type Password = Password of string

  let private emailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
  let private validateAndCreateEmail email : Email option =
    if Regex.IsMatch(email, emailRegex)
    then Some <| Email email
    else None

  let createEmail =
    validateAndCreateEmail

  let private passRegex = "^[A-Za-z\d]{8,}$" // Nums and chars 8+
  let private validateAndCreatePassword pass =
    if Regex.IsMatch(pass, passRegex)
    then Some <| Password pass
    else None

  let createPassword =
      validateAndCreatePassword
    
  type User = { email : Email; password : Password;  }

  let private createUserRecord e p =
    Some { email = e; password = p; }

  let createUser email pass = 
    let email = createEmail email
    let pass = createPassword pass
    Helpers.check2AndApply email pass createUserRecord 
    

  type Token = Token of string

  let private checkPassword pass user : Token option =
    if user.password = pass then Some <| Token "A" else None

  let getToken (getUser:Email -> User option) email pass = 
    email |> ( getUser >=> checkPassword pass )