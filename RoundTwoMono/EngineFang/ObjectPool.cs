using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace RoundTwoMono.EngineFang
{
    class ObjectPool : Component
    {
        List<Hitsparks> objects;
        public HitSpark type;
        ContentManager content;

        public ObjectPool(HitSpark type, ContentManager content)
        {
            objects = new List<Hitsparks>();
            this.type = type;
            this.content = content;
        }

        public Hitsparks Get() {
            foreach (Hitsparks current in objects)
            {
                if (!current.enabled) {
                    return current;
                }
            }
            return AddObject();
        }
        public Hitsparks AddObject() {
            Entity newEntity = new Entity();
            Hitsparks newHitSpark = new Hitsparks(type);
            newEntity.addComponent(newHitSpark);
            objects.Add(newHitSpark);
            newHitSpark.Load(content);
            entity.scene.addEntity(newEntity);
            return newHitSpark;
        }
    }
}
