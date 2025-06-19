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
using Util.Database;
using Projeto_futebol.Util;

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
            Utils.MensagemRetornoMenu("Nenhum jogo encontrado.");
            Console.ReadKey();
            return;
        }
        Utils.ExibirLista(jogos.OrderBy(j => j.Data).Select(jogo =>
        {
            string codigo = jogo.Data.ToString("ddMMyyyy");
            int timesGerados = jogo.TimesGerados?.Count ?? 0;
            int limiteTimes = jogo.LimiteTimes ?? 0;
            int interessados = jogo.Interessados?.Count ?? 0;
            int limiteInteressados = jogo.LimiteJogadores ?? 0;
            string timesStr = limiteTimes > 0 ? $"{timesGerados}/{limiteTimes} Times" : $"{timesGerados} Times";
            string interessadosStr = limiteInteressados > 0 ? $"{interessados}/{limiteInteressados} interessados" : $"{interessados} interessados";
            return $"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} às 19h | Local: {jogo.Local} | Campo: {jogo.TipoCampo} | {timesStr} | {interessadosStr}";
        }), "Jogos Disponíveis");
        Console.Write("Digite o ID do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigoBusca))
        {
            Utils.MensagemErro("ID inválido!");
            Console.ReadKey();
            return;
        }
        var jogoSelecionado = jogos.FirstOrDefault(j => j.Codigo == codigoBusca);
        if (jogoSelecionado == null)
        {
            Utils.MensagemErro("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }

        if (jogoSelecionado.TimesGerados != null && jogoSelecionado.TimesGerados.Count > 0)
        {
            Console.WriteLine("Já existem times gerados para este jogo. Não é possível gerar novamente.");
            Console.ReadKey();
            return;
        }

        if (jogoSelecionado.Interessados == null || jogoSelecionado.Interessados.Count == 0)
        {
            Console.WriteLine("Não há interessados nesse jogo para formar times.");
            Console.ReadKey();
            return;
        }

        int jogadoresPorTime = jogoSelecionado.JogadoresPorTime;
        if (jogadoresPorTime <= 0)
        {
            Console.WriteLine("O número de jogadores por time não está definido corretamente para este jogo.");
            Console.ReadKey();
            return;
        }

        var associadosInteressados = jogoSelecionado.Interessados
            .Select(id => listaDeAssociados.FirstOrDefault(a => a.Id == id))
            .Where(a => a != null)
            .ToList();

        // Exibe os jogadores considerados
        Console.WriteLine("\nJogadores considerados para formação dos times:");
        foreach (var jogador in associadosInteressados)
        {
            if (jogador != null)
                Console.WriteLine($"- {jogador.nome} (ID: {jogador.Id}, Posição: {jogador.posicao})");
        }

        int totalTimes = associadosInteressados.Count / jogadoresPorTime;
        List<Time> timesFormados = new List<Time>();
        for (int t = 0; t < totalTimes; t++)
        {
            Time novoTime = new Time(jogoSelecionado.Data, jogoSelecionado.Local, jogoSelecionado.TipoCampo, jogadoresPorTime)
            {
                Nome = $"Time {t + 1}"
            };
            timesFormados.Add(novoTime);
        }

        // 1. Prioriza goleiros: distribui 1 por time
        var goleiros = associadosInteressados.Where(j => j != null && j.posicao == Posicao.Goleiro).ToList();
        int idxGoleiro = 0;
        for (int t = 0; t < totalTimes && idxGoleiro < goleiros.Count; t++)
        {
            if (goleiros[idxGoleiro] != null)
                timesFormados[t].Jogadores.Add(goleiros[idxGoleiro]!);
            idxGoleiro++;
        }
        // Remove goleiros já alocados
        var idsGoleirosAlocados = timesFormados.SelectMany(t => t.Jogadores).Select(j => j.Id).ToHashSet();
        var filaRestante = associadosInteressados.Where(j => j != null && !idsGoleirosAlocados.Contains(j.Id)).ToList();
        int idxFila = 0;
        for (int t = 0; t < totalTimes; t++)
        {
            while (timesFormados[t].Jogadores.Count < jogadoresPorTime && idxFila < filaRestante.Count)
            {
                if (filaRestante[idxFila] != null)
                    timesFormados[t].Jogadores.Add(filaRestante[idxFila]!);
                idxFila++;
            }
        }

        jogoSelecionado.TimesGerados = timesFormados;
        SalvarJogosNoArquivo(jogos);
        Console.WriteLine("\nTimes gerados e salvos com sucesso!");
        Console.ReadKey();
    }

    public void GerarTimesPorPosicaoEquilibrada()
    {
        Console.Clear();
        Console.WriteLine("--- Gerar Times por Posição Equilibrada ---");
        var jogos = CarregarJogosDoArquivo();
        if (jogos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo encontrado.");
            Console.ReadKey();
            return;
        }
        Utils.ExibirLista(jogos.OrderBy(j => j.Data).Select(jogo =>
        {
            string codigo = jogo.Data.ToString("ddMMyyyy");
            int timesGerados = jogo.TimesGerados?.Count ?? 0;
            int limiteTimes = jogo.LimiteTimes ?? 0;
            int interessados = jogo.Interessados?.Count ?? 0;
            int limiteInteressados = jogo.LimiteJogadores ?? 0;
            string timesStr = limiteTimes > 0 ? $"{timesGerados}/{limiteTimes} Times" : $"{timesGerados} Times";
            string interessadosStr = limiteInteressados > 0 ? $"{interessados}/{limiteInteressados} interessados" : $"{interessados} interessados";
            return $"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} às 19h | Local: {jogo.Local} | Campo: {jogo.TipoCampo} | {timesStr} | {interessadosStr}";
        }), "Jogos Disponíveis");
        Console.Write("Digite o ID do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigoBusca))
        {
            Utils.MensagemErro("ID inválido!");
            Console.ReadKey();
            return;
        }
        var jogoSelecionado = jogos.FirstOrDefault(j => j.Codigo == codigoBusca);
        if (jogoSelecionado == null)
        {
            Utils.MensagemErro("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }
        if (jogoSelecionado.TimesGerados != null && jogoSelecionado.TimesGerados.Count > 0)
        {
            Console.WriteLine("Já existem times gerados para este jogo. Não é possível gerar novamente.");
            Console.ReadKey();
            return;
        }
        if (jogoSelecionado.Interessados == null || jogoSelecionado.Interessados.Count == 0)
        {
            Console.WriteLine("Não há interessados nesse jogo para formar times.");
            Console.ReadKey();
            return;
        }
        int jogadoresPorTime = jogoSelecionado.JogadoresPorTime;
        if (jogadoresPorTime <= 0)
        {
            Console.WriteLine("O número de jogadores por time não está definido corretamente para este jogo.");
            Console.ReadKey();
            return;
        }
        var associadosInteressados = jogoSelecionado.Interessados
            .Select(id => listaDeAssociados.FirstOrDefault(a => a.Id == id))
            .Where(a => a != null)
            .ToList();
        if (associadosInteressados.Count < jogadoresPorTime)
        {
            Console.WriteLine("Não há jogadores suficientes para formar um time completo.");
            Console.ReadKey();
            return;
        }
        // Separar por posição
        var goleiros = associadosInteressados.Where(j => j != null && j.posicao == Posicao.Goleiro).ToList();
        var defensores = associadosInteressados.Where(j => j != null && j.posicao == Posicao.Defesa).ToList();
        var atacantes = associadosInteressados.Where(j => j != null && j.posicao == Posicao.Atacante).ToList();
        int totalTimes = associadosInteressados.Count / jogadoresPorTime;
        List<Time> timesFormados = new List<Time>();
        int idxGoleiro = 0, idxDefesa = 0, idxAtacante = 0;
        for (int t = 0; t < totalTimes; t++)
        {
            Time novoTime = new Time(jogoSelecionado.Data, jogoSelecionado.Local, jogoSelecionado.TipoCampo, jogadoresPorTime)
            {
                Nome = $"Time {t + 1}"
            };
            // 1 goleiro por time se possível
            if (idxGoleiro < goleiros.Count)
                novoTime.Jogadores.Add(goleiros[idxGoleiro++]!);
            // 1 defensor por time se possível
            if (idxDefesa < defensores.Count && defensores[idxDefesa] != null)
                novoTime.Jogadores.Add(defensores[idxDefesa++]!);
            // 1 atacante por time se possível
            if (idxAtacante < atacantes.Count && atacantes[idxAtacante] != null)
                novoTime.Jogadores.Add(atacantes[idxAtacante++]!);
            // Preencher o restante equilibrando as posições
            while (novoTime.Jogadores.Count < jogadoresPorTime)
            {
                if (idxGoleiro < goleiros.Count && goleiros[idxGoleiro] != null)
                    novoTime.Jogadores.Add(goleiros[idxGoleiro++]!);
                else if (idxDefesa < defensores.Count && defensores[idxDefesa] != null)
                    novoTime.Jogadores.Add(defensores[idxDefesa++]!);
                else if (idxAtacante < atacantes.Count && atacantes[idxAtacante] != null)
                    novoTime.Jogadores.Add(atacantes[idxAtacante++]!);
                else
                    break;
            }
            timesFormados.Add(novoTime);
        }
        if (timesFormados.Count > 0)
        {
            Console.WriteLine($"\n{timesFormados.Count} time(s) completo(s) formado(s)!");
            foreach (var time in timesFormados)
            {
                Console.WriteLine($"\n--- {time.Nome} ---");
                foreach (var jogador in time.Jogadores)
                {
                    Console.WriteLine($"- {jogador.nome} ({jogador.posicao})");
                }
            }
            jogoSelecionado.TimesGerados = timesFormados;
            SalvarJogosNoArquivo(jogos);
            Console.WriteLine($"\nTimes salvos no próprio jogo em jogos.json!");
        }
        else
        {
            Console.WriteLine("\nNão foi possível formar nenhum time completo com os jogadores disponíveis.");
        }
        Console.ReadKey();
    }

    public void GerarTimesPorCriterioDoGrupo()
    {
        Console.Clear();
        Console.WriteLine("--- Gerar Times pelo Critério do Grupo (Idade) ---");
        var jogos = CarregarJogosDoArquivo();
        if (jogos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo encontrado.");
            Console.ReadKey();
            return;
        }
        Utils.ExibirLista(jogos.OrderBy(j => j.Data).Select(jogo =>
        {
            string codigo = jogo.Data.ToString("ddMMyyyy");
            int timesGerados = jogo.TimesGerados?.Count ?? 0;
            int limiteTimes = jogo.LimiteTimes ?? 0;
            int interessados = jogo.Interessados?.Count ?? 0;
            int limiteInteressados = jogo.LimiteJogadores ?? 0;
            string timesStr = limiteTimes > 0 ? $"{timesGerados}/{limiteTimes} Times" : $"{timesGerados} Times";
            string interessadosStr = limiteInteressados > 0 ? $"{interessados}/{limiteInteressados} interessados" : $"{interessados} interessados";
            return $"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} às 19h | Local: {jogo.Local} | Campo: {jogo.TipoCampo} | {timesStr} | {interessadosStr}";
        }), "Jogos Disponíveis");
        Console.Write("Digite o ID do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigoBusca))
        {
            Utils.MensagemErro("ID inválido!");
            Console.ReadKey();
            return;
        }
        var jogosList = jogos.ToList();
        var jogoSelecionado = jogosList.FirstOrDefault(j => j.Codigo == codigoBusca);
        if (jogoSelecionado == null)
        {
            Utils.MensagemErro("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }
        if (jogoSelecionado.TimesGerados != null && jogoSelecionado.TimesGerados.Count > 0)
        {
            Console.WriteLine("Já existem times gerados para este jogo. Não é possível gerar novamente.");
            Console.ReadKey();
            return;
        }
        if (jogoSelecionado.Interessados == null || jogoSelecionado.Interessados.Count == 0)
        {
            Console.WriteLine("Não há interessados nesse jogo para formar times.");
            Console.ReadKey();
            return;
        }
        int jogadoresPorTime = jogoSelecionado.JogadoresPorTime;
        if (jogadoresPorTime <= 0)
        {
            Console.WriteLine("O número de jogadores por time não está definido corretamente para este jogo.");
            Console.ReadKey();
            return;
        }
        var associadosInteressados = jogoSelecionado.Interessados
            .Select(id => listaDeAssociados.FirstOrDefault(a => a.Id == id))
            .Where(a => a != null)
            .OrderByDescending(a => a!.idade)
            .ToList();
        if (associadosInteressados.Count < jogadoresPorTime)
        {
            Console.WriteLine("Não há jogadores suficientes para formar um time completo.");
            Console.ReadKey();
            return;
        }
        int totalTimes = associadosInteressados.Count / jogadoresPorTime;
        List<Time> timesFormados = new List<Time>();
        for (int t = 0; t < totalTimes; t++)
        {
            Time novoTime = new Time(jogoSelecionado.Data, jogoSelecionado.Local, jogoSelecionado.TipoCampo, jogadoresPorTime)
            {
                Nome = $"Time {t + 1}"
            };
            timesFormados.Add(novoTime);
        }
        // 1. Prioriza goleiros: distribui 1 por time
        var goleiros = associadosInteressados.Where(j => j != null && j.posicao == Posicao.Goleiro).ToList();
        int idxGoleiro = 0;
        for (int t = 0; t < totalTimes && idxGoleiro < goleiros.Count; t++)
        {
            if (goleiros[idxGoleiro] != null)
                timesFormados[t].Jogadores.Add(goleiros[idxGoleiro]!);
            idxGoleiro++;
        }
        // Remove goleiros já alocados
        var idsGoleirosAlocados = timesFormados.SelectMany(t => t.Jogadores).Select(j => j.Id).ToHashSet();
        var filaRestante = associadosInteressados.Where(j => j != null && !idsGoleirosAlocados.Contains(j.Id)).ToList();
        // 2. Distribuição round-robin por idade
        int idxTime = 0;
        foreach (var jogador in filaRestante)
        {
            if (jogador != null && timesFormados[idxTime].Jogadores.Count < jogadoresPorTime)
                timesFormados[idxTime].Jogadores.Add(jogador);
            idxTime = (idxTime + 1) % totalTimes;
        }
        jogoSelecionado.TimesGerados = timesFormados;
        SalvarJogosNoArquivo(jogos);
        Console.WriteLine("\nTimes gerados e salvos com sucesso!");
        Console.ReadKey();
    }

    public void DesfazerTimesDoJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Desfazer Times do Jogo ---");
        var jogos = CarregarJogosDoArquivo();
        if (jogos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo encontrado.");
            Console.ReadKey();
            return;
        }
        Utils.ExibirLista(jogos.OrderBy(j => j.Data).Select(jogo =>
        {
            string codigo = jogo.Data.ToString("ddMMyyyy");
            int timesGerados = jogo.TimesGerados?.Count ?? 0;
            int limiteTimes = jogo.LimiteTimes ?? 0;
            return $"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} | Times gerados: {timesGerados}/{limiteTimes}";
        }), "Jogos Disponíveis");
        Console.Write("Digite o ID do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigoLong))
        {
            Utils.MensagemErro("ID inválido!");
            Console.ReadKey();
            return;
        }
        var jogoSel = jogos.FirstOrDefault(j => j.Codigo == codigoLong);
        if (jogoSel == null)
        {
            Utils.MensagemErro("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }
        if (jogoSel.TimesGerados == null || jogoSel.TimesGerados.Count == 0)
        {
            Console.WriteLine("Nenhum time gerado para este jogo.");
            Console.ReadKey();
            return;
        }
        jogoSel.TimesGerados.Clear();
        SalvarJogosNoArquivo(jogos);
        Console.WriteLine("Times desfeitos com sucesso!");
        Console.ReadKey();
    }

    public void ExibirTimesFormados()
    {
        Console.Clear();
        Console.WriteLine("--- Jogos e Times Formados ---");
        var jogos = CarregarJogosDoArquivo();
        if (jogos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo encontrado.");
            Console.ReadKey();
            return;
        }
        foreach (var jogo in jogos.OrderBy(j => j.Data))
        {
            string codigo = jogo.Data.ToString("ddMMyyyy");
            int timesGerados = jogo.TimesGerados?.Count ?? 0;
            int limiteTimes = jogo.LimiteTimes ?? 0;
            Console.WriteLine($"\nJogo ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} às 19h | Local: {jogo.Local} | Campo: {jogo.TipoCampo}");
            Console.WriteLine($"Times gerados: {timesGerados}/{limiteTimes}");
            if (jogo.TimesGerados != null && jogo.TimesGerados.Count > 0)
            {
                foreach (var time in jogo.TimesGerados)
                {
                    Console.Write($"  {time.Nome}: ");
                    if (time.Jogadores != null && time.Jogadores.Count > 0)
                    {
                        var nomes = time.Jogadores.Where(j => j != null).Select(j => j.nome).ToList();
                        Console.WriteLine(string.Join(", ", nomes));
                    }
                    else
                    {
                        Console.WriteLine("(Nenhum jogador neste time)");
                    }
                }
            }
            else
            {
                Console.WriteLine("  Nenhum time formado para este jogo.");
            }
        }
        Console.WriteLine("\nPressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    private List<Associados> CarregarJogadoresDoArquivo()
    {
        string caminho = Database.GetDatabaseFilePath("associados.json");
        if (!File.Exists(caminho))
            return new List<Associados>();
        string jsonString = File.ReadAllText(caminho);
        var lista = JsonSerializer.Deserialize<List<Associados>>(jsonString) ?? new List<Associados>();
        return lista;
    }

    private List<GerenciadorDeJogos> CarregarJogosDoArquivo()
    {
        string caminho = Database.GetDatabaseFilePath("jogos.json");
        if (!File.Exists(caminho))
            return new List<GerenciadorDeJogos>();
        string jsonString = File.ReadAllText(caminho);
        var lista = JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(jsonString) ?? new List<GerenciadorDeJogos>();
        return lista;
    }

    private void SalvarJogosNoArquivo(List<GerenciadorDeJogos> jogos)
    {
        string caminho = Database.GetDatabaseFilePath("jogos.json");
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        string jsonString = JsonSerializer.Serialize(jogos, options);
        File.WriteAllText(caminho, jsonString);
    }
}