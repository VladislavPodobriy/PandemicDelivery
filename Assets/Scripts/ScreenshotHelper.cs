using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScreenshotHelper : MonoBehaviour
    {
        public static Texture2D Save(int w, int h, int depth, Camera camera, string path)
        {
            var screenshot = RenderFromCamera(w, h, depth, camera);

            File.WriteAllBytes(path, screenshot.EncodeToPNG());

            return screenshot;
        }

        public static Texture2D SaveAndShare(int w, int h, int depth, Camera camera)
        {
            var path = Path.Combine(Application.temporaryCachePath, "shared.png");
        
            var screenshot = Save(w, h, depth, camera, path);

            return screenshot;
        }

        public static Texture2D RenderFromCamera(int w, int h, int depth, Camera camera)
        {
            RenderTexture.active = new RenderTexture(w, h, depth);
            camera.targetTexture = RenderTexture.active;

            Texture2D screenshotTexture = new Texture2D(w, h, TextureFormat.RGBAFloat, false);

            camera.Render();
            screenshotTexture.ReadPixels(new Rect(0, 0, w, h), 0, 0);

            camera.targetTexture = null;
            RenderTexture.active = null;  

            return screenshotTexture;
        }
    }
}
