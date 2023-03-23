using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AI_Manager : MonoBehaviour
{
    public OSC osc;
    public float emotion;
    private int maxValuesForAverage = 50;
    public float emotionAvg;
    public float param;
    private Queue<float> averageValues = new Queue<float>();

    // public TextMeshPro emotionText;

    public float xSpace, ySpace;
    public float xStart, yStart;
    public int cur_x, cur_y;
    public float timer = 0f;

    // rgb values and Color for spectrum to color mapping
    private float r;
    private float g;
    private float b;
    Color targetColor;
    Color white = new Color(1f, 1f, 1f);
    Color curColor = new Color (0.0f, 0.0f, 0.0f);
    float timeLeft;

    public GameObject prefab;
    public GameObject[,] grid = new GameObject[50, 100];
    public List<Vector2> coordinates;
    public GameObject Wall01;

    public float chaos = 0f;
    public float just = 1f;

    void instantiateGrid()
    {
        // make grid
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                // instantiate new cube
                //GameObject cube = Instantiate(prefab, new Vector3(xStart + (xSpace * (row % columnLength)), yStart + (-ySpace * (col / columnLength))), Quaternion.identity);
                GameObject cube = Instantiate(prefab, new Vector3(col, -row), Quaternion.identity);
                // set color of sphere
                cube.GetComponent<Renderer>().material.SetColor("_BaseColor", white);
                // give it a name
                cube.name = "cube" + col;
                // set this as a child of the parent Gameobject
                cube.transform.parent = this.transform;
                // put into array
                grid[row, col] = cube;
                // add cube position to list of all x,y locations 
                coordinates.Add(new Vector2(row, col));
            }
        }
    }
    
    // ----------------- color mapping helper function ---------------------- //

    // map spectrum max value to color spectrum from red to violet
    void getColor(float cur_value, float max_value)
    {
        float inc = 6.0f / max_value;
        float x = cur_value * inc;
        r = 0.0f; g = 0.0f; b = 0.0f;
        if ((0 <= x && x <= 1) || (5 <= x && x <= 6)) r = 1.0f;
        else if (4 <= x && x <= 5) r = x - 4;
        else if (1 <= x && x <= 2) r = 1.0f - (x - 1);
        if (1 <= x && x <= 3) g = 1.0f;
        else if (0 <= x && x <= 1) g = x - 0;
        else if (3 <= x && x <= 4) g = 1.0f - (x - 3);
        if (3 <= x && x <= 5) b = 1.0f;
        else if (2 <= x && x <= 3) b = x - 2;
        else if (5 <= x && x <= 6) b = 1.0f - (x - 5);
    }

    void changeColor(Color newColor)
    {
        // loop over cubes in grid
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                grid[row, col].GetComponent<Renderer>().material.SetColor("_BaseColor", newColor);
            }
        }
    }

    void changeColorLifeless(Color newColor)
    {
        // randomly pick one of all grid cells and make it white
        int x_rand = Random.Range (0, grid.GetLength(0));
        int y_rand = Random.Range (0, grid.GetLength(1));
        grid[x_rand, y_rand].GetComponent<Renderer>().material.SetColor("_BaseColor", white);
        // randomly pick one of all grid cells and make it black
        x_rand = Random.Range (0, grid.GetLength(0));
        y_rand = Random.Range (0, grid.GetLength(1));
        grid[x_rand, y_rand].GetComponent<Renderer>().material.SetColor("_BaseColor", Color.black);
        // randomly pick one of all grid cells and make it black
        x_rand = Random.Range (0, grid.GetLength(0));
        y_rand = Random.Range (0, grid.GetLength(1));
        grid[x_rand, y_rand].GetComponent<Renderer>().material.SetColor("_BaseColor", newColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        instantiateGrid();
        osc.SetAddressHandler( "/wek/outputs" , OnReceiveWek );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            chaos = 1;
        }

        if (Input.GetKeyDown("s"))
        {
            chaos = 0;
            just = 0;
            Wall01.SetActive(true);
        }

        // -------------- change color with Webcam input / from FaceOSC ---------------- //

        // average the OSC values
        averageValues.Enqueue(emotion);
        // Check if we exceeded the amount of values we want to average
        if (averageValues.Count > maxValuesForAverage)
        {
            // Max exceeded, dequeue the oldest item
            averageValues.Dequeue();
        }
        // Take the average
        emotionAvg = averageValues.Average();
        Debug.Log("emotion Average: " + emotionAvg);

        // get value from faceOSC
        param = emotionAvg/1.5f + 0.1f;
        Debug.Log("param: " + param);
        // get max value of faceOSC parameter
        float max = 1.1f;
        
        if (timeLeft <= Time.deltaTime)
        {
            // transition complete
            // assign the target color
            prefab.GetComponent<Renderer>().sharedMaterial.color = targetColor;
            getColor(param, max);

            // set color of cubes depending on facial expression / FaceOSC output
            targetColor = new Color(r, g, b);
            timeLeft = 0.1f;
        }
        else
        {
            // transition in progress
            // calculate interpolated color
            if (chaos == 1)
            {
                changeColorLifeless(Color.Lerp(prefab.GetComponent<Renderer>().sharedMaterial.color, targetColor, Time.deltaTime / timeLeft));
            }
            else {
                changeColor(Color.Lerp(prefab.GetComponent<Renderer>().sharedMaterial.color, targetColor, Time.deltaTime / timeLeft));
            }
            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }

    void OnReceiveWek(OscMessage message)
    {
		emotion = message.GetFloat(0);
        Debug.Log("emotion: " + emotion);
	}
}
