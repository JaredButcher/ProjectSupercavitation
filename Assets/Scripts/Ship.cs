using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Ship {
    public string ShipName { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Speed { get; set; }
    public int Camo { get; set; }
    public int Fire { get; set; }
    public int AntiAir { get; set; }
    public int Secondary { get; set; }
    public int Torps { get; set; }
    public int Depth { get; set; }
    public int Range { get; set; }
    public int SecondRange { get; set; }
    public int Armor { get; set; }
    public int Evasion { get; set; }
    public int Cost { get; set; }
    public bool Active { get; private set; }
    public ShipDesination ShipType { get; protected set; }
    public ShipButton ShipButton { get; set; }
    public ShipMask Mask;

    public const int TORP_RANGE = 8000;
    public const int TORP_DAMAGE = 15;

    public static Dictionary<ShipDesination, Ship> ShipClass = new Dictionary<ShipDesination, Ship>(12) {
        { ShipDesination.DE, new DE() }, { ShipDesination.DD, new DD() }, { ShipDesination.CL, new CL() }, { ShipDesination.CC, new CC() },
        { ShipDesination.CA, new CA() }, { ShipDesination.BD, new BD() }, { ShipDesination.BB, new BB() }, { ShipDesination.CE, new CE() },
        { ShipDesination.CV, new CV() }, { ShipDesination.SS, new SS() }, { ShipDesination.SA, new SA() }, { ShipDesination.AR, new AR() }
    };
    public static Dictionary<string, ShipDesination> Desninations = new Dictionary<string, ShipDesination>(12) {
        { "DE", ShipDesination.DE }, { "DD", ShipDesination.DD }, { "CL", ShipDesination.CL}, { "CC", ShipDesination.CC },
        { "CA", ShipDesination.CA }, { "BD", ShipDesination.BD }, { "BB", ShipDesination.BB}, { "CE", ShipDesination.CE },
        { "CV", ShipDesination.CV }, { "SS", ShipDesination.SS }, { "SA", ShipDesination.SA }, { "AR", ShipDesination.AR }
    };
    /// <param name="Percent">Percentage between 0 and 100 of amount of damage to recover</param>
    public void Repair(float Percent) {
        Health += Mathf.RoundToInt(Mathf.Clamp((MaxHealth - Health) * (Percent / 100), 0, MaxHealth));
    }
    public void UnEvade() {
        Evasion = ShipClass[ShipType].Evasion;
    }
}
#region SubClasses
public class DE : Ship {
    public DE() {
        ShipName = "Destoryer Escort";
        MaxHealth = 10;
        Speed = 16000;
        Camo = 10000;
        Fire = 10;
        AntiAir = 20;
        Secondary = 4;
        Torps = 4;
        Depth = 20;
        Range = 8000;
        SecondRange = 4000;
        Cost = 10;
        Armor = 10;
        Evasion = 50;
        Health = MaxHealth;
        ShipType = ShipDesination.DE;
    }
}
public class DD : Ship {
    public DD() {
        ShipName = "Destoryer";
        MaxHealth = 15;
        Speed = 10000;
        Camo = 10000;
        Fire = 15;
        AntiAir = 20;
        Secondary = 2;
        Torps = 8;
        Depth = 20;
        Range = 8000;
        SecondRange = 4000;
        Cost = 20;
        Armor = 10;
        Evasion = 60;
        Health = MaxHealth;
        ShipType = ShipDesination.DD;
    }

}
public class CL : Ship {
    public CL() {
        ShipName = "Light Crusier";
        MaxHealth = 25;
        Speed = 9000;
        Camo = 12000;
        Fire = 25;
        AntiAir = 20;
        Secondary = 2;
        Torps = 8;
        Depth = 20;
        Range = 10000;
        SecondRange = 4000;
        Cost = 30;
        Armor = 15;
        Evasion = 50;
        Health = MaxHealth;
        ShipType = ShipDesination.CL;
    }

}
public class CC : Ship {
    public CC() {
        ShipName = "Crusier";
        MaxHealth = 40;
        Speed = 8000;
        Camo = 14000;
        Fire = 45;
        AntiAir = 20;
        Secondary = 2;
        Torps = 4;
        Depth = 20;
        Range = 12000;
        SecondRange = 4000;
        Cost = 50;
        Armor = 30;
        Evasion = 40;
        Health = MaxHealth;
        ShipType = ShipDesination.CC;
    }

}
public class CA : Ship {
    public CA() {
        ShipName = "Battle Crusier";
        MaxHealth = 80;
        Speed = 8000;
        Camo = 16000;
        Fire = 100;
        AntiAir = 20;
        Secondary = 2;
        Torps = 0;
        Depth = 20;
        Range = 20000;
        SecondRange = 4000;
        Cost = 100;
        Armor = 50;
        Evasion = 30;
        Health = MaxHealth;
        ShipType = ShipDesination.CA;
    }

}
public class BD : Ship {
    public BD() {
        ShipName = "Dreadnought";
        MaxHealth = 150;
        Speed = 3000;
        Camo = 20000;
        Fire = 100;
        AntiAir = 20;
        Secondary = 2;
        Torps = 0;
        Depth = 20;
        Range = 20000;
        SecondRange = 4000;
        Cost = 100;
        Armor = 100;
        Evasion = 10;
        Health = MaxHealth;
        ShipType = ShipDesination.BD;
    }
}
public class BB : Ship {
    public BB() {
        ShipName = "Battleship";
        MaxHealth = 200;
        Speed = 5000;
        Camo = 25000;
        Fire = 150;
        AntiAir = 20;
        Secondary = 2;
        Torps = 0;
        Depth = 20;
        Range = 25000;
        SecondRange = 4000;
        Cost = 150;
        Armor = 150;
        Evasion = 20;
        Health = MaxHealth;
        ShipType = ShipDesination.BB;
    }
}
public class CV : Ship {
    public CV() {
        ShipName = "Fleet Carrier";
        MaxHealth = 10;
        Speed = 10;
        Camo = 5000;
        Fire = 10;
        AntiAir = 20;
        Secondary = 2;
        Torps = 4;
        Depth = 20;
        Range = 5000;
        SecondRange = 4000;
        Cost = 10;
        Armor = 10;
        Evasion = 10;
        Health = MaxHealth;
        ShipType = ShipDesination.CV;
    }
}
public class CE : Ship {
    public CE() {
        ShipName = "Escort Carrier";
        MaxHealth = 10;
        Speed = 10;
        Camo = 5000;
        Fire = 10;
        AntiAir = 20;
        Secondary = 2;
        Torps = 4;
        Depth = 20;
        Range = 5000;
        SecondRange = 4000;
        Cost = 10;
        Armor = 10;
        Evasion = 10;
        Health = MaxHealth;
        ShipType = ShipDesination.CE;
    }
}
public class SS : Ship {
    public SS() {
        ShipName = "Submarine";
        MaxHealth = 10;
        Speed = 10;
        Camo = 5000;
        Fire = 10;
        AntiAir = 20;
        Secondary = 2;
        Torps = 4;
        Depth = 20;
        Range = 5000;
        SecondRange = 4000;
        Cost = 10;
        Armor = 10;
        Evasion = 10;
        Health = MaxHealth;
        ShipType = ShipDesination.SS;
    }
}
public class SA : Ship {
    public SA() {
        ShipName = "Aviation Submarine";
        MaxHealth = 10;
        Speed = 10;
        Camo = 5000;
        Fire = 10;
        AntiAir = 20;
        Secondary = 2;
        Torps = 4;
        Depth = 20;
        Range = 5000;
        SecondRange = 4000;
        Cost = 10;
        Armor = 10;
        Evasion = 10;
        Health = MaxHealth;
        ShipType = ShipDesination.SA;
    }
}
public class AR : Ship {
    public AR() {
        ShipName = "Repair Ship";
        MaxHealth = 10;
        Speed = 10;
        Camo = 5000;
        Fire = 10;
        AntiAir = 20;
        Secondary = 2;
        Torps = 4;
        Depth = 20;
        Range = 5000;
        SecondRange = 4000;
        Cost = 10;
        Armor = 10;
        Evasion = 10;
        Health = MaxHealth;
        ShipType = ShipDesination.AR;
    }
}
#endregion
public enum ShipDesination {
    DE,
    DD,
    CL,
    CC,
    CA,
    BD,
    BB,
    CE,
    CV,
    SS,
    SA,
    AR
}