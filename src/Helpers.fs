namespace ChatBackend

module Helpers =

  let toString (b:byte[]) = System.Text.Encoding.ASCII.GetString b
  let fromString (s:string) = System.Text.Encoding.ASCII.GetBytes s
