using Associacao;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
namespace PROJETO_FUTEBOL.controller

{
    public class JogadorController
    {
        private List<Associados> listaDeAssociados;
        private readonly string caminhoArquivoJson = @"c:\Users\aliss\Documents\Faculdade\Programação Orientada a Objetos\Projeto Futebol\Projeto_futebol\Util\Database\associados.json";
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

            Console.Write("Posição (Goleiro, Defesa, Ataque): ");
            string posicaoStr = Console.ReadLine();
            Posicao posicao;
            Enum.TryParse(posicaoStr, true, out posicao);

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
            ListarJogadoresSemPausa();

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

                string posicaoStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(posicaoStr))
                {
                    Posicao novaPosicao;
                    if (Enum.TryParse(posicaoStr, true, out novaPosicao))
                    {
                        associadoParaAtualizar.posicao = novaPosicao;
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
            ListarJogadoresSemPausa();

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

        private void ListarJogadoresSemPausa()
        {
            if (listaDeAssociados.Count == 0)
            {
                Console.WriteLine("Nenhum associado cadastrado.");
            }
            else
            {
                foreach (var associado in listaDeAssociados)
                {
                    Console.WriteLine($"ID: {associado.Id} | Nome: {associado.nome} | Posição: {associado.posicao}");
                }
            }
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
}