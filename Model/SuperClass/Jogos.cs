namespace Jogos;
using System.Collections.Generic;

public abstract class Jogos
{
    public long Codigo { get; set; } // Identificador único do jogo (ex: data.Ticks)
    public List<int> Interessados { get; set; } = new  List<int>(); // Códigos dos jogadores interessados

    // Atributos Encapsulados
    
    public string Local {get;} = "Quadra Poliesportiva Mario Covas";

    public string TipoCampo { get; } = "Quadra Poliuretano (PU)";
 
    public int JogadoresPorTime { get; set; }
    public int? LimiteTimes { get; set; }
    public int? LimiteJogadores { get; set; }

    public DayOfWeek data;
    public DayOfWeek Data
    {
        get { return data; }
        set
        {
            data = DayOfWeek.Thursday;
        }
    }
}