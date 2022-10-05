using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCHenry
{
    Iddle, Follow, House, Final
}

public class NPC_Henry : MonoBehaviour
{
    [Header("Mode Follow Variables")]
    public ModeNPCHenry mode;
    public NavMeshAgent obNv;
    public Transform trPlayer;
    public Vector3 offset, diferenciaVector, posInicial,posfuera;
    public float distancia;
    public Animator obAnim;
    public float contador, temporizador;
    public bool tiempoesperando;
    public int contadorMaximo;
    public bool puedeSeguir;
    public bool finalBueno;
    public bool finalMalo;
    public float speedAtcual,speedMaxima;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] bool didDialogueStart;
    [SerializeField] bool isRange;
    [SerializeField] int index;
    NPC_Dialogue respuestaDada;
    HouseDialogue cabana;
    [SerializeField, TextArea(4, 6)] string[] linesA;
    [SerializeField, TextArea(4, 6)] string[] linesC;
    [SerializeField, TextArea(4, 6)] string[] lines;
    [SerializeField, TextArea(4, 6)] string[] linesAFinal;
    [SerializeField, TextArea(4, 6)] string[] linesCFinal;
    GameObject marker;

    [Header("Anim References")]
    public int numeroAnim;
    public Image blackScreen;

   
   
}
