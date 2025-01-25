using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    private const float CELL_SIZE = 1;

    [SerializeField]
    private float FLOOR_HEIGHT = 0;

    private int NCells;
    private int NCellsW;

    [SerializeField]
    private float Size = 100;

    [SerializeField]
    private float NoiseFactor = 0.07f;

    [SerializeField, Range(0, 1)]
    private float MountainThreshold = 0.6f;

    [SerializeField]
    private int SpaceshipCornerOffset = 10;

    [SerializeField]
    private int SpaceshipPlaneRadius = 5;

    [SerializeField]
    private Transform ObstaclesParent;

    [SerializeField]
    private Transform OthersParent;

    [SerializeField]
    private GameObject Mountains;

    [SerializeField]
    private GameObject PlayerStart;

    [SerializeField]
    private GameObject Player1;

    [SerializeField]
    private GameObject Player2;

    [SerializeField]
    private GameObject piece;

    [SerializeField]
    private Transform PiecesParent;

    private float RANDOM;

    public enum MapElement { Ground, Mountain, PlayerStart };

    private MapElement[][] Map;

    private List<GameObject> PlayerStarts;

    public List<GameObject> listPlayerStarts { get => PlayerStarts; set => PlayerStarts = value; }

    public void Awake()
    {
        GenerateMap();
    }

    public void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    Debug.Log("Redoing the map without randomizing");

        //    GenerateMap(false);
        //}
        //if (Input.GetKeyUp(KeyCode.Return))
        //{
        //    Debug.Log("Redoing the map");

        //    GenerateMap();
        //}
    }

    public int GetNCells()
    {
        return NCells;
    }

    public float GetFloorHeight()
    {
        return FLOOR_HEIGHT;
    }

    public void Randomize()
    {
        RANDOM = Random.Range(-10000, 10000);
    }

    public void DeleteMap()
    {
        Transform[] mapelements = { ObstaclesParent, /*PiecesParent, AlienNestsParent, AliensParent,*/ OthersParent };
        foreach (Transform element in mapelements)
        {
            foreach (Transform child in element)
            {
                Destroy(child.gameObject);
            }
        }
    }

    //Generate a logical map and create the physical one
    public void GenerateMap(bool randomize = true)
    {
        DeleteMap();
        if (randomize)
        {
            Randomize();
        }
        bool mapisok;

        int limits = 100;
        do
        {
            Randomize();
            PossibleMap();

            mapisok = MapIsOK();

            if (!mapisok)
            {
                Debug.Log("A NOT-CORRECT MAP HAS BEEN GENERATED");
            }
            limits--;

        } while (!mapisok && limits > 0);

        if (!mapisok)
        {
            Debug.Log("THE MAP IS NOT CORRECT");
        }
        else
        {
            Debug.Log("The map has been generated");
        }



        CreatePhysicalMap();
    }

    //Create one possible map
    private void PossibleMap()
    {
        //Create an empty 
        NCells = (int)Size / (int)CELL_SIZE;
        NCellsW = NCells + 30;

        Map = new MapElement[NCellsW][];
        for (int i = 0; i < NCellsW; i++)
        {
            Map[i] = new MapElement[NCells];
            for (int j = 0; j < NCells; j++)
            {
                Map[i][j] = MapElement.Ground;
            }
        }

        //Add Mountains
        for (int i = 0; i < NCellsW; i++)
        {
            for (int j = 0; j < NCells; j++)
            {
                if (i == 0 || i == NCellsW - 1 || j == 0 || j == NCells - 1)
                {
                    Map[i][j] = MapElement.Mountain;
                }
                else if (ShouldCreateMountain(i, j))
                {
                    Map[i][j] = MapElement.Mountain;
                }
            }
        }

        //Add First Spaceship
        OpenHole(SpaceshipCornerOffset, SpaceshipCornerOffset, SpaceshipPlaneRadius);
        Map[SpaceshipCornerOffset][SpaceshipCornerOffset] = MapElement.PlayerStart;

        //Add Second Spaceship
        OpenHole(NCellsW - SpaceshipCornerOffset, NCells - SpaceshipCornerOffset, SpaceshipPlaneRadius);
        Map[NCellsW - SpaceshipCornerOffset][NCells - SpaceshipCornerOffset] = MapElement.PlayerStart;


    }

    private bool ShouldCreateMountain(int x, int y)
    {
        return Mathf.PerlinNoise(x * NoiseFactor + RANDOM, y * NoiseFactor + RANDOM) > MountainThreshold;
    }

    private void CreatePhysicalMap()
    {
        int nplayer = 0;

        PlayerStarts = new List<GameObject>();
        for (int i = 0; i < NCellsW; i++)
        {
            for (int j = 0; j < NCells; j++)
            {
                switch (Map[i][j])
                {
                    case MapElement.Mountain:
                        {
                            GameObject mountain = SpawnMountain(i, j, Mountains);
                            if (!IsInteriorMountain(i, j))
                            {
                                //mountain.GetComponent<Obstacle>().ChangeMesh();
                            }
                            else
                            {
                                Destroy(mountain.GetComponent<NavMeshObstacle>());
                            }
                        }
                        break;
                    case MapElement.PlayerStart:
                        {
                            PlayerStarts.Add(SpawnOther(i, j, FLOOR_HEIGHT + 1, PlayerStart));

                            if (nplayer == 0)
                            {
                                SpawnOther(i + 5, j, FLOOR_HEIGHT, Player1);
                            }
                            else
                            {
                                SpawnOther(i + 5, j, FLOOR_HEIGHT, Player2);
                            }

                            nplayer++;

                        }
                        break;
                }
            }
        }
    }
    public bool IsInteriorMountain(int x, int y)
    {
        bool isinterior = true;

        int nx, ny;

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                nx = x - 1;
                ny = y;
            }
            else if (i == 1)
            {
                nx = x;
                ny = y + 1;
            }
            else if (i == 2)
            {
                nx = x + 1;
                ny = y;
            }
            else
            {
                nx = x;
                ny = y - 1;
            }

            if (!IsInMap(nx, ny))
            {
                return true;
            }

            isinterior = isinterior && IsMountain(nx, ny);
        }

        return isinterior;
    }

    public GameObject SpawnMountain(int x, int y, GameObject obj)
    {
        return Spawn(x, y, FLOOR_HEIGHT, obj, ObstaclesParent);
    }

    public GameObject SpawnPU(int x, int y, float height)
    {
        return Spawn(x, y, height, piece, PiecesParent);
    }

    public GameObject SpawnPU(Vector3 pos)
    {
        Quaternion rot = Quaternion.identity;
        return Instantiate(piece, pos, rot, PiecesParent);
    }

    //public GameObject SpawnAlienNest(int x, int y, float height, GameObject obj)
    //{
    //    return Spawn(x, y, height, obj, AlienNestsParent);
    //}

    //public GameObject SpawnAlien(Vector3 pos, GameObject obj)
    //{
    //    Quaternion rot = Quaternion.identity;
    //    return Instantiate(obj, pos, rot, AliensParent);
    //}

    public GameObject SpawnOther(int x, int y, float height, GameObject obj)
    {
        int rotation = Random.Range(0, 360);
        rotation = rotation - rotation % 90;

        Quaternion rot = Quaternion.Euler(new Vector3(0, rotation, 0));

        return Spawn(x, y, height, rot, obj, OthersParent);
    }

    public float GetOffSet()
    {
        return -((float)NCells / 2) * CELL_SIZE;
    }

    public GameObject Spawn(int x, int y, float height, GameObject obj, Transform Parent)
    {
        return Spawn(x, y, height, Quaternion.identity, obj, Parent);
    }

    public GameObject Spawn(int x, int y, float height, Quaternion rot, GameObject obj, Transform Parent)
    {
        float offset = GetOffSet();
        Vector3 pos = new Vector3(x + offset, height, y + offset);
        return Instantiate(obj, pos, rot, Parent);
    }

    private void OpenHole(int x, int y, int radius)
    {
        float sqrradius = radius * radius;
        for (int i = 0; i < NCellsW; i++)
        {
            for (int j = 0; j < NCells; j++)
            {
                float sqrdistance = (x - i) * (x - i) + (y - j) * (y - j);
                if (sqrdistance < sqrradius)
                {
                    Map[i][j] = MapElement.Ground;
                }
            }
        }
    }

    private bool MapIsOK()
    {
        PathFinder pathfinder = new PathFinder();
        return pathfinder.ThereIsAWay(NCells, Map, new Vector2(SpaceshipCornerOffset, SpaceshipCornerOffset), new Vector2(NCells - SpaceshipCornerOffset, NCells - SpaceshipCornerOffset));
    }

    public bool IsInMap(int x, int y)
    {
        return !(Map == null || x < 0 || x >= NCells || y < 0 || y >= NCells);
    }

    public bool IsGround(int x, int y)
    {
        return IsInMap(x, y) && Map[x][y] == MapElement.Ground;
    }

    public bool IsMountain(int x, int y)
    {
        return IsInMap(x, y) && Map[x][y] == MapElement.Mountain;
    }

    public Vector2 GetRandomPosition()
    {
        int x, y;

        do
        {
            x = Random.Range(0, NCells);
            y = Random.Range(0, NCells);
        } while (!IsGround(x, y));

        return new Vector2(x, y);
    }

    public Vector3 GetRandomGlobalPosition()
    {
        Vector2 logicalpos = GetRandomPosition();

        float offset = GetOffSet();
        return new Vector3(logicalpos.x + offset, FLOOR_HEIGHT, logicalpos.y + offset);
    }

    public GameObject GetClosestPiece(Vector3 pos, List<GameObject> excludeditems)
    {
        GameObject piece = null;
        float MinDistance = float.MaxValue;
        float distance;

        foreach (Transform child in PiecesParent)
        {
            if (!excludeditems.Contains(child.gameObject))
            {
                distance = Vector3.Distance(pos, child.position);
                if (!child.GetComponent<PowerUp>().IsTaken() && distance < MinDistance)
                {
                    piece = child.gameObject;
                    MinDistance = distance;
                }
            }

        }

        return piece;
    }

    public GameObject GetClosestPlayerStart(Vector3 pos)
    {
        GameObject PlayerStart = null;
        float MinDistance = float.MaxValue;
        float distance;

        foreach (GameObject sp in PlayerStarts)
        {
            distance = Vector3.Distance(pos, sp.transform.position);
            if (distance < MinDistance)
            {
                PlayerStart = sp;
                MinDistance = distance;
            }

        }

        return PlayerStart;
    }

    public void CombineObstacles()
    {
        Quaternion oldRot = ObstaclesParent.rotation;
        Vector3 oldPos = ObstaclesParent.position;

        ObstaclesParent.rotation = Quaternion.identity;
        ObstaclesParent.position = Vector3.zero;

        MeshFilter[] filters = ObstaclesParent.GetComponentsInChildren<MeshFilter>();

        Debug.Log("Combining " + filters.Length + " meshes");

        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for (int i = 0; i < filters.Length; i++)
        {
            if (filters[i].transform != ObstaclesParent)
            {
                //combiners[i].subMeshIndex = 0;
                combiners[i].mesh = filters[i].sharedMesh;
                combiners[i].transform = filters[i].transform.localToWorldMatrix;
            }
        }

        finalMesh.CombineMeshes(combiners);
        ObstaclesParent.GetComponent<MeshFilter>().sharedMesh = finalMesh;

        ObstaclesParent.rotation = oldRot;
        ObstaclesParent.position = oldPos;

        for (int i = 0; i < ObstaclesParent.childCount; i++)
        {
            ObstaclesParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
