using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDL
{
    internal sealed class Event
    {
        private Character hero;
        private enum Statuses { Hidden_Far, Hidden_Close, Spotted, Fighting, Clear, Away, Dead }
        private Statuses heroStatus;

        public bool friendly;
        public List<NPC> NPCs { get; private set; }
        public List<Item> Loot { get; private set; }

        Random rand = new Random(Guid.NewGuid().GetHashCode());

        public Event(char eventType, ref Character h)
        {
            hero = h;

            NPCs = new List<NPC>();
            Loot = new List<Item>();

            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            friendly = false; // (rand.Next(2) == 1 ? true : false);

            switch (eventType)
            {
                case 'T': // troll
                    AddNPCs("Troll", 1, 1);
                    break;
                case 'B': // bonfire
                    AddNPCs("Bandit", 2, 3);
                    break;
                case 'V': // abandoned village
                    AddNPCs("Wolf", 0, 4);
                    break;
                case 'C': // cave
                    AddNPCs("Bear", 0, 1);
                    break;
                case 'M': // magic lake
                    AddNPCs("Insane anchorite", 0, 1);
                    break;
            }
        }

        private void AddNPCs(string name, int min, int max)
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < rand.Next(min, max + 1); i++)
                NPCs.Add(new NPC(name, friendly));
        }

        private void GenerateLoot()
        {
            while (true)
            {
                if (rand.Next(100) < 35 || Loot.Count > 4) // 35% chance to drop out
                    return;

                if (rand.Next(100) < 30) // 30% chanse to get HP 
                {
                    Loot.Add(new Item("Healing potion"));
                    continue;
                }

                if (rand.Next(100) < 8) 
                {
                    Loot.Add(new Item("Shield"));
                    continue;
                }

                if (rand.Next(100) < 15)
                {
                    Loot.Add(new Item("Fur cape"));
                    continue;
                }

                if (rand.Next(100) < 8)
                {
                    Loot.Add(new Item("Sword"));
                    continue;
                }

                if (rand.Next(100) < 30)
                {
                    Loot.Add(new Item("Dagger"));
                    continue;
                }
            }
        }

        public void StartEvent()
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            int count = NPCs.Count;
            if (count > 0)
            {
                Console.Write("You see ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(count > 1 ? count.ToString() + " " + NPCs[0].name + "s" : "a " + NPCs[0].name);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(".");

                if (rand.Next(100) < 90) // 10% chance to get spooted
                {                        // hidden
                    heroStatus = Statuses.Hidden_Far;
                }
                else
                {                        // spotted
                    heroStatus = Statuses.Spotted;
                }
            }
            else            // no NPC's
                heroStatus = Statuses.Clear;

            while (true)
            {
                string replyString = AskPlayerAQuestion();
                if (replyString == "" || replyString == null)
                {
                    ThrowUnexpectedMessage();
                    continue;
                }
                char reply = replyString[0];
                switch (reply)
                {
                    case 's':
                        if (heroStatus == Statuses.Hidden_Far)
                            if (hero.SneakCheck(1))               // trying to sneak closer
                                heroStatus = Statuses.Hidden_Close;
                            else
                                heroStatus = Statuses.Spotted;
                        else if (heroStatus == Statuses.Hidden_Close)
                            if (hero.SneakCheck(1.5))             // trying to perform sneak attack
                            {
                                Console.Write("You've ");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("killed ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(NPCs[0].name);
                                Console.ForegroundColor = ConsoleColor.White;

                                NPCs.RemoveAt(0);

                                if (NPCs.Count > 0)
                                    if (hero.SneakCheck(1))                     // check stealth after attack (to hide body)
                                        Console.WriteLine(" and wasn't spooted.");
                                    else
                                    {                                           // spotted after attack
                                        Console.WriteLine(" but was spotted. Prepare to fight!");
                                        heroStatus = Statuses.Spotted;
                                    }
                                else
                                {                                                           // check NPC count
                                    Console.WriteLine(" and it was the last of them.");
                                    heroStatus = Statuses.Clear;
                                    break;
                                }
                            }
                            else
                            {                                               // spotted before attack
                                Console.Write("You've tried to attack ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(NPCs[0].name);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(" but he spotted you before. Prepare to fight!");

                                heroStatus = Statuses.Spotted;
                            }
                        else
                            ThrowUnexpectedMessage();
                        break;
                    case 'a':
                        if (heroStatus == Statuses.Hidden_Far || heroStatus == Statuses.Spotted)
                        {
                            Console.WriteLine("You draw your weapon and take a battle stance.");        // proceed to fighting
                            heroStatus = Statuses.Fighting;
                        }
                        else if (heroStatus == Statuses.Fighting)
                        {
                            int targetIndex = 0;
                            NPC target;

                            if (NPCs.Count > 1)
                            {
                                Console.WriteLine("Choose target to attack: ");
                                for (int i = 0; i < NPCs.Count; i++)
                                    Console.WriteLine((i + 1).ToString() + ". " + NPCs[i].name + (NPCs[i].HP < NPCs[i].maxHP ? " (wounded)" : ""));

                                while (true)
                                {                                                               // trying to get index from player
                                    try
                                    {
                                        targetIndex = Convert.ToInt32(Console.ReadLine()) - 1;  // possible exception
                                        target = NPCs[targetIndex];                             // possible exception
                                        break;
                                    }
                                    catch
                                    {
                                        ThrowUnexpectedMessage();
                                    }
                                }
                            }
                            else
                                target = NPCs[targetIndex];
                            // attacking NPC
                            int damage = hero.Attack(ref target);
                            Console.Write("You attacked ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(target.name);
                            Console.ForegroundColor = ConsoleColor.White;
                            if (damage > 0)
                            {
                                Console.Write(" and did ");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write(damage.ToString());
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(" damage.");
                            }
                            else
                            {
                                Console.Write(" but ");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("missed");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(".");
                            }

                            if (target.HP <= 0)
                            {                                   // HP check
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(target.name);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(" is dead.");

                                NPCs.RemoveAt(targetIndex);
                                if (NPCs.Count <= 0)                // clear check
                                {
                                    Console.WriteLine("Looks like there is no one left standing except you.");
                                    heroStatus = Statuses.Clear;
                                    break;                          // if clear it breaks here
                                }
                            }
                            heroStatus = Statuses.Fighting;

                            AttackHero();  // attacking hero
                        }
                        break;
                    case 'r':                       // trying to run away
                        if (heroStatus == Statuses.Hidden_Close || heroStatus == Statuses.Hidden_Far)
                        {                                           // if not spotted chances to run are 100%
                            Console.WriteLine("You ran away.");
                            heroStatus = Statuses.Away;
                        }
                        else if (heroStatus == Statuses.Spotted || heroStatus == Statuses.Fighting)
                            if (rand.Next(2) == 1)
                            {                                           // if spotted chances to run are 50%
                                Console.WriteLine("You ran away.");
                                heroStatus = Statuses.Away;
                            }
                            else
                            {
                                Console.WriteLine("You wasn't able to run away.");
                                heroStatus = Statuses.Fighting;
                                AttackHero();         // attacking hero
                            }
                        else
                            ThrowUnexpectedMessage();
                        break;
                    case 'g':                               // go before looting
                        if (heroStatus == Statuses.Clear)
                        {
                            Console.WriteLine("You leave this place.");
                            heroStatus = Statuses.Away;
                        }
                        else
                            ThrowUnexpectedMessage();
                        break;
                    case 'l':
                        if (heroStatus == Statuses.Clear)       // start looting
                        {
                            GenerateLoot();

                            if (Loot.Count <= 0)
                            {
                                Console.WriteLine("You have found nothing and decide to leave this place.");
                                heroStatus = Statuses.Away;
                                break;
                            }

                            int itemIndex;
                            Item item;
                            while (true)
                            {                                                               // trying to get index from player
                                Console.WriteLine("You've found these items:");
                                for (int i = 0; i < Loot.Count; i++)
                                    Console.WriteLine((i + 1).ToString() + ". " + Loot[i].ToString());
                                Console.Write("Input index to take item or (leave): ");

                                string lootReply = Console.ReadLine().ToLower();

                                if (lootReply == "leave" || lootReply == "l")
                                {
                                    Console.WriteLine("You leave this place.");
                                    heroStatus = Statuses.Away;
                                    break;
                                }
                                try
                                {
                                    itemIndex = Convert.ToInt32(lootReply) - 1;  // possible exception
                                    item = Loot[itemIndex];                             // possible exception
                                    hero.AddItem(item);
                                    Loot.RemoveAt(itemIndex);
                                }
                                catch
                                {
                                    ThrowUnexpectedMessage();
                                    break;
                                }

                                if (Loot.Count <= 0)
                                {
                                    Console.WriteLine("There is nothing more. You leave this place.");
                                    heroStatus = Statuses.Away;
                                    break;
                                }
                            }
                        }
                        else
                            ThrowUnexpectedMessage();
                        break;
                    case 'i':
                        hero.ManageInventory();
                        break;
                    case 'c':
                        hero.ShowStats();
                        break;
                    default:
                        ThrowUnexpectedMessage();
                        break;
                }
                if (heroStatus == Statuses.Dead || heroStatus == Statuses.Away)
                {
                    break;
                }
            }
        }

        private void AttackHero()
        {
            foreach (NPC enemy in NPCs)                 // attacking hero
            {
                int damage = enemy.Attack(ref hero);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(enemy.name);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" attacked you ");
                if (damage > 0)
                {
                    Console.Write(" and did ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(damage.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" damage.");
                }
                else
                {
                    Console.Write(" but ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("missed");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(".");
                }

                if (hero.HP <= 0)
                {                               // HP check
                    heroStatus = Statuses.Dead;
                    break;                      // stop attacks
                }

                enemy.Speak();
            }
        }

        private void ThrowUnexpectedMessage()
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            switch (rand.Next(8))
            {
                case 0:
                    Console.WriteLine("You don't make any scence. Try again.");
                    break;
                case 1:
                    Console.WriteLine("Can't do that. Try again.");
                    break;
                case 2:
                    Console.WriteLine("No can do. Try again.");
                    break;
                case 3:
                    Console.WriteLine("I'd think twice about saying that. Try again.");
                    break;
                case 4:
                    Console.WriteLine("What was that? Try again.");
                    break;
                case 5:
                    Console.WriteLine("Come on, you can do better. Try again.");
                    break;
                case 6:
                    Console.WriteLine("Not an option. Try again.");
                    break;
                case 7:
                    Console.WriteLine("You have to learn to press those buttons... Try again.");
                    break;
            }
        }

        private string AskPlayerAQuestion()
        {
            switch (heroStatus)
            {
                case Statuses.Hidden_Far:
                    Console.Write("You wasn't spotted. What do you do? (sneak closer / attack / run): ");
                    break;
                case Statuses.Hidden_Close:
                    Console.Write("You see a good opportunity for sneaky takedown. What do you do? (stealth attack / run): "); // add distract later
                    break;
                case Statuses.Spotted:
                    Console.Write("You was spotted! What do you do? (attack / run): ");
                    break;
                case Statuses.Fighting:
                    Console.Write("You face " + (NPCs.Count > 1 ? NPCs.Count.ToString() + " " + NPCs[0].name + "s" : "a " + NPCs[0].name) + ". What do you do? (attack / run): ");
                    break;
                case Statuses.Clear:
                    Console.Write("There is no one there. What do you do? (loot / go further): ");
                    break;
                default:
                    throw new Exception("Unexpected hero status in event.");
            }
            return Console.ReadLine().ToLower();
        }

    }
}
