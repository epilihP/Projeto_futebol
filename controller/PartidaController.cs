using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Partidas;
using TimeFutebol;
using Util.Database;
using Projeto_futebol.Util;

namespace PROJETO_FUTEBOL.controller;

public class PartidaController
{
    private readonly string caminhoHistorico = Database.GetDatabaseFilePath("historico_partidas.json");

    public void RegistrarPartida()
    {
        Console.Clear();
        Utils.ExibirJanela("Registrar Nova Partida", Array.Empty<string>(), ConsoleColor.Green, 70);
        // Carregar jogos e times formados
        var jogos = CarregarJogosComTimes();
        if (jogos.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum jogo com times formados encontrado.", 70);
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        Utils.ExibirLista(jogos.Select(jogo => $"ID: {jogo.Data:ddMMyyyy} | Data: {jogo.Data:dd/MM/yyyy} | Times: {jogo.TimesGerados.Count}"), "Jogos com Times Formados", ConsoleColor.Green, 70);
        Utils.ExibirJanela("Digite o ID do jogo:", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigoBusca))
        {
            Utils.MensagemErro("ID inválido, voltando ao menu.", 70);
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        var jogoSelecionado = jogos.FirstOrDefault(j => j.Codigo == codigoBusca);
        if (jogoSelecionado == null || jogoSelecionado.TimesGerados == null || jogoSelecionado.TimesGerados.Count < 2)
        {
            Utils.MensagemErro("Jogo não encontrado ou não há times suficientes.", 70);
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
            return;
        }
        Utils.ExibirJanela("Escolha o modo de disputa:", new[] { "1 - Quem ganha fica", "2 - Cada time joga duas vezes (exceto a primeira partida)" }, ConsoleColor.Green, 70);
        Utils.ExibirJanela("Modo:", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        string? modo = Console.ReadLine();
        if (modo == "1")
            RegistrarPartidaQuemGanhaFica(jogoSelecionado);
        else if (modo == "2")
            RegistrarPartidaDoisJogosPorTime(jogoSelecionado);
        else
        {
            Utils.MensagemErro("Modo inválido!", 70);
            Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            Console.ReadKey();
        }
    }

    private List<GerenciadorJogos.GerenciadorDeJogos> CarregarJogosComTimes()
    {
        string caminho = Database.GetDatabaseFilePath("jogos.json");
        if (!File.Exists(caminho))
            return new List<GerenciadorJogos.GerenciadorDeJogos>();
        string jsonString = File.ReadAllText(caminho);
        var lista = JsonSerializer.Deserialize<List<GerenciadorJogos.GerenciadorDeJogos>>(jsonString);
        return lista?.Where(j => j.TimesGerados != null && j.TimesGerados.Count >= 2).ToList() ?? new List<GerenciadorJogos.GerenciadorDeJogos>();
    }

    private void SalvarHistorico(List<Partidas.HistoricoRodada> historico)
    {
        var historicoAntigo = CarregarHistorico();
        historicoAntigo.AddRange(historico);
        string json = JsonSerializer.Serialize(historicoAntigo, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(caminhoHistorico, json);
    }

    public void ListarPartidas()
    {
        var historico = CarregarHistorico();
        var jogos = CarregarJogosComTimes();
        if (historico.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhuma partida registrada.", 70);
        }
        else
        {
            foreach (var jogo in jogos.OrderBy(j => j.Data))
            {
                string codigo = jogo.Data.ToString("ddMMyyyy");
                var rodadasDoJogo = historico.Where(h => h.codigoJogo == jogo.Codigo).ToList();
                var linhas = new List<string>();
                linhas.Add($"Jogo ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} | Local: {jogo.Local} | Campo: {jogo.TipoCampo}");
                if (rodadasDoJogo.Count == 0)
                {
                    linhas.Add("Nenhuma partida registrada para este jogo.");
                }
                else
                {
                    foreach (var partida in rodadasDoJogo)
                    {
                        string nomeRei = (jogo.TimesGerados != null && partida.reiAntes < jogo.TimesGerados.Count) ? jogo.TimesGerados[partida.reiAntes].Nome : $"Time {partida.reiAntes + 1}";
                        string nomeDesafiante = (jogo.TimesGerados != null && partida.desafiante < jogo.TimesGerados.Count) ? jogo.TimesGerados[partida.desafiante].Nome : $"Time {partida.desafiante + 1}";
                        string nomeVencedor = (jogo.TimesGerados != null && partida.vencedor < jogo.TimesGerados.Count && partida.vencedor >= 0) ? jogo.TimesGerados[partida.vencedor].Nome : (partida.vencedor == -1 ? "Empate" : $"Time {partida.vencedor + 1}");
                        linhas.Add($"Rodada {partida.rodada}: {nomeRei} x {nomeDesafiante} | Vencedor: {nomeVencedor}");
                    }
                    // Exibe o vencedor geral do jogo
                    var ultimaRodada = rodadasDoJogo.LastOrDefault();
                    if (ultimaRodada != null && jogo.TimesGerados != null && ultimaRodada.vencedor < jogo.TimesGerados.Count && ultimaRodada.vencedor >= 0)
                    {
                        linhas.Add($"Vencedor do jogo: {jogo.TimesGerados[ultimaRodada.vencedor].Nome}");
                    }
                    else if (ultimaRodada != null && ultimaRodada.vencedor == -1)
                    {
                        linhas.Add("O jogo terminou empatado.");
                    }
                }
                Utils.ExibirJanela($"Partidas do Jogo {codigo}", linhas.ToArray(), ConsoleColor.Green, 70);
            }
        }
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    private List<HistoricoRodada> CarregarHistorico()
    {
        if (!File.Exists(caminhoHistorico))
            return new List<HistoricoRodada>();
        string json = File.ReadAllText(caminhoHistorico);
        var lista = JsonSerializer.Deserialize<List<HistoricoRodada>>(json);
        return lista ?? new List<HistoricoRodada>();
    }

    public void ExibirClassificacaoAssociados()
    {
        string caminhoAssociados = Database.GetDatabaseFilePath("associados.json");
        if (!File.Exists(caminhoAssociados))
        {
            Utils.MensagemRetornoMenu("Nenhum associado cadastrado.", 70);
            Console.ReadKey();
            return;
        }
        var json = File.ReadAllText(caminhoAssociados);
        var associados = System.Text.Json.JsonSerializer.Deserialize<List<Associacao.Associados>>(json) ?? new List<Associacao.Associados>();
        if (associados.Count == 0)
        {
            Utils.MensagemRetornoMenu("Nenhum associado cadastrado.", 70);
            Console.ReadKey();
            return;
        }
        var ranking = associados.OrderByDescending(a => a.Pontos).ThenBy(a => a.nome).ToList();
        Console.Clear();
        var linhas = new List<string>();
        linhas.Add("Posição | Nome                | Pontos");
        linhas.Add("--------------------------------------");
        int pos = 1;
        foreach (var a in ranking)
        {
            linhas.Add($"{pos,6} | {a.nome,-20} | {a.Pontos,6}");
            pos++;
        }
        Utils.ExibirJanela("Classificação de Associados", linhas.ToArray(), ConsoleColor.Magenta, 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    private void AtualizarPontuacaoJogadores(GerenciadorJogos.GerenciadorDeJogos jogo, List<Partidas.HistoricoRodada> historico)
    {
        string caminhoAssociados = Database.GetDatabaseFilePath("associados.json");
        if (!File.Exists(caminhoAssociados)) return;
        var json = File.ReadAllText(caminhoAssociados);
        var associados = System.Text.Json.JsonSerializer.Deserialize<List<Associacao.Associados>>(json) ?? new List<Associacao.Associados>();

        // Zera pontos dos jogadores dos times do jogo
        foreach (var time in jogo.TimesGerados)
            foreach (var jogador in time.Jogadores)
                if (jogador != null)
                    associados.FirstOrDefault(a => a.Id == jogador.Id)!.Pontos = 0;

        // Soma pontos por rodada
        foreach (var rodada in historico)
        {
            var timeVencedor = jogo.TimesGerados[rodada.vencedor];
            var timePerdedor = jogo.TimesGerados[rodada.vencedor == rodada.reiAntes ? rodada.desafiante : rodada.reiAntes];
            foreach (var jogador in timeVencedor.Jogadores)
                if (jogador != null)
                    associados.FirstOrDefault(a => a.Id == jogador.Id)!.Pontos += 3;
            foreach (var jogador in timePerdedor.Jogadores)
                if (jogador != null)
                    associados.FirstOrDefault(a => a.Id == jogador.Id)!.Pontos += 0;
        }
        // Salva
        File.WriteAllText(caminhoAssociados, System.Text.Json.JsonSerializer.Serialize(associados, new JsonSerializerOptions { WriteIndented = true }));
    }

    private void RegistrarPartidaQuemGanhaFica(GerenciadorJogos.GerenciadorDeJogos jogo)
    {
        Console.Clear();
        Utils.ExibirJanela("Modo: Quem Ganha Fica", Array.Empty<string>(), ConsoleColor.Green, 70);
        var times = jogo.TimesGerados.ToList();
        int rodada = 1;
        int idxRei = 0;
        int idxDesafiante = 1;
        var historico = new List<Partidas.HistoricoRodada>();
        while (times.Count > 1)
        {
            var linhas = new List<string>
            {
                $"Rodada {rodada}: {times[idxRei].Nome} x {times[idxDesafiante].Nome}",
                $"1 - {times[idxRei].Nome}",
                $"2 - {times[idxDesafiante].Nome}"
            };
            Utils.ExibirJanela("Disputa", linhas.ToArray(), ConsoleColor.Green, 70);
            Utils.ExibirJanela("Quem venceu? (1, 2 ou E para empate):", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            string? vencedor = Console.ReadLine();
            if (vencedor?.ToUpper() == "E")
            {
                historico.Add(new Partidas.HistoricoRodada
                {
                    codigoJogo = jogo.Codigo,
                    rodada = rodada,
                    reiAntes = idxRei,
                    desafiante = idxDesafiante,
                    vencedor = -1 // empate
                });
                foreach (var jogador in times[idxRei].Jogadores)
                    if (jogador != null)
                        AtualizarPontosJogador(jogador.Id, 1);
                foreach (var jogador in times[idxDesafiante].Jogadores)
                    if (jogador != null)
                        AtualizarPontosJogador(jogador.Id, 1);
            }
            else
            {
                int idxVencedor = vencedor == "1" ? idxRei : idxDesafiante;
                int idxPerdedor = vencedor == "1" ? idxDesafiante : idxRei;
                historico.Add(new Partidas.HistoricoRodada
                {
                    codigoJogo = jogo.Codigo,
                    rodada = rodada,
                    reiAntes = idxRei,
                    desafiante = idxDesafiante,
                    vencedor = idxVencedor
                });
                foreach (var jogador in times[idxVencedor].Jogadores)
                    if (jogador != null)
                        AtualizarPontosJogador(jogador.Id, 3);
                foreach (var jogador in times[idxPerdedor].Jogadores)
                    if (jogador != null)
                        AtualizarPontosJogador(jogador.Id, 0);
                times.RemoveAt(idxPerdedor);
                if (idxVencedor == 1) idxRei = 0;
                idxDesafiante = (idxRei + 1) % times.Count;
            }
            rodada++;
        }
        SalvarHistorico(historico);
        Utils.MensagemSucesso("Partida registrada!", 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }

    private void AtualizarPontosJogador(int id, int pontos)
    {
        string caminhoAssociados = Database.GetDatabaseFilePath("associados.json");
        if (!File.Exists(caminhoAssociados)) return;
        var json = File.ReadAllText(caminhoAssociados);
        var associados = System.Text.Json.JsonSerializer.Deserialize<List<Associacao.Associados>>(json) ?? new List<Associacao.Associados>();
        var jogador = associados.FirstOrDefault(a => a.Id == id);
        if (jogador != null)
        {
            jogador.Pontos += pontos;
            File.WriteAllText(caminhoAssociados, System.Text.Json.JsonSerializer.Serialize(associados, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    private void RegistrarPartidaDoisJogosPorTime(GerenciadorJogos.GerenciadorDeJogos jogo)
    {
        Console.Clear();
        Utils.ExibirJanela("Modo: Chaveamento Duas Rodadas e Eliminação", Array.Empty<string>(), ConsoleColor.Green, 70);
        var times = jogo.TimesGerados.ToList();
        int n = times.Count;
        var derrotas = new int[n];
        var historico = new List<Partidas.HistoricoRodada>();
        int rodada = 1;
        var idxTimesAtivos = Enumerable.Range(0, n).ToList();

        // Primeira rodada: todos jogam (pares)
        for (int i = 0; i < idxTimesAtivos.Count - 1; i += 2)
        {
            int t1 = idxTimesAtivos[i];
            int t2 = idxTimesAtivos[i + 1];
            var linhas = new List<string>
            {
                $"Rodada {rodada}: {times[t1].Nome} x {times[t2].Nome}",
                $"1 - {times[t1].Nome}",
                $"2 - {times[t2].Nome}"
            };
            Utils.ExibirJanela("Disputa", linhas.ToArray(), ConsoleColor.Green, 70);
            Utils.ExibirJanela("Quem venceu? (1 ou 2):", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
            string? vencedor = Console.ReadLine();
            int idxVencedor = vencedor == "1" ? t1 : t2;
            int idxPerdedor = vencedor == "1" ? t2 : t1;
            historico.Add(new Partidas.HistoricoRodada
            {
                codigoJogo = jogo.Codigo,
                rodada = rodada,
                reiAntes = t1,
                desafiante = t2,
                vencedor = idxVencedor
            });
            derrotas[idxPerdedor]++;
            rodada++;
        }

        // Segunda rodada: vencedores vs vencedores, perdedores vs perdedores
        while (idxTimesAtivos.Count(t => derrotas[t] < 2) > 1)
        {
            var vencedores = idxTimesAtivos.Where(t => derrotas[t] == 0).ToList();
            var perdedores = idxTimesAtivos.Where(t => derrotas[t] == 1).ToList();
            var novaRodada = new List<(int, int)>();
            // Vencedores entre si
            for (int i = 0; i < vencedores.Count - 1; i += 2)
                novaRodada.Add((vencedores[i], vencedores[i + 1]));
            // Perdedor entre si
            for (int i = 0; i < perdedores.Count - 1; i += 2)
                novaRodada.Add((perdedores[i], perdedores[i + 1]));
            if (novaRodada.Count == 0) break;
            foreach (var (t1, t2) in novaRodada)
            {
                var linhas = new List<string>
                {
                    $"Rodada {rodada}: {times[t1].Nome} x {times[t2].Nome}",
                    $"1 - {times[t1].Nome}",
                    $"2 - {times[t2].Nome}"
                };
                Utils.ExibirJanela("Disputa", linhas.ToArray(), ConsoleColor.Green, 70);
                Utils.ExibirJanela("Quem venceu? (1 ou 2):", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
                string? vencedor = Console.ReadLine();
                int idxVencedor = vencedor == "1" ? t1 : t2;
                int idxPerdedor = vencedor == "1" ? t2 : t1;
                historico.Add(new Partidas.HistoricoRodada
                {
                    codigoJogo = jogo.Codigo,
                    rodada = rodada,
                    reiAntes = t1,
                    desafiante = t2,
                    vencedor = idxVencedor
                });
                derrotas[idxPerdedor]++;
                rodada++;
            }
            // Elimina quem perdeu duas vezes
            idxTimesAtivos = idxTimesAtivos.Where(t => derrotas[t] < 2).ToList();
        }
        // Exibe vencedor final
        if (idxTimesAtivos.Count == 1)
        {
            var vencedorFinal = times[idxTimesAtivos[0]].Nome;
            Utils.ExibirJanela($"Vencedor final: {vencedorFinal}", Array.Empty<string>(), ConsoleColor.Green, 70);
        }
        SalvarHistorico(historico);
        Utils.MensagemSucesso("Partida registrada!", 70);
        Utils.ExibirJanela("Pressione qualquer tecla para voltar...", Array.Empty<string>(), ConsoleColor.DarkGray, 70);
        Console.ReadKey();
    }
}
