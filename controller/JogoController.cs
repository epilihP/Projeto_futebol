using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Jogoss;

public class JogoController
{
    private List<Jogos> listaDeJogos = new List<Jogos>();
    private readonly string caminhoArquivo = "Database/jogos.json";

    public JogoController()
    {
        CarregarDoArquivo();
    }

    public void AgendarJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Agendar Novo Jogo ---");
        Jogos novoJogo = new Jogos();
        novoJogo.Codigo = DateTime.Now.Ticks;
        novoJogo.Data = DayOfWeek.Thursday; // Sempre quinta-feira

        Console.WriteLine("(Deixe em branco para usar o valor padrão)");

        Console.Write($"Local do jogo (Padrão: {novoJogo.Local}): ");
        string localInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(localInput))
            novoJogo.Local = localInput;

        Console.Write($"Tipo de campo (Padrão: {novoJogo.TipoCampo}): ");
        string tipoCampoInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(tipoCampoInput))
            novoJogo.TipoCampo = tipoCampoInput;

        Console.Write("Jogadores por time: ");
        int.TryParse(Console.ReadLine(), out int jogadoresPorTime);
        novoJogo.JogadoresPorTime = jogadoresPorTime;

        Console.Write("Limite de times (opcional): ");
        int.TryParse(Console.ReadLine(), out int limiteTimes);
        novoJogo.LimiteTimes = limiteTimes;

        Console.Write("Limite de jogadores (opcional): ");
        int.TryParse(Console.ReadLine(), out int limiteJogadores);
        novoJogo.LimiteJogadores = limiteJogadores;

        listaDeJogos.Add(novoJogo);
        SalvarNoArquivo();

        Console.WriteLine("\nJogo agendado com sucesso!");
        Console.ReadKey();
    }

    public void ListarJogos()
    {
        Console.Clear();
        Console.WriteLine("--- Lista de Jogos Agendados ---");
        if (listaDeJogos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo agendado.");
        }
        else
        {
            foreach (var jogo in listaDeJogos)
            {
                Console.WriteLine($"ID: {jogo.Codigo} | Dia: {DiaDaSemanaEmPortugues(jogo.Data)} | Local: {jogo.Local} | Campo: {jogo.TipoCampo} | {jogo.JogadoresPorTime} por time");
            }
        }
        Console.ReadKey();
    }

    public void AtualizarJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Atualizar Jogo ---");
        Console.Write("Digite o código do jogo: ");
        long.TryParse(Console.ReadLine(), out long codigo);

        var jogo = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogo == null)
        {
            Console.WriteLine("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }

        Console.Write($"Local ({jogo.Local}): ");
        string local = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(local)) jogo.Local = local;

        Console.Write($"Tipo de campo ({jogo.TipoCampo}): ");
        string tipoCampo = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(tipoCampo)) jogo.TipoCampo = tipoCampo;

        Console.Write($"Jogadores por time ({jogo.JogadoresPorTime}): ");
        string jogadoresStr = Console.ReadLine();
        if (int.TryParse(jogadoresStr, out int jogadores)) jogo.JogadoresPorTime = jogadores;

        Console.Write($"Limite de times ({jogo.LimiteTimes}): ");
        string limiteTimesStr = Console.ReadLine();
        if (int.TryParse(limiteTimesStr, out int limiteTimes)) jogo.LimiteTimes = limiteTimes;

        Console.Write($"Limite de jogadores ({jogo.LimiteJogadores}): ");
        string limiteJogadoresStr = Console.ReadLine();
        if (int.TryParse(limiteJogadoresStr, out int limiteJogadores)) jogo.LimiteJogadores = limiteJogadores;

        SalvarNoArquivo();
        Console.WriteLine("Jogo atualizado!");
        Console.ReadKey();
    }

    public void ExcluirJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Excluir Jogo ---");
        Console.Write("Digite o código do jogo: ");
        long.TryParse(Console.ReadLine(), out long codigo);

        var jogo = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogo == null)
        {
            Console.WriteLine("Jogo não encontrado!");
        }
        else
        {
            listaDeJogos.Remove(jogo);
            SalvarNoArquivo();
            Console.WriteLine("Jogo removido!");
        }
        Console.ReadKey();
    }

    public void GerenciarInteressados()
    {
        Console.Clear();
        Console.WriteLine("--- Gerenciar Interessados no Jogo ---");
        Console.Write("Digite o código do jogo: ");
        long.TryParse(Console.ReadLine(), out long codigo);

        var jogo = listaDeJogos.Find(j => j.Codigo == codigo);
        if (jogo == null)
        {
            Console.WriteLine("Jogo não encontrado!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Digite o código do jogador interessado (ou 0 para sair):");
        while (true)
        {
            long.TryParse(Console.ReadLine(), out long jogadorId);
            if (jogadorId == 0) break;
            if (!jogo.Interessados.Contains((int)jogadorId))
                jogo.Interessados.Add((int)jogadorId);
            Console.WriteLine("Adicionado! Próximo código ou 0 para sair:");
        }

        SalvarNoArquivo();
        Console.WriteLine("Interessados atualizados!");
        Console.ReadKey();
    }

    public static string DiaDaSemanaEmPortugues(DayOfWeek dia)
    {
        switch (dia)
        {
            case DayOfWeek.Monday: return "Segunda-feira";
            case DayOfWeek.Tuesday: return "Terça-feira";
            case DayOfWeek.Wednesday: return "Quarta-feira";
            case DayOfWeek.Thursday: return "Quinta-feira";
            case DayOfWeek.Friday: return "Sexta-feira";
            case DayOfWeek.Saturday: return "Sábado";
            case DayOfWeek.Sunday: return "Domingo";
            default: return dia.ToString();
        }
    }

    private void SalvarNoArquivo()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(listaDeJogos, options);
        File.WriteAllText(caminhoArquivo, json);
    }

    private void CarregarDoArquivo()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            if (!string.IsNullOrWhiteSpace(json))
                listaDeJogos = JsonSerializer.Deserialize<List<Jogos>>(json);
        }
    }
}