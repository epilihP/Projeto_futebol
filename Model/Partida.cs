namespace Partidas;

using System;
using System.Collections.Generic;
// Classe para registrar o histórico de cada rodada
public class HistoricoRodada
{
    public long codigoJogo { get; set; } // Código do jogo
    public int rodada { get; set; }
    public int reiAntes { get; set; }
    public int desafiante { get; set; }
    public int vencedor { get; set; }
}

public class Partida
{
    public bool ComecoFim = false;
    public bool Vitoria = true;
    public int LimiteDeJogosPorTime { get; set; } = 2;
    public static List<int> contagemTimes = new List<int>();
    public List<HistoricoRodada> historico = new List<HistoricoRodada>();

    public void GerenciarPartidas()
    {
        if (ComecoFim == true) // confere se a partida começa
        {
            Random rand = new Random(); // random para definir quem vai ganhar

            int rodada = 1;
            int indexDoDesafiante = 1;
            int ReiDaQuadra = contagemTimes[0];

            List<int> JogosRegistrados = new List<int>();
            List<int> TimesComLimites = new List<int>();

            while (TimesComLimites.Count < contagemTimes.Count - 1)
            {
                if (indexDoDesafiante >= contagemTimes.Count)
                {
                    indexDoDesafiante = 1;
                }

                int contagemDoDesafiante = contagemTimes[indexDoDesafiante];

                if (TimesComLimites.Contains(contagemDoDesafiante))
                {
                    indexDoDesafiante++;
                    continue;
                }
                JogosRegistrados.Add(contagemDoDesafiante);
                if (JogosRegistrados.Count(t => t == contagemDoDesafiante) >= LimiteDeJogosPorTime)
                {
                    TimesComLimites.Add(contagemDoDesafiante);
                }

                int vencedor = rand.Next(2);

                int reiAntes = ReiDaQuadra;
                int codigoVencedor;

                if (vencedor == 1)
                {
                    // Desafiante vira rei
                    codigoVencedor = contagemDoDesafiante;
                    ReiDaQuadra = contagemDoDesafiante;
                }
                else
                {
                    // Rei joga mais uma vez
                    codigoVencedor = ReiDaQuadra;
                    JogosRegistrados.Add(ReiDaQuadra);
                }

                // Adiciona ao histórico
                historico.Add(new HistoricoRodada
                {
                    codigoJogo = 0, // Atribua o código do jogo aqui, se disponível
                    rodada = rodada,
                    reiAntes = reiAntes,
                    desafiante = contagemDoDesafiante,
                    vencedor = codigoVencedor
                });

                indexDoDesafiante++;
                rodada++;
            }
        }
    }
}