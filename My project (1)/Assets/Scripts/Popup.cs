using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popup;
    public Text mensagem;
    public Button botao;

    private void Start() {
        popup.SetActive(false);
    }
    private void Update() {
        if(popup.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) FecharPopup();
        }
    }
    public void AbrirPopup(string msg, bool erro)
    {
        mensagem.text = msg;
        if(erro)
            mensagem.color = Color.red;
        else
            mensagem.color = Color.black;
        popup.SetActive(true);
    }
    public void FecharPopup()
    {
        popup.SetActive(false);
        
    }

    
}
