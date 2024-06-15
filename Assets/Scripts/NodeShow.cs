using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Debug = UnityEngine.Debug;
using UnityEngine.XR;
using System.Diagnostics;
using TMPro;

public class NodeShow : MonoBehaviour
{
    [SerializeField] private Sprite dotSprite;
    public GameObject JsonReaderObject;
    public GameObject JsonReaderObject2;
    
    public GameObject Text1;
    public List<GameObject> textlist;
    private RectTransform groupContainer;
    private RectTransform groupContainer2;
    private RectTransform window_Graph;
    [SerializeField] int splitCount = 15;
    public int LineReadererPointsCount = 200;
    public float PositionScale = 0.25f;
    public float RatioHighAndArea = 64;
    public Material linkMaterial;
    private static NodeShow instance;
    private GameObject tooltipGameObject;
    private List<GameObject> GameObjectList;
    public List<GameObject> GameLineObjectList;
    private GameObject[] barlist;
    private GameObject[] linklist;
    public Material Material_0;
    public Material Material_1;
    public Material Material_2;
    public Material Material_3;
    public Material Material_4;
    public Material Material_5;
    public Material Material_6;
    public Material Material_7;
    public Material Material_8;
    public Material Material_9;

    public Material Material_10;
    public Material Material_11;
    public Material Material_12;
    public Material Material_13;
    public Material Material_14;
    public Material Material_15;
    public Material Material_16;
    public Material Material_17;
    public Material Material_18;
    public Material Material_19;

    public Material Material_20;
    public Material Material_21;
    public Material Material_22;
    public Material Material_23;
    public Material Material_24;
    public Material Material_25;
    public Material Material_26;
    public Material Material_27;
    public Material Material_28;
    public Material Material_29;
    public Material Material_30;
    
    // public Material Material_HightLightNode;

    public Material graph2_Material;
    [Range(1, 16)]
    public int textFontSize = 10;
    [Range((float)0.01, (float)0.99)]
    
    public static bool continulFlag = false;
    public bool dragFlag = false;
    public bool reloadFlag = false;
    public String dragNode;

    private NodesStructure[] nodesStructures;
    private LinksStructure[] linksStructures;
   
    private NodesStructure[] nodesStructures2;
    private LinksStructure[] linksStructures2;

    //新的选择交互
    public bool isClicked = false;
    public bool clearShow = false;
    public bool second = false;
    public bool shareCubeShow = false;

    public UnityEngine.XR.InputDevice rightHand_device;
    public UnityEngine.XR.InputDevice leftHand_device;
    Dictionary<string, bool> stateDic;
    public bool HightLight = false;
    public bool HightLightNode = false;
    public bool HightLightLine = false;
    public string selectedNodeName = "";
    public string lastNode = "";
    public string lastLine = "";
    public string selectedLineName = "";

    public string selectedSecondLineName = "";
    public string selectedSecondNodeName = "";
    public string lastSecondLine = "";
    public string lastSecondNode = "";
    public Vector3 MinZ;
    private List<NodePosition> hightlightedNodes;
    private List<NodePosition> hightlightedSecondNodes;
    private List<NodePosition> hightlightedNodesTemp;
    private List<NodePosition> hightlightedSecondNodesTemp;
    public  List<int> hightlightedLinks;
    public  List<int> hightlightedSecondLinks;
    public  List<String> relatedNodes;
    public  List<String> relatedSecondNodes;
    public float lineAlpha = 0.3f;
    public float nodeAlpha = 0.8f;
    public float highLightNodeAlpha = 1f;
    public float disLightNodeAlpha = 0.3f;
    public float highLightLineAlpha = 0.8f;
    public float disLightLineAlpha = 0.1f;
    public float hoveredLineAlpha = 0.8f;
    public float hoveringNodeAlpha = 0.9f;
    public float smallline_value = 80.0f;
    public float smalllineAlpha = 0.5f;
    public bool reset = false;
    public bool resetHightLight = false;
    public bool resetSecondHightLight = false;
    public Vector3 oriCameraPosition;
    public Vector3 oriCameraRotation;
    public Vector3 oppCameraPosition;
    public Vector3 oppCameraRotation;
    public float shift = 100f;
    public float moveSpeed  = 10000f;
    public float rotateSpeed  = 60f;
    GameObject CgameObject;
    public int Count =  -6;
    public float xShift = 400f;
    public float zShift = -100f; 
    public float xTemp= 400f;
    public float sharecubeShift = 450f;
    public int rightHandReset = 0;
    public int allReset = 0;
    public int leftHandReset = 0;
    public int leftGraphReset  = 0;
    public float textAlpha = 1f;
    public float highLightTextAlpha = 1f;
    public float disLightTextAlpha = 0.5f;
    public int TwoFlag = 1;

