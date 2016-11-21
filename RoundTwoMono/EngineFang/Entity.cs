using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace RoundTwoMono
{
    class Entity
    {
        public Transform transform;
        public String id;
        public bool enabled;
        List<Component> componentList;
        List<Updateable> updateableComponents;
        List<Renderable> renderableComponents;

        public Entity() {
            componentList = new List<Component>();
            updateableComponents = new List<Updateable>();
            renderableComponents = new List<Renderable>();
            id = "";
            enabled = true;
            transform = new Transform(0, 0, 0);
        }
        public Entity(string id, Transform transform, bool enabled = true) {
            this.id = id;
            this.transform = transform;
            this.enabled = enabled;
        }
        // add a new componenet into the entity
        public void addComponent(Component newComponent) {
            newComponent.Init(transform, this);
            componentList.Add(newComponent);
            if (newComponent is Updateable) {
                updateableComponents.Add(newComponent as Updateable);
            }
            if (newComponent is Renderable) {
                renderableComponents.Add(newComponent as Renderable);
            }
        }

        public T getComponent<T>() where T : Component{
            for (int i = 0; i < componentList.Count; i++)
            {
                if (componentList[i] is T) {
                    return componentList[i] as T;
                }
            }
            
            return null;
        }
        // push update to all updateable components
        public void Update()
        {
            if (enabled) {  
                for (int i = 0; i < updateableComponents.Count; i++) {
                    updateableComponents[i].Update();
                }
            }
        }

        // push draw to all renderable components
        public void Draw(SpriteBatch spriteBatch) {
            if (enabled) {             
                for (int i = 0; i < renderableComponents.Count; i++) {
                    renderableComponents[i].Draw(spriteBatch);
                }
            }
        }
    }
}
