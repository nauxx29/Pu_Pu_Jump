using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum StairType
{
    Brick,
    Wood,
    Straw
}

public class StairManager : MonoSingleton<StairManager>
{
    private int _score = 0;
    private Dictionary<StairType, ObjectPool<Stair>> stairPools = new Dictionary<StairType, ObjectPool<Stair>>();
    private List<Stair> activeStairList = new List<Stair>();
    private Queue<Vector3> strawStairPositionRecord = new Queue<Vector3>();
    private int lastSpawnScore = 0; 

    [SerializeField] private Stair _brickStair;
    [SerializeField] private Stair _woodStair;
    [SerializeField] private Stair _strawStair;
    [SerializeField] private GameObject _returnLine;

    protected override void Awake()
    {
        base.Awake();
        stairPools.Add(StairType.Brick, InitPool(StairType.Brick));
        stairPools.Add(StairType.Wood, InitPool(StairType.Wood));
        stairPools.Add(StairType.Straw, InitPool(StairType.Straw));
    }

    // Control by GameLogicCenter
    public void Init()
    {
        InitStairs();
    }

    #region ObjectPool
    private ObjectPool<Stair> InitPool(StairType type)
    {
        void ResetAnimToDefault(Stair stair)
        {
            stair.StairAnimator.CrossFade("Idle", 0f);
            stair.StairAnimator.Update(0f);
        }

        Stair prefab = _brickStair;
        switch (type)
        {
            case StairType.Brick:
                prefab = _brickStair;
                break;
            case StairType.Wood:
                prefab = _woodStair;
                break;
            case StairType.Straw:
                prefab = _strawStair;
                break;
        }
        var pool = new ObjectPool<Stair>(() =>
        {
            return Instantiate(prefab);
        }, stair =>
        {
            stair.gameObject.SetActive(true);
        }, stair =>
        {
            if (stair.StairAnimator != null)
            {
                ResetAnimToDefault(stair);
            }
            stair.gameObject.SetActive(false);
        }, stair =>
        {
            Destroy(stair.gameObject);
        }, false, 10, 25);

        return pool;
    }

    public Stair FetchFromPool(StairType type)
    {
        var activeStair = stairPools[type].Get();
        activeStairList.Add(activeStair);
        return activeStair;
    }

    public void ReturnToPool(StairType type, Stair stair)
    {
        stairPools[type].Release(stair);
        activeStairList.Remove(stair);
    }

    #endregion


    public void UpdateScoreAndVibrate(int value)
    {

        void Vibratate()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        Vibration.VibrateAndroid(50);
#elif UNITY_iOS && !UNITY_EDITOR
        Vibration.VibrateIOS(ImpactFeedbackStyle.Light);
#endif
        }

        void CheckIfSpawnStair()
        {
            float spawnTriggerY = LastSpawnY - GameConst.LAST_STAIR_SAFE_DISTANCE;
            if (PlayerManager.Instance.LastRecordY > spawnTriggerY)
            {
                int spawnCount = (int)Mathf.Round((PlayerManager.Instance.LastRecordY - spawnTriggerY) / GameConst.SPAWN_INTERVAL_Y);
                SpawnStairDuplicateWrapper(spawnCount);
            }
        }

        _score += value;
        UiManager.Instance.UpdateScore(_score);

        if (_score - lastSpawnScore >= 5)
        {
            CheckIfSpawnStair();
            lastSpawnScore = _score;
        }

        if (PlayerRunTimeSettingData.VibrationSetting)
        {
            Vibratate();
        }
    }

    // for revive use
    public void UpdateStrawStairPositionRecord(Vector3 position)
    {
        if (strawStairPositionRecord.Count >= 3)
        {
            strawStairPositionRecord.Dequeue();
        }
        strawStairPositionRecord.Enqueue(position);
    }

    public void OnReviveResetStrawStair()
    {
        while (strawStairPositionRecord.Count != 0)
        {
            Vector3 recordPosition = strawStairPositionRecord.Dequeue();
            if (ScreenBoundaries.CheckIfOutOfBounds(recordPosition))
            {
                Stair stair = FetchFromPool(StairType.Straw);
                stair.transform.position = recordPosition;
            }
        }
    }


    public void Restart()
    {
        void ResetObjectPool()
        {
            for (int i = 0; i < activeStairList.Count; i++)
            {
                if (activeStairList[i].gameObject.activeSelf)
                {
                    stairPools[activeStairList[i].Type].Release(activeStairList[i]);
                }
            }
            activeStairList.Clear();
        }

        void ResetScore()
        {
            _score = 0;
            UiManager.Instance.UpdateScore(_score);
        }

        ResetObjectPool();
        ResetScore();
        InitStairs();
    }

    #region StairsSpawn
    [SerializeField] private Transform _lastTutorialBrick;
    public float LastSpawnY { get; private set; }

    public void InitStairs()
    {
        LastSpawnY = _lastTutorialBrick.position.y;
        SpawnStairDuplicateWrapper(GameConst.INIT_STAIR_AMOUNT);
        lastSpawnScore = 0;
    }

    public void SpawnStairDuplicateWrapper(int count)
    {
        for(int i = 0; i < count; i ++)
        {
            SpawnStair();
        }
    }

    private void SpawnStair()
    {
        float randomSpawnAxisX = Random.Range(BoundaryValue.LeftX, BoundaryValue.RightX);
        float randomSpawnAxisY = Random.Range(LastSpawnY + GameConst.SPAWN_INTERVAL_Y - GameConst.SPAWN_MARGIN_Y, LastSpawnY + GameConst.SPAWN_INTERVAL_Y + GameConst.SPAWN_MARGIN_Y);
        Vector3 stairPosition = new Vector3(randomSpawnAxisX, randomSpawnAxisY, 0);
        LastSpawnY = randomSpawnAxisY;

        StairType type = GetRandomType();
        Stair stair = FetchFromPool(type);
        stair.transform.position = stairPosition;
    }

    private StairType GetRandomType()
    {
        int randomType = Random.Range(0, 3);
        return (StairType)randomType;
    }

    #endregion

    private void CheckStairAfterrSuperJump()
    {
        foreach (Stair stair in activeStairList)
        {
            if (stair.transform.position.y < _returnLine.transform.position.y)
            {
                ReturnToPool(stair.Type, stair);
            }
        }
    }
}

