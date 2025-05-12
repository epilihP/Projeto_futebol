using System.Text.Json;
using Players;
using GerenciadorJogos;


// SalvarJogo (Create)
// ListarJogos (Read)
// AtualizarJogo (Update)
// RemoverJogo (Delete)

public class Database
{
    internal static readonly string FilePath = @"c:\Users\aliss\Documents\Faculdade\Programação Orientada a Objetos\Projeto Futebol\Projeto_futebol\Util\Database\associados.json";
    
    public static void SalvarJogador(Jogador jogador)
    {
        List<Jogador> jogadores;

        if (File.Exists(FilePath))  // rquivo existe?
        {
            // Le o arquivo
            string json = File.ReadAllText(FilePath);
            jogadores = string.IsNullOrWhiteSpace(json) ? new List<Jogador>() : JsonSerializer.Deserialize<List<Jogador>>(json);
        }
        else
        {
            // Cria uma nova lista se o arquivo não existir
            jogadores = new List<Jogador>();
        }

        // Adc o jogador
        jogadores.Add(jogador);

        // devolve a lista de volta para o JSON
        string novoJson = JsonSerializer.Serialize(jogadores, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(FilePath, novoJson); // pra salvar no aqrquivo
    }
    public static void AtualizarArquivo(List<Jogador> jogadores)
    {
        string novoJson = JsonSerializer.Serialize(jogadores, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, novoJson);
    }

    internal static readonly string JogosFilePath = @"c:\Users\aliss\Documents\Faculdade\Programação Orientada a Objetos\Projeto Futebol\Projeto_futebol\Util\Database\jogos.json";

// CREATE
public static void SalvarJogo(GerenciadorDeJogos jogo)
{
    List<GerenciadorDeJogos> jogos;
    if (File.Exists(JogosFilePath))
    {
        string json = File.ReadAllText(JogosFilePath);
        jogos = string.IsNullOrWhiteSpace(json) ? new List<GerenciadorDeJogos>() : JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(json);
    }
    else
    {
        jogos = new List<GerenciadorDeJogos>();
    }
    jogos.Add(jogo);
    string novoJson = JsonSerializer.Serialize(jogos, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(JogosFilePath, novoJson);
}

// READ
public static List<GerenciadorDeJogos> ListarJogos()
{
    if (!File.Exists(JogosFilePath)) return new List<GerenciadorDeJogos>();
    string json = File.ReadAllText(JogosFilePath);
    return string.IsNullOrWhiteSpace(json) ? new List<GerenciadorDeJogos>() : JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(json);
}

// UPDATE
public static bool AtualizarJogo(int codigo, GerenciadorDeJogos jogoAtualizado)
{
    var jogos = ListarJogos();
    var index = jogos.FindIndex(j => j.Codigo == codigo);
    if (index == -1) return false;
    jogos[index] = jogoAtualizado;
    File.WriteAllText(JogosFilePath, JsonSerializer.Serialize(jogos, new JsonSerializerOptions { WriteIndented = true }));
    return true;
}

// DELETE
public static bool RemoverJogo(int codigo)
{
    var jogos = ListarJogos();
    var jogo = jogos.FirstOrDefault(j => j.Codigo == codigo);
    if (jogo == null) return false;
    jogos.Remove(jogo);
    File.WriteAllText(JogosFilePath, JsonSerializer.Serialize(jogos, new JsonSerializerOptions { WriteIndented = true }));
    return true;
}

}