    // Start is called before the first frame update
    void Start()
    {
        TwoFlag = 1;
        AddTag("Cube", gameObject);
        AddTag("Cube2", gameObject);
        AddTag("shareCube", gameObject);
        AddTag("delCube", gameObject);
        AddTag("Link", gameObject);
        AddTag("Link2", gameObject);
        int layer1 = LayerMask.NameToLayer("Node");
        int layer2 = LayerMask.NameToLayer("Line");
        MinZ = new Vector3(-200f,0f,-200f);
        oriCameraPosition = Camera.main.transform.position;
        oriCameraRotation = Camera.main.transform.rotation.eulerAngles;
        
        oppCameraPosition = new Vector3(oriCameraPosition.x -1000,oriCameraPosition.y,oriCameraPosition.z + shift);
        oppCameraRotation = new Vector3(oriCameraRotation.x, oriCameraRotation.y + 80, oriCameraRotation.z);

        Physics.IgnoreLayerCollision(layer1, layer2, true);
        nodesStructures = JsonReaderObject.GetComponent<JsonReaderTest>().NodesStructures;
        linksStructures = JsonReaderObject.GetComponent<JsonReaderTest>().LinksStructures;

        nodesStructures2 = JsonReaderObject2.GetComponent<JsonReaderTest>().NodesStructures;
        linksStructures2 = JsonReaderObject2.GetComponent<JsonReaderTest>().LinksStructures;

        instance = this;
        barlist = new GameObject[nodesStructures.Length];
        linklist = new GameObject[JsonReaderObject.GetComponent<JsonReaderTest>().LinksStructures.Length];

        groupContainer = transform.GetComponent<RectTransform>();
        groupContainer2 = transform.GetChild(0).GetComponent<RectTransform>();

        GameObjectList = new List<GameObject>();
        GameLineObjectList = new List<GameObject>();

        hightlightedNodes = new List<NodePosition>();
        hightlightedNodesTemp = new List<NodePosition>();

        hightlightedSecondNodes = new List<NodePosition>();
        hightlightedSecondNodesTemp = new List<NodePosition>();

        List<int> hightlightedLinks = new List<int>();
        List<int> hightlightedSecondLinks = new List<int>();

        List<String> relatedNodes = new List<string>();
        List<String> relatedSecondNodes = new List<string>();
        CgameObject = GameObject.Find("XR Origin");

        showGraph(nodesStructures, linksStructures, TwoFlag);

        clearShow = true;
        
    }
    #region addtag
    void AddTag(string tag, GameObject obj)
    {
        if (!isHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(obj);//序列化物体
            SerializedProperty it = tagManager.GetIterator();//序列化属性
            while (it.NextVisible(true))//下一属性的可见性
            {
                if (it.name == "m_TagString")
                {
                    it.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                }
            }
        }
    }
    bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Equals(tag))
                return true;
        }
        return false;
    }
    #endregion

    private GameObject CreateBar(Vector3 graphPosition, float barWidth, float barHight, float Zposition, NodesStructure Node, int i)
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.layer = LayerMask.NameToLayer("Node");
        if(!second){
            gameObject.transform.SetParent(groupContainer, false);
            gameObject.transform.tag = "Cube";
        }
        else{
            gameObject.transform.SetParent(groupContainer2, false);
            gameObject.transform.tag = "Cube2";
            foreach(var obj in GameObjectList)
            {
                if(obj.name.StartsWith(Node.name)){
                    obj.transform.tag = "shareCube";
                    obj.AddComponent<GraphTwoInfor>();
                    obj.GetComponent<GraphTwoInfor>().graph2Value = Node.value.ToString();
                    gameObject.transform.tag = "delCube";
                }
            }
        }
        gameObject.GetComponent<MeshRenderer>().material = null;
        //Set coal, Hy and other materials in the material library of unity
        //Use the contain method to map the name to the specific cube
 
        int a = Node.index % 10;
        switch(a){
            case 0:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_0;
                    break;
                }
            case 1:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_1;
                    break;
                }
            case 2:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_2;
                    break;
                }
            case 3:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_3;
                    break;
                }
            case 4:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_4;
                    break;
                }
            case 5:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_5;
                    break;
                }
            case 6:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_6;
                    break;
                }
            case 7:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_7;
                    break;
                }
            case 8:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_8;
                    break;
                }
            case 9:
                {
                    gameObject.GetComponent<MeshRenderer>().material = Material_9;
                    break;
                }
            default:
                {
                    Debug.Log("set material error");
                    break;
                }
        };
        if(gameObject.transform.tag == "Cube2"){
            gameObject.GetComponent<MeshRenderer>().material = graph2_Material;
        }
        Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
        gameObject.GetComponent<MeshRenderer>().material.color = newColor;
        gameObject.AddComponent<AlphaData>();
        gameObject.GetComponent<AlphaData>().alphaValue = nodeAlpha;

        gameObject.name = Node.name + "@" + Node.value.ToString();
        //Use this method to facilitate subsequent string splitting operations
        gameObject.AddComponent<XRGrabInteractable>();

        //刚体设置
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.angularDrag = float.PositiveInfinity;

        //XRgrab设置
        XRGrabInteractable grabInteractable = gameObject.GetComponent<XRGrabInteractable>();
        grabInteractable.movementType = XRBaseInteractable.MovementType.Kinematic;
        grabInteractable.distanceCalculationMode = XRBaseInteractable.DistanceCalculationMode.TransformPosition;
        grabInteractable.trackRotation = false;
        grabInteractable.throwOnDetach = false;
        grabInteractable.hoverEntered.AddListener(OnHoverEnterNode);
        grabInteractable.hoverExited.AddListener(OnHoverExitNode);

        grabInteractable.selectEntered.AddListener(SelectEnterNode);
        grabInteractable.selectExited.AddListener(SelectExitNode);

        gameObject.AddComponent<NodeText>();
        gameObject.GetComponent<NodeText>().SetText(Node.name);

        return gameObject;
    }

    public void OnHoverExitNode(HoverExitEventArgs arg)
    {
        string hoveredGameObjectName = arg.interactableObject.transform.name;
        if (arg.interactorObject.transform.name.Contains("Right"))
        {
            selectedNodeName = "";
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);
            Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, gameObject.GetComponent<AlphaData>().GetValue());
            gameObject.GetComponent<MeshRenderer>().material.color = newColor;
            GameObject gb = GameObject.Find("Information");
            gb.GetComponent<TextMeshProUGUI>().text = "";
        }

        if(arg.interactorObject.transform.name.Contains("Left"))
        {
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);

                selectedSecondNodeName = "";
                Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, gameObject.GetComponent<AlphaData>().GetValue());
                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                
            
        }
        
    }

    public void OnHoverEnterNode(HoverEnterEventArgs arg)
    {
        string hoveredGameObjectName = arg.interactableObject.transform.name;
        if (arg.interactorObject.transform.name.Contains("Right"))
        {
            selectedLineName = "";
            selectedNodeName = hoveredGameObjectName;
            GameObject gb = GameObject.Find("Information");
            string[] sArray = selectedNodeName.Split('@');
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);
            if(gameObject.transform.tag == "shareCude"){
                gb.GetComponent<TextMeshProUGUI>().text = "Node:"+sArray[0]+"\n"+"Value1:"+sArray[1] +"\n" + "Value2:" + gameObject.GetComponent<GraphTwoInfor>().graph2Value;
            }
            else{
                gb.GetComponent<TextMeshProUGUI>().text = "Node:"+sArray[0]+"\n"+"Value:"+sArray[1];
            }

            Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, hoveringNodeAlpha);
            if(gameObject.GetComponent<AlphaData>().alphaValue != hoveringNodeAlpha){
                gameObject.GetComponent<AlphaData>().SetValue(hoveringNodeAlpha);
            }
            gameObject.GetComponent<MeshRenderer>().material.color = newColor;
        }

        if(arg.interactorObject.transform.name.Contains("Left"))
        {
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);
            selectedSecondNodeName = hoveredGameObjectName;
            selectedSecondLineName = "";
            Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, hoveringNodeAlpha);
            if(gameObject.GetComponent<AlphaData>().alphaValue != hoveringNodeAlpha){
                gameObject.GetComponent<AlphaData>().SetValue(hoveringNodeAlpha);
            }
            gameObject.GetComponent<MeshRenderer>().material.color = newColor;
            
        }

    }

    public void SelectEnterNode(SelectEnterEventArgs arg)
    {
        string selectedGameObjectName = arg.interactableObject.transform.name;
        isClicked = true;
        
        if (arg.interactorObject.transform.name.Contains("Right"))
        {
            selectedLineName = "";
            selectedNodeName = selectedGameObjectName;
            GameObject gb = GameObject.Find("Information");
            string[] sArray = selectedNodeName.Split('@');
            
            if(gb.transform.tag == "shareCude"){
                gb.GetComponent<TextMeshProUGUI>().text = "Node:"+sArray[0]+"\n"+"Value1:"+sArray[1] +"\n" + gb.GetComponent<GraphTwoInfor>().graph2Value;
            }
            else{
                gb.GetComponent<TextMeshProUGUI>().text = "Node:"+sArray[0]+"\n"+"Value:"+sArray[1];
            }

        }
    }
    public void SelectExitNode(SelectExitEventArgs arg)
    {
        string selectedGameObjectName = arg.interactableObject.transform.name;
        isClicked = false;
        if (arg.interactorObject.transform.name.Contains("Right"))
        {
            selectedNodeName = "";
            GameObject gb = GameObject.Find("Information");
            gb.GetComponent<TextMeshProUGUI>().text = "";
        }

    }

    private void showGraph(NodesStructure[] nodesStructures, LinksStructure[] links, int TwoOrThree)
    {
        for (int i = 0; i < nodesStructures.Length; i++)
        {
            float xPosition = (float)nodesStructures[i].x0;
            float yPosition = (float)nodesStructures[i].y0;

            float Width = (float)nodesStructures[i].x1 - xPosition;
            float barHight = (float)nodesStructures[i].y1 - yPosition;
            string Value = nodesStructures[i].value.ToString();
            string name = nodesStructures[i].name;

            yPosition += barHight / 2;
            xPosition += Width / 2;

            GameObjectList.AddRange(
            AddGraphVisual(new Vector3(xPosition, barHight / 2, yPosition), 
            20, 
            barHight+10, 
            20, 
            "name:" + name + " Value:" + Value + " Depth: " + nodesStructures[i].depth.ToString() + " layer: " + nodesStructures[i].layer.ToString(), 
            nodesStructures[i], i, second, TwoOrThree));
        }


        for (int i = 0; i < links.Length; i++)
        {
            GameLineObjectList.AddRange(AddGraphLineVisual("Value:" + (float)links[i].value, links[i]));

        }
    }

    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }

    public void update_inputDynamic_record(InputDevice device, InputFeatureUsage<bool> usage, int i)
    {
      string featureKey = device.characteristics + usage.name;        
        if (!stateDic.ContainsKey(featureKey))
        {   
            stateDic.Add(featureKey, false);
        }
        bool isDown;
        if (device.TryGetFeatureValue(usage, out isDown) && isDown)
        {
            
            if (!stateDic[featureKey])
            {
                stateDic[featureKey] = true;

            }
        }
        else
        {
            if (stateDic[featureKey])
            {
                
                if(i == 0 ){
                    if(HightLight == false){
                        if(selectedNodeName != ""){ 
                            GameObject gameObject = GameObject.Find(selectedNodeName);                                  
                            if(!(gameObject.transform.tag == "Cube2")){
                                hightlightedNodes.Clear();
                                hightlightedLinks.Clear();
                                hightlightedSecondLinks.Clear();
                                hightlightedSecondNodes.Clear();
                                relatedNodes.Clear();
                                relatedSecondNodes.Clear();
                                Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                NodePosition node1 = new NodePosition{
                                    name = selectedNodeName,
                                    OrdXCoord = gameObject.transform.position.x,
                                    OrdYCoord = gameObject.transform.position.y,
                                    OrdZCoord = gameObject.transform.position.z,
                                };
                                hightlightedNodes.Add(node1);

                                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MinZ.z); 
                                
                                string[] sArray = selectedNodeName.Split('@');
                                foreach (LinksStructure gb in linksStructures)
                                {
                                    //Find the link that source node' name is equal with the node name that highlight
                                    if (gb.SourceNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.TargetNode.name);
                                    }

                                    if (gb.TargetNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.SourceNode.name);
                                    }
                                }

                                foreach (GameObject gb in GameObjectList)
                                {
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    bool flagUp = false;
                                    for (int j = 0; j < relatedNodes.Count; j++)
                                    {
                                        if (targetName.Equals(relatedNodes[j]))
                                        {
                                            flagUp = true;
                                            NodePosition node2 = new NodePosition{
                                                name = gb.name.ToString(),
                                                OrdXCoord = gb.transform.position.x,
                                                OrdYCoord = gb.transform.position.y,
                                                OrdZCoord = gb.transform.position.z,                                           
                                            };
                                    
                                            if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                hightlightedNodes.Add(node2);

                                            }
                                            currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;

                                            gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z); 


                                        }
                                        if(!flagUp)
                                        {
                                            if (gb.name.ToString().Equals(selectedNodeName) == false){
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                            }
                                        }
                                    }
                                }
                                lastNode = selectedNodeName;
                                lastLine = "";
                                HightLight = true;
                                HightLightNode = true;
                                HightLightLine = false;
                                clearShow = true;
                                resetHightLight = true;
                            }
                        }

                        else if(selectedLineName != ""){
                            GameObject gameObject = GameObject.Find(selectedLineName);
                            if(!(gameObject.transform.tag == "Link2")){
                                hightlightedNodes.Clear();
                                hightlightedSecondNodes.Clear();
                                hightlightedSecondLinks.Clear();
                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                relatedSecondNodes.Clear();
                                //hightLight through trigger line
                                string sourceNode = gameObject.name.ToString().Split('@')[0];
                                int nameLength = gameObject.name.ToString().Split('@').Length;
                                string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                targetNode = targetNode.Split('&')[1];
                                foreach(LinksStructure link in linksStructures){
                                    if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                        hightlightedLinks.Add(link.index);
                                    }                                    
                                }
                                foreach (GameObject gb in GameObjectList){
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    bool flagUp = false;
                                    if(targetName == sourceNode || targetName == targetNode){
                                        flagUp = true;
                                        NodePosition node2 = new NodePosition{
                                            name = gb.name.ToString(),
                                            OrdXCoord = gb.transform.position.x,
                                            OrdYCoord = gb.transform.position.y,
                                            OrdZCoord = gb.transform.position.z,
                                        };
                                        if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                            hightlightedNodes.Add(node2);
                                            // Debug.Log(node2.name+node2.OrdZCoord);
                                        }
                                        Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                        gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                        gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z); 
   
                                    }
                                    if(!flagUp)
                                    {
                                        if (gb.name.ToString().Equals(selectedNodeName) == false){
                                            Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                        }
                                    }
                                }
                                lastLine = selectedLineName;
                                lastNode = "";
                                HightLightNode = false;
                                HightLightLine = true;
                                HightLight = true; 
                                clearShow = true;
                                resetHightLight = true;    
                            }
 
                        }

                        // reset = true;
                    }

                    else if(HightLight){
                        if(HightLightNode){
                            //重置
                            if(((selectedNodeName!= "" && selectedNodeName == lastNode))||(selectedNodeName == "" && selectedLineName == "")){
                                hightlightedSecondLinks.Clear();
                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                relatedSecondNodes.Clear();
                                hightlightedNodesTemp.Clear();    
                                hightlightedSecondNodesTemp.Clear();                            
                                foreach (NodePosition gb in hightlightedNodes){
        
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    
                                    // Debug.Log(gb.name+gb.OrdZCoord);
                                }
                                foreach (NodePosition gb in hightlightedSecondNodes){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    // Debug.Log(gb.name+gb.OrdZCoord);
                                }
                                foreach (GameObject gb in GameObjectList){
                                    Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                    Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                    gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha =textAlpha;
                                }
                                hightlightedNodes.Clear();
                                hightlightedSecondNodes.Clear();
                                HightLight = false;
                                HightLightLine = false;
                                HightLightNode = false;
                                lastLine = "";
                                lastNode = "";
                                clearShow = true;
                            }

                            //已经是HightLightNode模式下，然后点击了另一个节点
                            else if(selectedNodeName != "" && selectedNodeName != lastNode){
                                GameObject gameObject = GameObject.Find(selectedNodeName);
                                if(!(gameObject.transform.tag == "Cube2"))
                                {
                                    Color currentColor;
                                    Color newColor;
                                    foreach (NodePosition gb in hightlightedNodes){
                                        // if(gb.OrdZCoord!=MinZ.z){
                                            GameObject gameObject2 = GameObject.Find(gb.name);
                                            gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                        // }
                                    }
                                    if(!shareCubeShow){
                                        foreach (NodePosition gb in hightlightedSecondNodes){
                                            // if(gb.OrdZCoord!=MinZ.z){
                                                GameObject gameObject2 = GameObject.Find(gb.name);
                                                gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                            // }
                                        }
                                        hightlightedSecondNodes.Clear();
                                        hightlightedSecondLinks.Clear();
                                    }

                                    foreach (GameObject gb in GameObjectList){
                                        currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                        gb.GetComponent<AlphaData>().SetValue(nodeAlpha);
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                        gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = textAlpha;
                                    }

                                    hightlightedLinks.Clear();
                                    relatedNodes.Clear();
                                    hightlightedNodesTemp.Clear();

                                    if(hightlightedNodes.Exists(targetName=>targetName.name == gameObject.name.ToString())){
                                        hightlightedNodesTemp.Add(hightlightedNodes.Find(targetName=>targetName.name == gameObject.name.ToString()));
                                    }
                                    else{
                                        NodePosition node1 = new NodePosition{
                                        name = selectedNodeName,
                                        OrdXCoord = gameObject.transform.position.x,
                                        OrdYCoord = gameObject.transform.position.y,
                                        OrdZCoord = gameObject.transform.position.z,
                                        };
                                        hightlightedNodesTemp.Add(node1); 
                                    }
                                    currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                    gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                    gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                    gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MinZ.z); 
                                    // if(shareCubeShow){
                                    //     gameObject.transform.position = new Vector3( gameObject.transform.position.x,  gameObject.transform.position.y, sharecubeShift-50f);
                                    // }
                                    // else{
                                    //     gameObject.transform.position = new Vector3( gameObject.transform.position.x,  gameObject.transform.position.y, MinZ.z);
                                    // } 

                                    string[] sArray = selectedNodeName.Split('@');

                                    foreach (LinksStructure gb in linksStructures)
                                    {
                                        //Find the link that source node' name is equal with the node name that highlight
                                        if (gb.SourceNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedLinks.Add(gb.index);
                                            relatedNodes.Add(gb.TargetNode.name);
                                        }

                                        if (gb.TargetNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedLinks.Add(gb.index);
                                            relatedNodes.Add(gb.SourceNode.name);
                                        }
                                    }
                                    
                                    foreach (GameObject gb in GameObjectList)
                                    {
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        bool flagUp = false;
                                        for (int j = 0; j < relatedNodes.Count; j++)
                                        {
                                            if (targetName.Equals(relatedNodes[j]))
                                            {
                                                flagUp = true;
                                                //只记录节点的最开始的位置
                                                if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                    NodePosition node2 = new NodePosition{
                                                        name = gb.name.ToString(),
                                                        OrdXCoord = gb.transform.position.x,
                                                        OrdYCoord = gb.transform.position.y,
                                                        OrdZCoord = gb.transform.position.z,
                                                    };
                                                    hightlightedNodesTemp.Add(node2);
                                                }
                                                else{
                                                    hightlightedNodesTemp.Add(hightlightedNodes.Find(node => node.name == gb.name.ToString()));
                                                }
                                                
                                                // gb.GetComponent<MeshRenderer>().material = Material_HightLightNode;
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                                gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z); 
                                                // gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z);
                                                // if(shareCubeShow){
                                                //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, sharecubeShift-50f);
                                                // }
                                                // else{
                                                //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z);
                                                // } 
                                                // gb.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(gb.transform.position.x, gb.transform.position.y, MinZCoord));
                                            }
                                            if (!flagUp)
                                            {
                                                if (gb.name.ToString().Equals(selectedNodeName) == false){
                                                    currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                    gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                                }
                                            }
                                        }

                                    }
                                    hightlightedNodes.Clear();
                                    hightlightedNodes.AddRange(hightlightedNodesTemp);
                                    clearShow = true;
                                    lastNode = selectedNodeName; 
                                }

                                  
                            }

                            //已经是HightLightNode模式下,点击了一条line，line和节点应该同时高亮,其他不变
                            else if(selectedLineName != "" && selectedLineName != lastLine){
                                GameObject gameObject = GameObject.Find(selectedLineName);
                                string sourceNode = gameObject.name.ToString().Split('@')[0];
                                int nameLength = gameObject.name.ToString().Split('@').Length;
                                string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                targetNode = targetNode.Split('&')[1];
                                // Debug.Log("targetNode"+targetNode);
                                foreach(LinksStructure link in linksStructures){
                                    if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                        // Debug.Log("link.index"+link.index);
                                        hightlightedLinks.Add(link.index);
                                    }                                    
                                }
                                foreach (GameObject gb in GameObjectList){
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    if(targetName == sourceNode || targetName == targetNode){
                                        //  Debug.Log("targetName"+targetName);
                                        if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                            NodePosition node2 = new NodePosition{
                                            name = gb.name.ToString(),
                                            OrdXCoord = gb.transform.position.x,
                                            OrdYCoord = gb.transform.position.y,
                                            OrdZCoord = gb.transform.position.z,
                                            };
                                            hightlightedNodes.Add(node2);
                                            Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                            gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z); 
                                        }
                                    }
                                }
                                clearShow = true;
                                lastLine = selectedLineName;
                                
                            }

                        }
                        
                        //线条高亮模式
                        else if(HightLightLine){
                            if(selectedLineName != ""){
                                GameObject gameObject = GameObject.Find(selectedLineName);
                                    string sourceNode = gameObject.name.ToString().Split('@')[0];
                                    int nameLength = gameObject.name.ToString().Split('@').Length;
                                    string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                    targetNode = targetNode.Split('&')[1];
                                    foreach(LinksStructure link in linksStructures){
                                        if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                            hightlightedLinks.Add(link.index);
                                        }                                    
                                    }
                                    foreach (GameObject gb in GameObjectList){
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        if(targetName == sourceNode || targetName == targetNode){
                                            if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                NodePosition node2 = new NodePosition{
                                                name = gb.name.ToString(),
                                                OrdXCoord = gb.transform.position.x,
                                                OrdYCoord = gb.transform.position.y,
                                                OrdZCoord = gb.transform.position.z,
                                            };
                                                hightlightedNodes.Add(node2);
                                                Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                                gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z); 
                                            }
                                        }
                                    }
                                lastLine = selectedLineName;
                                clearShow = true;
                            }

                            else if(selectedLineName == "" && selectedNodeName != "" && (GameObject.Find(selectedNodeName).transform.tag != "Cube2")){
                                Color currentColor; 
                                Color newColor;
                                foreach (NodePosition gb in hightlightedNodes){
                                    if(gb.OrdZCoord!=MinZ.z){
                                        GameObject gameObject2 = GameObject.Find(gb.name);
                                        gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }
         
                                }

                                if(!shareCubeShow){
                                    foreach (NodePosition gb in hightlightedSecondNodes){
                                        // if(gb.OrdZCoord!=MinZ.z){
                                            GameObject gameObject2 = GameObject.Find(gb.name);
                                            gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                        // }
                                    }
                                    hightlightedSecondNodes.Clear();
                                    hightlightedSecondLinks.Clear();
                                }


                                foreach (GameObject gb in GameObjectList){
                                    currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                    gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                    gb.GetComponent<AlphaData>().SetValue(nodeAlpha);
                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                }

                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                hightlightedNodesTemp.Clear();

                                GameObject gameObject = GameObject.Find(selectedNodeName);
                                if(hightlightedNodes.Exists(targetName=>targetName.name == gameObject.name.ToString())){
                                    hightlightedNodesTemp.Add(hightlightedNodes.Find(targetName=>targetName.name == gameObject.name.ToString()));
                                }
                                else{
                                    NodePosition node1 = new NodePosition{
                                    name = selectedNodeName,
                                    OrdXCoord = gameObject.transform.position.x,
                                    OrdYCoord = gameObject.transform.position.y,
                                    OrdZCoord = gameObject.transform.position.z,
                                    };
                                    hightlightedNodesTemp.Add(node1); 
                                }
                                currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                gameObject.transform.position = new Vector3( gameObject.transform.position.x,  gameObject.transform.position.y, MinZ.z); 
                                // if(shareCubeShow){
                                //     gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, sharecubeShift-50f);
                                // }
                                // else{
                                //     gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MinZ.z);
                                // }     
                                string[] sArray = selectedNodeName.Split('@');

                                foreach (LinksStructure gb in linksStructures)
                                {
                                    //Find the link that source node' name is equal with the node name that highlight
                                    if (gb.SourceNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.TargetNode.name);
                                    }

                                    if (gb.TargetNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.SourceNode.name);
                                    }
                                }
                                //
                                foreach (GameObject gb in GameObjectList)
                                {
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    bool flagUp = false;
                                    for (int j = 0; j < relatedNodes.Count; j++)
                                    {
                                        if (targetName.Equals(relatedNodes[j]))
                                        {
                                            flagUp = true;
                                            //只记录节点的最开始的位置
                                            if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                NodePosition node2 = new NodePosition{
                                                    name = gb.name.ToString(),
                                                    OrdXCoord = gb.transform.position.x,
                                                    OrdYCoord = gb.transform.position.y,
                                                    OrdZCoord = gb.transform.position.z,
                                                };
                                                hightlightedNodesTemp.Add(node2);
                                            }
                                            else{
                                                hightlightedNodesTemp.Add(hightlightedNodes.Find(node => node.name == gb.name.ToString()));
                                            }
                                            
                                            // gb.GetComponent<MeshRenderer>().material = Material_HightLightNode;
                                            currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                            if(i == 0){
                                                gb.transform.position = new Vector3( gb.transform.position.x,  gb.transform.position.y, MinZ.z); 
                                            }
                                            else if(i == 6){
                                                gb.transform.position = new Vector3( gb.transform.position.x,  gb.transform.position.y, gb.transform.position.z); 
                                            }
                                            // if(shareCubeShow){
                                            //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, sharecubeShift-50f);
                                            // }
                                            // else{
                                            //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z);
                                            // }  
                                            // gb.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(gb.transform.position.x, gb.transform.position.y, MinZCoord));
                                        }
                                        if (!flagUp)
                                        {
                                            if (gb.name.ToString().Equals(selectedNodeName) == false){
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                            }
                                        }
                                    }

                                }
                                hightlightedNodes.Clear();
                                hightlightedNodes.AddRange(hightlightedNodesTemp);
                                clearShow = true;
                                lastNode = selectedNodeName; 
                                   
                                HightLightNode = true;
                                HightLightLine = false;

                            }

                            else if(selectedLineName == "" && selectedNodeName == ""){
                                hightlightedSecondLinks.Clear();
                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                hightlightedNodesTemp.Clear();
                                foreach (NodePosition gb in hightlightedNodes){
                                    if(gb.OrdZCoord!=MinZ.z){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }
                                }
                                foreach (NodePosition gb in hightlightedSecondNodes){
                                    GameObject gameObject = GameObject.Find(gb.name);
                                    gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                }
                                foreach (GameObject gb in GameObjectList){
                                    Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                    Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                    gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = textAlpha;
                                }
                                hightlightedSecondNodes.Clear();
                                hightlightedNodes.Clear();
                                HightLight = false;
                                HightLightLine = false;
                                HightLightNode = false;
                                lastLine = "";
                                lastNode = "";
                                clearShow = true;
                            }
                        
                        }
                    }
                }

                if(i == 1){
                    //摄像头重置
                    resetHightLight = true;
                    rightHandReset++;
                }
                if(i == 2){ // 所有重置
                    resetGraph();
                    allReset ++;         
                }

                if( i == 3){
                    if(shareCubeShow){
                        if(HightLight == false){
                            if(selectedSecondNodeName != ""){
                                hightlightedLinks.Clear();
                                relatedSecondNodes.Clear();
                                hightlightedSecondNodes.Clear();
                                hightlightedSecondLinks.Clear();
                                GameObject gameObject = GameObject.Find(selectedSecondNodeName);
                                Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;     
                                NodePosition node1 = new NodePosition{
                                    name = selectedSecondNodeName,
                                    OrdXCoord = gameObject.transform.position.x,
                                    OrdYCoord = gameObject.transform.position.y,
                                    OrdZCoord = gameObject.transform.position.z,
                                };  
                                hightlightedSecondNodes.Add(node1);
                                if(!(gameObject.transform.tag == "shareCube")){
                                    gameObject.transform.position = new Vector3(MinZ.x, gameObject.transform.position.y, gameObject.transform.position.z);  
                                }
                                else{
                                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + zShift); 
                                }
                                // new Vector3( xShift + (gameObject.transform.position.x*xTemp), gameObject.transform.position.y, gameObject.transform.position.z + zShift);
                                // gameObject.transform.position.x 
                                string[] sArray = selectedSecondNodeName.Split('@'); 
                                foreach (LinksStructure gb in linksStructures2)
                                {
                                    //Find the link that source node' name is equal with the node name that highlight
                                    if (gb.SourceNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedSecondLinks.Add(gb.index);
                                        relatedSecondNodes.Add(gb.TargetNode.name);
                                    }

                                    if (gb.TargetNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedSecondLinks.Add(gb.index);
                                        relatedSecondNodes.Add(gb.SourceNode.name);
                                    }
                                }   
                                foreach (GameObject gb in GameObjectList)
                                {
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    bool flagUp = false;
                                    for (int j = 0; j < relatedSecondNodes.Count; j++)
                                    {
                                        if (targetName.Equals(relatedSecondNodes[j]))
                                        {
                                            flagUp = true;
                                            NodePosition node2 = new NodePosition{
                                                name = gb.name.ToString(),
                                                OrdXCoord = gb.transform.position.x,
                                                OrdYCoord = gb.transform.position.y,
                                                OrdZCoord = gb.transform.position.z,                                           
                                            };
                                            // hightlightedNodes.Add(node2);
                                            if(!hightlightedSecondNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                hightlightedSecondNodes.Add(node2);
                                                // Debug.Log(node2.name+node2.OrdZCoord);
                                            }
                                            currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            if(!(gb.transform.tag == "shareCube")){
                                                gb.transform.position = new Vector3(MinZ.x, gb.transform.position.y, gb.transform.position.z);  
                                            }
                                            else{
                                               
                                                gb.transform.position = new Vector3(gb.transform.position.x , gb.transform.position.y, gb.transform.position.z + zShift); 
                                            }
                                           
                                        }
                                        if(!flagUp)
                                        {
                                            if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            }
                                        }
                                    }
                                }
                                clearShow = true;
                                HightLight = true;
                                lastSecondNode = selectedSecondNodeName;
                                lastSecondLine = "";
                                HightLightNode = true;
                                resetSecondHightLight  = true;
                            }
                            
                            else if(selectedSecondLineName != ""){     
                                GameObject gameObject = GameObject.Find(selectedSecondLineName);
                                if(gameObject.transform.tag == "Link2"){
                                    hightlightedLinks.Clear();
                                    hightlightedSecondNodes.Clear();
                                    hightlightedSecondLinks.Clear();
                                    relatedSecondNodes.Clear();
                                    //hightLight through trigger line
                                    string sourceNode = gameObject.name.ToString().Split('@')[0];
                                    int nameLength = gameObject.name.ToString().Split('@').Length;
                                    string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                    targetNode = targetNode.Split('&')[1];
                                    foreach(LinksStructure link in linksStructures2){
                                        if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                            hightlightedSecondLinks.Add(link.index);
                                        }                                    
                                    }
                                    foreach (GameObject gb in GameObjectList){
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        bool flagUp = false;
                                        if(targetName == sourceNode || targetName == targetNode){
                                            flagUp = true;
                                            NodePosition node2 = new NodePosition{
                                                name = gb.name.ToString(),
                                                OrdXCoord = gb.transform.position.x,
                                                OrdYCoord = gb.transform.position.y,
                                                OrdZCoord = gb.transform.position.z,
                                            };
                                            if(!hightlightedSecondNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                hightlightedSecondNodes.Add(node2);
                                                // Debug.Log(node2.name+node2.OrdZCoord);
                                            }
                                            Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            if(!(gb.transform.tag == "shareCube")){
                                                gb.transform.position = new Vector3(MinZ.x, gb.transform.position.y, gb.transform.position.z);  
                                            }
                                            else{
                                                gb.transform.position = new Vector3( gb.transform.position.x, gb.transform.position.y, gb.transform.position.z + zShift); 
                                            }
                                        }
                                        if(!flagUp)
                                        {
                                            if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                                Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            }
                                        }
                                    }
                                    
                                    lastSecondLine = selectedSecondLineName;
                                    lastSecondNode = "";
                                    HightLightNode = false;
                                    HightLightLine = true;
                                    HightLight = true; 
                                    clearShow = true;
                                    resetSecondHightLight  = true;
                                }
    
                            }

                        }

                        else if(HightLight){
                           
                            if(HightLightNode){
                                if((selectedSecondNodeName == lastSecondNode)||(selectedSecondNodeName == "" && selectedSecondLineName == "")){
                                    hightlightedLinks.Clear();
                                    hightlightedSecondLinks.Clear();
                                    relatedSecondNodes.Clear();
                                    hightlightedSecondNodesTemp.Clear();  

                                    foreach (NodePosition gb in hightlightedSecondNodes){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }
                                    foreach (NodePosition gb in hightlightedNodes){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }
                                    foreach (GameObject gb in GameObjectList){
                                        Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    }
                                    hightlightedSecondNodes.Clear();
                                    hightlightedNodes.Clear();
                                    HightLight = false;
                                    HightLightLine = false;
                                    HightLightNode = false;
                                    lastSecondLine = "";
                                    lastSecondNode = "";
                                    clearShow = true;
                                }

                                else if(selectedSecondNodeName != "" && selectedSecondNodeName != lastSecondNode){
                                    Color currentColor;
                                    Color newColor;
                                    foreach (NodePosition gb in hightlightedSecondNodes){
                                        GameObject gameObject2 = GameObject.Find(gb.name);
                                        gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }

                                    foreach (NodePosition gb in hightlightedNodes){
                                        GameObject gameObject3 = GameObject.Find(gb.name);
                                        gameObject3.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }
                                    foreach (GameObject gb in GameObjectList){
                                        currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    }
                                    hightlightedNodes.Clear();
                                    hightlightedSecondLinks.Clear();
                                    relatedSecondNodes.Clear();
                                    hightlightedSecondNodesTemp.Clear();
                                    GameObject gameObject = GameObject.Find(selectedSecondNodeName);
                                    if(hightlightedSecondNodes.Exists(targetName=>targetName.name == gameObject.name.ToString())){
                                        hightlightedSecondNodesTemp.Add(hightlightedSecondNodes.Find(targetName=>targetName.name == gameObject.name.ToString()));
                                    }
                                    else{
                                        NodePosition node1 = new NodePosition{
                                        name = selectedSecondNodeName,
                                        OrdXCoord = gameObject.transform.position.x,
                                        OrdYCoord = gameObject.transform.position.y,
                                        OrdZCoord = gameObject.transform.position.z,
                                        };
                                        hightlightedSecondNodesTemp.Add(node1); 
                                    }
                                    currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                    gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                    gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                    if(!(gameObject.transform.tag == "shareCube")){
                                        gameObject.transform.position = new Vector3(MinZ.x, gameObject.transform.position.y, gameObject.transform.position.z);  
                                    }
                                    else{
                                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + zShift); 
                                    }
                                    string[] sArray = selectedNodeName.Split('@');

                                    foreach (LinksStructure gb in linksStructures2)
                                    {
                                        //Find the link that source node' name is equal with the node name that highlight
                                        if (gb.SourceNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedSecondLinks.Add(gb.index);
                                            relatedNodes.Add(gb.TargetNode.name);
                                        }

                                        if (gb.TargetNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedSecondLinks.Add(gb.index);
                                            relatedSecondNodes.Add(gb.SourceNode.name);
                                        }
                                    }
                                    //
                                    foreach (GameObject gb in GameObjectList)
                                    {
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        bool flagUp = false;
                                        for (int j = 0; j < relatedSecondNodes.Count; j++)
                                        {
                                            if (targetName.Equals(relatedSecondNodes[j]))
                                            {
                                                flagUp = true;
                                                //只记录节点的最开始的位置
                                                if(!hightlightedSecondNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                    NodePosition node2 = new NodePosition{
                                                        name = gb.name.ToString(),
                                                        OrdXCoord = gb.transform.position.x,
                                                        OrdYCoord = gb.transform.position.y,
                                                        OrdZCoord = gb.transform.position.z,
                                                    };
                                                    hightlightedSecondNodesTemp.Add(node2);
                                                }
                                                else{
                                                    hightlightedSecondNodesTemp.Add(hightlightedSecondNodes.Find(node => node.name == gb.name.ToString()));
                                                }
                                                
                                                // gb.GetComponent<MeshRenderer>().material = Material_HightLightNode;
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                if(!(gb.transform.tag == "shareCube")){
                                                    gb.transform.position = new Vector3(MinZ.x, gb.transform.position.y, gb.transform.position.z);  
                                                }
                                                else{
                                                    gb.transform.position = new Vector3( gb.transform.position.x, gb.transform.position.y, gb.transform.position.z + zShift); 
                                                }
                                            }
                                            if (!flagUp)
                                            {
                                                if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                                    currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                    gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                }
                                            }
                                        }

                                    }
                                    hightlightedSecondNodes.Clear();
                                    hightlightedSecondNodes.AddRange(hightlightedSecondNodesTemp);
                                    clearShow = true;
                                    lastSecondNode = selectedSecondNodeName; 
            
                                    
                                }

                                else if(selectedSecondLineName != "" && lastSecondLine != selectedSecondLineName){
                                    GameObject gameObject = GameObject.Find(selectedSecondLineName);
                                        string sourceNode = gameObject.name.ToString().Split('@')[0];
                                        int nameLength = gameObject.name.ToString().Split('@').Length;
                                        string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                        targetNode = targetNode.Split('&')[1];
                                        foreach(LinksStructure link in linksStructures2){
                                            if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                                hightlightedSecondLinks.Add(link.index);
                                            }                                    
                                        }
                                        foreach (GameObject gb in GameObjectList){
                                            string targetName = gb.name.ToString().Split('@')[0];
                                            if(targetName == sourceNode || targetName == targetNode){
                                                if(!hightlightedSecondNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                    NodePosition node2 = new NodePosition{
                                                    name = gb.name.ToString(),
                                                    OrdXCoord = gb.transform.position.x,
                                                    OrdYCoord = gb.transform.position.y,
                                                    OrdZCoord = gb.transform.position.z,
                                                    };
                                                    hightlightedSecondNodes.Add(node2);
                                                    Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                    Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                    gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                    if(!(gb.transform.tag == "shareCube")){
                                                        gb.transform.position = new Vector3(MinZ.x, gb.transform.position.y, gb.transform.position.z);  
                                                    }
                                                    else{
                                                        gb.transform.position = new Vector3( xShift + (gb.transform.position.x*xTemp), gb.transform.position.y, gb.transform.position.z + zShift); 
                                                    }
                                                }
                                            }
                                        }
                                    clearShow = true;
                                    lastSecondLine = selectedSecondLineName;
                                    
                                }
                            }

                            else if(HightLightLine){
                               
                                if(selectedSecondLineName != "" && selectedSecondLineName != lastSecondLine){
                                    GameObject gameObject = GameObject.Find(selectedSecondLineName);
                                    string sourceNode = gameObject.name.ToString().Split('@')[0];
                                    int nameLength = gameObject.name.ToString().Split('@').Length;
                                    string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                    targetNode = targetNode.Split('&')[1];
                                    foreach(LinksStructure link in linksStructures2){
                                        if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                            hightlightedSecondLinks.Add(link.index);
                                        }                                    
                                    }
                                    foreach (GameObject gb in GameObjectList){
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        if(targetName == sourceNode || targetName == targetNode){
                                            if(!hightlightedSecondNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                NodePosition node2 = new NodePosition{
                                                name = gb.name.ToString(),
                                                OrdXCoord = gb.transform.position.x,
                                                OrdYCoord = gb.transform.position.y,
                                                OrdZCoord = gb.transform.position.z,
                                                };
                                                hightlightedSecondNodes.Add(node2);
                                                Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                if(!(gb.transform.tag == "shareCube")){
                                                    gb.transform.position = new Vector3(MinZ.x, gb.transform.position.y, gb.transform.position.z);  
                                                }
                                                else{
                                                    gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z + zShift); 
                                                }
                                            }
                                        }
                                    }
                                    lastSecondLine = selectedSecondLineName;
                                    clearShow = true;
                                }

                                else if(selectedSecondLineName == "" && selectedSecondNodeName !=""){
                                  
                                    Color currentColor; 
                                    Color newColor;
                                    foreach (NodePosition gb in hightlightedSecondNodes){
                                        GameObject gameObject2 = GameObject.Find(gb.name);
                                        gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }

                                    foreach (NodePosition gb in hightlightedNodes){
                                        GameObject gameObject2 = GameObject.Find(gb.name);
                                        gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }

                                    foreach (GameObject gb in GameObjectList){
                                        currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    }
                                    hightlightedSecondLinks.Clear();
                                    relatedSecondNodes.Clear();
                                    hightlightedSecondNodesTemp.Clear();

                                    GameObject gameObject = GameObject.Find(selectedSecondNodeName);
                                    if(hightlightedSecondNodes.Exists(targetName=>targetName.name == gameObject.name.ToString())){
                                        hightlightedSecondNodesTemp.Add(hightlightedSecondNodes.Find(targetName=>targetName.name == gameObject.name.ToString()));
                                    }
                                    else{
                                        NodePosition node1 = new NodePosition{
                                        name = selectedSecondNodeName,
                                        OrdXCoord = gameObject.transform.position.x,
                                        OrdYCoord = gameObject.transform.position.y,
                                        OrdZCoord = gameObject.transform.position.z,
                                        };
                                        hightlightedSecondNodesTemp.Add(node1); 
                                    }
                                    currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                    gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                    gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                    if(!(gameObject.transform.tag == "shareCube")){
                                        gameObject.transform.position = new Vector3(MinZ.x, gameObject.transform.position.y, gameObject.transform.position.z);  
                                    }
                                    else{
                                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + zShift); 
                                    }  
                                    string[] sArray = selectedNodeName.Split('@');
                                    foreach (LinksStructure gb in linksStructures2)
                                    {
                                        //Find the link that source node' name is equal with the node name that highlight
                                        if (gb.SourceNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedLinks.Add(gb.index);
                                            relatedSecondNodes.Add(gb.TargetNode.name);
                                        }

                                        if (gb.TargetNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedLinks.Add(gb.index);
                                            relatedSecondNodes.Add(gb.SourceNode.name);
                                        }
                                    }
                                    //
                                    foreach (GameObject gb in GameObjectList)
                                    {
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        bool flagUp = false;
                                        for (int j = 0; j < relatedSecondNodes.Count; j++)
                                        {
                                            if (targetName.Equals(relatedSecondNodes[j]))
                                            {
                                                flagUp = true;
                                                //只记录节点的最开始的位置
                                                if(!hightlightedSecondNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                    NodePosition node2 = new NodePosition{
                                                        name = gb.name.ToString(),
                                                        OrdXCoord = gb.transform.position.x,
                                                        OrdYCoord = gb.transform.position.y,
                                                        OrdZCoord = gb.transform.position.z,
                                                    };
                                                    hightlightedSecondNodesTemp.Add(node2);
                                                }
                                                else{
                                                    hightlightedSecondNodesTemp.Add(hightlightedSecondNodes.Find(node => node.name == gb.name.ToString()));
                                                }
                                                
                                                // gb.GetComponent<MeshRenderer>().material = Material_HightLightNode;
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                if(!(gb.transform.tag == "shareCube")){
                                                    gb.transform.position = new Vector3(MinZ.x, gb.transform.position.y, gb.transform.position.z);  
                                                }
                                                else{
                                                    gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z + zShift); 
                                                }
                                            }
                                            if (!flagUp)
                                            {
                                                if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                                    currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                    gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                }
                                            }
                                        }

                                    }
                                    hightlightedSecondNodes.Clear();
                                    hightlightedSecondNodes.AddRange(hightlightedSecondNodesTemp);
                                    clearShow = true;
                                    lastSecondNode = selectedSecondNodeName; 
                                    HightLightNode = true;
                                    HightLightLine = false;
                                }

                                else if(selectedSecondLineName == "" && selectedSecondNodeName == ""){
                                   
                                    hightlightedSecondLinks.Clear();
                                    hightlightedLinks.Clear();
                                    relatedSecondNodes.Clear();
                                    hightlightedSecondNodesTemp.Clear();
                                    foreach (NodePosition gb in hightlightedSecondNodes){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }

                                    foreach (NodePosition gb in hightlightedNodes){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }

                                    foreach (GameObject gb in GameObjectList){
                                        Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    }
                                    hightlightedSecondNodes.Clear();
                                    HightLight = false;
                                    HightLightLine = false;
                                    HightLightNode = false;
                                    lastSecondLine = "";
                                    lastSecondNode = "";
                                    clearShow = true;
                                }
                                
                            }
                            
                        }
    
                    }
             
                }
                if( i == 4){
                    //2D和3D切换
                    hightlightedNodes.Clear();
                    hightlightedLinks.Clear();
                    relatedNodes.Clear();
                    hightlightedNodesTemp.Clear();
                    hightlightedSecondNodes.Clear();
                    hightlightedSecondLinks.Clear();
                    relatedSecondNodes.Clear();
                    hightlightedSecondNodesTemp.Clear();
                    // reset = resetHightLight;
                    HightLight = false;
                    HightLightLine = false;
                    HightLightNode = false;
                    lastLine = "";
                    lastNode = "";
                    lastSecondLine = "";
                    lastSecondNode = "";
                    groupContainer2.anchoredPosition3D = new Vector3(0, 0, 0);
                    groupContainer2.localRotation = Quaternion.Euler(0, 0, 0);
                    foreach(GameObject gb in GameObjectList){
                        Destroy(gb);
                    }
                    foreach (GameObject line in GameLineObjectList)
                    {
                        Destroy(line);
                    }

                    GameObjectList.Clear();
                    GameLineObjectList.Clear();
                    second = false;
                    if(TwoFlag == 0){
                        TwoFlag = 1;
                    }
                    else{
                        TwoFlag = 0;
                    }
                    showGraph(nodesStructures, linksStructures, TwoFlag);
                    foreach (GameObject line in GameLineObjectList)
                    {
                        Destroy(line);
                    }
                    second = false;
                    for (int te = 0; te < linksStructures.Length; te++)
                    {
                        GameLineObjectList.AddRange(AddGraphLineVisual("Value:" + (float)linksStructures[te].value, linksStructures[te]));
                    }
                    // second = true;
                    // for (int i = 0; i < linksStructures2.Length; i++)
                    // {
                    //     GameLineObjectList.AddRange(AddGraphLineVisual("Value:" + (float)linksStructures2[i].value, linksStructures2[i]));
                    // }
                    Count = Time.frameCount;
                }

                if( i == 5){
                    if(shareCubeShow == false){
                        foreach(GameObject gb in GameObjectList){
                            if(gb.transform.tag == "shareCube"){
                                gb.transform.position = new Vector3(gb.transform.position.x,gb.transform.position.y, sharecubeShift);
                            }
                            if(gb.transform.tag =="Cube2"){
                                gb.transform.position = new Vector3(gb.transform.position.x,gb.transform.position.y,gb.transform.position.z+350f);
                            }
                        }
                        shareCubeShow = true;
                        clearShow = true;
                    }

                    else if(shareCubeShow == true){
                        resetGraph();
                        shareCubeShow = false;
                        clearShow = true;
                    }
                    leftGraphReset++;
                }

                if( i == 6 ){
                    if(HightLight == false){
                        if(selectedSecondNodeName != ""){ 
                            GameObject gameObject = GameObject.Find(selectedSecondNodeName);                                  
                            if(!(gameObject.transform.tag == "Cube2")){
                                hightlightedNodes.Clear();
                                hightlightedLinks.Clear();
                                hightlightedSecondLinks.Clear();
                                hightlightedSecondNodes.Clear();
                                relatedNodes.Clear();
                                relatedSecondNodes.Clear();
                                Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                NodePosition node1 = new NodePosition{
                                    name = selectedSecondNodeName,
                                    OrdXCoord = gameObject.transform.position.x,
                                    OrdYCoord = gameObject.transform.position.y,
                                    OrdZCoord = gameObject.transform.position.z,
                                };
                                hightlightedNodes.Add(node1);

                                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z); 
                                
                                string[] sArray = selectedSecondNodeName.Split('@');
                                foreach (LinksStructure gb in linksStructures)
                                {
                                    //Find the link that source node' name is equal with the node name that highlight
                                    if (gb.SourceNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.TargetNode.name);
                                    }

                                    if (gb.TargetNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.SourceNode.name);
                                    }
                                }

                                foreach (GameObject gb in GameObjectList)
                                {
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    bool flagUp = false;
                                    for (int j = 0; j < relatedNodes.Count; j++)
                                    {
                                        if (targetName.Equals(relatedNodes[j]))
                                        {
                                            flagUp = true;
                                            NodePosition node2 = new NodePosition{
                                                name = gb.name.ToString(),
                                                OrdXCoord = gb.transform.position.x,
                                                OrdYCoord = gb.transform.position.y,
                                                OrdZCoord = gb.transform.position.z,                                           
                                            };
                                    
                                            if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                hightlightedNodes.Add(node2);

                                            }
                                            currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;

                                            gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z); 


                                        }
                                        if(!flagUp)
                                        {
                                            if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                            }
                                        }
                                    }
                                }
                                lastNode = selectedSecondNodeName;
                                lastLine = "";
                                HightLight = true;
                                HightLightNode = true;
                                HightLightLine = false;
                                clearShow = true;
                                resetHightLight = true;
                            }
                        }

                        else if(selectedSecondLineName != ""){
                            GameObject gameObject = GameObject.Find(selectedSecondLineName);
                            if(!(gameObject.transform.tag == "Link2")){
                                hightlightedNodes.Clear();
                                hightlightedSecondNodes.Clear();
                                hightlightedSecondLinks.Clear();
                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                relatedSecondNodes.Clear();
                                //hightLight through trigger line
                                string sourceNode = gameObject.name.ToString().Split('@')[0];
                                int nameLength = gameObject.name.ToString().Split('@').Length;
                                string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                targetNode = targetNode.Split('&')[1];
                                foreach(LinksStructure link in linksStructures){
                                    if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                        hightlightedLinks.Add(link.index);
                                    }                                    
                                }
                                foreach (GameObject gb in GameObjectList){
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    bool flagUp = false;
                                    if(targetName == sourceNode || targetName == targetNode){
                                        flagUp = true;
                                        NodePosition node2 = new NodePosition{
                                            name = gb.name.ToString(),
                                            OrdXCoord = gb.transform.position.x,
                                            OrdYCoord = gb.transform.position.y,
                                            OrdZCoord = gb.transform.position.z,
                                        };
                                        if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                            hightlightedNodes.Add(node2);
                                        
                                        }
                                        Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                        gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                        gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z); 
   
                                    }
                                    if(!flagUp)
                                    {
                                        if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                            Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                        }
                                    }
                                }
                                lastLine = selectedSecondLineName;
                                lastNode = "";
                                HightLightNode = false;
                                HightLightLine = true;
                                HightLight = true; 
                                clearShow = true;
                                resetHightLight = true;    
                            }
 
                        }

                        // reset = true;
                    }

                    else if(HightLight){
                        if(HightLightNode){
                            //重置
                            if(((selectedSecondNodeName!= "" && selectedSecondNodeName == lastNode))||(selectedSecondNodeName == "" && selectedSecondLineName == "")){
                                hightlightedSecondLinks.Clear();
                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                relatedSecondNodes.Clear();
                                hightlightedNodesTemp.Clear();    
                                hightlightedSecondNodesTemp.Clear();                            
                                foreach (NodePosition gb in hightlightedNodes){
        
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    
                                    // Debug.Log(gb.name+gb.OrdZCoord);
                                }
                                foreach (NodePosition gb in hightlightedSecondNodes){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    // Debug.Log(gb.name+gb.OrdZCoord);
                                }
                                foreach (GameObject gb in GameObjectList){
                                    Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                    Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                    gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = textAlpha;
                                }
                                hightlightedNodes.Clear();
                                hightlightedSecondNodes.Clear();
                                HightLight = false;
                                HightLightLine = false;
                                HightLightNode = false;
                                lastLine = "";
                                lastNode = "";
                                clearShow = true;
                            }

                            //已经是HightLightNode模式下，然后点击了另一个节点
                            else if(selectedSecondNodeName != "" && selectedSecondNodeName != lastNode){
                                GameObject gameObject = GameObject.Find(selectedSecondNodeName);
                                if(!(gameObject.transform.tag == "Cube2"))
                                {
                                    Color currentColor;
                                    Color newColor;
                                    foreach (NodePosition gb in hightlightedNodes){
                                        // if(gb.OrdZCoord!=MinZ.z){
                                            GameObject gameObject2 = GameObject.Find(gb.name);
                                            gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                        // }
                                    }
                                    if(!shareCubeShow){
                                        foreach (NodePosition gb in hightlightedSecondNodes){
                                            // if(gb.OrdZCoord!=MinZ.z){
                                                GameObject gameObject2 = GameObject.Find(gb.name);
                                                gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                            // }
                                        }
                                        hightlightedSecondNodes.Clear();
                                        hightlightedSecondLinks.Clear();
                                    }

                                    foreach (GameObject gb in GameObjectList){
                                        currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                        newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                        gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                        gb.GetComponent<MeshRenderer>().material.color = newColor;
                                        gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = textAlpha;
                                    }

                                    hightlightedLinks.Clear();
                                    relatedNodes.Clear();
                                    hightlightedNodesTemp.Clear();

                                    if(hightlightedNodes.Exists(targetName=>targetName.name == gameObject.name.ToString())){
                                        hightlightedNodesTemp.Add(hightlightedNodes.Find(targetName=>targetName.name == gameObject.name.ToString()));
                                    }
                                    else{
                                        NodePosition node1 = new NodePosition{
                                        name = selectedSecondNodeName,
                                        OrdXCoord = gameObject.transform.position.x,
                                        OrdYCoord = gameObject.transform.position.y,
                                        OrdZCoord = gameObject.transform.position.z,
                                        };
                                        hightlightedNodesTemp.Add(node1); 
                                    }
                                    currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                    gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                    gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                    gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z); 
                                    // if(shareCubeShow){
                                    //     gameObject.transform.position = new Vector3( gameObject.transform.position.x,  gameObject.transform.position.y, sharecubeShift-50f);
                                    // }
                                    // else{
                                    //     gameObject.transform.position = new Vector3( gameObject.transform.position.x,  gameObject.transform.position.y, MinZ.z);
                                    // } 

                                    string[] sArray = selectedSecondNodeName.Split('@');

                                    foreach (LinksStructure gb in linksStructures)
                                    {
                                        //Find the link that source node' name is equal with the node name that highlight
                                        if (gb.SourceNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedLinks.Add(gb.index);
                                            relatedNodes.Add(gb.TargetNode.name);
                                        }

                                        if (gb.TargetNode.name.Equals(sArray[0]))
                                        {
                                            hightlightedLinks.Add(gb.index);
                                            relatedNodes.Add(gb.SourceNode.name);
                                        }
                                    }
                                    
                                    foreach (GameObject gb in GameObjectList)
                                    {
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        bool flagUp = false;
                                        for (int j = 0; j < relatedNodes.Count; j++)
                                        {
                                            if (targetName.Equals(relatedNodes[j]))
                                            {
                                                flagUp = true;
                                                //只记录节点的最开始的位置
                                                if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                    NodePosition node2 = new NodePosition{
                                                        name = gb.name.ToString(),
                                                        OrdXCoord = gb.transform.position.x,
                                                        OrdYCoord = gb.transform.position.y,
                                                        OrdZCoord = gb.transform.position.z,
                                                    };
                                                    hightlightedNodesTemp.Add(node2);
                                                }
                                                else{
                                                    hightlightedNodesTemp.Add(hightlightedNodes.Find(node => node.name == gb.name.ToString()));
                                                }
                                                
                                                // gb.GetComponent<MeshRenderer>().material = Material_HightLightNode;
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                                gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z); 
                                                // gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z);
                                                // if(shareCubeShow){
                                                //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, sharecubeShift-50f);
                                                // }
                                                // else{
                                                //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z);
                                                // } 
                                                // gb.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(gb.transform.position.x, gb.transform.position.y, MinZCoord));
                                            }
                                            if (!flagUp)
                                            {
                                                if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                                    currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                    gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                                }
                                            }
                                        }

                                    }
                                    hightlightedNodes.Clear();
                                    hightlightedNodes.AddRange(hightlightedNodesTemp);
                                    clearShow = true;
                                    lastNode = selectedSecondNodeName; 
                                }

                                  
                            }

                            //已经是HightLightNode模式下,点击了一条line，line和节点应该同时高亮,其他不变
                            else if(selectedSecondLineName != "" && selectedSecondLineName != lastLine){
                                // Debug.Log("3");
                                // Debug.Log(selectedSecondLineName );
                                GameObject gameObject = GameObject.Find(selectedSecondLineName);
                                string sourceNode = gameObject.name.ToString().Split('@')[0];
                                int nameLength = gameObject.name.ToString().Split('@').Length;
                                string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                targetNode = targetNode.Split('&')[1];
                                // Debug.Log("targetNode"+targetNode);
                                foreach(LinksStructure link in linksStructures){
                                    if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                        // Debug.Log("link.index"+link.index);
                                        hightlightedLinks.Add(link.index);
                                    }                                    
                                }
                                foreach (GameObject gb in GameObjectList){
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    if(targetName == sourceNode || targetName == targetNode){
                                        //  Debug.Log("targetName"+targetName);
                                        if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                            NodePosition node2 = new NodePosition{
                                            name = gb.name.ToString(),
                                            OrdXCoord = gb.transform.position.x,
                                            OrdYCoord = gb.transform.position.y,
                                            OrdZCoord = gb.transform.position.z,
                                            };
                                            hightlightedNodes.Add(node2);
                                            Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                            gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z);
                                        }
                                    }
                                }
                                clearShow = true;
                                lastLine = selectedSecondLineName;
                                
                            }

                        }
                        
                        //线条高亮模式
                        else if(HightLightLine){
                            if(selectedSecondLineName != ""){
                                GameObject gameObject = GameObject.Find(selectedSecondLineName);
                                    string sourceNode = gameObject.name.ToString().Split('@')[0];
                                    int nameLength = gameObject.name.ToString().Split('@').Length;
                                    string targetNode = gameObject.name.ToString().Split('@')[nameLength - 2];
                                    targetNode = targetNode.Split('&')[1];
                                    foreach(LinksStructure link in linksStructures){
                                        if(link.SourceNode.name == sourceNode && link.TargetNode.name == targetNode){
                                            hightlightedLinks.Add(link.index);
                                        }                                    
                                    }
                                    foreach (GameObject gb in GameObjectList){
                                        string targetName = gb.name.ToString().Split('@')[0];
                                        if(targetName == sourceNode || targetName == targetNode){
                                            if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                NodePosition node2 = new NodePosition{
                                                name = gb.name.ToString(),
                                                OrdXCoord = gb.transform.position.x,
                                                OrdYCoord = gb.transform.position.y,
                                                OrdZCoord = gb.transform.position.z,
                                            };
                                                hightlightedNodes.Add(node2);
                                                Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                                gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, gb.transform.position.z); 

                                            }
                                        }
                                    }
                                lastLine = selectedSecondLineName;
                                clearShow = true;
                            }

                            else if(selectedSecondLineName == "" && selectedSecondNodeName != "" && (GameObject.Find(selectedSecondNodeName).transform.tag != "Cube2")){
                                Color currentColor; 
                                Color newColor;
                                foreach (NodePosition gb in hightlightedNodes){
                                    if(gb.OrdZCoord!=MinZ.z){
                                        GameObject gameObject2 = GameObject.Find(gb.name);
                                        gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }
         
                                }

                                if(!shareCubeShow){
                                    foreach (NodePosition gb in hightlightedSecondNodes){
                                        // if(gb.OrdZCoord!=MinZ.z){
                                            GameObject gameObject2 = GameObject.Find(gb.name);
                                            gameObject2.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                        // }
                                    }
                                    hightlightedSecondNodes.Clear();
                                    hightlightedSecondLinks.Clear();
                                }


                                foreach (GameObject gb in GameObjectList){
                                    currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                    newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                    gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                }

                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                hightlightedNodesTemp.Clear();

                                GameObject gameObject = GameObject.Find(selectedSecondNodeName);
                                if(hightlightedNodes.Exists(targetName=>targetName.name == gameObject.name.ToString())){
                                    hightlightedNodesTemp.Add(hightlightedNodes.Find(targetName=>targetName.name == gameObject.name.ToString()));
                                }
                                else{
                                    NodePosition node1 = new NodePosition{
                                    name = selectedSecondNodeName,
                                    OrdXCoord = gameObject.transform.position.x,
                                    OrdYCoord = gameObject.transform.position.y,
                                    OrdZCoord = gameObject.transform.position.z,
                                    };
                                    hightlightedNodesTemp.Add(node1); 
                                }
                                currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                gameObject.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
                                gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                if(i == 0){
                                    gameObject.transform.position = new Vector3( gameObject.transform.position.x,  gameObject.transform.position.y, MinZ.z); 
                                }
                                else if(i == 6){
                                    gameObject.transform.position = new Vector3( gameObject.transform.position.x,  gameObject.transform.position.y,  gameObject.transform.position.z); 
                                }
                                // if(shareCubeShow){
                                //     gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, sharecubeShift-50f);
                                // }
                                // else{
                                //     gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, MinZ.z);
                                // }     
                                string[] sArray = selectedSecondNodeName.Split('@');

                                foreach (LinksStructure gb in linksStructures)
                                {
                                    //Find the link that source node' name is equal with the node name that highlight
                                    if (gb.SourceNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.TargetNode.name);
                                    }

                                    if (gb.TargetNode.name.Equals(sArray[0]))
                                    {
                                        hightlightedLinks.Add(gb.index);
                                        relatedNodes.Add(gb.SourceNode.name);
                                    }
                                }
                                //
                                foreach (GameObject gb in GameObjectList)
                                {
                                    string targetName = gb.name.ToString().Split('@')[0];
                                    bool flagUp = false;
                                    for (int j = 0; j < relatedNodes.Count; j++)
                                    {
                                        if (targetName.Equals(relatedNodes[j]))
                                        {
                                            flagUp = true;
                                            //只记录节点的最开始的位置
                                            if(!hightlightedNodes.Exists(targetName=>targetName.name == gb.name.ToString())){
                                                NodePosition node2 = new NodePosition{
                                                    name = gb.name.ToString(),
                                                    OrdXCoord = gb.transform.position.x,
                                                    OrdYCoord = gb.transform.position.y,
                                                    OrdZCoord = gb.transform.position.z,
                                                };
                                                hightlightedNodesTemp.Add(node2);
                                            }
                                            else{
                                                hightlightedNodesTemp.Add(hightlightedNodes.Find(node => node.name == gb.name.ToString()));
                                            }
                                            
                                            // gb.GetComponent<MeshRenderer>().material = Material_HightLightNode;
                                            currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                            newColor = new Color(currentColor.r, currentColor.g, currentColor.b, highLightNodeAlpha);
                                            gb.GetComponent<AlphaData>().alphaValue = highLightNodeAlpha;
                                            gb.GetComponent<MeshRenderer>().material.color = newColor;
                                            gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = highLightTextAlpha;
                                            if(i == 0){
                                                gb.transform.position = new Vector3( gb.transform.position.x,  gb.transform.position.y, MinZ.z); 
                                            }
                                            else if(i == 6){
                                                gb.transform.position = new Vector3( gb.transform.position.x,  gb.transform.position.y, gb.transform.position.z); 
                                            }
                                            // if(shareCubeShow){
                                            //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, sharecubeShift-50f);
                                            // }
                                            // else{
                                            //     gb.transform.position = new Vector3(gb.transform.position.x, gb.transform.position.y, MinZ.z);
                                            // }  
                                            // gb.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(gb.transform.position.x, gb.transform.position.y, MinZCoord));
                                        }
                                        if (!flagUp)
                                        {
                                            if (gb.name.ToString().Equals(selectedSecondNodeName) == false){
                                                currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, disLightNodeAlpha);
                                                gb.GetComponent<AlphaData>().alphaValue = disLightNodeAlpha;
                                                gb.GetComponent<MeshRenderer>().material.color = newColor;
                                                gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = disLightTextAlpha;
                                            }
                                        }
                                    }

                                }
                                hightlightedNodes.Clear();
                                hightlightedNodes.AddRange(hightlightedNodesTemp);
                                clearShow = true;
                                lastNode = selectedSecondNodeName; 
                                   
                                HightLightNode = true;
                                HightLightLine = false;

                            }

                            else if(selectedSecondLineName == "" && selectedSecondNodeName == ""){
                                hightlightedSecondLinks.Clear();
                                hightlightedLinks.Clear();
                                relatedNodes.Clear();
                                hightlightedNodesTemp.Clear();
                                foreach (NodePosition gb in hightlightedNodes){
                                    if(gb.OrdZCoord!=MinZ.z){
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);
                                    }
                                }
                                foreach (NodePosition gb in hightlightedSecondNodes){
                                    
                                        GameObject gameObject = GameObject.Find(gb.name);
                                        gameObject.transform.position = new Vector3(gb.OrdXCoord, gb.OrdYCoord, gb.OrdZCoord);

                                }
                                foreach (GameObject gb in GameObjectList){
                                    Color currentColor = gb.GetComponent<MeshRenderer>().material.color;
                                    Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, nodeAlpha);
                                    gb.GetComponent<AlphaData>().alphaValue = nodeAlpha;
                                    gb.GetComponent<MeshRenderer>().material.color = newColor;
                                    gb.transform.GetChild(0).GetComponent<TextMeshPro>().alpha = textAlpha;
                                }
                                hightlightedSecondNodes.Clear();
                                hightlightedNodes.Clear();
                                HightLight = false;
                                HightLightLine = false;
                                HightLightNode = false;
                                lastLine = "";
                                lastNode = "";
                                clearShow = true;
                            }
                        
                        }
                    }
                }


              stateDic[featureKey] = false;

            }
       }

    }

    public void resetGraph(){
        hightlightedNodes.Clear();
        hightlightedLinks.Clear();
        relatedNodes.Clear();
        hightlightedNodesTemp.Clear();
        hightlightedSecondNodes.Clear();
        hightlightedSecondLinks.Clear();
        relatedSecondNodes.Clear();
        hightlightedSecondNodesTemp.Clear();
        // reset = resetHightLight;
        HightLight = false;
        HightLightLine = false;
        HightLightNode = false;
        lastLine = "";
        lastNode = "";
        lastSecondLine = "";
        lastSecondNode = "";
        groupContainer2.anchoredPosition3D = new Vector3(0, 0, 0);
        groupContainer2.localRotation = Quaternion.Euler(0, 0, 0);
        foreach(GameObject gb in GameObjectList){
            Destroy(gb);
        }
        foreach (GameObject line in GameLineObjectList)
        {
            Destroy(line);
        }

        GameObjectList.Clear();
        GameLineObjectList.Clear();
        second = false;
        showGraph(nodesStructures, linksStructures, TwoFlag);
        foreach (GameObject line in GameLineObjectList)
        {
            Destroy(line);
        }
        second = false;
        for (int i = 0; i < linksStructures.Length; i++)
        {
            GameLineObjectList.AddRange(AddGraphLineVisual("Value:" + (float)linksStructures[i].value, linksStructures[i]));
        }

        Count = Time.frameCount;

    }

    public void get_right_hand()
    {
        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
            {
                rightHand_device = rightHandedControllers[0];
                // Debug.Log(rightHandedControllers[0]);
            }
        stateDic = new Dictionary<string, bool>();
    }

    public void get_left_hand()
    {
        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        if (leftHandedControllers.Count > 0)
            {
                leftHand_device = leftHandedControllers[0];
                // Debug.Log(rightHandedControllers[0]);
            }
        // stateDic = new Dictionary<string, bool>();
    }

    private void Update()
    {   
        //when change the display mode or rebuild the graph
        if (continulFlag == true)
        {
            //delete something don't need
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("(Clone)"))
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                if (transform.GetChild(i).name.Contains("@"))
                {
                    // Debug.Log(transform.GetChild(i).gameObject.name);
                    Destroy(transform.GetChild(i).gameObject);
                }

            }

            //wait some time to restart
            StartCoroutine(WaitForSeconds(0.05f, () =>
            {
                Start();
            }));
            continulFlag = false;
        }
        else
        {
            if(rightHand_device.isValid){
                update_inputDynamic_record(rightHand_device, CommonUsages.triggerButton,0); //扳机键
                update_inputDynamic_record(rightHand_device, CommonUsages.primaryButton,1); 
                update_inputDynamic_record(rightHand_device, CommonUsages.secondaryButton,2);
                
                // update_inputDynamic_record(rightHand_device, CommonUsages.gripButton,3); //
                
            }
            else if(!rightHand_device.isValid){
                get_right_hand();
                // Debug.Log("succ1");
            }
            if(leftHand_device.isValid){
                // update_inputDynamic_record(leftHand_device, CommonUsages.triggerButton,3);
                update_inputDynamic_record(leftHand_device, CommonUsages.primaryButton,4); 
                // update_inputDynamic_record(leftHand_device, CommonUsages.secondaryButton,5);
                update_inputDynamic_record(leftHand_device, CommonUsages.triggerButton,6);
            }
            else if(!leftHand_device.isValid){
                get_left_hand();
            }

            if(isClicked == true || clearShow == true){
               
                    // Debug.Log("DELET");
                 
                    foreach (GameObject line in GameLineObjectList)
                    {
                        Destroy(line);
                    }
                    GameLineObjectList.Clear();
                    // Debug.Log(GameLineObjectList.Count);
                    second = false;

                    for (int i = 0; i < linksStructures.Length; i++)
                    {
                        GameLineObjectList.AddRange(AddGraphLineVisual("Value:" + (float)linksStructures[i].value, linksStructures[i]));
                    }
                
                if(clearShow == true){
                    clearShow = false;
                }


            }

            if(reset && !resetHightLight&& !resetSecondHightLight){
                // GameObject gameObject = GameObject.Find("XR Origin");
                SmoothMoveAndRotate(oriCameraPosition, oriCameraRotation);              
                // gameObject.transform.position = oriCameraPosition;
                // gameObject.transform.rotation = Quaternion.Euler(oriCameraRotation);
            }

            if(resetHightLight&&!reset&&!resetSecondHightLight){
                // GameObject gameObject = GameObject.Find("XR Origin");
                SmoothMoveAndRotate(oriCameraPosition, oriCameraRotation); 
                 
                // GameObject gameObject = GameObject.Find("XR Origin");
                // if(gameObject.transform.position.z - oriCameraPosition.z < shift * 0.6){
                //     gameObject.transform.position = oriCameraPosition;
                //    gameObject.transform.rotation = Quaternion.Euler(oriCameraRotation);
                // }
                // else {
                //     gameObject.transform.position = oppCameraPosition;
                //     gameObject.transform.rotation = Quaternion.Euler(oppCameraRotation);
               
                // }
            }
            if(resetSecondHightLight&&!reset&&!resetHightLight){
                SmoothMoveAndRotate( oppCameraPosition, oppCameraRotation);
            }
            // Debug.Log(CgameObject.transform.position.z);
            if(Time.frameCount - Count == 5){
                clearShow = true;
            }
            
        }
        
    }
    void SmoothMoveAndRotate(Vector3 targetPosition, Vector3 targetEulerAngles) {
            // 平滑移动
            // gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            CgameObject.transform.position = Vector3.MoveTowards(CgameObject.transform.position, targetPosition, 10);
            
            // 平滑旋转
            Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
            // gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            // Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
            CgameObject.transform.rotation = Quaternion.RotateTowards(CgameObject.transform.rotation, targetRotation, 30);


            // 检测是否接近目标位置和角度
            if (Vector3.Distance(CgameObject.transform.position, targetPosition) < 10f &&
                Quaternion.Angle(CgameObject.transform.rotation, targetRotation) < 10f) {
                resetHightLight = false;
                reset = false;
                resetSecondHightLight = false;
                // 到达目标位置和角度，可以在这里添加停止移动的逻辑，或进行其他操作
            }
    }


    private interface IGraphVisual
    {
        List<GameObject> AddGraphVisual(Vector3 graphPosition, float barWidth, float barHight, float Zposition, string tooltipText, bool second, int TwoOrThree);
    }
    private interface IGraphVisualObject
    {
        void SetGraphVisualObjectInfo(Vector3 graphPosition, float barWidth, float barHight, float Zposition, string tooltipText,float a,bool second , int TwoOrThree);
    }
    public class BarChartVisualObject : IGraphVisualObject
    {
        private GameObject barGameObject;

        private float barWidth;
        private float barHight;
        private float Zposition;
        private float a;
        public BarChartVisualObject(GameObject barGameObject, float barWidth, float barHight, float Zposition, float a)
        {
            this.barGameObject = barGameObject;
            this.barWidth = barWidth;
            this.barHight = barHight;
            this.Zposition = Zposition;
            this.a = a;
        }

        public void SetGraphVisualObjectInfo(Vector3 graphPosition, float barWidth, float barHight, float Zposition, string tooltipText, float a, bool second, int TwoOrThree )
        {
            //物体高度
            // continulFlag = 1;
            float Xgap = 1.2f;
            float Zgap = 1.8f;
            float Ygap = 80f;
            if(second){
                Xgap = 0.8f;
                Zgap = 0.5f;
            }
            var temp = new Vector3();
            //Change change1 = barGameObject.AddComponent<Change>();
            //change1.setHight(graphPosition);
            if(TwoOrThree == 0){
                temp = new Vector3(graphPosition.x*Xgap, graphPosition.z*Zgap ,0);
            }
            else{
               temp = new Vector3(graphPosition.x*Xgap, graphPosition.z*Zgap ,a*Ygap);
            }

            barGameObject.GetComponent<Transform>().position = temp;
            // barGameObject.GetComponent<Transform>().position = graphPosition;
            barGameObject.GetComponent<Transform>().localScale = new Vector3(barWidth, barHight, Zposition);
            //change1.setGameObject(barGameObject);
            //change1.setLocal(new Vector3(barWidth, barHight, Zposition)); 
        }

    }

    public List<GameObject> AddGraphVisual(Vector3 graphPosition, float Width, float barHight, float barZStation, string tooltipText, NodesStructure a, int i,bool second, int TwoOrThree)
    {
        //计算节点高度
        GameObject barGameObject = CreateBar(graphPosition, Width, barHight, barZStation, a, i);
        float temp = (float)a.nodePosition;
        // barZStation = barZStation*0.1f;
        BarChartVisualObject barChartVisualObject = new BarChartVisualObject(barGameObject, Width, barHight, barZStation,temp);

        barChartVisualObject.SetGraphVisualObjectInfo(graphPosition, Width, barHight, barZStation, tooltipText,temp, second, TwoOrThree);

        return new List<GameObject>() { barGameObject };
    }


    //create link method input the link data
    private GameObject CreateLink(LinksStructure link)
    {
        GameObject lineobject = new GameObject("line");
        lineobject.layer = LayerMask.NameToLayer("Line");

        string SourceNodeName = link.SourceNode.name + "@" + link.SourceNode.value;
        GameObject SourceNode = GameObject.Find(SourceNodeName);

        lineobject.transform.parent = this.transform;
        if(!second){
           lineobject.transform.parent = this.transform;
           lineobject.transform.tag = "Link";
        }
        else{
           lineobject.transform.parent = this.transform.GetChild(0).transform;
           lineobject.transform.tag = "Link2";
           foreach(GameObject obj in GameObjectList){
                if(obj.name.StartsWith(link.SourceNode.name) && (obj.transform.tag == "shareCube")){
                    SourceNodeName = obj.name;
                    SourceNode = obj;
                }
           }
        }
        var meshFilter = lineobject.AddComponent<MeshFilter>();
        lineobject.AddComponent<MeshRenderer>();
        lineobject.AddComponent<LineRenderer>();
        lineobject.AddComponent<MeshCollider>();
        
        MeshCollider meshCollider = lineobject.GetComponent<MeshCollider>();
        meshCollider.convex = false;
        LineRenderer line = lineobject.GetComponent<LineRenderer>();
        line.alignment = LineAlignment.TransformZ;

        float y0_3D = SourceNode.transform.position.y + (float)link.y0_3D + (float)link.width / 2 - SourceNode.transform.localScale.y / 2;
        //float y0_3D = (float)link.y0_3D + (float)link.width / 2;
        string TargetNodeName = link.TargetNode.name + "@" + link.TargetNode.value;
        GameObject TargetNode = GameObject.Find(TargetNodeName);

        float y1_3D = TargetNode.transform.position.y + (float)link.y1_3D + (float)link.width / 2 - TargetNode.transform.localScale.y / 2;
        //float y1_3D = (float)link.y1_3D + (float)link.width / 2;

        float width = (float)link.width;
        //Specific values of left and right up and down

        float z0 = (float)(SourceNode.transform.position.z) * PositionScale - 10 / 2;
        float x0 = (float)(SourceNode.transform.position.x) * PositionScale + 10 / 2;
        float z1 = (float)(TargetNode.transform.position.z) * PositionScale - 10 / 2;
        float x1 = (float)(TargetNode.transform.position.x) * PositionScale - 10 / 2;
        //n1 is left vector
        //Bezier curve control point
        Vector3 n1 = new Vector3(x0, y0_3D, z0);
        Vector3 n2 = new Vector3((x0 + x1) / 2, y0_3D, (z0 + z1) / 2);
        //n4 is right vector
        Vector3 n3 = new Vector3((x0 + x1) / 2, y1_3D, (z0 + z1) / 2);
        Vector3 n4 = new Vector3(x1, y1_3D, z1);
        //acconding the data to draw the link

        DrawLinearCurve(meshFilter, line, n1, n2, n3, n4, width, 10, 10,lineobject);
         //设置线的材质
        lineobject.name = SourceNode.name + "&" + TargetNode.name + "/" + link.value;
        lineobject.GetComponent<MeshRenderer>().material = linkMaterial;
        Color currentColor = SourceNode.GetComponent<MeshRenderer>().material.color;  
        if(lineobject.transform.tag == "Link2"){
            currentColor = TargetNode.GetComponent<MeshRenderer>().material.color;  
        }
        float temp = lineAlpha;
        if(link.value < smallline_value){
            temp = smalllineAlpha;
        }
        if(!(lineobject.transform.tag == "Link2")){
            if(HightLight == true){
                bool flagUp = false;
                foreach(int i in hightlightedLinks){
                    if(link.index == i){
                        flagUp = true;
                        temp = highLightLineAlpha;
                    }
                }
                if(flagUp == false){
                    temp = disLightLineAlpha;
                }
            }

        }
        else{
            if(HightLight == true){
                bool flagUp = false;
                foreach(int i in hightlightedSecondLinks){
                    if(link.index == i){
                        flagUp = true;
                        temp = highLightLineAlpha;
                    }
                }
                if(flagUp == false){
                    temp = disLightLineAlpha;
                }
            }
        }

        lineobject.AddComponent<AlphaData>();
        lineobject.GetComponent<AlphaData>().alphaValue = temp;
        lineobject.GetComponent<AlphaData>().SetValue(temp);
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, temp);
        lineobject.GetComponent<MeshRenderer>().material.color = newColor;

        // 刚体设置
        lineobject.AddComponent<Rigidbody>();
        Rigidbody rb =  lineobject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.angularDrag = float.PositiveInfinity;

        // XRgrab设置
        lineobject.AddComponent<XRGrabInteractable>();
        XRGrabInteractable grabInteractable =  lineobject.GetComponent<XRGrabInteractable>();
        grabInteractable.movementType = XRBaseInteractable.MovementType.Kinematic;
        grabInteractable.trackRotation = false;
        grabInteractable.trackPosition = false;
        grabInteractable.throwOnDetach = false;

        grabInteractable.hoverEntered.AddListener(OnHoverEnterLine);
        grabInteractable.hoverExited.AddListener(OnHoverExitLine);
        return lineobject;
    }

    public void OnHoverExitLine(HoverExitEventArgs arg){
        string hoveredGameObjectName = arg.interactableObject.transform.name;
        
        if (arg.interactorObject.transform.name.Contains("Right") )
        {
            // 只有右手手柄时执行代码
            selectedLineName = "";
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);
            Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, gameObject.GetComponent<AlphaData>().GetValue());
            gameObject.GetComponent<MeshRenderer>().material.color = newColor;
            GameObject gb = GameObject.Find("Information");
            gb.GetComponent<TextMeshProUGUI>().text = "";
        }

        if (arg.interactorObject.transform.name.Contains("Left") ){
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);
            if(gameObject.transform.tag == "Link2"){
                selectedSecondLineName = "";
                Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, gameObject.GetComponent<AlphaData>().GetValue());
                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
            }
        }

    }

    public void OnHoverEnterLine(HoverEnterEventArgs arg)
    {
        // isHovering = true;
        string hoveredGameObjectName = arg.interactableObject.transform.name;
        if (arg.interactorObject.transform.name.Contains("Right") )
        {
            // 只有右手手柄时执行代码
            selectedNodeName = "";
            selectedLineName = hoveredGameObjectName;
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);
            Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, hoveredLineAlpha);
            if(gameObject.GetComponent<AlphaData>().alphaValue != hoveredLineAlpha){
                gameObject.GetComponent<AlphaData>().SetValue(hoveredLineAlpha);
            }
            // gameObject.GetComponent<AlphaData>().SetValue(hoveredLineAlpha);
            gameObject.GetComponent<MeshRenderer>().material.color = newColor;
            GameObject gb = GameObject.Find("Information");

            string sourceNode = selectedLineName.Split('@')[0];
            int nameLength = selectedLineName.Split('@').Length;
            string targetNode = selectedLineName.Split('@')[nameLength - 2];
            string value = selectedLineName.Split('/')[1];
            targetNode = targetNode.Split('&')[1];
            gb.GetComponent<TextMeshProUGUI>().text = "Link:" + sourceNode + "->" + targetNode + "\n" + "Value:" + value;
        }

        if (arg.interactorObject.transform.name.Contains("Left") ){
            GameObject gameObject = GameObject.Find(hoveredGameObjectName);
            if(gameObject.transform.tag == "Link2"){
                selectedSecondLineName = hoveredGameObjectName;
                selectedSecondNodeName = "";
                Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
                Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, hoveredLineAlpha);
                if(gameObject.GetComponent<AlphaData>().alphaValue != hoveredLineAlpha){
                    gameObject.GetComponent<AlphaData>().SetValue(hoveredLineAlpha);
                }
                gameObject.GetComponent<MeshRenderer>().material.color = newColor;
            }
        }

    }
    

    private float getMinLength(NodesStructure[] nodes)
    {
        //The method dynamically calculates the maximum base area x value
        //Algorithm idea: Calculate the number of elements in the column with the most elements
        //The column spacing with the most elements in the 2d version is fixed
        int max = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (max < nodes[i].layer)
            {
                max = nodes[i].layer;
            }
        }

        int[] ay = new int[max];
        for (int m = 0; m < max; m++)
        {
            int number = 0;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].layer == m)
                {
                    number += 1;
                }

            }
            ay[m] = number;

        }
        int m1 = 0;
        int index = 0;
        for (int y = 0; y < ay.Length; y++)
        {
            if (ay[y] > m1)
            {
                m1 = ay[y];
                index = y;
            }
        }
        // Debug.Log("index" + index);
        List<NodesStructure> node1 = new List<NodesStructure>();
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].layer == index)
            {
                node1.Add(nodes[i]);
            }
        }
        //Y0 of the second element of the column with the most elements-y1 of the first element
        float ymin = 100;
        float ymax = 100;
        for (int i = 0; i < node1.Count; i++)
        {
            if (node1[i].y1 < ymin)
            {
                ymin = (float)node1[i].y1;
            }
        }
        for (int i = 0; i < node1.Count; i++)
        {
            if (node1[i].y0 < ymax && node1[i].y0 > ymin)
            {
                ymax = (float)node1[i].y0;
            }
        }
        //When x is greater than the spacing, there must be a bottom area overlap
        //So the x value of the required bottom area is the spacing
        return (float)(ymax - ymin);


    }

    //Draw the Bizare curve of Link
    private void DrawLinearCurve(MeshFilter lineMeshobject, LineRenderer lineRenderer, Vector3 position1, Vector3 position2, Vector3 position3, Vector3 position4, float width, float LDepth, float RDepth, GameObject lineobject)
    {
        List<Vector3> curveDataList = new List<Vector3>();
        //input the bezier data to the datalist
        curveDataList.AddRange(Bezier_CubicCurvePoints(position1, position2, position3, position4, splitCount));

        //Vertex data.
        int numPoints = curveDataList.Count * 8;
        Vector3[] verts = new Vector3[numPoints];
        // set the uv  but we dont use the texture.
        //the RDepth and LDepth is 10
        Vector2[] uvs = new Vector2[numPoints];
        float widthInterval = (RDepth - LDepth) / (curveDataList.Count - 1);
        //this is use in another type of uv 
        float curvelength = 0;

        for (int i = 0; i < curveDataList.Count - 1; i++)
        {
            curvelength += Vector3.Distance(curveDataList[i], curveDataList[i + 1]);
        }

        // Vertex DATA Setup
        float u1, u2 = 0;
        int vertexSpace = 8;
        for (int i = 0; i < curveDataList.Count - 1; i++)
        {   
            float meshLDepth = LDepth + i * widthInterval;
            float meshRDepth = LDepth + (i + 1) * widthInterval;
            verts[i * vertexSpace + 0] = curveDataList[i];
            verts[i * vertexSpace + 1] = curveDataList[i] + Vector3.down * width;
            verts[i * vertexSpace + 2] = curveDataList[i + 1];
            verts[i * vertexSpace + 3] = curveDataList[i + 1] + Vector3.down * width;

            verts[i * vertexSpace + 4] = curveDataList[i] + Vector3.forward * meshLDepth;
            verts[i * vertexSpace + 5] = curveDataList[i + 1] + Vector3.forward * meshRDepth;

            verts[i * vertexSpace + 6] = curveDataList[i] + Vector3.forward * meshLDepth + Vector3.down * width;
            verts[i * vertexSpace + 7] = curveDataList[i + 1] + Vector3.forward * meshRDepth + Vector3.down * width;

            
            u1 = (curveDataList[i].x - curveDataList[0].x) / (curveDataList[curveDataList.Count - 1].x - curveDataList[0].x);
            u2 = (curveDataList[i + 1].x - curveDataList[0].x) / (curveDataList[curveDataList.Count - 1].x - curveDataList[0].x);
            uvs[i * vertexSpace + 0] = new Vector2(u1, 1);
            uvs[i * vertexSpace + 1] = new Vector2(u1, 0);
            uvs[i * vertexSpace + 2] = new Vector2(u2, 1);
            uvs[i * vertexSpace + 3] = new Vector2(u2, 0);
            uvs[i * vertexSpace + 4] = new Vector2(u1, 0);
            uvs[i * vertexSpace + 5] = new Vector2(u2, 0);
        }

        // Indices Setup
        int numTris = numPoints - 2 - 2;
        int[] indices = new int[numTris * 3];
        int indiceSpace = 24;
        //detail trangle data
        for (int i = 0; i < curveDataList.Count - 1; i++)
        {
            indices[i * indiceSpace + 0] = i * vertexSpace + 0;
            indices[i * indiceSpace + 1] = i * vertexSpace + 2;
            indices[i * indiceSpace + 2] = i * vertexSpace + 1;

            indices[i * indiceSpace + 3] = i * vertexSpace + 1;
            indices[i * indiceSpace + 4] = i * vertexSpace + 2;
            indices[i * indiceSpace + 5] = i * vertexSpace + 3;

            indices[i * indiceSpace + 6] = i * vertexSpace + 0;
            indices[i * indiceSpace + 7] = i * vertexSpace + 4;
            indices[i * indiceSpace + 8] = i * vertexSpace + 5;

            indices[i * indiceSpace + 9] = i * vertexSpace + 5;
            indices[i * indiceSpace + 10] = i * vertexSpace + 2;
            indices[i * indiceSpace + 11] = i * vertexSpace + 0;
            //-----------------------------------------
            indices[i * indiceSpace + 12] = i * vertexSpace + 4;
            indices[i * indiceSpace + 13] = i * vertexSpace + 6;
            indices[i * indiceSpace + 14] = i * vertexSpace + 5;

            indices[i * indiceSpace + 15] = i * vertexSpace + 5;
            indices[i * indiceSpace + 16] = i * vertexSpace + 6;
            indices[i * indiceSpace + 17] = i * vertexSpace + 7;

            indices[i * indiceSpace + 18] = i * vertexSpace + 6;
            indices[i * indiceSpace + 19] = i * vertexSpace + 1;
            indices[i * indiceSpace + 20] = i * vertexSpace + 7;

            indices[i * indiceSpace + 21] = i * vertexSpace + 7;
            indices[i * indiceSpace + 22] = i * vertexSpace + 1;
            indices[i * indiceSpace + 23] = i * vertexSpace + 3;
        }

        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = indices;
        mesh.RecalculateBounds();
        lineMeshobject.mesh = mesh;

        lineobject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private Vector3[] Bezier_CubicCurvePoints(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int splits)
    {
        Vector3[] res = new Vector3[splits];
        float delta = 1f / (splits - 1);
        float dist = 0;
        for (int i = 0; i < (splits - 1); i++)
        {
            res[i] = Bezier_CubicCurvePoint(p1, p2, p3, p4, dist);
            dist += delta;
        }
        res[splits - 1] = Bezier_CubicCurvePoint(p1, p2, p3, p4, 1);
        return res;
    }

    private Vector3 Bezier_CubicCurvePoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        Vector3 res = Vector3.zero;
        res.x = (1 - t) * (1 - t) * (1 - t) * p1.x + 3 * (1 - t) * (1 - t) * t * p2.x + 3 * (1 - t) * t * t * p3.x + t * t * t * p4.x;
        res.y = (1 - t) * (1 - t) * (1 - t) * p1.y + 3 * (1 - t) * (1 - t) * t * p2.y + 3 * (1 - t) * t * t * p3.y + t * t * t * p4.y;
        res.z = (1 - t) * (1 - t) * (1 - t) * p1.z + 3 * (1 - t) * (1 - t) * t * p2.z + 3 * (1 - t) * t * t * p3.z + t * t * t * p4.z;
        return res;
    }


    private Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }

    public List<GameObject> AddGraphLineVisual(string tooltipText, LinksStructure a)
    {
        GameObject lineGameObject = CreateLink(a);
        return new List<GameObject>() { lineGameObject };
    }
    public class NodePosition
    {
        public string name { get; set; }
        public float OrdXCoord { get; set; }
        public float OrdYCoord { get; set; }
        public float OrdZCoord { get; set; }
        // public Material nodeMaterial{ get; set; }
    }
      // -------------------------------------------------------------------------------------------------------

}

