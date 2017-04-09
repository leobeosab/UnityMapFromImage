using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class LevelEditorExtension : EditorWindow
{
    //Simple struct to hold our level object
    [System.Serializable]
    public class LevelObject
    {
        public Color color;
        public GameObject prefab;

        public LevelObject(Color color, GameObject prefab)
        {
            this.color = color;
            this.prefab = prefab;
        }
    }

    private string parent_name;
    private Texture2D level_data;
    private List<LevelObject> level_items = new List<LevelObject>();

    [MenuItem("Window/MapGenerator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one
        LevelEditorExtension window = (LevelEditorExtension)EditorWindow.GetWindow(typeof(LevelEditorExtension));
        window.Show();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        level_data = (Texture2D)EditorGUILayout.ObjectField("Load Map", level_data, typeof(Texture2D), false);
        parent_name = EditorGUILayout.TextField("Parent Object Name: ", "Level");
        if (level_data)
        {
            GUILayout.Label("Custom Objects");

            if (level_items.Count < 1)
                ReadImage();

            GenerateFields();

            if (GUILayout.Button("Generate Level"))
                InsertObjects();
        }
    }

    void GenerateFields()
    {
        for (int i = 0; i < level_items.Count; i++)
        {
            level_items[i].color = EditorGUILayout.ColorField(level_items[i].color);
            level_items[i].prefab = (GameObject)EditorGUILayout.ObjectField("Object to use", level_items[i].prefab, typeof(GameObject), false);
        }
    }

    void ReadImage()
    {
        //Easier to put colors in a list and check if col is in the list than use a for loop over all the LevelObjects
        List<Color> colors = new List<Color>();

        foreach (Color col in level_data.GetPixels())
        {
            if (!colors.Contains(col) && col.a == 1) {
                colors.Add(col);
                level_items.Add( new LevelObject(col, null));
            }
        }

        //Free up some memory
        colors = null;
    }

    void InsertObjects()
    {
        DestroyImmediate(GameObject.Find(parent_name));

        GameObject parent = new GameObject(parent_name);

        int height = level_data.height;
        int width = level_data.width;

        float tile_size = level_items[0].prefab.GetComponent<Renderer>().bounds.size.x;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                foreach (LevelObject obj in level_items)
                {
                    if (level_data.GetPixel(x, y).Equals(obj.color))
                    {
                        GameObject tmp = Instantiate(obj.prefab, new Vector2(x * tile_size, y * tile_size), Quaternion.identity);
                        tmp.transform.parent = parent.transform;
                    }
                }
            }
        }
    }
}
