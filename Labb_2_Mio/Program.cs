using Labb_2_Mio;
using Labb_2_Mio.LevelElements;
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Run();
    }
    public class Game
    {
        private string combatLog = string.Empty;
        private LevelData levelData = new LevelData();
        private bool _isRunning = true;

        public void Run()
        {
            levelData.Load(".\\LevelFiles\\Level1.txt");
            while (_isRunning)
            {
                Draw();
                PlayerInput();
                Update();
            }
        }

        private void Draw()
        {
            Console.Clear();
            PlayerStats();
            Console.WriteLine(combatLog);
            combatLog = string.Empty;

            foreach (var element in levelData.Elements)
            {
                double distance = Math.Sqrt(Math.Pow(levelData.player.X - element.X, 2) + Math.Pow(levelData.player.Y - element.Y, 2));

                if (element is Enemy enemy && !enemy.ShouldDraw)
                {
                    levelData.RemoveEnemy(enemy);
                    continue;
                }

                if (distance <= 5)
                {
                    element.Draw();

                    if (element is Wall wall)
                    {
                        wall.IsDiscovered = true;
                    }
                }
                else if (element is Wall wall && wall.IsDiscovered)
                {
                    wall.Draw();
                }
            }
        }
        public void PlayerInput()
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape)
            {
                _isRunning = false;
                return;
            }
            levelData.player.Input(key, levelData);
        }

        public void Update()
        {
            if (levelData.PendingCombat.HasValue)
            {
                var pendingCombat = levelData.PendingCombat.Value;
                CombatEncounter(pendingCombat.player, pendingCombat.enemy, levelData, pendingCombat.playerInitiated);

                if (!pendingCombat.enemy.IsAlive)
                {
                    levelData.RemoveEnemy(pendingCombat.enemy);
                }
                levelData.PendingCombat = null;
            }

            var elementsCopy = new List<LevelElement>(levelData.Elements);

            foreach (var element in elementsCopy)
            {
                if (element is Enemy enemy)
                {
                    enemy.Update(levelData.player, levelData);

                    if (levelData.PendingCombat.HasValue)
                    {
                        var pendingCombat = levelData.PendingCombat.Value;
                        CombatEncounter(pendingCombat.player, pendingCombat.enemy, levelData, pendingCombat.playerInitiated);

                        if (!pendingCombat.enemy.IsAlive)
                        {
                            levelData.RemoveEnemy(pendingCombat.enemy);
                        }

                        levelData.PendingCombat = null;
                    }
                }
            }
        }

        public void PlayerStats()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"Name: {levelData.player.Name} | ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Health: {levelData.player.Health} | ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Moves: {levelData.player.Moves}");
            Console.ResetColor();
        }

        public void CombatEncounter(Player player, Enemy enemy, LevelData levelData, bool playerInitiated)
        {
            string combatOutcome = "";

            if (playerInitiated)
            {
                int playerAttackDamage = player.AttackDice.Throw();
                int enemyDefenceReduction = enemy.DefenceDice.Throw();
                int damageToEnemy = playerAttackDamage - enemyDefenceReduction;

                combatOutcome += $"You (ATK: {player.AttackDice} => {playerAttackDamage}) attacked the {enemy.Name} (DEF: {enemy.DefenceDice} => {enemyDefenceReduction})\n";

                if (damageToEnemy > 0)
                {
                    enemy.Health -= damageToEnemy;
                }

                if (enemy.Health <= 0)
                {
                    combatOutcome += $"The {enemy.Name} has been defeated\n";
                    enemy.ShouldDraw = false;
                    levelData.RemoveEnemy(enemy);
                    combatLog = combatOutcome;
                    return;
                }

                int enemyAttackDamage = enemy.AttackDice.Throw();
                int playerDefenceReduction = player.DefenceDice.Throw();
                int damageToPlayer = enemyAttackDamage - playerDefenceReduction;

                combatOutcome += $"The {enemy.Name} (ATK: {enemy.AttackDice} => {enemyAttackDamage}) attacked you (DEF: {player.DefenceDice} => {playerDefenceReduction})\n";

                if (damageToPlayer > 0)
                {
                    player.Health -= damageToPlayer;

                    if (player.Health <= 0)
                    {
                        combatOutcome += "You have been defeated.. Game Over.\n";
                        Console.WriteLine();
                        Console.WriteLine(combatOutcome);
                        Console.ReadKey();
                        Environment.Exit(0);
                        return;
                    }
                }
            }
            else
            {
                int enemyAttackDamage = enemy.AttackDice.Throw();
                int playerDefenceReduction = player.DefenceDice.Throw();
                int damageToPlayer = enemyAttackDamage - playerDefenceReduction;

                combatOutcome += $"The {enemy.Name} (ATK: {enemy.AttackDice} => {enemyAttackDamage}) attacked you (DEF: {player.DefenceDice} => {playerDefenceReduction})\n";

                if (damageToPlayer > 0)
                {
                    player.Health -= damageToPlayer;

                    if (player.Health <= 0)
                    {
                        combatOutcome += "You have been defeated.. Game Over.\n";
                        Console.WriteLine();
                        Console.WriteLine(combatOutcome);
                        Console.ReadKey();
                        Environment.Exit(0);
                        return;
                    }
                }

                int playerAttackDamage = player.AttackDice.Throw();
                int enemyDefenceReduction = enemy.DefenceDice.Throw();
                int damageToEnemy = playerAttackDamage - enemyDefenceReduction;

                combatOutcome += $"You (ATK: {player.AttackDice} => {playerAttackDamage}) attacked the {enemy.Name} (DEF: {enemy.DefenceDice} => {enemyDefenceReduction})\n";

                if (damageToEnemy > 0)
                {
                    enemy.Health -= damageToEnemy;
                }

                if (enemy.Health <= 0)
                {
                    combatOutcome += $"The {enemy.Name} has been defeated\n";
                    enemy.ShouldDraw = false;
                    levelData.RemoveEnemy(enemy);
                }
            }

            combatLog = combatOutcome;
        }

    }
}