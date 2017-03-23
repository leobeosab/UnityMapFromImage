using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReader : MonoBehaviour {

    [System.Serializable]
    public class LevelObject
    {
        public Color color;
        public GameObject prefab;
    }

    public LevelObject[] level_items;
    public Texture2D level_data;

    public void Start()
    {
        LoadLevel();
    }

	
	void LoadLevel () {

        int height = level_data.height;
        int width = level_data.width;

        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                foreach(LevelObject obj in level_items)
                {
                    if (level_data.GetPixel(x, y).Equals(obj.color))
                    {
                        Debug.Log("Added item");
                        GameObject tmp = Instantiate(obj.prefab, new Vector2(x * .75f, y * .75f), Quaternion.identity);
                        tmp.transform.parent = this.transform;
                    }
                    else
                    {
                        Debug.Log("Level_Data: " + level_data.ToString());
                        Debug.Log("Object Data: " + obj.color.ToString());
                    }
                } 
            }
        }
	}
}
