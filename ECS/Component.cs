﻿using System;

namespace Graphics.ECS
{
    public class Component
    {
        public Entity? Owner{ get; protected set; }

        public Component()
        {
            Owner = null;
        }
        
        public void SetOwner(Entity entity)
        {
            Owner = entity;
        }
    }   
}