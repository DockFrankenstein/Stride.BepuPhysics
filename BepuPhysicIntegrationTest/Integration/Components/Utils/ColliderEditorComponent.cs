﻿using System.Linq;
using BepuPhysicIntegrationTest.Integration.Components.Colliders;
using BepuPhysicIntegrationTest.Integration.Components.Constraints;
using BepuPhysicIntegrationTest.Integration.Components.Containers;
using BepuPhysicIntegrationTest.Integration.Configurations;
using BepuPhysics.Constraints;
using Silk.NET.OpenGL;
using Stride.Engine;
using Stride.Input;

namespace BepuPhysicIntegrationTest.Integration.Components.Utils
{
    //[DataContract("SpawnerComponent", Inherited = true)]
    [ComponentCategory("Bepu - Utils")]
    public class ColliderEditorComponent : SyncScript
    {
        public ColliderComponent? Component { get; set; }


        public override void Start()
        {
        }

        public override void Update()
        {
            if (Component == null || ! (Component is BoxColliderComponent))
                return;

            if (Input.IsKeyPressed(Keys.U))
            {
                ((BoxColliderComponent)Component).Size += new Stride.Core.Mathematics.Vector3(0, 1, 0);
                ((BoxColliderComponent)Component).Entity.Transform.Scale += new Stride.Core.Mathematics.Vector3(0, 1, 0);
            }
            if (Input.IsKeyPressed(Keys.J))
            {
                ((BoxColliderComponent)Component).Size -= new Stride.Core.Mathematics.Vector3(0, 1, 0);
                ((BoxColliderComponent)Component).Entity.Transform.Scale -= new Stride.Core.Mathematics.Vector3(0, 1, 0);
            }
            if (Input.IsKeyPressed(Keys.N))
            {
                var rr = (BodyContainerComponent)Component.Container;
                rr.Kinematic = !rr.Kinematic;
            }
            DebugText.Print($"Size : {((BoxColliderComponent)Component).Size} (numpad u & j) + n for toggle kinematic", new(BepuAndStrideExtensions.X_DEBUG_TEXT_POS, 350));
        }
    }
}