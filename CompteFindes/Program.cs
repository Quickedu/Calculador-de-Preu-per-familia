using System.Reflection;
using System.Security.Principal;

namespace CompteFindes;
public class Program{
    private static string input;
    private static bool sortir = false;
    private static Dictionary<string,Familia> families = new ();
    public static void Main(){
        Console.WriteLine("Families i persones");
        var k = Console.ReadLine();
        for (int i = 0; i < int.Parse(k); i++){
            Console.WriteLine("Nom de la familia");
            input = Console.ReadLine();
            Console.WriteLine("es queden a dormir? (0,1)");
            var dorm = int.Parse(Console.ReadLine());
            families.Add(input,new Familia(input,dorm));
            Console.WriteLine("Quantes persones hi ha a la familia?");
            var num = Console.ReadLine();
            for (int j = 0; j < int.Parse(num); j++){
                Console.WriteLine("Introdueix les dades seguents: NOM, (OPCIONAL) SI ES NEN (0,1)");
                var input2 = Console.ReadLine().Split(',');
                if (input2.Length==1){
                    families[input].AfegirPersona(new Persona(input2[0]));
                }else families[input].AfegirPersona(new Persona(int.Parse(input2[1]),input2[0]));
            }
        }
        while(!sortir){
            Console.WriteLine("Que vols fer?");
            Console.WriteLine("1. Veure totes les persones d'una familia");
            Console.WriteLine("2. Calcular quan paga cadascun");
            Console.WriteLine("3. Canviar una persona");
            Console.WriteLine("4. Sortir");
            input = Console.ReadLine();
            switch(input){
                case "1":
                    seefamily();
                break;
                case "2":
                    calculate();
                break;
                case "3":
                    change();
                break;
                case "4":
                    sortir=true;
                break;
                default:
                    Console.WriteLine("Opcio no valida");
                break;
            }
        }
        Console.WriteLine("Adeu!");
    }

    private static void seefamily()
    {
        Console.WriteLine("Quina familia vols veure?");
        input = Console.ReadLine();
        foreach (var item in families[input].Persones){
            Console.WriteLine(item.Nom);
        }
        Console.WriteLine("\n");
    }

    private static void calculate(){
        Console.WriteLine("Introdueix el preu total");
        var preuTotal = int.Parse(Console.ReadLine());
        var totalPesos = 0;
        foreach (var familia in families.Values){
            var pesFamilia = 0;
            foreach (var persona in familia.Persones){
                pesFamilia += persona.nen ? 1 : 2; // Els nens compten com 1, els adults com 2
            }
            if (familia.dorm) pesFamilia *= 2; // Si la família dorm, multiplica el pes per 2
            totalPesos += pesFamilia; // Suma el pes de la família al pes total
        }
        // Calcular i mostrar el cost per a cada família
            using (var writer = new StreamWriter("Calcul.txt")){
            writer.WriteLine("Càlcul de costos per família:");
            writer.WriteLine($"Preu total: {preuTotal} euros\n");
            // Calcular i mostrar el cost per a cada família
            foreach (var familia in families.Values){
                var pesFamilia = 0;
                foreach (var persona in familia.Persones){
                    pesFamilia += persona.nen ? 1 : 2; // Els nens compten com 1, els adults com 2
                }
                if (familia.dorm) pesFamilia *= 2; // Si la família dorm, multiplica el pes per 2
                var costFamilia = (preuTotal * pesFamilia) / totalPesos; // Calcula el cost proporcional
                Console.WriteLine($"La familia {familia.Nom} ha de pagar: {costFamilia} euros.");
                // Escriure les dades al fitxer
                writer.WriteLine($"Familia: {familia.Nom}");
                writer.WriteLine($"  Cost total: {costFamilia} euros");
                writer.WriteLine("  Membres:");
                foreach (var persona in familia.Persones){
                    var costPersona = persona.nen ? (costFamilia / pesFamilia) * 1 : (costFamilia / pesFamilia) * 2;
                    writer.WriteLine($"    - {persona.Nom} ({(persona.nen ? "Nen" : "Adult")}) - {costPersona:F2} euros");
                }
                writer.WriteLine();
            }
        }

        Console.WriteLine("Les dades s'han guardat al fitxer Calcul.txt");
    }
    private static void change(){
        Console.WriteLine("Què vols fer?");
        Console.WriteLine("1. Canviar l'estat d'una persona");
        Console.WriteLine("2. Afegir un nou membre a una família");
        Console.WriteLine("3. Eliminar una família");
        Console.WriteLine("4. Eliminar un membre d'una família");
        var opcio = Console.ReadLine();
        switch (opcio){
            case "1": // Canviar l'estat d'una persona
                Console.WriteLine("Introdueix el nom de la família:");
                var nomFamilia = Console.ReadLine();
                if (families.ContainsKey(nomFamilia)){
                    Console.WriteLine("Introdueix el nom de la persona:");
                    var nomPersona = Console.ReadLine();
                    var persona = families[nomFamilia].Persones.FirstOrDefault(p => p.Nom == nomPersona);
                    if (persona != null){
                        persona.nen = !persona.nen; // Canvia l'estat de nen/adult
                        Console.WriteLine($"L'estat de {persona.Nom} s'ha canviat a {(persona.nen ? "Nen" : "Adult")}.");
                    } else {
                        Console.WriteLine("Persona no trobada.");
                    }
                } else{
                    Console.WriteLine("Família no trobada.");
                }
                break;
            case "2": // Afegir un nou membre a una família
                Console.WriteLine("Introdueix el nom de la família:");
                nomFamilia = Console.ReadLine();
                if (families.ContainsKey(nomFamilia)){
                    Console.WriteLine("Introdueix les dades del nou membre: NOM, (OPCIONAL) SI ES NEN (0,1)");
                    var input = Console.ReadLine().Split(',');
                    if (input.Length == 1){
                        families[nomFamilia].AfegirPersona(new Persona(input[0]));
                    }else{
                        families[nomFamilia].AfegirPersona(new Persona(int.Parse(input[1]), input[0]));
                    }
                    Console.WriteLine("Nou membre afegit correctament.");
                }else{
                    Console.WriteLine("Família no trobada.");
                }
                break;

            case "3": // Eliminar una família
                Console.WriteLine("Introdueix el nom de la família a eliminar:");
                nomFamilia = Console.ReadLine();
                if (families.Remove(nomFamilia)){
                    Console.WriteLine($"La família {nomFamilia} s'ha eliminat correctament.");
                } else {
                    Console.WriteLine("Família no trobada.");
                }
                break;

            case "4": // Eliminar un membre d'una família
                Console.WriteLine("Introdueix el nom de la família:");
                nomFamilia = Console.ReadLine();
                if (families.ContainsKey(nomFamilia)){
                    Console.WriteLine("Introdueix el nom del membre a eliminar:");
                    var nomPersona = Console.ReadLine();
                    var persona = families[nomFamilia].Persones.FirstOrDefault(p => p.Nom == nomPersona);
                    if (persona != null){
                        families[nomFamilia].Persones.Remove(persona);
                        Console.WriteLine($"El membre {nomPersona} s'ha eliminat correctament.");
                    } else{
                        Console.WriteLine("Persona no trobada.");
                    }
                } else {
                    Console.WriteLine("Família no trobada.");
                }
                break;

            default:
                Console.WriteLine("Opció no vàlida.");
                break;
        }
    }

}