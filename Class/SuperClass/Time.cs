namespace Time;
using System.Collections.Generic;
using System;
using Jogos;
using Partidas;
using Players;
using GerenciadorJogos;

public class Times : GerenciadorDeJogos
{
    public static List<string>? PrimeiroTime { get; set; } = new List<string>();
    public static List<string>? SegundoTime { get; set; } = new List<string>();
    public static List<string>? TerceiroTime { get; set; } = new List<string>();

    public Times()
    {
        PrimeiroTime = new List<string>() = 5;
        SegundoTime = new List<string>() = 5;
        TerceiroTime = new List<string>() = 5;
        
    }

    public void StartToPlay(List<Jogador> jogadores)
    {
        // a ideia é que isso ajude controlar o limite de pessoas por time alterando entre 0 e 1
        int limitador = 0;

        foreach (var jogadorInteressado in Interessados)
        {
            // var jogador = todosJogadores.FirstOrDefault(j => j.RA == jogadorInteressado);
            // var goleiro = jogadores.FirstOrDefault(j => j.Posicao == "Goleiro" && Interessados.Contains(j.RA));
            // if (PrimeiroTime <= 5)
            // {
            //     if()
            // }
        }
    }



}