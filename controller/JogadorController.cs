using Associacao;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Util.Database;
using Projeto_futebol.Util;

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
        Utils.ExibirJanela("Cadastro de Novo Associado", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        Utils.ExibirJanela("Informe o nome do Associado:", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        string? nome = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nome))
        {
            Utils.MensagemErro("Nome não pode ser vazio. Cadastro cancelado.", 70);
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        Utils.ExibirJanela("Informe a idade do Associado:", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        int.TryParse(Console.ReadLine(), out int idade);
        Utils.ExibirJanela("Posição: 1 - Atacante, 2 - Defesa, 3 - Goleiro", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        Utils.ExibirJanela("Digite o número correspondente à posição:", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        if (!int.TryParse(Console.ReadLine(), out int posicaoInt) || posicaoInt < 1 || posicaoInt > 3)
        {
            Utils.MensagemErro("Posição inválida! Cadastro cancelado.", 70);
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        Posicao posicao = (Posicao)posicaoInt;
        int novoId = new Associados().GerarCodigoUnico(new HashSet<int>(listaDeAssociados.Select(a => a.Id)));
        Associados novoAssociado = new Associados
        {
            Id = novoId,
            nome = nome!,
            idade = idade,
            posicao = posicao
        };
        listaDeAssociados.Add(novoAssociado);
        SalvarNoArquivo();
        Utils.MensagemSucesso("Associado cadastrado com sucesso!", 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    public void ListarJogadores()
    {
        Console.Clear();
        Utils.ExibirLista(listaDeAssociados.Select(a => $"ID: {a.Id} | Nome: {a.nome} | Idade: {a.idade} | Posição: {a.posicao}"), "Lista de Associados Cadastrados", ConsoleColor.Magenta, 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    public void AtualizarJogador()
    {
        Console.Clear();
        Utils.ExibirJanela("Atualizar Associado", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        if (listaDeAssociados.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum associado cadastrado. Pressione qualquer tecla para voltar...", 70);
            Console.ReadKey();
            return;
        }
        else
        {
            Utils.ExibirLista(listaDeAssociados.Select(a => $"ID: {a.Id} | Nome: {a.nome}"), "Associados", ConsoleColor.Magenta, 70);
            Utils.ExibirJanela("Pressione qualquer tecla para seguir para a alteração...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
        }
        Utils.ExibirJanela("Digite o ID do associado que deseja atualizar:", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        string? inputId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(inputId) || !Utils.ValidarId(inputId, out int idParaAtualizar))
        {
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        Associados? associadoParaAtualizar = listaDeAssociados.Find(j => j.Id == idParaAtualizar);
        if (associadoParaAtualizar == null)
        {
            Utils.MensagemErro("Associado não encontrado.", 70);
        }
        else
        {
            Utils.ExibirJanela("Deixe em branco para não alterar.", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Utils.ExibirJanela($"Novo nome (Atual: {associadoParaAtualizar.nome}):", Array.Empty<string>(), ConsoleColor.Magenta, 70);
            string? nome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nome))
            {
                associadoParaAtualizar.nome = nome;
            }
            Utils.ExibirJanela($"Nova idade (Atual: {associadoParaAtualizar.idade}):", Array.Empty<string>(), ConsoleColor.Magenta, 70);
            string? idadeStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(idadeStr) && int.TryParse(idadeStr, out int idade))
            {
                associadoParaAtualizar.idade = idade;
            }
            Utils.ExibirJanela($"Nova posição (1 - Atacante, 2 - Defesa, 3 - Goleiro) (Atual: {associadoParaAtualizar.posicao})", Array.Empty<string>(), ConsoleColor.Magenta, 70);
            string? posicaoStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(posicaoStr))
            {
                if (int.TryParse(posicaoStr, out int posicaoInt) && posicaoInt >= 1 && posicaoInt <= 3)
                {
                    associadoParaAtualizar.posicao = (Posicao)posicaoInt;
                }
                else
                {
                    Utils.MensagemErro("Posição inválida! Valor não alterado.", 70);
                }
            }
            SalvarNoArquivo();
            Utils.MensagemSucesso("Associado atualizado com sucesso!", 70);
        }
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    public void ExcluirJogador()
    {
        Console.Clear();
        Utils.ExibirJanela("Excluir Associado", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        if (listaDeAssociados.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum associado cadastrado. Pressione qualquer tecla para voltar...", 70);
            Console.ReadKey();
            return;
        }
        else
        {
            Utils.ExibirLista(listaDeAssociados.Select(a => $"ID: {a.Id} | Nome: {a.nome} | Idade: {a.idade} | Posição: {a.posicao}"), "Associados", ConsoleColor.Magenta, 70);
            Utils.ExibirJanela("Pressione qualquer tecla para seguir com a exclusão...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
        }
        Utils.ExibirJanela("Digite o ID do associado que você deseja excluir:", Array.Empty<string>(), ConsoleColor.Magenta, 70);
        string? inputId = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(inputId) || !Utils.ValidarId(inputId, out int idParaExcluir))
        {
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        Associados? associadoParaExcluir = listaDeAssociados.Find(j => j.Id == idParaExcluir);
        if (associadoParaExcluir == null)
        {
            Utils.MensagemErro("Associado não encontrado.", 70);
        }
        else
        {
            listaDeAssociados.Remove(associadoParaExcluir);
            SalvarNoArquivo();
            Utils.MensagemSucesso("Associado excluído com sucesso!", 70);
        }
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
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
        var result = JsonSerializer.Deserialize<List<Associados>>(jsonString);
        return result ?? new List<Associados>();
    }
}