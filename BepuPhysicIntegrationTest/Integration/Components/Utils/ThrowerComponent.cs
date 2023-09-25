﻿using System.Linq;
using BepuPhysicIntegrationTest.Integration.Components.Containers;
using BepuPhysicIntegrationTest.Integration.Components.Simulations;
using BepuPhysics;
using BepuPhysics.Collidables;
using Stride.Engine;
using Stride.Input;

namespace BepuPhysicIntegrationTest.Integration.Components.Utils
{
    //[DataContract("SpawnerComponent", Inherited = true)]
    [ComponentCategory("Bepu - Utils")]
    public class ThrowerComponent : Spawner
    {
        public Entity SpawnPosition { get; set; }

        public float Speed { get; set; } = 20f;

        public override void Update()
        {
            DebugText.Print("t - throw", new(1000, 10));
            if (Input.IsKeyPressed(Keys.T))
            {
                var camera = Game.Services.GetService<SceneSystem>().GraphicsCompositor.Cameras[0].Camera;
                var forward = Stride.Core.Mathematics.Vector3.TransformNormal(-Stride.Core.Mathematics.Vector3.UnitZ, Stride.Core.Mathematics.Matrix.RotationQuaternion(camera.Entity.Transform.Rotation)).ToNumericVector();

                Spawn(SpawnPosition.Transform.Position, (forward * Speed).ToStrideVector(), new());
            }
        }
    }
}