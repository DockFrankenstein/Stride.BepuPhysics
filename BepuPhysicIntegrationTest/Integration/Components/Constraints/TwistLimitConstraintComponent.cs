﻿using BepuPhysicIntegrationTest.Integration.Processors;
using BepuPhysics.Constraints;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Design;

namespace BepuPhysicIntegrationTest.Integration.Components.Constraints
{
    [DataContract]
    [DefaultEntityComponentProcessor(typeof(ConstraintProcessor), ExecutionMode = ExecutionMode.Runtime)]
    [ComponentCategory("Bepu - Constraint")]
    public class TwistLimitConstraintComponent : ConstraintComponent
    {
        internal TwistLimit _bepuConstraint = new();

        public Quaternion LocalBasisA
        {
            get
            {
                return _bepuConstraint.LocalBasisA.ToStrideQuaternion();
            }
            set
            {
                _bepuConstraint.LocalBasisA = value.ToNumericQuaternion();
                if (ConstraintData?.Exist == true)
                    ConstraintData.BepuSimulation.Simulation.Solver.ApplyDescription(ConstraintData.CHandle, _bepuConstraint);
            }
        }

        public Quaternion LocalBasisB
        {
            get
            {
                return _bepuConstraint.LocalBasisB.ToStrideQuaternion();
            }
            set
            {
                _bepuConstraint.LocalBasisB = value.ToNumericQuaternion();
                if (ConstraintData?.Exist == true)
                    ConstraintData.BepuSimulation.Simulation.Solver.ApplyDescription(ConstraintData.CHandle, _bepuConstraint);
            }
        }

        public float MinimumAngle
        {
            get { return _bepuConstraint.MinimumAngle; }
            set
            {
                _bepuConstraint.MinimumAngle = value;
                if (ConstraintData?.Exist == true)
                    ConstraintData.BepuSimulation.Simulation.Solver.ApplyDescription(ConstraintData.CHandle, _bepuConstraint);
            }
        }

        public float MaximumAngle
        {
            get { return _bepuConstraint.MaximumAngle; }
            set
            {
                _bepuConstraint.MaximumAngle = value;
                if (ConstraintData?.Exist == true)
                    ConstraintData.BepuSimulation.Simulation.Solver.ApplyDescription(ConstraintData.CHandle, _bepuConstraint);
            }
        }

        public SpringSettings SpringSettings
        {
            get
            {
                return _bepuConstraint.SpringSettings;
            }
            set
            {
                _bepuConstraint.SpringSettings = value;
                if (ConstraintData?.Exist == true)
                    ConstraintData.BepuSimulation.Simulation.Solver.ApplyDescription(ConstraintData.CHandle, _bepuConstraint);
            }
        }
    }
}