using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Text;
using System;


public class GenerateAtom : MonoBehaviour
{
    [Tooltip("The perovskite crystal lattice will be x units by x units")]
    public int dimensions = 1;
    [Tooltip("Size of unit cell")]
    public int unit = 1;
    private int unitTransformed = 1;

    private GameObject[] OctahedraArray;
    private GameObject[] AatomsArray = new GameObject[8];
    private static GameObject[] XatomsArray = new GameObject[7];

    [Tooltip("Atom coordinates")]
    public Vector3[] XatomCoords = new Vector3[7];
    public Vector3[] AatomCoords = new Vector3[8];

    //public float testVar = 0.5f;


    //public MeshFilter[] mf = new MeshFilter[8];
    private Mesh[] meshArray = new Mesh[8];
    private Mesh[] meshArrayCopy = new Mesh[8];

    private GameObject[] planeArray = new GameObject[8];
    //this array is necessary for rendering both sides of the mesh
    private GameObject[] planeArrayCopy = new GameObject[8];

    //2D array for atom arrangements (will be fed in)
    private Vector3[][] meshVerts;

    //variable for transparency
    private static float alphaLevel = 0.6f;

    private Color planeColor = new Color(0, 1, 1, alphaLevel);

    [Tooltip("Atom Radius")]
    //radius of atoms
    public float XatomRadius = 0.1f;
    public float AatomRadius = 0.5f;
    public float BatomRadius = 0.5f;

    [Tooltip("Transform whole octahedra by:")]
    public float transformX = 0;
    public float transformY = 0;
    public float transformZ = 10;
    private Vector3 transformOct = new Vector3();

    public static GameObject center;

    public static GenerateAtom Instance;
    //UnityEngine.Object[] prefabArr = new UnityEngine.Object[7];

