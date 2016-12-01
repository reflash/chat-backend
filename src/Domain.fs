namespace ChatBackend
open System
open System.Text.RegularExpressions


module Domain =
  type Email = Email of string
  type Password = Password of string

  let private emailRegex = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"
  let private validateAndCreateEmail email =
    if Regex.IsMatch(email, emailRegex)
    then Email email
    else Nothing

  let createEmail : Email option =
    validateAndCreateEmail

  let private passRegex = "^[A-Za-z\d]{8,}$" // Nums and chars 8+
  let private validateAndCreatePassword pass =
    if Regex.IsMatch(pass, passRegex)
    then Password pass
    else Nothing

  let createPassword : Password option =
      validateAndCreatePassword
