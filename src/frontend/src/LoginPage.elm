module LoginPage exposing(..)

import Html exposing (..)
import Html.Attributes exposing (href, class, style, height)

import Array exposing (Array)

import Material
import Material.Scheme
import Material.Color as Color
import Material.Grid exposing (..)
import Material.Button as Button
import Material.Textfield as Textfield
import Material.Options exposing (Style, css)
import Material.Helpers exposing (pure)

style : Int -> List (Style a)
style h =
  [ css "text-sizing" "border-box"
  , css "background-color" "#BDBDBD"
  , css "height" (toString h ++ "px")
  , css "padding-left" "8px"
  , css "padding-top" "4px"
  , css "color" "white"
  ]

democell : Int -> List (Style a) -> List (Html a) -> Cell a
democell k styling =
  cell <| List.concat [style k, styling]


small : List (Style a) -> List (Html a) -> Cell a
small = democell 50


std : List (Style a) -> List (Html a) -> Cell a
std = democell 700

color : Int -> Style a
color k =
    Array.get ((k + 0) % Array.length Color.hues) Color.hues
      |> Maybe.withDefault Color.Teal
      |> flip Color.color Color.S500
      |> Color.background
-- MODEL

type Email = Email String
type Password = Password String

type alias Model =
  { email    : Email
  , password : Password
  , confirm  : Password
  , increaseButtonModel : Button.Model
  , resetButtonModel : Button.Model
  }

model : Model
model =
  { email    = Email    ""
  , password = Password ""
  , confirm  = Password ""
  , increaseButtonModel = Button.defaultModel
  , resetButtonModel = Button.defaultModel
  }

-- UPDATE

type Msg
  = Login
  | Register
  | LoginButtonMsg Button.Msg
  | RegisterButtonMsg Button.Msg

update : Msg -> Model -> (Model, Cmd Msg)
update msg model =
  case msg of
    Login ->
      ( model , Cmd.none )
    Register ->
      ( model , Cmd.none )
    LoginButtonMsg msg_ ->
      let
        (submodel, fx) =
          Button.update msg_ model.increaseButtonModel
      in
        ( { model | increaseButtonModel = submodel }
        , Cmd.map LoginButtonMsg fx
        )

    RegisterButtonMsg msg_ ->
      let
        (submodel, fx) =
          Button.update msg_ model.resetButtonModel
      in
        ( { model | resetButtonModel = submodel }
        , Cmd.map RegisterButtonMsg fx
        )

-- VIEW

view : Model -> Html Msg
view model =
  grid [] [
    std [size All 4, size Tablet 6, color 3]
    [
      text "My Chat"
    , div [height 700] [
      Textfield.view
        [ Textfield.label
            ("Multiline textfield (" ++
                (toString (String.length model.str6))
                  ++ " of " ++ (toString (truncate model.length))
                  ++ " char limit)")
--        , Textfield.onInput Upd6
        , Textfield.textarea
        , Textfield.maxlength (truncate model.length)
        , Textfield.autofocus
        , Textfield.floatingLabel
        ]
    ]
    , div []
        [ Button.view LoginButtonMsg model.increaseButtonModel
            [ Button.raised
            , Button.colored
            , Button.ripple
            , Button.onClick Login
            , css "width" "47%"
            , css "margin" "5px"
            ]
            [ text "Log in" ]

          , Button.view RegisterButtonMsg model.resetButtonModel
            [ Button.raised
            , Button.colored
            , Button.ripple
            , Button.onClick Register
            , css "margin" "5px"
            , css "width" "47%"
            , css "bottom" "0" ]
            [ text "Register" ]

            ]
      ]
    ]
    |> Material.Scheme.topWithScheme Color.Teal Color.Green

main =
  Html.program
    { init = ( model, Cmd.none )
    , view = view
    , update = update
    , subscriptions = always Sub.none
    }
