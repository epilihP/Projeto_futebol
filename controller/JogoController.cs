using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Jogoss;
using GerenciadorJogos;

public class JogoController
{
    private List<GerenciadorDeJogos> listaDeJogos = new List<GerenciadorDeJogos>();
    private readonly string caminhoArquivo = @"c:\Users\aliss\Documents\Faculdade\Programação Orientada a Objetos\Projeto Futebol\Projeto_futebol\Util\Database\jogos.json";
    public JogoController()
    {
        CarregarDoArquivo();
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

    public void AgendarJogo()
    {
        Console.Clear();
        Console.WriteLine("--- Agendar Novo Jogo ---");
        // Sempre próxima quinta-feira às 19h
        DateTime data = ProximaQuintaDisponivel();
        string codigo = data.ToString("ddMMyyyy");
        if (listaDeJogos.Exists(j => j.Data.Date == data.Date))
        {
            DateTime sugestao = ProximaQuintaDisponivel(data.AddDays(7));
            Console.WriteLine($"Já existe um jogo agendado para {data:dd/MM/yyyy}. Próxima quinta-feira disponível: {sugestao:dd/MM/yyyy}");
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            return;
        }
        int jogadoresPorTime = 5;
        int limiteTimes = 6;
        int limiteJogadores = jogadoresPorTime * limiteTimes;
        Console.WriteLine($"Data do jogo: {data:dd/MM/yyyy} às 19h");
        Console.WriteLine($"Código do jogo: {codigo}");
        Console.Write("Local do jogo (padrão: Quadra Poliesportiva): ");
        string local = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(local)) local = "Quadra Poliesportiva";
        Console.Write("Tipo de campo (padrão: Quadra Polietano (PU)): ");
        string tipoCampo = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(tipoCampo)) tipoCampo = "Quadra Polietano (PU)";
        GerenciadorDeJogos novoJogo = new GerenciadorDeJogos(
            data,
            local,
            tipoCampo,
            jogadoresPorTime,
            limiteTimes,
            limiteJogadores
        );
        // novoJogo.Codigo = DataTime.Now.Ticks;
        novoJogo.Codigo = long.Parse(codigo); // substituido acima, pois, gerava um cpodigo muito longo
        listaDeJogos.Add(novoJogo);
        SalvarNoArquivo();
        Console.WriteLine($"\nLimite de times: {limiteTimes}");
        Console.WriteLine($"Limite de jogadores: {limiteJogadores}");
        Console.WriteLine("\nJogo agendado com sucesso!");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }

    private DateTime ProximaQuintaDisponivel(DateTime? inicio = null)
    {
        DateTime data = inicio ?? DateTime.Now;
        // Ajusta para próxima quinta-feira
        int daysUntilThursday = ((int)DayOfWeek.Thursday - (int)data.DayOfWeek + 7) % 7;
        if (daysUntilThursday == 0) daysUntilThursday = 7;
        DateTime quinta = data.Date.AddDays(daysUntilThursday).AddHours(19); // 19h
        while (listaDeJogos.Exists(j => j.Data.Date == quinta.Date))
        {
            quinta = quinta.AddDays(7); // próxima quinta
        }
        return quinta;
    }

    public void ListarJogos()
    {
        Console.Clear();
        Console.WriteLine("--- Lista de Jogos Agendados ---");
        var jogosFuturos = listaDeJogos
            .Where(j => j.Data >= DateTime.Now)
            .OrderBy(j => j.Data)
            .ToList();
        if (jogosFuturos.Count == 0)
        {
            Console.WriteLine("Nenhum jogo agendado.");
        }
        else
        {
            foreach (var jogo in jogosFuturos)
            {
                string codigo = jogo.Data.ToString("ddMMyyyy");
                Console.WriteLine($"ID: {codigo} | Data: {jogo.Data:dd/MM/yyyy} às 19h | Local: {jogo.Local} | Campo: {jogo.TipoCampo}");
            }
        }
        Console.WriteLine("\nPressione qualquer tecla para voltar...");
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

        // verifica se existe o RA
        string caminhoAssociados = @"c:\Users\aliss\Documents\Faculdade\Programação Orientada a Objetos\Projeto Futebol\Projeto_futebol\Util\Database\associados.json";
        List<int> raValidos = new List<int>();
        if (File.Exists(caminhoAssociados))
        {
            string jsonAssociados = File.ReadAllText(caminhoAssociados);
            var associados = JsonSerializer.Deserialize<List<Associacao.Associados>>(jsonAssociados);
            raValidos = associados.Select(a => a.Id).ToList();
        }

        Console.WriteLine("Digite o código do jogador interessado (ou 0 para sair):");
        while (true)
        {
            long.TryParse(Console.ReadLine(), out long jogadorId);
            if (jogadorId == 0) break;
            if (!raValidos.Contains((int)jogadorId))
            {
                Console.WriteLine("RA não encontrado nos associados. Tente novamente.");
                continue;
            }
            if (!jogo.Interessados.Contains((int)jogadorId))
                jogo.Interessados.Add((int)jogadorId);
            Console.WriteLine("Adicionado! Próximo código ou 0 para sair:");
        }

        SalvarNoArquivo();
        Console.WriteLine("Interessados atualizados!");
        Console.ReadKey();
    }
    private void SalvarNoArquivo()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(listaDeJogos, options);
        Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
        File.WriteAllText(caminhoArquivo, json);
    }

    private void CarregarDoArquivo()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            if (!string.IsNullOrWhiteSpace(json))
                listaDeJogos = JsonSerializer.Deserialize<List<GerenciadorDeJogos>>(json);
            else
                listaDeJogos = new List<GerenciadorDeJogos>();
        }
        else
        {
            listaDeJogos = new List<GerenciadorDeJogos>();
        }
    }
}