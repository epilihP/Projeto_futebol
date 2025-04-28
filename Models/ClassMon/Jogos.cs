namespace Jogos;

public abstract class Jogos
{
    private DateTime PastDate;
    public DateTime Data
    {
        get { return PastDate; }
        set
        {
            if (value < DateTime.Now)
            {
                //por precaução pra não travar
                throw new ArgumentException("A data não pode ser no passado.");
            }
            else
            {
                PastDate = value;
            }
        }
    }


    public string Local { get; set; }

    public string TipoCampo { get; set; }

    public int JogadoresPorTime { get; set; }

    public int? LimiteTimes { get; set; }

    public int? LimiteJogadores { get; set; }

    public List<string> Interessados { get; private set; }

    public Jogos(DateTime data, string local, string tipoCampo, int jogadoresPorTime, int? limiteTimes = null, int? limiteJogadores = null)
    {
        Data = data;
        Local = local;
        TipoCampo = tipoCampo;
        JogadoresPorTime = jogadoresPorTime;
        LimiteTimes = limiteTimes;
        LimiteJogadores = limiteJogadores;
        Interessados = new List<string>();
    }

    public void AdicionarInteressados(string nome)
    {
        if (LimiteJogadores.HasValue && Interessados.Count < LimiteJogadores.Value)
        {
            Interessados.Add(nome);
        }
        else
        {
            Console.WriteLine("Limite de jogadores atingidos!");
        }

    }
}