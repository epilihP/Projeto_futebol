namespace TimeFutebol;

using System.Collections.Generic;
using System;
using Jogoss;
using Partidas;
using Players;
using Associacao;
using GerenciadorJogos;
using Projeto_futebol.Util;

public class Time : GerenciadorDeJogos
{
    public List<Associados> Jogadores { get; set; } = new List<Associados>();
    public string Nome { get; set; }

    public Time(DateTime data, string local, string tipoCampo, int jogadoresPorTime, int? limiteTimes = null, int? limiteJogadores = 20)
        : base(data, local, tipoCampo, jogadoresPorTime, limiteTimes, limiteJogadores)
    {
        Nome = string.Empty;
    }

    public void StartToPlay(List<Jogador> jogadores)
    {
        var interessadosFila = new List<int>(Interessados);
        var goleiros = jogadores.Where(j => j.posicao == Posicao.Goleiro && interessadosFila.Contains(j.RA)).ToList();
        var jogadoresLinha = jogadores.Where(j => j.posicao != Posicao.Goleiro && interessadosFila.Contains(j.RA)).ToList();
        var linhasGoleiros = goleiros.Select(g => $"- {g.nome}").ToList();
        var linhasJogadores = jogadoresLinha.Select(j => $"- {j.nome}").ToList();
        Utils.ExibirJanela("Goleiros:", linhasGoleiros.ToArray(), ConsoleColor.Cyan, 70);
        Utils.ExibirJanela("Jogadores de linha:", linhasJogadores.ToArray(), ConsoleColor.Cyan, 70);
        // Implemente aqui a lógica de formação de times conforme necessário
    }
}