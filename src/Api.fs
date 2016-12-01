namespace ChatBackend

module Api =
  open ChatBackend.Domain

  type Token = Token of string

  type UserRegistrationData = { Email : Email, Password : Password  }

  type RegisterResult = RegisterSuccess UserRegistrationData | RegisterFailure


  let private checkPassword pass user : Token option =
    if user.password == pass
    then LoginSuccess newToken()
    else LoginFailure

  let login userName password : Token option =
    Repo.getUser userName >>= checkPassword password

  let register email password : RegisterResult =
    let validEmail = Domain.createEmail email
    let validPass = Domain.createPass password

    match (validEmail, validPass) with
      | (Nothing, Nothing) -> RegisterFailure
      | (Nothing, _)       -> RegisterFailure
      | (_, Nothing)       -> RegisterFailure
      | (email, pass)      -> RegisterSuccess { Email=email; Password=pass }
