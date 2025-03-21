namespace CompteFindes;
public class Persona
{
    public string Nom { get; set; }
    public bool nen { get; set; }
    public bool dorm { get; set; }
    public Persona(int nen, string nom){
        if (nen==1){
            this.nen=true;
        }else {
            this.nen=false;
        }
        Nom=nom;
    }
    public Persona(string nom){
        Nom=nom;
        nen=false;
        dorm=false;
    }
}
