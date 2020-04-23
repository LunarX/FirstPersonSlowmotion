﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{

    // Tuple.Create(var1, var2, var3, ...)
    // if (NeedNewTargets)
    // var list = GenerateTargets()
    // var newTargets = list.Item1 
    // var directions = lsit.Item2
    // for i in newTargets
    //  stack[direction[i]] = newTargets[i]

    // Variables pour la génération aléatoire des cibles
    private float currentTime;
    private float oldTime;
    private static readonly Vector2[] acceptedDir = { Vector2.left, Vector2.right, Vector2.down, Vector2.up };
    private int randDir;
    private int toolbarInt = 2;
    private string[] toolbarStrings = new string[] { "Lent", "Moyen", "Rapide" };
    public static float interv = 0.6f;

    public static int numbSkel = 0;

    public static Dictionary<int, GameObject> dicSkel = new Dictionary<int, GameObject>();

    public static int indexx = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;        // Temps initial (au lancement du jeu)
    }

    // Update is called once per frame
    void Update()
    {
        if (toolbarInt == 0)
            interv = 2.5f;
        else if (toolbarInt == 1)
            interv = 1.5f;
        else if (toolbarInt == 2)
            interv = 0.6f;

        currentTime = Time.time;
        if (Mathf.Abs(currentTime - oldTime) > interv)      // S'active seul toutes les 'interv' secondes
        {
            randDir = Random.Range(0, 6);                   // Direction de la cible, générée aléatoirement
            oldTime = Time.time;                            // Pour la prochaine cible
            print("RandDir = " + randDir);
            if (randDir < 4)
            {
                GameObject t = GameManager.tg.GenerateSingleTarget(randDir, 3f, 0.2f);
                //GameObject t = TargetGenerator.GenerateSingleTarget(acceptedDir[randDir], 3f, 0.2f);
                GameManager.stacks[randDir].Add(t);     // Evite de faire 4 if

                // Génère les squelettes :
                if (numbSkel < 20)       // Limite du nombre de squelette
                {
                    GameObject s = SkeletonGenerator.CreateSkel(indexx);
                    dicSkel.Add(indexx, s);
                    numbSkel += 1;
                    indexx += 1;
                }
            }
            else
            {
                if (randDir == 4)
                {
                    var ts = GameManager.tg.GenerateDoubleTarget(GameManager.LEFT, GameManager.RIGHT, 3f, 0.2f);
                    GameManager.stacks[GameManager.LEFT].Add(ts.Item1);
                    GameManager.stacks[GameManager.RIGHT].Add(ts.Item2);
                }
                if (randDir == 5)
                {
                    var ts = GameManager.tg.GenerateDoubleTarget(GameManager.UP, GameManager.DOWN, 3f, 0.2f);
                    GameManager.stacks[GameManager.UP].Add(ts.Item1);
                    GameManager.stacks[GameManager.DOWN].Add(ts.Item2);
                }

            }
        }
    }

    // gui, pour modifier la vitesse de génération des cibles
    void OnGUI()
    {
       toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
    }
}