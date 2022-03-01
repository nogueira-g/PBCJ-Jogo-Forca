using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    // Start is called before the first frame update
    //variável "score", padrão do Unity, é zerada toda vez que o jogo se inicia
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //chama a primeira cena que criamos
    public void StartMundoGame()
    {
        SceneManager.LoadScene("Lab1");
    }

    // mostra a página de créditos
    public void MostraCreditos()
    {
        SceneManager.LoadScene("Lab1_Creditos");
    }

    //volta ao menu principal
    public void VoltaMenu()
    {
        SceneManager.LoadScene("Lab1_start");
    }
}
