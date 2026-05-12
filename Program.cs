const int Width = 20;
const int Height = 10;

int heroX = Width / 2;
int heroY = Height / 2;

Console.Clear();

for (int y = 0; y < Height; y++)
{
    for (int x = 0; x < Width; x++)
    {
        char c;
        if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
            c = '#';                       
        else if (x == heroX && y == heroY)
            c = '@';                       
        else
            c = '.';                       

        Console.Write(c);
    }
    Console.WriteLine();
}
