﻿using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;

namespace Stride.BepuPhysics.Definitions.Raycast;

internal struct RayHitsCollectionHandler : IRayHitHandler, ISweepHitHandler
{
    private readonly ICollection<HitInfo> _collection;
    private readonly BepuSimulation _sim;

    public CollisionMask CollisionMask { get; set; }

    public RayHitsCollectionHandler(BepuSimulation sim, ICollection<HitInfo> collection, CollisionMask collisionMask)
    {
        _collection = collection;
        CollisionMask = collisionMask;
        _sim = sim;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AllowTest(CollidableReference collidable) => CollisionMask.AllowTest(collidable, _sim);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AllowTest(CollidableReference collidable, int childIndex)
    {
        return true;
    }

    public void OnRayHit(in RayData ray, ref float maximumT, float t, Vector3 normal, CollidableReference collidable, int childIndex)
    {
        _collection.Add(new(ray.Origin + ray.Direction * t, normal, t, _sim.GetContainer(collidable)));
    }

    public void OnHit(ref float maximumT, float t, Vector3 hitLocation, Vector3 hitNormal, CollidableReference collidable)
    {
        _collection.Add(new(hitLocation, hitNormal, t, _sim.GetContainer(collidable)));
    }

    public void OnHitAtZeroT(ref float maximumT, CollidableReference collidable)
    {
        // Right now just ignore the hit;
        // We can't just set info to invalid data, it'll be confusing for users,
        // but we might need to find a way to notify that the shape at its resting pose is already intersecting.
    }
}