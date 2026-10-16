namespace Labb_2_Mio.LevelElements
{
    public abstract class Enemy : LevelElement
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }
        public bool IsAlive = true;
        public bool ShouldDraw = true;
        public Enemy(string name, int health, char symbol, ConsoleColor color)
        {
            Name = name;
            Health = health;
            Symbol = symbol;
            Color = color;
        }
        public abstract void Update(Player player, LevelData levelData);
    }
}
