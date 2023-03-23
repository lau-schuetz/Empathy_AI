using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_01 : MonoBehaviour
{
    public GameObject prefab01;
    public GameObject[,] grid01 = new GameObject[25, 50];
    public List<Vector2> coordinates;
    private TextMeshProUGUI text0;
    private int rand;

    void instantiateGrid01()
    {
        // make grid
        for (int row = 0; row < grid01.GetLength(0); row++)
        {
            for (int col = 0; col < grid01.GetLength(1); col++)
            {
                // instantiate new cube
                //GameObject cube = Instantiate(prefab, new Vector3(xStart + (xSpace * (row % columnLength)), yStart + (-ySpace * (col / columnLength))), Quaternion.identity);
                GameObject cube01 = Instantiate(prefab01, new Vector3(col, -row, 0), Quaternion.identity);
                // give it a name
                cube01.name = "cube01" + col;
                // set this as a child of the parent Gameobject
                cube01.transform.parent = this.transform;
                // put into array
                grid01[row, col] = cube01;
                // add cube position to list of all x,y locations 
                coordinates.Add(new Vector2(row, col));
            }
        }
    }

    void move()
    {
        // make grid
        for (int row = 0; row < grid01.GetLength(0); row++)
        {
            for (int col = 0; col < grid01.GetLength(1); col++)
            {
                float cur_x = grid01[row, col].transform.position.x;
                float cur_y = grid01[row, col].transform.position.y;
                grid01[row, col].transform.position = new Vector3(cur_x+25, cur_y-14, +20);
            }
        }

    }

    void change01()
    {
        // randomly pick one of all grid cells and make it 0 or 1
        int x_rand = Random.Range (0, grid01.GetLength(0));
        int y_rand = Random.Range (0, grid01.GetLength(1));
        int rand = Random.Range (0, 2);
        Debug.Log("rand: " + rand);

        text0 = grid01[x_rand, y_rand].GetComponentInChildren<TextMeshProUGUI>();
        //text0.enabled = false;

        if (rand == 1)
        {
            text0.SetText("1");
            //text0.enabled = false;
        }
        else if (rand == 0)
        {
            text0.SetText("0");
            //text0.enabled = true;
        } 

    }

    // Start is called before the first frame update
    void Start()
    {
        instantiateGrid01();
        move();
    }

    // Update is called once per frame
    void Update()
    {     
        change01();
    }
}
