using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCSend : MonoBehaviour
{
    public OSC osc;
    public GameObject AI_Wall;
    public AI_Manager script;

    // Start is called before the first frame update
    void Start()
    {
        script = AI_Wall.GetComponent<AI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {       
        // send param to Chuck via OSC‚
        OscMessage message = new OscMessage();
        message.address = "/chuck";
        message.values.Add(script.param);
        message.values.Add(script.chaos);
        message.values.Add(script.just);
        osc.Send(message);
    }
}
