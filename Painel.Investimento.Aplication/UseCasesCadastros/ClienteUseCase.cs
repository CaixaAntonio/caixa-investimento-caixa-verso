using Microsoft.Extensions.Logging;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Valueobjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.UseCasesCadastros
{
    public class ClienteUseCase
    {
        private readonly IClienteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClienteUseCase> _logger;

        public ClienteUseCase(IClienteRepository repository, IUnitOfWork unitOfWork, ILogger<ClienteUseCase> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
            try
            {
                _logger.LogInformation("Criando cliente {Nome} {Sobrenome}", nome, sobrenome);

                var cliente = new Cliente(id, nome, sobrenome, email, celular, cpf, dataDeNascimento, perfilDeRiscoId);

                await _repository.AdicionarAsync(cliente);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Cliente criado com sucesso: Id={Id}", cliente.Id);

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente {Nome} {Sobrenome}", nome, sobrenome);
                throw;
            }
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Consultando cliente Id={Id}", id);
                var cliente = await _repository.ObterPorIdAsync(id);

                if (cliente == null)
                    _logger.LogWarning("Cliente não encontrado: Id={Id}", id);

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar cliente Id={Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Cliente>> ListarTodosAsync()
        {
            try
            {
                _logger.LogInformation("Listando todos os clientes.");
                var clientes = await _repository.ObterTodosAsync();
                _logger.LogInformation("Foram encontrados {Quantidade} clientes.", clientes?.Count() ?? 0);
                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar clientes.");
                throw;
            }
        }

        public async Task<Cliente?> AtualizarPerfilAsync(int id, int novoPerfilDeRiscoId)
        {
            try
            {
                _logger.LogInformation("Atualizando perfil de risco do cliente Id={Id}", id);

                var cliente = await _repository.ObterPorIdAsync(id);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado para atualização de perfil: Id={Id}", id);
                    return null;
                }

                cliente.AjustarPerfil(novoPerfilDeRiscoId);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Perfil de risco atualizado com sucesso para Cliente Id={Id}", id);

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar perfil de risco do cliente Id={Id}", id);
                throw;
            }
        }

        public async Task<bool> RemoverAsync(int id)
        {
            try
            {
                _logger.LogInformation("Removendo cliente Id={Id}", id);

                var cliente = await _repository.ObterPorIdAsync(id);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado para remoção: Id={Id}", id);
                    return false;
                }

                _repository.Remover(cliente);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Cliente removido com sucesso: Id={Id}", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover cliente Id={Id}", id);
                throw;
            }
        }

        public async Task<Cliente?> AdicionarEnderecoAsync(int clienteId, Endereco endereco)
        {
            try
            {
                _logger.LogInformation("Adicionando endereço para ClienteId={ClienteId}", clienteId);

                var cliente = await _repository.ObterPorIdAsync(clienteId);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado para adicionar endereço: Id={ClienteId}", clienteId);
                    return null;
                }

                cliente.AdicionarEndereco(endereco);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Endereço adicionado com sucesso para ClienteId={ClienteId}", clienteId);

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar endereço para ClienteId={ClienteId}", clienteId);
                throw;
            }
        }

        public async Task<Cliente?> RemoverEnderecoAsync(int clienteId, int enderecoId)
        {
            try
            {
                _logger.LogInformation("Removendo endereço Id={EnderecoId} do ClienteId={ClienteId}", enderecoId, clienteId);

                var cliente = await _repository.ObterPorIdAsync(clienteId);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado para remover endereço: Id={ClienteId}", clienteId);
                    return null;
                }

                cliente.RemoverEndereco(enderecoId);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Endereço removido com sucesso para ClienteId={ClienteId}", clienteId);

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover endereço Id={EnderecoId} do ClienteId={ClienteId}", enderecoId, clienteId);
                throw;
            }
        }

        public async Task<Cliente?> AtualizarEnderecoAsync(int clienteId, int enderecoId, string logradouro,
                                                           string numero, string complemento, string bairro,
                                                           string cidade, string estado, string cep)
        {
            try
            {
                _logger.LogInformation("Atualizando endereço Id={EnderecoId} do ClienteId={ClienteId}", enderecoId, clienteId);

                var cliente = await _repository.ObterPorIdAsync(clienteId);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado para atualizar endereço: Id={ClienteId}", clienteId);
                    return null;
                }

                cliente.AtualizarEndereco(enderecoId, logradouro, numero, complemento, bairro, cidade, estado, cep);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Endereço atualizado com sucesso para ClienteId={ClienteId}", clienteId);

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar endereço Id={EnderecoId} do ClienteId={ClienteId}", enderecoId, clienteId);
                throw;
            }
        }
    }
}
