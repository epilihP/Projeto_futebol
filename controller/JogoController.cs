using System;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Jogoss;

namespace PROJETO_FUTEBOL.controller
{
    public class JogoController
    {
        private List<Jogos> listaDeJogos;
        private readonly string caminhoArquivoJson = "jogos.json";

        public JogoController()
        {
            listaDeJogos = CarregarDoArquivo();
        }

        public void AgendarJogo()
        {
            Console.Clear();
            Console.WriteLine("--- Agendar Novo Jogo ---");
            
            // 1. Criamos o objeto ANTES de pedir os dados.
            //    Agora, ele já nasce com os valores padrão definidos na classe.
            Jogos novoJogo = new Jogos();
            novoJogo.Id = (int)DateTime.Now.Ticks;
            
            Console.WriteLine("(Deixe em branco e aperte Enter para usar o valor padrão)");

            // 2. Mostramos o valor padrão e só alteramos se o usuário digitar algo.
            Console.Write($"Local do jogo (Padrão: {novoJogo.Local}): ");
            string localInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(localInput))
            {
                novoJogo.Local = localInput;
            }

            // 3. Fazemos o mesmo para o Tipo de Campo.
            Console.Write($"Tipo de campo (Padrão: {novoJogo.TipoCampo}): ");
            string tipoCampoInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tipoCampoInput))
            {
                novoJogo.TipoCampo = tipoCampoInput;
            }

            // Os outros dados continuam sendo pedidos normalmente
            Console.Write("Data do jogo (dd/MM/yyyy): ");
            DateTime.TryParse(Console.ReadLine(), out novoJogo.Data);

            Console.Write("Jogadores por time: ");
            int.TryParse(Console.ReadLine(), out int jogadoresPorTime);
            novoJogo.JogadoresPorTime = jogadoresPorTime;

            Console.Write("Limite total de jogadores (0 se não houver limite): ");
            int.TryParse(Console.ReadLine(), out int limiteTotalJogadores);
            novoJogo.LimiteTotalJogadores = limiteTotalJogadores;
            
            listaDeJogos.Add(novoJogo);
            SalvarNoArquivo();

            Console.WriteLine("\nJogo agendado com sucesso!");
            Console.ReadKey();
        }

        public void ListarJogos()
        {
            Console.Clear();
            Console.WriteLine("--- Lista de Jogos Agendados ---");

            if (listaDeJogos.Count == 0)
            {
                Console.WriteLine("Nenhum jogo agendado.");
            }
            else
            {
                foreach (var jogo in listaDeJogos)
                {
                    Console.WriteLine($"ID: {jogo.Id} | Data: {jogo.Data.ToShortDateString()} | Local: {jogo.Local} | Campo: {jogo.TipoCampo} | {jogo.JogadoresPorTime} por time");
                }
            }
            Console.WriteLine("\nPressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        private void SalvarNoArquivo()
        {
            var options = new JsonSerializerOptions { 
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            string jsonString = JsonSerializer.Serialize(listaDeJogos, options);
            File.WriteAllText(caminhoArquivoJson, jsonString);
        }

        private List<Jogos> CarregarDoArquivo()
        {
            if (!File.Exists(caminhoArquivoJson) || string.IsNullOrEmpty(File.ReadAllText(caminhoArquivoJson)))
            {
                return new List<Jogos>();
            }
            string jsonString = File.ReadAllText(caminhoArquivoJson);
            return JsonSerializer.Deserialize<List<Jogos>>(jsonString);
        }
    }
}