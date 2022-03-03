using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numTentativas;           // Armazena as tentativas válidas da rodada
    private int maxNumTentativas;        // Número máximo de tentativas para Forca ou Salvação
    int score = 0;

    public GameObject letra;             // prefab da letra do Game
    public GameObject centro;            // objeto de texto que indica o centro da tela

    private string palavraSecreta = "";  // palavra a ser descoberta 
    //private string [] palavrasSecretas = new string [] {"carro", "elefante", "futebol"};  // array de palavras secretas (usado no Lab2 - Parte A)

    private int tamanhoPalvaraSecreta;   // tamanho da palavra secreta
    char [] letrasSecretas;              // letras da palavra secreta
    bool [] letrasDescobertas;           // indicador de quais letras foram descobertas

    /*
    Start é chamado antes do update do primerio frame
    a variável centro guarda o objeto centroDaTela que criamos anteriormente
    */
    void Start()
    {
        centro = GameObject.Find("centroDaTela");

        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        CheckTeclado();
    }

    /*
    Cada letra criada é alocada em uma posição diferente (80 char de distancia, centralizando o meio da palavra)
    Todas as letras, enumeradas de 1 a x na hierarquia, são "filhas" do GameObject Canvas
    Com a variável "centro" sendo chamada, agora as letras serão alocadas dentro do espaó que definimos para o "centroDaTela"
    */
    void InitLetras()
    {
        int numLetras = tamanhoPalvaraSecreta;

        for (int i = 0; i < numLetras; i++) 
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i - numLetras/2.0f) * 80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject) Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);

        }
    }

    void InitGame()
    {
        //palavraSecreta = "Elefante";                         // definição da palavra a ser desoberta (usado no Lab1 - ParteA)
        //int numeroAleatorio = Random.Range(0, palavrasSecretas.Length);  // sorteamos um numero aleatorio dentro do numero de palavras do array (usado no Lab2 - Parte A)
        //palavraSecreta = palavrasSecretas[numeroAleatorio];              // sorteamos uma palavra do array

        palavraSecreta = PegaUmaPalavraDoArquivo();
        tamanhoPalvaraSecreta = palavraSecreta.Length;         // determina-se o número de letras da palavra secreta
        palavraSecreta = palavraSecreta.ToUpper();             // transforma-se a palavra em maiúscula
        letrasSecretas = new char[tamanhoPalvaraSecreta];      // instanciamos o array char das letras da palavra
        letrasDescobertas = new bool[tamanhoPalvaraSecreta];   // instanciamos a array bool do indicador de letras acertadas
        letrasSecretas = palavraSecreta.ToCharArray();         // copia-se a palvar no array de letras
    }

    /*
    Toda vez que a letra digitada pertencer a palavra secreta, sua posição é descoberta na palavra e substituída em tela (do "?" para a letra em si)
    A cada nova jogada, a potuação do jogador é atualizada, junto ao seu número de tentativas
    Caso Ultrapasse o número máximo de tentativas sem acertar, o jogador é enviado para a cena de forca
    */
    void CheckTeclado()
    {
        if(Input.anyKeyDown)
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);

            if (letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();

                bool letraEncontrada = false;
                for (int i = 0; i <= tamanhoPalvaraSecreta - 1; i++)
                {
                    if (! letrasDescobertas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if (letrasSecretas[i] == letraTeclada)
                        {
                            letraEncontrada = true;
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            VerificaPalavraDescoberta();
                        }
                    }
                }
                VerificaForca();
                EfeitoSonoroPosTentativa(letraEncontrada);
            }
        }
    }

    void VerificaForca()
    {
        if (numTentativas >= maxNumTentativas)
        {
            SceneManager.LoadScene("Lab1_Forca");
        }
    }

    void EfeitoSonoroPosTentativa(bool letraEncontrada)
    {
        if (letraEncontrada)
        {
            GameObject.Find("rightSoundEffect").GetComponent<AudioSource>().Play();
        }
        else
        {
            GameObject.Find("wrongSoundEffect").GetComponent<AudioSource>().Play();
        }
    }

    // Coloca mensagem criada, de numero de tentativas, pelo maximo de tentativas, no objeto numTentativas que pedimos para encontrar
    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " | " + maxNumTentativas;
    }

    // Mostra na tela qual a score atual do jogador
    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Score " + score;
    }


    // Quando todas as letras da palavra secreta forem descobertas, o jogador vai para a tela de vitória, onde a plavra secreta é revelada por inteiro
    void VerificaPalavraDescoberta()
    {
       bool condicao = true;

       for (int i = 0; i < tamanhoPalvaraSecreta; i++)
       {
           condicao = condicao && letrasDescobertas[i];
       } 

       if (condicao) {
           PlayerPrefs.SetString("palavraOculta", palavraSecreta);
           SceneManager.LoadScene("Lab1_Salvo");
       }
    }

    /*
    lemos o arquivo "palavra", onde estão nossas opções, 
    criamos um vetor das palavras separando por espaços em branco para então sortearmos nossa palavra secreta
    */
    string PegaUmaPalavraDoArquivo()
    {
        TextAsset t1 = (TextAsset)Resources.Load("palavras", typeof(TextAsset));
        string s = t1.text;
        string[] palavras = s.Split(' ');
        int palavraAleatoria = Random.Range(0, palavras.Length);
        return (palavras[palavraAleatoria]);
    }
}
