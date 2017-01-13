using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RoundTwoMono
{
    
    class Hitsparks: Component, Updateable
    {
        public HitSpark type;
        SpriteAnimator<HitSpark> animator;
        public bool enabled = false;

        int timeLeftEnabled;

        public Hitsparks(HitSpark type) {
            this.type = type;
            animator = new SpriteAnimator<HitSpark>(new Vector2(30,30));
        }
        public void Load(ContentManager content) {
            Animation anim  = new Animation(animationType.oneShot);
            if (type == HitSpark.block)
            {
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29361"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29362"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29363"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29364"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29365"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29366"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29367"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/Block/29368"), 1);
            }
            else if (type == HitSpark.light)
            {

                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30102"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30103"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30104"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30105"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30106"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30107"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30108"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30109"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30110"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/light/30111"), 1);
            }
            else if (type == HitSpark.medium)
            {

                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30112"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30113"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30114"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30115"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30116"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30117"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30118"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30119"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30120"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/medium/30121"), 1);
            }
            else if (type == HitSpark.heavy)
            {

                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30122"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30123"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30123"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30124"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30125"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30126"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30127"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30128"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30129"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30130"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/heavy/30131"), 1);
            }
            else if (type == HitSpark.special)
            {

                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30132"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30133"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30134"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30135"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30136"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30137"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30138"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30139"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30140"), 1);
                anim.addFrame(content.Load<Texture2D>("HitEffects/special/30141"), 1);
            }
            anim.renderOneshotAfterCompletion = false;
            animator.addAnimation(type, anim);
            
            entity.addComponent(animator);
        }

        public void Play(Vector2 position) {
            transform.position = new Vector3(position.X, -position.Y, 0);
            timeLeftEnabled = 30;
            enabled = true;
            animator.PlayAnimation(type, true);
        }
        public void Update() {
            if (enabled) {
                timeLeftEnabled--;
                if (timeLeftEnabled <= 0) {
                    enabled = false;
                }
            }
        }

    }
    enum HitSpark
    {
        block,
        light,
        medium,
        heavy,
        special

    }
}
