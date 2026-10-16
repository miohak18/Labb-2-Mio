namespace Labb_2_Mio.LevelElements
{
    public class Player : LevelElement

    {
        public string Name = "Adventurer";

        public int Health = 50;

        public int Moves = 0;

        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }

        public Player(int x, int y)
        {

            AttackDice = new Dice(2, 6, 2);
            DefenceDice = new Dice(2, 6, 2);
            Symbol = '@';
            Color = ConsoleColor.Yellow;
            X = x;
            Y = y;
        }

        public void Input(ConsoleKey key, LevelData levelData)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow: Move(0, -1, levelData); break;
                case ConsoleKey.DownArrow: Move(0, 1, levelData); break;
                case ConsoleKey.LeftArrow: Move(-1, 0, levelData); break;
                case ConsoleKey.RightArrow: Move(1, 0, levelData); break;
                case ConsoleKey.Spacebar: Move(0, 0, levelData); break;
            }
            Moves++;
        }
    }
}
