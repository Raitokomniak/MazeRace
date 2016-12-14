using UnityEngine;
using System.Collections;

public class TrapMechanic : MonoBehaviour {
    //public float m_TrapDelay = 1f;          //not working  as intended ????
    public float y1 = -2f;               //lowest point that object has when moving on y axis
    public float y2 = 0f;                //highest point that object has when moving on y axis
    public int TrapStepsOnSecond = 30;      //how much trap moves every frame
    public bool trapFlag = true;            //set false to start trap from up position. true to start from bottom.

    private float x;
    private float z;
    private float yDelta;
    private float y;
    private WaitForSeconds m_TrapWait;
    

    void Start()
    {
        yDelta = y2 - y1;
        //m_TrapWait = new WaitForSeconds(m_TrapDelay);//????
        x = transform.position.x;
        z = transform.position.z;

        transform.localScale = new Vector3(2f, 1.5f, 2f);
        

        if(trapFlag)
        {
            transform.position = new Vector3(x, y1, z);
        }
        else
        {
            transform.position = new Vector3(x, y2, z);
        }
    }

    void Update()
    {
        if (trapFlag == true)
        {
            if (transform.position.y >= y2)
            {
                trapFlag = false;
            }
            else
            {
                //StartCoroutine(TrapUp());
                trapUp();
            }
        }

        else if (trapFlag == false)
        {
            if (transform.position.y <= y1)
            {
                trapFlag = true;
            }
            else
            {
                //StartCoroutine(TrapDown());
                trapDown();
            }

        }
    }


    //private IEnumerator TrapUp()
    private void trapUp()
    {
        y = transform.position.y + (yDelta / TrapStepsOnSecond * (Time.deltaTime * 10)); 
        transform.position = new Vector3(x, y, z);
    //    Debug.Log("up");
        //yield return m_TrapWait;//???
    }

    //private IEnumerator TrapDown()
    private void trapDown()
    {
        y = transform.position.y - (yDelta / TrapStepsOnSecond * (Time.deltaTime * 10));
        transform.position = new Vector3(x, y, z);
       // Debug.Log("down");
        //yield return m_TrapWait;//???
    }
}
