using UnityEditor;

public class ArtSpritePostprocessor : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith("Assets/Resources/Art/"))
        {
            return;
        }

        TextureImporter textureImporter = assetImporter as TextureImporter;
        if (textureImporter == null)
        {
            return;
        }

        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
        textureImporter.alphaIsTransparency = true;
        textureImporter.mipmapEnabled = false;
        textureImporter.maxTextureSize = 2048;
        textureImporter.textureCompression = TextureImporterCompression.Compressed;
    }
}
