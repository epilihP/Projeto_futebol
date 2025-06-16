namespace TimeFutebol;
using System.Collections.Generic;
using System;
using Jogoss;
using Partidas;
using Players;
using Associacao;
using GerenciadorJogos;

public class Times : GerenciadorDeJogos
{
    public static List<string>? PrimeiroTime { get; set; } = new List<string>();
    public static List<string>? SegundoTime { get; set; } = new List<string>();
    public static List<string>? TerceiroTime { get; set; } = new List<string>();
    public List<Associados> Jogadores { get; set; } = new List<Associados>();
    public string Nome { get; set; }

    // Ajuste os parâmetros para casar com o construtor base corrigido!
    public Times(DateTime data, string local, string tipoCampo, int jogadoresPorTime, int? limiteTimes = null, int? limiteJogadores = 20)
        : base(data, local, tipoCampo, jogadoresPorTime, limiteTimes, limiteJogadores)
    {
        PrimeiroTime = new List<string>();
        SegundoTime = new List<string>();
        TerceiroTime = new List<string>();
    }

    // Monta dois times por ordem de chegada, priorizando goleiros
    public void StartToPlay(List<Jogador> jogadores)
    {
        PrimeiroTime?.Clear();
        SegundoTime?.Clear();
        TerceiroTime?.Clear();

        var interessadosFila = new List<int>(Interessados);

        // Separa goleiros e jogadores de linha
        var goleiros = jogadores
            .Where(j => j.posicao == Posicao.Goleiro && interessadosFila.Contains(j.RA))
            .ToList();

        var jogadoresLinha = jogadores
            .Where(j => j.posicao != Posicao.Goleiro && interessadosFila.Contains(j.RA))
            .ToList();

        // Adiciona goleiros aos times
        if (goleiros.Count > 0)
        {
            PrimeiroTime.Add(goleiros[0].nome);
            interessadosFila.Remove(goleiros[0].RA);
        }
        if (goleiros.Count > 1)
        {
            SegundoTime.Add(goleiros[1].nome);
            interessadosFila.Remove(goleiros[1].RA);
        }

        // Remove goleiros já adicionados da lista de linha (caso estejam lá)
        jogadoresLinha.RemoveAll(j => goleiros.Take(2).Any(g => g.RA == j.RA));

        // Junta o restante dos jogadores (inclusive goleiros extras, se houver)
        var resto = jogadores
            .Where(j => interessadosFila.Contains(j.RA))
            .ToList();

        // Alterna a distribuição
        bool paraPrimeiro = PrimeiroTime.Count <= SegundoTime.Count;
        foreach (var jogador in resto)
        {
            if (paraPrimeiro)
                PrimeiroTime.Add(jogador.nome);
            else
                SegundoTime.Add(jogador.nome);
            paraPrimeiro = !paraPrimeiro;
        }
    }
}