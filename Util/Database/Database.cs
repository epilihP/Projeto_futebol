using System.Text.Json;
using Players;

public class Database
{
    private static readonly string FilePath = @"c:\Users\aliss\Documents\Faculdade\Programação Orientada a Objetos\Projeto Futebol\Projeto_futebol\Util\Database\associados.json";

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
}