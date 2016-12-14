﻿using UnityEngine;
using System.Collections;


public class TrapInteraction : MonoBehaviour {
    public AudioSource m_DamageAudio;
    public AudioClip m_Damage1;
    public AudioClip m_Damage2;
    int rand;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {


        if (other.gameObject.CompareTag("Trap"))
        {
            rand = Random.Range(1, 3);
            Debug.Log(rand);
            if (!m_DamageAudio.isPlaying)
            {
                if(rand == 1)
                {
                    m_DamageAudio.clip = m_Damage1;
                }
                else if(rand == 2)
                {
                    m_DamageAudio.clip = m_Damage2;
                }
                m_DamageAudio.pitch = Random.Range(1f, 1.5f);
                m_DamageAudio.Play();
            }

            //gameObject.SetActive(false);
        }
    }
}
