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

  # --- 4. OPERAÇÃO DE CHECK-IN ---
  Cenário: Vendedor realiza check-out com sucesso
    Dado que o Vendedor "Carlos" realizou um Check-in na "Padaria Silva" às 10:00
    Quando o Vendedor solicitar o Check-out às 10:45
    E estiver dentro do raio de 100 metros do cliente
    Então o sistema deve registrar o horário de saída
    E calcular a duração da visita como "45 minutos".

  Cenário: Vendedor tenta check-out fora do local
    Dado que o Vendedor está a 500 metros de distância do cliente
    Quando tentar realizar o check-out
    Então a API deve retornar o erro "403 Forbidden"
    E a mensagem "Fora do raio permitido para este cliente"

  # ---5. OPERAÇÃO Um vendedor não pode estar em dois lugares ao mesmo tempo ou esquecer de fechar uma visita antes de abrir outra.
  Cenário: Bloqueio de múltiplos Check-ins simultâneos
    Dado que o Vendedor já possui um Check-in aberto no "Cliente A"
    Quando ele tentar realizar um novo Check-in no "Cliente B" sem ter feito o Check-out do anterior
    Então o sistema deve retornar um erro "409 Conflict"
    E informar que ele precisa encerrar a visita atual antes de iniciar uma nova.
  
  Cenário: Tentativa de Check-out sem Check-in prévio
    Dado que o Vendedor não iniciou nenhuma visita
    Quando ele enviar uma requisição de Check-out
    Então o sistema deve retornar um erro "400 Bad Request"
    E a mensagem "Não existe uma visita ativa para este vendedor".

  # --- 6. SEGURANÇA ---
  Cenário: Vendedor tenta acessar funções de Admin
    Dado que um utilizador com perfil "VENDEDOR" tenta acessar a rota "POST /clientes"
    Então a API deve retornar o erro "403 Forbidden"
