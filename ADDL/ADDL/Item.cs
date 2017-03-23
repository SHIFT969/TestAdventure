using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDL
{
    internal sealed class Item
    {
        public string Name { get; private set;}
        public int Attack { get; private set; }
        public int Defence { get; private set; }
        public int Heal { get; private set; }

        public Item(string nm)
        {
            Name = nm;
            switch (Name)
            {
                case "Sword":
                    Attack = 8;
                    Defence = 0;
                    Heal = 0;
                    break;
                case "Short sword":
                    Attack = 6;
                    Defence = 0;
                    Heal = 0;
                    break;
                case "Dagger":
                    Attack = 4;
                    Defence = 0;
                    Heal = 0;
                    break;
                case "Healing potion":
                    Attack = 0;
                    Defence = 0;
                    Heal = 50;
                    break;
                case "Shield":
                    Attack = 0;
                    Defence = 10;
                    Heal = 0;
                    break;
                case "Fur cape":
                    Attack = 0;
                    Defence = 5;
                    Heal = 0;
                    break;
            }
        }

        public override string ToString()
        {
            string output = Name + " (";
            if (Attack != 0)
                output += "attack - " + Convert.ToString(Attack);
            if (Defence != 0)
                output += "defence - " + Convert.ToString(Defence);
            if (Heal != 0)
                output += "heal - " + Convert.ToString(Heal);
            output += ")";
            return output;
        }
    }
}
