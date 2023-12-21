﻿using DotRecast.Detour;
using DotRecast.Recast.Toolset;
using DotRecast.Recast.Toolset.Builder;
using Stride.BepuPhysics.Components.Containers;
using Stride.BepuPhysics.Navigation.Components;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Materials.ComputeColors;
using Stride.Rendering.Materials;
using System.Xml.Linq;
using Stride.Core;
using Stride.Input;
using DotRecast.Core.Numerics;

namespace Stride.BepuPhysics.Navigation.Processors;
public class RecastMeshProcessor : EntityProcessor<BepuNavigationBoundingBoxComponent>
{

	public List<Vector3> Points = new List<Vector3>();
	public List<int> Indices = new List<int>();

	private StrideNavMeshBuilder _navMeshBuilder = new();
	private RcNavMeshBuildSettings _navSettings = new();
	private DtNavMesh? _navMesh;
	private List<BepuNavigationBoundingBoxComponent> _boundingBoxes = new();
	private IGame _game;
	private SceneSystem _sceneSystem;
	private InputManager _input;

	private List<ContainerComponent> _containerComponents = new List<ContainerComponent>();
	private int _previousContainerCount = 0;

	public RecastMeshProcessor()
	{
		Order = 20000;
	}

	protected override void OnSystemAdd()
	{
		base.OnSystemAdd();
		_game = Services.GetService<IGame>();
		_sceneSystem = Services.GetService<SceneSystem>();
		_input = Services.GetSafeServiceAs<InputManager>();
	}

	protected override void OnEntityComponentAdding(Entity entity, [NotNull] BepuNavigationBoundingBoxComponent component, [NotNull] BepuNavigationBoundingBoxComponent data)
	{
		_boundingBoxes.Add(data);
		var test = entity.Scene.Entities;
		foreach (var entityTest in test)
		{
			foreach(var child in entityTest.GetChildren())
			{
				var container = child.Get<StaticContainerComponent>();
				if(container != null)
				{
					_containerComponents.Add(container);
				}
			}	
		}
	}

	protected override void OnEntityComponentRemoved(Entity entity, [NotNull] BepuNavigationBoundingBoxComponent component, [NotNull] BepuNavigationBoundingBoxComponent data)
	{

	}

	public override void Update(GameTime time)
	{
		if (_input.IsKeyPressed(Keys.Space))
		{
			Points.Clear();
			Indices.Clear();
			foreach (var container in _containerComponents)
			{
				AddContainerData(container);
			}
			_previousContainerCount = _containerComponents.Count;
			CreateNavMesh();
		}
	}

	public void CreateNavMesh()
	{
		List<float> verts = new();
		// dotrecast wants a list of floats, so we need to convert the list of vectors to a list of floats
		// this may be able to be changed in the StrideGeomProvider class
		foreach (var v in Points)
		{
			verts.Add(v.X);
			verts.Add(v.Y);
			verts.Add(v.Z);
		}
		StrideGeomProvider geom = new StrideGeomProvider(verts, Indices);
		var result = _navMeshBuilder.Build(geom, _navSettings);

		_navMesh = result.NavMesh;

		var tileCount = _navMesh.GetTileCount();
		var tiles = new List<DtMeshTile>();
		for (int i = 0; i < tileCount; i++)
		{
			tiles.Add(_navMesh.GetTile(i));
		}

		List<Vector3> strideVerts = new List<Vector3>();
		
		// TODO: this is just me debugging should remove later
		for (int i = 0; i < tiles.Count; i++)
		{
			for (int j = 0; j < tiles[i].data.verts.Count();)
			{
				strideVerts.Add(
					new Vector3(tiles[i].data.verts[j++], tiles[i].data.verts[j++], tiles[i].data.verts[j++])
					);
			}
		}
		SpawPrefabAtVerts(strideVerts);
	}

	// TODO: this is just me debugging should remove later
	private void SpawPrefabAtVerts(List<Vector3> verts)
	{
		// Make sure the cube is a root asset or else this wont load
		var cube = _game.Content.Load<Model>("Cube");

		foreach (var vert in verts)
		{
			AddMesh(_game.GraphicsDevice, _sceneSystem.SceneInstance.RootScene, vert, cube.Meshes[0].Draw);
		}
	}

	// TODO: this is just me debugging should remove later
	Entity AddMesh(GraphicsDevice graphicsDevice, Scene rootScene, Vector3 position, MeshDraw meshDraw)
	{
		var entity = new Entity { Scene = rootScene, Transform = { Position = position } };
		var model = new Model
		{
		new MaterialInstance
		{
			Material = Material.New(graphicsDevice, new MaterialDescriptor
			{
				Attributes = new MaterialAttributes
				{
					DiffuseModel = new MaterialDiffuseLambertModelFeature(),
					Diffuse = new MaterialDiffuseMapFeature
					{
						DiffuseMap = new ComputeVertexStreamColor()
					},
				}
			})
		},
		new Mesh
		{
			Draw = meshDraw,
			MaterialIndex = 0
		}
		};
		entity.Add(new ModelComponent { Model = model });
		return entity;
	}

	public void AddContainerData(ContainerComponent containerData)
	{
		var shape = containerData.GetShapeData();
		AppendArrays(shape.Points.ToArray(), shape.Indices.ToArray(), containerData.Entity.Transform.WorldMatrix);
	}

	public void AppendArrays(Vector3[] vertices, int[] indices, Matrix objectTransform)
	{
		// Copy vertices
		int vbase = Points.Count;
		for (int i = 0; i < vertices.Length; i++)
		{
			Points.Add(vertices[i]);
		}

		// Copy indices with offset applied
		foreach (int index in indices)
		{
			Indices.Add(index + vbase);
		}
	}
}