namespace Roguelike;

public class Hero
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Glyph => '@';

    public Hero(int x, int y)
    {
        X = x;
        Y = y;
    }
}
