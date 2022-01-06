using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [HideInInspector]
    public JsonReader jsonReader;

    [SerializeField] private float animationDelay;
    [SerializeField] private float animationDuration;
    [SerializeField] private float cubeLifetime;
    [SerializeField] private float jumpPower;
    [SerializeField] private int jumpCount;
    
    [SerializeField] private Transform positionA;
    [SerializeField] private Transform positionB;

    [SerializeField] private TextMeshProUGUI cubeCounter;

    private int _cubeSpawnCount;
    // private bool _canSpawnFromPool;

    private void OnEnable()
    {
        InitSpawner();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            StartCoroutine(SpawnFromPool(animationDelay));
	}

    private void InitSpawner()
	{
        if (jsonReader == null)
            jsonReader = FindObjectOfType<JsonReader>();

        jsonReader.LoadedEvent += GetDataFromJson;

        // _canSpawnFromPool = true;
    }

    private void GetDataFromJson(JsonReader.CubeMovement cubeMovement)
	{
        animationDelay = cubeMovement.movementData[0].animationDelay;
        animationDuration = cubeMovement.movementData[0].animationDuration;
        cubeLifetime = cubeMovement.movementData[0].cubeLifetime;
        jumpPower = cubeMovement.movementData[0].jumpPower;
        jumpCount = cubeMovement.movementData[0].jumpCount;
        
        positionA.position = new Vector3(cubeMovement.movementData[0].pathPosition[0].x, cubeMovement.movementData[0].pathPosition[0].y, cubeMovement.movementData[0].pathPosition[0].z);
        positionB.position = new Vector3(cubeMovement.movementData[0].pathPosition[1].x, cubeMovement.movementData[0].pathPosition[1].y, cubeMovement.movementData[0].pathPosition[1].z);
    }

    private IEnumerator SpawnFromPool(float delay)
    {
        // if (!_canSpawnFromPool) 
        //     yield break;
        
        GameObject cube = ObjectPooler.sharedInstance.GetPooledObject("Player");
        if (cube != null)
        {
            UpdateCube(cube, true);
            IncrementHud();
        }
        
        // _canSpawnFromPool = false;
        
        yield return new WaitForSeconds(delay);
            
        MoveCube(cube);
    }

    private void IncrementHud()
    {
        _cubeSpawnCount++;
        cubeCounter.text = "Spawned Cubes: " + _cubeSpawnCount;
    }

    private void UpdateCube(GameObject cube, bool visible)
    {
        cube.transform.SetPositionAndRotation(positionA.position, Quaternion.identity);
        cube.SetActive(visible);
    }

    private void MoveCube(GameObject cube)
	{
        var position = positionB.position;
        cube.transform.DOMove(position, animationDuration).SetEase(Ease.OutSine);// .OnComplete(ResetSpawn);
        cube.transform.DOJump(position, jumpPower, jumpCount, animationDuration).SetEase(Ease.OutSine);
        
        StartCoroutine(ReturnToPool(cubeLifetime, cube));
    }

    // private void ResetSpawn()
    // {
    //     _canSpawnFromPool = true;
    // }

    private IEnumerator ReturnToPool(float lifetime, GameObject cube)
	{
       yield return new WaitForSeconds(lifetime);

        UpdateCube(cube, false);
    }
}
