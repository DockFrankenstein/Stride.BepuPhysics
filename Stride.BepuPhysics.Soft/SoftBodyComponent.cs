﻿using System.Diagnostics;
using BepuPhysics;
using BepuPhysics.Collidables;
using Stride.BepuPhysics.Soft.Definitions;
using Stride.BepuPhysics.Systems;
using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.BepuPhysics.Soft
{
    [ComponentCategory("Bepu")]
    public class SoftBodyComponent : StartupScript
    {
        private int _simulationIndex = 0;
        private Model _model;

        [DataMemberIgnore]
        public BepuSimulation? Simulation { get; private set; }

        public int SimulationIndex
        {
            get
            {
                return _simulationIndex;
            }
            set
            {
                _simulationIndex = value;
            }
        }

        [MemberRequired(ReportAs = MemberRequiredReportType.Error)]
        public required Model Model
        {
            get => _model;
            set
            {
                _model = value;
            }
        }

        public override void Start()
        {
            base.Start();
            Simulation = Services.GetService<BepuConfiguration>().BepuSimulations[_simulationIndex];
            var modelData = ShapeCacheSystem.ExtractBepuMesh(Model, Game, Simulation.BufferPool);
            Newt.Create(Simulation, modelData);
        }

    }
}
