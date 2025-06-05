namespace GerenciadorJogos;
using Jogos;
using Players;
using System.Collections.Generic;

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
        if (LimiteTimes.HasValue && LimiteTimes.Value >= 2)
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

    public void ListaDeInteressadosAberta()
    {
        if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
        {
            
        }
    }
}




//talves n precise ser em json porque não precisa de banco de dados.
// nos jogos o que da ja temos função para criar
// 