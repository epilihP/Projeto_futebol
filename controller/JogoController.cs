using System;
using GerenciadorJogos;

public class JogoController
{
    public void CadastrarJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Cadastro de Novo Jogo ---");

        Console.Write("Local: ");
        string local = Console.ReadLine();

        Console.Write("Tipo de Campo: ");
        string tipoCampo = Console.ReadLine();

        Console.Write("Jogadores por Time: ");
        int.TryParse(Console.ReadLine(), out int jogadoresPorTime);

        Console.Write("Limite de Times: ");
        int.TryParse(Console.ReadLine(), out int limiteTimes);

        Console.Write("Limite de Jogadores: ");
        int.TryParse(Console.ReadLine(), out int limiteJogadores);

        Console.Write("Dia da Semana (ex: Monday): ");
        string diaSemana = Console.ReadLine();
        DayOfWeek data;
        Enum.TryParse(diaSemana, true, out data);

        var jogo = new GerenciadorDeJogos(data, local, tipoCampo, jogadoresPorTime, limiteTimes, limiteJogadores)
        {
            Codigo = DateTime.Now.Ticks
        };

        Database.SalvarJogo(jogo);

        Console.WriteLine("\nJogo cadastrado com sucesso!");
        Console.ReadKey();
    }

    public void ListarJogos()
    {
        Console.Clear();
        Console.WriteLine("--- Lista de Jogos ---");
        var jogos = Database.ListarJogos();
        if (jogos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo cadastrado.");
        }
        else
        {
            foreach (var jogo in jogos)
            {
                Console.WriteLine($"Código: {jogo.Codigo} | Local: {jogo.Local} | Tipo: {jogo.TipoCampo} | Jogadores/Time: {jogo.JogadoresPorTime} | Limite Times: {jogo.LimiteTimes} | Limite Jogadores: {jogo.LimiteJogadores} | Dia: {jogo.Data}");
            }
        }
        Console.WriteLine("\nPressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    public void AtualizarJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Atualizar Jogo ---");
        ListarJogos();
        Console.Write("\nDigite o código do jogo que deseja atualizar: ");
        if (!long.TryParse(Console.ReadLine(), out long codigo))
        {
            Console.WriteLine("Código inválido.");
            Console.ReadKey();
            return;
        }

        var jogos = Database.ListarJogos();
        var jogo = jogos.Find(j => j.Codigo == codigo);
        if (jogo == null)
        {
            Console.WriteLine("Jogo não encontrado.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nDeixe em branco para não alterar.");
        Console.Write($"Novo Local (Atual: {jogo.Local}): ");
        string local = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(local)) jogo.Local = local;

        Console.Write($"Novo Tipo de Campo (Atual: {jogo.TipoCampo}): ");
        string tipoCampo = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(tipoCampo)) jogo.TipoCampo = tipoCampo;

        Console.Write($"Novo Jogadores por Time (Atual: {jogo.JogadoresPorTime}): ");
        string jogadoresPorTimeStr = Console.ReadLine();
        if (int.TryParse(jogadoresPorTimeStr, out int jogadoresPorTime)) jogo.JogadoresPorTime = jogadoresPorTime;

        Console.Write($"Novo Limite de Times (Atual: {jogo.LimiteTimes}): ");
        string limiteTimesStr = Console.ReadLine();
        if (int.TryParse(limiteTimesStr, out int limiteTimes)) jogo.LimiteTimes = limiteTimes;

        Console.Write($"Novo Limite de Jogadores (Atual: {jogo.LimiteJogadores}): ");
        string limiteJogadoresStr = Console.ReadLine();
        if (int.TryParse(limiteJogadoresStr, out int limiteJogadores)) jogo.LimiteJogadores = limiteJogadores;

        Console.Write($"Novo Dia da Semana (Atual: {jogo.Data}): ");
        string diaSemana = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(diaSemana) && Enum.TryParse(diaSemana, true, out DayOfWeek data)) jogo.Data = data;

        // Atualiza no arquivo
        Database.AtualizarJogo((int)codigo, jogo);

        Console.WriteLine("\nJogo atualizado com sucesso!");
        Console.ReadKey();
    }

    public void ExcluirJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Excluir Jogo ---");
        ListarJogos();
        Console.Write("\nDigite o código do jogo que deseja excluir: ");
        if (!long.TryParse(Console.ReadLine(), out long codigo))
        {
            Console.WriteLine("Código inválido.");
            Console.ReadKey();
            return;
        }

        bool removido = Database.RemoverJogo((int)codigo);
        if (removido)
            Console.WriteLine("Jogo excluído com sucesso!");
        else
            Console.WriteLine("Jogo não encontrado.");

        Console.ReadKey();
    }
}