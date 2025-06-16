using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Jogoss;
using GerenciadorJogos;
using Util.Database;

namespace PROJETO_FUTEBOL.controller;

public class JogoController
{
    private List<GerenciadorDeJogos> listaDeJogos = new List<GerenciadorDeJogos>();
    private readonly string caminhoArquivo = Database.GetDatabaseFilePath("jogos.json");
    public JogoController()
    {
        CarregarDoArquivo();
    }

    public static string DiaDaSemanaEmPortugues(DayOfWeek dia)
    {
        switch (dia)
        {
            case DayOfWeek.Monday: return "Segunda-feira";
            case DayOfWeek.Tuesday: return "Terça-feira";
            case DayOfWeek.Wednesday: return "Quarta-feira";
            case DayOfWeek.Thursday: return "Quinta-feira";
            case DayOfWeek.Friday: return "Sexta-feira";
            case DayOfWeek.Saturday: return "Sábado";
            case DayOfWeek.Sunday: return "Domingo";
            default: return dia.ToString();
        }
    }

    public void AgendarJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Agendar Novo Jogo ---");
        // Sempre próxima quinta-feira às 19h
        DateTime data = ProximaQuintaDisponivel();
        string codigo = data.ToString("ddMMyyyy");
        if (listaDeJogos.Exists(j => j.Data.Date == data.Date))
        {
            DateTime sugestao = ProximaQuintaDisponivel(data.AddDays(7));
            Console.WriteLine($"Já existe um jogo agendado para {data:dd/MM/yyyy}. Próxima quinta-feira disponível: {sugestao:dd/MM/yyyy}");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            return;
        }
        int jogadoresPorTime = 5;
        int limiteTimes = 6;
        int limiteJogadores = jogadoresPorTime * limiteTimes;
        Console.WriteLine($"Data do jogo: {data:dd/MM/yyyy} às 19h");
        Console.WriteLine($"Código do jogo: {codigo}");
        Console.Write("Local do jogo (padrão: Quadra Poliesportiva): ");
        string local = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(local)) local = "Quadra Poliesportiva";
        Console.Write("Tipo de campo (padrão: Quadra Polietano (PU)): ");
        string tipoCampo = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(tipoCampo)) tipoCampo = "Quadra Polietano (PU)";
        GerenciadorDeJogos novoJogo = new GerenciadorDeJogos(
            data,
            local,
            tipoCampo,
            jogadoresPorTime,
            limiteTimes,
            limiteJogadores
        );
        // novoJogo.Codigo = DataTime.Now.Ticks;
        novoJogo.Codigo = long.Parse(codigo); // substituido acima, pois, gerava um cpodigo muito longo
        listaDeJogos.Add(novoJogo);
        SalvarNoArquivo();
        Console.WriteLine($"\nJogadores por time: {jogadoresPorTime}");
        Console.WriteLine($"Limite de times: {limiteTimes}");
        Console.WriteLine($"Limite de jogadores: {limiteJogadores}");
        Console.WriteLine("\nJogo agendado com sucesso!");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    private DateTime ProximaQuintaDisponivel(DateTime? inicio = null)
    {
        DateTime data = inicio ?? DateTime.Now;
        // Ajusta para próxima quinta-feira
        int daysUntilThursday = ((int)DayOfWeek.Thursday - (int)data.DayOfWeek + 7) % 7;
        if (daysUntilThursday == 0) daysUntilThursday = 7;
        DateTime quinta = data.Date.AddDays(daysUntilThursday).AddHours(19); // 19h
        while (listaDeJogos.Exists(j => j.Data.Date == quinta.Date))
        {
            quinta = quinta.AddDays(7); // próxima quinta
        }
        return quinta;
    }

    public void ListarJogos()
    {
        Console.Clear();
        Console.WriteLine("--- Lista de Jogos Agendados ---");
        var jogosFuturos = listaDeJogos
            .Where(j => j.Data >= DateTime.Now)
            .OrderBy(j => j.Data)
            .ToList();
        if (jogosFuturos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo agendado.");
        }
        else
        {
            foreach (var jogo in jogosFuturos)
            {
                string codigo = jogo.Data.ToString("ddMMyyyy");
                int timesGerados = jogo.TimesGerados?.Count ?? 0;
                int limiteTimes = jogo.LimiteTimes ?? 0;
                Console.WriteLine($"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} às 19h | Local: {jogo.Local} | Campo: {jogo.TipoCampo} | Times gerados: {timesGerados}/{limiteTimes}");
            }
        }
        Console.WriteLine("\nPressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    public void AtualizarJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Atualizar Jogo ---");
        // Exibe a lista de jogos disponíveis
        var jogosFuturos = listaDeJogos
            .OrderBy(j => j.Data)
            .ToList();
        if (jogosFuturos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo agendado.");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("\nLista de Jogos Disponíveis:");
        foreach (var jogoItem in jogosFuturos)
        {
            string codigoJogo = jogoItem.Data.ToString("ddMMyyyy");
            Console.WriteLine($"ID: {codigoJogo} | Data: {jogoItem.Data:dd/MM/yyyy} às 19h | Local: {jogoItem.Local} | Campo: {jogoItem.TipoCampo}");
        }
        Console.WriteLine("\nPara manter os valores padrão, basta deixar em branco.");
        Console.Write("\nDigite o código do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigo))
        {
            Console.WriteLine("Código inválido!");
            Console.ReadKey();
            return;
        }
        var jogoSel = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogoSel == null)
        {
            Console.WriteLine("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }

        Console.Write($"Local ({jogoSel.Local}): ");
        string? local = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(local)) jogoSel.Local = local;

        Console.Write($"Tipo de campo ({jogoSel.TipoCampo}): ");
        string? tipoCampo = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(tipoCampo)) jogoSel.TipoCampo = tipoCampo;

        Console.Write($"Jogadores por time ({jogoSel.JogadoresPorTime}): ");
        string? jogadoresStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(jogadoresStr))
        {
            if (int.TryParse(jogadoresStr, out int jogadores) && jogadores > 0)
                jogoSel.JogadoresPorTime = jogadores;
            else
                Console.WriteLine("Valor inválido. Mantido valor anterior.");
        }

        Console.Write($"Limite de times ({jogoSel.LimiteTimes}): ");
        string? limiteTimesStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(limiteTimesStr))
        {
            if (int.TryParse(limiteTimesStr, out int limiteTimes) && limiteTimes > 0)
                jogoSel.LimiteTimes = limiteTimes;
            else
                Console.WriteLine("Valor inválido. Mantido valor anterior.");
        }

        // Limite de jogadores sempre alinhado ao produto
        jogoSel.LimiteJogadores = jogoSel.JogadoresPorTime * (jogoSel.LimiteTimes ?? 1);

        Console.WriteLine("Recalculando:");
        Console.WriteLine($"\nJogadores por time: {jogoSel.JogadoresPorTime}");
        Console.WriteLine($"Limite de times: {jogoSel.LimiteTimes}");
        Console.WriteLine($"Limite de jogadores: {jogoSel.LimiteJogadores}");

        SalvarNoArquivo();
        Console.WriteLine("Jogo atualizado!");
        Console.ReadKey();
    }

    public void ExcluirJogo()
    {
        Console.Clear();
        // Exibe a lista de jogos disponíveis
        var jogosFuturos = listaDeJogos
            .OrderBy(j => j.Data)
            .ToList();
        Console.WriteLine("Lista de Jogos Disponíveis:");
        if (jogosFuturos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo agendado.");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            return;
        }
        foreach (var jogoItem in jogosFuturos)
        {
            string codigoJogo = jogoItem.Data.ToString("ddMMyyyy");
            Console.WriteLine($"ID: {codigoJogo} | Data: {jogoItem.Data:dd/MM/yyyy} às 19h | Local: {jogoItem.Local} | Campo: {jogoItem.TipoCampo}");
        }
        Console.WriteLine("--- Excluir Jogo ---");
        Console.Write("Digite o código do jogo: ");
        long.TryParse(Console.ReadLine(), out long codigo);

        var jogo = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogo == null)
        {
            Console.WriteLine("Jogo não encontrado!");
        }
        else
        {
            listaDeJogos.Remove(jogo);
            SalvarNoArquivo();
            Console.WriteLine("Jogo removido!");
        }
        Console.ReadKey();
    }

    public void GerenciarInteressados()
    {
        Console.Clear();
        Console.WriteLine("--- Gerenciar Interessados no Jogo ---");
        // Exibe a lista de jogos disponíveis
        var jogosFuturos = listaDeJogos
            .OrderBy(j => j.Data)
            .ToList();
        Console.WriteLine("Lista de Jogos Disponíveis:");
        if (jogosFuturos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo agendado.");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            return;
        }
        foreach (var jogoItem in jogosFuturos)
        {
            string codigoJogo = jogoItem.Data.ToString("ddMMyyyy");
            Console.WriteLine($"ID: {codigoJogo} | Data: {jogoItem.Data:dd/MM/yyyy} às 19h | Local: {jogoItem.Local} | Campo: {jogoItem.TipoCampo}");
        }
        Console.Write("Digite o código do jogo: ");
        long.TryParse(Console.ReadLine(), out long codigo);

        var jogoSel = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogoSel == null)
        {
            Console.WriteLine("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }

        // Carrega associados
        string caminhoAssociados = Database.GetDatabaseFilePath("associados.json");
        List<Associacao.Associados> associados = new List<Associacao.Associados>();
        if (File.Exists(caminhoAssociados))
        {
            string jsonAssociados = File.ReadAllText(caminhoAssociados);
            associados = JsonSerializer.Deserialize<List<Associacao.Associados>>(jsonAssociados) ?? new List<Associacao.Associados>();
        }

        // Exibe interessados
        Console.WriteLine("\nInteressados neste jogo:");
        int goleiros = 0;
        foreach (var id in jogoSel.Interessados)
        {
            var assoc = associados.FirstOrDefault(a => a.Id == id);
            if (assoc != null)
            {
                string pos = assoc.posicao == (Associacao.Posicao)3 ? "Goleiro" : assoc.posicao.ToString();
                Console.WriteLine($"- {assoc.nome} (RA: {assoc.Id}) | Posição: {pos}");
                if (assoc.posicao == (Associacao.Posicao)3) goleiros++;
            }
        }
        int limiteTimes = jogoSel.LimiteTimes ?? 1;
        int limiteJogadores = jogoSel.LimiteJogadores ?? 0;
        int faltamGoleiros = limiteTimes - goleiros;
        if (faltamGoleiros > 0)
            Console.WriteLine($"\nAtenção: Faltam {faltamGoleiros} goleiro(s) para completar todos os times!");
        else
            Console.WriteLine("\nQuantidade de goleiros suficiente para os times.");
        Console.WriteLine($"\nTotal de interessados: {jogoSel.Interessados.Count}/{limiteJogadores}");
        if (jogoSel.Interessados.Count >= limiteJogadores)
        {
            Console.WriteLine("Limite de interessados atingido! Não é possível adicionar mais.");
        }
        // Menu de opções
        while (true)
        {
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1 - Adicionar novo interessado");
            Console.WriteLine("2 - Remover interesse de um jogador");
            Console.WriteLine("0 - Sair");
            Console.Write("Opção: ");
            string op = Console.ReadLine();
            if (op == "0") break;
            if (op == "1")
            {
                if (jogoSel.Interessados.Count >= limiteJogadores)
                {
                    Console.WriteLine("Limite de interessados atingido!");
                    continue;
                }
                Console.Write("Digite o código do jogador interessado: ");
                long.TryParse(Console.ReadLine(), out long jogadorId);
                if (!associados.Any(a => a.Id == jogadorId))
                {
                    Console.WriteLine("RA não encontrado nos associados.");
                    continue;
                }
                if (!jogoSel.Interessados.Contains((int)jogadorId))
                {
                    jogoSel.Interessados.Add((int)jogadorId);
                    Console.WriteLine("Adicionado!");
                }
                else
                {
                    Console.WriteLine("Jogador já está na lista de interessados.");
                }
            }
            else if (op == "2")
            {
                Console.Write("Digite o código do jogador para remover: ");
                long.TryParse(Console.ReadLine(), out long jogadorId);
                if (jogoSel.Interessados.Contains((int)jogadorId))
                {
                    jogoSel.Interessados.Remove((int)jogadorId);
                    Console.WriteLine("Removido!");
                }
                else
                {
                    Console.WriteLine("Jogador não está na lista de interessados.");
                }
            }
            else
            {
                Console.WriteLine("Opção inválida!");
            }
            // Atualiza exibição após cada ação
            Console.WriteLine("\nInteressados neste jogo:");
            goleiros = 0;
            foreach (var id in jogoSel.Interessados)
            {
                var assoc = associados.FirstOrDefault(a => a.Id == id);
                if (assoc != null)
                {
                    string pos = assoc.posicao == (Associacao.Posicao)3 ? "Goleiro" : assoc.posicao.ToString();
                    Console.WriteLine($"- {assoc.nome} (RA: {assoc.Id}) | Posição: {pos}");
                    if (assoc.posicao == (Associacao.Posicao)3) goleiros++;
                }
            }
            faltamGoleiros = limiteTimes - goleiros;
            if (faltamGoleiros > 0)
                Console.WriteLine($"\nAtenção: Faltam {faltamGoleiros} goleiro(s) para completar todos os times!");
            else
                Console.WriteLine("\nQuantidade de goleiros suficiente para os times.");
            Console.WriteLine($"\nTotal de interessados: {jogoSel.Interessados.Count}/{limiteJogadores}");
            if (jogoSel.Interessados.Count >= limiteJogadores)
                Console.WriteLine("Limite de interessados atingido! Não é possível adicionar mais.");
        }
        SalvarNoArquivo();
        Console.WriteLine("Interessados atualizados!");
        Console.ReadKey();
    }
    private void SalvarNoArquivo()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(listaDeJogos, options);
        Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
        File.WriteAllText(caminhoArquivo, json);
    }

    private void CarregarDoArquivo()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            if (!string.IsNullOrWhiteSpace(json))
                listaDeJogos = JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(json);
            else
                listaDeJogos = new List<GerenciadorDeJogos>();
        }
        else
        {
            listaDeJogos = new List<GerenciadorDeJogos>();
        }
    }
}