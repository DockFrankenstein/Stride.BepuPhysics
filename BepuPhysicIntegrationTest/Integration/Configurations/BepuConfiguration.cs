﻿using System.Collections.Generic;
using Stride.Core;
using Stride.Data;

namespace BepuPhysicIntegrationTest.Integration.Configurations;
[DataContract]
[Display("Bepu Settings")]
public class BepuConfiguration : Configuration
{
    public List<BepuSimulation> BepuSimulations = new();

}