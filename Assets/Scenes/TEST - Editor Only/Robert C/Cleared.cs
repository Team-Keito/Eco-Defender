using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using UnityEngine.Analytics;
public class Cleared : MonoBehaviour
{ 
 //   public Light spotlight;
    bool _check1 = false;
    public Renderer cubeRenderer;
    bool isPaused;
    public GameObject Player;
    public TextMeshProUGUI Text;
    public GameObject Text1;
    public GameObject canvas;
    private Color newCubeColor;
    public GameObject cube;
    public Material material1, material2;
    public float FiveSec = 5;
    Vector3 newPosition;
  private  ActivateEvent activated;
    // Start is called before the first frame updateD



    private void Start()
    {
        cubeRenderer = cube.GetComponent<Renderer>();
       // spotlight = GetComponent<Light>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Text= GetComponent<TextMeshProUGUI>();
        canvas.SetActive(false);
        
      
    }

   

    public void OnTriggerEnter(Collider other)
    {
        _check1 = true;
        canvas.SetActive(true);
        // spotlight.intensity = 100;
        newCubeColor = new Color(1f, 1f, 1f, 1f);
        cubeRenderer.material.color = material1.color;

        newPosition = Player.transform.position; 
        if (FiveSec > 0)
        {
            PauseGame();
            FiveSec -= 1 * Time.deltaTime;
           
        }

    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    private void Update()
    {
        
    } 
 }
   