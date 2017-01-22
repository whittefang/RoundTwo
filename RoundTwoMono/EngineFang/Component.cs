using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EngineFang
{
    class Component
    {
        public Transform transform;
        public Entity entity;
        public Component() {
        }

        public void Init(Transform transform, Entity entity) {
            this.transform = transform;
            this.entity = entity;
        }

    }
}
