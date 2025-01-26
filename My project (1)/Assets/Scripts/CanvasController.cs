using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject alterarProduto2, criarProduto, alterarProduto, visualizarLista, mainMenu;
    public ProdutoController prodController;
    ProductManager productManager;

    public LoadProducts loadProducts;

    private void Awake() {
        productManager = FindObjectOfType<ProductManager>();
        loadProducts = FindObjectOfType<LoadProducts>();
    }


    public void AlterarProduto(GameObject gameObject)
    {
        alterarProduto.SetActive(true);
        productManager.nomeOrID[0].Select();
        productManager.isActive = true;
        gameObject.SetActive(false);
    }
    public void AlterarProduto2nd()
    {
        alterarProduto.SetActive(false);
        alterarProduto2.SetActive(true);
        productManager.caixasOuQuantidades[0].Select();
    }

    public void AddProdutoBanco()
    {
        mainMenu.SetActive(false);
        criarProduto.SetActive(true);
        prodController.inputs[0].Select();
        prodController.isActive = true;
    }

    public void AddList()
    {
        mainMenu.SetActive(false);
        loadProducts.LoadList();
        visualizarLista.SetActive(true);


    }
    public void MenuInicial()
    {
        mainMenu.SetActive(true);
        criarProduto.SetActive(false);
        alterarProduto.SetActive(false);
        alterarProduto2.SetActive(false);
    }
}
