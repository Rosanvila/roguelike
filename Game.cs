namespace Roguelike;

public class Game
{
    private readonly Map _map;
    private readonly Hero _hero;
    private bool _running = true;

    public Game()
    {
        _map = new Map(60, 25);
        var start = _map.Rooms[0];
        _hero = new Hero(start.CenterX, start.CenterY);
    }

    public void Run()
    {
        Console.CursorVisible = false;
        while (_running)
        {
            Draw();
            HandleInput();
        }
        Console.CursorVisible = true;
        Console.Clear();
        Console.WriteLine("À bientôt aventurier !");
    }

    private void Draw()
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < _map.Height; y++)
        {
            for (int x = 0; x < _map.Width; x++)
            {
                char c = (x == _hero.X && y == _hero.Y)
                    ? _hero.Glyph
                    : _map.GetTile(x, y) switch
                    {
                        TileType.Wall => '#',
                        TileType.Floor => '.',
                        _ => '?'
                    };
                Console.Write(c);

            }
            Console.WriteLine();
        }
    }

    private void HandleInput()
    {
        var key = Console.ReadKey(intercept: true).Key;

        if (key == ConsoleKey.Escape)
        {
            _running = false;
            return;
        }

        var (dx, dy) = key switch
        {
            ConsoleKey.Z or ConsoleKey.UpArrow => (0, -1),
            ConsoleKey.S or ConsoleKey.DownArrow => (0, 1),
            ConsoleKey.Q or ConsoleKey.LeftArrow => (-1, 0),
            ConsoleKey.D or ConsoleKey.RightArrow => (1, 0),
            _ => (0, 0)
        };

        int newX = _hero.X + dx;
        int newY = _hero.Y + dy;
        if (_map.IsWalkable(newX, newY))
        {
            _hero.X = newX;
            _hero.Y = newY;
        }
    }
}
