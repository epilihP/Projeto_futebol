namespace Jogoss;

using System.Collections.Generic;

public class Jogos
{
    public long Codigo { get; set; } // Identificador único do jogo (ex: data.Ticks)
    public List<int> Interessados { get; set; } = new List<int>(); // Códigos dos jogadores interessados

    // Atributos Encapsulados

    public string Local { get; set; } = "Quadra Poliesportiva Mario Covas"; // <-- Corrigido: agora tem set

    public string TipoCampo { get; set; } = "Quadra Poliuretano (PU)"; // <-- Corrigido: agora tem set

    public int JogadoresPorTime { get; set; }
    public int? LimiteTimes { get; set; }
    public int? LimiteJogadores { get; set; }

    public DayOfWeek data = DayOfWeek.Thursday; // Valor padrão

    public DayOfWeek Data
    {
        get { return data; }
        set { data = value; }
    }
}