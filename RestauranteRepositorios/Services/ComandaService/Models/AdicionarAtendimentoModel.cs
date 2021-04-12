using System;

namespace RestauranteRepositorios.Services
{
    public class AdicionarAtendimentoModel
    {
        public int QtdePessoasMesa { get; set; }
        public int MesaId { get; set; }

        public void Validar()
        {
            if (QtdePessoasMesa > 4 || QtdePessoasMesa < 1)
                throw new Exception("Quantidade de pessoas invalida! Máxido de 4 pessoas por mesa.");

            if (MesaId < 1 || MesaId > 16)
                throw new Exception("Favor escolher uma mesa valida");
        }
    }
}
