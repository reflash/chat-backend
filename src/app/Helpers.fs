namespace ChatBackend

module Helpers =
  open Suave
  open Suave.Successful
  open Suave.RequestErrors

  let resultOrFail (x: 'a option) : WebPart =
    match x with
    | Some x -> OK (x.ToString())
    | None -> RequestErrors.FORBIDDEN "Incorrect input"

  let toString (b:byte[]) = System.Text.Encoding.ASCII.GetString b
  let fromString (s:string) = System.Text.Encoding.ASCII.GetBytes s

  let check2AndApply x y f =
    match (x, y) with
      | (None, None)     -> None
      | (None, _)        -> None
      | (_, None)        -> None
      | (Some x, Some y) -> f x y

  let (>=>) f g x = f x |> Option.bind g
