using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Partidas;
using TimeFutebol;

public class PartidaController
{
    private readonly string caminhoHistorico = @"c:\\Users\\aliss\\Documents\\Faculdade\\Programação Orientada a Objetos\\Projeto Futebol\\Projeto_futebol\\Util\\Database\\historico_partidas.json";

    public void RegistrarPartida()
    {
        Console.Clear();
        Console.WriteLine("--- Registrar Nova Partida ---");
        // Carregar jogos e times formados
        var jogos = CarregarJogosComTimes();
        if (jogos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo com times formados encontrado.");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("Selecione o jogo pelo ID:");
        foreach (var jogo in jogos)
        {
            string codigo = jogo.Data.ToString("ddMMyyyy");
            Console.WriteLine($"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} | Times: {jogo.TimesGerados.Count}");
        }
        Console.Write("Digite o ID do jogo: ");
        string? codigoStr = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(codigoStr) || !long.TryParse(codigoStr, out long codigoBusca))
        {
            Console.WriteLine("ID inválido!");
            Console.ReadKey();
            return;
        }
        var jogoSelecionado = jogos.FirstOrDefault(j => j.Codigo == codigoBusca);
        if (jogoSelecionado == null || jogoSelecionado.TimesGerados == null || jogoSelecionado.TimesGerados.Count < 2)
        {
            Console.WriteLine("Jogo não encontrado ou não há times suficientes.");
            Console.ReadKey();
            return;
        }
        Console.WriteLine("Escolha o modo de disputa:");
        Console.WriteLine("1 - Quem ganha fica");
        Console.WriteLine("2 - Cada time joga duas vezes (exceto a primeira partida)");
        Console.Write("Modo: ");
        string? modo = Console.ReadLine();
        if (modo == "1")
            RegistrarPartidaQuemGanhaFica(jogoSelecionado);
        else if (modo == "2")
            RegistrarPartidaDoisJogosPorTime(jogoSelecionado);
        else
        {
            Console.WriteLine("Modo inválido!");
            Console.ReadKey();
        }
    }

    private List<GerenciadorJogos.GerenciadorDeJogos> CarregarJogosComTimes()
    {
        string caminho = @"c:\\Users\\aliss\\Documents\\Faculdade\\Programação Orientada a Objetos\\Projeto Futebol\\Projeto_futebol\\Util\\Database\\jogos.json";
        if (!File.Exists(caminho))
            return new List<GerenciadorJogos.GerenciadorDeJogos>();
        string jsonString = File.ReadAllText(caminho);
        var lista = JsonSerializer.Deserialize<List<GerenciadorJogos.GerenciadorDeJogos>>(jsonString);
        return lista?.Where(j => j.TimesGerados != null && j.TimesGerados.Count >= 2).ToList() ?? new List<GerenciadorJogos.GerenciadorDeJogos>();
    }

    private void RegistrarPartidaQuemGanhaFica(GerenciadorJogos.GerenciadorDeJogos jogo)
    {
        Console.Clear();
        Console.WriteLine("--- Modo: Quem Ganha Fica ---");
        var times = jogo.TimesGerados.ToList();
        int rodada = 1;
        int idxRei = 0;
        int idxDesafiante = 1;
        var historico = new List<Partidas.HistoricoRodada>();
        while (times.Count > 1)
        {
            Console.WriteLine($"\nRodada {rodada}: {times[idxRei].Nome} x {times[idxDesafiante].Nome}");
            Console.WriteLine($"1 - {times[idxRei].Nome}");
            Console.WriteLine($"2 - {times[idxDesafiante].Nome}");
            Console.Write("Quem venceu? ");
            string? vencedor = Console.ReadLine();
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
            times.RemoveAt(idxPerdedor);
            if (idxVencedor == 1) idxRei = 0; // desafiante vira rei
            idxDesafiante = (idxRei + 1) % times.Count;
            rodada++;
        }
        SalvarHistorico(historico);
        Console.WriteLine("Partida registrada!");
        Console.ReadKey();
    }

    private void RegistrarPartidaDoisJogosPorTime(GerenciadorJogos.GerenciadorDeJogos jogo)
    {
        Console.Clear();
        Console.WriteLine("--- Modo: Chaveamento Duas Rodadas e Eliminação ---");
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
            Console.WriteLine($"Rodada {rodada}: {times[t1].Nome} x {times[t2].Nome}");
            Console.WriteLine($"1 - {times[t1].Nome}");
            Console.WriteLine($"2 - {times[t2].Nome}");
            Console.Write("Quem venceu? ");
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
                Console.WriteLine($"Rodada {rodada}: {times[t1].Nome} x {times[t2].Nome}");
                Console.WriteLine($"1 - {times[t1].Nome}");
                Console.WriteLine($"2 - {times[t2].Nome}");
                Console.Write("Quem venceu? ");
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
            Console.WriteLine($"\nVencedor final: {times[idxTimesAtivos[0]].Nome}");
        SalvarHistorico(historico);
        Console.WriteLine("Partida registrada!");
        Console.ReadKey();
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
            Console.WriteLine("Nenhuma partida registrada.");
        }
        else
        {
            foreach (var jogo in jogos.OrderBy(j => j.Data))
            {
                string codigo = jogo.Data.ToString("ddMMyyyy");
                Console.WriteLine($"\nJogo ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} | Local: {jogo.Local} | Campo: {jogo.TipoCampo}");
                var rodadasDoJogo = historico.Where(h => h.codigoJogo == jogo.Codigo).ToList();
                if (rodadasDoJogo.Count == 0)
                {
                    Console.WriteLine("  Nenhuma partida registrada para este jogo.");
                }
                else
                {
                    foreach (var partida in rodadasDoJogo)
                    {
                        string nomeRei = (jogo.TimesGerados != null && partida.reiAntes < jogo.TimesGerados.Count) ? jogo.TimesGerados[partida.reiAntes].Nome : $"Time {partida.reiAntes + 1}";
                        string nomeDesafiante = (jogo.TimesGerados != null && partida.desafiante < jogo.TimesGerados.Count) ? jogo.TimesGerados[partida.desafiante].Nome : $"Time {partida.desafiante + 1}";
                        string nomeVencedor = (jogo.TimesGerados != null && partida.vencedor < jogo.TimesGerados.Count) ? jogo.TimesGerados[partida.vencedor].Nome : $"Time {partida.vencedor + 1}";
                        Console.WriteLine($"  Rodada {partida.rodada}: {nomeRei} x {nomeDesafiante} | Vencedor: {nomeVencedor}");
                    }
                    // Exibe o vencedor geral do jogo
                    var ultimaRodada = rodadasDoJogo.LastOrDefault();
                    if (ultimaRodada != null && jogo.TimesGerados != null && ultimaRodada.vencedor < jogo.TimesGerados.Count)
                    {
                        Console.WriteLine($"  Vencedor do jogo: {jogo.TimesGerados[ultimaRodada.vencedor].Nome}");
                    }
                }
            }
        }
        Console.WriteLine("\nPressione qualquer tecla para voltar...");
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
}
