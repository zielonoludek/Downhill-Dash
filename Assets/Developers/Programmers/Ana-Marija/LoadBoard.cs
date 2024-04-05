using UnityEngine;

public class LoadBoard : MonoBehaviour
{
    public GameObject[] boardPrefabs;
    public Transform spawnPoint;
    public GameObject board;
    public MeshFilter boardMesh;
    
    void Start()
    {
        int selectedBoard = PlayerPrefs.GetInt("selectedBoard");
        boardMesh.sharedMesh = Resources.Load<Mesh>("selectedBoard");
        GameObject prefab = boardPrefabs[selectedBoard];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        clone.transform.SetParent(board.transform);
        
    }
}


