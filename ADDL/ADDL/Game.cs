using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDL
{
    internal sealed class Game : MainMenu
    {
        const int sizeOfMap = 80;

        private static Character hero;
        private enum Statuses { Alive, Dead, EndGame }
        private Statuses heroStatus;
        private static char[,] map;
        private int heroX;
        private int heroY;
        private int questX;
        private int questY;
        private int latestVicinity;

        private Event currentEvent;

        Random rand = new Random(Guid.NewGuid().GetHashCode());

        public void NewGame()
        {
            GenerateMap();

            //generating hero
            while (true)
            {
                Console.WriteLine("Choose your class (warrior / rogue):");
                string reply = Console.ReadLine().ToLower();

                if (reply == "warrior" || reply == "w" || reply == "rogue" || reply == "r")
                {
                    hero = new Character(reply);
                    break;
                }
                else
                    Console.WriteLine("This doesn't make any scence to me.");
            }
            Console.WriteLine("\n\n");
            hero.ShowStats();
            heroStatus = Statuses.Alive;

            Console.WriteLine("\nYou wake up early in the morning by the cold bonfire in the forest alone." +
                "\nYou don't know who you are nor how you got here. " +
                "The only thing that you are sure about is that you have to go somewhere." +
                "This thought is very obsessive and it doesn't leave your head." +
                "\nYou inspect your belongings. This is what you've found:\n");
            hero.ShowInventory();

            Console.WriteLine("It's time to get moving. Your instinct tells you where you should go and you face north.");

            while (heroStatus == Statuses.Alive)
            {
                Console.Write("\nWhat to do? (go North / go West / go East / go South): ");
                string reply = Console.ReadLine().ToLower();
 
                switch (reply)
                {
                    case "go north":
                    case "n":
                        ProcessMove('n');
                        break;
                    case "go west":
                    case "w":
                        ProcessMove('w');
                        break;
                    case "go east":
                    case "e":
                        ProcessMove('e');
                        break;
                    case "go south":
                    case "s":
                        ProcessMove('s');
                        break;
                    case "i":
                        hero.ManageInventory();
                        break;
                    case "c":
                        hero.ShowStats();
                        break;
                    case "show map":
                        OutputMap();
                        break;
                    default:
                        Console.WriteLine("This doesn't make any scence to me. Try again.");
                        break;
                }
                CheckQuestProgress();
                CheckHeroStatus();
            }
        }

        private void CheckQuestProgress()
        {
            if (heroX == questX && heroY == questY)
            {
                Console.WriteLine("You are walking onto the meadow with bonfire in center. There is two large flat rocks by it. " +
                    "On one of them you see man in black robe. He has been waiting for you...\n\n" + 
                    "You have reached the end of the game. Thanks for playing ang I hope you enjoyed.\n\n\n");
                heroStatus = Statuses.EndGame;
                return;
            }

            int newVicinity = Math.Abs(heroX - questX) + Math.Abs(heroY - questY);

            //Console.WriteLine(newVicinity - latestVicinity); // gebug info

            if (newVicinity - latestVicinity >= 3)
            {
                PM.OutputMessage("   You scence that you are getting ", Red, "farther away", White, " from your destination. You feel almost ill.");
                latestVicinity = Math.Abs(heroX - questX) + Math.Abs(heroY - questY);
            }
            else if (newVicinity - latestVicinity <= -3)
            {
                PM.OutputMessage(White, "   You scence that now you are ", Green,"closer", White, " to your destination. You feel your mood rising.");
                latestVicinity = Math.Abs(heroX - questX) + Math.Abs(heroY - questY);
            }
        }

        private void CheckHeroStatus()
        {
            if (hero.HP <= 0)
            {
                heroStatus = Statuses.Dead;
                Console.WriteLine("   You are dead.\n\n");
            }
        }

        private void ProcessMove(char p)
        {
            map[heroX, heroY] = ','; // visited location
            switch (p)
            {
                case 'n':
                    heroX--;
                    break;
                case 'e':
                    heroY++;
                    break;
                case 'w':
                    heroY--;
                    break;
                case 's':
                    heroX++;
                    break;
            }

            char place = map[heroX, heroY]; // get place before changing

            map[heroX, heroY] = 'H'; // new location
            
            switch (place)
            {
                case 'f':
                    Console.WriteLine("   Forest's floor covered by fallen leafs and branches.");
                    break;
                case '_':
                    Console.WriteLine("   You walk across empty field. There is some grass and bushes.");
                    break;
                case ',':
                    Console.WriteLine("   You've already been here.");
                    break;
                case '.':
                    Console.WriteLine("   You walk on the dried land. Almost nothing grows here.");
                    break;
                case 'w':
                    Console.WriteLine("   This lifeless landscape worries you. You don't want to go there.");
                    break;
                case 'W':
                    Console.WriteLine("   This is wasteland. Nothing alive here. You feel depressed. You start crying.");
                    break;
                case 'X':
                    Console.WriteLine("   You lay on the ground and die. At least now you are free.");
                    heroStatus = Statuses.Dead;
                    break;
                case 't':
                    PM.OutputMessage(White, "   You notice loud grunting and noises of rocks clanking to the ", Green, GetEventDirection(place), White, ".");
                    Console.WriteLine();
                    break;
                case 'T':
                    Console.WriteLine("   You see big ugly troll sitting by his cave. He's bashing rocks against each other. For some reason.");
                    startEvent(place);
                    break;
                case 'b':
                    PM.OutputMessage(White, "   Light of fireplace shines through branches to the ", Green, GetEventDirection(place), White, ".\n");
                    break;
                case 'B':
                    Console.WriteLine("   From your cover you see the bonfire and some peole near it.");
                    startEvent(place);
                    break; 
                case 'v':
                    PM.OutputMessage(White, "   Over the crowns of trees to the ", Green, GetEventDirection(place), White, " you notice tall church bell tower.\n");
                    break;
                case 'V':
                    Console.WriteLine("   It's an abandoned village. You see some houses, but not much.");
                    startEvent(place);
                    break;
                case 'c':
                    PM.OutputMessage(White, "   As you walk, your way passes flock of bats. They're flying to the ", Green, GetEventDirection(place), White, ".\n");
                    break;
                case 'C':
                    Console.WriteLine("   You've found a cave. It's not too deep, sun will shine your way inside.");
                    startEvent(place);
                    break;
                case 'm':
                    PM.OutputMessage(White, "   Some invisible power pulls you to the ", Green, GetEventDirection(place), White, ". You can resist, but should you?\n");
                    break;
                case 'M':
                    Console.WriteLine("   You've found a small lake. It shines by itself. It's definately magical.");
                    startEvent(place);
                    break;
                case 'q':
                    PM.OutputMessage(White, "You feel like you are almost there. Invisible force pulls you to the ", Green, GetEventDirection(place), White, ".\n");
                    break;
            }
        }

        private string GetEventDirection(char p)
        {
            p = Char.ToUpper(p);
            if (map[heroX + 1, heroY] == p)
                return "South";
            else if (map[heroX - 1, heroY] == p)
                return "North";
            else if (map[heroX, heroY + 1] == p)
                return "East";
            else if (map[heroX, heroY - 1] == p)
                return "West";
            else return "";

            //PM.OutputMessage(White, beginning, ConsoleColor.Green, eventDirection, ConsoleColor.White, ending);
        }

        private void startEvent(char place)
        {
            currentEvent = new Event(place, ref hero);
            currentEvent.StartEvent();

            CheckHeroStatus();
            if (heroStatus == Statuses.Alive)
            {
                Console.WriteLine("   You are away enough from the place that can't see it at all. You have a feeling that you will never see it again.");
                ClearCurrentPosition();
            }
        }

        private void ClearCurrentPosition()
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());

            if (map[heroX, heroY + 1] != ',')
                map[heroX, heroY + 1] = rand.Next(2) == 0 ? '_' : 'f';
            if (map[heroX, heroY - 1] != ',')
                map[heroX, heroY - 1] = rand.Next(2) == 0 ? '_' : 'f';
            if (map[heroX + 1, heroY] != ',')
                map[heroX + 1, heroY] = rand.Next(2) == 0 ? '_' : 'f';
            if (map[heroX - 1, heroY] != ',')
                map[heroX - 1, heroY] = rand.Next(2) == 0 ? '_' : 'f';
        }

        // map
        public void GenerateMap()
        {
            map = new char[sizeOfMap, sizeOfMap];

            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            heroX = sizeOfMap - 10;
            heroY = rand.Next(10, sizeOfMap - 10);
            map[heroX, heroY] = 'H'; // hero

            //rand = new Random(Guid.NewGuid().GetHashCode());
            questY = rand.Next(10, sizeOfMap - 10); // get x axys for quest
            questX = 10;
            map[questX, questY] = 'Q'; // quest
            map[questX, questY + 1] = 'q';
            map[questX, questY - 1] = 'q';
            map[questX+1, questY] = 'q';
            map[questX-1, questY] = 'q';

            latestVicinity = Math.Abs(heroX - questX) + Math.Abs(heroY - questY);

            for (int i = 0; i < sizeOfMap; i++)
                for (int j = 0; j < sizeOfMap; j++)
                {
                    if ((i == 0 || j == 0) || (i == sizeOfMap - 1 || j == sizeOfMap - 1))
                        map[i, j] = 'X'; // dead lands
                    else if ((i == 1 || j == 1) || (i == sizeOfMap - 2 || j == sizeOfMap - 2))
                        map[i, j] = 'W'; // wasteland
                    else if ((i == 2 || j == 2) || (i == sizeOfMap - 3 || j == sizeOfMap - 3))
                        map[i, j] = 'w'; // get worried
                    else if ((i == 3 || j == 3 || i == 4 || j == 4)
                        || (i == sizeOfMap - 4 || j == sizeOfMap - 4 || i == sizeOfMap - 5 || j == sizeOfMap - 5)
                        && (map[i, j] == '\0'))
                        map[i, j] = '.'; // nothing should be here (except notifications)
                    else
                    {
                        //rand = new Random(Guid.NewGuid().GetHashCode());
                        if (map[i, j] == '\0')
                            if (rand.Next(2) == 0)
                                map[i, j] = '_'; // empty field
                            else
                                map[i, j] = 'f'; // forest

                        if (CheckForPlaceAround(i, j)) // if there is a place r, l, f, b
                        {
                            //rand = new Random(Guid.NewGuid().GetHashCode());
                            if (rand.Next(20) == 0) // 5% chance
                                GenerateEvent(i, j);
                        }
                    }
                }
        }

        private bool CheckForPlaceAround(int i, int j)
        {
            if ((map[i, j] == '_' || map[i, j] == 'f' || map[i, j] == '.' || map[i, j] == '\0') // current position
                && (map[i + 1, j] == '_' || map[i + 1, j] == 'f' || map[i + 1, j] == '.' || map[i + 1, j] == '\0') // to the right
                && (map[i - 1, j] == '_' || map[i - 1, j] == 'f' || map[i - 1, j] == '.' || map[i - 1, j] == '\0') // to the left
                && (map[i, j + 1] == '_' || map[i, j + 1] == 'f' || map[i, j + 1] == '.' || map[i, j + 1] == '\0') // forward
                && (map[i, j - 1] == '_' || map[i, j - 1] == 'f' || map[i, j - 1] == '.' || map[i, j - 1] == '\0')) // bacwards
                return true;
            else
                return false;
        }

        private void GenerateEvent(int i, int j)
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            char val;
            switch (rand.Next(5))
            {
                case 0:
                    val = 'T'; //troll
                    break;
                case 1:
                    val = 'B'; //bonfire
                    break;
                case 2:
                    val = 'V'; //abandoned village
                    break;
                case 3:
                    val = 'C'; //cave
                    break;
                case 4:
                    val = 'M'; //magic lake
                    break;
                default:
                    val = 'f'; //forest. Shouldn't happen.
                    break;
            }
            map[i, j] = val;
            //string temp = val.ToString();
            //temp.ToLower();
            val = Char.ToLower(val);// temp[0];
            map[i + 1, j] = val;
            map[i - 1, j] = val;
            map[i, j + 1] = val;
            map[i, j - 1] = val;
        }

        private void OutputMap()
        {
            for (int i = 0; i < sizeOfMap; i++)
            {
                for (int j = 0; j < sizeOfMap; j++)
                {
                    if (map[i, j] == 'H' || map[i, j] == 'Q' || map[i, j] == 'q')
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (map[i, j] == '_' || map[i, j] == 'f' || map[i, j] == ',' || map[i, j] == '.' || map[i, j] == 'X' || map[i, j] == 'w' || map[i, j] == 'W')
                        Console.ForegroundColor = ConsoleColor.Gray;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write(map[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = White;
        }
    }
}
