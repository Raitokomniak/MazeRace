using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CubePicking : MonoBehaviour {

    public int numberOfPicups;
    public Text countText;

    private Rigidbody rb;
    private int count;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        countText.text = "";
        count = numberOfPicups;
        setText();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count - 1;
            setText();
            if(count == 0)
            {
                countText.text = "Ending is now open";
            }
        }

        if (other.gameObject.CompareTag("Finish") && count == 0)
        {
            //endgame
            Debug.Log("The end");
            countText.text = "The end.";
        }
    }

    void setText()
    {
        countText.text = "Number of objectives left: " + count.ToString();
    }
}
