namespace GerenciadorJogos;
using Jogos;
using System.Collections.Generic;
using PROJETO_FUTEBOL.controller;
using Players;


public class GerenciadorDeJogos : Jogos
{
    public GerenciadorDeJogos(DayOfWeek data, string local, string tipoCampo, int jogadoresPorTime, int? limiteTimes = null, int? limiteJogadores = 20)
    {
        Data = data;
        Local = local;
        TipoCampo = tipoCampo;
        JogadoresPorTime = jogadoresPorTime;
        LimiteTimes = limiteTimes;
        LimiteJogadores = limiteJogadores;
        Interessados = new List<int>(); // linkar com os jogadores
    }

    public void AdicionarInteressado(int idJogador)
    {
        if (LimiteJogadores.HasValue && Interessados.Count < LimiteJogadores.Value)
            Interessados.Add(idJogador);
        else
            Console.WriteLine("Limite de jogadores atingido!");
    }

    public void ExibirInteressados(List<Jogador> jogadores)
    {
        Console.WriteLine("Interessados:");
        foreach (var codigo in Interessados)
        {
            var jogador = jogadores.FirstOrDefault(j => j.RA == codigo);
            if (jogador != null)
                Console.WriteLine($"{jogador.nome} (Código: {jogador.RA})");
        }
    }
    public bool PodeConfirmarPartida()
    {
        if (!ListaDeInteressadosDisponivel())
        {
            // Verifica se existem pelo menos 2 times completos
            return Interessados.Count >= 2 * JogadoresPorTime;
        }
        return false;
    }

    public void ClearInteressados()
    {
        Interessados.Clear();
    }
    public bool ListaDeInteressadosDisponivel()       //use isso para conferir se há jogo, logo se n for dia de jogo pode adicionar pessoas a lista, então use ela pa
    {
        if (DateTime.Now.DayOfWeek != Data)
        {
            Console.WriteLine("Aberta!");
            return true;
        }
        
        else
        {
            Console.WriteLine("Fechado");
            return false;

        }

    }
}

