using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TelaCreditos : MonoBehaviour
{

    int tempo = 0;
    // Start is called before the first frame update
    public float speed;
    void Update()
    {
        LoadCreditos();
        VerificaTempoDeCreditos();
    }
    void LoadCreditos()
    {
        Vector3 posicao;
        posicao = new Vector3(transform.position.x , transform.position.y + speed, transform.position.z) ;
        transform.position = posicao;
    }

    void VerificaTempoDeCreditos()
    {
        if(tempo>=6700)
        {
            SceneManager.LoadScene("Lab1_start");
        }
        tempo++;
    }
}
