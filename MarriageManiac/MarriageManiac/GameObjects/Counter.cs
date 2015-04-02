﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MarriageManiac.Texts;
using System.Timers;

namespace MarriageManiac.GameObjects
{
    class Counter : Text
    {
        TimeSpan _CurrentTime;

        public Counter(TimeSpan time, int xPos, int yPos, Color color)
            : this(time, xPos, yPos, "Comic", color, String.Empty)
        {
        }

        public Counter(TimeSpan time, int xPos, int yPos, string font, Color color, string texture)
            : base(xPos, yPos, font, color, String.Empty, texture)
        {
            if (time != null)
            {
                CurrentTime = InitialTime = time;
            }
            else
            {
                CurrentTime = InitialTime = TimeSpan.Zero;
            }

            Text = CurrentTime.ToString(@"mm\:ss");
        }

        public event EventHandler<TimeChangedEventArgs> TimeChanged;
        public event EventHandler CountDownFinished;        
        private TimeSpan InitialTime { get; set; }

        public virtual TimeSpan CurrentTime
        {
            get { return _CurrentTime; }
            set
            {
                _CurrentTime = value;
                OnTimeChanged(this, value);
            }
        }

        public void CountDown()
        {
            CountDownFrom(InitialTime);
        }

        public void CountDownFrom(TimeSpan countDown)
        {
            var startTime = DateTime.Now;

            var timer = new Timer(500);
            timer.Elapsed += (sender, e) =>
            {
                var elapsed = e.SignalTime - startTime;
                CurrentTime = countDown - elapsed;

                Text = CurrentTime.ToString(@"mm\:ss");

                if (CurrentTime <= TimeSpan.Zero)
                {
                    timer.Dispose();
                    OnCountDownFinished(this);
                }
            };

            timer.Start();
        }

        protected void OnTimeChanged(object sender, TimeSpan currentTime)
        {
            if (TimeChanged != null)
            {
                TimeChanged(sender, new TimeChangedEventArgs(currentTime));
            }
        }

        protected void OnCountDownFinished(object sender)
        {
            if (CountDownFinished != null)
            {
                CountDownFinished(sender, new EventArgs());
            }
        }
    }


    class TimeChangedEventArgs : EventArgs
    {
        public TimeChangedEventArgs(TimeSpan currentTime)
        {
            CurrentTime = currentTime;
        }

        public TimeSpan CurrentTime { get; set; }
    }
}