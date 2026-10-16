using Labb_2_Mio.LevelElements;

namespace Labb_2_Mio
{
    public class LevelData
    {
        private List<LevelElement> elements = new();

        public IReadOnlyList<LevelElement> Elements => elements;

        public Player player { get; set; }

        public (Player player, Enemy enemy, bool playerInitiated)? PendingCombat { get; set; }

        public void Load(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    char ch = lines[y][x];

                    switch (ch)
                    {
                        case '#':
                            elements.Add(new Wall(x, y));
                            break;

                        case 'r':
                            elements.Add(new Rat(x, y));
                            break;

                        case 's':
                            elements.Add(new Snake(x, y));
                            break;

                        case '@':
                            player = new Player(x, y);
                            elements.Add(player);
                            break;
                    }
                }
            }

        }

        public bool IsWall(int x, int y)
        {
            foreach (var element in elements)
            {
                if (element is Wall && element.X == x && element.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public LevelElement GetElementAt(int x, int y)
        {
            return elements.FirstOrDefault(e => e.X == x && e.Y == y);
        }
        public void RemoveEnemy(Enemy enemy)
        {
            elements.Remove(enemy);
        }
    }
}
