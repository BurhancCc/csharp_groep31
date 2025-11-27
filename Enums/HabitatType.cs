namespace csharp_groep31.Enums
{
    [Flags]
    public enum HabitatType
    // Een flags enum betekent dat iedere waarde zijn eigen bit moet zijn zodat je meerdere tegelijk kan gebruiken.
    // Ik schrijf het op met bitwise omdat het dan schaalbaardere code is :).
    {
        None = 0,
        Forest = 1 << 0, // = 1
        Aquatic = 1 << 1, // = 2
        Desert = 1 << 2, // = 4
        Grassland = 1 << 3 // = 8
    }
}