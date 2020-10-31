using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int totalPellets = 0;
    public int score = 0;
    private static int boardWidth = 28;
    private static int boardHeight = 36;
    public GameObject[,] board = new GameObject[boardWidth,boardHeight];

        // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position; 
            if (o.name != "pacman" && o.name != "Nodes" && o.name != "NonNodes" && o.name != "Maze" && o.name != "Pellets") {

                if (o.GetComponent<Tile>() != null) {
                    
                    if (o.GetComponent<Tile>().isPellet || o.GetComponent<Tile>().isSuperPellet) 
                    { 
                        totalPellets ++;
                    }
                }
                board [(int)pos.x, (int)pos.y] = o;
            } else {
                Debug.Log("Found PacMan at: " + pos);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
