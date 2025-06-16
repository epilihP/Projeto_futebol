using Associacao;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Util.Database;

namespace PROJETO_FUTEBOL.controller;

public class JogadorController
{
    private List<Associados> listaDeAssociados;
    private readonly string caminhoArquivoJson = Database.GetDatabaseFilePath("associados.json");
    public JogadorController()
    {
        listaDeAssociados = CarregarDoArquivo();
    }

    public void CadastrarJogador()
    {
        Console.Clear();
        Console.WriteLine("--- Cadastro de Novo Associado ---");



        Console.Write("Nome do Associado: ");
        string nome = Console.ReadLine();

        Console.Write("Idade do Associado: ");
        int.TryParse(Console.ReadLine(), out int idade);

        Console.WriteLine("Posição: 1 - Atacante, 2 - Defesa, 3 - Goleiro");
        Console.Write("Digite o número correspondente à posição: ");
        if (!int.TryParse(Console.ReadLine(), out int posicaoInt) || posicaoInt < 1 || posicaoInt > 3)
        {
            Console.WriteLine("Posição inválida! Cadastro cancelado. Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            return;
        }
        Posicao posicao = (Posicao)posicaoInt;

        int novoId = new Associados().GerarCodigoUnico(new HashSet<int>(listaDeAssociados.Select(a => a.Id)));

        Associados novoAssociado = new Associados
        {
            Id = novoId,
            nome = nome,
            idade = idade,
            posicao = posicao
        };

        listaDeAssociados.Add(novoAssociado);
        SalvarNoArquivo();

        Console.WriteLine("\nAssociado cadastrado com sucesso!");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    public void ListarJogadores()
    {
        Console.Clear();
        Console.WriteLine("--- Lista de Associados Cadastrados ---");

        if (listaDeAssociados.Count == 0)
        {
            Console.WriteLine("Nenhum associado cadastrado.");
        }
        else
        {
            foreach (var associado in listaDeAssociados)
            {
                Console.WriteLine($"ID: {associado.Id} | Nome: {associado.nome} | Idade: {associado.idade} | Posição: {associado.posicao}");
            }
        }
        Console.WriteLine("\nPressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    public void AtualizarJogador()
    {
        Console.Clear();
        Console.WriteLine("--- Atualizar Associado ---");
        if (listaDeAssociados.Count == 0)
        {
            Console.WriteLine("Nenhum associado cadastrado.");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            return;
        }
        else
        {
            foreach (var associado in listaDeAssociados)
            {
                Console.WriteLine($"ID: {associado.Id} | Nome: {associado.nome}");
            }
            Console.WriteLine("\nPressione qualquer tecla para seguir para a alteração...");
            Console.ReadKey();
        }

        Console.Write("\nDigite o ID do associado que deseja atualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int idParaAtualizar))
        {
            Console.WriteLine("ID inválido.");
            Console.ReadKey();
            return;
        }

        Associados associadoParaAtualizar = listaDeAssociados.Find(j => j.Id == idParaAtualizar);

        if (associadoParaAtualizar == null)
        {
            Console.WriteLine("Associado não encontrado.");
        }
        else
        {
            Console.WriteLine("\nDeixe em branco para não alterar.");
            Console.Write($"Novo nome (Atual: {associadoParaAtualizar.nome}): ");
            string nome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nome))
            {
                associadoParaAtualizar.nome = nome;
            }

            Console.Write($"Nova idade (Atual: {associadoParaAtualizar.idade}): ");
            string idadeStr = Console.ReadLine();
            if (int.TryParse(idadeStr, out int idade))
            {
                associadoParaAtualizar.idade = idade;
            }

            Console.WriteLine("Nova posição (1 - Atacante, 2 - Defesa, 3 - Goleiro) (Atual: {0}): ", associadoParaAtualizar.posicao);
            string posicaoStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(posicaoStr))
            {
                if (int.TryParse(posicaoStr, out int posicaoInt) && posicaoInt >= 1 && posicaoInt <= 3)
                {
                    associadoParaAtualizar.posicao = (Posicao)posicaoInt;
                }
                else
                {
                    Console.WriteLine("Posição inválida! Valor não alterado.");
                }
            }

            SalvarNoArquivo();
            Console.WriteLine("\nAssociado atualizado com sucesso!");
        }
        Console.ReadKey();
    }

    public void ExcluirJogador()
    {
        Console.Clear();
        Console.WriteLine("--- Excluir Associado ---");
        ListarJogadores();

        Console.Write("\nDigite o ID do associado que você deseja excluir: ");
        if (!int.TryParse(Console.ReadLine(), out int idParaExcluir))
        {
            Console.WriteLine("ID inválido.");
            Console.ReadKey();
            return;
        }

        Associados associadoParaExcluir = listaDeAssociados.Find(j => j.Id == idParaExcluir);

        if (associadoParaExcluir == null)
        {
            Console.WriteLine("Associado não encontrado.");
        }
        else
        {
            listaDeAssociados.Remove(associadoParaExcluir);
            SalvarNoArquivo();
            Console.WriteLine("Associado excluído com sucesso!");
        }
        Console.ReadKey();
    }

    private void SalvarNoArquivo()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        string jsonString = JsonSerializer.Serialize(listaDeAssociados, options);
        File.WriteAllText(caminhoArquivoJson, jsonString);
    }

    private List<Associados> CarregarDoArquivo()
    {
        if (!File.Exists(caminhoArquivoJson) || string.IsNullOrEmpty(File.ReadAllText(caminhoArquivoJson)))
        {
            return new List<Associados>();
        }
        string jsonString = File.ReadAllText(caminhoArquivoJson);
        return JsonSerializer.Deserialize<List<Associados>>(jsonString);
    }
}