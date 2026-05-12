namespace Roguelike;

public record Room(int X, int Y, int Width, int Height)
{
    public int Right   => X + Width - 1;
    public int Bottom  => Y + Height - 1;
    public int CenterX => X + Width / 2;
    public int CenterY => Y + Height / 2;

    // +1 partout pour garantir au moins 1 mur entre 2 salles
    public bool Intersects(Room other) =>
        X <= other.Right + 1 && Right + 1 >= other.X &&
        Y <= other.Bottom + 1 && Bottom + 1 >= other.Y;
}
