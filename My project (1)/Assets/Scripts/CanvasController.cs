using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject alterarProduto2, criarProduto, alterarProduto, mainMenu;
    ProdutoController prodController;

    private void Awake() {
        prodController = GetComponent<ProdutoController>();
    }


    public void AlterarProduto()
    {
        alterarProduto2.SetActive(true);
        alterarProduto.SetActive(false);
    }

    public void AddProdutoBanco()
    {
        mainMenu.SetActive(false);
        criarProduto.SetActive(true);
        prodController.inputs[0].Select();
    }
}
