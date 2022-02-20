using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Modules.Core;

public class NavMeshController : Singleton<NavMeshController>
{
    private NavMeshSurface2d _navMeshSurface2D;

    private void Awake()
    {
        _navMeshSurface2D = GetComponent<NavMeshSurface2d>();
    }

    public void Init()
    {
        _navMeshSurface2D.BuildNavMesh();
    }
}