    public int octCounter = 1;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
        
    }
    void Start()
    {
        
        
        //generate initial Octahedra at origin
        generateOctahedra();
        /*
        //generate Octahedra 2 at 200
        transformX = 2;
        generateOctahedra();
        resetTransforms();
        
        //generate at 002
        transformZ = 12;
        generateOctahedra();
        resetTransforms();
        //generate at 202
        transformX = 2;
        transformZ = 12;
        generateOctahedra();
        resetTransforms();
        //generate at 020
        transformY = 2;
        transformZ = 10;
        generateOctahedra();
        resetTransforms();
        //generate at 220
        transformX = 2;
        transformY = 2;
        transformZ = 10;
        generateOctahedra();
        resetTransforms();
        //generate at 022
        transformY = 2;
        transformZ = 12;
        generateOctahedra();
        resetTransforms();
        //generate 222
        transformX = 2;
        transformY = 2;
        transformZ = 12;
        generateOctahedra();
        resetTransforms();
        */

    }
    public void extendLattice()
    {
        
            //new transforming goes here
            unitTransformed = 2 * unit;
            //generate Octahedra 2 at 200
            transformX += unitTransformed;
            generateOctahedra();
            resetTransforms();

        //generate at 002
        //transformZ = 12;
        transformZ += unitTransformed;
            generateOctahedra();
            resetTransforms();
            //generate at 202
            transformX = 2;
            transformZ = 12;
            generateOctahedra();
            resetTransforms();
            //generate at 020
            transformY = 2;
            transformZ = 10;
            generateOctahedra();
            resetTransforms();
            //generate at 220
            transformX = 2;
            transformY = 2;
            transformZ = 10;
            generateOctahedra();
            resetTransforms();
            //generate at 022
            transformY = 2;
            transformZ = 12;
            generateOctahedra();
            resetTransforms();
            //generate 222
            transformX = 2;
            transformY = 2;
            transformZ = 12;
            generateOctahedra();
            resetTransforms();
            


    }

    public void resetTransforms()
    {
        transformX = 0;
        transformY = 0;
        transformZ = 0;
    }
    //this generates one perovskite unit cell (rename this later
    public void generateOctahedra()
    {
        for (int m = 0; m < dimensions*dimensions; m++)
        {
            OctahedraArray = new GameObject[dimensions*dimensions];
            OctahedraArray[m] = new GameObject("Octahedra " + octCounter);

            //EVENTUALLY REPLACE THESE WITH A UNIT VARIABLE?!?!
            //hardcode X atom coords
            XatomCoords[0] = new Vector3(0, -unit, 0);
            XatomCoords[1] = new Vector3(0, unit, 0);
            XatomCoords[2] = new Vector3(unit, 0, 0);
            XatomCoords[3] = new Vector3(0, 0, unit);
            XatomCoords[4] = new Vector3(0, 0, 0);
            XatomCoords[5] = new Vector3(-unit, 0, 0);
            XatomCoords[6] = new Vector3(0, 0, -unit);

            //hardcode A atom coords
            AatomCoords[0] = new Vector3(unit, -unit, unit);
            AatomCoords[1] = new Vector3(unit, -unit, -unit);
            AatomCoords[2] = new Vector3(-unit, -unit, -unit);
            AatomCoords[3] = new Vector3(-unit, unit, -unit);
            AatomCoords[4] = new Vector3(unit, unit, unit);
            AatomCoords[5] = new Vector3(unit, unit, -unit);
            AatomCoords[6] = new Vector3(-unit, unit, unit);
            AatomCoords[7] = new Vector3(-unit, -unit, unit);

            transformOct = new Vector3(transformX, transformY, transformZ);
            //loop for creating atoms and assigning respective coordinates
            for (int n = 0; n < AatomsArray.Length; n++)
            {
                if (n < XatomsArray.Length)
                {
                    XatomsArray[n] = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                    //apply change to XatomCoords if there is an input
                    XatomCoords[n] = XatomCoords[n] + transformOct;
                    XatomsArray[n].transform.position = XatomCoords[n];

                    //change color (of all atoms in the array D:)
                    XatomsArray[n].GetComponent<Renderer>().material.color = Color.green;

                    //Makes the current gameobject the parent of the atoms
                    //atomsArray[n].transform.parent = gameObject.transform;
                    XatomsArray[n].transform.parent = OctahedraArray[m].transform;

                    //change radius of sphere
                    XatomsArray[n].transform.localScale = new Vector3(XatomRadius, XatomRadius, XatomRadius);

                    //apply Interactible.cs script from Hololens ModelExplorer Demo
                    XatomsArray[n].AddComponent(Type.GetType("Interactible"));

                    //Debug.Log("asdf");
                    /*
                    //create empty prefab to prepare prefab asset
                    prefabArr[n] = PrefabUtility.CreatePrefab("Assets/prefabtestfolder/Whatever " + n + ".prefab", XatomsArray[n], ReplacePrefabOptions.ConnectToPrefab);
                    prefabArr[n].hideFlags = HideFlags.HideInHierarchy;*/
                }
                AatomsArray[n] = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                //apply change to AatomCoords if there is an input
                AatomCoords[n] = AatomCoords[n] + transformOct;
                AatomsArray[n].transform.position = AatomCoords[n];

                //change color (of all atoms in the array D:)
                AatomsArray[n].GetComponent<Renderer>().material.color = Color.white;

                //Makes the current gameobject the parent of the atoms
                //AatomsArray[n].transform.parent = Octahedra.transform;
                AatomsArray[n].transform.parent = gameObject.transform;
                //change radius of sphere
                AatomsArray[n].transform.localScale = new Vector3(AatomRadius, AatomRadius, AatomRadius);

                //apply Interactible.cs script from Hololens ModelExplorer Demo
                AatomsArray[n].AddComponent(Type.GetType("Interactible"));
                //Debug.Log(n);

            }
            //radius of center
            XatomsArray[4].transform.localScale = new Vector3(BatomRadius, BatomRadius, BatomRadius);
            /*
            prefabArr[0] = PrefabUtility.CreatePrefab("Assets/prefabtestfolder/Whatever " + 1 + ".prefab", XatomsArray[0], ReplacePrefabOptions.ConnectToPrefab);
            prefabArr[1] = PrefabUtility.CreatePrefab("Assets/prefabtestfolder/Whatever " + 2 + ".prefab", XatomsArray[1], ReplacePrefabOptions.ConnectToPrefab);*/
            center = XatomsArray[4];
            meshVerts = new Vector3[][] {
            new Vector3[] { XatomCoords[0], XatomCoords[3], XatomCoords[2] },
            new Vector3[] { XatomCoords[6], XatomCoords[2], XatomCoords[1] },
            new Vector3[] { XatomCoords[5], XatomCoords[1], XatomCoords[6] },
            new Vector3[] { XatomCoords[0], XatomCoords[5], XatomCoords[6] },
            new Vector3[] { XatomCoords[0], XatomCoords[6], XatomCoords[2] },
            new Vector3[] { XatomCoords[5], XatomCoords[3], XatomCoords[1] },
            new Vector3[] { XatomCoords[2], XatomCoords[1], XatomCoords[3] },
            new Vector3[] { XatomCoords[6], XatomCoords[5], XatomCoords[3] }
        };


            //loop for instantiating new plane array objects
            for (int j = 0; j < meshArray.Length; j++)
            {
                planeArray[j] = new GameObject("plane " + (j + 1));
                planeArrayCopy[j] = new GameObject("plane copy" + (j + 1));

                planeArray[j].AddComponent<MeshFilter>();
                planeArrayCopy[j].AddComponent<MeshFilter>();
                planeArray[j].AddComponent<MeshRenderer>();
                planeArrayCopy[j].AddComponent<MeshRenderer>();

                meshArray[j] = new Mesh();
                meshArrayCopy[j] = new Mesh();

                meshArray[j] = planeArray[j].GetComponent<MeshFilter>().mesh;
                meshArrayCopy[j] = planeArrayCopy[j].GetComponent<MeshFilter>().mesh;

                meshArray[j].Clear();
                meshArrayCopy[j].Clear();


                meshArray[j].vertices = meshVerts[j];
                meshArrayCopy[j].vertices = meshVerts[j];

                meshArray[j].uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
                meshArrayCopy[j].uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };

                meshArray[j].triangles = new int[] { 0, 2, 1 };
                meshArrayCopy[j].triangles = swapTriIndices(meshArray[j].triangles);


                //set plane color
                planeArray[j].GetComponent<Renderer>().material.color = planeColor;
                planeArrayCopy[j].GetComponent<Renderer>().material.color = planeColor;

                //access rendering mode and enable transparency
                planeArray[j].GetComponent<Renderer>().material.SetFloat("_Mode", 3);
                planeArray[j].GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                planeArray[j].GetComponent<Renderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                planeArray[j].GetComponent<Renderer>().material.SetInt("_ZWrite", 0);
                planeArray[j].GetComponent<Renderer>().material.DisableKeyword("_ALPHATEST_ON");
                planeArray[j].GetComponent<Renderer>().material.EnableKeyword("_ALPHABLEND_ON");
                planeArray[j].GetComponent<Renderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                planeArray[j].GetComponent<Renderer>().material.renderQueue = 3000;

                planeArrayCopy[j].GetComponent<Renderer>().material.SetFloat("_Mode", 3);
                planeArrayCopy[j].GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                planeArrayCopy[j].GetComponent<Renderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                planeArrayCopy[j].GetComponent<Renderer>().material.SetInt("_ZWrite", 0);
                planeArrayCopy[j].GetComponent<Renderer>().material.DisableKeyword("_ALPHATEST_ON");
                planeArrayCopy[j].GetComponent<Renderer>().material.EnableKeyword("_ALPHABLEND_ON");
                planeArrayCopy[j].GetComponent<Renderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                planeArrayCopy[j].GetComponent<Renderer>().material.renderQueue = 3000;

                //Makes the current gameobject the parent of the planes
                planeArray[j].transform.parent = OctahedraArray[m].transform;
                planeArrayCopy[j].transform.parent = OctahedraArray[m].transform;
            }
            //increment octahedra counter to update the name of gameobject
            octCounter++;
            //make octahedra child of current gameobject (Perovskite)
            OctahedraArray[m].transform.parent = gameObject.transform;

            





        }
    }

    // Update is called once per frame
    void Update()
{
        rotateObject();
    }

    public void rotateObject()
    {
        //make sure coordinates are correct
        //OctahedraArray[0].transform.RotateAround(new Vector3(0, 0, 10), Vector3.up, 20 * Time.deltaTime);
    }
//vector3[] toString method
public static string SerializeVector3Array(Vector3[] aVectors)
{
    StringBuilder sb = new StringBuilder();
    foreach (Vector3 v in aVectors)
    {
        sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append("|");
    }
    if (sb.Length > 0) // remove last "|"
        sb.Remove(sb.Length - 1, 1);
    return sb.ToString();
}
//toString method for debugging
public static string toString(int[] a)
{
    String str = "";
    for (var i = 0; i < a.Length; i++)
    {
        str = str + a[i].ToString();
    }
    return str;
}
//assumes int array only contains 3 points (because triangle)
public int[] swapTriIndices(int[] a)
{
    int temp = 0;
    a[0] = temp;
    a[0] = a[1];
    a[1] = temp;
    return a;
}
public GameObject[] getGameObjectArr()
{
    return XatomsArray;
}


}
