# language: pt
Funcionalidade: Gestão de Localização e Check-in de Vendas
  Como Gestor do Sistema
  Desejo controlar o acesso de administradores e vendedores
  Para garantir que os vendedores estão realmente nos clientes.

  Contexto: 
    Dado que o sistema possui uma margem de erro de 100 metros para check-in

  # --- 1. AUTENTICAÇÃO ---
  Cenário: Login de utilizador por CPF
    Dado que existe um utilizador cadastrado com o CPF "12345678900"
    Quando o utilizador tentar autenticar-se com este CPF
    Então o sistema deve verificar o perfil do utilizador
    E retornar um token JWT válido por 8 horas

  Cenário: Tentativa de login com utilizador inexistente
    Dado que o CPF "00000000000" não está na base de dados
    Quando tentar realizar o login
    Então a API deve retornar o erro "401 Unauthorized"

  # --- 2. GESTÃO ADMINISTRATIVA ---
  Cenário: Cadastro de novo Vendedor (Apenas ADMIN)
    Dado que o Administrador está autenticado com perfil "ADMIN"
    Quando solicitar o cadastro de "João Silva" com CPF "98765432100"
    Então o sistema deve validar a unicidade do CPF
    E criar o registo com perfil "VENDEDOR"

  Cenário: Cadastro de Cliente com Localização Alvo
    Dado que o Administrador informa os dados do cliente
    E as coordenadas Latitude "-23.550520" e Longitude "-46.633308"
    Quando salvar o registo
    Então o sistema deve armazenar estes pontos para validação

  # --- 3. OPERAÇÃO DE CHECK-IN ---
  Cenário: Vendedor realiza check-in dentro do raio permitido
    Dado que o Vendedor está autenticado e o cliente está na Lat -23.550520 / Long -46.633308
    Quando o Vendedor enviar sua posição como Lat -23.550600 / Long -46.633400
    Então o sistema deve calcular a distância usando a fórmula de Haversine
    E registrar o check-in com a distância calculada e o timestamp atual

  Cenário: Vendedor tenta check-in fora do local
    Dado que o Vendedor está a 500 metros de distância do cliente
    Quando tentar realizar o check-in
    Então a API deve retornar o erro "403 Forbidden"
    E a mensagem "Fora do raio permitido para este cliente"

  # --- 4. SEGURANÇA ---
  Cenário: Vendedor tenta acessar funções de Admin
    Dado que um utilizador com perfil "VENDEDOR" tenta acessar a rota "POST /clientes"
    Então a API deve retornar o erro "403 Forbidden"
