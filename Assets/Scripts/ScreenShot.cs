using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    SpriteRenderer image;
    [SerializeField] SpriteRenderer img;
    private Texture2D currentCapture;
    bool isCapturing = false;
    Vector3 position;
    [SerializeField] Transform head;
    int c = 0;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0,0,0);
    }
    

    public void clickScreenShot()
    {
        ScreenshotToImage();
        position.x = Camera.main.transform.forward.normalized.x * 4;
        position.y = Camera.main.transform.forward.normalized.y * 4;
        position.z = Camera.main.transform.forward.normalized.z * 4;
        image = Instantiate(img);
    }

    IEnumerator CaptureRoutine()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            isCapturing = true;
            currentCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            currentCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            currentCapture.Apply();
            Debug.Log(currentCapture != null);
            Debug.Log(isCapturing);

            Debug.Log("Capturing image");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Screen capture failed!");
            Debug.LogError(e.ToString());
            isCapturing = false;
        }
    }

    void Update()
    {

        if (isCapturing && currentCapture != null)
        {
            Sprite sprite = Sprite.Create(currentCapture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0, 0));
            if (image)
            {
                image.sprite = sprite;
                image.transform.position = head.transform.position + position;
                image.transform.LookAt(head);
                Vector3 theScale = image.transform.localScale;
                if(theScale.x > 0)
                {
                    theScale.x *= -1;
                    image.transform.localScale = theScale;
                }
                
                Debug.Log(position);
            }
            isCapturing = false;
        }
        
    }

    public void ScreenshotToImage()
    {
        StartCoroutine(CaptureRoutine());
    }

}
