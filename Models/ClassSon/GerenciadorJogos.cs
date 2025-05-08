namespace GerenciadorJogos;
using Newtonsoft.Json;
using Jogos;
public class GerenciadorDeJogos : Jogos
{
    private readonly string pasta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

    public GerenciadorDeJogos(DateTime data, string local, string tipoCampo, int jogadoresPorTime, int? limiteTimes = null, int? limiteJogadores = null) 
        : base(data, local, tipoCampo, jogadoresPorTime, limiteTimes, limiteJogadores)
    {
    }
    private string Caminho(int id) => Path.Combine(pasta, $"{id}.json");

    private JsonSerializerSettings Config => new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    public void Criar(Jogos jogo)
    {
        var caminho = Caminho(jogo.GetHashCode());
        if (File.Exists(caminho))
            throw new Exception("Já existe um jogo com esse ID.");

        var json = JsonConvert.SerializeObject(jogo, Config);
        File.WriteAllText(caminho, json);
    }

    public Jogos Ler(int id)
    {
        var caminho = Caminho(id);
        if (!File.Exists(caminho))
            throw new FileNotFoundException("Jogo não encontrado.");

        var json = File.ReadAllText(caminho);
        return JsonConvert.DeserializeObject<Jogos>(json, Config);
    }

    public List<Jogos> ListarTodos()
    {
        return Directory.GetFiles(pasta, "*.json")
            .Select(f => JsonConvert.DeserializeObject<Jogos>(File.ReadAllText(f), Config))
            .ToList();
    }


    public void Atualizar(Jogos jogo)
    {
        var caminho = Caminho(jogo.GetHashCode());
        if (!File.Exists(caminho))
            throw new FileNotFoundException("Jogo não encontrado.");

        var json = JsonConvert.SerializeObject(jogo, Config);
        File.WriteAllText(caminho, json);
    }

    public void Deletar(int id)
    {
        var caminho = Caminho(id);
        if (File.Exists(caminho))
            File.Delete(caminho);
    }
}
