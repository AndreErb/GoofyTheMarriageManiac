using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MarriageManiac.Characters;
using MarriageManiac.Core;
using MarriageManiac.GameObjects;
using MarriageManiac.Core.Rectangles;
using MarriageManiac.Texts;

namespace MarriageManiac
{
    public class CreditsScene : Scene
    {
        private string NL = Environment.NewLine;

        public CreditsScene()
            : base()
        {
        }

        public override void Load()
        {
            BackColor = Color.Black;
            var music = SoundStore.Create("MyPrinceWillCome");
            music.Instance.IsLooped = true;
            music.Instance.Play();

            var text = "Hauptdarsteller:" + NL + NL
                     + "   Goofy (Goofy)" + NL + NL
                     + "   Daisy (Daniela Ruf)" + NL + NL
                     + "   Panzerknacker (Panzerknacker Boyz)" + NL + NL + NL + NL
                     + "Nebendarsteller:" + NL + NL
                     + "   Pfadfinder (Snoopy)" + NL + NL
                     + "   Lampengeist #1 (Professor Schmid)" + NL + NL
                     + "   Lampengeist #2 (Professor Garloff)" + NL + NL
                     + "   Männlicher Hochzeitsgast (Mickey Mouse)" + NL + NL
                     + "   Weiblicher Hochzeitsgast (Mini Mouse)" + NL + NL
                     + "   Der spendable Onkel (Dagobert Duck)" + NL + NL
                     + "   Die Neffen (Tick, Trick & Track)" + NL + NL + NL + NL
                     + "Charakter Design:" + NL + NL
                     + "   Walt Disney Company (WDC)" + NL + NL + NL + NL
                     + "Programmierung:" + NL + NL
                     + "   Daniel Seidel (WeddingSoft Inc.)" + NL + NL
                     + "   Andre Erb (WeddingSoft Inc.)" + NL + NL + NL + NL
                     + "Level Design:" + NL + NL
                     + "   Daniel Seidel (WeddingSoft Inc.)" + NL + NL
                     + "   Andre Erb (WeddingSoft Inc.)" + NL + NL + NL + NL
                     + "Tester:" + NL + NL
                     + "   Martina Erb" + NL + NL + NL + NL
                     + "Entwickelt unter:" + NL + NL
                     + "   Visual Studio 2013" + NL + NL
                     + "   Microsoft .NET 4.0 (C#)" + NL + NL
                     + "   Microsoft XNA Framework";
            
            var credits = new MovableText(350, 800, "Comic", Color.Gold, text, null);
            credits.MoveToTarget(350, -1700, 0.04f);
            DrawableObjects.Add(credits);

            credits.TargetReached += (obj, e) =>
            {
                text = "Wir danken unseren Familien und Freunden für ihre Unterstützung und ihr" + NL
                     + "grenzenloses Verständnis während den zahllosen Stunden welche wir gerne" + NL
                     + "und mit viel Spaß in die Entwicklung dieses Blockbuster-Games gesteckt haben." + NL + NL
                     + "Wir wünschen Goofy und Dani ein tolle gemeinsame Zukunft, einen reichen" + NL
                     + "Kindersegen, vorallem aber viel Spaß beim Zocken von ©GoofyTheMarriageManiac ;-)" + NL + NL
                     + "Euer Dani und André von WeddingSoft Inc.";
                var thanksgiving = new DelayedText(TimeSpan.FromSeconds(0.05), 150, 120, "Comic", Color.Gold, text, null);
                DrawableObjects.Add(thanksgiving);

                thanksgiving.Completed += (ob, ev) =>
                {
                    DrawableObjects.Add(new GoofyJunior(300, 400));
                    SoundStore.Create("Baby").Instance.Play();
                };
            };            
        }
                        
        public override void Update(GameTime gameTime)
        {
            // Update the collidables and drawables
            base.Update(gameTime);
        }
    }
}