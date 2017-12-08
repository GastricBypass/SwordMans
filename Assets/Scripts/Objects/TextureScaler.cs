using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class TextureScaler : MonoBehaviour
{
    public Texture texture;
    public float size = 2f;

    // Only one of these can be set at a time or else it will default to the first one.
    public bool scaleToXZ = true;
    public bool scaleToXY = false;
    public bool scaleToYZ = false;

    Vector3 prevScale = Vector3.one;
    float prevTextureScale = -1f;

    // Use this for initialization
    void Start()
    {
        if (texture == null)
        {
            texture = this.GetComponent<Renderer>().sharedMaterial.mainTexture;
        }

        this.prevScale = gameObject.transform.lossyScale;
        this.prevTextureScale = this.size;

        this.UpdateTiling();
    }

    // Update is called once per frame
    void Update()
    {
        
        //If something has changed
        if (gameObject.transform.lossyScale != prevScale || !Mathf.Approximately(this.size, prevTextureScale))
            this.UpdateTiling();

        // Maintain previous state variables
        this.prevScale = gameObject.transform.lossyScale;
        this.prevTextureScale = this.size;
        

    }

    [ContextMenu("UpdateTiling")]
    void UpdateTiling()
    {
        // A Unity plane is 10 units x 10 units
        float planeSize = 10f;

        // Figure out texture-to-mesh width based on user set texture-to-mesh height
        float textureScale = ((float)this.texture.width / this.texture.height) * this.size;

        Vector2 newTextureScale = Vector2.one;

        if (scaleToXZ)
        {
            newTextureScale = new Vector2(planeSize * gameObject.transform.lossyScale.x / textureScale, planeSize * gameObject.transform.lossyScale.z / size);
        }
        else if (scaleToXY) // ^v^v These are switched and we can't explain why
        {
            newTextureScale = new Vector2(planeSize * gameObject.transform.lossyScale.x / textureScale, planeSize * gameObject.transform.lossyScale.y / size);
        }
        else if (scaleToYZ)
        {
            newTextureScale = new Vector2(planeSize * gameObject.transform.lossyScale.z / textureScale, planeSize * gameObject.transform.lossyScale.y / size);

        }

        this.gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureScale = newTextureScale;

    }
}