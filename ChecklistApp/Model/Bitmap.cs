namespace ChecklistApp.Model;

public class Bitmap
{
    private static readonly int s_headerSize = 54;
    private byte[] _bitmap;
    private readonly int _width, _height;
    private readonly bool _hasAlpha;
    
    public Bitmap(int width, int height, bool hasAlpha = true)
    {
        _width = width;
        _height = height;
        _hasAlpha = hasAlpha;
        int bytesPerPixel = hasAlpha ? 4 : 3;
        _bitmap = new byte[(width * bytesPerPixel) * height + s_headerSize];
        GenerateHeader(width, height, bytesPerPixel * 8);
    }

    private void GenerateHeader(int width, int height, int colorDepth)  //  TODO: transition from BITMAPINFOHEADER to BITMAPV4HEADER to support alpha channel
    {
        //  BITMAPINFOHEADER Generation, see https://en.wikipedia.org/wiki/BMP_file_format
        byte[] bitmapHeader = new byte[54];
        bitmapHeader[0] = 0x42;
        bitmapHeader[1] = 0x4d;
        Array.Copy(BitConverter.GetBytes(_bitmap.Length), 0, bitmapHeader, 2, 4);                   // File size
        Array.Copy(BitConverter.GetBytes(0), 0, bitmapHeader, 6, 2);                                // Meaningless in this context
        Array.Copy(BitConverter.GetBytes(0), 0, bitmapHeader, 8, 2);                                // Meaningless in this context
        Array.Copy(BitConverter.GetBytes(54), 0, bitmapHeader, 10, 4);                              // Pixel array offset
        Array.Copy(BitConverter.GetBytes(40), 0, bitmapHeader, 14, 4);                              // Length of header (40 bytes)
        Array.Copy(BitConverter.GetBytes(width), 0, bitmapHeader, 18, 4);                           // Image width
        Array.Copy(BitConverter.GetBytes(height), 0, bitmapHeader, 22, 4);                          // Image height
        Array.Copy(BitConverter.GetBytes(1), 0, bitmapHeader, 26, 2);                               // Number of color planes (1)
        Array.Copy(BitConverter.GetBytes(colorDepth), 0, bitmapHeader, 28, 2);                      // Bits per pixel
        Array.Copy(BitConverter.GetBytes(0), 0, bitmapHeader, 30, 4);                               // Compression method (0: none)
        Array.Copy(BitConverter.GetBytes(_bitmap.Length - s_headerSize), 0, bitmapHeader, 34, 4);   // Raw bitmap size
        Array.Copy(BitConverter.GetBytes(0), 0, bitmapHeader, 38, 4);                               // Pixel per meter?? width
        Array.Copy(BitConverter.GetBytes(0), 0, bitmapHeader, 42, 4);                               // Pixel per meter?? height
        Array.Copy(BitConverter.GetBytes(0), 0, bitmapHeader, 46, 4);                               // N colors in palette
        Array.Copy(BitConverter.GetBytes(0), 0, bitmapHeader, 50, 4);                               // Every color is important (???)
        
        Array.Copy(bitmapHeader, 0, _bitmap, 0,  bitmapHeader.Length);
    }

    public void MapPixel(int x, int y, Color color)
    {
        int bytesPerPixel = _hasAlpha ? 4 : 3;
        int offset = ((_width * bytesPerPixel) * y) + (x * bytesPerPixel) + s_headerSize;
        _bitmap[offset] = (byte)(color.Blue * 255);
        _bitmap[offset + 1] = (byte)(color.Green * 255);
        _bitmap[offset + 2] = (byte)(color.Red * 255);
        if (_hasAlpha)
            _bitmap[offset + 3] = (byte)color.Alpha;
    }

    public byte[] AsByteArray()
    {
        return _bitmap;
    }
}