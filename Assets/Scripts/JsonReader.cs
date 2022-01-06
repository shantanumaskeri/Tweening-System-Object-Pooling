using System;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [SerializeField] private TextAsset dataJson;

    [Serializable]
    public class PathPosition
    {
        public int x;
        public int y;
        public int z;
    }

    [Serializable]
    public class MovementData
    {
        public PathPosition[] pathPosition;
        public float animationDelay;
        public float animationDuration;
        public float cubeLifetime;
        public float jumpPower;
        public int jumpCount;
    }

    [Serializable]
    public class CubeMovement
	{
        public MovementData[] movementData;
	}

    [SerializeField] private CubeMovement cubeMovement = new CubeMovement();

    public delegate void DataLoaded(CubeMovement cube);
    public event DataLoaded LoadedEvent;

    private void Start()
    {
        InitReader();
    }

    private void InitReader()
	{
        cubeMovement = JsonUtility.FromJson<CubeMovement>(dataJson.text);
        
        if (LoadedEvent != null)
            LoadedEvent.Invoke(cubeMovement);
	}
}
