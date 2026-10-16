namespace Labb_2_Mio.LevelElements
{
    public abstract class LevelElement
    {
        public int X { get; set; }

        public int Y { get; set; }

        public char Symbol { get; set; }

        public ConsoleColor Color { get; set; }

        public void Move(int deltaX, int deltaY, LevelData levelData)
        {
            int newX = X + deltaX;
            int newY = Y + deltaY;

            if (levelData.IsWall(newX, newY))
            {
                return;
            }

            var occupying = levelData.GetElementAt(newX, newY);

            if (occupying == null)
            {
                X = newX;
                Y = newY;
                return;
            }

            if (this is Player && occupying is Enemy)
            {
                levelData.PendingCombat = (this as Player, occupying as Enemy, true);
                return;
            }

            if (this is Enemy && occupying is Player)
            {
                levelData.PendingCombat = (occupying as Player, this as Enemy, false);
                return;
            }
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y + 4);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);
            Console.ResetColor();
        }
    }
}
