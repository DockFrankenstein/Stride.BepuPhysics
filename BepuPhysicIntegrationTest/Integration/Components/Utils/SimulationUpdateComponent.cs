﻿using BepuPhysicIntegrationTest.Integration.Configurations;
using Stride.Core;
using Stride.Engine;

namespace BepuPhysicIntegrationTest.Integration.Components.Utils
{
    public abstract class SimulationUpdateComponent : SyncScript
    {

        public int SimulationIndex { get; set; } = 0; //TODO : Cancel/restart on edit. + Check Services.GetService<BepuConfiguration>().BepuSimulations bounds.


        [DataMemberIgnore]
        protected BepuSimulation BepuSimulation { get; set; }

        public override void Start()
        {
            base.Start();
            BepuSimulation = Services.GetService<BepuConfiguration>().BepuSimulations[SimulationIndex];
            BepuSimulation.Register(this);
        }
        public override void Cancel()
        {
            base.Cancel();
            BepuSimulation.Unregister(this);
        }

        public abstract void SimulationUpdate(float simTimeStep);
    }
}
