﻿using Stride.BepuPhysics.Processors;
using Stride.Core;
using Stride.Engine;
using Stride.Engine.Design;
using Stride.Rendering;

namespace Stride.BepuPhysics.Components.Containers
{
    [DataContract]
    [DefaultEntityComponentProcessor(typeof(ContainerProcessor), ExecutionMode = ExecutionMode.Runtime)]
    [ComponentCategory("Bepu - Containers")]
    public class BodyMeshContainerComponent : BodyContainerComponent, IMeshContainerComponent
    {

        private float _mass = 1f;
        private bool _closed = true;

        public float Mass
        {
            get => _mass;
            set
            {
                _mass = value;
                ContainerData?.TryUpdateContainer();
            }
        }
        public bool Closed
        {
            get => _closed;
            set
            {
                _closed = value;
                ContainerData?.TryUpdateContainer();
            }
        }

        public Model? Model { get; set; }
    }
}
