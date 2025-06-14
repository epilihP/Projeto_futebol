using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Associacao;
using TimeFutebol;
using GerenciadorJogos;

namespace PROJETO_FUTEBOL.controller;

public class TimeController
{
    private List<Associados> listaDeAssociados;

    public TimeController()
    {
        listaDeAssociados = CarregarJogadoresDoArquivo();
    }

    public void GerarTimesPorOrdemDeChegada()
    {
        Console.Clear();
        Console.WriteLine("--- Gerar Times por Ordem de Chegada ---");

        var jogos = CarregarJogosDoArquivo();
        if (jogos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo encontrado.");
            Console.ReadKey();
            return;
        }

        // Permita o usuário escolher o jogo (ou pegue o último)
        Console.WriteLine("Selecione o jogo:");
        for (int i = 0; i < jogos.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {jogos[i].Data:dd/MM/yyyy} - {jogos[i].Local}");
        }
        int escolha = 0;
        while (escolha < 1 || escolha > jogos.Count)
        {
            Console.Write("Digite o número do jogo: ");
            int.TryParse(Console.ReadLine(), out escolha);
        }
        var jogoSelecionado = jogos[escolha - 1];

        if (jogoSelecionado.Interessados == null || jogoSelecionado.Interessados.Count == 0)
        {
            Console.WriteLine("Não há interessados nesse jogo para formar times.");
            Console.ReadKey();
            return;
        }

        Console.Write("Digite quantos jogadores por time (incluindo goleiro): ");
        if (!int.TryParse(Console.ReadLine(), out int jogadoresPorTime) || jogadoresPorTime <= 0)
        {
            Console.WriteLine("Número inválido.");
            Console.ReadKey();
            return;
        }

        // Filtrar só os associados interessados
        var associadosInteressados = listaDeAssociados
            .Where(a => jogoSelecionado.Interessados.Contains(a.Id)) // Ajuste o nome do campo de ID se necessário!
            .ToList();

        var goleiros = associadosInteressados
            .Where(j => j.posicao.ToString().ToLower() == "goleiro").ToList();
        var jogadoresDeLinha = associadosInteressados
            .Where(j => j.posicao.ToString().ToLower() != "goleiro").ToList();

        List<Times> timesFormados = new List<Times>();
        int numeroTime = 1;

        while (goleiros.Count >= 1 && jogadoresDeLinha.Count >= (jogadoresPorTime - 1))
        {
            Times novoTime = new Times(DateTime.Now, "Quadra Central", "Society", jogadoresPorTime)
            {
                Nome = $"Time {numeroTime}"
            };

            var goleiroDoTime = goleiros.First();
            novoTime.Jogadores.Add(goleiroDoTime);
            goleiros.Remove(goleiroDoTime);

            var jogadoresDoTime = jogadoresDeLinha.Take(jogadoresPorTime - 1).ToList();
            novoTime.Jogadores.AddRange(jogadoresDoTime);
            jogadoresDeLinha.RemoveAll(j => jogadoresDoTime.Contains(j));

            timesFormados.Add(novoTime);
            numeroTime++;
        }

        if (timesFormados.Count > 0)
        {
            Console.WriteLine("\n--- Times Formados ---");
            foreach (var time in timesFormados)
            {
                Console.WriteLine($"\n--- {time.Nome} ---");
                foreach (var jogador in time.Jogadores)
                {
                    Console.WriteLine($"- {jogador.nome} ({jogador.posicao})");
                }
            }
            SalvarTimesNoArquivo(timesFormados);
            Console.WriteLine("\n\nTimes salvos em 'times.json'!");
        }
        else
        {
            Console.WriteLine("\nNão foi possível formar nenhum time completo com os jogadores disponíveis.");
        }

        Console.ReadKey();
    }

    public void GerarTimesPorPosicaoEquilibrada() // a devinir função
    {
        Console.WriteLine("Função de gerar times por posição equilibrada ainda não implementada.");
        Console.ReadKey();
    }

    private List<Associados> CarregarJogadoresDoArquivo()
    {
        string caminho = "associados.json";
        if (!File.Exists(caminho) || string.IsNullOrEmpty(File.ReadAllText(caminho)))
        {
            return new List<Associados>();
        }
        string jsonString = File.ReadAllText(caminho);
        return JsonSerializer.Deserialize<List<Associados>>(jsonString);
    }

    private List<GerenciadorDeJogos> CarregarJogosDoArquivo()
    {
        string caminho = "jogos.json";
        if (!File.Exists(caminho) || string.IsNullOrEmpty(File.ReadAllText(caminho)))
        {
            return new List<GerenciadorDeJogos>();
        }
        string jsonString = File.ReadAllText(caminho);
        return JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(jsonString);
    }

    private void SalvarTimesNoArquivo(List<Times> times)
    {
        string caminho = "times.json";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        string jsonString = JsonSerializer.Serialize(times, options);
        File.WriteAllText(caminho, jsonString);
    }
}