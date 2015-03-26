using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.Texts
{
    public class DelayedText : DrawableTextBase
    {
        string _FullText;
        int _TextLength;
        int _CharIndex;
        TimeSpan _Interval;
        TimeSpan _LastTime;

        public DelayedText(TimeSpan interval, int xPos, int yPos, string font, Color color, string text, string texture)
            : base(xPos, yPos, font, color, texture)
        {
            _LastTime = new TimeSpan();
            _Interval = interval;

            Text = String.Empty;
            _FullText = text;
            _TextLength = _FullText.Length;
            _CharIndex = 0;
        }

        public override void Update(GameTime gameTime)
        {
            var elapsed = gameTime.TotalGameTime - _LastTime;
            var isInInterval = elapsed >= _Interval;
            
            if (isInInterval)
            {
                _LastTime = gameTime.TotalGameTime;

                if (_CharIndex < _TextLength)
                {
                    Text += _FullText[_CharIndex++].ToString();

                    if (Text == _FullText)
                    {
                        Complete();
                    }
                }
            }
        }
    }
}