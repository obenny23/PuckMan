using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    private float pacSize = 0.244f;
    public float speed = 40.0f;
    private Vector2 dest = Vector2.zero;
    private Vector2 nextDest;

    private Node currentNode,targetNode, previousNode;

    public Sprite idleSprite;


    // Start is called before the first frame update
    void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition);

        if (node!= null) {
            currentNode = node;
            Debug.Log(currentNode);
        }

        dest = Vector2.left;
        ChangePosition(dest);

    }

    // Update is called once per frame
    void Update()
    {
        Joystick();
        Move();
        updateOrientation();
        UpdateAnimationState();
    
    }

    void Joystick()
    {
        //Move Up with left key
        if (Input.GetKey(KeyCode.LeftArrow)) {
            ChangePosition(Vector2.left);
        }
        
        //Move Right with arrow key
        else if (Input.GetKey(KeyCode.RightArrow)) {
            ChangePosition(Vector2.right);
        }
        
        //Move Down with arrow key
        else if (Input.GetKey(KeyCode.DownArrow)) {
            ChangePosition(Vector2.down);
        }
            //Move Up with arrow key
        else if (Input.GetKey(KeyCode.UpArrow)){
            ChangePosition(Vector2.up);
        }
    }

    void Move() 
    {
        if (targetNode != currentNode && targetNode != null){
            if (nextDest == dest *-1) {
                dest *= -1;
                Node tempNode = targetNode;
                targetNode = previousNode;
                previousNode = tempNode;
            }

            if (OverShotTarget()) 
            {
                currentNode = targetNode;

                transform.localPosition = currentNode.transform.position;

                GameObject otherPortal = GetPortal(currentNode.transform.position);
                if (otherPortal != null) {
                    transform.localPosition = otherPortal.transform.position;
                    currentNode = otherPortal.GetComponent<Node>();
                }
                
                Node moveToNode = CanMove(nextDest);

                if (moveToNode != null) 
                    dest = nextDest;   

                if (moveToNode == null)
                    moveToNode = CanMove(dest);

                if (moveToNode != null) {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else {
                    dest = Vector2.zero;
                }
            } 

            else {
                transform.localPosition += (Vector3)(dest * speed) * Time.deltaTime;          
            }
        }
    }

    void ChangePosition(Vector2 d) 
    {
        if ( d != dest) {
            nextDest = d;
        }

        if (currentNode != null) 
        {
            Node moveToNode = CanMove(d);

            if (moveToNode != null) {
                dest = d;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    void MoveToNode (Vector2 d) {
        Node moveToNode = CanMove(d);

        if(moveToNode != null) {
            transform.localPosition = moveToNode.transform.position;
            currentNode = moveToNode;
        }
    }
    void updateOrientation() 
    {
        if (dest == Vector2.left){
            transform.localScale = new Vector3(-pacSize,pacSize,1);
            transform.localRotation = Quaternion.Euler(0,0,0);
        } 
        
        else if (dest == Vector2.up){
            transform.localScale = new Vector3(pacSize,pacSize, 1);
            transform.localRotation = Quaternion.Euler(0,0,90);
        } 
        
        else if (dest == Vector2.right){
            transform.localScale = new Vector3(pacSize, pacSize, 1);
            transform.localRotation = Quaternion.Euler(0,0,0);
        } 
        
        else if (dest == Vector2.down){
            transform.localScale = new Vector3(pacSize, pacSize, 1);
            transform.localRotation = Quaternion.Euler(0,0,270);
        }
    }

    void UpdateAnimationState() 
    {
        if (dest == Vector2.zero) 
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }

        else 
        {
            GetComponent<Animator>().enabled = true;
        }
    }

    Node CanMove(Vector2 d){
        Node moveToNode = null;

        for (int i = 0; i < currentNode.neighbors.Length; i++) {
            if (currentNode.validDirections[i] == d) {
                moveToNode = currentNode.neighbors[i];
                break;
            }
        }
        return moveToNode;
    }
    Node GetNodeAtPosition(Vector2 pos) {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard> ().board [(int)pos.x, (int)pos.y];

        if (tile != null) {
            return tile.GetComponent<Node>();
        }

        return null;
    }
    bool OverShotTarget() 
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);
        return nodeToSelf > nodeToTarget;
    }

    float LengthFromNode (Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

    GameObject GetPortal (Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if (tile != null) {
            if( tile.GetComponent<Tile>() != null) {

                if (tile.GetComponent<Tile>().isPortal) {
                    GameObject otherPortal = tile.GetComponent<Tile>().portalReciever;
                    return otherPortal;
                }   
            }
        }
        return null;
    }
}

