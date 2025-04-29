using Players;
using Associacao;

// teste para ver se capta 
Jogador jogador3 = new Jogador("Philié", 18, Posicao.DefesaDireita);
Console.WriteLine(jogador3.ToString());
Database.SalvarJogador(jogador3);

