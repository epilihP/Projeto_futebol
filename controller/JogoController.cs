using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Jogoss;
using GerenciadorJogos;
using Util.Database;
using Projeto_futebol.Util;

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
        Utils.ExibirJanela("Agendar Novo Jogo", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        DateTime data = ProximaQuintaDisponivel();
        string codigo = data.ToString("ddMMyyyy");
        if (listaDeJogos.Exists(j => j.Data.Date == data.Date))
        {
            DateTime sugestao = ProximaQuintaDisponivel(data.AddDays(7));
            Utils.ExibirJanela($"Já existe um jogo agendado para {data:dd/MM/yyyy}. Próxima quinta-feira disponível: {sugestao:dd/MM/yyyy}", Array.Empty<string>(), ConsoleColor.Red, 70);
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        int jogadoresPorTime = 5;
        int limiteTimes = 6;
        int limiteJogadores = jogadoresPorTime * limiteTimes;
        Utils.ExibirJanela($"Data do jogo: {data:dd/MM/yyyy} às 19h", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        Utils.ExibirJanela($"Código do jogo: {codigo}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
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
        novoJogo.Codigo = long.Parse(codigo);
        listaDeJogos.Add(novoJogo);
        SalvarNoArquivo();
        Utils.ExibirJanela($"Jogadores por time: {jogadoresPorTime}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        Utils.ExibirJanela($"Limite de times: {limiteTimes}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        Utils.ExibirJanela($"Limite de jogadores: {limiteJogadores}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        Utils.MensagemSucesso("Jogo agendado com sucesso!", 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
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
        var jogosFuturos = listaDeJogos
            .Where(j => j.Data >= DateTime.Now)
            .OrderBy(j => j.Data)
            .Select(jogo =>
            {
                string codigo = jogo.Data.ToString("ddMMyyyy");
                int timesGerados = jogo.TimesGerados?.Count ?? 0;
                int limiteTimes = jogo.LimiteTimes ?? 0;
                return $"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} às 19h | Local: {jogo.Local} | Campo: {jogo.TipoCampo} | Times gerados: {timesGerados}/{limiteTimes}";
            })
            .ToList();
        Utils.ExibirLista(jogosFuturos, "Lista de Jogos Agendados", ConsoleColor.Yellow, 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    public void AtualizarJogo()
    {
        Console.Clear();
        Utils.ExibirJanela("Atualizar Jogo", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        var jogosFuturos = listaDeJogos.OrderBy(j => j.Data).ToList();
        if (jogosFuturos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo agendado. Pressione qualquer tecla para voltar...", 70);
            Console.ReadKey();
            return;
        }
        Utils.ExibirLista(jogosFuturos.Select(jogoItem => $"ID: {jogoItem.Data:ddMMyyyy} | Data: {jogoItem.Data:dd/MM/yyyy} às 19h | Local: {jogoItem.Local} | Campo: {jogoItem.TipoCampo}"), "Jogos Disponíveis", ConsoleColor.Yellow, 70);
        Utils.ExibirJanela("Para manter os valores padrão, basta deixar em branco.", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.Write("\nDigite o código do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigo))
        {
            Utils.MensagemErro("Código inválido, voltando ao menu.", 70);
            Console.ReadKey();
            return;
        }
        var jogoSel = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogoSel == null)
        {
            Utils.MensagemErro("Jogo não encontrado, voltando ao menu.", 70);
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
                Utils.MensagemErro("Valor inválido. Mantido valor anterior.", 70);
        }

        Console.Write($"Limite de times ({jogoSel.LimiteTimes}): ");
        string? limiteTimesStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(limiteTimesStr))
        {
            if (int.TryParse(limiteTimesStr, out int limiteTimes) && limiteTimes > 0)
                jogoSel.LimiteTimes = limiteTimes;
            else
                Utils.MensagemErro("Valor inválido. Mantido valor anterior.", 70);
        }

        // Limite de jogadores sempre alinhado ao produto
        jogoSel.LimiteJogadores = jogoSel.JogadoresPorTime * (jogoSel.LimiteTimes ?? 1);

        Utils.ExibirJanela($"Jogadores por time: {jogoSel.JogadoresPorTime}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        Utils.ExibirJanela($"Limite de times: {jogoSel.LimiteTimes}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        Utils.ExibirJanela($"Limite de jogadores: {jogoSel.LimiteJogadores}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        SalvarNoArquivo();
        Utils.MensagemSucesso("Jogo atualizado!", 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    public void ExcluirJogo()
    {
        Console.Clear();
        var jogosFuturos = listaDeJogos.OrderBy(j => j.Data).ToList();
        Utils.ExibirLista(jogosFuturos.Select(jogoItem => $"ID: {jogoItem.Data:ddMMyyyy} | Data: {jogoItem.Data:dd/MM/yyyy} às 19h | Local: {jogoItem.Local} | Campo: {jogoItem.TipoCampo}"), "Jogos Disponíveis", ConsoleColor.Yellow, 70);
        if (jogosFuturos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo agendado. Pressione qualquer tecla para voltar...", 70);
            Console.ReadKey();
            return;
        }
        Utils.ExibirJanela("Excluir Jogo", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        Console.Write("Digite o código do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigo))
        {
            Utils.MensagemErro("Código inválido, voltando ao menu.", 70);
            Console.ReadKey();
            return;
        }
        var jogo = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogo == null)
        {
            Utils.MensagemErro("Jogo não encontrado!", 70);
        }
        else
        {
            listaDeJogos.Remove(jogo);
            SalvarNoArquivo();
            Utils.MensagemSucesso("Jogo removido!", 70);
        }
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    public void GerenciarInteressados()
    {
        Console.Clear();
        Utils.ExibirJanela("Gerenciar Interessados no Jogo", Array.Empty<string>(), ConsoleColor.Yellow, 70);
        var jogosFuturos = listaDeJogos.OrderBy(j => j.Data).ToList();
        if (jogosFuturos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo agendado. Pressione qualquer tecla para voltar...", 70);
            Console.ReadKey();
            return;
        }
        Utils.ExibirLista(jogosFuturos.Select(jogoItem => $"ID: {jogoItem.Data:ddMMyyyy} | Data: {jogoItem.Data:dd/MM/yyyy} às 19h | Local: {jogoItem.Local} | Campo: {jogoItem.TipoCampo}"), "Jogos Disponíveis", ConsoleColor.Yellow, 70);
        Utils.ExibirJanela("Digite o código do jogo para gerenciar interessados:", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.Write("Digite o código do jogo: ");
        long.TryParse(Console.ReadLine(), out long codigo);
        var jogoSel = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogoSel == null)
        {
            Utils.MensagemErro("Jogo não encontrado!", 70);
            Console.ReadKey();
            return;
        }
        string caminhoAssociados = Database.GetDatabaseFilePath("associados.json");
        List<Associacao.Associados> associados = new List<Associacao.Associados>();
        if (File.Exists(caminhoAssociados))
        {
            string jsonAssociados = File.ReadAllText(caminhoAssociados);
            associados = JsonSerializer.Deserialize<List<Associacao.Associados>>(jsonAssociados) ?? new List<Associacao.Associados>();
        }
        while (true)
        {
            var interessados = jogoSel.Interessados.Select(id =>
            {
                var assoc = associados.FirstOrDefault(a => a.Id == id);
                if (assoc != null)
                {
                    string pos = assoc.posicao == (Associacao.Posicao)3 ? "Goleiro" : assoc.posicao.ToString();
                    return $"- {assoc.nome} (RA: {assoc.Id}) | Posição: {pos}";
                }
                return null;
            }).Where(x => x != null).ToList();
            Utils.ExibirLista(interessados, "Interessados neste jogo", ConsoleColor.Yellow, 70);
            int goleiros = jogoSel.Interessados.Select(id => associados.FirstOrDefault(a => a.Id == id)).Count(a => a != null && a.posicao == (Associacao.Posicao)3);
            int limiteTimes = jogoSel.LimiteTimes ?? 1;
            int limiteJogadores = jogoSel.LimiteJogadores ?? 0;
            int faltamGoleiros = limiteTimes - goleiros;
            if (faltamGoleiros > 0)
                Utils.ExibirJanela($"Atenção: Faltam {faltamGoleiros} goleiro(s) para completar todos os times!", Array.Empty<string>(), ConsoleColor.Red, 70);
            else
                Utils.ExibirJanela("Quantidade de goleiros suficiente para os times.", Array.Empty<string>(), ConsoleColor.Green, 70);
            Utils.ExibirJanela($"Total de interessados: {jogoSel.Interessados.Count}/{limiteJogadores}", Array.Empty<string>(), ConsoleColor.Yellow, 70);
            if (jogoSel.Interessados.Count >= limiteJogadores)
                Utils.ExibirJanela("Limite de interessados atingido! Não é possível adicionar mais.", Array.Empty<string>(), ConsoleColor.Red, 70);
            Utils.ExibirJanela("Escolha uma opção:", new[] { "1 - Adicionar novo interessado", "2 - Remover interesse de um jogador", "0 - Sair" }, ConsoleColor.DarkGray, 70);
            Console.Write("Opção: ");
            string? op = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(op))
            {
                Utils.MensagemErro("Opção inválida!", 70);
                continue;
            }
            if (op == "0") break;
            if (op == "1")
            {
                if (jogoSel.Interessados.Count >= limiteJogadores)
                {
                    Utils.MensagemErro("Limite de interessados atingido!", 70);
                    continue;
                }
                Utils.ExibirJanela("Digite o código do jogador interessado:", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
                long.TryParse(Console.ReadLine(), out long jogadorId);
                if (!associados.Any(a => a.Id == jogadorId))
                {
                    Utils.MensagemErro("RA não encontrado nos associados.", 70);
                    continue;
                }
                if (!jogoSel.Interessados.Contains((int)jogadorId))
                {
                    jogoSel.Interessados.Add((int)jogadorId);
                    Utils.MensagemSucesso("Adicionado!", 70);
                }
                else
                {
                    Utils.MensagemErro("Jogador já está na lista de interessados.", 70);
                }
            }
            else if (op == "2")
            {
                Utils.ExibirJanela("Digite o código do jogador para remover:", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
                long.TryParse(Console.ReadLine(), out long jogadorId);
                if (jogoSel.Interessados.Contains((int)jogadorId))
                {
                    jogoSel.Interessados.Remove((int)jogadorId);
                    Utils.MensagemSucesso("Removido!", 70);
                }
                else
                {
                    Utils.MensagemErro("Jogador não está na lista de interessados.", 70);
                }
            }
            else
            {
                Utils.MensagemErro("Opção inválida!", 70);
            }
        }
        SalvarNoArquivo();
        Utils.MensagemSucesso("Interessados atualizados!", 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }
    private void SalvarNoArquivo()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(listaDeJogos, options);
        var dir = Path.GetDirectoryName(caminhoArquivo);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllText(caminhoArquivo, json);
    }

    private void CarregarDoArquivo()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            if (!string.IsNullOrWhiteSpace(json))
            {
                var tempList = JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(json);
                listaDeJogos = tempList ?? new List<GerenciadorDeJogos>();
            }
            else
                listaDeJogos = new List<GerenciadorDeJogos>();
        }
        else
        {
            listaDeJogos = new List<GerenciadorDeJogos>();
        }
    }
}