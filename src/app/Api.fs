namespace ChatBackend

module Api =
  open Suave

  open ChatBackend.Domain
  open ChatBackend.Repo
  open ChatBackend.Helpers

  let private login_ email pass: Token option =
    let email_ = Domain.createEmail email
    let pass_ = Domain.createPassword pass
    Helpers.check2AndApply email_ pass_ (Domain.getToken Repo.getUser)
      
  let login email pass : WebPart =
    login_ email pass |> Helpers.resultOrFail


  let private register_ : string -> string -> User option =
    Domain.createUser
  
  let register email pass : WebPart =
    register_ email pass |> Helpers.resultOrFail

