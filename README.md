# caixa-investimento-caixa-verso

Claro, Antonio 👌. Vou estruturar um **documento técnico explicativo** sobre o sistema que desenvolvemos, cobrindo tecnologias, segurança e funcionamento.  

---

# 📄 Documento Técnico – Sistema de Investimentos

## 1. Tecnologias Utilizadas
- **.NET 8 (ASP.NET Core)**  
  - Framework principal para desenvolvimento da API.  
  - Permite criação de serviços RESTful escaláveis e de alta performance.  

- **Entity Framework Core (EF Core)**  
  - ORM para mapeamento objeto-relacional.  
  - Utilizado para persistência de dados e aplicação de migrations.  

- **SQL Server 2022**  
  - Banco de dados relacional.  
  - Armazena entidades como `Cliente`, `Endereco`, `Investimentos`, `PerfilDeRisco`, `ProdutoInvestimento` e `PerfilProduto`.  

- **Docker & Docker Compose**  
  - Containerização da aplicação e do banco de dados.  
  - Facilita deploy e execução em ambientes isolados.  

- **Camadas do Projeto**  
  - **API**: Exposição dos endpoints REST.  
  - **Application**: Regras de negócio e casos de uso.  
  - **Infra**: Persistência e configuração do banco.  
  - **Domain**: Entidades e lógica de domínio (DDD).  

---

## 2. Segurança
- **Autenticação e Autorização**  
  - Implementação baseada em **JWT (JSON Web Tokens)** para autenticação de usuários.  
  - Tokens garantem acesso seguro aos endpoints da API.  

- **Validação de Dados**  
  - Todas as entidades possuem validações internas (ex.: `ProdutoInvestimento` exige risco entre 1 e 100, rentabilidade > 0, etc.).  
  - Evita inserção de dados inválidos no banco.  

- **Proteção de Dados Sensíveis**  
  - Campos como CPF, e-mail e celular são encapsulados em **Value Objects** (`Cpf`, `Email`, `Celular`).  
  - Isso garante consistência e validação automática.  

- **Banco de Dados**  
  - Acesso protegido por usuário e senha (`SA_PASSWORD`).  
  - Conexão configurada com `TrustServerCertificate=True` para evitar problemas de certificado em ambientes de desenvolvimento.  

- **Containerização**  
  - Cada serviço roda em container isolado.  
  - Reduz superfície de ataque e facilita controle de rede entre API e banco.  

---

## 3. Funcionamento do Sistema
1. **Cadastro de Cliente**  
   - Usuário é registrado com dados pessoais (nome, CPF, e-mail, celular, data de nascimento).  
   - Associado a um **Perfil de Risco** (Conservador, Moderado, Arrojado).  

2. **Cadastro de Endereço**  
   - Cada cliente possui endereço vinculado.  
   - Relacionamento 1:N (um cliente pode ter vários endereços).  

3. **Produtos de Investimento**  
   - Sistema possui produtos como CDB, LCI, Ações e Fundos Multimercado.  
   - Cada produto tem atributos: rentabilidade anual, risco, liquidez, tributação, garantia e descrição.  

4. **Investimentos**  
   - Cliente aplica em um produto de investimento.  
   - Registro inclui valor investido, prazo em meses, data da aplicação e informações de retirada.  

5. **Perfis de Risco e Associação**  
   - Produtos são vinculados a perfis de risco via entidade `PerfilProduto`.  
   - Exemplo: CDB → Conservador, LCI → Moderado, Ações → Arrojado.  

6. **Seeder Automático**  
   - Ao iniciar o sistema, o `DatabaseSeeder` garante:  
     - Criação do banco de dados.  
     - Aplicação de migrations.  
     - Inserção inicial de clientes, endereços, perfis de risco, produtos e investimentos.  

7. **Execução em Containers**  
   - API sobe em porta `8080/8081`.  
   - SQL Server roda em porta `1433`.  
   - Comunicação entre serviços é feita via rede interna do Docker Compose.  

---

## 4. Fluxo Resumido
- Usuário → API → EF Core → SQL Server.  
- API valida dados → aplica regras de negócio → persiste no banco.  
- Segurança garantida por autenticação JWT e validações de domínio.  
- Deploy simplificado via Docker Compose.  

---

👉 Esse documento pode servir como **manual técnico** para desenvolvedores e arquitetos que forem dar manutenção ou evoluir o sistema.  

Quer que eu formate esse documento também em **versão para stakeholders (não técnicos)**, explicando de forma mais simples como o sistema ajuda clientes a investir com segurança?