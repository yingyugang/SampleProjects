using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class MachineController : MonoBehaviour
{

    #region khai báo biến
    private Animator animator;

    public int sortingLayerID;
    public float timeProduct;
    public float timeCount;
    public float timeLeft;

    private GameObject productPrefabs;
    private GameObject productClone;

    public int idMachinePosition; // Để xác định bánh bay ra từ phía nào của băng chuyền --> chọn điểm changpoint

    private GameObject buttonViewPrefabs;
    private GameObject buttonViewClone;

    private GameObject machineClonePrefabs;
    private GameObject machineClone;

    public int indextMachine; // số hàng chờ
    private GameObject queuePrefabs;
    private GameObject queueClone;

    public int countViewButton; // số lượng loại sản phẩm sản xuất mà máy hiển thị trong mỗi màn.
    public int idMachineType; // xác định loai máy nào được chọn để có thể hiển thị các sản phẩm tương ứng, 
    //giá trị biến này được gán trong phần tạo máy của class ButtonInPopupController

    public string nameMachine; // tên của máy được xác định trong class ButtonInPopupController - phương thức CreatMachine
    public int costMachine; // giá thành của máy được xác định trong class ButtonInPopupController - phương thức CreatMachine
    public int levelMachine;
    public Queue<ProductInfomation> IDProductQueue; // hàng chờ sản xuất
    public List<int> listIndext; // lưu trữ vị trí của loại sản phẩm tương ứng trong mảng cập nhập kết quả trên task(nếu có)

    private GameObject factoryTextPrefabs;
    private GameObject factoryTextClone;
    private GameObject nameMachinePrefabs;
    private GameObject nameMachineClone;

    private GameObject buttonClickPayPrefabs;
    private GameObject buttonClickPayClone;

    public static GameObject machineSelect; // xác định chính xác xem cái nào là máy đang được lựa chọn

    private SpriteRenderer[] TexttureObjects; // xác định tất cả các ảnh của đối tượng để phục vụ cho quá trình chuyển screen bỏ hiển thị ảnh giảm GPU
    public static bool isUpdate; // cho update lại hình ảnh của machine,mục đích giảm CPU

    // Các biến khai báo cho chức năng lỗi máy
    public int rationFail;
    private float countTimeCheck;
    private int timeCheck;
    public bool isFail;
    public bool isRePairting;
    private GameObject rePairPrefabs;
    private GameObject rePairClone;
    private GameObject iconFailPrefabs;
    private GameObject iconFailClone;

    private Vector3 posMouseClick;
    private Vector3 posMouseUpdate;

    public WarningTextView warningFail;
    private float timeCheckWarning;
    private float timeCountWarning;

    public WarningTextView warningLackOfProduct;
    List<int> listIDMaterialOfMachine;
    private float timeCheckWarningLack;
    private float timeCountWarninglack;

    private GameObject commonObject;

    AudioControl audioControl;
    float timeRandomSound;
    float timeCountSound;
    #endregion
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        timeRandomSound = 10f;
        audioControl.PlaySound("May moc chay");

        listIDMaterialOfMachine = new List<int>();
        // timeCheck = Random.Range(10000, 20000) / 1000;
        timeCheck = 20;
        timeCheckWarning = 30.0f;
        timeCheckWarningLack = 30.0f;

        commonObject = GameObject.Find("CommonObject");

        animator = GetComponent<Animator>();
        SetIDLayer(this.sortingLayerID);
        machineClonePrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/MachineClone");

        //timeProduct = 10.0f;
        //print(idMachineType);
        rationFail = FactoryScenesController.listMachineInfomation[idMachineType].listRationFail[levelMachine - 1];
        rePairPrefabs = (GameObject)Resources.Load("Factory/Machine/RePair");

        timeCount = timeProduct;
        productPrefabs = (GameObject)Resources.Load("Factory/Product/Product");

        ReadMaterialForMachine();

        factoryTextPrefabs = (GameObject)Resources.Load("Factory/Queue/TimeLeftMachine");
        nameMachinePrefabs = (GameObject)Resources.Load("Factory/Queue/NameMachine");
        buttonClickPayPrefabs = (GameObject)Resources.Load("Factory/Queue/ButtonClickPay");
    }


    // Update is called once per frame
    void Update()
    {

        posMouseUpdate = Input.mousePosition;
        CreatProduct();
        RandomFail();
        SetStatusMachine();
        SetStatusImageMachine();
        ControlViewHelp();
        CheckStatusLackOfMaterial();
        if (Application.loadedLevelName.Equals("Mission"))
        {
            Destroy(gameObject);
        }
        if (Input.touchCount > 0)
            print(Input.touchCount);
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            //transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
        }
    }

    void RandomFail()
    {
        if (!DialogShop.BoughtItem[11])
        {
            //print("vao day");
            if (MissionData.factoryDataMission.positionUnlock.isCanBreak)
            {
                if (!isFail)
                {
                    if (countTimeCheck >= timeCheck)
                    {
                        int result = Random.Range(0, 10000) % 99;
                        print(result + "_" + rationFail);
                        if (result <= rationFail)
                        {
                            isFail = true;
                            CreatIconFail();
                            countTimeCheck = 0;
                            // timeCheck = Random.Range(10000, 20000) / 1000;
                            timeCheck = 20;
                            warningFail = new WarningTextView(FactoryScenesController.languageHungBV["MACHINEFAIL"], 4);
                        }
                        else
                        {
                            //timeCheck = Random.Range(10000, 20000) / 1000;
                            // print("timeCheck " + timeCheck );
                            timeCheck = 20;
                            countTimeCheck = 0;
                        }
                    }
                    else if (countTimeCheck < timeCheck)
                    {
                        countTimeCheck += Time.deltaTime;
                    }
                    // print(countTimeCheck);
                }
            }
        }
    }
    void OnMouseDown()
    {
        // buttonViewPrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/BTView" + FactoryScenesController.ListProductAllowedOfMachine[idMachineType].Count);
        if (Application.loadedLevelName.Equals("Factory"))
        {
            posMouseClick = Input.mousePosition;
            if (!isFail)
            {
                FactoryScenesController.nameClick = "MachineClick";
                //print(FactoryScenesController.ListProductAllowedOfMachine[idMachineType].Count.ToString()) ;
                buttonViewPrefabs = (GameObject)Resources.Load("Factory/Button/Prefabs/BTView" + FactoryScenesController.ListProductAllowedOfMachine[idMachineType].Count);
                //queueLevel = levelMachine;
                queuePrefabs = (GameObject)Resources.Load("Factory/Queue/ProductQueueLevel" + levelMachine);

                machineSelect = gameObject;
                if (!FactoryScenesController.isHelp)
                    DestroyPopupPre();
            }
            else
            {
                print("isFail" + isFail.ToString());
            }
        }
    }

    void OnMouseUp()
    {
        if (FactoryScenesController.isHelp)
        {
            if (VariableSystem.mission == 1 && CreatAndControlPanelHelp.countClickHelpPanel == 10)
            {
                CreatAndControlPanelHelp.countClickHelpPanel = 11;
                DestroyObjecHelp("CircleHelp");
                DestroyObjecHelp("HandHelp");
                ControlOnMouseUp();
            }
            else if (VariableSystem.mission == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 1)
            {
                CreatAndControlPanelHelp.countClickHelpPanel = 2;
                DestroyObjecHelp("CircleHelp");
                DestroyObjecHelp("HandHelp");
                ControlOnMouseUp();
            }
            else if (VariableSystem.mission == 13 && CreatAndControlPanelHelp.countClickHelpPanel == 1)
            {
                DestroyObjecHelp("CircleHelp");
                DestroyObjecHelp("HandHelp");
                FindAnDestroyChild("IconFail");
                CreatRePair();
                FactoryScenesController.isHelp = false;
                EndHelp();
                //kết thúc hướng dẫn sửa máy
            }
        }
        else
        {
            if (Application.loadedLevelName.Equals("Factory"))
            {
                if (Mathf.Abs(posMouseClick.x - posMouseUpdate.x) > 0.5f && Mathf.Abs(posMouseClick.y - posMouseUpdate.y) > 0.5f)
                {
                    CameraController.isDrag = true;

                }
                else
                {
                    CameraController.IDPosition = idMachinePosition;
                    CameraController.isDrag = false;

                    if (!isFail)
                    {
                        ControlOnMouseUp();
                    }
                    else
                    {
                        if (!isRePairting)
                        {
                            FindAnDestroyChild("IconFail");
                            InvisibleInforMachine();
                            CreatRePair();
                        }
                    }

                }
            }
        }
    }

    void CreatRePair()
    {
        isRePairting = true;
        rePairClone = (GameObject)Instantiate(rePairPrefabs, new Vector3(transform.position.x + 0.1f, transform.position.y + 1, transform.position.z), transform.rotation);
        rePairClone.GetComponent<RePairController>().sortingLayerID = this.sortingLayerID;
        rePairClone.transform.parent = transform;
    }
    void CreatIconFail()
    {
        iconFailPrefabs = (GameObject)Resources.Load("Factory/Machine/IconFail");
        iconFailClone = (GameObject)Instantiate(iconFailPrefabs);
        iconFailClone.transform.parent = transform;
        iconFailClone.name = iconFailPrefabs.name;
        iconFailClone.GetComponent<SpriteRenderer>().sortingLayerID = this.sortingLayerID;
        iconFailClone.GetComponent<SpriteRenderer>().sortingOrder = 1;
        iconFailClone.transform.localPosition = new Vector3(0.25f, 0.74f, 1f);
    }

    // Dùng để ẩn các infor của máy nào đó (nếu đang hiển thị) khi bấm vào 1 máy hỏng để sửa chữa bất kỳ
    void InvisibleInforMachine()
    {
        if (ButtonViewController.animator != null)
            ButtonViewController.animator.SetTrigger("Collape");
        if (ProductQueueController.animator != null)
            ProductQueueController.animator.SetTrigger("Collape");
    }
    void FindAnDestroyChild(string name)
    {
        Transform childObject = transform.Find(name);
        if (childObject != null)
            Destroy(childObject.gameObject);
    }
    void ControlOnMouseUp()
    {
        buttonViewClone = (GameObject)Instantiate(buttonViewPrefabs);
        buttonViewClone.GetComponent<ButtonViewController>().idMachineTypeForReadProduct = idMachineType;
        buttonViewClone.name = "buttonView";
        buttonViewClone.transform.parent = transform;
        buttonViewClone.transform.localPosition = new Vector3(0, 0.5f, -2);

        CreatChildInforMachine(machineClonePrefabs, machineClone, "Cicle", this.gameObject, new Vector3(0, 0, 0));

        queueClone = (GameObject)Instantiate(queuePrefabs, new Vector3(transform.position.x, (transform.position.y + 0.5f), queuePrefabs.transform.position.z),
            transform.rotation);
        queueClone.name = "ProductQueue";
        if (queueClone != null)
            queueClone.GetComponent<ProductQueueController>().CreatProductInQueue();
        queueClone.transform.parent = transform;

        CreatChildInforMachine(factoryTextPrefabs, factoryTextClone, "factoryText", queueClone, new Vector3(-0.9f, -0.8f, 0));
        CreatChildInforMachine(nameMachinePrefabs, nameMachineClone, "nameMachine", queueClone, new Vector3(0.5f, -0.3f, 0));
        CreatChildInforMachine(buttonClickPayPrefabs, buttonClickPayClone, "ButtonPay", queueClone, new Vector3(0, 0, 0));
    }

    void CreatChildInforMachine(GameObject childPrefabs, GameObject childClone, string childName, GameObject childParen, Vector3 localPosition)
    {
        childClone = (GameObject)Instantiate(childPrefabs);
        childClone.name = childName;
        childClone.transform.parent = childParen.transform;
        childClone.transform.localPosition = localPosition;
    }

    void CreatProduct()
    {

        if (IDProductQueue.Count != 0)
        {
            if (!isFail)
                timeCount += Time.deltaTime;
            timeProduct = IDProductQueue.Peek().productionTime;
            if (FactoryScenesController.isHelp)
            {
                timeLeft = timeProduct - 0;
                if (CreatAndControlPanelHelp.countClickHelpPanel == 16)
                {
                    timeLeft = 0;
                }
            }
            else
            {
                timeLeft = timeProduct - timeCount;
            }
            if (timeCount >= timeProduct)
            {
                if (Application.loadedLevelName.Equals("Factory"))
                {
                    audioControl.PlaySound("Lo vi Song");
                    productClone = (GameObject)Instantiate(productPrefabs, new Vector3(transform.position.x - 0.5f, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                    productClone.GetComponent<ProductController>().IDProduct = IDProductQueue.Peek().IDProduct;
                    productClone.GetComponent<ProductController>().sortingLayerID = this.sortingLayerID > 6 ? 10 : 7;
                    productClone.GetComponent<ProductController>().countPosition = (int)(idMachinePosition);
                    productClone.name = productPrefabs.name;
                }
                //tăng chỉ số mục tiêu ở đây
                MissionData.shopDataMission.listProducts[listIndext[0]].currentLevel += 1;
                //  print("Product have index " + listIndext[0] + " haved count: " + MissionData.shopDataMission.listProducts[listIndext[0]].currentLevel);
                //tăng tổng lượng sản phẩm từng loại
                CommonObjectScript.arrayProducts[IDProductQueue.Peek().IDProduct - 7] += 1;
                StorageController.checknewProduct(IDProductQueue.Peek().IDProduct - 7);
                MissionData.factoryDataMission.currentNumber += 1;
                listIndext.RemoveAt(0);
                IDProductQueue.Dequeue();
                if (queueClone != null)
                    queueClone.GetComponent<ProductQueueController>().CreatProductInQueue();


                //for (int i = 0; i < CommonObjectScript.arrayProducts.Length; i++)
                //{
                //    print("product " + i + " : " + CommonObjectScript.arrayProducts[i]);
                //}
                // print("countBread : " + FactoryScenesController.countBread);

                timeCount = 0;
            }
        }
    }
    void DestroyPopupPre()
    {
        GameObject[] CountPre = GameObject.FindGameObjectsWithTag("Count");
        foreach (GameObject gameObject in CountPre)
            if (gameObject != null)
                Destroy(gameObject);
        FindAndDestroyGameObject("Cicle");
        FindAndDestroyGameObject("buttonView");
        FindAndDestroyGameObject("ProductQueue");
    }

    void FindAndDestroyGameObject(string nameGameObject)
    {
        GameObject tempGameObject = GameObject.Find(nameGameObject);
        if (tempGameObject != null)
        {
            Destroy(tempGameObject);
        }
    }
    void SetIDLayer(int sortingLayerID)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject gObject = transforms[i].gameObject;
            if (gObject.GetComponent<SpriteRenderer>() != null)
            {
                gObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
            }
        }
    }

    void SetStatusMachine()
    {
        if (!isFail)
        {

            if (IDProductQueue != null)
            {
                if (IDProductQueue.Count > 0)
                {
                    if (animator != null)
                        animator.Play("may" + (levelMachine) + "_working");
                    if (Application.loadedLevelName.Equals("Factory"))
                    {
                        if (timeCountSound < timeRandomSound)
                        {
                            timeCountSound += Time.deltaTime;
                        }
                        else
                        {
                            RandomSound(30, "May moc chay");
                            timeCountSound = 0;

                        }
                    }
                }
                else
                {

                    if (animator != null)
                        animator.Play("may" + (levelMachine) + "_idle");
                }
            }
        }
        else
        {
            if (!Application.loadedLevelName.Equals("Factory") && commonObject != null && !isRePairting)
                commonObject.GetComponent<CommonObjectScript>().WarningVisible(CommonObjectScript.Button.Factory);
            if (animator != null)
                animator.Play("may" + (levelMachine) + "_i");
            if (timeCountWarning >= timeCheckWarning)
            {
                warningFail = new WarningTextView(FactoryScenesController.languageHungBV["MACHINEFAIL"], 4);
                timeCountWarning = 0;
            }
            else
            {
                timeCountWarning += Time.deltaTime;
            }
        }
    }

    void SetStatusImageMachine()
    {

        TexttureObjects = gameObject.GetComponentsInChildren<SpriteRenderer>();
        if (!Application.isLoadingLevel)
        {
            if (!Application.loadedLevelName.Equals("Factory"))
            {

                SetViewImageMachine(false);
            }
            else
            {

                commonObject.GetComponent<CommonObjectScript>().WarningInvisible(CommonObjectScript.Button.Factory);
                SetViewImageMachine(true);
            }
        }
    }
    void SetViewImageMachine(bool isView)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = isView;
        foreach (SpriteRenderer TexttureObject in TexttureObjects)
        {
            TexttureObject.enabled = isView;
        }
    }


    void CreatObjectHelp(string nameObject, Vector3 vectorScale)
    {
        Transform objectPre = transform.Find(nameObject);
        if (objectPre == null)
        {
            GameObject objectPrefabs = (GameObject)Resources.Load("Help/" + nameObject);
            GameObject objectClone = (GameObject)Instantiate(objectPrefabs,
                new Vector3(transform.position.x, transform.position.y + 1.0f, objectPrefabs.transform.position.z),
               transform.rotation);
            objectClone.transform.parent = gameObject.transform;
            objectClone.transform.localScale = vectorScale;
            objectClone.name = objectPrefabs.name;
        }
    }
    void DestroyObjecHelp(string nameObject)
    {
        Transform objectClonePre = transform.Find(nameObject);
        if (objectClonePre != null)
        {
            Destroy(objectClonePre.gameObject);
        }
    }
    void ControlViewHelp()
    {
        if (FactoryScenesController.isHelp)
        {
            if ((VariableSystem.mission == 1 && CreatAndControlPanelHelp.countClickHelpPanel == 10) ||
                (VariableSystem.mission == 4 && CreatAndControlPanelHelp.countClickHelpPanel == 1) ||
                 (VariableSystem.mission == 13 && CreatAndControlPanelHelp.countClickHelpPanel == 1)
                )
            {
                CreatObjectHelp("CircleHelp", new Vector3(1f, 1f, 1f));
                CreatObjectHelp("HandHelp", new Vector3(1f, 1f, 1f));
            }
        }
    }

    void ReadMaterialForMachine()
    {
        List<ProductForMachine> ListProductAllowedOfMachine = FactoryScenesController.ListProductAllowedOfMachine[idMachineType];
        for (int id = ListProductAllowedOfMachine.Count - 1; id >= 0; id--) // sinh các button tương ứng với các mission và gán id cho các button
        {
            List<int> listIDMaterialTemp = FactoryScenesController.listProductInformation[ListProductAllowedOfMachine[id].iDType - 7].listIDMaterial;

            AddIDMaterial(listIDMaterialTemp[1]);
            if (listIDMaterialTemp[0].Equals(2))
                AddIDMaterial(listIDMaterialTemp[3]);

            //print("máy loại " + idMachineType + " sản xuất các sp có ID: " + ListProductAllowedOfMachine[id]);
            //foreach (int t in listIDMaterialOfMachine)
            //{
            //    print(t);
            //}
        }
    }
    void AddIDMaterial(int id)
    {
        bool isDenyAdd = false;
        foreach (int temp in listIDMaterialOfMachine)
        {
            if (id == temp)
            {
                isDenyAdd = true;
                break;
            }
        }
        if (!isDenyAdd)
        {
            listIDMaterialOfMachine.Add(id);
        }
    }
    void CheckStatusLackOfMaterial()
    {
        bool isDenyCreatWarningLack = false;
        if (timeCountWarninglack >= timeCheckWarningLack)
        {
            foreach (int ID in listIDMaterialOfMachine)
            {
                if (CommonObjectScript.arrayMaterials[ID - 1] <= 0)
                {
                    isDenyCreatWarningLack = true;
                    break;
                }
            }
            if (isDenyCreatWarningLack)
            {
                warningLackOfProduct = new WarningTextView(FactoryScenesController.languageHungBV["LACKOFMATERIAL"], 7);
            }
            else
            {
                if (warningLackOfProduct != null)
                    warningLackOfProduct.RemoveWarning(7);
                else
                {
                    WarningTextView warningLackOfProductTemp = new WarningTextView();
                    warningLackOfProductTemp.RemoveWarning(7);
                }
            }
            timeCountWarninglack = 0;
        }
        else
        {
            timeCountWarninglack += Time.deltaTime;
        }
    }

    void RandomSound(int ratation, string nameSound)
    {
        int result = Random.Range(0, 10000) % 99;
        if (result <= ratation)
        {
            audioControl.PlaySoundInstance(nameSound, false, false);
        }
    }

    public void DestroyPopup()
    {
        Transform buttonView = this.transform.Find("buttonView");
        Transform cicle = this.transform.Find("Cicle");
        Transform productView = this.transform.Find("ProductQueue");
        if (buttonView != null)
            Destroy(buttonView.gameObject);

        if (cicle != null)
            Destroy(cicle.gameObject);

        if (productView != null)
            Destroy(productView.gameObject);
    }

    void EndHelp()
    {
        CommonObjectScript.CompleteGuide();
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().setActiveButton(true, true, true, true, true, true, true, true);
    }
}
