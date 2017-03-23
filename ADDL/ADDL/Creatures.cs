using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDL
{
    internal class Creature
    {
        public int maxHP;
        protected int strength;
        protected int attack = 1;
        protected int stealth;
        protected int accuracy;
        protected int defence;
        protected bool animal;

        public int HP { get; set; }

        public string name;

        private Random rand = new Random(Guid.NewGuid().GetHashCode());

        public int Attack(ref NPC target)
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            bool hit = rand.Next(10) < accuracy ? true : false; // % chance of a hit
            if (!hit)
                return 0;
            //rand = new Random(Guid.NewGuid().GetHashCode());
            int block = 0;
            if (rand.Next(10) < target.accuracy && target.defence > 0) // % chance of a block if target has def
                block = attack / 2 * (rand.Next(defence) + 1); // calc blocked damage
            int damage = attack * (rand.Next(strength) + 1) - block;
            target.HP -= damage;
            return damage;
        }

        public int Attack(ref Character target) // had to split for types....
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            bool hit = rand.Next(10) < accuracy ? true : false; // % chance of a hit
            if (!hit)
                return 0;
            //rand = new Random(Guid.NewGuid().GetHashCode());
            int block = 0;
            if (rand.Next(10) < target.accuracy && target.defence > 0) // % chance of a block if target has def
                block = attack / 2 * (rand.Next(defence) + 1); // calc blocked damage
            int damage = attack * (rand.Next(strength) + 1) - block;
            target.HP -= damage;
            return damage;
        }

        public bool SneakCheck(double difficulty)
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(10) < (double)stealth / difficulty ? true : false;
        }

        protected void GenerateStats(string n)
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            name = n;
            switch (n)
            {
                // NPCs generation
                case "Troll":
                    maxHP = rand.Next(400, 600);
                    strength = 10;
                    attack = rand.Next(7, 9);
                    accuracy = rand.Next(1, 4);
                    defence = rand.Next(0, 10);
                    animal = true;
                    break;
                case "Bandit":
                    maxHP = rand.Next(50, 75);
                    strength = rand.Next(3, 6); ;
                    attack = rand.Next(3, 6);
                    accuracy = rand.Next(7, 9);
                    defence = rand.Next(0, 10);
                    animal = false;
                    break;
                case "Wolf":
                    maxHP = rand.Next(40, 60);
                    strength = rand.Next(2, 5); ;
                    attack = rand.Next(2, 5);
                    accuracy = rand.Next(7, 9);
                    animal = true;
                    break;
                case "Bear":
                    maxHP = rand.Next(300, 400);
                    strength = rand.Next(8, 11);
                    attack = rand.Next(6, 8);
                    accuracy = rand.Next(2, 5);
                    animal = true;
                    break;
                case "Insane anchorite":
                    maxHP = rand.Next(150, 200);
                    strength = rand.Next(5, 7);
                    attack = rand.Next(3, 5);
                    accuracy = rand.Next(7, 9);
                    animal = false;
                    break;

                //characters generation
                case "warrior":
                case "w":
                    name = "Warrior";
                    maxHP = rand.Next(200, 250);
                    strength = rand.Next(6, 9);
                    stealth = rand.Next(3, 6);
                    accuracy = rand.Next(7, 9);
                    break;
                case "rogue":
                case "r":
                    name = "Rogue";
                    maxHP = rand.Next(125, 175);
                    strength = rand.Next(3, 6);
                    stealth = rand.Next(6, 9);
                    accuracy = rand.Next(8, 10);
                    break;
            }
            HP = maxHP;
        }
    }

    internal sealed class NPC : Creature
    {
        public bool friendly;

        private Random rand = new Random(Guid.NewGuid().GetHashCode());

        public NPC(string n, bool f)
        {
            GenerateStats(n);
            friendly = f; // common state
        }

        public void Speak()
        {
            //Random rand = new Random(Guid.NewGuid().GetHashCode());
            if (animal)
            {
                switch (rand.Next(3))
                {
                    case 0:
                        Console.WriteLine(name + ": ARRRRRRR!");
                        break;
                    case 1:
                        Console.WriteLine(name + " growls loudly");
                        break;
                    case 2:
                        Console.WriteLine(name + " digs dirt preparing to attack");
                        break;
                }
            }
            else if (!friendly)
            {
                switch (rand.Next(6))
                {
                    case 0:
                        Console.WriteLine(name + ": AAAAAAAAAAAA!");
                        break;
                    case 1:
                        Console.WriteLine(name + ": You better run!");
                        break;
                    case 2:
                        Console.WriteLine(name + ": You don't have any chances against me!");
                        break;
                    case 3:
                        Console.WriteLine(name + ": Give up and I'll kill you quickly.");
                        break;
                    case 4:
                        Console.WriteLine(name + ": I will put you down!");
                        break;
                    case 5:
                        Console.WriteLine(name + " is focused on you.");
                        break;
                }
            }
            else if (friendly)
            {
                switch (rand.Next(4))
                {
                    case 0:
                        Console.WriteLine(name + ": Leave me alone.");
                        break;
                    case 1:
                        Console.WriteLine(name + ": Belive me, you don't want to see me angry. Leave.");
                        break;
                    case 2:
                        Console.WriteLine(name + ": M-m-m? Ha! Ha-ha-ha-ha-ha-ha! Aaaaaah...");
                        break;
                    case 3:
                        Console.WriteLine(name + ": Who are you? Wait, don't answer, I don't care.");
                        break;
                }
            }
        }
    }

    internal sealed class Character : Creature
    {
        public List<Item> inventory;

        private Random rand = new Random(Guid.NewGuid().GetHashCode());

        public Character(string cl)
        {
            GenerateStats(cl);
            GenerateInventory();
        }

        private void GenerateInventory()
        {
            inventory = new List<Item>();
            switch (name)
            {
                case "Warrior":
                    AddItem("Sword");
                    AddItem("Healing potion");
                    AddItem("Healing potion");
                    break;
                case "Rogue":
                    AddItem("Short sword");
                    AddItem("Healing potion");
                    AddItem("Dagger");
                    AddItem("Dagger");
                    AddItem("Dagger");
                    break;
            }
        }

        public void AddItem(string nm) // add by name
        {
            if (inventory.Count <= 10)
            {
                inventory.Add(new Item(nm));
                if (inventory.Last().Attack > attack)
                    attack = inventory.Last().Attack;
                if (inventory.Last().Defence != 0)
                    defence += inventory.Last().Defence;
            }
            else
                Console.WriteLine("Your inventory is full.");
        }

        public void AddItem(Item item) // add by example
        {
            if (inventory.Count <= 10)
            {
                inventory.Add(item);
                if (inventory.Last().Attack > attack)
                    attack = inventory.Last().Attack;
                if (inventory.Last().Defence != 0)
                    defence += inventory.Last().Defence;
            }
            else
                Console.WriteLine("Your inventory is full.");
        }

        public void RemoveItem(int i)
        {
            try
            {
                if (inventory[i - 1].Attack != 0)
                {
                    attack = 1;
                    EquipStrongestWeapon();
                }
                if (inventory[i - 1].Defence != 0)
                    defence -= inventory.Last().Defence;
                inventory.RemoveAt(i-1);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("You don't have item in that slot.");
            }
        }

        private void EquipStrongestWeapon()
        {
            foreach (Item weapon in inventory)
            {
                if (weapon.Attack > attack)
                    attack = weapon.Attack;
            }
        }

        public void UseItem(int i)
        {
            try
            {
                Item item = inventory[i-1];
                if (item.Heal != 0)
                {
                    int gainHP = (HP + item.Heal >= maxHP ? maxHP - HP : item.Heal);
                    HP += gainHP;
                    Console.WriteLine("You've gained {0} HP. Now you're at {1} / {2}", gainHP, HP, maxHP);
                    string itemName = item.ToString();
                    RemoveItem(i);
                    Console.WriteLine(itemName + " is gone.");
                }
                else if (item.Attack != 0)
                {
                    //Random rand = new Random(Guid.NewGuid().GetHashCode());
                    bool hit = rand.Next(10) < accuracy ? true : false; // % chance of a hit
                    if (!hit)
                        Console.WriteLine("You've tried to hit yourself with a " + item.Name + ", but missed...");
                    else
                    {
                        //rand = new Random(Guid.NewGuid().GetHashCode());
                        int damage = attack * (rand.Next(strength) + 1);
                        HP -= damage;
                        Console.WriteLine("You've hit yourself with a " + item.Name + "... and did " 
                            + Convert.ToString(damage) + " damage. You must really hate yourself...");
                    }
                }
                else
                    Console.WriteLine("You don't need to use this. This item is equipped automaticly.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("You don't have item in that slot.");
            }
        }

        public void ShowInventory()
        {
            int i = 0;
            foreach (Item inventoryItem in inventory)
            {
                i++;
                Console.WriteLine("{0}. {1}", i, inventoryItem);
            }
        }

        // вывод статов
        public void ShowStats()
        {
            Console.WriteLine("Class: {0}\nHealth: {1} / {2}\nStrength: {3}\nStealth: {4}\nAccuracy: {5}"
                , name, HP, maxHP, strength, stealth, accuracy);
        }

        public void ManageInventory()
        {
            while (true)
            {
                ShowInventory();
                Console.Write("\nChoose item by index (or quit) :");
                try
                {
                    string reply = Console.ReadLine();
                    if (reply == "quit" || reply == "q") //exit
                        break;
                    int i = Convert.ToInt32(reply);
                    string itemName = inventory[i - 1].ToString();

                    bool cont = true; // continue
                    
                    Console.Write("It's a " + itemName + ". What to do? (use / throw / cancel): ");
                    while (cont)
                    {
                        reply = Console.ReadLine();
                        switch (reply)
                        {
                            case "use":
                            case "u":
                                UseItem(i);
                                cont = false;
                                break;
                            case "throw":
                            case "t":
                                RemoveItem(i);
                                Console.WriteLine(itemName + " is gone.");
                                cont = false;
                                break;
                            case "cancel":
                            case "c":
                                cont = false;
                                break;
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Index is a NUMBER, you silly. Try again.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("You don't have items by that index.");
                }
            }
        }
    }
}
