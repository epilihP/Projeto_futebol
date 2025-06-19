namespace TimeFutebol;

using System.Collections.Generic;
using System;
using Jogoss;
using Partidas;
using Players;
using Associacao;
using GerenciadorJogos;

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
        Console.WriteLine("Goleiros:");
        foreach (var g in goleiros) Console.WriteLine($"- {g.nome}");
        Console.WriteLine("Jogadores de linha:");
        foreach (var j in jogadoresLinha) Console.WriteLine($"- {j.nome}");
        // Implemente aqui a lógica de formação de times conforme necessário
    }
}