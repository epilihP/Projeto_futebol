namespace Jogos;

public class Jogos
{
    public long Codigo { get; set; } // Identificador único do jogo (ex: data.Ticks)
    public List<int> Interessados { get; set; } = new List<int>(); // Códigos dos jogadores interessados
    // atributos
    public string Local { get; set; }
    public string TipoCampo { get; set; }
    public int JogadoresPorTime { get; set; }
    public int? LimiteTimes { get; set; }
    public int? LimiteJogadores { get; set; }

    private DateTime data;
    public DateTime Data
    {
        get { return data; }
        set
        {
            data = value; // Remova a validação aqui!
        }
    }
}