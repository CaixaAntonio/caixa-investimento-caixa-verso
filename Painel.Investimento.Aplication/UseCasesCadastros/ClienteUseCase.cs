using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Valueobjects;

namespace Painel.Investimento.Aplication.UseCasesCadastros
{
    public class ClienteUseCase
    {
        private readonly IClienteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteUseCase(IClienteRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Cliente> ExecuteAsync(
            int id,
            string nome,
            string sobrenome,
            Email email,
            Celular celular,
            Cpf cpf,
            DataDeNascimento dataDeNascimento,
            int perfilDeRiscoId)
        {
            var cliente = new Cliente(id, nome, sobrenome, email, celular, cpf, dataDeNascimento, perfilDeRiscoId);

            await _repository.AdicionarAsync(cliente);
            await _unitOfWork.CommitAsync();

            return cliente;
        }

        public async Task<Cliente?> ObterPorIdAsync(int id) => await _repository.ObterPorIdAsync(id);

        public async Task<IEnumerable<Cliente>> ListarTodosAsync() => await _repository.ObterTodosAsync();

        public async Task<Cliente?> AtualizarPerfilAsync(int id, int novoPerfilDeRiscoId)
        {
            var cliente = await _repository.ObterPorIdAsync(id);
            if (cliente == null) return null;

            cliente.AjustarPerfil(novoPerfilDeRiscoId);
            await _unitOfWork.CommitAsync();
            return cliente;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var cliente = await _repository.ObterPorIdAsync(id);
            if (cliente == null) return false;

            _repository.Remover(cliente);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Cliente?> AdicionarEnderecoAsync(int clienteId, Endereco endereco)
        {
            var cliente = await _repository.ObterPorIdAsync(clienteId);
            if (cliente == null) return null;

            cliente.AdicionarEndereco(endereco);
            await _unitOfWork.CommitAsync();
            return cliente;
        }

        public async Task<Cliente?> RemoverEnderecoAsync(int clienteId, int enderecoId)
        {
            var cliente = await _repository.ObterPorIdAsync(clienteId);
            if (cliente == null) return null;

            cliente.RemoverEndereco(enderecoId);
            await _unitOfWork.CommitAsync();
            return cliente;
        }

        public async Task<Cliente?> AtualizarEnderecoAsync(int clienteId, int enderecoId, string logradouro,
                                                           string numero, string complemento, string bairro,
                                                           string cidade, string estado, string cep)
        {
            var cliente = await _repository.ObterPorIdAsync(clienteId);
            if (cliente == null) return null;

            cliente.AtualizarEndereco(enderecoId, logradouro, numero, complemento, bairro, cidade, estado, cep);
            await _unitOfWork.CommitAsync();
            return cliente;
        }

    }

}
