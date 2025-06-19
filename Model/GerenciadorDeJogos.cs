namespace GerenciadorJogos;

using Jogoss;
using System.Collections.Generic;
using PROJETO_FUTEBOL.controller;
using Players;
using TimeFutebol;

public class GerenciadorDeJogos : Jogos
{
    public List<Time> TimesGerados { get; set; } = new List<Time>();

    public GerenciadorDeJogos(DateTime data, string local, string tipoCampo, int jogadoresPorTime, int? limiteTimes = null, int? limiteJogadores = 20)
    {
        Data = data;
        Local = local;
        TipoCampo = tipoCampo;
        JogadoresPorTime = jogadoresPorTime;
        LimiteTimes = limiteTimes;
        LimiteJogadores = limiteJogadores;
        Interessados = new List<int>();
    }

    public void AdicionarInteressado(int idJogador)
    {
        if (LimiteJogadores.HasValue && Interessados.Count < LimiteJogadores.Value)
            Interessados.Add(idJogador);
        else
            Projeto_futebol.Util.Utils.ExibirJanela("Limite de jogadores atingido!", Array.Empty<string>(), ConsoleColor.Yellow, 70);
    }

    public void ExibirInteressados(List<Jogador> jogadores)
    {
        var linhas = new List<string>();
        foreach (var codigo in Interessados)
        {
            var jogador = jogadores.FirstOrDefault(j => j.RA == codigo);
            if (jogador != null)
                linhas.Add($"{jogador.nome} (Código: {jogador.RA})");
        }
        Projeto_futebol.Util.Utils.ExibirJanela("Interessados:", linhas.ToArray(), ConsoleColor.Yellow, 70);
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

    public bool ListaDeInteressadosDisponivel()
    {
        // Agora compara apenas o dia da semana
        if (DateTime.Now.DayOfWeek != Data.DayOfWeek)
        {
            Projeto_futebol.Util.Utils.ExibirJanela("Aberta!", Array.Empty<string>(), ConsoleColor.Yellow, 70);
            return true;
        }
        else
        {
            Projeto_futebol.Util.Utils.ExibirJanela("Fechado", Array.Empty<string>(), ConsoleColor.Yellow, 70);
            return false;
        }
    }
}