using System.Text.Json;
using Players;
using GerenciadorJogos;
using Partidas;
using System.IO;

namespace Util.Database;

// SalvarJogo (Create)
// ListarJogos (Read)
// AtualizarJogo (Update)
// RemoverJogo (Delete)

public class Database
{
    internal static readonly string FilePath = GetDatabaseFilePath("associados.json");

    public static void SalvarJogador(Jogador jogador)
    {
        List<Jogador> jogadores;
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            jogadores = string.IsNullOrWhiteSpace(json) ? new List<Jogador>() : JsonSerializer.Deserialize<List<Jogador>>(json) ?? new List<Jogador>();
        }
        else
        {
            jogadores = new List<Jogador>();
        }
        jogadores.Add(jogador);
        string novoJson = JsonSerializer.Serialize(jogadores, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, novoJson);
    }

    public static void AtualizarArquivo(List<Jogador> jogadores)
    {
        string novoJson = JsonSerializer.Serialize(jogadores, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, novoJson);
    }

    internal static readonly string JogosFilePath = GetDatabaseFilePath("jogos.json");

    // CREATE
    public static void SalvarJogo(GerenciadorDeJogos jogo)
    {
        List<GerenciadorDeJogos> jogos;
        if (File.Exists(JogosFilePath))
        {
            string json = File.ReadAllText(JogosFilePath);
            jogos = string.IsNullOrWhiteSpace(json) ? new List<GerenciadorDeJogos>() : JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(json) ?? new List<GerenciadorDeJogos>();
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
        return string.IsNullOrWhiteSpace(json) ? new List<GerenciadorDeJogos>() : JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(json) ?? new List<GerenciadorDeJogos>();
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


    private static readonly string HistoricoFilePath = GetDatabaseFilePath("historico_partidas.json");

    public static void SalvarHistorico(List<HistoricoRodada> historico)
    {
        var json = JsonSerializer.Serialize(historico, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(HistoricoFilePath, json);
    }

    public static List<HistoricoRodada> LerHistorico()
    {
        if (!File.Exists(HistoricoFilePath))
            return new List<HistoricoRodada>();
        var json = File.ReadAllText(HistoricoFilePath);
        return string.IsNullOrWhiteSpace(json) ? new List<HistoricoRodada>() : JsonSerializer.Deserialize<List<HistoricoRodada>>(json) ?? new List<HistoricoRodada>();
    }

    public static string GetDatabaseFilePath(string fileName)
    {
        // Caminho absoluto a partir da raiz do projeto
        string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
        return Path.Combine(projectRoot, "Util", "Database", fileName);
    }
}