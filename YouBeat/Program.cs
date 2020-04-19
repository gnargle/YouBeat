using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace YouBeat {
    class Program {
        static void Main(string[] args) {

            var game = new Game("YouBeat", 1920, 1080, 60, false);
            //game.Start();
            game.Color = Color.Grey;           
            game.Start(new TitleScene());

            /*
            var game = new Game("Tweening!");
            // Set the background color to something nice.
            game.Color = new Color(0.2f, 0.2f, 0.3f);

            // Create a Scene.
            var scene = new Scene();
            // Add a bunch of Entities to it to demonstrate ease types.
            scene.Add(new MovingTween(Ease.Linear));
            scene.Add(new MovingTween(Ease.ExpoOut));
            scene.Add(new MovingTween(Ease.ExpoIn));
            scene.Add(new MovingTween(Ease.ExpoInOut));
            scene.Add(new MovingTween(Ease.SineOut));
            scene.Add(new MovingTween(Ease.SineIn));
            scene.Add(new MovingTween(Ease.SineInOut));
            scene.Add(new MovingTween(Ease.BackOut));
            scene.Add(new MovingTween(Ease.BackIn));
            scene.Add(new MovingTween(Ease.BackInOut));
            scene.Add(new MovingTween(Ease.ElasticOut));
            scene.Add(new MovingTween(Ease.ElasticIn));
            scene.Add(new MovingTween(Ease.ElasticInOut));
            scene.Add(new MovingTween(Ease.QuintOut));
            scene.Add(new MovingTween(Ease.QuintIn));
            scene.Add(new MovingTween(Ease.QuintInOut));

            // Add some Entities that demonstrate tweening different fields.
            scene.Add(new ScalingTween(60, 400, Ease.Linear));
            scene.Add(new ScalingTween(180, 400, Ease.ElasticInOut));
            scene.Add(new ScalingTween(300, 400, Ease.BackInOut));

            // Add an Entity to demonstrate tweening values to change Color.
            scene.Add(new ColorTween(420, 400));

            // Add an Entity that tweens in response to a key press.
            scene.Add(new ReactiveTween(540, 400));

            // Start 'er up.
            game.Start(scene);
        }    

    class ScalingTween : Entity {
        public ScalingTween(float x, float y, Func<float, float> easeType) : base(x, y) {
            // Add a simple circle graphic.
            AddGraphic(Image.CreateCircle(20, Color.White));
            Graphic.CenterOrigin();

            // Tween the scale of the graphic with the easetype for 120 frames.
            // Also reflect and repeat the tween forever!
            Tween(Graphic, new { ScaleX = 2, ScaleY = 2 }, 120)
              .Ease(easeType)
              .Reflect()
              .Repeat();
        }
    }

    class ColorTween : Entity {
        // Tween this value to determine the color later.
        float hue;

        public ColorTween(float x, float y) : base(x, y) {
            // Add a simple circle graphic.
            AddGraphic(Image.CreateCircle(30, Color.White));
            Graphic.CenterOrigin();

            // Tween the hue from 0 to 1 and repeat it forever over 360 frames.
            Tween(this, new { hue = 1 }, 360)
              .Repeat();
        }

        public override void Update() {
            base.Update();
            // Update the Color every update by using the tweened hue value.
            Graphic.Color = Color.FromHSV(hue, 1, 1, 1);
        }
    }

    class ReactiveTween : Entity {
        public ReactiveTween(float x, float y) : base(x, y) {
            // Add a simple circle graphic.
            AddGraphic(Image.CreateCircle(30, Color.White));
            Graphic.CenterOrigin();
        }

        public override void Update() {
            base.Update();

            if (Input.KeyPressed(Key.Any)) {
                // If a key is pressed do a cool tween.
                Tween(Graphic, new { ScaleX = 1, ScaleY = 1 }, 30)
                  .From(new { ScaleX = 2, ScaleY = 0.5f })
                  .Ease(Ease.ElasticOut);
            }
        }
    }

        class MovingTween : Entity {
            // The next Y position to place the Entity at.
            static float nextY;
            // How far each MovingTween Entity should be spaced from each other vertically.
            static float spacing = 20;
            // The next hue value to color the Graphic with.
            static float nextHue;

            public MovingTween(Func<float, float> easeType) {
                // Create a Color using the nextHue value.
                var color = Color.FromHSV(nextHue, 1, 1, 1);
                // Make a circle using that color.
                var image = Image.CreateCircle(8, color);
                // Make it fancy.
                image.OutlineColor = Color.Black;
                image.OutlineThickness = 1;
                image.CenterOrigin();
                AddGraphic(image);

                // Adjust the nextY and nextHue for the future MovingTweens.
                nextY += spacing;
                nextHue += 0.05f;

                // Set the position here.
                X = 40;
                Y = nextY;

                // Tween the Entity across the screen and back for 180 frames.
                Tween(this, new { X = Game.Instance.Width - 40 }, 180)
                  .Ease(easeType)
                  .Reflect()
                  .RepeatDelay(30)
                  .Repeat();
            }
            */
        }        
    }    
}
