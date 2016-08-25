using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GenerateAtoms : MonoBehaviour
{
    public Text atomDisplay;
    public Text keyA;
    public Text keyB;
    public Text keyX;


    [Tooltip("The perovskite structure will be x unit cells by x unit cells")]
    public int dimensions = 1;

    //change to 1 when making 2by2
    private GameObject[] octahedraArray /*= new GameObject[8]*/;
    private GameObject[] perovskiteUnitCell;
    private GameObject[] AatomsArray = new GameObject[8];
    private GameObject[] XatomsArray = new GameObject[7];
    private GameObject[] BatomsArray = new GameObject[8];

    private GameObject AatomKey;
    private GameObject BatomKey ;
    private GameObject XatomKey ;

    private float XatomsRadius = 0.25f;
    private float BatomsRadius = 0.7f;
    private float AatomsRadius = 0.5f;
    private Color XatomsColor = Color.red;
    private Color BatomsColor = new Color(0.24f,1,0);
    //face vertices (sn)
    private Color AatomsColor = new Color(0.4f,0.502f,0.502f);

    private Vector3[] XatomCoords = new Vector3[7];
    private Vector3[] AatomCoords = new Vector3[8];

    private Vector3[][] meshVerts = new Vector3[8][];
    private Mesh mesh;/* = new Mesh();*/

    public int unit = 1;

    private float transformX = 0;
    private float transformY = 0;
    private float transformZ = 0;

    private GameObject[] planeArray = new GameObject[8];
    private GameObject[] planeArrayCopy = new GameObject[8];
    private float planeTransparency = 0.3f;

    private MeshFilter filter;
    private MeshRenderer renderer2;

    private GameObject SelectedCubeCage;
    //private int octCounter = 1;
    public Slider slider;
    private float sliderValue;

    public Dropdown dropdown;
    private string dropdownValue;
    //max value is changed in the ChangeValue script
    //public float maxSliderValue = 180;

        //glazer variables
    
    private string xPhase = "";
    private string yPhase = "";
    private string zPhase = "";

    //rotation variables
    float rotAngle = 0f;
    Vector3 rotAxisX = new Vector3();
    Vector3 rotAxisY = new Vector3();
    Vector3 rotAxisZ = new Vector3();
    Vector3 rotAxisDefault = new Vector3();
    Quaternion rotX;
    Quaternion rotAntiX;
    
    Quaternion rotY;
    Quaternion rotAntiY;
    Quaternion rotZ;
    Quaternion rotAntiZ;
    Quaternion rotDef;
    Quaternion rotAll;
    Quaternion rotOpp;

    private float Xangle;
    private float Yangle;
    private float Zangle;
    //private GameObject Batom = new GameObject("B");
    void Start()
    {
        octahedraArray = new GameObject[/*dimensions*dimensions*dimensions*/1];
        perovskiteUnitCell = new GameObject[/*dimensions*dimensions*dimensions*/1];
        sliderValue = slider.value;
        
        

        //initiating axes of rotation
        rotAxisX = Vector3.right; 
        rotAxisY = Vector3.forward;
        rotAxisZ = Vector3.up;
        rotAxisDefault = new Vector3(0, 0, 0);

        //alright, cool, put this in the update function
        //Debug.Log(dropdown.captionText.text);

        generatePerovskite();
    }
   void Update()
    {
       simulateTilt();


    }
    public void simulateTilt()
    {
        dropdownValue = dropdown.captionText.text;
        //this is the glazer notation stuff delimiting?
        xPhase = char.ToString(dropdownValue[1]);
        yPhase = char.ToString(dropdownValue[3]);
        zPhase = char.ToString(dropdownValue[5]);


        rotAngle = slider.value; //degrees of rotation (toggled by slider value)
        rotDef = Quaternion.AngleAxis(rotAngle, rotAxisDefault);

        rotX = rotDef;
        rotY = rotDef;
        rotZ = rotDef;

        rotAntiX = rotDef;
        rotAntiY = rotDef;
        rotAntiZ = rotDef;

   
        if (!xPhase.Contains("0"))
        {
            rotX = Quaternion.AngleAxis(rotAngle, rotAxisX);
            rotAntiX = Quaternion.AngleAxis(rotAngle, -rotAxisX);
            //rotAll = rotX;
            //rotOpp=rotAntiX;
            if (xPhase.Contains("+"))
            {
                rotAll = rotX;
                rotOpp = rotAntiX;
            }
            else if (xPhase.Contains("-"))
            {
                rotAll = rotAntiX;
                rotOpp = rotX;
            }
        }
        else
        {
            rotAll = rotX;
            rotOpp = rotAntiX;
        }
        if (!yPhase.Contains("0"))
        {
            rotY = Quaternion.AngleAxis(rotAngle, rotAxisY);
            rotAntiY = Quaternion.AngleAxis(rotAngle, -rotAxisY);
            if (yPhase.Contains("+"))
            {
                rotAll *= rotY;
                rotOpp *= rotAntiY;
            }
            else if (yPhase.Contains("-"))
            {
                rotAll *= rotAntiY;
                rotOpp *= rotY;
            }
        }
        else
        {
            rotAll *= rotY;
            rotOpp *= rotAntiY;
        }
        if (!zPhase.Contains("0"))
        {
            
            rotZ = Quaternion.AngleAxis(rotAngle, rotAxisZ);
            rotAntiZ = Quaternion.AngleAxis(rotAngle, -rotAxisZ);
            if (zPhase.Contains("+"))
            {
                rotAll *= rotZ;
                rotOpp *= rotAntiZ;
            }
            else if (zPhase.Contains("-"))
            {
                rotAll *= rotAntiZ;
                rotOpp *= rotZ;
            }
        }
        else
        {
            rotAll *= rotZ;
            rotOpp *= rotAntiZ;
        }
        //rotAll = rotX * rotY;
        //rotAll *= rotZ;
        if (xPhase.Contains("+") )
        {

            octahedraArray[0].transform.rotation = rotAll;
            octahedraArray[1].transform.rotation = rotAll;
            octahedraArray[2].transform.rotation = rotOpp;
            octahedraArray[3].transform.rotation = rotOpp;
            octahedraArray[4].transform.rotation = rotOpp;
            octahedraArray[5].transform.rotation = rotOpp;
            octahedraArray[6].transform.rotation = rotAll;
            octahedraArray[7].transform.rotation = rotAll;
        }
        else if ( xPhase.Contains("-"))
        {

            octahedraArray[0].transform.rotation = rotAll;
            octahedraArray[1].transform.rotation = rotOpp;
            octahedraArray[2].transform.rotation = rotOpp;
            octahedraArray[3].transform.rotation = rotAll;
            octahedraArray[4].transform.rotation = rotOpp;
            octahedraArray[5].transform.rotation = rotAll;
            octahedraArray[6].transform.rotation = rotAll;
            octahedraArray[7].transform.rotation = rotOpp;
        }
        //a0b+c0 and a0b0c+ need different arrangements! (use if statements?)
        
        if (yPhase.Contains("+") )
        {
            
            octahedraArray[0].transform.rotation = rotAll;
            octahedraArray[1].transform.rotation = rotOpp;
            octahedraArray[2].transform.rotation = rotAll;
            octahedraArray[3].transform.rotation = rotOpp;
            octahedraArray[4].transform.rotation = rotOpp;
            octahedraArray[5].transform.rotation = rotAll;
            octahedraArray[6].transform.rotation = rotOpp;
            octahedraArray[7].transform.rotation = rotAll;
        }
        else if ( yPhase.Contains("-"))
        {

            octahedraArray[0].transform.rotation = rotAll;
            octahedraArray[1].transform.rotation = rotOpp;
            octahedraArray[2].transform.rotation = rotOpp;
            octahedraArray[3].transform.rotation = rotAll;
            octahedraArray[4].transform.rotation = rotAll;
            octahedraArray[5].transform.rotation = rotOpp;
            octahedraArray[6].transform.rotation = rotOpp;
            octahedraArray[7].transform.rotation = rotAll;
        }
        
        if (zPhase.Contains("+"))
        {

            octahedraArray[0].transform.rotation = rotAll;
            octahedraArray[1].transform.rotation = rotOpp;
            octahedraArray[2].transform.rotation = rotOpp;
            octahedraArray[3].transform.rotation = rotAll;
            octahedraArray[4].transform.rotation = rotAll;
            octahedraArray[5].transform.rotation = rotOpp;
            octahedraArray[6].transform.rotation = rotOpp;
            octahedraArray[7].transform.rotation = rotAll;
        }
        

        else if (zPhase.Contains("-"))
        {
            octahedraArray[0].transform.rotation = rotAll;
            octahedraArray[1].transform.rotation = rotOpp;
            octahedraArray[2].transform.rotation = rotOpp;
            octahedraArray[3].transform.rotation = rotAll;
            octahedraArray[4].transform.rotation = rotOpp;
            octahedraArray[5].transform.rotation = rotAll;
            octahedraArray[6].transform.rotation = rotAll;
            octahedraArray[7].transform.rotation = rotOpp;
        }

        

    }
    /*
   //Coroutine for rotating
    IEnumerator RotateMe(GameObject obj, Vector3 byAngles)
    {
        
        var fromAngle = obj.transform.rotation;
        
        var toAngle = Quaternion.Euler(byAngles);
        
            obj.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, 1f);
            yield return null;
        
    }
    */
    public void resetX()
    {
        transformX -=2;
        }
    public void resetY()
    {
        transformY -= 2;
    }
    public void resetZ()
    {
        transformZ -= 2;
    }
    public void generatePerovskite()
    {
        
        mesh = new Mesh();



        XatomCoords = new Vector3[]{
            new Vector3(0, -unit, 0),
            new Vector3(0, unit, 0),
            new Vector3(unit, 0, 0),
            new Vector3(0, 0, unit),
            new Vector3(0, 0, 0),
            new Vector3(-unit, 0, 0),
            new Vector3(0, 0, -unit)
        };
        AatomCoords = new Vector3[]
        {
            new Vector3(-unit,unit,unit),
            new Vector3(unit,unit,unit),
            new Vector3(-unit,unit,-unit),
            new Vector3(unit,unit,-unit),
            new Vector3(unit,-unit,-unit),
            new Vector3(unit,-unit,unit),
            new Vector3(-unit,-unit,unit),
            new Vector3(-unit,-unit,-unit)
        };

        //generate key for atom display here
        
        AatomKey = new GameObject(string.Concat(atomDisplay.text[3], atomDisplay.text[4]));
        AatomKey=GameObject.CreatePrimitive(PrimitiveType.Sphere);
        AatomKey.transform.position = keyA.transform.position;
        AatomKey.transform.localScale = new Vector3(AatomsRadius, AatomsRadius, AatomsRadius);
        AatomKey.GetComponent<Renderer>().material.color = AatomsColor;
        
        BatomKey = new GameObject(string.Concat(atomDisplay.text[1], atomDisplay.text[2]));
        BatomKey = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        BatomKey.transform.position = keyB.transform.position;
        BatomKey.transform.localScale = new Vector3(BatomsRadius, BatomsRadius, BatomsRadius);
        BatomKey.GetComponent<Renderer>().material.color = BatomsColor;

        XatomKey = new GameObject(string.Concat(atomDisplay.text[5]));
        XatomKey = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        XatomKey.transform.position = keyX.transform.position;
        XatomKey.transform.localScale = new Vector3(XatomsRadius, XatomsRadius, XatomsRadius);
        XatomKey.GetComponent<Renderer>().material.color = XatomsColor;
        for (int m = 0; m < octahedraArray.Length; m++)
        {
            
            octahedraArray[m] = new GameObject("Octahedra " + (m));
            perovskiteUnitCell[m] = new GameObject("Perovskite Unit Cell " + (m));
            
            for (int i = 0; i < AatomsArray.Length; i++)
            {

                //Debug.Log("X: "+transformX);
                //Debug.Log("Z: " + transformZ);
                //AatomCoords[i] += new Vector3(transformX, transformY, transformZ);
                
                
               // Debug.Log(AatomCoords[i]);
                //make the actual sphere
                AatomsArray[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                AatomsArray[i].name = "A " + (i + 1);

                AatomsArray[i].transform.position = AatomCoords[i];
                
                //radius
                AatomsArray[i].transform.localScale = new Vector3(AatomsRadius, AatomsRadius, AatomsRadius);

                //make octahedra the parent of the A atoms
                //AatomsArray[i].transform.parent = octahedraArray[m].transform;
                AatomsArray[i].transform.parent = perovskiteUnitCell[m].transform;
                AatomsArray[i].GetComponent<Renderer>().material.color = AatomsColor;


                
                
                //add interactible script
                AatomsArray[i].AddComponent<Interactible>();

                
                    if (i < XatomsArray.Length)
                    {

                        //XatomCoords[i] += new Vector3(transformX, transformY, transformZ);


                        //make the actual sphere
                        XatomsArray[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        XatomsArray[i].name = "X " + (i + 1);
                        XatomsArray[i].transform.position = XatomCoords[i];
                        //radius
                        XatomsArray[i].transform.localScale = new Vector3(XatomsRadius, XatomsRadius, XatomsRadius);

                        //make octahedra the parent of the X atoms
                        XatomsArray[i].transform.parent = octahedraArray[m].transform;

                        octahedraArray[m].transform.parent = perovskiteUnitCell[m].transform;
                        perovskiteUnitCell[m].transform.parent = gameObject.transform;

                        XatomsArray[i].GetComponent<Renderer>().material.color = XatomsColor;

                        /*
                        //COLLIDERS??!?
                        //XatomsArray[i].AddComponent<Collider>();
                        XatomsArray[i].GetComponent<Collider>().enabled = false;
                        XatomsArray[i].AddComponent<Rigidbody>();
                        XatomsArray[i].GetComponent<Rigidbody>().useGravity = false;
                        */
                        //add interactible script
                        XatomsArray[i].AddComponent<Interactible>();
                    }
                
            }
            //resetTransforms();
            //configure settings for B atom (center)
            XatomsArray[4].name = "B";
            XatomsArray[4].transform.localScale = new Vector3(BatomsRadius, BatomsRadius, BatomsRadius);
            XatomsArray[4].GetComponent<Renderer>().material.color = BatomsColor;
            //octahedraArray[m].transform.position = XatomCoords[4];

            //og line of code below
            //octahedraArray[m].transform.position = /*XatomsArray[4].transform.position*/ new Vector3(transformX, transformY, transformZ);
            perovskiteUnitCell[m].transform.position = /*XatomsArray[4].transform.position*/ new Vector3(transformX, transformY, transformZ);
            if (m == 2)
            {
                //octahedraArray[m].transform.position = new Vector3(0, 0, 2);
            }
            //Debug.Log(octahedraArray[m].transform.position);

            

            meshVerts = new Vector3[][]{
                new Vector3[3] { XatomCoords[0], XatomCoords[2], XatomCoords[3] },
                new Vector3[3] { XatomCoords[6], XatomCoords[1], XatomCoords[2] },
                new Vector3[3] { XatomCoords[5], XatomCoords[1], XatomCoords[6] },
                new Vector3[3] { XatomCoords[0], XatomCoords[5], XatomCoords[6] },
                new Vector3[3] { XatomCoords[0], XatomCoords[6], XatomCoords[2] },
                new Vector3[3] { XatomCoords[5], XatomCoords[3], XatomCoords[1] },
                new Vector3[3] { XatomCoords[2], XatomCoords[1], XatomCoords[3] },
                new Vector3[3] { XatomCoords[0], XatomCoords[3], XatomCoords[5] },

            };
            
            //test for xatomsarray
            for (int p = 0; p < meshVerts.Length; p++)
            {
                for (int o = 0; o < meshVerts[0].Length; o++)
                {
                    //transform the octahedral planes
                    meshVerts[p][o] += new Vector3(transformX, transformY, transformZ);
                }
            }
            /*
            planeArray[0].AddComponent<MeshFilter>();
            planeArray[0].AddComponent<MeshRenderer>();
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            mesh.vertices = meshVerts[0];
            mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
            mesh.triangles = new int[] { 0, 1, 2 };*/
            //Mesh mesh = new Mesh();



            for (int j = 0; j < planeArray.Length; j++)
            {
                planeArray[j] = new GameObject("plane" + (j + 1));
                filter = planeArray[j].AddComponent<MeshFilter>();
                renderer2 = planeArray[j].AddComponent<MeshRenderer>();
                mesh = filter.mesh;
                mesh.Clear();
                mesh.vertices = meshVerts[j];
                mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
                mesh.triangles = new int[] { 0, 1, 2 };


                Material mat = planeArray[j].GetComponent<Renderer>().material;
                //enable access to transparency renderer
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                planeArray[j].GetComponent<Renderer>().material.color = new Color(0, 1, 1, planeTransparency);


                planeArray[j].transform.parent = octahedraArray[m].transform;
            }
            //if(m==1)
            //transformX = 2;
            ///HERE BEGINS THE TRANSFORMING
            switch (m%7)
            {
                case 0:                    
                    transformX +=2;
                break;

                case 1:
                    //z+=2
                    
                    resetX();
                    transformZ = 2;
                    break;
                case 2:
                    transformX = 2;
                    break;
                case 3:
                    //y+=2
                    resetX();
                    resetZ();
                    transformY += 2;
                    break;

                case 4:

                    //y+=2, x+=2
                transformX += 2;
                    break;
                case 5:
                    //y+=2, z+=2
                    resetX();
                    transformZ += 2;
                    break;
                case 6:

                    //x, z+=2, y+=2
                transformX += 2;
                    break;
                //default: break;
        }
            

            

        }


        //legit transform here
        //DELTA TIME IS ONLY FOR UPDATE FUNCTION?!?!?!
        //octahedraArray[0].transform.Rotate(Vector3.back, /*Time.deltaTime */ 15, Space.Self);
        //octahedraArray[0].transform.Rotate(Vector3.right, /*Time.deltaTime */ 15, Space.Self);
        //for (int t = 0; t < octahedraArray.Length; t++)
        //{

        /*initiating axes of rotation
        rotAxisX = Vector3.right;
        rotAxisY = Vector3.forward;
        rotAxisZ = Vector3.up;
        */
        Xangle = 10f;
        Yangle = 10;
        Zangle = 10;
        /*
        ///a+a+c+
         octahedraArray[0].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 0").FindChild("Octahedra 0").FindChild("B").position, rotAxisX, Xangle);
          octahedraArray[0].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 0").FindChild("Octahedra 0").FindChild("B").position, -rotAxisY, Yangle);
        octahedraArray[0].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 0").FindChild("Octahedra 0").FindChild("B").position, rotAxisZ, Zangle);
        //use this as template pl0x
        //octahedraArray[0].transform.RotateAround(gameObject.transform.FindChild(perovskiteUnitCell[0].name).FindChild(octahedraArray[0].name).FindChild("B").position, Vector3.up, sliderValue);

        octahedraArray[1].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 1").FindChild("Octahedra 1").FindChild("B").position, -rotAxisX, Xangle);
        octahedraArray[1].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 1").FindChild("Octahedra 1").FindChild("B").position, rotAxisY, Yangle);
        octahedraArray[1].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 1").FindChild("Octahedra 1").FindChild("B").position, -rotAxisZ, Zangle);
        
        octahedraArray[2].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 2").FindChild("Octahedra 2").FindChild("B").position, -rotAxisX, Xangle);
        octahedraArray[2].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 2").FindChild("Octahedra 2").FindChild("B").position, rotAxisY, Yangle);
        octahedraArray[2].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 2").FindChild("Octahedra 2").FindChild("B").position, -rotAxisZ, Zangle);

        octahedraArray[3].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 3").FindChild("Octahedra 3").FindChild("B").position, rotAxisX, Xangle);
        octahedraArray[3].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 3").FindChild("Octahedra 3").FindChild("B").position, -rotAxisY, Yangle);
        octahedraArray[3].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 3").FindChild("Octahedra 3").FindChild("B").position, rotAxisZ, Zangle);
        
        octahedraArray[4].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 4").FindChild("Octahedra 4").FindChild("B").position, -rotAxisX, Xangle);
        octahedraArray[4].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 4").FindChild("Octahedra 4").FindChild("B").position, rotAxisY, Yangle);
        octahedraArray[4].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 4").FindChild("Octahedra 4").FindChild("B").position, rotAxisZ, Zangle);
        
        octahedraArray[5].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 5").FindChild("Octahedra 5").FindChild("B").position, rotAxisX, Xangle);
        octahedraArray[5].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 5").FindChild("Octahedra 5").FindChild("B").position, -rotAxisY, Yangle);
        octahedraArray[5].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 5").FindChild("Octahedra 5").FindChild("B").position, -rotAxisZ, Zangle);

        octahedraArray[6].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 6").FindChild("Octahedra 6").FindChild("B").position, rotAxisX, Xangle);
        octahedraArray[6].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 6").FindChild("Octahedra 6").FindChild("B").position, -rotAxisY, Yangle);
        octahedraArray[6].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 6").FindChild("Octahedra 6").FindChild("B").position, -rotAxisZ, Zangle);

        octahedraArray[7].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 7").FindChild("Octahedra 7").FindChild("B").position, -rotAxisX, Xangle);
        octahedraArray[7].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 7").FindChild("Octahedra 7").FindChild("B").position, rotAxisY, Yangle);
        octahedraArray[7].transform.RotateAround(gameObject.transform.FindChild("Perovskite Unit Cell 7").FindChild("Octahedra 7").FindChild("B").position, rotAxisZ, Zangle);
        */
    }
    
    public void rotate(GameObject oct)
    {
        oct.transform.Rotate(Vector3.up * Time.deltaTime * 10, Space.World);
    }
    public void reverseTriIndex(int[] a)
    {
        int temp = 0;
        temp = a[0];
        a[0] = a[1];
        a[1] = temp;
    }
    public void unitCellSelected(/*GameObject obj*/)
    {
        //Material mat = obj.GetComponent<Renderer>().material;
        //mat.color = Color.red;
        Debug.Log("asdf");
        SelectedCubeCage = new GameObject("Selected");
        SelectedCubeCage = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //GameObject parent = GameObject.Find(parentObjName);
        //XatomsArray[i].transform.position = XatomCoords[i];
        //radius
        //XatomsArray[i].transform.localScale = new Vector3(XatomsRadius, XatomsRadius, XatomsRadius);

        //make octahedra the parent of the X atoms
        //XatomsArray[i].transform.parent = octahedraArray[m].transform;
        //octahedraArray[m].transform.parent = perovskiteUnitCell[m].transform;
        //perovskiteUnitCell[m].transform.parent = gameObject.transform;

        //XatomsArray[i].GetComponent<Renderer>().material.color = XatomsColor;
    }
}
