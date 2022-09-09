namespace Graphics
{

    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new(1280, 800, "Graphics"))
            {
                game.Run();
            }
        }
    }
}