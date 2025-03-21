namespace CompteFindes;
public class Familia{
    public string Nom { get; set; }
    public bool dorm { get; set; }
    public List<Persona> Persones { get; set; } = new();
    public Familia(string nom, int dorm){
        Nom=nom;
        if (dorm == 1){
            this.dorm = true;
        }else{
            this.dorm = false;
        }
    }
    public void AfegirPersona(Persona p){
        Persones.Add(p);
    }
}