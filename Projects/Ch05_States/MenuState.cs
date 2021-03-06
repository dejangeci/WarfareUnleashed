﻿namespace Ch05_States
{
  using SFML.Graphics;
  using SFML.System;
  using SFML.Window;
  using System.Collections.Generic;

  internal class MenuState : State
  {
    private Sprite backgroundSprite;

    private List<Text> options;
    private int optionIndex;

    public MenuState(StateStack stack, Context context)
      : base(stack, context)
    {
      backgroundSprite = new Sprite();
      options = new List<Text>();

      var texture = context.Textures.Get(Textures.ID.TitleScreen);
      var font = context.Fonts.Get(Fonts.ID.Main);

      backgroundSprite.Texture = texture;

      // A simple menu demonstration
      var playOption = new Text();
      playOption.Font = font;
      playOption.DisplayedString = "Play";
      Utility.CenterOrigin(playOption);
      playOption.Position = context.Window.GetView().Size / 2f;
      options.Add(playOption);

      var exitOption = new Text();
      exitOption.Font = font;
      exitOption.DisplayedString = "Exit";
      Utility.CenterOrigin(exitOption);
      exitOption.Position = playOption.Position + new Vector2f(0, 30);
      options.Add(exitOption);

      UpdateOptionText();
    }

    public override void Draw()
    {
      var window = GetContext().Window;

      window.SetView(window.DefaultView);
      window.Draw(backgroundSprite);

      foreach (var option in options)
      {
        window.Draw(option);
      }
    }

    public override bool Update(Time dt)
    {
      return true;
    }

    public override bool HandleKeyboardInput(Keyboard.Key key, bool isPressed)
    {
      // The demonstration menu logic
      if (!isPressed) return false;

      if (key == Keyboard.Key.Return)
      {
        if (optionIndex == (int)OptionNames.Play)
        {
          RequestStackPop();
          RequestStackPush(States.ID.Game);
        }
        else if (optionIndex == (int)OptionNames.Exit)
        {
          // The exit option was chosen, by removing itself, the stack will be empty, and the game will know it is time to close.
          RequestStackPop();
        }
      }
      else if (key == Keyboard.Key.Up)
      {
        // Decrement and wrap-around
        if (optionIndex > 0)
        {
          optionIndex--;
        }
        else
        {
          optionIndex = options.Count - 1;
        }

        UpdateOptionText();
      }
      else if (key == Keyboard.Key.Down)
      {
        // Increment and wrap-around
        if (optionIndex < options.Count - 1)
        {
          optionIndex++;
        }
        else
        {
          optionIndex = 0;
        }

        UpdateOptionText();
      }

      return true;
    }

    private void UpdateOptionText()
    {
      if (options.Count == 0) return;

      // White all texts
      foreach (var option in options)
      {
        option.Color = Color.White;
      }

      // Red the selected text
      options[optionIndex].Color = Color.Red;
    }

    private enum OptionNames
    {
      Play,
      Exit
    }
  }
}
