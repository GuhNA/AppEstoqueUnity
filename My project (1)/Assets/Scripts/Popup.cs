using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popup;
    public Text mensagem;
    public Button botao;
    public GameObject simOuNao;

    CanvasController canvas;

    bool _permiteEnter = true;

    public bool permiteEnter
    {
        get{return _permiteEnter;}
        set{ _permiteEnter = value;}
    }

    void Awake()
    {
        canvas = FindObjectOfType<CanvasController>();
    }
    private void Start() {
        popup.SetActive(false);
    }
    private void Update() {
        if(popup.activeSelf)
        {
            permiteEnter = false;
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)){ FecharPopup(); Invoke("PermitirEnter",0.25f);}
        }
    }
    public void AbrirPopup(string msg, bool erro, bool ctz)
    {
        if(erro)
                mensagem.color = Color.red;
            else
                mensagem.color = Color.white;
        mensagem.text = msg;
        if(!ctz)
        {
            simOuNao.SetActive(false);
            botao.GetComponent<Transform>().gameObject.SetActive(true);
        }
        else 
        {
            simOuNao.SetActive(true);
            botao.GetComponent<Transform>().gameObject.SetActive(false);
        }
        popup.SetActive(true);
    }
    public void FecharPopup()
    {
        popup.SetActive(false);
        
    }
    void PermitirEnter()
    {
        permiteEnter = true;
         if(canvas.alterarProduto.activeInHierarchy)
            {FindObjectOfType<ProductManager>().ID.Select();}
    }
}
