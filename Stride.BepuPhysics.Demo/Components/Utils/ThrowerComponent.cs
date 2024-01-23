﻿using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;

namespace Stride.BepuPhysics.Demo.Components.Utils
{
    //[DataContract("SpawnerComponent", Inherited = true)]
    [ComponentCategory("BepuDemo - Utils")]
    public class ThrowerComponent : Spawner
    {
        public Entity? SpawnPosition { get; set; }

        public float Speed { get; set; } = 20f;

        public override void SimulationUpdate(float simTimeStep)
        {
        }

        public override void AfterSimulationUpdate(float simTimeStep)
        {
        }

        public override void Update()
        {
            DebugText.Print("Throw a prefab (T)", new(Game.Window.PreferredWindowedSize.X - 500, 125));

            if (SpawnPosition == null) return;

            if (Input.IsKeyPressed(Keys.T))
            {
                var camera = Game.Services.GetService<SceneSystem>().GraphicsCompositor.Cameras[0].Camera;
                var forward = Vector3.TransformNormal(-Vector3.UnitZ, Matrix.RotationQuaternion(camera.Entity.Transform.GetWorldRot()));

                Spawn(SpawnPosition.Transform.GetWorldPos(), (forward * Speed), new());
            }
        }
    }
}
