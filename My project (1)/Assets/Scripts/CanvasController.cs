using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject alterarProduto2, criarProduto, alterarProduto, visualizarLista, mainMenu;
    public ProdutoController prodController;
    ProductManager productManager;

    LoadProducts loadProducts;

    [Header("Notification")]
    public GameObject nome;
    public GameObject quantia;
    public RectTransform content;

    public GameObject textPrefab;


    private void Awake() {
        productManager = FindObjectOfType<ProductManager>();
        loadProducts = FindObjectOfType<LoadProducts>();
    }

    private void Start() {
        loadProducts.LoadList(nome, quantia, content, textPrefab);
    }
    void Update() {
        Escape();
    }

    public void AlterarProduto(GameObject gameObject)
    {
        productManager.LoadDropdown();
        alterarProduto.SetActive(true);
        productManager.ID.Select();
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
    public void MenuInicial(GameObject page)
    {
        loadProducts.LoadList(nome, quantia, content, textPrefab);
        mainMenu.SetActive(true);
        page.SetActive(false);
    }

    void Escape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject[] pages = new GameObject[4] {alterarProduto2, criarProduto, alterarProduto, visualizarLista};
            foreach (GameObject page in pages)
            {
                if(page.activeInHierarchy && !alterarProduto2.activeInHierarchy){ MenuInicial(page); break;}
                else if(alterarProduto2.activeInHierarchy){ AlterarProduto(alterarProduto2); break;}
                else Application.Quit();
            }
        }
    }
}
