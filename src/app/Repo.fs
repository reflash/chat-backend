namespace ChatBackend

module Repo =
    open ChatBackend.Domain

    let getUser email : User option = Some { email = email; password = Password "" }
    let addUser email password = ignore
