using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObrabotka : MonoBehaviour
{
    // Start is called before the first frame update
    public int score1, score2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        score1 = (int)GetComponent<Server>().dataAns[4];
        score2 = (int)GetComponent<Server>().dataAns[9];
        if(GetComponent<Server>().dataAns[1]==3)
            GetComponent<Server>().dataAns[6] = 4;

        if (GetComponent<Server>().dataAns[6] == 3)
            GetComponent<Server>().dataAns[1] = 4;

        if ((score1 + score2) >= 320)
        {
            if(score1>score2)
            {
                GetComponent<Server>().dataAns[1] = 3;
                GetComponent<Server>().dataAns[6] = 4;
            }
            if (score2 > score1)
            {
                GetComponent<Server>().dataAns[1] = 4;
                GetComponent<Server>().dataAns[6] = 3;
            }
        }
    }
}
