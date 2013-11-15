﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Badminton.Screens.Menus.Components;

namespace Badminton.Screens.Menus
{
	class MainMenu : GameScreen
	{
		List<Button> choices;
		private bool upPressed, downPressed, confirmPressed;

		public MainMenu()
		{
			choices = new List<Button>();
			choices.Add(new Button(Vector2.UnitX * 960 + Vector2.UnitY * 500, MainGame.tex_mm_comp, "comp"));
			choices.Add(new Button(Vector2.UnitX * 960 + Vector2.UnitY * 620, MainGame.tex_mm_coop, "coop"));
			choices.Add(new Button(Vector2.UnitX * 960 + Vector2.UnitY * 740, MainGame.tex_mm_cust, "cust"));
			choices.Add(new Button(Vector2.UnitX * 960 + Vector2.UnitY * 860, MainGame.tex_mm_exit, "exit"));
			choices[0].Selected = true;
			confirmPressed = true;
			upPressed = true;
			downPressed = true;
		}

		public GameScreen Update(GameTime gameTime)
		{
			foreach (Button b in choices)
			{
				b.Update();
				if (b.IsMouseOver())
				{
					GetSelectedButton().Selected = false;
					b.Selected = true;
				}
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
			{
				if (!downPressed)
				{
					downPressed = true;
					for (int i = 0; i < choices.Count; i++)
					{
						if (choices[i].Selected)
						{
							choices[i].Selected = false;
							i++;
							if (i < choices.Count)
								choices[i].Selected = true;
							else
								choices[0].Selected = true;
						}
					}
				}
			}
			else
				downPressed = false;

			if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp))
			{
				if (!upPressed)
				{
					upPressed = true;
					for (int i = choices.Count - 1; i >= 0; i--)
					{
						if (choices[i].Selected)
						{
							choices[i].Selected = false;
							i--;
							if (i >= 0)
								choices[i].Selected = true;
							else
								choices[choices.Count - 1].Selected = true;
						}
					}
				}
			}
			else
				upPressed = false;

			if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
			{
				if (!confirmPressed)
				{
					confirmPressed = true;
					foreach (Button b in choices)
					{
						if (b.Selected)
						{
							if (b.ReturnString == "comp")
								return new Menus.PlayerSelect(0);
							else if (b.ReturnString == "coop")
								return new Menus.PlayerSelect(1);
							else if (b.ReturnString == "cust")		// TODO: Change this when we have character customization
								return this;
							else
								return null;
						}
					}
				}
			}
			else
				confirmPressed = false;

			return this;
		}

		private Button GetSelectedButton()
		{
			foreach (Button b in choices)
				if (b.Selected)
					return b;

			// If no buttons are selected, select the first one
			choices[0].Selected = true;
			return choices[0];
		}

		public GameScreen Exit()
		{
			return null;
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_logo, Vector2.UnitX * 960 + Vector2.UnitY * 240, null, Color.White, 0f, new Vector2(MainGame.tex_logo.Width / 2, MainGame.tex_logo.Height / 2), 1f, SpriteEffects.None, 0);
			foreach (Button b in choices)
				b.Draw(sb);
		}
	}
}