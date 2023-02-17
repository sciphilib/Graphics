using Graphics.WindowStuff;

namespace Graphics
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            using (Window window = new(1200, 900, "Graphics"))
            {
                window.Run();
            }
        }
    }
}