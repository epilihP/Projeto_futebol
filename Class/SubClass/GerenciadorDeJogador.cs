namespace Players;
using Associacao;

public class Jogador : Associados
{
    // Atributos encapsulados
    public int RA { get; private set; }
    private static List<Jogador> jogadores = new List<Jogador>();

    // Método contrutor
    public Jogador(string nome, int idade, Posicao posicao)
    {
        // Carrega todos os jogadores do JSON
        var jogadoresExistentes = Jogador.ListarJogadores();
        var codigosExistentes = new HashSet<int>(jogadoresExistentes.Select(j => j.RA));

        RA = GerarCodigoUnico(codigosExistentes);
        this.Nome = nome;
        this.Idade = idade;
        this.Posicao = posicao;
    }

    public override string ToString()
    {
        return $"Código: {RA}, Nome: {Nome}, Idade: {Idade}, Posição: {Posicao}";
    }

    // --------------- CRUD ---------------
    // Create: Adicionar jogador
    public static void AdicionarJogador(Jogador jogador)
    {
        jogadores.Add(jogador);
        Database.SalvarJogador(jogador); // existente no Database
    }

    // Read: Listar todos os jogadores
    public static List<Jogador> ListarJogadores()
    {
        return jogadores;
    }

    // Update: Atualizar jogador por código
    public static bool AtualizarJogador(int RA, string novoNome, int novaIdade, Posicao novaPosicao)
    {
        var jogador = jogadores.FirstOrDefault(j => j.RA == RA);
        if (jogador != null) // Se esse jogador existir
        {
            jogador.Nome = novoNome;
            jogador.Idade = novaIdade;
            jogador.Posicao = novaPosicao;

            // Atualiza o arquivo JSON
            Database.SalvarJogador(jogador);
            return true;
        }
        return false;
    }

    // Delete: Remover jogador por código
    public static bool RemoverJogador(int RA)
    {
        var jogador = jogadores.FirstOrDefault(j => j.RA == RA);
        if (jogador != null)
        {
            jogadores.Remove(jogador);

            // Atualiza o arquivo JSON
            Database.AtualizarArquivo(jogadores);
            return true;
        }
        return false;
    }
}