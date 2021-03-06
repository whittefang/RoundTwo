﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EngineFang
{
    class Scene
    {
        List<Entity> entities;
        public Scene() {
            entities = new List<Entity>();
        }

        public void addEntity(Entity entity) {
            entity.scene = this;
            entities.Add(entity);
        }

        public void Load(ContentManager content) {
            foreach (Entity e in entities)
            {
                e.Load(content);
            }
            
        }

        public void Update() {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update();
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw(spriteBatch);
            }
        }

    }
}
