namespace generics;
using IPartidaGenerica;
using Players;
using Associacao;


public class GerericPlayer : Associados, ICaixaObjeto<string>
    {

        public List<string> ListaDeNomes {get; set;} = new List<string>();
        public void Guardar(string nome)
        { 
           
        }
    }

