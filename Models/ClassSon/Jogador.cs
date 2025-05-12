namespace Players;
using Associacao;

public class Jogador : Associados
{
    // Atributos encapsulados
    private int Codigo;
    public int codigo { get => Codigo; set => Codigo = value; }

    // Método contrutor
    public Jogador(string nome, int idade, Posicao posicao)
    {
        Codigo = GerarCodigoUnico();
        this.Nome = nome;
        this.Idade = idade;
        this.Posicao = posicao;
    }

    // usando override em um metodo padrão C#
    public override string ToString()
    {
        return $"Código: {Codigo}, Nome: {Nome}, Idade: {Idade}, Posição: {Posicao}";
    }

    // Lista para armazenar jogadores em memória
    private static List<Jogador> jogadores = new List<Jogador>();

    // implementação do método CRUD

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
    public static bool AtualizarJogador(int codigo, string novoNome, int novaIdade, Posicao novaPosicao)
    {
        var jogador = jogadores.FirstOrDefault(j => j.codigo == codigo);
        if (jogador != null)
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
    public static bool RemoverJogador(int codigo)
    {
        var jogador = jogadores.FirstOrDefault(j => j.codigo == codigo);
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