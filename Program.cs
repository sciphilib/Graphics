﻿namespace Graphics
{

    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new())
            {
                game.Run();
            }
        }
    }
}